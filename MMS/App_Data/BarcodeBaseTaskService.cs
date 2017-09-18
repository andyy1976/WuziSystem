using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Camc.Web.Library;
using System.Configuration;
using System.Data;

namespace mms
{
    public class BarcodeBaseTaskService
    {
        private static string DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();

        /// <summary>
        /// 从北京签收任务并在天津本地处理
        /// </summary>
        /// <param name="TaskList"></param>
        /// <returns></returns>
        public static bool TaskManage(List<TaskDataBody> TaskList, string userAccount)
        {   
            string strSQL = string.Empty;
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                foreach (TaskDataBody taskItem in TaskList)
                {
                    BarcodeBaseTaskService service = new BarcodeBaseTaskService();
                    strSQL = "Select ID FROM [ProductBarCodeManagementAndTrack].[dbo].[Task] Where [TaskNum] = '" + taskItem.TaskNum + "' And [DrawingNum] = '" + taskItem.DrawingNum + "' And [Enable] = 1";
                    string taskID = DBI.GetSingleValue(strSQL);
                    switch (taskItem.DisposeType)
                    {
                        //新增任务
                        case "1":
                            if (!DBI.CheckExistData(service.CheckTaskIsExist(taskItem.TaskNum, taskItem.DrawingNum)))
                            {
                                strSQL = service.AddTask(taskItem);
                                string createTaskID = DBI.GetSingleValue(strSQL);
                                //插入日志
                                strSQL = service.CreateOprateLog(createTaskID, userAccount, "create", "操作成功");
                                DBI.Execute(strSQL);
                                break;
                            }
                            else
                            {
                                //strSQL = service.AddTaskSync(taskItem, "新增,本地存在该任务,请核对");
                                //DBI.Execute(strSQL);
                                return false;
                            }

                        //更改任务
                        case "2":
                            if (DBI.CheckExistData(service.CheckTaskIsExist(taskItem.TaskNum, taskItem.DrawingNum)))
                            {
                                //将原任务标记为废除
                                strSQL = service.UpdateTask_DropOldTask(taskItem, taskID);
                                DBI.Execute(strSQL);
                                //插入更新的任务记录并获取新任务的ID
                                strSQL = service.UpdateTask_CreateNewTask(taskItem, taskID);
                                string newTaskID = DBI.GetSingleValue(strSQL);
                                //更新原任务下条码对应的任务ID为新任务ID
                                strSQL = service.UpdateTask_UpdateBarcodeTaskID(newTaskID, taskID, taskItem);
                                DBI.Execute(strSQL);
                                //插入日志
                                strSQL = service.CreateOprateLog(newTaskID, userAccount, "update", "更新成功");
                                break;
                            }
                            else
                            {
                                //strSQL = service.AddTaskSync(taskItem, "更新,本地不存在该任务,请核对");
                                //DBI.Execute(strSQL);
                                //break;
                                DBI.RollbackTrans();
                                return false;
                            }

                        //删除任务
                        case "3":
                            if (DBI.CheckExistData(service.CheckTaskIsExist(taskItem.TaskNum, taskItem.DrawingNum)))
                            {
                                strSQL = service.DeleteTask(taskItem, taskID);
                                DBI.Execute(strSQL);
                                //插入日志
                                strSQL = service.CreateOprateLog(taskID, userAccount, "delete", "删除成功");
                                break;
                            }
                            else
                            {
                                strSQL = service.AddTaskSync(taskItem, "删除,本地不存在该任务,请核对");
                                DBI.Execute(strSQL);
                                break;
                            }
                        default:
                            DBI.RollbackTrans();
                            return false;
                    }
                }
                DBI.CommitTrans();
                return true;
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                return false;
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        /// <summary>
        /// 检查当前任务中是否存在相同任务
        /// </summary>
        /// <param name="taskNum">任务号</param>
        /// <param name="drawingNum">图号</param>
        /// <returns></returns>
        protected string CheckTaskIsExist(string taskNum, string drawingNum)
        {
            //DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            //bool exist = false;
            string strSQL;
            try
            {
                strSQL = "SELECT ID FROM [ProductBarCodeManagementAndTrack].[dbo].[Task] Where replace([TaskNum],CHAR(13)+char(10),'') = '" + taskNum.Trim().Trim('\r').Trim('\n') + "' And replace([DrawingNum],CHAR(13)+char(10),'') = '" + drawingNum.Trim().Trim('\r').Trim('\n') + "' And [Enable] = 1";
                //exist = DBI.CheckExistData(strSQL);
                return strSQL;
            }
            catch (Exception e)
            {
                throw new Exception("检查当前任务中是否存在相同任务时出现异常" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 通过任务号和图号查找本地任务并获取任务ID
        /// </summary>
        /// <param name="TaskNum">任务号</param>
        /// <param name="DrawingNum">图号</param>
        /// <returns></returns>
        protected string GetTaskID(string taskNum, string drawingNum)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = "Select ID FROM [ProductBarCodeManagementAndTrack].[dbo].[Task] Where [TaskNum] = '" + taskNum + "' And [DrawingNum] = '" + drawingNum + "'";
                return DBI.GetSingleValue(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 生成数据库操作SQL-创建任务
        /// </summary>
        /// <param name="taskBody"></param>
        protected string AddTask(TaskDataBody taskBody)
        {
            string strSQL;
            try
            {
                //数据库操作串
                strSQL = @"INSERT INTO [ProductBarCodeManagementAndTrack].[dbo].[Task]
                                       ([ProductModel]
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
                                       ,[Enable]
                                       ,[DeptCode]
                                       ,[RecId])
                                 VALUES
                                       ('" + taskBody.ProductModel + "','" + taskBody.TaskNum + "','" + taskBody.DrawingNum + "','" + taskBody.ProductName + "'" +
                                   ",'" + taskBody.PlanAmount + "', " + taskBody.MakeAmount + ", '" + taskBody.ProgressDate + "', " + taskBody.FinishAmount + "" +
                                   ",'" + taskBody.FinishDate + "', '" + taskBody.Material + "', '" + taskBody.Remark + "', '" + taskBody.CraftRoute + "'" +
                                   ", '" + taskBody.Enable + "','" + taskBody.DeptCode + "', '"+taskBody.RecId+"');Select SCOPE_IDENTITY()";
                return strSQL;
            }
            catch (Exception e)
            {
                return "生成数据库操作SQL-创建任务时出现异常,异常的任务号为:" + taskBody.TaskNum + "图号为：" + taskBody.DrawingNum;
            }
        }

        /// <summary>
        /// 生成数据库SQL-修改任务--标记原始任务为不可用
        /// </summary>
        protected string UpdateTask_DropOldTask(TaskDataBody taskBody, string taskID)
        {
            string strSQL;
            try
            {
                //标记修改的任务为不可用
                strSQL = "Update [ProductBarCodeManagementAndTrack].[dbo].[Task] Set [Enable] = '0' Where ID = '" + taskID + "' And [Enable] = '1'";
                return strSQL;
            }
            catch (Exception e)
            {
                return "生成生成数据库SQL-修改任务--标记原始任务为不可用时出现异常,异常任务号:" + taskBody.TaskNum + "图号:" + taskBody.DrawingNum;
            }
        }

        /// <summary>
        /// 生成数据库SQL-修改任务--创建新任务记录并返回新任务ID
        /// </summary>
        /// <param name="taskBody"></param>
        /// <returns></returns>
        protected string UpdateTask_CreateNewTask(TaskDataBody taskBody, string taskID)
        {
            string strSQL;
            try
            {
                //插入新任务并返回新的任务ID
                strSQL = @"INSERT INTO [ProductBarCodeManagementAndTrack].[dbo].[Task]
                                       ([ProductModel]
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
                                       ,[Enable]
                                       ,[DeptCode]
                                       ,[UpdatedOld])
                                 VALUES
                                       ('" + taskBody.ProductModel + "','" + taskBody.TaskNum + "','" + taskBody.DrawingNum + "','" + taskBody.ProductName + "'" +
                                       ",'" + taskBody.PlanAmount + "', " + taskBody.MakeAmount + ", '" + taskBody.ProgressDate + "', " + taskBody.FinishAmount + "" +
                                       ",'" + taskBody.FinishDate + "', '" + taskBody.Material + "', '" + taskBody.Remark + "', '" + taskBody.CraftRoute + "', '" + taskBody.Enable + "'" +
                                       ",'" + taskBody.DeptCode + "', '" + taskID + "');Select SCOPE_IDENTITY()";
                return strSQL;
            }
            catch (Exception e)
            {
                return "生成数据库SQL-修改任务--创建新任务记录并返回新任务ID时出现异常,异常任务号:" + taskBody.TaskNum + "图号:" + taskBody.DrawingNum;
            }
        }

        /// <summary>
        /// 生成数据库SQL-修改任务--更新原任务下的条码对应的任务ID
        /// </summary>
        /// <param name="taskBody"></param>
        /// <returns></returns>
        protected string UpdateTask_UpdateBarcodeTaskID(string newTaskID, string oldTaskID, TaskDataBody taskBody)
        {
            string strSQL;
            try
            {
                //将原任务上的条码挂到修改后的新任务上
                strSQL = "Update [ProductBarCodeManagementAndTrack].[dbo].[BarCode] Set TaskID = '" + newTaskID + "' Where TaskID = '" + oldTaskID + "'";
                return strSQL;
            }
            catch (Exception e)
            {
                return "生成数据库SQL-修改任务--更新原任务下的条码对应的任务ID时出现异常,异常任务号:" + taskBody.TaskNum + "图号:" + taskBody.DrawingNum;
            }
        }

        /// <summary>
        /// 生成数据库SQL-删除任务
        /// </summary>
        protected string DeleteTask(TaskDataBody taskBody, string taskID)
        {
            string strSQL;
            try
            {
                strSQL = "Update Task Set Enable = '0' Where ID = '" + taskID + "' And [Enable] = '1'";
                return strSQL;
            }
            catch (Exception e)
            {
                return "生成数据库SQL-删除任务时出现异常,异常任务号:" + taskBody.TaskNum + "图号:" + taskBody.DrawingNum;
            }
        }

        /// <summary>
        /// 生成数据库SQL-创建日志
        /// </summary>
        /// <param name="userAccount">操作人帐号</param>
        /// <param name="Type">操作类型（创建：create,更新:update,删除:delete）</param>
        /// <param name="result">操作结果</param>
        /// <param name="taskID">操作的任务ID</param>
        /// <returns></returns>
        protected string CreateOprateLog(string taskID, string userAccount, string operateType, string result)
        {
            string strSQL;
            try
            {
                strSQL = @"INSERT INTO [ProductBarCodeManagementAndTrack].[dbo].[TaskOperateLog]
                                   ([CreateDate]
                                   ,[CreatePerson],[TaskID],[Type],[Result])
                             VALUES
                                   ('" + DateTime.Now + "','" + userAccount + "','" + taskID + "' ,'" + operateType + "', '" + result + "')";
                return strSQL;
            }
            catch (Exception e)
            {
                return "生成数据库SQL-创建日志时出现异常";
            }
        }

        /// <summary>
        /// 生成数据库操作SQL-将签收有问题的任务插入到表TaskSync中
        /// </summary>
        /// <param name="taskBody"></param>
        protected string AddTaskSync(TaskDataBody taskBody, string sign)
        {
            string strSQL;
            try
            {
                //数据库操作串
                strSQL = @"INSERT INTO [ProductBarCodeManagementAndTrack].[dbo].[TaskSync]
                                       ([ProductModel]
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
                                       ,[Enable]
                                       ,[DeptCode]
                                       ,[RecId]
                                       ,[Sign])
                                 VALUES
                                       ('" + taskBody.ProductModel + "','" + taskBody.TaskNum + "','" + taskBody.DrawingNum + "','" + taskBody.ProductName + "'" +
                                   ",'" + taskBody.PlanAmount + "', " + taskBody.MakeAmount + ", '" + taskBody.ProgressDate + "', " + taskBody.FinishAmount + "" +
                                   ",'" + taskBody.FinishDate + "', '" + taskBody.Material + "', '" + taskBody.Remark + "', '" + taskBody.CraftRoute + "'" +
                                   ", '" + taskBody.Enable + "','" + taskBody.DeptCode + "', '" + taskBody.RecId + "', '" + sign + "')";
                return strSQL;
            }
            catch (Exception e)
            {
                return "生成数据库操作SQL-创建任务时出现异常,异常的任务号为:" + taskBody.TaskNum + "图号为：" + taskBody.DrawingNum;
            }
        }
    }
}