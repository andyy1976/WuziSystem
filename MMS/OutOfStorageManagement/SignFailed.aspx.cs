using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Camc.Web.Library;
using System.Configuration;
using Telerik.Web.UI;

namespace mms.OutOfStorageManagement
{
    public partial class SignFailed : System.Web.UI.Page
    {

        static string DBContractConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
            DBI = DBFactory.GetDBInterface(DBContractConn);
            if (!IsPostBack)
            {
                Session["GridSource"] = Common.AddTableRowsID(GetStockBill_Rec());
            }
        }

        public DataTable GetStockBill_Rec()
        {
            string strSql = " select stockbill_Rec.LastUpdate, stockbill_Rec.ErrorInfo, ReleaseStockBill_T_Item.*" +
                " from ReleaseStockBill_T_Item " +
                " join stockbill_T_Item on stockbill_T_Item.ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID" +
                " join stockbill_Rec on stockbill_Rec.userSysBillBid = stockbill_T_Item.userSysBillBid" +
                " where stockbill_T_Item.State = '0'";
            DataTable dt = DBI.Execute(strSql, true);
            return dt;
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = Session["GridSource"];
        }
		 protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "签收失败信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "签收失败信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "签收失败信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}