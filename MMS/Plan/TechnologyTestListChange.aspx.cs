using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Collections;
using Camc.Web.Library;
using System.Configuration;

namespace mms.Plan
{
    public partial class TechnologyTestListChange : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                if (Request.QueryString["t"] != null && Request.QueryString["t"].ToString() != "")
                {
                    string title = "";
                    switch (Request.QueryString["t"].ToString())
                    {
                        case "1":
                            //Common.CheckPermission(Session["UserName"].ToString(), "TechnologyTestListChange1", this.Page);
                            title = "新增工艺试验件";
                            HiddenField.Value = "物资需求-->工艺试验件任务";  
                            break;

                        case "2":
                            //Common.CheckPermission(Session["UserName"].ToString(), "TechnologyTestListChange2", this.Page);
                            title = "新增技术创新课题";
                            HiddenField.Value = "物资需求-->技术创新课题任务";
                            break;
                        case "3":
                            //Common.CheckPermission(Session["UserName"].ToString(), "TechnologyTestListChange3", this.Page);
                            title = "新增生产备料";
                            HiddenField.Value = "物资需求-->生产备料任务";
                            break;
                    }
                }
                RadTabStrip1.Tabs[0].NavigateUrl = "TechnologyTestList.aspx?t=" + Request.QueryString["t"].ToString();

                GetMDemandMergeList("");
            }
        }

        protected void GetMDemandMergeList(string strWhere)
        {
            string strSQL = " select M_Demand_Merge_List.ID, M_Demand_Merge_List.TaskCode, Drawing_No, ItemCode1, NumCasesSum, DemandNumSum, Dept, DemandDate, M_Demand_Merge_List.Submit_Date" +
                " , Special_Needs, Secret_Level, Shipping_Address, Material_Name, Manufacturer" +
                " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes , Convert(float, NumCasesSum) as NumCasesSum1" +
                " , isnull((select top 1 Submission_Status from GetRqStatus_T_Item where USER_RQ_LINE_ID = M_Demand_Merge_List.ID order by SUBMITED_SYNC_STATUS desc),'已提交') as State" +
                " from M_Demand_Merge_List join M_Demand_Plan_List on M_Demand_Plan_List.ID = M_Demand_Merge_List.MDPID" +
                " join Sys_DeptEnum on Sys_DeptEnum.DeptCode = M_Demand_Merge_List.MaterialDept" +
                " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                " where M_Demand_Plan_List.Submit_Type = '" + Request.QueryString["t"].ToString() + "' and Submit_State = '4' and Is_Submit = 'true' " + strWhere + " order by M_Demand_Plan_List.ID desc, M_Demand_Merge_List.ID desc";
            DataTable dt = DBI.Execute(strSQL, true);
            this.ViewState["_gds"] = dt;

        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["_gds"];
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                Label lbltop = e.Item.FindControl("lbltop") as Label;
                if (lbltop != null)
                {
                    switch (Request.QueryString["t"].ToString())
                    {
                        case "1":
                            lbltop.Text = "工艺试验件任务";
                            break;

                        case "2":
                            lbltop.Text = "技术创新课题任务";
                            break;
                        case "3":
                            lbltop.Text = "生产备料任务";
                            break;
                    }
                }
            }
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
            RadGrid1.ExportSettings.FileName = "天津公司车间备料物资需求变更列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "天津公司车间备料物资需求变更列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "天津公司车间备料物资需求变更列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}