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
    public partial class MaterialApplicationModel : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "MaterialApplicationModel", this.Page);
                Session["StrWhereModel"] = null;
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
                    + " ,isnull(Sys_Model.Model, P_Pack.Model) as Model, P_Pack_Task.TaskCode, P_Pack_Task.ProductName as Com_ProductName, TaskDrawingCode as Com_Drawing_No"
                    + " , isnull(Sys_Phase.Phase,M_Demand_Merge_List.Stage) as Com_Stage"
                    + " from M_Demand_Merge_List" 
                    + " join P_Pack_Task on P_Pack_Task.TaskId = M_Demand_Merge_List.TaskId "
                    + " join P_Pack on P_Pack.PackId = M_Demand_Merge_List.PackId  left join Sys_Model on Convert(nvarchar(50),Sys_Model.ID) = P_Pack.Model "
                    + " left join Sys_Phase on Sys_Phase.Code =  M_Demand_Merge_List.Stage"
                    + " where Submit_Type = '0' and Is_submit = 'true' and MaterialDept = '" + HF_DeptCode.Value + "'"
                    + " and M_Demand_Merge_List.ID not in (select Material_ID from MaterialApplication where Is_del = 'false' and Material_ID is not null)";
            if (Session["StrWhereModel"] != null)
            {
                strSQL += Session["StrWhereModel"].ToString();
            }
            strSQL += " order by M_Demand_Merge_List.Id desc";

            DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
            Session["MAMGridSource"] = dt;
        }

        protected void RadGridMDML_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridMDML.DataSource = Session["MAMGridSource"];
        }

        protected void RadGridMDML_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                string id = item.GetDataKeyValue("ID").ToString();
                    
                DataRow datarow = ((Session["MAMGridSource"]) as DataTable).Select("ID='" + id + "'")[0];
                string Correspond_Draft_Code = datarow["Correspond_Draft_Code"].ToString();
                string mddldid =  Correspond_Draft_Code.Split(',')[0];
                string strSQL = "";
                strSQL = " select TDM_Description, Material_Name, Material_Mark, CN_Material_State, Material_Tech_Condition, Mat_Rough_Weight from M_Demand_DetailedList_Draft where Id = '" + mddldid + "'";
                DataTable dt = DBI.Execute(strSQL, true);

                item["TDM_Description"].Text = dt.Rows[0]["TDM_Description"].ToString();
                item["Material_Name"].Text = dt.Rows[0]["Material_Name"].ToString();
                item["Material_Mark"].Text = dt.Rows[0]["Material_Mark"].ToString();
                item["CN_Material_State"].Text = dt.Rows[0]["CN_Material_State"].ToString();
                item["Material_Tech_Condition"].Text = dt.Rows[0]["Material_Tech_Condition"].ToString();
                item["Mat_Rough_Weight"].Text = dt.Rows[0]["Mat_Rough_Weight"].ToString();
               
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
            Session["StrWhereModel"] = "";
            if (taskCode != "")
            {
                Session["StrWhereModel"] += " and P_Pack_Task.TaskCode like '%" + taskCode + "%'";
            }
            if (DrawingNo != "")
            {
                Session["StrWhereModel"] += " and Drawing_No like '%" + DrawingNo + "%'";
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
            RadGridMDML.ExportSettings.FileName = "请领物资信息列表--型号" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMDML.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGridMDML.ExportSettings.FileName = "请领物资信息列表--型号" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMDML.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGridMDML.ExportSettings.FileName = "请领物资信息列表--型号" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMDML.ExportSettings.IgnorePaging = true;
            RadGridMDML.MasterTableView.ExportToPdf();
            RadGridMDML.ExportSettings.IgnorePaging = false;
        }
    }
}