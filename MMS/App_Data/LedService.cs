using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Camc.Web.Library;
using System.Configuration;

namespace mms
{
    public class LedService
    {
        private static string DBContractConn = ConfigurationManager.AppSettings["DBBarcodeManagement"];
        //    method: 显示方式
        //   1.   立即显示
        //   2.   左滚显示
        //   3.   连续左滚
        //   4.   上滚显示
        //   5.   中间向上下展开
        //   6.   中间向两边展开
        //   7.   中间向四周展开
        //   8.   向左移入
        //   9.   向右移入
        //  10.   从左向右展开
        //  11.   从右向左展开
        //  12.   从右上角移入
        //  13.   从右下角移入
        //  14.   从左上角移入
        //  15.   从左下角移入
        //  16.   从上向下移入
        //  17.   从下向上移入
        //  18.   闪  烁
        //19.   从上向下滚动
        //20.   从左向右滚动
        //21.   连续下滚
        //speed:   显示速度（1-8）越大越快
        //transparent: 是否透明   0=不透明 1=透明


        //string eCommPort = "1";  //串口号
        //int cmbDevType = 1;      //通讯方式  0串口通讯  1网络通讯
        //int cmbBaudRate = 0;      //波特率    0:57600  1:38400  2:19200  3:9600
        //uint eLocalPort = 8881;   //本地端口  8881
        //private const int WM_LED_NOTIFY = 1;    //默认一个参数不知道是做什么的 设为1，则handle 就没用了
        //string Text = string.Empty;
        //string eAddress = "0";                 //控制卡地址
        //string eRemoteHost = "10.20.254.208";   //led屏地址

        LedCommon LEDSender = new LedCommon();
        /// <summary>
        /// 初始化方法
        /// </summary>
        public void LedLoad()
        {
            DataTable LedDT = GetLedManageList();

            //遍历LED表 获取到这个LED所在部门
            for (int i = 0; i < LedDT.Rows.Count; i++)
            {
                string Text = string.Empty;

                int dev = -1;  //标记


                string eAddress = "0";
                string eRemoteHost = LedDT.Rows[i]["LedIP"].ToString();
                string DeptCode = LedDT.Rows[i]["DeptCode"].ToString();//
                //string LevelCode = LedDT.Rows[i]["LevelCode"].ToString();
                string LedShowTitel = LedDT.Rows[i]["LedShowTitel"].ToString();
                try
                {
                    dev = OpenScreen(dev);
                    if (dev >= 0)
                    {
                        sendText(LedShowTitel, DeptCode, dev, eAddress, eRemoteHost);
                    }
                    CloseLed(dev);
                }
                catch
                {

                    continue;
                }
            }

        }
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        private int OpenScreen(int dev)
        {
            string eCommPort = "1";  //串口号
            int cmbDevType = 1;      //通讯方式  0串口通讯  1网络通讯
            int cmbBaudRate = 0;      //波特率    0:57600  1:38400  2:19200  3:9600
            uint eLocalPort = 8881;   //本地端口  8881
            int WM_LED_NOTIFY = 1;    //默认一个参数不知道是做什么的 设为1，则handle 就没用了

            LedCommon.DEVICEPARAM param = new LedCommon.DEVICEPARAM();
            GetDeviceParam(ref param, cmbDevType, eCommPort, cmbBaudRate, eLocalPort);
            //dev = LEDSender.LED_Open(ref param, LedCommon.NOTIFY_EVENT, (int)Handle, WM_LED_NOTIFY);原
            dev = LEDSender.LED_Open(ref param, LedCommon.NOTIFY_EVENT, 1, WM_LED_NOTIFY);
            return dev;
        }
        /// <summary>
        /// 获取打开串
        /// </summary>
        /// <param name="param"></param>
        /// <param name="cmbDevType"></param>
        /// <param name="eCommPort"></param>
        /// <param name="cmbBaudRate"></param>
        /// <param name="eLocalPort"></param>
        private void GetDeviceParam(ref LedCommon.DEVICEPARAM param, int cmbDevType, string eCommPort, int cmbBaudRate, uint eLocalPort)
        {
            switch (cmbDevType)
            {
                case 0:
                    param.devType = (uint)LedCommon.eDevType.DEV_COM;
                    break;
                case 1:
                    param.devType = (uint)LedCommon.eDevType.DEV_UDP;
                    break;
            }
            param.ComPort = (uint)Convert.ToInt16(eCommPort);
            switch (cmbBaudRate)
            {
                case 0: param.Speed = (uint)LedCommon.eBaudRate.SBR_57600; break;
                case 1: param.Speed = (uint)LedCommon.eBaudRate.SBR_38400; break;
                case 2: param.Speed = (uint)LedCommon.eBaudRate.SBR_19200; break;
                case 3: param.Speed = (uint)LedCommon.eBaudRate.SBR_9600; break;
                default: param.Speed = (uint)LedCommon.eBaudRate.SBR_57600; break;
            }
            param.locPort = eLocalPort;
            param.rmtPort = 6666;
        }
        /// <summary>
        /// 获取Led列表 遍历
        /// </summary>
        /// <returns></returns>
        public DataTable GetLedManageList()
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select * from BC_LedManage order by LedIP ASC";
                return DBI.Execute(strSQL, true);

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 通过DeptCode获取
        /// </summary>
        /// <returns></returns>
        public DataTable GetLedManageByDeptCode(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select * from BC_LedManage where DeptCode='"+DeptCode+"'";
                return DBI.Execute(strSQL, true);

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="dev"></param>
        private void CloseLed(int dev)
        {
            LEDSender.LED_Close(dev);
        }
        /// <summary>
        /// 获取不同状态下的暂停和超期数量
        /// </summary>
        /// <param name="Dept"></param>
        /// <returns></returns>
        public DataTable GetPauseAndPushCount(string Dept)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select a.ID ,
                            isnull((select COUNT(*) from VI_PushServiceDistinct as b where a.ID=b.PauseStatus and b.DeptID='{0}' group by PauseStatus),0) as PushCount,
                            isnull((select COUNT(*) from BC_PauseTask as c left join DeptEnum as d on c.SolveDeptName=d.DeptName  where c.PauseStatus=a.ID and d.DeptCode='{0}' and IsParent='false'),0) as PauseCount
                            from BC_PauseStatus_C as a
                            where a.ID in (10,20,30) order by a.ID asc";
                strSQL = string.Format(strSQL, Dept);
                return DBI.Execute(strSQL, true);

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }

        }
        /// <summary>
        /// 获取待确认的超期数量，因为带确认指向的是车间的超期 所以要单独查询
        /// </summary>
        /// <param name="Dept"></param>
        /// <returns></returns>
        public int GetPush30Count(string Dept)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select COUNT(*) from VI_PushServiceDistinct as a
                            left join BC_PauseTask as b on a.PauseID=b.ID
                            where a.PauseStatus=30 and b.SolveDeptName=(select DeptName from DeptEnum where DeptCode='{0}')";
                strSQL = string.Format(strSQL, Dept);
                return Convert.ToInt32(DBI.GetSingleValue(strSQL));

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取当前处室 不同状态 不同车间的数量
        /// </summary>
        /// <param name="Dept"></param>
        /// <returns></returns>
        public DataTable GetRaiseDeptCount(string Dept)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select DeptCode,
                        (select COUNT(*) from BC_PauseTask as b where IsDel='false' and IsParent='false' and SolveDeptName=(select DeptName from DeptEnum where DeptCode='{0}') and RaiseDeptName=a.DeptName and PauseStatus='10') as count10,
                        (select COUNT(*) from BC_PauseTask as b where IsDel='false' and IsParent='false' and SolveDeptName=(select DeptName from DeptEnum where DeptCode='{0}') and RaiseDeptName=a.DeptName and PauseStatus='20') as count20,
                        (select COUNT(*) from BC_PauseTask as b where IsDel='false' and IsParent='false' and SolveDeptName=(select DeptName from DeptEnum where DeptCode='{0}') and RaiseDeptName=a.DeptName and PauseStatus='30') as count30
                        from DeptEnum  as a
                        where DeptCode in ('51','53','55','56','58') order by DeptCode asc";
                strSQL = string.Format(strSQL, Dept);
                return DBI.Execute(strSQL, true);

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取待处理的详细
        /// </summary>
        /// <param name="Dept"></param>
        /// <returns></returns>
        public DataTable GetPuaseListIn10(string Dept)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select * from VI_PauseList where IsDel='false' and IsParent='false' and SolveDeptName=(select DeptName from DeptEnum where DeptCode='{0}')and PauseStatus='10'
                        order by PauseLevel desc";
                strSQL = string.Format(strSQL, Dept);
                return Common.AddTableRowsID( DBI.Execute(strSQL, true));

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }

        }
        /// <summary>
        /// 发送消息到LED
        /// </summary>
        /// <param name="LedShowTitel"></param>
        /// <param name="DeptCode"></param>
        /// <param name="LevelCode"></param>
        /// <param name="dev"></param>
        /// <param name="eAddress"></param>
        /// <param name="eRemoteHost"></param>
        private void sendText(string LedShowTitel, string DeptCode, int dev, string eAddress, string eRemoteHost)
        {
            DataTable PauseAndPushDT = GetPauseAndPushCount(DeptCode);

            string tou = LedShowTitel;
            int daiChuLi = 0;
            int daiChuLiOut = 0;
            int chuLiZhong = 0;
            int chuLiZhongOut = 0;
            int daiQueRen = 0;
            if (PauseAndPushDT.Rows.Count > 0)
            {
                daiChuLi = Convert.ToInt32(PauseAndPushDT.Rows[0]["PauseCount"]);
                daiChuLiOut = Convert.ToInt32(PauseAndPushDT.Rows[0]["PushCount"]);
                chuLiZhong = Convert.ToInt32(PauseAndPushDT.Rows[1]["PauseCount"]);
                chuLiZhongOut = Convert.ToInt32(PauseAndPushDT.Rows[1]["PushCount"]);
                daiQueRen = Convert.ToInt32(PauseAndPushDT.Rows[2]["PauseCount"]);
            }

            int daiQueRenOut = GetPush30Count(DeptCode); //Convert.ToInt32(PauseAndPushDT.Rows[2]["PushCount"]);
            //string newRecord = @"设备坏了，51车间壁板铣设备坏了，51车间壁板铣设备坏了，51车间壁板铣设备坏了，51车间壁板铣";
            string newRecord = "";

            //string[] cheJian; //= { "1/1/1", "2/2/2", "3/3/3", "4/4/4", "5/5/5" };
            DataTable PuaseListIn10DT = GetPuaseListIn10(DeptCode);
            for (int i = 0; i < PuaseListIn10DT.Rows.Count; i++)
            {
                string remark = PuaseListIn10DT.Rows[i]["Remark"].ToString();
                if (PuaseListIn10DT.Rows[i]["Remark"].ToString() == "")
                {
                    remark = "无备注!";
                }
                newRecord += "(" + (i + 1) + ") " + PuaseListIn10DT.Rows[i]["RaiseDeptName"].ToString() + " 【" + PuaseListIn10DT.Rows[i]["PauseLevelName"].ToString() + "】" + "\n    " + PuaseListIn10DT.Rows[i]["StationName"].ToString() + "\n    " + Convert.ToDateTime(PuaseListIn10DT.Rows[i]["DispatchSubmitTime"]).ToString("yyyy-MM-dd HH:mm") + "\n\n";
            }
            if (newRecord == "")
            {
                newRecord = "暂无未处理问题！";
            }


            LedCommon.RECT r;
            //dev = 0;

            LEDSender.MakeRoot(LedCommon.eRootType.ROOT_PLAY, LedCommon.eScreenType.SCREEN_COLOR);
            LEDSender.AddLeaf(5000);
            r.left = 0;
            r.right = 256;
            r.top = 0;
            r.bottom = 16;
            LEDSender.AddTextEx(tou, ref r, 1, 1, 1, "宋体", 12, 0x0000FF00, 1, 1);

            int chang = 64;
            int gao = 16;
            int x = 3;
            int y = 5;

            r.left = x;
            r.right = chang;
            r.top = y + gao;
            r.bottom = y + 2 * gao;
            LEDSender.AddText("待处理:" + daiChuLi, ref r, 1, 1, 1, "宋体", 9, 255, 0);

            r.left = chang;
            r.right = 2 * chang;
            r.top = y + gao;
            r.bottom = y + 2 * gao;
            LEDSender.AddText("超时:" + daiChuLiOut, ref r, 1, 1, 1, "宋体", 9, 255, 0);

            r.left = x;
            r.right = chang;
            r.top = y + 2 * gao;
            r.bottom = y + 3 * gao;
            LEDSender.AddText("处理中:" + chuLiZhong, ref r, 1, 1, 1, "宋体", 9, 0x0000FFFF, 0);

            r.left = chang;
            r.right = 2 * chang;
            r.top = y + 2 * gao;
            r.bottom = y + 3 * gao;
            LEDSender.AddText("超时:" + chuLiZhongOut, ref r, 1, 1, 1, "宋体", 9, 0x0000FFFF, 0);

            r.left = x;
            r.right = chang;
            r.top = y + 3 * gao;
            r.bottom = y + 4 * gao;
            LEDSender.AddText("待确认:" + daiQueRen, ref r, 1, 1, 1, "宋体", 9, 0x0000FF00, 0);

            r.left = chang;
            r.right = 2 * chang;
            r.top = y + 3 * gao;
            r.bottom = y + 4 * gao;
            LEDSender.AddText("超时:" + daiQueRenOut, ref r, 1, 1, 1, "宋体", 9, 0x0000FF00, 0);

            r.left = 2 * chang - 10;
            r.right = 4 * chang;
            r.top = y + gao;
            r.bottom = y + 4 * gao;
            LEDSender.AddTextEx(newRecord, ref r, 4, 1, 1, "宋体", 9, 255, 0, 1);

            r.left = 120;
            r.right = 4 * chang;
            r.top = y + 7 * gao;
            r.bottom = y + 8 * gao;
            LEDSender.AddDateTime(ref r, 1, "宋体", 9, 0x0000FF80, LedCommon.eTimeFormat.DF_YMD, 0);

            r.left = 205;
            r.right = 4 * chang;
            r.top = y + 7 * gao;
            r.bottom = y + 8 * gao;
            LEDSender.AddDateTime(ref r, 1, "宋体", 9, 0x0000FF80, LedCommon.eTimeFormat.DF_HNS, 0);


            //绘制64*64大小的图片
            LEDSender.UserCanvas_Init(256, 48);
            //绘制直线
            //  LEDSender.UserCanvas_Draw_Line(0, 0, 16, 16, 1, 255);
            //绘制矩形，红色框，黑色填充
            LEDSender.UserCanvas_Draw_Rectangle(x, 0, 250, 48, 1, 0x0000FF80, 0);

            for (int i = 1; i < 5; i++)
            {
                LEDSender.UserCanvas_Draw_Line(x + 49 * i, 0, x + 49 * i, 48, 1, 0x0000FF80);
            }

            LEDSender.UserCanvas_Draw_Line(x, 22, 250, 22, 1, 0x0000FF80);
            //绘制椭圆形，红色框，黑色填充
            //  LEDSender.UserCanvas_Draw_Rectangle(48, 48, 64, 64, 1, 255, 0);
            //绘制文字

            LEDSender.UserCanvas_Draw_Text(x + 1, 1, 47, 21, "51", "宋体", 9, 0x0000FF80, 0, 1);
            LEDSender.UserCanvas_Draw_Text(x + 1 + 49 * 1, 1, 47, 21, "53", "宋体", 9, 0x0000FF80, 0, 1);
            LEDSender.UserCanvas_Draw_Text(x + 1 + 49 * 2, 1, 47, 21, "55", "宋体", 9, 0x0000FF80, 0, 1);
            LEDSender.UserCanvas_Draw_Text(x + 1 + 49 * 3, 1, 47, 21, "56", "宋体", 9, 0x0000FF80, 0, 1);
            LEDSender.UserCanvas_Draw_Text(x + 1 + 49 * 4, 1, 47, 21, "58", "宋体", 9, 0x0000FF80, 0, 1);

            DataTable RaiseDeptDT = GetRaiseDeptCount(DeptCode);
            for (int i = 0; i < RaiseDeptDT.Rows.Count; i++)
            {
                string List = RaiseDeptDT.Rows[i]["count10"].ToString() + "/" + RaiseDeptDT.Rows[i]["count20"].ToString() + "/" + RaiseDeptDT.Rows[i]["count30"].ToString();
                LEDSender.UserCanvas_Draw_Text(x + 1 + 49 * i, 25, 47, 22, List, "宋体", 9, 255, 0, 1);
            }
            //for (int i = 1; i < 6; i++)
            //{

            //}

            r.left = 0;
            r.right = 4 * chang - x;
            r.top = y + 4 * gao;
            r.bottom = y + 7 * gao;
            LEDSender.AddUserCanvas(ref r, 1, 1, 1);

            LEDSender.UserCanvas_Init(256, 3);
            LEDSender.UserCanvas_Draw_Line(0, 0, 256, 0, 1, 0x0000FF80);
            r.left = 0;
            r.right = 256;
            r.top = 16;
            r.bottom = 19;
            LEDSender.AddUserCanvas(ref r, 1, 1, 1);

            //黄色 0x0000FFFF  绿色0x0000FF80 红色 255
            LEDSender.LED_SendToScreen(dev, (byte)Convert.ToInt16(eAddress), eRemoteHost, 6666);

        }
    }
}