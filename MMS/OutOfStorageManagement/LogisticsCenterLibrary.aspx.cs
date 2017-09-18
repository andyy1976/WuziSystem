using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data;
using Camc.Web.Library;
using System.Configuration;
using Telerik.Web.UI;

namespace mms.OutOfStorageManagement
{
    public partial class LogisticsCenterLibrary : System.Web.UI.Page
    {
        static string DBContractConn ;
        DBInterface DBI ;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
            DBI = DBFactory.GetDBInterface(DBContractConn);
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "LogisticsCenterLibrary", this.Page);
                this.ViewState["GridSource"] = GetReleaseStockBill_T_Item("");
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["GridSource"];
        }

        protected DataTable GetReleaseStockBill_T_Item(string strWhere)
        {
            DataTable dt = new DataTable();
            string strSQL = " select  *, case when jc_jstype = 'A' then '实物签收+结算单据签收' when jc_jstype = 'B' then '实物签收' when  jc_jstype = 'C' then '结算单据签收' else jc_jstype end as jc_jstype1 from ReleaseStockBill_T_Item where 1= 1" + strWhere + " order by billdate desc, billno desc";
            dt = DBI.Execute(strSQL,true);
            return dt;
        }

        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string billno = RTB_billno.Text.Trim();
            string Startbilldate = RDP_Startbilldate.SelectedDate.ToString();
            string Endbilldate = RDP_Endbilldate.SelectedDate.ToString();
            string invcode = RTB_invcode.Text.Trim();
            string invname = RTB_invname.Text.Trim();
            string hgz_no = RTB_hgz_no.Text.Trim();
            string jc_rwh = RTB_jc_rwh.Text.Trim();
            string jc_th = RTB_jc_th.Text.Trim();
            string hgz_zjdbillno = RTB_hgz_zjdbillno.Text.Trim();
            string hgz_zydh = RTB_hgz_zydh.Text.Trim();

            string strWhere = "";
            if (billno != "")
            {
                strWhere += " and billno like '%" + billno + "%'";
            }
            if (Startbilldate != "" && Startbilldate != null)
            {
                strWhere += " and billdate >= '" + Convert.ToDateTime(Startbilldate).ToString("yyyy-MM-dd") + "'";
            }
            if (Endbilldate != "" && Endbilldate != null)
            {
                strWhere += " and billdate <= '" + Convert.ToDateTime(Endbilldate).ToString("yyyy-MM-dd") + "'";
            }
            if (invcode != "")
            {
                strWhere += " and invcode like '%" + invcode + "%'";
            }
            if (invname != "")
            {
                strWhere += " and invname like '%" + invname + "%'";
            }
            if (hgz_no != "")
            {
                strWhere += " and hgz_no like '%" + hgz_no + "%'";
            }
            if (jc_rwh != "")
            {
                strWhere += " and jc_rwh like '%" + jc_rwh + "%'";
            }
            if (jc_th != "")
            {
                strWhere += " and jc_th like '%" + jc_th + "%'";
            }
            if (hgz_zjdbillno != "")
            {
                strWhere += " and hgz_zjdbillno like '%" + hgz_zjdbillno + "%'";
            }
            if (hgz_zydh != "")
            {
                strWhere += " and hgz_zydh like '%" + hgz_zydh + "%'";
            }
            this.ViewState["GridSource"] = GetReleaseStockBill_T_Item(strWhere);
            RadGrid1.Rebind();
        }


        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}