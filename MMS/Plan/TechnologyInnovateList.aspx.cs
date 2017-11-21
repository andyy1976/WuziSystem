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
    public partial class TechnologyInnovateList : System.Web.UI.Page
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
                    InitTable.Columns.Add("Submit_Type");
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
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                GridSource = Common.AddTableRowsID(GetTechnologyTestList());
                this.ViewState["lastSelectDeptCode"] = "";
                this.ViewState["lastSelectAccount"] = "";
                btnNewAdd.Attributes["onclick"] = "return ShowTechnologyInnovateAdd('')";
            }
        }
        protected DataTable GetTechnologyTestList()
        {
            try
            {
                string strSQL = "";
                string UserName = Session["UserName"].ToString();
                if (UserName == "Admin" || UserName == "admin")
                    strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,* from V_M_Demand_Plan_List where Submit_Type=2";//1－工艺试验件；2－技术创新课题；3-生产备料
                else
                    strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,* from V_M_Demand_Plan_List where Submit_Type=2 and UserAccount='" + UserName + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取技术创新课题清单信息出错" + ex.Message.ToString());
            }
        }

        protected void RadGrid_TechnologyInnovateList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_TechnologyInnovateList.DataSource = GridSource;
        }
        protected void RadGrid_TechnologyInnovateList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //DataTable table = GridSource;
            //GridDataItem dataitem = e.Item as GridDataItem;
            //if (e.CommandName == "Submit")
            //{
            //    string ID = dataitem.GetDataKeyValue("ID").ToString();
            //    Response.Redirect("~/Plan/TechnologyInnovateAdd.aspx?MDPId=" + ID);
            //}
        }
        protected void RadGrid_TechnologyInnovateList_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                DataTable table = GridSource;
                string MDP_Code = table.Rows[e.Item.DataSetIndex]["MDP_Code"].ToString();
                string MDPID = table.Rows[e.Item.DataSetIndex]["ID"].ToString();
                string Submit_State = table.Rows[e.Item.DataSetIndex]["Submit_State"].ToString();
                string SubmitType = "2";//1－技术创新课题；2－技术创新
                //RadButton btnDetails = e.Item.FindControl("RadButtonDetails") as RadButton;
                //btnDetails.Attributes["onclick"] = "return ShowTechnologyTestListDetails('" + MDP_Code + "','" + MDPID + "','" + SubmitType + "')";
                RadButton btnDetails = e.Item.FindControl("RadButtonDetails") as RadButton;
                btnDetails.Attributes["onclick"] = "return ShowTechnologyTestListDetails('" + MDP_Code + "','" + MDPID + "','" + SubmitType + "','1')";
                RadButton btnSubmit = e.Item.FindControl("RadBtnSubmit") as RadButton;
                btnSubmit.Attributes["onclick"] = "return ShowTechnologyInnovateAdd('" + MDPID + "')";
                if (Submit_State == "False")
                {
                    btnDetails.Visible = false;
                    btnSubmit.Visible = true;
                }
                else
                {
                    btnDetails.Visible = true;
                    btnSubmit.Visible = false;
                }
            }
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyInnovateList.ExportSettings.FileName = "技术创新课题物资需求计划" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyInnovateList.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyInnovateList.ExportSettings.FileName = "技术创新课题物资需求计划" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyInnovateList.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyInnovateList.ExportSettings.FileName = "技术创新课题物资需求计划" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyInnovateList.ExportSettings.IgnorePaging = true;
            RadGrid_TechnologyInnovateList.MasterTableView.ExportToPdf();
            RadGrid_TechnologyInnovateList.ExportSettings.IgnorePaging = false;
        }
    }
}