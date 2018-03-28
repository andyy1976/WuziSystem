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
    public partial class TechnologyChange : System.Web.UI.Page
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
                    InitTable.Columns.Add("rownum");
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("MDP_Code");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("DraftId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("SecretLevel");
                    InitTable.Columns.Add("SpecialNeeds");
                    InitTable.Columns.Add("UrgencyDegre");
                    InitTable.Columns.Add("stage1");
                    InitTable.Columns.Add("UseDes");
                    InitTable.Columns.Add("Certification1");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("Drawing_No");
                    InitTable.Columns.Add("TaskCode");
                    InitTable.Columns.Add("MaterialDept");
                    InitTable.Columns.Add("ItemCode1");
                    InitTable.Columns.Add("DemandNumSum");
                    InitTable.Columns.Add("NumCasesSum");
                    InitTable.Columns.Add("Mat_Unit");
                    InitTable.Columns.Add("Quantity");
                    InitTable.Columns.Add("Rough_Size");
                    InitTable.Columns.Add("Dinge_Size");
                    InitTable.Columns.Add("Rough_Spec");
                    InitTable.Columns.Add("DemandDate");
                    InitTable.Columns.Add("Special_Needs");
                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Stage");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Unit_Price");
                    InitTable.Columns.Add("Sum_Price");
                    InitTable.Columns.Add("Submit_Type");
                    InitTable.Columns.Add("Is_Submit");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("Contact_Way");
                    InitTable.Columns.Add("Submit_Date");
                    InitTable.Columns.Add("Submit_Type");
                    InitTable.Columns.Add("Get_Quantity");
                    InitTable.Columns.Add("Return_State");
                    InitTable.Columns.Add("subtype");
                    InitTable.Columns.Add("reState");
                    InitTable.Columns.Add("substate");
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
        public string title = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                if ((Request.QueryString["MDP_Code"] != null && Request.QueryString["MDP_Code"].ToString() != "")
                    && (Request.QueryString["MDPID"] != null && Request.QueryString["MDPID"].ToString() != "")
                    && (Request.QueryString["SubmitType"] != null && Request.QueryString["SubmitType"].ToString() != ""))
                {
                    this.span_gysyjCode.InnerText = Request.QueryString["MDP_Code"].ToString();
                    GridSource = Common.AddTableRowsID(GetTechnologyTestList(Request.QueryString["MDPID"].ToString()));
                    this.ViewState["MDP_Code"] = Request.QueryString["MDP_Code"].ToString();
                    this.span_title.InnerHtml = title = Request.QueryString["SubmitType"].ToString() == "1" ? "工艺试验件－更新" : "技术创新课题－更新";
                    Page.Title = title;
                    
                }
                else
                {
                    Response.Redirect("~/Plan/TechnologyTestList.aspx");
                }
                this.ViewState["lastSelectDeptCode"] = "";
                this.ViewState["lastSelectAccount"] = "";
            }
        }
        protected DataTable GetTechnologyTestList(string MDPID)
        {
            try
            {
                string strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,* from V_M_Technology_Apply where MDPID='" + MDPID + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取" + title + "数据出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_TechnologyList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_TechnologyList.DataSource = GridSource;
        }
        protected void RadGrid_TechnologyList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            DataTable table = GridSource;
            GridDataItem dataitem = e.Item as GridDataItem;
            if (e.CommandName == "Update")
            {
                
            }
        }
    }
}