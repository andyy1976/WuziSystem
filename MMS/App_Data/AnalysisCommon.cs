using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Camc.Web.Library;
using System.Configuration;
using System.Data;

namespace mms
{
    public class AnalysisCommon
    {
        private static string DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();

        /// <summary>
        /// 统计任务执行比例
        /// </summary>
        /// <param name="DeptCode">指定车间，若为空则为全部</param>
        /// <returns></returns>
        public static decimal CountWorkedTaskProportion(string DeptCode, string ProductModel)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            decimal workedProportion = 0;
            try
            {
                strSQL = "SELECT COUNT(ID) FROM [ProductBarCodeManagementAndTrack].[dbo].[Task] Where [Enable]='1' And [DeptCode] like '%" + DeptCode + "%' And [ProductModel] = '" + ProductModel + "'";
                decimal allTaskAmount = Convert.ToInt32(DBI.GetSingleValue(strSQL));
                strSQL = "SELECT COUNT(a.ID) FROM [ProductBarCodeManagementAndTrack].[dbo].[BarCode] a left join dbo.Task b on a.TaskID = b.ID Where a.[Enable]='1' And b.[DeptCode] like '%" + DeptCode + "%' And b.[ProductModel] = '" + ProductModel + "'";
                decimal workedTaskAmount = Convert.ToInt32(DBI.GetSingleValue(strSQL));
                if (allTaskAmount != 0)
                {
                    workedProportion = workedTaskAmount / allTaskAmount;
                }
                return System.Decimal.Round(workedProportion, 2);
            }
            catch (Exception e)
            {
                throw new Exception("统计指定车间已执行任务比例" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计任务完成比例
        /// </summary>
        /// <param name="DeptCode">指定车间，若为空则为全部</param>
        /// <returns></returns>
        public static decimal CountFinishedTaskProportion(string DeptCode, string ProductModel)
        {
            decimal finishedProportion = 0;
            try
            {
                decimal allTask = GetAllTaskCount(DeptCode, ProductModel);
                decimal finishedTask = GetFinishedTaskCount(DeptCode, ProductModel);
                if (finishedTask > 0)
                {
                    finishedProportion = finishedTask / allTask;
                }
                return System.Decimal.Round(finishedProportion, 2);

            }
            catch (Exception e)
            {
                throw new Exception("统计任务完成比例时出现异常" + e.Message.ToString());
            }

        }

        /// <summary>
        /// 获取指定车间最新的扫码记录
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public static DataTable GetLastScanRecord(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT Top 1 BarCode,ScanTime,StationName,
                              case when WorkState = '0' then '开工' 
                              when WorkState = '1' then '完工'
                              when WorkState = '2' then '暂停' 
                              end as StateName
                              ,b.DeptCode,DeptName
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] a join dbo.Station b
                              on a.StationID = b.ID Join DeptEnum c on b.DeptCode = c.DeptCode
                              Where b.DeptCode like '%" + DeptCode + "%' Order By [ScanTime] DESC";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取指定车间最新的扫码记录时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计所有任务数
        /// </summary>
        /// <param name="DeptCode">指定车间代号，为空时为全公司所有任务</param>
        /// <returns></returns>
        public static int GetAllTaskCount(string DeptCode, string ProductModel)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT COUNT(ID) as TaskCount
                                      FROM [ProductBarCodeManagementAndTrack].[dbo].[Task]
                                      Where DeptCode like '%" + DeptCode + "%' And [Enable] = '1' And [ProductModel] = '" + ProductModel + "'";
                string str = DBI.GetSingleValue(strSQL);
                return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                throw new Exception("统计所有任务数时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计已处理任务数
        /// </summary>
        /// <param name="DeptCode">指定的部门代号，若为空则统计全部</param>
        /// <returns></returns>
        public static int GetDisposedTaskCount(string DeptCode, string ProductModel)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT COUNT(b.ID) as BarcodeCount
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[BarCode] a left Join dbo.Task b on a.TaskID = b.ID
                              Where b.[Enable] = '1' And a.[Enable] = '1' And b.[DeptCode] like '%" + DeptCode + "%' And b.[ProductModel] = '" + ProductModel + "'";
                string str = DBI.GetSingleValue(strSQL);
                return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                throw new Exception("统计已处理任务数时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计已完成任务数量
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public static int GetFinishedTaskCount(string DeptCode, string ProductModel)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                //                strSQL = @"SELECT COUNT(a.ID) as FinishedCount
                //                                  FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] a Join dbo.Station b on a.StationID = b.ID
                //                                  Where a.CertificateID <> '' And b.DeptCode like '%" + DeptCode + "%'";
                strSQL = @"select COUNT(*) as FinishAmount From (SELECT b.Barcode,MAX(b.ScanTime) as ScanTime
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[BarCode] a
                              Join dbo.ScanLog b On a.BarcodeFull = b.Barcode
                              Join dbo.Task c On a.TaskID = c.ID
                              where b.CertificateID <> '' And b.Enable = 1 And c.Enable = 1 And c.DeptCode like '%" + DeptCode + "%' And c.[ProductModel] = '" + ProductModel + "'" +
                              " group by b.Barcode) as H";
                string str = DBI.GetSingleValue(strSQL);
                return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                throw new Exception("统计已完成任务数量时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计所有条码数量
        /// </summary>
        /// <param name="DeptCode">指定车间代号，若为空则为全部</param>
        /// <returns></returns>
        public static int GetAllScanCount(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT COUNT(a.ID) as BarcodeCount
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] a Join dbo.Station b
                              On a.StationID = b.ID Where b.DeptCode like '%" + DeptCode + "%'";
                string str = DBI.GetSingleValue(strSQL);
                return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                throw new Exception("统计所有条码数量时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计新条码数量
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public static int GetNewBarcodeCount(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT Count(a.ID) as NewCreateBarcodeCount
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[CreateBarcodeOperateLog] a
                              join dbo.BarCode b on a.BarcodeID = b.ID join dbo.Task c On b.TaskID = c.ID
                              Where [Type] = 'create' And c.DeptCode like '%" + DeptCode + "%' And DATEDIFF(DD,CreateDate,GetDate()) = 0";
                string str = DBI.GetSingleValue(strSQL);
                return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                throw new Exception("统计新条码数量时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计所有新扫描数量
        /// </summary>
        /// <param name="DeptCode">指定部门，若为空则为所有</param>
        /// <returns></returns>
        public static int GetNewScanCount(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT COUNT(a.ID) as NewScanCount
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] a Join dbo.Station b
                              On a.StationID = b.ID
                              Where DATEDIFF(DD,ScanTime,GetDate()) = 0 And b.DeptCode like '%" + DeptCode + "%'";
                string str = DBI.GetSingleValue(strSQL);
                return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                throw new Exception("统计所有新扫描数量时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取暂停的条码
        /// </summary>
        /// <param name="DeptCode">指定部门，若为空则为全部</param>
        /// <returns></returns>
        public static DataTable GetPausedBarcodeList(string DeptCode, string PauseTypeID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT a.Barcode, b.ScanTime, c.StationName, d.AllowSplitAmount, f.PauseType, e.TaskNum, e.DrawingNum, e.ProductName, g.DeptName, b.Remark
                            ,c.DeptCode, b.PauseTypeID, f.ImagesUrl
                            FROM [LastScanRecord] a
                            Left Join dbo.ScanLog b On a.LastScanLogID = b.ID
                            Left Join dbo.Station c On b.StationID = c.ID
                            Left Join dbo.BarCode d On a.Barcode = d.BarcodeFull
                            Left Join dbo.Task e On d.TaskID = e.ID
                            Left Join dbo.Sys_PasueReason f On b.PauseTypeID = f.ID
                            Left Join dbo.DeptEnum g On c.DeptCode = g.DeptCode
                            Where b.WorkState = '2' And d.Enable = 1 And e.Enable = 1" +
                            " And c.DeptCode like '%" + DeptCode + "%' And b.PauseTypeID like '%" + PauseTypeID + "%'" +
                            " Order By [ScanTime] DESC";
                return Common.AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("统计所有新扫描数量时出现异常" + e.Message.ToString());
            }
        }
        public static DataTable GetPausedPauseTask(string PauseType, string solveDeptName, string raiseDeptName, string type = "")
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                if (type == "Complete")
                {
                    strSQL = @"select a.* ,a.PauseLevel as DrawLevel,c.DrawingNum,c.TaskNum,b.TaskID,g.StationName ,c.TaskNum + '-' + c.DrawingNum as effectTack "
                        + ", case when a.PauseLevel = '1' then '一般' when a.PauseLevel = '2' then '紧急' when a.PauseLevel = '3' then '特急' else Convert(varchar(6),a.PauseLevel) end as PauseLevelName"
                        + ", case when a.PauseStatus='5' then '审核中' when a.PauseStatus = '10' then '待接收' when a.PauseStatus = '20' then '暂停处理中' when a.PauseStatus = '30' then '已处理，待确认' when a.PauseStatus='40' then '已完成' else convert(varchar(20), a.PauseStatus) end as PauseStatussolve "
                        + " from BC_PauseTask as a join BarCode  as b on a.BarCodeFull=b.BarcodeFull join Task as c on c.ID=b.TaskID"
                        + " join LastScanRecord as d on a.BarCodeFull=d.Barcode join ScanLog as f on d.LastScanLogID=f.ID join Station as g on f.StationID=g.ID"
                        + " where solveDeptName like '%" + solveDeptName + "%' and PauseType like '%" + PauseType + "%' and raiseDeptName like '%" + raiseDeptName + "%' and IsDel='false' and PauseStatus=40 order by ID desc";
                }
                else
                {

                    strSQL = @"select a.* ,a.PauseLevel as DrawLevel,c.DrawingNum,c.TaskNum,b.TaskID,g.StationName ,c.TaskNum + '-' + c.DrawingNum as effectTack "
                        + ", case when a.PauseLevel = '1' then '一般' when a.PauseLevel = '2' then '紧急' when a.PauseLevel = '3' then '特急' else Convert(varchar(6),a.PauseLevel) end as PauseLevelName"
                        + ", case when a.PauseStatus='5' then '审核中' when a.PauseStatus = '10' then '待接收' when a.PauseStatus = '20' then '暂停处理中' when a.PauseStatus = '30' then '已处理，待确认' when a.PauseStatus='40' then '已完成' else convert(varchar(20), a.PauseStatus) end as PauseStatussolve "
                        + " from BC_PauseTask as a join BarCode  as b on a.BarCodeFull=b.BarcodeFull join Task as c on c.ID=b.TaskID"
                        + " join LastScanRecord as d on a.BarCodeFull=d.Barcode join ScanLog as f on d.LastScanLogID=f.ID join Station as g on f.StationID=g.ID"
                        + " where solveDeptName like '%" + solveDeptName + "%' and PauseType like '%" + PauseType + "%' and raiseDeptName like '%" + raiseDeptName + "%' and IsDel='false' and PauseStatus<40 order by ID desc";
                }

                return Common.AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("统计所有新扫描数量时出现异常" + e.Message.ToString());
            }
        }
        public static DataTable GetPausedPauseTaskMyList(string Dept)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {

                strSQL = @"select * from BC_PauseTask where  IsDel='false' and PauseStatus='10' and SolveDeptName='" + Dept + "'"
                        + "union all "
                        + "select * from BC_PauseTask where  IsDel='false' and PauseStatus='20' and SolveDeptName='" + Dept + "' "
                        + "union all "
                        + "select * from BC_PauseTask where  IsDel='false' and PauseStatus='30' and RaiseDeptName='" + Dept + "' order by id asc";


                return Common.AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("统计所有新扫描数量时出现异常" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取暂停的条码
        /// </summary>
        /// <param name="DeptCode">指定部门，若为空则为全部</param>
        /// <returns></returns>
        //        public static DataTable GetPausedBarcodeList(string DeptCode, string PauseTypeID)
        //        {
        //            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
        //            string strSQL;
        //            try
        //            {
        //                strSQL = @"Select a.BarCode,a.ScanTime,a.StationID,a.Remark,f.PauseType,b.DeptCode,b.StationName,c.AllowSplitAmount,c.TaskID,e.DeptName,
        //							d.TaskNum,d.DrawingNum,d.ProductName,d.CraftRoute,d.MakeAmount
        //                            From (SELECT Barcode,max([ScanTime]) as NewTime
        //                              FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] where WorkState = '2'" +
        //                              @" Group by Barcode) as rel Left Join dbo.ScanLog a On rel.Barcode = a.Barcode And rel.NewTime = a.ScanTime
        //                              Join dbo.Station b On a.StationID = b.ID
        //                              Join dbo.BarCode c On a.Barcode = c.BarcodeFull
        //                              join dbo.Task d On c.TaskID = d.ID
        //                              join dbo.DeptEnum e On b.DeptCode = e.DeptCode
        //                              left join dbo.Sys_PasueReason f On a.PauseTypeID = f.ID" +
        //                              " Where b.DeptCode like '%" + DeptCode + "%' And c.Enable = '1'";
        //                if (!string.IsNullOrEmpty(PauseTypeID))
        //                {
        //                    strSQL += " And [PauseTypeID] = '" + PauseTypeID + "' Order By [ScanTime] DESC";
        //                }
        //                else
        //                {
        //                    strSQL += " Order By [ScanTime] DESC";
        //                }
        //                return Common.AddTableRowsID(DBI.Execute(strSQL, true));
        //            }
        //            catch (Exception e)
        //            {
        //                throw new Exception("统计所有新扫描数量时出现异常" + e.Message.ToString());
        //            }
        //        }

        /// <summary>
        /// 统计各工位在制品
        /// </summary>
        /// <param name="DeptCode">指定部门</param>
        /// <returns></returns>
        public static DataTable GetStationWorking(string DeptCode)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            try
            {
                strSQL = @"Select a.ID as StationID,a.DeptCode,a.StationName,c.UserName,b.DeptName,z.WorkingAmount From dbo.Station a Join (select r.StationID, COUNT(StationID) as WorkingAmount From 
                            (SELECT 
                                  [Barcode]
                                  ,[StationID]
                                  ,[StationName]
                                  ,Max(ScanTime) as Time
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] a join dbo.Station b on a.StationID= b.ID where b.DeptCode like '%" + DeptCode + "%'" +
                              @" group by [Barcode],[StationID],[StationName]) as r join dbo.BarCode x on r.Barcode = x.BarcodeFull
                              Join dbo.Task y on x.TaskID = y.ID
                              Where y.Enable = '1' GROUP by r.StationID) as z On z.StationID = a.ID
                              Join dbo.DeptEnum b on a.DeptCode = b.DeptCode
                              Join dbo.UserInfo c On a.ChargeBy = c.UserAccount";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计当月每日扫描概况
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <param name="ProductModel"></param>
        /// <returns></returns>
        public static DataTable GetThisMonthScanInfo(string DeptCode, string ProductModel, int month)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            try
            {
                strSQL = @"SELECT COUNT(DAY(ScanTime)) as Times,DAY(ScanTime) as day
                                  FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] a Join dbo.Station b on a.StationID = b.ID
                                  Join dbo.BarCode c On a.Barcode = c.BarcodeFull
                                  Join dbo.Task d On c.TaskID = d.ID
                                  where year(ScanTime) = year(GETDATE())
                                  And month(ScanTime) = " + month + "" +
                                @"And c.Enable = 1 
                                  And d.Enable= 1
                                  And d.ProductModel = '" + ProductModel + "'" +
                                  " And b.DeptCode like '%" + DeptCode + "%'" +
                                  " group by DAY(ScanTime) Order By DAY(ScanTime)";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("统计当月每日扫描概况时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计车间概况总览：工位数、扫描数、打印数、条码数
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <param name="ProductModel"></param>
        /// <returns></returns>
        public static DataTable GetWorkShopGlobel(string DeptCode, string ProductModel)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            try
            {
                strSQL = @"Select 
                             (Select COUNT(a.ID) From dbo.BarCode a Join dbo.Task b On a.TaskID = b.ID Where b.DeptCode like '%" + DeptCode + "%' And b.ProductModel = '" + ProductModel + "' And a.Enable = 1 And b.Enable = 1) as BNum," +
                            "(Select COUNT(a.ID) From dbo.PrintLog a Join dbo.BarCode b On a.BarcodeID = b.ID Join dbo.Task c On b.TaskID = c.ID Where c.DeptCode like '%" + DeptCode + "%' And c.ProductModel = '" + ProductModel + "' And b.Enable = 1 And c.Enable = 1) as PNum," +
                            "(Select COUNT(a.ID) From dbo.ScanLog a Join dbo.BarCode b On a.Barcode = b.BarcodeFull Join dbo.Task c On b.TaskID = c.ID Join dbo.Station d On a.StationID = d.ID Where d.DeptCode like '%" + DeptCode + "%' And c.ProductModel = '" + ProductModel + "' And b.Enable = 1 And c.Enable = 1) as CNum," +
                            "(Select COUNT(a.ID) From dbo.Station a Where a.DeptCode like '%" + DeptCode + "%') as SNum";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("统计车间概况总览时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 统计指定车间工位流转情况，车间代号为空时为全部车间
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <param name="ProductModel"></param>
        /// <returns></returns>
        public static DataTable GetStationGlobel(string DeptCode, string ProductModel)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            try
            {
                strSQL = @"select b.StationName,COUNT(StationID) as CountNum from dbo.ScanLog a
                                Join dbo.Station b On a.StationID = b.ID
                                Join dbo.BarCode c On a.Barcode = c.BarcodeFull
                                Join dbo.Task d On c.TaskID = d.ID
                                Where c.Enable = 1 And d.Enable = 1 And b.DeptCode like '%" + DeptCode + "%' And d.ProductModel = '" + ProductModel + "'" +
                                " group by b.ID,b.StationName order by COUNT(b.ID) desc";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("统计指定车间工位流转情况时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获得生产任务总量
        /// </summary>
        /// <returns></returns>
        public static string GetWxAllTaskCount(string DeptCode, string WxDept, string Month)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            if (WxDept == "全部") WxDept = "";
            try
            {
                strSQL = @"select count(*)  from dbo.Sys_WX_ExportInfo as a left join dbo.BarCode as b on a.BarcodeID = b.ID where b.Enable = 1 and MONTH(a.CreateTime) like '%" + Month + "%' and a.DeptCode like '%" + DeptCode + "%' and a.WxCompany like '%" + WxDept + "%'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获得生产任务总量时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获得未签收任务总量
        /// </summary>
        /// <returns></returns>
        public static string GetWxNotSignedTaskCount(string DeptCode, string WxDept, string Month)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            if (WxDept == "全部") WxDept = "";
            try
            {
                strSQL = @"select count(*)  from dbo.Sys_WX_ExportInfo as a left join dbo.BarCode as b on a.BarcodeID = b.ID where b.Enable = 1 and MONTH(a.CreateTime) like '%" + Month + "%' and a.DeptCode like '%" + DeptCode + "%' and a.WxCompany like '%" + WxDept + "%' and SignState = 0";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获得未签收任务总量时出现异常" + e.Message.ToString());
            }
        }


        /// <summary>
        /// 获得已签收任务总量
        /// </summary>
        /// <returns></returns>
        public static string GetWxSignedTaskCount(string DeptCode, string WxDept, string Month)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            if (WxDept == "全部") WxDept = "";
            try
            {
                strSQL = @"select count(*)  from dbo.Sys_WX_ExportInfo as a left join dbo.BarCode as b on a.BarcodeID = b.ID where b.Enable = 1 and MONTH(a.CreateTime) like '%" + Month + "%' and a.DeptCode like '%" + DeptCode + "%' and a.WxCompany like '%" + WxDept + "%' and SignState = 1";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获得已签收任务总量时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获得已完成任务总量
        /// </summary>
        /// <returns></returns>
        public static string GetWxFinishedTaskCount(string DeptCode, string WxDept, string Month)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            if (WxDept == "全部") WxDept = "";
            try
            {
                strSQL = @"select count(*)  from dbo.Sys_WX_ExportInfo as a left join dbo.BarCode as b on a.BarcodeID = b.ID where b.Enable = 1 and MONTH(a.CreateTime) like '%" + Month + "%' and a.DeptCode like '%" + DeptCode + "%' and a.WxCompany like '%" + WxDept + "%' and SignState = 2";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获得已完成任务总量" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 处于告警状态的总量
        /// </summary>
        /// <returns></returns>
        public static string GetWxWarningTaskCount(string DeptCode, string WxDept, string Month)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            if (WxDept == "全部") WxDept = "";
            try
            {
                strSQL = @"select count(*)  from dbo.Sys_WX_ExportInfo as a left join dbo.BarCode as b on a.BarcodeID = b.ID where b.Enable = 1 and MONTH(a.CreateTime) like '%" + Month + "%' and a.DeptCode like '%" + DeptCode + "%' and a.WxCompany like '%" + WxDept + "%' and  SignState = 1 and (FinishDate - CURRENT_TIMESTAMP)<= 3";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("处于告警状态的总量" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取线性图数据源
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <param name="WxDept"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public static DataTable GetLineChartDataSource(string DeptCode, string WxDept, string Month)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            if (WxDept == "全部") WxDept = "";
            try
            {
                strSQL = @"select convert(varchar(12),c.ScanTime,112) as days,COUNT (convert(varchar(12),c.ScanTime,112)) as Times from dbo.Sys_WX_ExportInfo as a inner join dbo.BarCode as b on a.BarcodeID = b.ID
                                         inner join dbo.ScanLog as c on b.BarcodeFull = c.Barcode
                                         inner join dbo.Station as d on c.StationID = d.ID
                                         where MONTH(c.ScanTime) like '%" + Month
                                             + "%' and a.DeptCode like '%" + DeptCode
                                              + "%'and WxCompany like '%" + WxDept
                                                + "%' and b.Enable = 1 and d.IsOutSide = 1 and d.StationName like '%#%' group by convert(varchar(12),c.ScanTime,112) order by  convert(varchar(12),c.ScanTime,112) asc";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取线性图数据源" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取外协任务完成情况柱状图
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <param name="WxDept"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public static DataTable GetBarChartDataSource(string DeptCode, string WxDept, string Month)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            if (WxDept == "全部") WxDept = "";
            try
            {
                strSQL = @"select convert(varchar(12),c.ScanTime,112) as days,COUNT (convert(varchar(12),c.ScanTime,112)) as Times from dbo.Sys_WX_ExportInfo as a inner join dbo.BarCode as b on a.BarcodeID = b.ID
                                         inner join dbo.ScanLog as c on b.BarcodeFull = c.Barcode
                                         inner join dbo.Station as d on c.StationID = d.ID
                                         where MONTH(c.ScanTime) like '%" + Month
                                             + "%' and a.DeptCode like '%" + DeptCode
                                              + "%'and WxCompany like '%" + WxDept
                                                + "%' and b.Enable = 1 and d.IsOutSide = 1 and d.StationName like '%#%' group by convert(varchar(12),c.ScanTime,112) order by  convert(varchar(12),c.ScanTime,112) asc";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取线性图数据源" + e.Message.ToString());
            }
        }
    }
}