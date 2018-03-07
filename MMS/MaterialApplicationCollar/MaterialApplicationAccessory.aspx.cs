using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Camc.Web.Library;
using Telerik.Web.UI;

namespace mms.MaterialApplicationCollar
{
    public partial class MaterialApplicationAccessory : System.Web.UI.Page
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
                Common.CheckPermission(Session["UserName"].ToString(), "MaterialApplicationAccessory", this.Page);
                Session["StrWhere"] = null;
                string userId = Session["UserId"].ToString();
                string strSQL = " select DeptCode, Dept from Sys_DeptEnum where ID = (select Dept from Sys_UserInfo_PWD where Id = '" + userId + "')";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("/Admin/NoRights.aspx");
                }
                else
                {
                    HF_DeptCode.Value = dt.Rows[0]["DeptCode"].ToString();
                }
                GetMDML();
            }
        }

        public void GetMDML()
        {
            string strSQL = " select M_Demand_Merge_List.* "
                    + " , CUX_DM_PROJECT.DICT_Name as Model"
                    + " , (select Phase from Sys_Phase where Code = M_Demand_Merge_List.Stage) as Stage1"

                    + " from M_Demand_Merge_List"
                    + " left join GetBasicdata_T_Item as CUX_DM_PROJECT on CUX_DM_PROJECT.DICT_CODE = M_Demand_Merge_List.Project and DICT_CLASS='CUX_DM_PROJECT'"
                    + " where Submit_Type = '3' and Is_submit = 'true' and MaterialDept = '" + HF_DeptCode.Value + "'"
                    + "and Quantity_Left>0 and DemandNum_Left>0";
                   // + " and M_Demand_Merge_List.ID not in (select Material_ID from MaterialApplication where Is_del = 'false' and Material_ID is not null)";
            if (Session["StrWhere"] != null)
            {
                strSQL += Session["StrWhere"].ToString();
            }
            strSQL += " order by M_Demand_Merge_List.Id desc";

            DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
            Session["MAMGridSource"] = dt;
        }

        protected void RadGridMDML_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridMDML.DataSource = ((Session["MAMGridSource"]) as DataTable);
        }

        protected void RadGridMDML_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadButton rb = e.Item.FindControl("RB_APP") as RadButton;
                if (rb != null)
                {
                    string id = ((e.Item) as GridDataItem).GetDataKeyValue("ID").ToString();
                    rb.Attributes["onclick"] = string.Format("return ShowMaterialAppWindow({0})", id);
                }
            }
        }

        protected void RB_App_Click(object sender, EventArgs e)
        {
            GridDataItem gdi = RadGridMDML.SelectedItems[0] as GridDataItem;
            string id = gdi.GetDataKeyValue("ID").ToString();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "ShowMaterialAppWindow(" + id + ");", true);
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                GetMDML();
                RadGridMDML.Rebind();
            }
            else
            {
                throw new Exception("刷新页面出错，请按F5刷新！");
            }

        }

        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string taskCode = RTB_TaskCode.Text.Trim();
            string DrawingNo = RTB_Drawing_No.Text.Trim();
            string ID = RTB_ID.Text.Trim();
            string PROJECT = RTB_Project.Text.Trim();
            string Material_Name = RTB_Material_Name.Text.Trim();
            string ItemCode1 = RTB_ItemCode1.Text.Trim();
            string startTime = RDPStart.SelectedDate.ToString();
            string endTime = RDPEnd.SelectedDate.ToString();
            Session["StrWhere"] = "";
            if (taskCode != "")
            {
                Session["StrWhere"] += " and TaskCode like '%" + taskCode + "%'";
            }
            if (DrawingNo != "")
            {
                Session["StrWhere"] += " and Drawing_No like '%" + DrawingNo + "%'";
            }
            if (PROJECT != "")
            {
                Session["StrWhere"] += " and PROJECT like '%" + PROJECT + "%'";
            }
            if (Material_Name != "")
            {
                Session["StrWhere"] += " and Material_Name like '%" + Material_Name + "%'";
            }
            if (ItemCode1 != "")
            {
                Session["StrWhere"] += " and ItemCode1 like '%" + DrawingNo + "%'";
            }
            try
            {
                Session["StrWhere"] += " and SUBMIT_DATE >= '" + Convert.ToDateTime(startTime).ToString() + "'";
            }
            catch { }
            try
            {
                Session["StrWhere"] += " and SUBMIT_DATE <= '" + Convert.ToDateTime(endTime).ToString() + "'";
            }
            catch { }
            if (ID != "")
            {
                Session["StrWhere"] += " and M_Demand_Merge_List.ID like '%" + ID + "%'";
            }
            GetMDML();
            RadGridMDML.Rebind();
        }

        protected void RadGridMDML_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadGridMDML.SelectedItems.Count > 0)
            {
                HFMDMLID.Value = ((RadGridMDML.SelectedItems[0]) as GridDataItem).GetDataKeyValue("ID").ToString();
            }
            else 
            {
                HFMDMLID.Value = "";
            }
        }
		protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGridMDML.ExportSettings.FileName = "物资信息列表--车间备料" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMDML.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGridMDML.ExportSettings.FileName = "物资信息列表--车间备料" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMDML.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGridMDML.ExportSettings.FileName = "物资信息列表--车间备料" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMDML.ExportSettings.IgnorePaging = true;
            RadGridMDML.MasterTableView.ExportToPdf();
            RadGridMDML.ExportSettings.IgnorePaging = false;
        }
    }
}