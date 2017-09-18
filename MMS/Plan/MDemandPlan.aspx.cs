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
    public partial class MDemandPlan : System.Web.UI.Page
    {
        private DataTable GridSource
        {
            get
            {
                Object obj = this.ViewState["_gds"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                else
                {
                    DataTable InitTable = new DataTable();
                    InitTable.Columns.Add("RowsId");
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("MDP_Code");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("DraftId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("Submit_Date");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["ID"] };
                    this.ViewState["_gds"] = InitTable;
                    return InitTable;
                }
            }
            set
            {
                this.ViewState["_gds"] = value;
                ((DataTable)this.ViewState["_gds"]).PrimaryKey = new DataColumn[] { ((DataTable)this.ViewState["_gds"]).Columns["ID"] };
            }
        }

        private static string DBConn;
        private DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                if (Request.QueryString["PlanCode"] != null && Request.QueryString["PlanCode"].ToString() != "")
                {
                    Session["PlanCode"] = Request.QueryString["PlanCode"].ToString();
                }
                else {
                    Session["PlanCode"] = null;
                }
                GridSource = Common.AddTableRowsID(GetMDemandPlanList());
                Session["MDPCode"] = null;
                this.ViewState["lastSelectDeptCode"] = "";
                this.ViewState["lastSelectAccount"] = "";

               // RadTabStrip1.Tabs[0].NavigateUrl = "MDemandDetails.aspx?PackId=" + Request.QueryString["PackId"].ToString();
                RadTabStrip1.Tabs[0].NavigateUrl = "MDemandDetailsTreeList.aspx?PackId=" + Request.QueryString["PackId"].ToString();
            }
        }

        protected DataTable GetMDemandPlanList()
        {
            try
            {
                string strSQL = "";
                string UserName = Session["UserName"].ToString();
                if (UserName == "Admin" || UserName == "admin")
                    strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,* from V_M_Demand_Plan_List where Submit_Type=0";
                else
                    strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,* from V_M_Demand_Plan_List where Submit_Type=0 and UserAccount='" + UserName + "'";
                if (Session["MDPCode"]!=null && Session["MDPCode"].ToString()!="")
                    strSQL = strSQL+" and MDP_Code='"+Session["MDPCode"].ToString() + "'";
                if (Session["PlanCode"] != null && Session["PlanCode"].ToString() != "")
                {
                    strSQL = strSQL + " and PlanCode='" + Session["PlanCode"].ToString() + "'";
                }
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求计划清单信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_MDemandPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MDemandPlan.DataSource = GridSource;
        }
        protected void RadGrid_MDemandPlan_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //DataTable table = GridSource;
            //GridDataItem dataitem = e.Item as GridDataItem;
            //if (e.CommandName == "Detail")
            //{
            //    string Model = table.Rows[e.Item.DataSetIndex]["Model"].ToString();
            //    string Draft_Code = table.Rows[e.Item.DataSetIndex]["Draft_Code"].ToString();
            //    string MDPId = table.Rows[e.Item.DataSetIndex]["ID"].ToString();
            //    string MDP_Code = table.Rows[e.Item.DataSetIndex]["MDP_Code"].ToString();
            //    string url = "~/Plan/MDemandPlanList.aspx?MDP_Code=" +
            //        MDP_Code + "&Draft_Code=" + Draft_Code + "&Model=" + Model + "&MDPId=" + MDPId;
            //    Response.Redirect(url);
            //}

        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandPlan.ExportSettings.FileName = "物资需求计划清单（提交版）" + DateTime.Now.ToString("yyyy-MM-dd");//导出物资需求清单详表
            RadGrid_MDemandPlan.MasterTableView.ExportToExcel();
        }
        protected void RadGrid_MDemandPlan_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                DataTable table = GridSource;
                string MDP_Code = table.Rows[e.Item.DataSetIndex]["MDP_Code"].ToString();
                string MDPID = table.Rows[e.Item.DataSetIndex]["ID"].ToString();
                RadButton btn = e.Item.FindControl("RadButtonDetails") as RadButton;
                btn.Attributes["onclick"] = "return ShowMDemandPlanDetails('" + MDP_Code + "','" + MDPID + "')";
            }
        }
    }
}