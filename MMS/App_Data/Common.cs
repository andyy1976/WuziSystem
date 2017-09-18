using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using Camc.Web.Library;
using System.Configuration;
using System.Web.UI;

namespace mms
{
    public class Common
    {
        private static string DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
        /// <summary>
        /// 给已有Table添加行号
        /// </summary>
        /// <param name="srcTable"></param>
        /// <returns></returns>
        public static DataTable AddTableRowsID(DataTable dt)
        {
            //需要返回的值
            DataTable dtNew;
            if (dt.Columns.IndexOf("RowsId") >= 0)
            {
                dtNew = dt;
            }
            else
            {
                int rowLength = dt.Rows.Count;
                int colLength = dt.Columns.Count;
                DataRow[] newRows = new DataRow[rowLength];

                dtNew = new DataTable();
                //第一行添加RowsID列
                dtNew.Columns.Add("RowsId");
                for (int i = 0; i < colLength; i++)
                {
                    dtNew.Columns.Add(dt.Columns[i].ColumnName);
                    //复制dt中的数据
                    for (int j = 0; j < rowLength; j++)
                    {
                        if (newRows[j] == null)
                        {
                            newRows[j] = dtNew.NewRow();
                        }
                        //将其他数据填充到第二列之后，第一列已为新增的序号列
                        newRows[j][i + 1] = dt.Rows[j][i];
                    }
                }
                foreach (DataRow row in newRows)
                {
                    dtNew.Rows.Add(row);
                }
            }
            //对序号填充，从1递增
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dtNew.Rows[i]["RowsId"] = i + 1;
            }
            return dtNew;
        }

        /// <summary>
        /// 获取指定用户信息
        /// </summary>
        /// <param name="UserAccount"></param>
        /// <returns>真实名称、部门、部门代号</returns>
        public static List<string> GetUserInfoByAccount(string UserAccount)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT UserName+'*'+b.DeptName+'*'+b.DeptCode FROM [ProductBarCodeManagementAndTrack].[dbo].[UserInfo] a 
                            join dbo.DeptEnum b on a.DeptID = b.DeptCode 
                            Where a.UserAccount = '" + UserAccount + "'";
                string strUserInfo = DBI.GetSingleValue(strSQL);
                List<string> info = new List<string>();
                if (!string.IsNullOrEmpty(strUserInfo))
                {
                    info.Add(strUserInfo.Split('*')[0]);
                    info.Add(strUserInfo.Split('*')[1]);
                    info.Add(strUserInfo.Split('*')[2]);
                }
                return info;
            }
            catch (Exception e)
            {
                throw new Exception("将任务编码转换为任务类别失败！" + e.Message.ToString());
            }
        }

        //读取EXCEL中的数据并形成一个DataTable
        public static DataTable ReadExcel(string sExcelFile)
        {
            string strSql;
            string tableName;
            DataTable schemaTable;
            DataTable ExcelTable;
            DataSet ds = new DataSet();

            //Excel的连接
            OleDbConnection objConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sExcelFile + ";" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'");
            objConn.Open();
            schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            tableName = schemaTable.Rows[0][2].ToString().Trim();//获取Excel的表名，默认值是sheet1  
            strSql = "select * from [" + tableName + "] Where [项次] IS NOT NULL";

            //连接OLE DB
            OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
            OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
            myData.Fill(ds, tableName);//填充数据
            objConn.Close();

            //填充DataTable
            ExcelTable = ds.Tables[tableName];

            if (ExcelTable.Columns.Count == 12)
            {
                foreach (DataRow row in ExcelTable.Rows)
                {
                    //获取任务编号
                    string Identifier = row["任务编号"].ToString();

                    if (!string.IsNullOrEmpty(Identifier))
                    {

                    }
                    else
                    {
                        row.Delete();
                    }
                }
                return ExcelTable;
            }
            else
            {
                throw new Exception("请检测文件，确认文件符合模版要求！");
            }
        }

        /// <summary>
        /// 通过任务号获得型号
        /// </summary>
        /// <param name="taskNum"></param>
        /// <returns></returns>
        public static string GetProductModelByTaskNum(string taskNum)
        {
            if (!string.IsNullOrEmpty(taskNum))
            {
                string taskHead = taskNum.Split('-')[0].ToString();
                if (taskHead.Length == 6)
                {
                    return Common.GetTaskNameByNumber(taskNum.Substring(0, 2));
                }
                else if (taskHead.Length == 7)
                {
                    return Common.GetTaskNameByNumber(taskNum.Substring(0, 3));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过任务号获取车间
        /// </summary>
        public static string GetDeptIDByTaskNum(string taskNum)
        {
            if (!string.IsNullOrEmpty(taskNum))
            {
                if (taskNum.Split('-').Length > 0)
                {
                    return taskNum.Split('-')[1].ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //根据任务类别编码获取任务类别
        public static string GetTaskNameByNumber(string TaskNumber)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select Name From BarCodeEnum Where TaskNumber = '" + TaskNumber + "' and Type = 1";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("将任务编码转换为任务类别失败！" + e.Message.ToString());
            }
        }


        //根据车间编号获取车间名称
        public static string GetDeptCodeByCode(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select DeptName From DeptEnum Where DeptCode = '" + DeptCode + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("将部门ID转换为部门代号时出现异常！" + e.Message.ToString());
            }
        }

        //获取部门列表
        public static DataTable GetDeptList()
        {
            DataTable StationInfo = new DataTable();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);

            string strSQL;
            try
            {
                strSQL = "Select * From DeptEnum  Where Enable = '1' Order By [Order]";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取部门列表信息出错！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTaskList(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select * From Task Where Enable = '1' And DeptCode like '%" + DeptCode + "%'";
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("将任务编码转换为任务类别失败！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 检查任务是否存在条码
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public static bool CheckTaskExistBarcode(string TaskID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL = "";
            try
            {
                strSQL = "Select ID From [ProductBarCodeManagementAndTrack].[dbo].[BarCode] Where TaskID = '" + TaskID + "' And [Enable] = '1'";
                return DBI.CheckExistData(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("检查任务是否存在条码时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 创建生产任务管理操作日志
        /// </summary>
        /// <param name="operateType">操作类型。创建:create,更新:update,删除:delete。</param>
        /// <param name="UserAccount"></param>
        /// <param name="LogID"></param>
        /// <returns></returns>
        public static string CreateTaskOperateLog(string operateType, string UserAccount, string taskID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            DBI.OpenConnection();
            string strSQL = "";
            try
            {
                DBI.BeginTrans();
                //创建任务生成日志并返回日志记录编号
                strSQL = @"INSERT INTO [ProductBarCodeManagementAndTrack].[dbo].[TaskOperateLog]
                                   ([CreateDate]
                                   ,[CreatePerson],[TaskID],[Type])
                             VALUES
                                   ('" + DateTime.Now + "','" + UserAccount + "','" + taskID + "' ,'" + operateType + "')" +
                            "Select SCOPE_IDENTITY()";
                string logID = DBI.GetSingleValue(strSQL);
                DBI.CommitTrans();
                return logID;
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("创建生产任务操作日志时出现异常！" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        /// <summary>
        /// 设置生产任务日志操作结果
        /// </summary>
        /// <param name="logID">日志ID</param>
        /// <param name="operateResult">结果类型。成功:operatedSucceed，失败:operatedFailed</param>
        public static void SetTaskOperateLogResult(string logID, string operateResult)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            DBI.OpenConnection();
            string strSQL = "";
            try
            {
                DBI.BeginTrans();
                //创建任务生成日志并返回日志记录编号
                switch (operateResult)
                {
                    case "operatedSucceed":
                        strSQL = "Update [ProductBarCodeManagementAndTrack].[dbo].[TaskOperateLog] Set Result = '操作成功' Where ID = '" + logID + "'";
                        break;
                    case "operatedFailed":
                        strSQL = "Update [ProductBarCodeManagementAndTrack].[dbo].[TaskOperateLog] Set Result = '操作失败' Where ID = '" + logID + "'";
                        break;

                }
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("设置生产任务日志操作结果时出现异常！" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        //根据部门ID号获取工位信息
        public static DataTable GetStationInfoByDeptCode(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                if (string.IsNullOrEmpty(DeptCode) || DeptCode == "0")
                {
                    strSQL = @"Select  a.ID
                                      ,a.StationType
                                      ,a.StationName
                                      ,b.UserName
                                      ,a.TypeName
                                      ,c.DeptName
                                      ,case when a.IsOutSide =1 then '是' else '否' end as IsOutSide
                                      ,IP
                                      ,a.[Enable] From dbo.Station   as a 
                                      left join        dbo.UserInfo  as b on a.ChargeBy = b.UserAccount
                                      left join        dbo.DeptEnum  as c on a.DeptCode = c.DeptCode
                                      Where a.Enable = '1' order by a.DeptCode asc";
                }
                else
                {
                    strSQL = @"Select  a.ID
                                      ,a.StationType
                                      ,a.StationName
                                      ,b.UserName
                                      ,a.TypeName
                                      ,c.DeptName
                                      ,case when a.IsOutSide =1 then '是' else '否' end as IsOutSide
                                      ,IP
                                      ,a.[Enable] From dbo.Station   as a 
                                      left join        dbo.UserInfo  as b on a.ChargeBy = b.UserAccount
                                      left join        dbo.DeptEnum  as c on a.DeptCode = c.DeptCode
                                      Where a.Enable = '1' and  c.DeptCode = '" + DeptCode + "'order by a.DeptCode asc";
                }
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("将任务编码转换为任务类别失败！" + e.Message.ToString());
            }
        }

        //根据部门ID查询工位分类信息
        public static DataTable GetStationCategoryByDeptID(string DeptID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                if (string.IsNullOrEmpty(DeptID) || DeptID == "0")
                {
                    strSQL = @"SELECT a.[ID]
                                     ,[TypeID]
                                     ,[Name]
                                     ,a.DeptID
                                     ,b.DeptName
                                      FROM [ProductBarCodeManagementAndTrack].[dbo].[StationType]  as a left join dbo.DeptEnum as b on a.DeptID = b.DeptCode     
                                      ORDER BY [DeptID] ASC";
                }
                else
                {
                    strSQL = @"SELECT a.[ID]
                                     ,[TypeID]
                                     ,[Name]
                                     ,a.DeptID
                                     ,b.DeptName
                                      FROM [ProductBarCodeManagementAndTrack].[dbo].[StationType]  as a left join dbo.DeptEnum as b on a.DeptID = b.DeptCode     
                                      where a.DeptID = '" + DeptID + "' ORDER BY [DeptID] ASC";
                }
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("将任务编码转换为任务类别失败！" + e.Message.ToString());
            }
        }


        //根据工位ID号获取工位信息
        public static DataTable GetStationInfoByID(string ID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"Select * From dbo.Station where ID = '" + ID + "'";

                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("将任务编码转换为任务类别失败！" + e.Message.ToString());
            }
        }

        //根据部门ID号获取工位分类信息
        public static DataTable GetStationType(string DeptID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"Select * From dbo.StationType where DeptID = '" + DeptID + "'";
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("根据部门获取工位分类信息失败！" + e.Message.ToString());
            }
        }

        //根据IP获取工位信息
        public static DataTable GetStationInfoByIP(string IP)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"Select * From dbo.Station where IP = '" + IP + "' And Enable = '1'";
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("根据部门获取工位分类信息失败！" + e.Message.ToString());
            }
        }

        //根据IP获取当前车间所有工位信息
        public static DataTable GetDeptStationInfoByIP(string IP)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select * from Station where DeptCode in 
                                (select distinct(DeptCode) from dbo.Station where IP = '" + IP + "') And Enable = '1'";
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("根据部门获取工位分类信息失败！" + e.Message.ToString());
            }
        }

        //根据部门ID号获取员工信息
        public static DataTable GetUserInfoByDeptID(string DeptID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"Select * From dbo.UserInfo where DeptID = '" + DeptID + "'";
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("根据部门获取工位分类信息失败！" + e.Message.ToString());
            }
        }

        //更新指定ID的工位信息
        public static void UpdateStationInfo(string ID, string StationType, string StationName, string ChargeBy, string TypeName, string DeptCode, string IsOutSide, string IP)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            DBI.OpenConnection();
            string strSQL = "";

            try
            {
                DBI.BeginTrans();
                strSQL = @"Update [ProductBarCodeManagementAndTrack].[dbo].[Station] Set StationType = '" + StationType + "',StationName = '" + StationName + "',ChargeBy = '"
                    + ChargeBy + "',TypeName = '" + TypeName + "',DeptCode ='" + DeptCode + "',IsOutSide = '" + IsOutSide + "',IP = '" + IP + "' where ID = '" + ID + "'";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("更新工位信息出错，请联系管理员！" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        //插入新的工位信息
        public static void InsertStationInfo(string StationType, string StationName, string ChargeBy, string TypeName, string DeptCode, string IsOutSide, string IP, bool Enable)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            DBI.OpenConnection();
            string strSQL = "";

            try
            {
                DBI.BeginTrans();
                strSQL = @"Insert into [ProductBarCodeManagementAndTrack].[dbo].[Station](StationType,StationName,ChargeBy,TypeName,DeptCode,IsOutSide,IP,Enable) values ('"
                    + StationType + "','" + StationName + "','" + ChargeBy + "','" + TypeName + "','" + DeptCode + "','" + IsOutSide + "','" + IP + "','" + Enable + "')";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("插入工位信息出错，请联系管理员！" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        /// <summary>
        /// 根据条码号获取任务信息
        /// </summary>
        /// <param name="Barcode">条码号</param>
        /// <returns></returns>
        public static DataTable GetTaskInfoByBarcode(string Barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL = string.Empty;
            string barcodeClass = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(Barcode))
                {
                    barcodeClass = CheckBarcodeClass(Barcode);
                    switch (barcodeClass)
                    {
                        case "0":
                            strSQL = @"SELECT b.ID, ProductModel, TaskNum, DrawingNum, ProductName, PlanAmount, MakeAmount, ProgressDate,
                            FinishAmount, FinishDate, Material, Remark, CraftRoute, State, DistributeDate, b.Enable, TaskCreatedLogID, UpdatedOld,a.AllowSplitAmount    
                            FROM [ProductBarCodeManagementAndTrack].[dbo].[BarCode] a join dbo.Task b on a.TaskID = b.ID Where a.BarcodeFull = '" + Barcode + "' And a.Enable = '1'";
                            break;

                        case "1":
                            strSQL = @"SELECT b.ID, ProductModel, TaskNum, DrawingNum, ProductName, PlanAmount, MakeAmount, ProgressDate,
                            FinishAmount, FinishDate, Material, Remark, CraftRoute, State, DistributeDate, b.Enable, TaskCreatedLogID, UpdatedOld,a.TotalNumber as AllowSplitAmount    
                            FROM [ProductBarCodeManagementAndTrack].[dbo].[User_BZ_BarCode] a join dbo.Task b on a.TaskID = b.ID Where a.BarcodeFull = '" + Barcode + "' And a.Enable = '1'";
                            break;
                    }

                    return DBI.Execute(strSQL, true);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception("根据条码号获取任务信息时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取客户端真实IP
        /// </summary>a
        /// <returns></returns>
        public static string GetClientIP()
        {
            HttpContext context = HttpContext.Current;
            string ip = string.Empty;
            if (context.Request.ServerVariables["HTTP_VIA"] != null)  //判断是否有代理
            {
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                ip = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;
        }

        //查询条码流转信息
        public static DataTable GetBarCodeTrack(string Barcode)
        {
            //string BC = Barcode.Split('=')[1];
            DataTable dtReuslt = new DataTable();
            if (Barcode != null)
            {
                if (Barcode.Substring(0, 2) != "ZC")
                {
                    Barcode = Barcode.Split('=')[1];
                }
            }
            if (!string.IsNullOrEmpty(Barcode))
            {
                string barcodeClass = Common.GetBarcodeClass(Barcode);
                DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
                string strSQL = string.Empty;
                try
                {
                    switch (barcodeClass)
                    {
                        case "0":
                            strSQL = "Exec TrackBarcode @BarcodeFull = '" + Barcode + "'";
                            break;
                        case "1":
                            strSQL = "Exec BZ_TrackBarcode @BarcodeFull = '" + Barcode + "'";
                            break;
                    }
                    dtReuslt = DBI.Execute(strSQL, true);
                    dtReuslt.Columns.Add("WorkingState");
                    foreach (DataRow dr in dtReuslt.Rows)
                    {
                        switch (dr["WorkState"].ToString())
                        {
                            case "0":
                                dr["WorkingState"] = "开工";
                                break;
                            case "1":
                                dr["WorkingState"] = "完工";
                                break;
                            case "2":
                                dr["WorkingState"] = "暂停";
                                break;
                            case "3":
                                dr["WorkingState"] = "交接单提交";
                                break;
                            case "4":
                                dr["WorkingState"] = "交接单签收";
                                break;
                            case "5":
                                dr["WorkingState"] = "交接单驳回";
                                break;
                            case "6":
                                dr["WorkingState"] = "订制单提交";
                                break;
                            case "7":
                                dr["WorkingState"] = "订制单签收";
                                break;
                            case "8":
                                dr["WorkingState"] = "订制单驳回";
                                break;

                            default:
                                dr["WorkingState"] = "未知";
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("获取条码流转信息出错！" + e.Message.ToString());
                }
            }
            return dtReuslt;
        }

        //根据条码查询工艺路线
        public static DataTable GetCraftRouteByBarcode(string Barcode)
        {
            string barcodeClass = Common.GetBarcodeClass(Barcode);
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL = string.Empty;
            try
            {
                switch (barcodeClass)
                {
                    case "0":
                        strSQL = @"select b.CraftRoute from dbo.BarCode as a 
                                              left join dbo.Task as b on a.TaskID = b.ID  where a.BarcodeFull = '" + Barcode + "'";
                        break;
                    case "1":
                        strSQL = @"select b.CraftRoute from User_BZ_BarCode as a 
                                              left join dbo.Task as b on a.TaskID = b.ID  where a.BarcodeFull = '" + Barcode + "'";
                        break;
                }
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取条码流转信息出错！" + e.Message.ToString());
            }
        }


        /// <summary>
        /// 获取基础数据信息
        /// </summary>
        /// <param name="typeName">类型</param>
        /// <returns></returns>
        public static DataTable GetBaseInfoByType(string type)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            try
            {
                strSQL = "Select * From BarCodeEnum Where Type = '" + type + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取基础数据信息时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 判断指定用户是否有指定资源的访问许可
        /// </summary>
        /// <param name="UserAccount">用户名</param>
        /// <param name="SourceSign">资源标记</param>
        /// <returns></returns>
        public static bool IsHasRight(string UserAccount, string SourceSign)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            try
            {
                strSQL = @"SELECT *
                            FROM [dbo].[Sys_RoleInPermission] a
                            join dbo.Sys_Permission b on a.PermissionID = b.ID
                            join dbo.Sys_UserInRole c on a.RoleID = c.RoleID
                            join dbo.Sys_UserInfo_PWD d on c.UserID = d.ID
                            Where d.UserAccount = '" + UserAccount + "' And b.PerMissionSign = '" + SourceSign + "'";
                return DBI.CheckExistData(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("判断指定用户是否有指定资源的访问许可时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 页面判断是否当前用户有权限
        /// </summary>
        /// <param name="UserAccount">用户帐号</param>
        /// <param name="SourceSign">资源标记</param>
        /// <param name="page">传入页面</param>
        public static void CheckPermission(string UserAccount, string SourceSign, Page page)
        {
            bool hasRight = IsHasRight(UserAccount, SourceSign);
            if (!hasRight)
            {
                page.Response.Redirect("/Admin/NoRights.aspx");
            }
        }

        //根据用户权限获取用户过滤部门
        public static DataTable GetAllowDeptList(string userAccount)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            DataTable dtResult;
            try
            {
                strSQL = @"SELECT DeptCode, DeptName
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[Sys_DeptFilter] a
                              join dbo.UserInfo b on a.UserID = b.ID
                              join dbo.DeptEnum c on a.AllowDeptID = c.ID
                              Where b.UserAccount = '" + userAccount + "' Order By [Order]";
                dtResult = DBI.Execute(strSQL, true);
                if (dtResult.Rows.Count > 0)
                {
                    return dtResult;
                }
                else
                {
                    strSQL = @"SELECT DeptCode, DeptName
                                  FROM [ProductBarCodeManagementAndTrack].[dbo].[UserInfo] a
                                  join dbo.DeptEnum b on a.DeptID = b.DeptCode
                                  Where UserAccount = '" + userAccount + "'";
                    return DBI.Execute(strSQL, true);
                }
            }
            catch (Exception e)
            {
                throw new Exception("获取部门列表信息出错！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 从Cookie中读取用户帐号
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetUserAccountFromCookie(Page page)
        {
            if (page.Request.Cookies["userAccount"] != null)
            {
                return page.Request.Cookies["userAccount"].Value.ToString();
            }
            else
            {
                page.Response.Redirect("~/Login.aspx");
                return null;
            }
        }
        ///<summary>
        ///通过用户名获取部门名称
        ///</summary>
        public static string GetUserDeptFromAccount(Page page)
        {

            string userAccount = GetUserAccountFromCookie(page);
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            try
            {
                string SQL = @"select a.DeptName from [ProductBarCodeManagementAndTrack].[dbo].[DeptEnum] as a join [ProductBarCodeManagementAndTrack].[dbo].[UserInfo] as b on 
                             a.DeptCode=b.DeptID where b.UserAccount='" + userAccount + "'";
                return DBI.GetSingleValue(SQL);
            }
            catch (Exception e)
            {
                throw new Exception("获取部门名称时出现异常！" + e.Message.ToString());
            }
        }

        ///<summary>
        ///通过用户名获取部门名称
        ///</summary>
        public static string GetUserLevelFromAccount(Page page)
        {

            string userAccount = GetUserAccountFromCookie(page);
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            try
            {
                string SQL = @"select LevelID from UserInfo where  UserAccount='" + userAccount + "'";
                return DBI.GetSingleValue(SQL);
            }
            catch (Exception e)
            {
                throw new Exception("获取部门名称时出现异常！" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取全部角色列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRoleList()
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            try
            {
                strSQL = "Select * From [ProductBarCodeManagementAndTrack].[dbo].[Sys_Roles]";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取全部角色列表时出现异常" + e.Message.ToString());
            }
        }

        //获取系统全部权限资源列表
        public static DataTable GetSysSourceList()
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"Select * From dbo.Sys_Source";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取系统全部权限资源列表时出现异常" + e.Message.ToString());
            }
        }

        //获取指定角色的所拥有的资源权限
        public static DataTable GetSysSourceList(string roleID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"Select b.ID, SourceName, PerMissionSign, PID From [ProductBarCodeManagementAndTrack].[dbo].[Sys_RolePermission] a
                                join dbo.Sys_Source b on a.SourceID = b.ID Where a.RoleID = '" + roleID + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取指定角色的资源信息时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取所有已有条码的任务
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTaskListWhichHasBarcode()
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT distinct(a.[ID])
                              ,[ProductModel]
                              ,[TaskNum]
                              ,[DrawingNum]
                              ,[ProductName]
                              ,[PlanAmount]
                              ,[MakeAmount]
                              ,[ProgressDate]
                              ,[FinishAmount]
                              ,[FinishDate]
                              ,[Material]
                              ,[Remark]
                              ,[CraftRoute]
                              ,[State]
                              ,[DistributeDate]
                              ,[DeptCode]
                              ,[TaskCreatedLogID]
                              ,[UpdatedOld]
                          FROM [ProductBarCodeManagementAndTrack].[dbo].[Task] a join dbo.BarCode b on a.ID = b.TaskID";
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("获取所有已有条码的任务时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 通过图号获取任务信息
        /// </summary>
        /// <param name="drawingNum">图号</param>
        /// <returns></returns>
        public static DataTable GetTaskInfoByDrawingNum(string drawingNum)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select * FROM [ProductBarCodeManagementAndTrack].[dbo].[Task] Where [DrawingNum] = '" + drawingNum + "' And [Enable] = '1' ";
                return AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("通过图号获取任务信息时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 根据条码获取该条码最新扫描记录
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public static DataTable GetLastRecordByBarcode(string Barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "SELECT Top 1 * FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] Where Barcode ='" + Barcode + "' Order By ScanTime DESC";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("根据条码获取该条码最新扫描记录时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 根据天津公司部门编号获取211部门编号
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        public static string GetDeptCodeIn211(string DeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "select DeptCodeIn211 from dbo.DeptEnum where DeptCode='" + DeptCode + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("查询211公司部门编号失败！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 根据211部门编号获取天津公司部门编号
        /// </summary>
        /// <param name="DeptCodeIn211"></param>
        /// <returns></returns>
        public static string GetDeptCodeInTJ(string DeptCodeIn211)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "select DeptCode from dbo.DeptEnum where DeptCodeIn211 = '" + DeptCodeIn211 + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("查询天津公司部门编号失败！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 根据天津公司帐号查询211厂帐号
        /// </summary>
        /// <param name="UserAccount"></param>
        /// <returns></returns>
        public static string GetUserAccountIn211(string UserDeptCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "select UserIn211 from dbo.Sys_UserTo211 where UserDeptCode='" + UserDeptCode + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("查询用户在211公司的帐号失败！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取暂停分类列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPauseTypeList()
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select * From [ProductBarCodeManagementAndTrack].[dbo].[Sys_PasueReason] Where [Enable] = 1";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取暂停分类列表时出现异常" + e.Message.ToString());
            }
        }

        public static List<TaskDataBody> GetTaskDataBody(DataTable DataInput)
        {
            List<TaskDataBody> DataOutput = new List<TaskDataBody>();

            foreach (DataRow DataRow in DataInput.Rows)
            {
                string ID = "";//不管
                string ProductModel = Common.GetProductModelByTaskNum(DataRow["PLANTASK_NO"].ToString());
                string TaskNum = DataRow["PLANTASK_NO"].ToString().Trim().Trim('\r').Trim('\n');
                string DrawingNum = DataRow["DRAWING_NO"].ToString().Trim().Trim('\r').Trim('\n');
                string ProductName = DataRow["PART_NAME"].ToString();
                int PlanAmount = Convert.ToInt32(DataRow["DEL_DESIGNER_QUANTITY"].ToString());
                int MakeAmount = Convert.ToInt32(DataRow["PLAN_QUANTITY"].ToString());
                string ProgressDate = DataRow["PLAN_DATE"].ToString();
                int FinishAmount = Convert.ToInt32(DataRow["HAND_QUANTITY"].ToString());
                //string FinishDate = "";//未知
                string Material = "";//未知
                string Remark = DataRow["PLANTASK_MEMO"].ToString();
                string CraftRoute = DataRow["TECHNICS_LINE"].ToString();
                string State = "1";//1代表已签收，0代表未签收
                string DistributeDate = System.DateTime.Now.ToString();
                bool Enable = true;
                string DeptCode = Common.GetDeptCodeInTJ(DataRow["DEPTNAME"].ToString());
                string DisposeType = DataRow["PLANTASK_TYPE"].ToString();
                //string TaskCreatedLogID  = "";//未知，无用
                //string UpdatedOld = "";//未知，无用

                TaskDataBody newElement = new TaskDataBody();
                newElement.ID = ID;
                newElement.ProductModel = ProductModel;
                newElement.TaskNum = TaskNum;
                newElement.DrawingNum = DrawingNum;
                newElement.ProductName = ProductName;
                newElement.PlanAmount = PlanAmount;
                newElement.MakeAmount = MakeAmount;
                newElement.ProgressDate = ProgressDate;
                newElement.FinishAmount = FinishAmount;
                //newElement.FinishDate = FinishDate;
                newElement.Material = Material;
                newElement.Remark = Remark;
                newElement.CraftRoute = CraftRoute;
                newElement.State = State;
                newElement.DistributeDate = DistributeDate;
                newElement.Enable = Enable;
                newElement.DeptCode = DeptCode;
                newElement.DisposeType = DisposeType;
                //newElement.TaskCreatedLogID = TaskCreatedLogID;
                //newElement.UpdatedOld = UpdatedOld;

                DataOutput.Add(newElement);
            }
            return DataOutput;
        }

        /// <summary>
        /// 订制单接口，通过任务号图号获取对应条码列表
        /// </summary>
        /// <param name="taskNum"></param>
        /// <param name="drawingNum"></param>
        /// <returns></returns>
        public static DataTable GetBarcodeListDingZhi(string taskNum, string drawingNum)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT b.ID as BarcodeID,b.BarcodeFull, b.AllowSplitAmount
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[Task] a 
                              Join dbo.BarCode b On a.ID = b.TaskID
                              Where a.TaskNum = '" + taskNum + "' And a.DrawingNum = '" + drawingNum + "' And a.[Enable]  = '1' And b.[Enable] = '1'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("订制单接口，通过任务号图号获取对应条码列表时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 根据211部门代码获取天津部门名称
        /// </summary>
        /// <param name="DeptCodeIn211"></param>
        /// <returns></returns>
        public static string GetDeptNameBy211DeptCode(string DeptCodeIn211)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select  DeptName  from dbo.DeptEnum where DeptcodeIn211 = '" + DeptCodeIn211 + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("根据厂部门代码获取天津部门名称时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 通过订制单ID获取绑定条码
        /// </summary>
        /// <param name="DingZhiDanID">订制单RecID</param>
        /// <returns></returns>
        public static string GetBarcodeFromDingZhiDan(string DingZhiDanID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"Select Barcode FROM [ProductBarCodeManagementAndTrack].[dbo].[User_DingZhiRecord] Where [RecID] = '" + DingZhiDanID + "' And [Enable] = '1'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("通过订制单ID获取绑定条码时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 通过订制单ID获取绑定条码
        /// </summary>
        /// <param name="DingZhiDanID">订制单RecID</param>
        /// <returns></returns>
        public static int GetDingzhiDanIDInTJ(string RecId)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"Select ID FROM [ProductBarCodeManagementAndTrack].[dbo].[User_DingZhiRecord] Where [RecID] = '" + RecId + "' And [Enable] = '1'";
                string IDstr = DBI.GetSingleValue(strSQL);
                return string.IsNullOrEmpty(IDstr) ? 0 : Convert.ToInt32(IDstr);
            }
            catch (Exception e)
            {
                throw new Exception("通过订制单ID获取绑定条码时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 通过ProdJournalTable的RECID获取ProdJournalRoute
        /// </summary>
        /// <param name="RECID"></param>
        /// <returns></returns>
        public static string GetDingzhdRecid(string RECID)
        {
            string ConnectString = ConfigurationManager.AppSettings["AxConnectionString"];
            DBInterface DBI = DBFactory.GetDBInterface(ConnectString);
            string strSQL;
            try
            {
                strSQL = @"select b.RECID from ProdJournalTable as a left join ProdJournalRoute as b on a.JOURNALID = b.JOURNALID WHERE A.RECID = '" + RECID + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("出现异常！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 通过ID获取订制单状态
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string CheckDingZhiDanState(DingZhiBody dingZhiBody)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT [State] FROM [ProductBarCodeManagementAndTrack].[dbo].[User_DingZhiRecord] Where ID = '" + dingZhiBody.ID + "' And [Enable] = '1'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("出现异常！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 根据订制单ID更改订制单状态
        /// </summary>
        /// <param name="dingZhiBody"></param>
        public static void UpdateDingZhiDanState(DingZhiBody dingZhiBody)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            DBI.OpenConnection();
            DBI.BeginTrans();
            try
            {
                if (dingZhiBody.AcceptDate != DateTime.MinValue)
                {
                    strSQL = @"Update [ProductBarCodeManagementAndTrack].[dbo].[User_DingZhiRecord] Set [State] = '" + dingZhiBody.State + "'," +
                                               "[BackReason] = '" + dingZhiBody.BackReason + "',AcceptDate=getdate() Where [ID] = '" + dingZhiBody.ID + "'";
                }
                else
                {
                    strSQL = @"Update [ProductBarCodeManagementAndTrack].[dbo].[User_DingZhiRecord] Set [State] = '" + dingZhiBody.State + "'," +
                           "[BackReason] = '" + dingZhiBody.BackReason + "' Where [ID] = '" + dingZhiBody.ID + "'";
                }

                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("出现异常！" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        /// <summary>
        /// 通过扫码内容分析结果并格式化
        /// </summary>
        /// <param name="scanText"></param>
        /// <returns></returns>
        public static List<string> CheckScanInfo(string scanText)
        {
            List<string> scanInfo = new List<string>();

            string operationType;
            string productBarcode;
            string signID;

            try
            {
                int textLenth = scanText.Length;
                if (scanText.IndexOf("=") != -1)
                {
                    signID = Convert.ToInt32(scanText.Split('=')[0].Substring(0, 6)).ToString();
                    operationType = scanText.Split('=')[0].Substring(6, 2);
                    productBarcode = scanText.Split('=')[1];
                }
                else
                {
                    signID = string.Empty;
                    operationType = "PC";
                    productBarcode = scanText;
                }
                scanInfo.Add(operationType);
                scanInfo.Add(productBarcode);
                scanInfo.Add(signID);
                return scanInfo;
            }
            catch (Exception e)
            {
                throw new Exception("通过扫码内容分析结果并格式化时出现异常" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 根据条码 判断不同的条码类型
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public static string GetBarCodeType(string BarcodeFull)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string ret = "";
            string strSQL;
            try
            {
                strSQL = "Select BarcodeClass From User_BarcodeClass  Where BarcodeFull='" + BarcodeFull + "'";
                ret = DBI.GetSingleValue(strSQL);
                if (ret == "1")
                {
                    string isEixt = "select * from User_DingZhiRecord where BarCode='" + BarcodeFull + "' and State='1'";
                    if (DBI.CheckExistData(isEixt))
                    {
                        ret = "0";
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("获取超期对应时间推送表出错！" + e.Message.ToString());
            }
            return ret;
        }
        /// <summary>
        /// 获取条码分类
        /// </summary>
        /// <param name="Barcode">条码</param>
        /// <returns></returns>
        public static string GetBarcodeClass(string Barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "Select [BarcodeClass] From User_BarcodeClass Where [BarcodeFull] = '" + Barcode + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取指定订制单信息
        /// </summary>
        /// <param name="ID">订制单标识</param>
        /// <returns></returns>
        public static DingZhiBody GetDingZhiInfo(string ID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            DingZhiBody dingZhiBody = new DingZhiBody();
            string strSQL;
            try
            {
                strSQL = @"Select *,case when [State] = '0' then '已创建' when [State] = '1' then '已提交' when [State] = '2' then '待确认' when [State] = '3' then '已签收'  end as StateCN
                            FROM [ProductBarCodeManagementAndTrack].[dbo].[User_DingZhiRecord] Where [ID] = '" + ID + "'";
                DataTable dtDingZhi = DBI.Execute(strSQL, true);
                if (dtDingZhi.Rows.Count > 0)
                {
                    dingZhiBody.ID = ID;
                    dingZhiBody.RecId = dtDingZhi.Rows[0]["RecId"].ToString();
                    dingZhiBody.Barcode = dtDingZhi.Rows[0]["Barcode"].ToString();
                    dingZhiBody.TaskNum = dtDingZhi.Rows[0]["TaskNum"].ToString(); ;
                    dingZhiBody.ProductName = dtDingZhi.Rows[0]["ProductName"].ToString();
                    dingZhiBody.DrawingNum = dtDingZhi.Rows[0]["DrawingNum"].ToString();
                    dingZhiBody.Unit = dtDingZhi.Rows[0]["Unit"].ToString();
                    dingZhiBody.Amount = dtDingZhi.Rows[0]["Amount"].ToString();
                    dingZhiBody.DingZhiXingZhi = dtDingZhi.Rows[0]["DingZhiXingZhi"].ToString();
                    dingZhiBody.DingZhiDescription = dtDingZhi.Rows[0]["DingZhiDescription"].ToString();
                    dingZhiBody.OrderWorkShop = dtDingZhi.Rows[0]["OrderWorkShop"].ToString();
                    dingZhiBody.DisposeWorkShop = dtDingZhi.Rows[0]["DisposeWorkShop"].ToString();
                    dingZhiBody.OrderDate = string.IsNullOrEmpty(dtDingZhi.Rows[0]["OrderDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dtDingZhi.Rows[0]["OrderDate"]);
                    dingZhiBody.AppointDate = dtDingZhi.Rows[0]["AppointDate"].ToString();
                    dingZhiBody.Remark = dtDingZhi.Rows[0]["Remark"].ToString();
                    dingZhiBody.CreatePerson = dtDingZhi.Rows[0]["CreatePerson"].ToString();
                    dingZhiBody.State = dtDingZhi.Rows[0]["StateCN"].ToString();
                    dingZhiBody.BackReason = dtDingZhi.Rows[0]["BackReason"].ToString();
                }
                return dingZhiBody;
            }
            catch (Exception e)
            {
                throw new Exception("获取指定订制单信息时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 生产任务操作更新日志
        /// </summary>
        /// <param name="UserAccount"></param>
        /// <param name="CreateDate"></param>
        /// <param name="OperateType"></param>
        /// <param name="RECID"></param>
        /// <param name="OperateResult"></param>
        public static void UpdateShengchanLog(string UserAccount, string CreateDate, string OperateType, string RECID, string OperateResult)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"insert into dbo.ShengchanOperateLog(UserAccount,CreateDate,OperateType,RECID,Result) values ('" + UserAccount
                                                                                                             + "','" + CreateDate
                                                                                                             + "','" + OperateType
                                                                                                             + "','" + RECID
                                                                                                             + "','" + OperateResult
                                                                                                             + "'); ";
                DBI.Execute(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("更新生产操作日志出现异常！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 根据部装条码号获取部装任务信息
        /// </summary>
        /// <param name="Barcode">条码号</param>
        /// <returns></returns>
        public static DataTable GetBZTaskInfoByBarcode(string Barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            TaskDataBody taskBody = new TaskDataBody();
            try
            {
                strSQL = @"SELECT b.ID, ProductModel, TaskNum, DrawingNum, ProductName, PlanAmount, MakeAmount, ProgressDate,
                            FinishAmount, FinishDate, Material, Remark, CraftRoute, State, DistributeDate, b.Enable, TaskCreatedLogID, UpdatedOld,a.AllowSplitAmount
                            ,a.[ChildProductName],a.[ChildDrawingNum],a.[ChildExpectFinishDate],a.[ZhiKongCardNum]
                            FROM [dbo].[User_BZ_BarCode] a join dbo.Task b on a.TaskID = b.ID Where a.BarcodeFull = '" + Barcode + "' And a.Enable = '1'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("根据条码号获取任务信息时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 创建条码操作日志
        /// </summary>
        /// <param name="operateType">操作类型。创建:create,分码:split,删除:delete。</param>
        /// <param name="UserAccount"></param>
        /// <param name="LogID"></param>
        /// <returns></returns>
        public static void CreateBarcodeOperateLog(string operateType, string UserAccount, string barcodeID, string result)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            DBI.OpenConnection();
            string strSQL = "";
            try
            {
                DBI.BeginTrans();

                strSQL = @"INSERT INTO [ProductBarCodeManagementAndTrack].[dbo].[CreateBarcodeOperateLog]
                                   ([CreateDate]
                                   ,[CreatePerson],[BarcodeID],[Type],[Result])
                             VALUES
                                   ('" + DateTime.Now + "','" + UserAccount + "','" + barcodeID + "' ,'" + operateType + "','" + result + "')" +
                            "Select SCOPE_IDENTITY()";
                DBI.GetSingleValue(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("创建生产任务操作日志时出现异常！" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        /// <summary>
        /// 获取交接单订单信息
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public static JiaoJieDanModel.PartsOrder GetJiaoJieInfoByBarcode(string Barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL = string.Empty;
            string barcodeClass = string.Empty;
            JiaoJieDanModel.PartsOrder JiaoJieDanOrder = new JiaoJieDanModel.PartsOrder();
            try
            {
                barcodeClass = CheckBarcodeClass(Barcode);
                switch (barcodeClass)
                {
                    case "0":
                        strSQL = @"SELECT a.BarcodeFull, b.TaskNum, b.ProductName,b.DrawingNum, d.CertificateID,a.AllowSplitAmount 
                                FROM BarCode a
                                Left Join dbo.Task b On a.TaskID = b.ID
                                Left Join dbo.LastScanRecord c On a.BarcodeFull = c.Barcode
                                Left Join dbo.ScanLog d On c.LastScanLogID = d.ID
                                Where a.Enable = 1 And b.Enable = 1 And a.BarcodeFull = '" + Barcode + "'";
                        DataTable dtParts = DBI.Execute(strSQL, true);
                        if (dtParts.Rows.Count == 1)
                        {
                            JiaoJieDanOrder.Barcode = dtParts.Rows[0]["BarcodeFull"].ToString();
                            JiaoJieDanOrder.TaskNum = dtParts.Rows[0]["TaskNum"].ToString();
                            JiaoJieDanOrder.ProductName = dtParts.Rows[0]["ProductName"].ToString();
                            JiaoJieDanOrder.DrawingNum = dtParts.Rows[0]["DrawingNum"].ToString();
                            JiaoJieDanOrder.CertificateID = dtParts.Rows[0]["CertificateID"].ToString();
                            JiaoJieDanOrder.ProductAmount = string.IsNullOrEmpty(dtParts.Rows[0]["AllowSplitAmount"].ToString()) ? 0 : Convert.ToInt32(dtParts.Rows[0]["AllowSplitAmount"]);
                        }
                        break;

                    case "1":
                        strSQL = @"SELECT a.BarcodeFull, b.TaskNum, b.ProductName,b.DrawingNum, d.CertificateID,a.TotalNumber 
                                FROM User_BZ_BarCode a
                                Left Join dbo.Task b On a.TaskID = b.ID
                                Left Join dbo.LastScanRecord c On a.BarcodeFull = c.Barcode
                                Left Join dbo.ScanLog d On c.LastScanLogID = d.ID
                                Where a.Enable = 1 And b.Enable = 1 And a.BarcodeFull = '" + Barcode + "'";
                        DataTable dtKit = DBI.Execute(strSQL, true);
                        if (dtKit.Rows.Count == 1)
                        {
                            JiaoJieDanOrder.Barcode = dtKit.Rows[0]["BarcodeFull"].ToString();
                            JiaoJieDanOrder.TaskNum = dtKit.Rows[0]["TaskNum"].ToString();
                            JiaoJieDanOrder.ProductName = dtKit.Rows[0]["ProductName"].ToString();
                            JiaoJieDanOrder.DrawingNum = dtKit.Rows[0]["DrawingNum"].ToString();
                            JiaoJieDanOrder.CertificateID = dtKit.Rows[0]["CertificateID"].ToString();
                            JiaoJieDanOrder.ProductAmount = string.IsNullOrEmpty(dtKit.Rows[0]["TotalNumber"].ToString()) ? 0 : Convert.ToInt32(dtKit.Rows[0]["TotalNumber"]);
                        }
                        break;
                }
                return JiaoJieDanOrder;
            }
            catch (Exception e)
            {
                throw new Exception("获取交接单订单信息时出现异常" + "\n异常二维码：" + JiaoJieDanOrder.Barcode + e.Message.ToString());
            }
        }

        /// <summary>
        /// 检查零件条码是否存在
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public static bool CheckBarcodeExist(string barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL = "";
            bool result = false;
            try
            {
                strSQL = @"Select ID From [dbo].[BarCode] Where [BarcodeFull] = '" + barcode + "'";
                result = DBI.CheckExistData(strSQL);
                if (!result)
                {
                    strSQL = "Select ID From [dbo].[User_BZ_BarCode] Where [BarcodeFull] = '" + barcode + "'";
                    result = DBI.CheckExistData(strSQL);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("检查零件条码是否存在时出现异常！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 检查合格证是否存在
        /// </summary>
        /// <param name="certificateCode">合格证号</param>
        /// <returns></returns>
        public static bool CheckCertificateExist(string certificateCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL = "";
            try
            {
                strSQL = @"Select * FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] Where CertificateID = '" + certificateCode + "'";
                return DBI.CheckExistData(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("检查合格证是否存在时出现异常！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 通过交接单ID获取交接单状态
        /// </summary>
        /// <param name="jjdID"></param>
        /// <returns></returns>
        public static string GetJiaoJieDanState(string jjdID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL = "";
            try
            {
                strSQL = @"Select OperationState From User_JiaoJieDan_Main Where [ID] = '" + jjdID + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获取交接单状态时出现异常！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 保存扫描记录
        /// </summary>
        /// <param name="scanLog"></param>
        public static void SaveScanRecord(ScanLogBody scanLog)
        {
            string strSQL;
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = @"INSERT INTO ScanLog
                               ([Barcode]
                               ,[ScanTime]
                               ,[StationID]
                               ,[ScanIP]
                               ,[ScanPerson]
                               ,[CertificateID]
                               ,[WorkState]
                               ,[Remark]
                               ,[PauseTypeID]
                               ,Enable)
                         VALUES
                               ('" + scanLog.Barcode + "','" + scanLog.ScanTime + "','" + scanLog.StationID + "','" + scanLog.ScanIP + "','" + scanLog.ScanPerson + "','" + scanLog.CertificateID + "'" +
                                    ",'" + scanLog.WorkState + "','" + scanLog.Remark + "', '" + scanLog.PauseTypeID + "','1');Select SCOPE_IDENTITY()";
                string logID = DBI.GetSingleValue(strSQL);
                strSQL = "Select * FROM LastScanRecord Where [Barcode] = '" + scanLog.Barcode + "'";
                if (!DBI.CheckExistData(strSQL))
                {
                    strSQL = "Insert Into LastScanRecord " +
                        "([Barcode],[LastScanLogID]) Values ('" + scanLog.Barcode + "', '" + logID + "')";
                }
                else
                {
                    strSQL = "Update LastScanRecord Set [LastScanLogID]='" + logID + "' Where [Barcode] = '" + scanLog.Barcode + "'";
                }
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("保存扫描记录时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        /// <summary>
        /// 通过合格证号获取条码号
        /// </summary>
        /// <param name="certificateCode"></param>
        /// <returns></returns>
        public static string GetBarcodeByCertificate(string certificateCode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL = "";
            try
            {
                strSQL = @"SELECT TOP 1 [Barcode]
                              FROM [ProductBarCodeManagementAndTrack].[dbo].[ScanLog] 
                              Where CertificateID = '" + certificateCode + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获取交接单状态时出现异常！" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 检查订制单是否存在
        /// </summary>
        /// <param name="dingzhidanNum"></param>
        /// <returns></returns>
        public static bool CheckDingZhiDanExist(string dingzhidanNum)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                if (dingzhidanNum.Length < 29 || dingzhidanNum.IndexOf("DZ=") == -1)
                {
                    return false;
                }
                else
                {
                    string id = dingzhidanNum.Substring(0, 6);
                    strSQL = @"Select * From User_DingZhiRecord Where ID = '" + id + "'";
                    return DBI.CheckExistData(strSQL);
                }
            }
            catch (Exception e)
            {
                throw new Exception("检查订制单是否存在时出现异常！" + e.Message.ToString());
            }

        }
        /// <summary>
        /// 检查条码类型
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <returns></returns>
        public static string CheckBarcodeClass(string barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            string barcodeClass = string.Empty;
            try
            {
                strSQL = @"Select [BarcodeClass] FROM User_BarcodeClass Where [BarcodeFull] = '" + barcode + "'";
                barcodeClass = DBI.GetSingleValue(strSQL);
                return barcodeClass;
            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取当前自己的暂停 qjq
        /// </summary>
        /// <param name="Dept"></param>
        /// <returns></returns>
        public static DataTable GetPausedPauseTaskMyList(string Dept)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
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
        /// 获取部门的类别：0、处室 1、天津工厂 2、未知 qjq
        /// </summary>
        /// <param name="Dept"></param>
        /// <returns></returns>
        public static string GetWorkShopByDeptName(string Dept)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {

                strSQL = @"select WorkShop from [DeptEnum] where DeptName='" + Dept + "' and Enable='true'";


                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("统计所有新扫描数量时出现异常" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取暂停列表
        /// </summary>
        /// <param name="PauseStatus">暂停状态</param>
        /// <param name="SolveDeptName">解决部门 处室</param>
        /// <param name="RaiseDeptName">提出部门 车间</param>
        /// <param name="PauseType">暂停原因</param>
        /// <returns></returns>
        public static DataTable GetPauseList(string PauseStatus, string SolveDeptName, string RaiseDeptName, string PauseType, string PauseLevel, string Order)
        {
            DataTable dt = new DataTable();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {


                strSQL = @"select * from VI_PauseList"
                        + " where IsDel='false' and PauseStatus in ({0}) and SolveDeptName like '%{1}%' and RaiseDeptName like '%{2}%' and PauseType like '%{3}%'";
                strSQL = string.Format(strSQL, PauseStatus, SolveDeptName, RaiseDeptName, PauseType);
                if (PauseLevel != "0" && !string.IsNullOrEmpty(PauseLevel))
                {
                    strSQL += " and PauseLevel='" + PauseLevel + "' ";
                }
                dt = DBI.Execute(strSQL + Order, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取列表时出现异常" + e.Message.ToString());
            }
            return dt;
        }
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="PauseStatus"></param>
        /// <param name="SolveDeptName"></param>
        /// <param name="RaiseDeptName"></param>
        /// <param name="PauseLevel"></param>
        /// <returns></returns>
        public static string GetPuaseCount(string PauseStatus, string SolveDeptName, string RaiseDeptName, string PauseLevel)
        {

            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = @"select count(*) from VI_PauseList"
                        + " where IsDel='false'  and PauseStatus in ({0}) and SolveDeptName like '%{1}%' and RaiseDeptName like '%{2}%' and IsParent='false'";
                strSQL = string.Format(strSQL, PauseStatus, SolveDeptName, RaiseDeptName);
                if (PauseLevel != "0" && !string.IsNullOrEmpty(PauseLevel))
                {
                    strSQL += " and PauseLevel='" + PauseLevel + "' ";
                }
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("获取列表时出现异常" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取每个部门的数量
        /// </summary>
        /// <param name="PauseStatus"></param>
        /// <returns></returns>
        public static DataTable GetPuaseDeptCount(string PauseStatus)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                string DeptType;
                string Dept;
                if (PauseStatus == "10" || PauseStatus == "20")
                {
                    Dept = "SolveDeptName";
                    DeptType = "0";
                }
                else
                {
                    Dept = "RaiseDeptName";
                    DeptType = "1";
                }
                strSQL = @" select DeptName,(select COUNT(*) from BC_PauseTask as b where b.{2}=a.DeptName and PauseStatus='{0}' and IsDel='false') as mainCount"
                            + " from DeptEnum as a where  a.WorkShop='{1}' and a.Enable='true'";
                strSQL = string.Format(strSQL, PauseStatus, DeptType, Dept);
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取列表时出现异常" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="PauseStatus"></param>
        /// <returns>
        /// 如果在40或50 则返回True  如果不在暂停中 也返回True
        /// 只有在 当前条码暂停中 且当前条码已完成或已驳回 返回Flase 证明当前条码不能被继续扫码
        /// </returns>
        public static bool GetPuaseIs40(string Barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                if (IsExitPause(Barcode))
                {
                    strSQL = "select count(*) from BC_PauseTask where BarCodeFull='{0}' and IsDel='false' and IsParent='false' and PauseStatus in (40,50)";
                    strSQL = string.Format(strSQL, Barcode);
                    string  ret= DBI.GetSingleValue(strSQL);
                    if (ret == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("GetPuaseIs40" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="PauseStatus"></param>
        /// <returns></returns>
        public static bool IsExitPause(string Barcode)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string strSQL;
            try
            {
                strSQL = "select * from BC_PauseTask where BarCodeFull='{0}' and IsDel='false' and IsParent='false'";
                strSQL = string.Format(strSQL, Barcode);
                return DBI.Execute(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("IsPauseExit" + e.Message.ToString());
            }
        }

    }
}