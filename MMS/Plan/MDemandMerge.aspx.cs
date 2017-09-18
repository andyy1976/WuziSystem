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
    public partial class MDemandMerge : System.Web.UI.Page
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
                    InitTable.Columns.Add("MergeList_Code");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("DraftId");
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
                int submit = 0;
                if (Request.QueryString["submit"] != null && Request.QueryString["submit"].ToString() != "")
                    submit = Convert.ToInt32(Request.QueryString["submit"].ToString());
                GridSource = Common.AddTableRowsID(GetMDemandMerge(submit));
                this.ViewState["submit"] = submit;
                this.ViewState["lastSelectDeptCode"] = "";
                this.ViewState["lastSelectAccount"] = "";
            }
        }

        protected DataTable GetMDemandMerge(int submit)
        {
            try
            {
                string strSQL;
                strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,* from V_M_Demand_Merge where Is_Submit=" + submit;
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单列表信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_MDemandMerge_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MDemandMerge.DataSource = GridSource;
        }
        protected void RadGrid_MDemandMerge_ItemCommand(object sender, GridCommandEventArgs e)
        {
            DataTable table = GridSource;
            GridDataItem dataitem = e.Item as GridDataItem;
            if (e.CommandName == "Detail")
            {
                string Is_Submit = table.Rows[e.Item.DataSetIndex]["Is_Submit"].ToString();
                string Model = table.Rows[e.Item.DataSetIndex]["Model"].ToString();
                string Draft_Code = table.Rows[e.Item.DataSetIndex]["Draft_Code"].ToString();
                string MDMId = table.Rows[e.Item.DataSetIndex]["ID"].ToString();
                string url = "";
                string MergeList_Code ="";
                string MDP_Code ="";
                if (Is_Submit == "False")
                {
                    MergeList_Code = table.Rows[e.Item.DataSetIndex]["MergeList_Code"].ToString();
                    url = "~/Plan/MDemandMergeList.aspx?MergeList_Code=" + MergeList_Code + "&DraftCode=" + Draft_Code + "&model=" + Model + "&MDMId=" + MDMId;
                }
                else {
                    MDP_Code = table.Rows[e.Item.DataSetIndex]["MDP_Code"].ToString();
                    url = "~/Plan/MDemandMergeList.aspx?MDP_Code=" + MDP_Code + "&DraftCode=" + Draft_Code + "&model=" + Model;
                }
                Response.Redirect(url);
            }

        }
        protected void RadGrid_MDemandMerge_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                //DataTable table = GridSource;
                //GridDataItem dataitem = e.Item as GridDataItem;
                //if (table.Rows[e.Item.DataSetIndex]["Is_Submit"].ToString() == "False")
                //{
                //    RadGrid_MDemandMerge.Columns[2].Visible = true;
                //    RadGrid_MDemandMerge.Columns[3].Visible = false;
                //}
                //else
                //{
                //    RadGrid_MDemandMerge.Columns[2].Visible = false;
                //    RadGrid_MDemandMerge.Columns[3].Visible = true;
                //}
            }
        }
    }
}