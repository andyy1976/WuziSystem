using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Camc.Web.Library;
using System.Configuration;

namespace ProductBarCodeManagementAndTrack
{
    public class PushService
    {
        private static string DBContractConn = ConfigurationManager.AppSettings["DBBarcodeManagement"];
        /// <summary>
        /// 启动
        /// </summary>
        public void LoadService()
        {
            DateTime PushTimeDate = DateTime.Now;
            RemovePush(PushTimeDate);
            string PushPauseId = PushServie(PushTimeDate);//获取有变化的id            
            string InsertPushService_Log_result = InsertPushService_Log(PushTimeDate, PushPauseId);

        }

        /// <summary>
        /// 1、遍历Pause暂停表（ 注意：必须写好状态变化后的推送状态变化）
        /// 根据不同的暂停状态 根据流程 再遍历BC_PushTime 暂停时间分类表（改为视图VI_PauseTime）  依次判断每个暂停是否超期
        /// </summary>
        private string PushServie(DateTime PushTimeDate)
        {
            DataTable PauseDt = GetPauseList("10,20,30");
            string PushPauseId = "";
            for (int i = 0; i < PauseDt.Rows.Count; i++)
            {

                string PauseId = PauseDt.Rows[i]["ID"].ToString();//暂停id
                string PauseLevel = PauseDt.Rows[i]["PauseLevel"].ToString(); //暂停等级               
                string PauseStatus = PauseDt.Rows[i]["PauseStatus"].ToString();//当前暂停的状态
                string ThisDept = "";
                DateTime OldTime = new DateTime();
                if (PauseStatus == "10")
                {
                    ThisDept = PauseDt.Rows[i]["SDeptCode"].ToString();//响应时间对应提交到车间 -提出部门
                    OldTime = Convert.ToDateTime(PauseDt.Rows[i]["DispatchSubmitTime"]);//提出时间+超期时间

                }
                else if (PauseStatus == "20")
                {
                    ThisDept = PauseDt.Rows[i]["SDeptCode"].ToString();//实际完成时间超期对应提交到
                    OldTime = Convert.ToDateTime(PauseDt.Rows[i]["PlanTime"]);//预计完成时间
                }
                else if (PauseStatus == "30")
                {
                    ThisDept = PauseDt.Rows[i]["RDeptCode"].ToString();
                    OldTime = Convert.ToDateTime(PauseDt.Rows[i]["CompleteTime"]);//答复时间超期 提交到提出部门
                }
                if (PauseStatus == "10" || PauseStatus == "20" || PauseStatus == "30")//10，响应时间
                {

                    DataTable PushTimeDt = GetPushTimeList(PauseStatus, PauseLevel);//遍历推送时间表
                    if (PushTimeDt.Rows.Count > 0)
                    {
                        for (int j = 0; j < PushTimeDt.Rows.Count; j++)
                        {

                            string PushReasonID = PushTimeDt.Rows[j]["PushReasonCode"].ToString();
                            string LevelCode = PushTimeDt.Rows[j]["LevelCode"].ToString();

                            if (!IsPushed(PauseId, LevelCode))//如果已存在不操作，不存在判断是否超期，超期新增
                            {
                                int PushTimeHour = Convert.ToInt32(PushTimeDt.Rows[j]["PushTime"]);

                                DateTime OverTime = GetWorkDate(OldTime, PushTimeHour);
                                if (OverTime < DateTime.Now)//超期
                                {
                                    string ret = InsertPushService(PauseId, ThisDept, LevelCode, PushReasonID, PauseStatus, PauseLevel, PushTimeDate);
                                    if (ret != "")
                                    {
                                        PushPauseId += "," + PauseId;
                                    }
                                }
                                //DispatchSubmitTime
                            }
                        }
                    }
                }
            }
            return PushPauseId;
        }
        /// <summary>
        /// 预计时间过长超期
        /// </summary>
        /// <param name="dtNow"></param>
        /// <param name="PlanTime"></param>
        /// <param name="PauseId"></param>
        public void AddPushPlanTime(DateTime dtNow, DateTime PlanTime, string PauseId)
        {
            DataTable PauseDt = GetPuaseByID(PauseId);
            string PauseLevel = PauseDt.Rows[0]["PauseLevel"].ToString();
            string ThisDept = PauseDt.Rows[0]["SolveDeptName"].ToString();
            DataTable PushTimeDt = GetPushTimeList("15", PauseLevel);//遍历推送时间表
            if (PushTimeDt.Rows.Count > 0)
            {
                for (int j = 0; j < PushTimeDt.Rows.Count; j++)
                {
                    string PushReasonID = PushTimeDt.Rows[j]["PushReasonCode"].ToString();
                    string LevelCode = PushTimeDt.Rows[j]["LevelCode"].ToString();
                    int PushTimeHour = Convert.ToInt32(PushTimeDt.Rows[j]["PushTime"]);
                    DateTime OverTime = GetWorkDate(dtNow, PushTimeHour);
                    if (OverTime < PlanTime)//超期
                    {
                        string DeptCode = GetDeptCodeByName(ThisDept);
                        string ret = InsertPushService(PauseId, DeptCode, LevelCode, PushReasonID, "15", PauseLevel, dtNow);                        
                    }
                    //DispatchSubmitTime
                }
            }
        }
        public string GetDeptCodeByName(string DeptName)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "select DeptCode from DeptEnum where DeptName='" + DeptName + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 预计时间过长超期推送删除
        /// </summary>
        /// <param name="barcode"></param>
        public bool DelPlanPushByBarCode(int PauseID)
        {

            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                if (PauseID.ToString() != "" && PauseID > 0)
                {
                    strSQL = "update BC_PushService set IsDel='true' ,  DelTime='{0}' where PauseID='{1}' and PauseStatus='15' and IsDel='false'";
                    strSQL = string.Format(strSQL, DateTime.Now, PauseID);
                    return DBI.Execute(strSQL);
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }


        }
        /// <summary>
        /// 获取Pause的一行
        /// </summary>
        /// <param name="pauseID"></param>
        /// <returns></returns>
        private DataTable GetPuaseByID(string pauseID)
        {
            DataTable dt = new DataTable();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            try
            {
                string SelectSQL = @"select a.*,c.*, f.*from dbo.BC_PauseTask a left join 
                                    BarCode b on a.BarCodeFull=b.BarcodeFull left join
                                    Task c on b.TaskID=c.ID left join 
                                    LastScanRecord d on a.BarCodeFull=d.Barcode left join 
                                    ScanLog e on d.LastScanLogID=e.ID left join 
                                    Station f on e.StationID=f.ID
                                    where a.ID=" + pauseID + "";
                return DBI.Execute(SelectSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取数据时出现异常！" + e.Message.ToString());
            }

        }
        /// <summary>
        /// 推送取消
        /// </summary>
        /// <param name="Time"></param>
        private void RemovePush(DateTime Time)
        {

            DataTable PushServiceDt = GetPushServiceAllList();
            for (int i = 0; i < PushServiceDt.Rows.Count; i++)
            {
                if (PushServiceDt.Rows[i]["PauseStatus"].ToString() != PushServiceDt.Rows[i]["PauseTask_PauseStatus"].ToString())
                {
                    bool ret = DelPushService(Convert.ToInt32(PushServiceDt.Rows[i]["ID"]), Time);

                }
            }
        }
        /// <summary>
        /// 获取超期对应时间推送表
        /// </summary>
        /// <returns></returns>
        private DataTable GetPushTimeList(string PauseStatus, string PauseLevel)
        {
            DataTable dt = new DataTable();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select * From VI_PauseTime  Where PauseStatus='" + PauseStatus + "' and PauseLevel='" + PauseLevel + "' Order By id asc";
                dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count < 1)
                {
                    strSQL = "Select * From VI_PauseTime  Where   PauseStatus='" + PauseStatus + "' and PauseLevel='0' Order By id asc";//0为不分等级的数据
                    dt = DBI.Execute(strSQL, true);
                }
            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
            return dt;
        }
        /// <summary>
        /// 获取暂停列表
        /// </summary>
        /// <returns></returns>
        private DataTable GetPushServiceAllList()
        {
            DataTable dt = new DataTable();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select a.*,b.PauseStatus as PauseTask_PauseStatus From BC_PushService as a join BC_PauseTask as b on a.PauseID=b.ID where a.IsDel='false'";
                dt = DBI.Execute(strSQL, true);

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
            return dt;
        }
        public bool DelPushService(int ID, DateTime DelTime)
        {

            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                if (ID != 0 && ID.ToString() != "")
                {
                    strSQL = "update BC_PushService set IsDel='true' ,  DelTime='{0}' where ID='{1}'";
                    strSQL = string.Format(strSQL, DelTime, ID);
                    return DBI.Execute(strSQL);
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }


        }
        /// <summary>
        /// 获取暂停的列表
        /// </summary>
        /// <returns></returns>
        private DataTable GetPauseList(string PauseStatus)
        {
            DataTable dt = new DataTable();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select a.*,b.DeptCode as SDeptCode,c.DeptCode as RDeptCode From BC_PauseTask as a join DeptEnum as b on a.SolveDeptName=b.DeptName join DeptEnum as c on a.RaiseDeptName=c.DeptName Where IsDel = 'false' and RecoverTime is null and PauseStatus in(" + PauseStatus + ")  Order By PauseStatus asc";
                dt = DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取暂停的列表出错！" + e.Message.ToString());
            }
            return dt;
        }
        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="PauseId"></param>
        /// <param name="LevelCode"></param>
        /// <returns></returns>
        public bool IsPushed(string PauseId, string LevelCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select * From BC_PushService  Where IsDel = 'false' and  PauseID='" + PauseId + "' and LevelID='" + LevelCode + "' ";
                return DBI.CheckExistData(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("判断是否存在数据出错！" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 推送表新增
        /// </summary>
        /// <param name="PauseID"></param>
        /// <param name="DeptID"></param>
        /// <param name="LevelID"></param>
        /// <param name="PushReasonID"></param>
        /// <param name="PauseStatus"></param>
        /// <param name="PuaseLevel"></param>
        /// <param name="PushTime"></param>
        /// <returns></returns>
        private string InsertPushService(string PauseID, string DeptID, string LevelID, string PushReasonID, string PauseStatus, string PuaseLevel, DateTime PushTime)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string insertStr;
            try
            {
                insertStr = "insert into BC_PushService (PauseID,DeptID,LevelID,PushReasonID,PauseStatus,IsDel,PuaseLevel,PushTime)   values ('{0}','{1}','{2}','{3}','{4}','false','{5}','{6}');Select SCOPE_IDENTITY()";
                insertStr = string.Format(insertStr, PauseID, DeptID, LevelID, PushReasonID, PauseStatus, PuaseLevel, PushTime);
                return DBI.GetSingleValue(insertStr);
            }
            catch (Exception e)
            {
                throw new Exception("判断是否存在数据出错！" + e.Message.ToString());
            }

        }
        private string InsertPushService_Log(DateTime PushSeriveTime, string PushPauseID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string insertStr;
            try
            {
                insertStr = "insert into BC_PushService_Log (PushSeriveTime,PushPauseID)   values ('{0}','{1}');Select SCOPE_IDENTITY()";
                insertStr = string.Format(insertStr, PushSeriveTime, PushPauseID);
                return DBI.GetSingleValue(insertStr);
            }
            catch (Exception e)
            {
                throw new Exception("判断是否存在数据出错！" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 计算工作时间的add
        /// </summary>
        /// <param name="Time">计算前的时间</param>
        /// <param name="addTime">增加小时</param>
        /// <returns></returns>
        public DateTime GetWorkDate(DateTime Time, int addTime)
        {
            DateTime retTime = new DateTime();
            int addDay = addTime / 8;
            int addHour = addTime % 8;

            DateTime addHourTime = Convert.ToDateTime(Time.ToString("yyyy-MM-dd 17:30:00"));
            //if (Time > addHourTime)
            //{
            //    Time = addHour;
            //}
            retTime = Time.AddDays(addDay);
            if (Time.AddHours(addHour) > addHourTime)
            {
                retTime = Time.AddHours(addHour + 15);
            }
            return retTime;

        }

        #region 测试
        public string Ceshi()
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"insert into Ceshi (nowdate,name) values ('{0}','{1}');Select SCOPE_IDENTITY()";
                strSQL = string.Format(strSQL, DateTime.Now, "秦杰琼");
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
        }
        #endregion

    }
}