using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Camc.Web.Library;
using System.Configuration;
using mms;
using Telerik.Web.UI;
using System.Drawing;

namespace mms.Plan
{
    public partial class GetRqStatus : System.Web.UI.Page
    {
        string DBContractConn ;
        DBInterface DBI ;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
            DBI = DBFactory.GetDBInterface(DBContractConn);

            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "GetRqStatus", this.Page);

                this.ViewState["GridSource"] = GetRqStatus_T_Item("");
            }
        }

        protected DataTable GetRqStatus_T_Item(string strWhere)
        {
            string strSQL = "select GetRqStatus_T_Item.*, M_Demand_Merge_List.Material_Name, M_Demand_Merge_List.NumCasesSum, M_Demand_Merge_List.DemandNumSum from GetRqStatus_T_Item left join M_Demand_Merge_List on M_Demand_Merge_List.ID =  GetRqStatus_T_Item.User_RQ_Line_ID where 1 = 1 " + strWhere + " order by SUBMITED_SYNC_STATUS desc";
            DataTable dt = DBI.Execute(strSQL, true);
            return dt;
        }

        protected void RB_Query_Click(object sender, EventArgs e)
        {
            string ID = RTB_ID.Text.Trim();

            string startConfirmDate = RDP_ConfirmDateStart.SelectedDate.ToString();
            string endConfirmDate = RDP_ConfirmDateEnd.SelectedDate.ToString();

            string Material_Name = RTB_Material_Name.Text.Trim();
            
            string State = RDDL_State.SelectedValue.ToString();
           
            string strSQL = "";

            if (startConfirmDate != "")
            {
                strSQL += " and Confirmation_Date >= '" + startConfirmDate + "'";
            }
            if (endConfirmDate != "")
            {
                strSQL += " and Confirmation_Date < '" + Convert.ToDateTime(endConfirmDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            if (Material_Name != "")
            {
                strSQL += " and M_Demand_Merge_List.Material_Name = '" + Material_Name + "'";
            }
           
            if (ID != "")
            {
                strSQL += " and GetRqStatus_T_Item.User_RQ_Line_ID like '%" + ID + "%'";
            }

            if (State != "")
            {
                strSQL += " and GetRqStatus_T_Item.SUBMISSION_STATUS = '" + State + "'";
            }

            this.ViewState["GridSource"] = GetRqStatus_T_Item(strSQL);
            RadGrid1.Rebind();

        }

     
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["GridSource"];
        }
        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "需求申请提交状态变更信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
           
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "需求申请提交状态变更信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "需求申请提交状态变更信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}