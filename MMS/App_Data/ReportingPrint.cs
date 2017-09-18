using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Camc.Web.Library;

namespace mms
{
    public class ReportingPrint
    {
        private static string DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();


        /// <summary>
        /// 为打印交接单准备数据源：通过主表ID获取所有订单信息
        /// </summary>
        /// <param name="mainTableID"></param>
        /// <returns></returns>
        public static List<JiaoJieDanModel.PartsOrder> Print_JJDList(string mainTableID)
        {
            DBInterface DBI = DBFactory.GetDBInterface(DBConn);
            List<JiaoJieDanModel.PartsOrder> ordersList = new List<JiaoJieDanModel.PartsOrder>();
            string strSQL;
            try
            {
                strSQL = "Select * From User_JiaoJieDan_Orders Where MainTableID = '" + mainTableID + "'";
                DataTable result = DBI.Execute(strSQL, true);
                if (result.Rows.Count > 0)
                {
                    foreach (DataRow dr in result.Rows)
                    {
                        JiaoJieDanModel.PartsOrder order = new JiaoJieDanModel.PartsOrder();
                        order.TaskNum = dr["TaskNum"].ToString();
                        order.ProductName = dr["ProductName"].ToString();
                        order.DrawingNum = dr["DrawingNum"].ToString();
                        order.UnitName = dr["UnitName"].ToString();
                        order.ProductAmount = string.IsNullOrEmpty(dr["ProductAmount"].ToString()) ? 0 : Convert.ToInt32(dr["ProductAmount"]);
                        order.Cast = string.IsNullOrEmpty(dr["Cast"].ToString()) ? 0 : Convert.ToDecimal(dr["Cast"]);
                        order.Remark = dr["Remark"].ToString();
                        ordersList.Add(order);
                    }
                }
                return ordersList;

            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message.ToString());
            }
        }

    }
}