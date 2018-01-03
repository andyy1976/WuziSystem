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
using System.Xml;

namespace mms.Plan
{
    public partial class MChangeRecordQuery : System.Web.UI.Page
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
                Common.CheckPermission(Session["UserName"].ToString(), "MChangeRecordQuery", this.Page);

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
            string ID = RTB_ID.Text.Trim();
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
                strSQL += " and submit_type = '" + type + "'";
            }
            if (startSubmitDate != "")
            {
                strSQL += " and Submit_Date >= '" + startSubmitDate + "'";
            }
            if (endSubmitDate != "")
            {
                strSQL += " and Submit_Date < '" + Convert.ToDateTime(endSubmitDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
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
                strSQL += " and Urgency_Degre = '" + Urgency_Degre + "'";
            }
            if (ID != "")
            {
                strSQL += " and M_Demand_Merge_List.ID like '%" + ID + "%'";
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
            string strSQL = " select M_Demand_Merge_List.ID, TDM_Description,TaskCode, Drawing_No, ItemCode1, NumCasesSum, DemandNumSum, Dept, DemandDate, Submit_Date, Special_Needs, Secret_Level" +
                " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes , M_Demand_Merge_List.Shipping_Address" +
                " , Column_Changed, Original_Value, Changed_Value" +
                " , M_Demand_Merge_List.Material_Name" +
                " , (select Top 1 USER_RCO_HEADER_NO from WriteRcoOrder_SentHeader where MDPLId = Convert(nvarchar(50),M_Change_Record.MDPID) ) as USER_RCO_HEADER_NO" +
                " from M_Demand_Merge_List join M_Change_Record on M_Change_Record.MDMID = M_Demand_Merge_List.ID" +
                " join Sys_DeptEnum on Sys_DeptEnum.DeptCode = M_Demand_Merge_List.MaterialDept" +
                " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                //" left join GetCustInfo_T_ACCT_SITE on Convert(nvarchar(50),GetCustInfo_T_ACCT_SITE.LOCATION_ID) = M_Demand_Merge_List.Shipping_Address" +
                " where 1 = 1" + strWhere + " order by M_Change_Record.ID desc";
            DataTable dt = DBI.Execute(strSQL, true);
            this.ViewState["_gds"] = dt;
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["_gds"];
        }

        protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                XmlDocument column = new XmlDocument();

                column.Load(Server.MapPath(@"~\Plan\Column_Changed.xml"));

                string Column_Changed = (e.Item as GridDataItem)["Column_Changed"].Text;
                if (column.GetElementsByTagName(Column_Changed)[0].InnerText != null)
                {
                    (e.Item as GridDataItem)["Column_Changed"].Text = column.GetElementsByTagName(Column_Changed)[0].InnerText.ToString();
                }
            }
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资更改列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资更改列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物资更改列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}