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

namespace mms.Plan
{
    public partial class MDemandMergeListChange : System.Web.UI.Page
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
                if (Request.QueryString["PackId"] != null && Request.QueryString["PackId"].ToString() != "")
                {
                    Common.CheckPermission(Session["UserName"].ToString(), "MDemandMergeListChange", this.Page);

                    string packId = Request.QueryString["PackId"].ToString();
                    string DraftCode = "";
                    string draftid = "";
                    string Model = "";
                    string PlanCode = "";
                    string strSQL = " Select * From V_M_Draft_List where packid='" + packId + "'";
                    DataTable dt = DBI.Execute(strSQL, true);

                    DraftCode = dt.Rows[0]["DraftCode"].ToString();
                    draftid = dt.Rows[0]["draftid"].ToString();
                    Model = dt.Rows[0]["model_1"].ToString();
                    PlanCode = dt.Rows[0]["PlanCode"].ToString();


                    this.span_DraftCode.InnerText = DraftCode;
                    this.span_model.InnerText = Model;
                    this.span_plancode.InnerText = PlanCode;
                    this.span_PlanName.InnerText = dt.Rows[0]["PlanName"].ToString();
                    if (Request.QueryString["fromPage"] == "1")
                    {
                        RadTabStrip1.Tabs[0].NavigateUrl = "import_MDemandDetails.aspx?PackId=" + packId;
                    }
                    else
                    {
                        RadTabStrip1.Tabs[0].NavigateUrl = "MDemandDetails.aspx?PackId=" + packId;
                    }
                    strSQL = "select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'";
                    dt = DBI.Execute(strSQL, true);
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
        }

        protected void RB_Query_Click(object sender, EventArgs e)
        {
            string startSubmitDate = RDP_SubmitDateStart.SelectedDate.ToString();
            string endSubmitDate = RDP_SubmitDateEnd.SelectedDate.ToString();
            string taskCode = RTB_Task.Text.Trim();
            string drawing_No = RTB_Drawing_No.Text.Trim();
            string itemCode = RTB_ItemCode.Text.Trim();
            string Urgency_Degre = RDDL_Urgency_Degre.SelectedValue.ToString();
            string dept = RDDL_Dept.SelectedValue.ToString();
            string secret_Level = RDDL_Secret_Level.SelectedValue.ToString();
            string startDemandDate = RDP_DemandDateStart.SelectedDate.ToString();
            string endDemandDate = RDP_DemandDateEnd.SelectedDate.ToString();

            string strSQL = "";
           
            if (startSubmitDate != "")
            {
                strSQL += " and SUBMISSION_DATE >= '" + startSubmitDate + "'";
            }
            if (endSubmitDate != "")
            {
                strSQL += " and SUBMISSION_DATE < '" + Convert.ToDateTime(endSubmitDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (taskCode != "")
            {
                strSQL += " and TaskCode = '" + taskCode + "'";
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
                strSQL += " and Urgenc_Degre = '" + Urgency_Degre + "'";
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
            RadGrid1.Rebind();
        }

        protected void GetMDemandMergeList(string strWhere)
        {
            string strSQL = " select M_Demand_Merge_List.ID, TaskCode, Drawing_No,TDM_Description, ItemCode1, NumCasesSum, DemandNumSum, Dept, DemandDate, Submit_Date,SUBMISSION_DATE" +
                " , Special_Needs, Secret_Level, Shipping_Address, Material_Name, Manufacturer" +
                " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes , Convert(float, NumCasesSum) as NumCasesSum1" +
                " , isnull((select top 1 Submission_Status from GetRqStatus_T_Item where USER_RQ_LINE_ID = M_Demand_Merge_List.ID order by SUBMITED_SYNC_STATUS desc),'已提交') as State" +
                " from M_Demand_Merge_List" +
                " join Sys_DeptEnum on Sys_DeptEnum.DeptCode = M_Demand_Merge_List.MaterialDept" +
                " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                " where PackId = '" + Request.QueryString["PackId"].ToString() + "' and Is_Submit = 'true' " + strWhere + " order by SUBMISSION_DATE desc";
            DataTable dt = DBI.Execute(strSQL, true);
            this.ViewState["_gds"] = dt;

           
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["_gds"];
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadButton RB_ShowWin = e.Item.FindControl("RB_ShowWin") as RadButton;
                if (RB_ShowWin != null)
                {
                    string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                    RB_ShowWin.Attributes["onclick"] = string.Format("return ShowWin({0})", id);
                }
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                GetMDemandMergeList("");
                RadGrid1.Rebind();
            }
            else
            {
                throw new Exception("刷新页面出错，请按F5刷新！");
            }
        }
		protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资需求-需求变更列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资需求-需求变更列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资需求-需求变更列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}