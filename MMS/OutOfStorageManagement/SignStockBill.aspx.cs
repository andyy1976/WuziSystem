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
using mms.WriteStockBillService;

namespace mms.OutOfStorageManagement
{
    public partial class SignStockBill : System.Web.UI.Page
    {
        static string DBContractConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
            DBI = DBFactory.GetDBInterface(DBContractConn);
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "SignStockBill", this.Page);
                this.ViewState["GridSource"] = GetReleaseStockBill_T_Item(" and (select count(*) from Stockbill_T_Item where ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID and state = '1') = 0");
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["GridSource"];
        }

        protected DataTable GetReleaseStockBill_T_Item(string strWhere)
        {
            DataTable dt = new DataTable();
            string strSQL = " select  *, case when jc_jstype = 'A' then '实物签收+结算单据签收' when jc_jstype = 'B' then '实物签收' when  jc_jstype = 'C' then '结算单据签收' else jc_jstype end as jc_jstype1" +
                " , case when (select count(*) from Stockbill_T_Item where ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID and state = '1') = 0 then 'false' else 'true' end as SignState" +
                " , 'false' as Sign" +
                " from ReleaseStockBill_T_Item where 1= 1" + strWhere + " order by billdate desc";
            dt = DBI.Execute(strSQL, true);
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
            string sign = RDDL_SignState.SelectedValue.ToString();

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
            if (sign != "0")
            {
                if (sign == "1")
                {
                    strWhere += " and (select count(*) from Stockbill_T_Item where ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID and state = '1')  = 0";
                }
                else
                {
                    strWhere += " and (select count(*) from Stockbill_T_Item where ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID and state = '1')  > 0";
                }
            }
            this.ViewState["GridSource"] = GetReleaseStockBill_T_Item(strWhere);
            RadGrid1.Rebind();
        }

        protected void RB_Sign_CheckedChanged(object sender, EventArgs e)
        {
            RadButton RB = sender as RadButton;
            string id = (RB.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            (this.ViewState["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["Sign"] = RB.Checked.ToString().ToLower();
        }

        protected void RB_AllSign_CheckedChanged(object sender, EventArgs e)
        {
            RadButton RB = sender as RadButton;
            
            foreach (var item in RadGrid1.Items)
            {
                if (item is GridDataItem)
                {
                    string id = (item as GridDataItem).GetDataKeyValue("ID").ToString();
                    ((item as GridDataItem).FindControl("RB_Sign") as RadButton).Checked = RB.Checked;
                    if ((this.ViewState["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["SignState"].ToString() == "false")
                    {
                        (this.ViewState["GridSource"] as DataTable).Select("ID='" + id + "'")[0]["Sign"] = RB.Checked.ToString().ToLower();
                    }

                }
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                DataRow datarow = (this.ViewState["GridSource"] as DataTable).Select("ID='" + id + "'")[0];
                RadButton RB_Sign = (e.Item as GridDataItem).FindControl("RB_Sign") as RadButton;
                bool signstate = Convert.ToBoolean(datarow["SignState"]);
                bool sign = Convert.ToBoolean(datarow["Sign"]);
                if (signstate == true)
                {
                    if (RB_Sign!=null)
                    {
                        RB_Sign.Checked = true;
                        RB_Sign.Enabled = false;
                    }
                }
                else {
                    if (RB_Sign!=null)
                    {
                        RB_Sign.Enabled = true;
                        if (sign == true)
                        {
                            RB_Sign.Checked = true;
                        }
                        else
                        {
                            RB_Sign.Checked = false;
                        }
                    }
                }
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Sign")
            {
                string idStr = "";
                DataRow[] datarows = (this.ViewState["GridSource"] as DataTable).Select("SignState='false' and Sign='true'");
                if (datarows.Length == 0)
                {
                    RadNotificationAlert.Text = "请选择签收行！";
                    RadNotificationAlert.Show();
                    return;
                }
                for (int i = 0; i < datarows.Length; i++)
                { 
                    string id = datarows[i]["ID"].ToString();                    
                    idStr += id + ",";
                }
               
                if (idStr != "")
                {
                    string ReleaseStockBill_T_ItemIDStr = idStr.Substring(0, idStr.Length - 1);
                    string customersyscode = "TJ_WZ";
                    string customersyspwd = "TJ_WZ";
                    string customersysip = "10.20.232.48";
                    string customersysport = "80";
                    string row_count = (idStr.Length - idStr.Replace(",", "").Length).ToString();
                    string SignUser = RTB_SignUser.Text.Trim();

                    string strSQL = "Exec Proc_Save_StockBill '" + ReleaseStockBill_T_ItemIDStr + "', '" +
                        customersyscode + "','" +
                        customersyspwd + "','" +
                        customersysip + "','" +
                        customersysport + "','" +
                        row_count + "','" +
                        SignUser + "'";

                    string sentID = DBI.GetSingleValue(strSQL);

                    LogisticsCenterBLL bll = new LogisticsCenterBLL();
                    string result = bll.WriteStockBill(Convert.ToInt32(sentID));
                    try
                    {
                        Convert.ToInt32(result);
                        if (result == "0")
                        {
                            RadNotificationAlert.Text = "签收成功！";
                            RadNotificationAlert.Show();
                        }
                        else
                        {
                            int scount = idStr.Length - idStr.Replace(",", "").Length - Convert.ToInt32(result);

                            RadNotificationAlert.Text = "签收成功：" + scount.ToString() + "条, 失败：" + result + "条";
                            RadNotificationAlert.Show();
                        }

                        this.ViewState["GridSource"] = GetReleaseStockBill_T_Item(" and (select count(*) from Stockbill_T_Item where ReleaseStockBill_T_ItemID = ReleaseStockBill_T_Item.ID) = 0");
                        RadGrid1.Rebind();
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "签收失败！<br />" + result;
                        RadNotificationAlert.Show();
                    }
                }
            }
        }
		 protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "签收出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "签收出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "签收出库单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}