using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Camc.Web.Library;
using System.Configuration;

namespace mms.Plan
{
    public partial class Exe_Inf : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "Exe_Inf", this.Page);

                string strSQL = "select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'";
                DataTable dt = DBI.Execute(strSQL, true);
                RDDL_Urgency_Degre.DataSource = dt;
                RDDL_Urgency_Degre.DataTextField = "DICT_Name";
                RDDL_Urgency_Degre.DataValueField = "DICT_CODE";
                RDDL_Urgency_Degre.DataBind();

                strSQL = "select Dept, DeptCode from Sys_DeptEnum where Is_Del = 'false'";
                dt = DBI.Execute(strSQL, true);
                RDDL_Dept.DataSource = dt;
                RDDL_Dept.DataTextField = "Dept";
                RDDL_Dept.DataValueField = "DeptCode";
                RDDL_Dept.DataBind();

                strSQL = "SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)";
                dt = DBI.Execute(strSQL, true);
                RDDL_Secret_Level.DataSource = dt;
                RDDL_Secret_Level.DataValueField = "SecretLevel_Name";
                RDDL_Secret_Level.DataTextField = "SecretLevel_Name";
                RDDL_Secret_Level.DataBind();

                GetExeInf("");
            }
        }

        protected void RB_Query_Click(object sender, EventArgs e)
        {
            string type = RDDL_Task.SelectedValue.ToString();
            string startSubmitDate = RDP_SubmitDateStart.SelectedDate.ToString();
            string endSubmitDate = RDP_SubmitDateEnd.SelectedDate.ToString();
            string taskCode = RTB_Task.Text.Trim();
            string drawing_No = RTB_Drawing_No.Text.Trim();
            string itemCode = RTB_ItemCode.Text.Trim();
            string Urgency_Degre = RDDL_Urgency_Degre.SelectedValue.ToString();
            string dept = RDDL_Dept.SelectedValue.ToString();
            string secret_Level = RDDL_Secret_Level.SelectedValue.ToString();
            string id = RTB_ID.Text.Trim();
            string startDemandDate = RDP_DemandDateStart.SelectedDate.ToString();
            string endDemandDate = RDP_DemandDateEnd.SelectedDate.ToString();

            string strSQL = "";

            if (type != "")
            {
                strSQL += " and M_Demand_Merge_List.submit_type = '" + type + "'";
            }
            if (id != "")
            {
                strSQL += " and GetExeInf_T_Item.USER_RQ_LINE_ID = '" + id + "'";
            }
            if (startSubmitDate != "")
            {
                strSQL += " and M_Demand_Merge_List.Submit_Date >= '" + startSubmitDate + "'";
            }
            if (endSubmitDate != "")
            {
                strSQL += " and M_Demand_Merge_List.Submit_Date < '" + Convert.ToDateTime(endSubmitDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (taskCode != "")
            {
                strSQL += " and M_Demand_Merge_List.TaskCode = '" + taskCode + "'";
            }
            if (drawing_No != "")
            {
                strSQL += " and M_Demand_Merge_List.Drawing_No = '" + drawing_No + "'";
            }
            if (itemCode != "")
            {
                strSQL += " and M_Demand_Merge_List.ItemCode1 = '" + itemCode + "'";
            }
            if (Urgency_Degre != "")
            {
                strSQL += " and M_Demand_Merge_List.Urgency_Degre = '" + Urgency_Degre + "'";
            }
            if (dept != "")
            {
                strSQL += " and M_Demand_Merge_List.MaterialDept = '" + dept + "'";
            }
            if (secret_Level != "")
            {
                strSQL += " and M_Demand_Merge_List.Secret_Level = '" + secret_Level + "'";
            }
            if (startDemandDate != "")
            {
                strSQL += " and M_Demand_Merge_List.DemandDate >= '" + startDemandDate + "'";
            }
            if (endDemandDate != "")
            {
                strSQL += " and M_Demand_Merge_List.DemandDate < '" + Convert.ToDateTime(endDemandDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            GetExeInf(strSQL);
        }

        protected void GetExeInf(string strWhere)
        {
            //string strSQL = " select a.ID, a.USER_RQ_LINE_ID, b.Total_Pr_Quantity,b.Total_Po_Quantity, b.Total_Received_Quantity, b.Total_Inspected_Quantity, b.Total_Delivered_Quantity" +
            //   " ,b.Total_Shipped_Quantity, b.Total_Borrowed_Quantity, b.TOTAL_RESERVED_QUANTITY, b.Rq_Execution_Status" +
            //   " ,b.Rq_Execution_Phase, b.Rq_Status, b.Close_Reason, b.Closed_By, b.Close_Date, b.Last_Update_Date" +
            //   " from GetExeInf_T_Item as a join (select * from GetExeInf_T_Item as a" +
            //   " where ID = (select top 1 ID  from GetExeInf_T_Item where User_RQ_Line_ID =a.User_RQ_Line_ID order by Last_Update_Date desc,ID desc)) as b" +
            //   " on b.ID = a.ID" +
            //   " where 1 = 1" + strWhere + " order by a.Last_Update_Date desc";
            string strSQL = " select GetExeInf_T_Item.* from GetExeInf_T_Item left join M_Demand_Merge_List on M_Demand_Merge_List.ID = GetExeInf_T_Item.User_Rq_Line_Id " +
                " where 1 = 1" + strWhere + " order by Last_Update_Date desc";
            DataTable dt = DBI.Execute(strSQL, true);
            this.ViewState["_gds"] = dt;
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["_gds"];
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资需求执行信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资需求执行信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资需求执行信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}