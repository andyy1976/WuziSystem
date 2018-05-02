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
using mms;

namespace mms.Plan
{
	public partial class ShowP_Pack_Task : System.Web.UI.Page
	{
        static string DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
        DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"]==null||Session["UserId"] == null) 
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
            }
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "ShowP_Pack_Task", this.Page); 

                string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
                this.ViewState["GridSource"] = Common.AddTableRowsID(GetP_Pack_Task());

                string strSQL = " select PlanCode, State , PlanName, Remark , isnull((select Model from Sys_Model where Convert(nvarchar(50),ID) = P_Pack.Model), P_Pack.Model) as Model from P_Pack where PackID = '" + PackID + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                lblPlanCode.Text = dt.Rows[0]["PlanCode"].ToString();
                lblModel.Text = dt.Rows[0]["Model"].ToString();
                HFState.Value = dt.Rows[0]["State"].ToString();
                if (dt.Rows[0]["State"].ToString() == "2")
                {
                    table1.Visible = true;
                    lblPlanName.Text = dt.Rows[0]["PlanName"].ToString();
                    lblRemark.Text = dt.Rows[0]["Remark"].ToString();
                    RTB_PlanName.Visible = false;
                    RTB_Remark.Visible = false;
                }
                else
                {
                    RB_Add.Visible = true;
                    RTB_PlanName.Text = dt.Rows[0]["PlanName"].ToString();
                    RTB_Remark.Text = dt.Rows[0]["Remark"].ToString();
                }
            }
        }

        protected DataTable GetP_Pack_Task()
        {
            string PaskID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            string strWhere = "";
            if (Session["PTWhere"] != null) { strWhere = Session["PTWhere"].ToString(); }
            string strSQL = " select * , case when Stage = '1' then 'M' when Stage = '2' then 'C' when Stage = '3' then 'S' else 'D' end as Stage1 from P_Pack_Task where IsDel = 'false' and PackId = '" + PaskID + "' " + strWhere + " order by TaskCode";
            DataTable dt = new DataTable();
            try
            {
                 dt = DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            { 
                RadNotificationAlert.Text = "获取计划包列表失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }
            return dt;
        }

        protected void RadGridP_Pack_Task_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridP_Pack_Task.DataSource = this.ViewState["GridSource"];
        }

        protected void RadGridP_Pack_Task_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TaskID"].ToString();
                string IsSpread = (this.ViewState["GridSource"] as DataTable).Select("TaskID='" + ID + "'")[0]["IsSpread"].ToString().ToLower();
                RadioButtonList RBL = e.Item.FindControl("RBL_IsSpread") as RadioButtonList;
                if (RBL != null)
                {
                    RBL.CssClass = ID.ToString();
                    if (HFState.Value == "2")
                    {
                        RBL.Visible = false;
                        Label lbl = e.Item.FindControl("lbl_IsSpread") as Label;
                        lbl.Text = (IsSpread == "true" ? "是" : "否");
                    }
                    else
                    {
                        if (IsSpread != "" && IsSpread != null)
                        {
                            RBL.SelectedValue = IsSpread.ToLower();
                        }
                    }
                }
            }
            if (e.Item is GridCommandItem)
            {
                if (HFState.Value == "2")
                {
                    RadioButtonList RBL = e.Item.FindControl("RBL_IsSpreadAll") as RadioButtonList;
                    RBL.Visible = false;
                    Label lbl = e.Item.FindControl("lbl") as Label;
                    lbl.Visible = false;
                }
                System.Web.UI.WebControls.Label hfield = e.Item.FindControl("HiddenField") as System.Web.UI.WebControls.Label;
                if (Request.QueryString["type"].ToString() == "1")
                {
                    hfield.Text = "企业备料计划任务列表";

                }
                else
                {
                    hfield.Text = "型号投产计划任务列表";
                }
            }
        }

        protected void RBL_IsSpread_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = sender as RadioButtonList;
            string TaskID = rbl.CssClass;
            string IsSpread = rbl.SelectedValue;
            if (TaskID != "")
            {
                string strSQL = " Update P_Pack_Task set IsSpread = '" + IsSpread + "' where TaskID = '" + TaskID + "'";
                try
                {
                    DBI.Execute(strSQL);
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "修改展开状态失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
        }

        protected void RB_Query_Click(object sender, EventArgs e)
        {
            string ProductName = RTB_ProductName.Text.Trim();
            string DrawingNum = RTB_DrawingNum.Text.Trim();
            string TaskNum = RTB_TaskNum.Text.Trim();
            DateTime? StartDate = RDP_StartDate.SelectedDate;
            DateTime? EndDate = RDP_EndDate.SelectedDate;

            string strWhere = "";
            if (ProductName != "")
            {
                strWhere += " and ProductName like '%" + ProductName + "%'";
            }
            if (DrawingNum != "")
            {
                strWhere += " and TaskDrawingCode like '%" + DrawingNum + "%'";
            }
            if (TaskNum != "")
            {
                strWhere += " and TaskCode like '%" + TaskNum + "%'";
            }
            if (StartDate != null)
            {
                strWhere += " and PlanFinishTime >= '" + Convert.ToDateTime(StartDate).ToString("yyyy-MM-dd") + "'";
            }
            if (EndDate != null)
            {
                strWhere += " and PlanFinishTime <= '" + Convert.ToDateTime(EndDate).ToString("yyyy-MM-dd") + "'";

            }
            Session["PTWhere"] = strWhere;
            this.ViewState["GridSource"] = GetP_Pack_Task();
            RadGridP_Pack_Task.Rebind();

        }

        protected void RB_Add_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RadGridP_Pack_Task.Items.Count; i++)
            {
                RadioButtonList RBL = RadGridP_Pack_Task.Items[i].FindControl("RBL_IsSpread") as RadioButtonList;
                if (RBL != null)
                {
                    if (RBL.SelectedValue == "" || RBL.SelectedValue == null)
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行没有选择是否展开";
                        RadNotificationAlert.Show();
                        return;
                    }
                }
            }

            string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            try
            {
                string strSQL = " Update P_Pack set State = '2' where PackID = '" + PackID + "'";
                DBI.Execute(strSQL);
                RadNotificationAlert.Text = "归档成功！";
                RadNotificationAlert.Show();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }
        }

        protected void RBL_IsSpreadAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (sender) as RadioButtonList;
            string value = rbl.SelectedItem.Value;
            for (int i = 0; i < RadGridP_Pack_Task.Items.Count; i++)
            {
                RadioButtonList rbl1 = RadGridP_Pack_Task.Items[i].FindControl("RBL_IsSpread") as RadioButtonList;
                if (value == "") { rbl1.SelectedIndex = -1; }
                else { rbl1.SelectedValue = value; }
            }
        }
	}
}