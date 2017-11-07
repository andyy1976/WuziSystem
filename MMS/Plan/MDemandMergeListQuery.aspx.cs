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
    public partial class MDemandMergeListQuery : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "MDemandMergeListQuery", this.Page);

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

                GetMDemandMergeList("");
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
            string State = RDDL_State.SelectedValue.ToString();
            string startDemandDate = RDP_DemandDateStart.SelectedDate.ToString();
            string endDemandDate = RDP_DemandDateEnd.SelectedDate.ToString();

            string strSQL = "";
            if (type != "")
            {
                strSQL += " and M_Demand_Merge_List.submit_type = '" + type + "'";
            }
            if (startSubmitDate != "")
            {
                strSQL += " and M_Demand_Merge_List.Submit_Date >= '" + startSubmitDate + "'";
            }
            if (endSubmitDate != "")
            {
                strSQL += " and Submit_Date < '" + Convert.ToDateTime(endSubmitDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (taskCode != "")
            {
                strSQL += " and M_Demand_Merge_List.TaskCode = '" + taskCode + "'";
            }
            if (drawing_No != "")
            {
                strSQL += " and Drawing_No = '" + drawing_No + "'";
            }
            if (itemCode != "")
            {
                strSQL += " and ItemCode1 = '" + itemCode + "'";
            }
            if (Urgency_Degre != "")
            {
                strSQL += " and Urgency_Degre = '" + Urgency_Degre + "'";
            }
            if (dept != "")
            {
                strSQL += " and DeptCode = '" + dept + "'";
            }
            if (secret_Level != "")
            {
                strSQL += " and Secret_Level = '" + secret_Level + "'";
            }
            if (startDemandDate != "")
            {
                strSQL += " and DemandDate >= '" + startDemandDate + "'";
            }
            if (endDemandDate != "")
            {
                strSQL += " and DemandDate < '" + Convert.ToDateTime(endDemandDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            GetMDemandMergeList(strSQL);
        }

        protected void GetMDemandMergeList(string strWhere)
        {
            string strSQL = " select M_Demand_Merge_List.ID, M_Demand_Merge_List.Material_Name,M_Demand_Merge_List.Project, M_Demand_Merge_List.TaskCode, Drawing_No, ItemCode1, NumCasesSum, DemandNumSum, Dept, DemandDate, M_Demand_Merge_List.Submit_Date, Special_Needs, Secret_Level" +
                " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, Use_Des as UseDes, Shipping_Address" + //, GetCustInfo_T_ACCT_SITE.ADDRESS as ADDRESS
                " , isnull((select top 1 Submission_Status from GetRqStatus_T_Item where USER_RQ_LINE_ID = M_Demand_Merge_List.ID order by SUBMITED_SYNC_STATUS desc),'已提交') as State" +
                " from M_Demand_Merge_List" +
                " join M_Demand_Plan_List on M_Demand_Plan_List.ID = M_Demand_Merge_List.MDPID" +
                " join Sys_DeptEnum on Sys_DeptEnum.DeptCode = M_Demand_Merge_List.MaterialDept" +
                " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                //" left join GetCustInfo_T_ACCT_SITE on Convert(nvarchar(50),GetCustInfo_T_ACCT_SITE.LOCATION_ID) = M_Demand_Merge_List.Shipping_Address" +
                " where M_Demand_Merge_List.Is_submit = 'true' and M_Demand_Plan_List.Submit_state = '4'" + strWhere + " order by Submit_Date desc, ID desc";
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
            RadGrid1.ExportSettings.FileName = "物资需求列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资需求列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资需求列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging =true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}