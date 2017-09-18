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
    public partial class MDemandMergeListChange1 : System.Web.UI.Page
    {
            string DBConn;
            DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                if (Session["UserName"] == null)
                {
                    Response.Redirect("/Default.aspx");
                }
                Common.CheckPermission(Session["UserName"].ToString(), "MDemandMergeListChange", this.Page); 
                string packId = Request.QueryString["PackId"].ToString();
                RadTabStrip1.Tabs[0].NavigateUrl = "MDemandDetails.aspx?PackId=" + packId;

                Session["GridSource"] = GetMDemandMergeList("");
            }
        }

        protected DataTable GetMDemandMergeList(string strWhere)
        {
            DataTable dt = new DataTable();

            string strSQL = " select M_Demand_Merge_List.*, Dept, Convert(nvarchar(50),Submit_Date, 111) as Submit_Date1" +
                ", (select top 1 Submission_Status from GetRqStatus_T_Item where User_RQ_Line_ID = M_Demand_Merge_List.ID order by DATA_LAST_UPDATE_DATE desc) as State" +
                " from M_Demand_Merge_List" +
                " join Sys_DeptEnum on Sys_DeptEnum.DeptCode = M_Demand_Merge_List.MaterialDept" +
                " where PackId = '" + Request.QueryString["PackId"].ToString() + "' " + strWhere + " order by Submit_Date desc";
            dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
            return dt;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = Session["GridSource"];
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                var datarow = (Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0];
                //配送地址
                RadDropDownList RDDL_Shipping_Address = e.Item.FindControl("RDDL_Shipping_Address") as RadDropDownList;
                if (RDDL_Shipping_Address != null)
                {
                    string dept = datarow["MaterialDept"].ToString();
                    string strSQL = " select '' as KeyWord , '' as keyWordCode union select KeyWord , keyWordCode from Sys_Dict where TypeID = '2' " +
                        " and  '2-' + Convert(nvarchar(50), KeyWordCode) in (select Shipping_Addr_ID from Sys_Dept_ShipAddr where Dept_Id= (select ID from Sys_DeptEnum where DeptCode='53')) order by KeyWord";
                    DataTable dt = DBI.Execute(strSQL, true);
                    RDDL_Shipping_Address.DataSource = dt;
                    RDDL_Shipping_Address.DataTextField = "KeyWord";
                    RDDL_Shipping_Address.DataValueField = "KeyWordCode";
                    RDDL_Shipping_Address.DataBind();
                    if (RDDL_Shipping_Address.FindItemByText(datarow["Shipping_Address"].ToString()) != null)
                    {
                        RDDL_Shipping_Address.FindItemByText(datarow["Shipping_Address"].ToString()).Selected = true;
                    }
                }
                //需求时间
                RadDatePicker RDP_DemandDate = e.Item.FindControl("RDP_DemandDate") as RadDatePicker;
                if (RDP_DemandDate != null)
                {
                    RDP_DemandDate.SelectedDate = Convert.ToDateTime(datarow["DemandDate"]);
                }

                //特殊需求
                RadTextBox RTB_Special_Needs = e.Item.FindControl("RTB_Special_Needs") as RadTextBox;
                if (RTB_Special_Needs != null)
                {
                    RTB_Special_Needs.Text = datarow["Special_Needs"].ToString();
                }
                //紧急程度
                RadDropDownList RDDL_Urgency_Degre = e.Item.FindControl("RDDL_Urgency_Degre") as RadDropDownList;
                if (RDDL_Urgency_Degre != null)
                {
                    RDDL_Urgency_Degre.SelectedValue = datarow["Urgency_Degre"].ToString();
                }
                //密级
                RadDropDownList RDDL_Secret_Level = e.Item.FindControl("RDDL_Secret_Level") as RadDropDownList;
                if (RDDL_Secret_Level != null)
                {
                    RDDL_Secret_Level.SelectedValue = datarow["Secret_Level"].ToString();
                }
                //用途
                RadDropDownList RDDL_Use_Des = e.Item.FindControl("RDDL_Use_Des") as RadDropDownList;
                if (RDDL_Use_Des != null)
                {
                    RDDL_Use_Des.SelectedValue = datarow["Use_Des"].ToString(); 
                }
                //生产厂家
                RadTextBox RTB_Manufacturer = e.Item.FindControl("RTB_Manufacturer") as RadTextBox;
                if (RTB_Manufacturer != null)
                {
                    RTB_Manufacturer.Text = datarow["Manufacturer"].ToString();
                }
            }
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Change")
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                var datarow = (Session["GridSource"] as DataTable).Select("ID='" + id + "'")[0];
              
                RadDropDownList RDDL_Shipping_Address = e.Item.FindControl("RDDL_Shipping_Address") as RadDropDownList;
                RadDatePicker RDP_DemandDate = e.Item.FindControl("RDP_DemandDate") as RadDatePicker;
                RadTextBox RTB_Special_Needs = e.Item.FindControl("RTB_Special_Needs") as RadTextBox;
                RadDropDownList RDDL_Urgency_Degre = e.Item.FindControl("RDDL_Urgency_Degre") as RadDropDownList;
                RadDropDownList RDDL_Secret_Level = e.Item.FindControl("RDDL_Secret_Level") as RadDropDownList;
                RadDropDownList RDDL_Use_Des = e.Item.FindControl("RDDL_Use_Des") as RadDropDownList;
                RadTextBox RTB_Manufacturer = e.Item.FindControl("RTB_Manufacturer") as RadTextBox;

                string shippingAddress = RDDL_Shipping_Address == null ? "" : RDDL_Shipping_Address.SelectedText.ToString();
                string DemandDate = RDP_DemandDate == null ? "" : RDP_DemandDate.SelectedDate.ToString();
                string Special_Needs = RTB_Special_Needs == null ? "" : RTB_Special_Needs.Text.Trim();
                string Urgency_Degre = RDDL_Urgency_Degre == null ? "" : RDDL_Urgency_Degre.SelectedValue.ToString();
                string Secret_Level = RDDL_Secret_Level == null ? "" : RDDL_Secret_Level.SelectedText.ToString();
                string use_Des = RDDL_Use_Des == null ? "" : RDDL_Use_Des.SelectedValue.ToString();
                string manufcturer = RTB_Manufacturer == null ? "" : RTB_Manufacturer.Text.Trim();

                string shippingAddress1 = datarow["Shipping_Address"].ToString();
                string DemandDate1 = datarow["DemandDate"].ToString();
                string Special_Needs1 = datarow["Special_Needs"].ToString();
                string Urgency_Degre1 = datarow["Urgency_Degre"].ToString();
                string Secret_Level1 = datarow["Secret_Level"].ToString();
                string use_Des1 = datarow["Use_Des"].ToString();
                string manufcturer1 = datarow["Manufacturer"].ToString();
            }
        }
    }
}