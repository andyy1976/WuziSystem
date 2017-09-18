using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Camc.Web.Library;
using System.Configuration;

namespace mms
{
    public class PushSerice
    {
        private static string DBContractConn = ConfigurationManager.AppSettings["DBBarcodeManagement"];
        /// <summary>
        /// 1、遍历Pause暂停表（ 注意：必须写好状态变化后的推送状态变化）
        /// 根据不同的暂停状态 根据流程 再遍历BC_PushTime 暂停时间分类表（改为视图VI_PauseTime）  依次判断每个暂停是否超期
        /// </summary>
        public void PageLoad()
        {

            DataTable PauseDt = GetPauseList();
            for (int i = 0; i < PauseDt.Rows.Count; i++)
            {
                if (PauseDt.Rows[i]["PauseStatus"].ToString() == "10")//响应时间
                {
                    string PauseLevel = PauseDt.Rows[i]["PauseLevel"].ToString();
                    DataTable PushTimeDt = GetPushTimeList("XYOT", PauseLevel);//遍历推送时间表
                    if (PushTimeDt.Rows.Count > 0)
                    {
                        for (int j = 0; j < PushTimeDt.Rows.Count; j++)
                        {
                            string LevelCode = PushTimeDt.Rows[j]["LevelCode"].ToString();
                            string PauseId = PauseDt.Rows[i]["ID"].ToString();

                            if (!IsPushed(PauseId, LevelCode))//如果已存在不操作，不存在判断是否超期，超期新增
                            {
                                string PuaseTime = PauseDt.Rows[j]["PuaseTime"].ToString();
                                DateTime DispatchSubmitTime = Convert.ToDateTime(PauseDt.Rows[i]["DispatchSubmitTime"]);
                                //DispatchSubmitTime
                            }
                        }
                    }
                }


                else if (PauseDt.Rows[i]["PauseStatus"].ToString() == "20")
                {

                }
                else if (PauseDt.Rows[i]["PauseStatus"] == "30")
                {

                }
            }
        }
        /// <summary>
        /// 获取超期对应时间推送表
        /// </summary>
        /// <returns></returns>
        private DataTable GetPushTimeList(string PushReasonCode, string PauseLevel)
        {
            DataTable dt = new DataTable();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select * From VI_PauseTime  Where IsDel = 'false' and PushReasonCode='" + PushReasonCode + "' and PauseLevel='" + PauseLevel + "' Order By id asc";
                dt = DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
            return dt;
        }
        /// <summary>
        /// 获取暂停的列表
        /// </summary>
        /// <returns></returns>
        private DataTable GetPauseList()
        {
            DataTable dt = new DataTable();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select * From BC_PauseTask  Where IsDel = 'false' and RecoverTime is null  Order By PauseStatus asc";
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
        private bool IsPushed(string PauseId, string LevelCode)
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

        private string InsertPushService(string PauseID, string DeptID, string LevelID, string PushReasonID, string PauseStatus)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string insertStr;
            try
            {
                insertStr = "insert into BC_PushService (PauseID,DeptID,LevelID,PushReasonID,PauseStatus,IsDel)   values ('" + PauseID + "','" + DeptID + "','" + LevelID + "','" + PushReasonID + "','" + PauseStatus + "','false');Select SCOPE_IDENTITY()";
                return DBI.GetSingleValue(insertStr);
            }
            catch (Exception e)
            {
                throw new Exception("判断是否存在数据出错！" + e.Message.ToString());
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Time">计算前的时间</param>
        /// <param name="addTime">增加的时间</param>
        /// <returns></returns>
        private DateTime GetWorkDate(DateTime Time,int addTime)
        {
            int addDay = addTime / 8;
            return Time;
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