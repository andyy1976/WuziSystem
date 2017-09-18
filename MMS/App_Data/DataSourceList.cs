using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Camc.Web.Library;

namespace mms
{
    public class DataSourceList
    {
        /// <summary>
        /// 获取数据源的方法
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <param name="AppSettings">连接字符串，均在WebConfig中进行设置</param>
        /// <param name="ErrInfo">错误信息</param>
        /// <returns>返回查询数据源</returns>
        public static DataTable GetDataSource(string SqlString,string AppSettings,string ErrInfo)
        {
            DataTable DataSource = new DataTable();
            string ConnectString = ConfigurationManager.AppSettings[AppSettings];
            DBInterface DBI = DBFactory.GetDBInterface(ConnectString);

            try 
            {
                DataSource = Common.AddTableRowsID(DBI.Execute(SqlString, true));//DBI.Execute(SqlString, true); 
                return DataSource;
            }
            catch(Exception err)
            {
                throw new Exception(ErrInfo + err.Message.ToString());
            }
        }

        /// <summary>
        /// 更新数据库的方法
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <param name="AppSettings">连接字符串，均在WebConfig中进行设置</param>
        /// <param name="ErrInfo">错误信息</param>
        /// <returns>返回查询数据源</returns>
        public static void UpdateDingzhiInfo(string SqlString, string AppSettings, string ErrInfo)
        {
            string ConnectString = ConfigurationManager.AppSettings[AppSettings];
            DBInterface DBI = DBFactory.GetDBInterface(ConnectString);

            try 
            {
                DBI.BeginTrans();
                DBI.Execute(SqlString);
                DBI.CommitTrans();
            }
            catch(Exception error)
            {
                DBI.RollbackTrans();
                throw new Exception(ErrInfo + error.Message.ToString());
            }
        }
    }
}