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
    public partial class GetRcoStatus : System.Web.UI.Page
    {
        string DBContractConn;
        DBInterface DBI;
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
                Common.CheckPermission(Session["UserName"].ToString(), "GetRcoStatus", this.Page);
                this.ViewState["GridSource"] = GetRcoStatus_T_Item("");
            }
        }

        protected void RB_Query_Click(object sender, EventArgs e)
        {
            string RCO_No = RTB_RCO_No.Text.Trim();



            string Material_Name = RTB_Material_Name.Text.Trim();

      

            string strSQL = "";

   

            if (Material_Name != "")
            {
                strSQL += " and M_Demand_Merge_List.Material_Name = '" + Material_Name + "'";
            }

            if (RCO_No != "")
            {
                strSQL += " and GetRcoStatus_T_Item.RCO_No like '%" + RCO_No + "%'";
            }


            this.ViewState["GridSource"] = GetRcoStatus_T_Item(strSQL);
            RadGrid1.Rebind();

        }

        protected DataTable GetRcoStatus_T_Item(string strWhere)
        {
            string strSQL = "select GetRcoStatus_T_Item.*, Material_Name, M_Change_Record.Column_Changed,M_Change_Record.Original_Value,M_Change_Record.Changed_Value from GetRcoStatus_T_Item" +
                " left join WriteRcoOrder_SentLine on WriteRcoOrder_SentLine.ID = GetRcoStatus_T_Item.Attribute1" +
                " left join M_Change_Record on M_Change_Record.ID = WriteRcoOrder_SentLine.Group_ID " +
                " left join M_Demand_Merge_List on M_Demand_Merge_List.ID = M_Change_Record.MDMId" +
                " where 1 = 1 " + strWhere + " order by LAST_STATUS_CHANGE_DATE desc";
            DataTable dt = DBI.Execute(strSQL, true);
            return dt;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["GridSource"];
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            { 
                try
                {
                    XmlDocument column = new XmlDocument();

                    column.Load(Server.MapPath(@"~\Plan\Column_Changed.xml"));

                    string Column_Changed = (e.Item as GridDataItem)["Column_Changed"].Text;
                    if (column.GetElementsByTagName(Column_Changed)[0].InnerText != null)
                    {
                        (e.Item as GridDataItem)["Column_Changed"].Text = column.GetElementsByTagName(Column_Changed)[0].InnerText.ToString();
                    }
                }
                catch
                { 
                    
                }
            }
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "需求变更申请状态变更信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "需求变更申请状态变更信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "需求变更申请状态变更信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}