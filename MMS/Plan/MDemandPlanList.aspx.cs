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
    public partial class MDemandPlanList : System.Web.UI.Page
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
                    InitTable.Columns.Add("Correspond_Draft_Code");
                    InitTable.Columns.Add("Drawing_No");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("TaskId");
                    InitTable.Columns.Add("DraftId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("TechnicsLine");
                    InitTable.Columns.Add("ItemCode1");
                    InitTable.Columns.Add("NumCasesSum");
                    InitTable.Columns.Add("DemandNumSum");
                    InitTable.Columns.Add("Mat_Unit");
                    InitTable.Columns.Add("Quantity");
                    InitTable.Columns.Add("Rough_Size");
                    InitTable.Columns.Add("Dinge_Size");
                    InitTable.Columns.Add("MaterialsDes");
                    InitTable.Columns.Add("Special_Needs");
                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Stage");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Unit_Price");
                    InitTable.Columns.Add("Sum_Price");
                    InitTable.Columns.Add("Is_Submit");
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
                string MDP_Code = "";
                if ((Request.QueryString["MDP_Code"] != null && Request.QueryString["MDP_Code"].ToString() != "")
                    && (Request.QueryString["MDPID"] != null && Request.QueryString["MDPID"].ToString() != "")&&
                    Session["UserId"]!=null)
                {
                    MDP_Code = Request.QueryString["MDP_Code"].ToString();
                    GetMDemandPlan(MDP_Code);
                    GridSource = Common.AddTableRowsID(GetMDemandPlanList(Request.QueryString["MDPID"].ToString()));
                    GridSource1 = Common.AddTableRowsID(GetChangeRecord(Request.QueryString["MDPID"].ToString()));
                    this.ViewState["MDP_Code"] = MDP_Code;
                }
                else {
                    Response.Redirect("~/Plan/MDemandPlan.aspx");
                }
                this.ViewState["lastSelectDeptCode"] = "";
                this.ViewState["lastSelectAccount"] = "";
            }
        }
        protected void GetMDemandPlan(string MDP_Code)
        {
            try
            {
                string strSQL = "select * from V_M_Demand_Plan_List where MDP_Code='" + MDP_Code + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count > 0)
                {
                    this.span_xqjhCode.InnerText = MDP_Code;
                    this.span_model.InnerText = dt.Rows[0][0].ToString();
                    this.span_listNo.InnerText = dt.Rows[0][1].ToString();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求计划清单列表信息出错" + ex.Message.ToString());
            }
        }
        protected DataTable GetMDemandPlanList(string MDPID)
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string stocked = "0";//备料数量，从接口中获得，未完，等待接口
                string strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,('" + stocked + "'+'/'+Convert(nvarchar(10),Quantity))as schedule,* from V_M_Demand_Merge_List where MDPID='" + MDPID + "'";// MDP_Code='" + MDP_Code + "'";
                //string strSQL = @"exec Proc_Build_M_Demand_Plan " + MDPID + "," + userid ;
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求计划清单列表信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_MDemandPlanList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MDemandPlanList.DataSource = GridSource;
        }

     
        private DataTable GridSource1
        {
            get
            {
                Object obj = this.ViewState["_gds1"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                else
                {
                    DataTable InitTable = new DataTable();
                    InitTable.Columns.Add("RowsId");
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("Change_Code");
                    InitTable.Columns.Add("Change_Evidence_Id");
                    InitTable.Columns.Add("Correspond_Draft_Code");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("Change_Date");
                    InitTable.Columns.Add("Change_State");
                    InitTable.Columns.Add("MReduce_Num");
                    InitTable.Columns.Add("User_ID");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["ID"] };       //设置RowsId列为主键，用于datatable删除
                    this.ViewState["_gds1"] = InitTable;
                    return InitTable;
                }
            }
            set
            {
                this.ViewState["_gds1"] = value;
                ((DataTable)this.ViewState["_gds1"]).PrimaryKey = new DataColumn[] { ((DataTable)this.ViewState["_gds1"]).Columns["ID"] };
            }
        }
        protected void RadGrid_ChangeRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_ChangeRecord.DataSource = GridSource1;
        }
        protected DataTable GetChangeRecord(string MDPID)
        {
            try
            {
                string strSQL = @"select * from V_M_Change_Record where MDPID='" + MDPID + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count > 0)
                {
                    this.divListTitle.Visible = true;
                    this.divListContent.Visible = true;
                }
                else
                {
                    this.divListTitle.Visible = false;
                    this.divListContent.Visible = false;
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("获取数据出错" + ex.Message.ToString());
            }
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandPlanList.ExportSettings.FileName = "物资需求计划清单" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandPlanList.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandPlanList.ExportSettings.FileName = "物资需求计划清单" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandPlanList.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandPlanList.ExportSettings.FileName = "物资需求计划清单" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandPlanList.ExportSettings.IgnorePaging = true;
            RadGrid_MDemandPlanList.MasterTableView.ExportToPdf();
            RadGrid_MDemandPlanList.ExportSettings.IgnorePaging = false;
        }
    }
}