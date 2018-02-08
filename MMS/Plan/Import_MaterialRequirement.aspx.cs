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
using mms.PublicClass;
using System.Data.OleDb;
using System.IO;

namespace mms.Plan
{
    public partial class Import_MaterialRequirement : System.Web.UI.Page
    {
         #region M_Demand_DetailedList_Draft属性
        public class M_Demand_DetailedList_Draft
        {
            public string Id { get; set; }
            public System.Nullable<int> VerCode { get; set; }
            public string Class_Id { get; set; }
            public string Object_Id { get; set; }
            public System.Nullable<int> Stage { get; set; }
           public System.Nullable<int> Material_State { get; set; }
            public string Material_Tech_Condition { get; set; }
            public string Material_Code { get; set; }
            public System.Nullable<int> ParentId { get; set; }
            public string Material_Spec { get; set; }
            public string TDM_Description { get; set; }
            public string Material_Name { get; set; }
            public System.Nullable<int> PackId  { get; set; }
            public System.Nullable<int> TaskId { get; set; }
            public string DraftId { get; set; }
            public string Drawing_No { get; set; }
            public string Technics_Line { get; set; }
            public string Technics_Comment { get; set; }
            public string Material_Mark { get; set; }
            public string ItemCode1 { get; set; }
            public string ItemCode2 { get; set; }
            public string MaterialsNum { get; set; }
            public string Mat_Unit { get; set; }
            public string LingJian_Type { get; set; }
            public string Mat_Rough_Weight { get; set; }
            public string Mat_Pro_Weight { get; set; }
            public string Mat_Weight { get; set; }
            public string Mat_Efficiency { get; set; }
            public string Mat_Comment { get; set; }
            public string Mat_Technics { get; set; }
            public string Rough_Spec { get; set; }
            public string Rough_Size { get; set; }
            public string Dinge_Size { get; set; }
            public string MaterialsDes { get; set; }
            public System.Nullable<int> StandAlone { get; set; }
            public System.Nullable<int> ThisTimeOperation { get; set; }
            public string PredictDeliveryDate { get; set; }
            public System.Nullable<decimal> DemandNumSum { get; set; }
            public System.Nullable<decimal> NumCasesSum { get; set; }
            public string DemandDate { get; set; }
            public string Quantity { get; set; }
            public string Tech_Quantity { get; set; }
            public string Memo_Quantity { get; set; }
            public string Test_Quantity { get; set; }
            public string Required_Quantity { get; set; }
            public string Other_Quantity { get; set; }
            public string Ballon_No { get; set; }
            public string Comment { get; set; }
            public string Is_allow_merge { get; set; }
            public System.Nullable<System.DateTime> Import_Date { get; set; }
            public int User_ID { get; set; }
            public string TaskCode { get; set; }
            public string MaterailDept { get; set; }
            public string MissingDescription { get; set; }
            public string CN_Material_State { get; set; }
            public string MaterialDept { get; set; }
            public string MDML_Id { get; set; }
            public System.Nullable<int> MDPId { get; set; }
            public bool Is_del { get; set; }
            public string JSGS_Des { get; set; }
            public string Special_Needs { get; set; }

            public string Urgency_Degre { get; set; }
            public string Secret_Level { get; set; }
            public string Use_Des { get; set; }
            public string Shipping_Address { get; set; }
            public string Certification { get; set; }
            public string Manufacturer { get; set; }
            public string Attribute4 { get; set; }
        }
         #endregion
        private static DataTable GridSource1;
        private  DataTable GridSource
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
                    InitTable.Columns.Add("Dinge_Size");
                    InitTable.Columns.Add("Rough_Size");
                    InitTable.Columns.Add("Rough_Spec");
                    InitTable.Columns.Add("DemandDate");
                    InitTable.Columns.Add("Special_Needs");

                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Manufacturer");
                    InitTable.Columns.Add("Tech_Quantity");
                    InitTable.Columns.Add("Technics_Comment");
                    InitTable.Columns.Add("Memo_Quantity");
                    InitTable.Columns.Add("Mat_Comment");

                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Stage");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Sum_Price");
                    InitTable.Columns.Add("Submit_Type");
                    InitTable.Columns.Add("Is_Submit");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("Contact_Way");
                    InitTable.Columns.Add("Submit_Date");
                    InitTable.Columns.Add("Get_Quantity");
                    InitTable.Columns.Add("subtype");
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
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
    
            if (!IsPostBack)
            {
             //   Common.CheckPermission(Session["UserName"].ToString(), "MDemandImport", this.Page); 

                string PackId = "";
              
                if (Request.QueryString["PackId"] != null && Request.QueryString["PackId"].ToString() != "")
                {
                    PackId = Request.QueryString["PackId"].ToString();

                
                }
  
                   GridSource1 = new System.Data.DataTable();
                 
                   this.hfBh.Value = "";
                   this.hfTaskId.Value = "";
                   this.ViewState["submit_type"] = "0";
                    GridSource = Common.AddTableRowsID(GetDetailedListList());
                    Session["gds"] = null;         
                    this.span_apply_time1.InnerText = DateTime.Now.ToString("yyyy-MM-dd"); 

                    BindDeptUserAddress();
      

                    this.ViewState["GridSource2"] = Common.AddTableRowsID(GetP_Pack_Task());

                    string strSQL = " select PlanCode, State , PlanName, Remark , isnull((select Model from Sys_Model where Convert(nvarchar(50),ID) = P_Pack.Model), P_Pack.Model) as Model from P_Pack where PackID = '" + PackId + "'";
                    DataTable dt = DBI.Execute(strSQL, true);
                    lblPlanCode.Text = dt.Rows[0]["PlanCode"].ToString();
                    lblModel.Text = dt.Rows[0]["Model"].ToString();
                    HFState.Value = dt.Rows[0]["State"].ToString();
                    if (dt.Rows[0]["State"].ToString() == "2")
                    {
                        table1.Visible = true;
                        lblPlanName.Text = dt.Rows[0]["PlanName"].ToString();
                        lblRemark.Text = dt.Rows[0]["Remark"].ToString();
                        RTB_PlanName.Visible = false;
                        RTB_Remark.Visible = false;
                    }
                    else
                    {
                        RTB_PlanName.Text = dt.Rows[0]["PlanName"].ToString();
                        RTB_Remark.Text = dt.Rows[0]["Remark"].ToString();
                    }
           
            }
        }


        protected DataTable GetP_Pack_Task()
        {
            string PaskID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            string strWhere = "";
            if (Session["PTWhere"] != null) { strWhere = Session["PTWhere"].ToString(); }
            string strSQL = " select * , case when Stage = '1' then 'M' when Stage = '2' then 'C' when Stage = '3' then 'S' else 'D' end as Stage1 from P_Pack_Task where IsDel = 'false' and PackId = '" + PaskID + "' " + strWhere + " order by TaskCode";
            DataTable dt = new DataTable();
            try
            {
                dt = DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "获取计划包列表失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }
            return dt;
        }

        protected void RadGridP_Pack_Task_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridP_Pack_Task.DataSource = this.ViewState["GridSource2"];
        }

        protected void RadGridP_Pack_Task_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TaskID"].ToString();
                string IsSpread = (this.ViewState["GridSource2"] as DataTable).Select("TaskID='" + ID + "'")[0]["IsSpread"].ToString().ToLower();
                RadioButtonList RBL = e.Item.FindControl("RBL_IsSpread") as RadioButtonList;
                if (RBL != null)
                {
                    RBL.CssClass = ID.ToString();
                    if (HFState.Value == "2")
                    {
                        RBL.Visible = false;
                        Label lbl = e.Item.FindControl("lbl_IsSpread") as Label;
                        lbl.Text = (IsSpread == "true" ? "是" : "否");
                    }
                    else
                    {
                        if (IsSpread != "" && IsSpread != null)
                        {
                            RBL.SelectedValue = IsSpread.ToLower();
                        }
                    }
                }
            }
            if (e.Item is GridCommandItem)
            {
                if (HFState.Value == "2")
                {
                    RadioButtonList RBL = e.Item.FindControl("RBL_IsSpreadAll") as RadioButtonList;
                    RBL.Visible = false;
                    Label lbl = e.Item.FindControl("lbl") as Label;
                    lbl.Visible = false;
                }
            }
        }

        protected void RBL_IsSpread_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = sender as RadioButtonList;
            string TaskID = rbl.CssClass;
            string IsSpread = rbl.SelectedValue;
            if (TaskID != "")
            {
                string strSQL = " Update P_Pack_Task set IsSpread = '" + IsSpread + "' where TaskID = '" + TaskID + "'";
                try
                {
                    DBI.Execute(strSQL);
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "修改展开状态失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
        }

        protected void RB_Query_Click(object sender, EventArgs e)
        {
            string ProductName = RTB_ProductName.Text.Trim();
            string DrawingNum = RTB_DrawingNum.Text.Trim();
            string TaskNum = RTB_TaskNum.Text.Trim();
            DateTime? StartDate = RDP_StartDate.SelectedDate;
            DateTime? EndDate = RDP_EndDate.SelectedDate;

            string strWhere = "";
            if (ProductName != "")
            {
                strWhere += " and ProductName like '%" + ProductName + "%'";
            }
            if (DrawingNum != "")
            {
                strWhere += " and TaskDrawingCode like '%" + DrawingNum + "%'";
            }
            if (TaskNum != "")
            {
                strWhere += " and TaskCode like '%" + TaskNum + "%'";
            }
            if (StartDate != null)
            {
                strWhere += " and PlanFinishTime >= '" + Convert.ToDateTime(StartDate).ToString("yyyy-MM-dd") + "'";
            }
            if (EndDate != null)
            {
                strWhere += " and PlanFinishTime <= '" + Convert.ToDateTime(EndDate).ToString("yyyy-MM-dd") + "'";

            }
            Session["PTWhere"] = strWhere;
            this.ViewState["GridSource2"] = GetP_Pack_Task();
            RadGridP_Pack_Task.Rebind();

        }

        protected void RB_Add_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RadGridP_Pack_Task.Items.Count; i++)
            {
                RadioButtonList RBL = RadGridP_Pack_Task.Items[i].FindControl("RBL_IsSpread") as RadioButtonList;
                if (RBL != null)
                {
                    if (RBL.SelectedValue == "" || RBL.SelectedValue == null)
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行没有选择是否展开";
                        RadNotificationAlert.Show();
                        return;
                    }
                }
            }

            string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            try
            {
                string strSQL = " Update P_Pack set State = '2' where PackID = '" + PackID + "'";
                DBI.Execute(strSQL);
                RadNotificationAlert.Text = "归档成功！";
                RadNotificationAlert.Show();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }
        }

        protected void RBL_IsSpreadAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (sender) as RadioButtonList;
            string value = rbl.SelectedItem.Value;
            for (int i = 0; i < RadGridP_Pack_Task.Items.Count; i++)
            {
                RadioButtonList rbl1 = RadGridP_Pack_Task.Items[i].FindControl("RBL_IsSpread") as RadioButtonList;
                if (value == "") { rbl1.SelectedIndex = -1; }
                else { rbl1.SelectedValue = value; }
            }
        }

        protected DataTable GetDetailedListList()
        {
            try
            {
                string strSQL = "";
                string TaskID = this.hfTaskId.Value;
                if (TaskID != "")
                {
                    strSQL =
                        " select M_Demand_DetailedList_Draft.* , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                        " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1" +
                        //" , CUX_DM_PROJECT.DICT_Name as Model" +
                        ",CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes " +
                        ",case when LingJian_Type='1' then '标准件' else case when LingJian_Type='2' then '成品件'  else case when LingJian_Type='3' then '通用件' else case when LingJian_Type='4' then '专用件' else case when LingJian_Type='5' then '组件'   else '其它' end  end end end end as LingJian_Type1" +
                       
                        " from M_Demand_DetailedList_Draft"+
                        " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_DetailedList_Draft.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                        " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_DetailedList_Draft.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'"+
                      //  " left join GetBasicdata_T_Item as CUX_DM_PROJECT on CUX_DM_PROJECT.DICT_CODE = M_Demand_Merge_List.Project and CUX_DM_PROJECT.DICT_CLASS='CUX_DM_PROJECT'" +
                        "where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))" +

                        " union all select M_Demand_DetailedList_Draft.*, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
                        ",CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes " +
                        ",case when LingJian_Type='1' then '标准件' else case when LingJian_Type='2' then '成品件'  else case when LingJian_Type='3' then '通用件' else case when LingJian_Type='4' then '专用件' else case when LingJian_Type='5' then '组件' else '其它' end  end end end end as LingJian_Type1" +
    
                        " from M_Demand_DetailedList_Draft "+
                       " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_DetailedList_Draft.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                       " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_DetailedList_Draft.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'"+
   
                        "where PackId = '" + Request.QueryString["PackId"].ToString() +"' and TaskId='"+TaskID+ "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and is_del = 'false' and Material_State = '0' order by ID";
                }
                else
                {
                    strSQL =
                      " select M_Demand_DetailedList_Draft.* , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                      " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1" +

                        ",CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes " +
                        //" , CUX_DM_PROJECT.DICT_Name as Model" +
                      ",case when LingJian_Type='1' then '标准件' else case when LingJian_Type='2' then '成品件' else case when LingJian_Type='3' then '通用件' else case when LingJian_Type='4' then '专用件' else case when LingJian_Type='5' then '组件'   else '其它' end  end end end end as LingJian_Type1" +
                      " from M_Demand_DetailedList_Draft"+

                      " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_DetailedList_Draft.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                      " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_DetailedList_Draft.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                        //  " left join GetBasicdata_T_Item as CUX_DM_PROJECT on CUX_DM_PROJECT.DICT_CODE = M_Demand_Merge_List.Project and CUX_DM_PROJECT.DICT_CLASS='CUX_DM_PROJECT'" +

                      "where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))" +


                      " union all select M_Demand_DetailedList_Draft.*, 'false' as checked, '未提交' as mstate, '0' as quantity1" +

                       ",CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes " +
                        //" , CUX_DM_PROJECT.DICT_Name as Model" +
                      ",case when LingJian_Type='1' then '标准件' else case when LingJian_Type='2' then '成品件' else case when LingJian_Type='3' then '通用件' else case when LingJian_Type='4' then '专用件' else case when LingJian_Type='5' then '组件'   else '其它' end  end end end end as LingJian_Type1" +

                      " from M_Demand_DetailedList_Draft"+
                      " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_DetailedList_Draft.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                      " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_DetailedList_Draft.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                      //  " left join GetBasicdata_T_Item as CUX_DM_PROJECT on CUX_DM_PROJECT.DICT_CODE = M_Demand_Merge_List.Project and CUX_DM_PROJECT.DICT_CLASS='CUX_DM_PROJECT'" +

                      "where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and is_del = 'false' and Material_State = '0' order by ID";
 
                }
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }

        protected void RadGrid_DemandDetailedList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_DemandDetailedList.DataSource = GridSource;
        }
        protected void RadGrid_DemandDetailedList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            DataTable table = GridSource;
            GridDataItem dataitem = e.Item as GridDataItem;
            if (e.CommandName == "delete")
            {
                int ID =Convert.ToInt32( dataitem.GetDataKeyValue("ID"));
                string MDPId = table.Rows[e.Item.DataSetIndex]["MDPId"].ToString();
                DeletePlanDetailsRecord(MDPId, ID);
                GridSource = Common.AddTableRowsID(GetDetailedListList());
                RadGrid_DemandDetailedList.DataSource = GridSource;
            }
        }
        protected void DeletePlanDetailsRecord(string MDPId, int ID)
        {
            DBI.OpenConnection();
            try
            {
             //   if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            //int userid = Convert.ToInt32(Session["UserId"].ToString());
             //   string strSQL = @"exec Proc_DeleteTechnologyNoSubmit " + MDPId + "," + ID + "," + userid;
                string strSQL = "delete from M_Demand_DetailedList_Draft where ID=" + ID;
                DBI.Execute(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("删除信息时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        protected void RadComboBoxUrgencyDegree_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("Id='" + id + "'")[0]["Urgency_Degre"] = cb.SelectedItem.Value;
        }

        protected void RadComboBoxSecretLevel_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("Id='" + id + "'")[0]["Secret_Level"] = cb.SelectedItem.Value;
        }

        protected void RadComboBoxUseDes_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("Id='" + id + "'")[0]["Use_Des"] = cb.SelectedItem.Value;
        }

        protected void RadComboBoxShippingAddress_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("Id='" + id + "'")[0]["Shipping_Address"] = cb.SelectedItem.Value;
        }

        protected void RadComboBoxCertification_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("Id='" + id + "'")[0]["Certification"] = cb.SelectedItem.Value;
        }

        protected void rtb_SpecialNeeds_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("Id='" + id + "'")[0]["Special_Needs"] = rtb.Text;
        }

        protected void RTB_MANUFACTURER_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("ID='" + id + "'")[0]["MANUFACTURER"] = rtb.Text;
        }
        private void BindDDlShipping_Addr(RadComboBox rcb, string DeptCode)
        {
            string strSQL = "select KeyWord from Sys_Dict" +
                " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
                " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + DeptCode + "')";
            DataTable dt = DBI.Execute(strSQL, true);
            rcb.DataSource = dt;
            rcb.DataTextField = "KeyWord";
            rcb.DataValueField = "KeyWord";
            rcb.DataBind();
        }
        protected void RadGrid_Importlist_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
              
                /*  string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                if (Session["UserId"] == null)
                {
                    RadNotificationAlert.Text = "登录超时，请重新登录！" ;
                    RadNotificationAlert.Show();
                    return;
                }
                string userid = Session["UserId"].ToString();
                string strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                                " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                                " where Sys_UserInfo_PWD.ID = '" + userid + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                RadComboBox RadComboBoxMaterialDept = e.Item.FindControl("RadComboBoxMaterialDept") as RadComboBox;

                RadComboBoxMaterialDept.DataSource = dt;
                RadComboBoxMaterialDept.DataTextField = "Dept";
                RadComboBoxMaterialDept.DataValueField = "DeptCode";
                RadComboBoxMaterialDept.DataBind();
*/
         /*       RadComboBox RDDL_LingJian_Type = e.Item.FindControl("RDDL_LingJian_Type") as RadComboBox;
                if (GridSource1.Select("ID='" + id + "'")[0]["LingJian_Type"] != null)
                {
                    RDDL_LingJian_Type.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["LingJian_Type"].ToString()).Selected = true;
                }
         

                RadComboBox rcbAddr = e.Item.FindControl("RadComboBoxShippingAddress") as RadComboBox;
                string MaterialDept = dt.Rows[0]["DeptCode"].ToString();

                rcbAddr.CssClass = id;
                BindDDlShipping_Addr(rcbAddr, MaterialDept);
                if (GridSource1.Select("ID='" + id + "'")[0]["Shipping_Address"] != null)
                {
                    rcbAddr.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Shipping_Address"].ToString()).Selected = true;
                }
                RadComboBox RadComboBoxUseDes = e.Item.FindControl("RadComboBoxUseDes") as RadComboBox;
                if (GridSource1.Select("ID='" + id + "'")[0]["Use_Des"] != null)
                {
                    RadComboBoxUseDes.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Use_Des"].ToString()).Selected = true;
                }
                RadComboBox RadComboBoxSecretLevel = e.Item.FindControl("RadComboBoxSecretLevel") as RadComboBox;
                if (GridSource1.Select("ID='" + id + "'")[0]["Secret_Level"] != null)
                {
                    RadComboBoxSecretLevel.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Secret_Level"].ToString()).Selected = true;
                    //RadComboBoxSecretLevel.FindItemByText("内部").Selected = true;
                }
                RadComboBox RadComboBoxUrgencyDegree = e.Item.FindControl("RadComboBoxUrgencyDegree") as RadComboBox;
                if (GridSource1.Select("ID='" + id + "'")[0]["Urgency_Degre"] != null)
                {
                    RadComboBoxUrgencyDegree.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Urgency_Degre"].ToString()).Selected = true;
                    //RadComboBoxUrgencyDegree.FindItemByText("一般").Selected = true;
                }
                RadComboBox RadComboBoxCertification = e.Item.FindControl("RadComboBoxCertification") as RadComboBox;
                if (GridSource1.Select("ID='" + id + "'")[0]["Certification"] != null)
                {
                    RadComboBoxCertification.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Certification"].ToString()).Selected = true;
                }
                RadComboBox RadComboBoxAttribute4 = e.Item.FindControl("RadComboBoxAttribute4") as RadComboBox;
                if (GridSource1.Select("ID='" + id + "'")[0]["Attribute4"] != null)
                {
                    RadComboBoxAttribute4.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Attribute4"].ToString()).Selected = true;
                }

                RadTextBox rtbSpecialNeeds = e.Item.FindControl("rtb_SpecialNeeds") as RadTextBox;
                if (GridSource1.Select("ID='" + id + "'")[0]["Special_Needs"] != null)
                {
                    rtbSpecialNeeds.Text = (GridSource1.Select("ID='" + id + "'")[0]["Special_Needs"].ToString());
                }
          */
            }
        }

        protected void RadGridImport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridImport.DataSource = GridSource1;
        }

     
        protected void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            HFGridItemsCount.Value = "0";
            if (RadAsyncUpload1.UploadedFiles.Count == 1)
            {
                //导入文件存在服务器上
                string filderPath = Server.MapPath(@"~\Plan\ImportExcel\");
                if (!System.IO.Directory.Exists(filderPath))
                {
                    System.IO.Directory.CreateDirectory(filderPath);
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") +
                                  RadAsyncUpload1.UploadedFiles[0].FileName;
                string filePath = Path.Combine(filderPath, fileName);
              /*   if(RadGridP_Pack_Task.MasterTableView.Items.Count>1)
                 {
                   if( RadGridP_Pack_Task.MasterTableView.GetSelectedItems().Count()<1)
                   {
                       File.Delete(filePath);
                       RadNotificationAlert.Text = "请先选择一条任务记录";
                       RadNotificationAlert.Show();
                       return;

                   }
                 }
                */
       
                RadAsyncUpload1.UploadedFiles[0].SaveAs(filePath);

                HFFileName.Value = fileName;

                string conn = " Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath(@"~\Plan\ImportExcel\") +
                              "\\" + fileName + "; Extended Properties ='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                OleDbConnection thisconnection = new OleDbConnection(conn);

                try
                {
                    if (fileName != "")
                    {
                        thisconnection.Open();

                        string sql = " select * from [Sheet1$]";
                        OleDbDataAdapter command = new OleDbDataAdapter(sql, thisconnection);
                        DataSet ds = new DataSet();
                        command.Fill(ds, "[Sheet1$]");

                        thisconnection.Close();

                        GridSource1 = ds.Tables[0];
                        int columnscount = 0;
                        for (int i = 0; i < GridSource1.Columns.Count; i++)
                        {
                            switch (GridSource1.Columns[i].ColumnName.Trim())
                            {
    

                         
                               case "图号":
                                    GridSource1.Columns[i].ColumnName = "DRAWING_NO";
                                    columnscount++;
                                    break;
                               /*    
                             case "任务号":
                                 GridSource1.Columns[i].ColumnName = "TaskCode";
                                 columnscount++;
                                 break;

                             case "产品编号":
                                 GridSource1.Columns[i].ColumnName = "Material_Code";
                                 columnscount++;
                                 break;
                             case "研制阶段":
                                 GridSource1.Columns[i].ColumnName = "stage";
                                 columnscount++;
                                 break;
                              */
                   
                    
                                case "产品名称":
                                    GridSource1.Columns[i].ColumnName = "TDM_Description";
                                    columnscount++;
                                    break;
                                case "零件类型":
                                    GridSource1.Columns[i].ColumnName = "LingJian_Type";
                                    columnscount++;
                                    break;

                                case "工艺路线":
                                    GridSource1.Columns[i].ColumnName = "Technics_Line";
                                    columnscount++;
                                    break;


                               case "物资名称":
                                     GridSource1.Columns[i].ColumnName = "Material_Name";
                                     columnscount++;
                                     break;

                               case "物资牌号":
                                     GridSource1.Columns[i].ColumnName = "Material_Mark";
                                     columnscount++;
                                     break;

                               case "供应状态":
                                     GridSource1.Columns[i].ColumnName = "CN_Material_State";
                                     columnscount++;
                                     break;

                               case "技术标准":
                                     GridSource1.Columns[i].ColumnName = "Material_Tech_Condition";
                                     columnscount++;
                                     break;


                               case "坯料规格":
                                     GridSource1.Columns[i].ColumnName = "Rough_Spec";
                                     columnscount++;
                                     break;

                                 case "需求尺寸":
                                     GridSource1.Columns[i].ColumnName = "ROUGH_SIZE";
                                     columnscount++;
                                     break;
                                  
                   

                                 case "单件质量":
                                     GridSource1.Columns[i].ColumnName = "Mat_Rough_Weight";
                                     columnscount++;
                                     break;

                                 case "每产品质量":
                                     GridSource1.Columns[i].ColumnName = "Mat_Pro_Weight";
                                     columnscount++;
                                     break;

                                case "物资编码":
                                    GridSource1.Columns[i].ColumnName = "ItemCode1";
                                    columnscount++;
                                    break;
                                case "坯料尺寸":
                                    GridSource1.Columns[i].ColumnName = "Dinge_Size";
                                    columnscount++;
                                    break;
                                case "特殊需求":
                                    GridSource1.Columns[i].ColumnName = "Special_Needs";
                                    columnscount++;
                                    break;
                                case "需求件数":
                                    GridSource1.Columns[i].ColumnName = "NumCasesSum";
                                    columnscount++;
                                    break;

                                case "计量单位":
                                    GridSource1.Columns[i].ColumnName = "MAT_UNIT";
                                    columnscount++;
                                    break;
                                case "需求数量":
                                    GridSource1.Columns[i].ColumnName = "DemandNumSum";
                                    columnscount++;
                                    break;

                                case "工艺数量":
                                    GridSource1.Columns[i].ColumnName = "Tech_Quantity";
                                    columnscount++;
                                    break;

                                case "路线备注":
                                    GridSource1.Columns[i].ColumnName = "Technics_Comment";
                                    columnscount++;
                                    break;

                                case "备件数量":
                                    GridSource1.Columns[i].ColumnName = "Memo_Quantity";
                                    columnscount++;
                                    break;

                                case "定额备注":
                                    GridSource1.Columns[i].ColumnName = "Mat_Comment";
                                    columnscount++;
                                    break;



                                case "需求日期":
                                    GridSource1.Columns[i].ColumnName = "DemandDate";
                                    columnscount++;
                                    break;

                            

                                case "紧急程度":
                                    GridSource1.Columns[i].ColumnName = "Urgency_Degre";
                                    columnscount++;
                                    break;
                                case "密级":
                                    GridSource1.Columns[i].ColumnName = "Secret_Level";
                                    columnscount++;
                                    break;
                                case "用途":
                                    GridSource1.Columns[i].ColumnName = "Use_Des";
                                    columnscount++;
                                    break;
                                case "配送地址":
                                    GridSource1.Columns[i].ColumnName = "Shipping_Address";
                                    columnscount++;
                                    break;
                                case "合格证":
                                    GridSource1.Columns[i].ColumnName = "Certification";
                                    columnscount++;
                                    break;
                                case "国产/进口":
                                    GridSource1.Columns[i].ColumnName = "Attribute4";
                                    columnscount++;
                                    break;

                                case "生产厂家":
                                    GridSource1.Columns[i].ColumnName = "Manufacturer";
                                    columnscount++;
                                    break;

                            }
                        }

                        GridSource1.Columns.Add("ID");     
                        if (columnscount < 27)
                        {
                            GridSource1 = new System.Data.DataTable();
                            RadGridImport.Rebind();
                            File.Delete(filePath);
                            RadNotificationAlert.Text = "失败！请参照Excel模板页面表头";
                            RadNotificationAlert.Show();
                            return;
                        }
                        int rowsid = 1;
                        for (int i = 0; i < GridSource1.Rows.Count; i++)
                        {
                            string itemCode1 = GridSource1.Rows[i]["ItemCode1"].ToString();
                            if (itemCode1 != "")
                            {
                                GridSource1.Rows[i]["ID"] = rowsid;
                                rowsid++; 
                            }
                            else
                            {
                                GridSource1.Rows[i].Delete();
                                RadNotificationAlert.Text = "物资编码不能为空";
                                RadNotificationAlert.Show();
                            }
                        }     

                        RadGridImport.Rebind();
                        HFGridItemsCount.Value = RadGridImport.Items.Count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    GridSource1 = new System.Data.DataTable();
                    RadGridImport.Rebind();
                    File.Delete(filePath);

                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
            else
            {
                GridSource1 = new System.Data.DataTable();
                RadGridImport.Rebind();

                RadNotificationAlert.Text = "请选择文件";
                RadNotificationAlert.Show();
                return;
            }
        }

        protected void Set_Txt_ByItemCode1(string ItemCode1,int i)
        {
   
            string strSQL = " select * from GetCommItem_T_Item where seg3 = '" + ItemCode1 + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            if (dt.Rows.Count > 0)
            {
                string Seg5 = dt.Rows[0]["Seg5"].ToString().Substring(0, 4);
                GridSource1.Rows[i]["Material_Mark"] = dt.Rows[0]["Seg13"].ToString();
                GridSource1.Rows[i]["CN_Material_State"] = "";
                switch (Seg5)
                {
                    case "YY01":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg21"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY02":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg15"].ToString();
                        break;
                    case "YY03":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg15"].ToString();
                        GridSource1.Rows[i]["DINGE_SIZE"] = dt.Rows[0]["Seg16"].ToString();
                        break;
                    case "YY04":
                    case "YY05":
                    case "YY06":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY07":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg20"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY08":
                    case "YY09":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    default:
                        GridSource1.Rows[i]["Material_Name"] = "";
                        GridSource1.Rows[i]["MAT_UNIT"] = "";
                        GridSource1.Rows[i]["Rough_Spec"] = "";
                        break;
                }
            }
            else
            {
                RadNotificationAlert.Text = "物资编码不存在";
                RadNotificationAlert.Show();
                GridSource1.Rows[i].Delete();
                return;
            }
        }


        protected void RBClear_Click(object sender, EventArgs e)
        {
            try
            {
              //  for (int i = 0; i < RadGridImport.Items.Count; i++)
               // {
                   // GridSource1.Rows.RemoveAt(i);
               // }
                GridSource1.Dispose();
                GridSource1 = new System.Data.DataTable();
                RadGridImport.Rebind();
                HFGridItemsCount.Value = RadGridImport.Items.Count.ToString();
                RadNotificationAlert.Text = "清空成功！";
                RadNotificationAlert.Show();
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "清空失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                return;
            }

        }
        protected void RBDelete_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < RadGridImport.SelectedItems.Count; i++)
                {
                    GridSource1.Rows.RemoveAt(i);
                }
                RadGridImport.Rebind();
                HFGridItemsCount.Value = RadGridImport.Items.Count.ToString();

            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "删除失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                return;
            }

        }

       

        protected void RBImport_Click(object sender, EventArgs e)
        {
            string TaskID = "";
            if (RadGridP_Pack_Task.MasterTableView.Items.Count > 1)
            {

                GridDataItem[] dataItems = RadGridP_Pack_Task.MasterTableView.GetSelectedItems();
                if (dataItems.Count() < 1)
                {
                    RadNotificationAlert.Text = "请先选择一条任务记录";
                    RadNotificationAlert.Show();
                    return;

                }
                else if (dataItems.Count() > 1)
                {
                    RadNotificationAlert.Text = "请勿选择多条任务记录";
                    RadNotificationAlert.Show();
                    return;
                }
                else
                {
                    TaskID = dataItems[0].GetDataKeyValue("TaskID").ToString();
                }
            }
            else
            {
                GridDataItem dataItem = RadGridP_Pack_Task.MasterTableView.Items[0];
                TaskID = dataItem.GetDataKeyValue("TaskID").ToString();
            }
            this.hfTaskId.Value = TaskID;
            if (RadGridImport.Items.Count == 0)
            {
                RadNotificationAlert.Text = "失败！没有可导入数据";
                RadNotificationAlert.Show();
                return;
            }
         


          //  try
            {
                if (Session["UserId"] == null)
                {
                    RadNotificationAlert.Text = "登录超时，请重新登录！";
                    RadNotificationAlert.Show();
                    return;
                }
                string DBContractConn =
                    ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString
                        .ToString();
                DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
               
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = "";
              
                int Submit_Type = Convert.ToInt32(this.ViewState["submit_type"].ToString());//1－工艺试验件；2－技术创新课题；3－车间备料 0-型号物资导入

                int PackId = Convert.ToInt32(Request.QueryString["PackId"].ToString());

                M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft()
                {
                    Ballon_No = "",
                    Class_Id = null,
                    CN_Material_State = "",
                    Special_Needs = "",
                    DemandDate = "",
                    DemandNumSum = 0,
                    DraftId = null,
                    Drawing_No = "",

                    Import_Date = null,
                    Is_allow_merge = null,
                    Is_del = false,
                    ItemCode1 = "",
                    ItemCode2 = "",
                    JSGS_Des = "",
                    LingJian_Type = null,
                    Mat_Comment = "",
                    Mat_Efficiency = "",
                    Mat_Pro_Weight = "",
                    Mat_Rough_Weight = "",
                    Mat_Technics = "",
                    Mat_Unit = "",
                    Mat_Weight = "",
                    Material_Code = "",
                    Material_Mark = "",
                    
                    Material_Name = "",
                    Material_Spec = "",
                    Material_State = 0,
                    Material_Tech_Condition = "",
                    MaterialDept = "",
                    MaterialsDes = "",
                    MaterialsNum = null,
                    MDML_Id = "",
                    MDPId = null,
                    Memo_Quantity = "",
                    MissingDescription = "",
                    NumCasesSum = 0,
                    Object_Id = null,
                    Other_Quantity = "",
                    PackId = PackId,
                    ParentId = -1,
                    PredictDeliveryDate = null,
                    Quantity = "",
                    Required_Quantity = "",
                    Rough_Size = "",
                    Dinge_Size="",
                    Rough_Spec = "",
                    Stage = null,
                    StandAlone = null,   //单机配套数量
                    TaskCode = "",
                    TaskId = null,
                    TDM_Description = "",
                    Tech_Quantity = "",
                    Technics_Comment = "",
                    Technics_Line = "",
                    Test_Quantity = "",
                    ThisTimeOperation = null, //投产数量
                    User_ID = userid,
                    VerCode = 1,
                    Urgency_Degre = null,
                    Secret_Level = null,
                    Use_Des  = null,
                    Shipping_Address   = null,
                    Certification  = null,
                    Manufacturer = null,
                    Attribute4 = null,
                };
            //    strSQL = " Select * From V_M_Draft_List where packid='" + PackId + "'";
                strSQL = " Select * From M_Demand_Plan_List where packid='" + PackId + "'";
                DataTable dt = DBI.Execute(strSQL, true);

                string DraftID = "";
                int MDPID = 0;
                if (dt.Rows.Count > 0)
                {
                    DraftID = dt.Rows[0]["draftid"].ToString();
                    this.hfBh.Value = dt.Rows[0]["Id"].ToString();
                }

                if (DraftID == "" || DraftID == null)
                {
                    string Draft_Code = DBI.GetSingleValue(" Exec  [Proc_CodeBuildByCodeDes1] '材料清单编号','JZWZ'");
                    strSQL = " Insert into [dbo].[M_Draft_List] (Draft_Code, Material_State, Lasttime_Synchro_Time, PackId, Task_Type, List_Maker)"
                        + " values ('" + Draft_Code + "','1',GetDate(),'" + PackId + "','0','" + userid + "') select @@identity";
                    DraftID = DBI.GetSingleValue(strSQL);

                
                }
               // if (DraftID != "" & DraftID != null)
           //     {
                //    MDDLD.DraftId = Convert.ToInt32(DraftID);
           //     }

         

             strSQL = " select TaskId, PackId, ProductName, TaskDrawingCode, TaskCode, Stage, Unit, MatingNum, DefrayNum, ProductionNum, PlanFinishTime"
                + " , IsSpread, LastChangeTime, ChangeTimes, IsDel"
                + " , (select top 1 case when IsGetBOM = 'true' then AreaCode else '' end from P_Pack left join Sys_Model on Sys_Model.Id = P_Pack.Model left join Sys_Area on Sys_Area.Id = Sys_Model.AreaId where PackId = '" + PackId + "') as AreaCode"
                + " from P_Pack_Task where PackID = '" + PackId +"' and TaskId='"+ TaskID+"' and IsDel = 'false'";
             dt = DBI.Execute(strSQL, true);

             

          //  for (int j = 0; j < dt.Rows.Count; j++)
          //  {
            //    MDDLD.Drawing_No = dt.Rows[0]["TaskDrawingCode"].ToString();
                MDDLD.Import_Date = DateTime.Now;
     
              //  MDDLD.NumCasesSum = Convert.ToDecimal(dt.Rows[0]["ProductionNum"].ToString());
                //MDDLD.Quantity = dt.Rows[0]["ProductionNum"].ToString();
             
               
                MDDLD.Stage = Convert.ToInt32(dt.Rows[0]["Stage"].ToString());
                MDDLD.StandAlone = Convert.ToInt32(dt.Rows[0]["MatingNum"].ToString()); //单机配套数量
                MDDLD.TaskCode = dt.Rows[0]["TaskCode"].ToString();

                MDDLD.TaskId = Convert.ToInt32(dt.Rows[0]["TaskID"].ToString());
                MDDLD.ThisTimeOperation = Convert.ToInt32(dt.Rows[0]["ProductionNum"].ToString()); //投产数量
                MDDLD.TDM_Description = dt.Rows[0]["ProductName"].ToString();//产品名称
                  GridItemCollection gridItems=null;
                  if (RadGridImport.SelectedItems.Count > 0)
                  {
                      gridItems = RadGridImport.SelectedItems;
                  }
                  else
                  {
                      gridItems = RadGridImport.Items;
                  }

                  for (int i = 0; i < gridItems.Count; i++)
                  {
                    if (gridItems[i] is GridDataItem)
                    {
                        GridDataItem item = gridItems[i] as GridDataItem;
                        MDDLD.Drawing_No = item["Drawing_No"].Text.Trim();
                        MDDLD.Technics_Line = item["Technics_Line"].Text.Trim();
                        MDDLD.Material_Name = item["Material_Name"].Text.Trim();
                        MDDLD.Material_Mark = item["Material_Mark"].Text.Trim();

                        MDDLD.CN_Material_State = item["CN_Material_State"].Text.Trim();
                        MDDLD.Material_Tech_Condition = item["Material_Tech_Condition"].Text.Trim();

                        MDDLD.Rough_Spec = item["Rough_Spec"].Text.Trim();
                        MDDLD.Rough_Size = item["ROUGH_SIZE"].Text.Trim();
                        MDDLD.Mat_Unit = item["MAT_UNIT"].Text.Trim();
                        MDDLD.Dinge_Size = item["Dinge_Size"].Text.Trim();

                        MDDLD.Mat_Rough_Weight = item["Mat_Rough_Weight"].Text.Trim();
                        try
                        {
                            Convert.ToDecimal(MDDLD.Mat_Rough_Weight);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "单件质量，请输入数字！";
                            RadNotificationAlert.Show();
                            return;
                        }

                        MDDLD.Mat_Pro_Weight = item["Mat_Pro_Weight"].Text.Trim();
                        try
                        {
                            Convert.ToDecimal(MDDLD.Mat_Pro_Weight);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "每产品质量，请输入数字！";
                            RadNotificationAlert.Show();
                            return;
                        }
                       
      
                        MDDLD.ItemCode1 = item["ItemCode1"].Text.Trim();
                        MDDLD.Dinge_Size = item["Dinge_Size"].Text.Trim();
                       
                        try
                        {
                            MDDLD.NumCasesSum = Convert.ToDecimal(item["NumCasesSum"].Text.Trim());

                        }
                        catch
                        {
                            RadNotificationAlert.Text = "需求件数，请输入数字！";
                            RadNotificationAlert.Show();
                            return;
                        }

                        MDDLD.Quantity = item["NumCasesSum"].Text.Trim();

                        try
                        {
                            MDDLD.DemandNumSum = Convert.ToDecimal(item["DemandNumSum"].Text.Trim());
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "需求数量（重量），请输入数字！";
                            RadNotificationAlert.Show();
                            return;
                        }
     



                        MDDLD.Technics_Comment = item["Technics_Comment"].Text.Trim();

                        MDDLD.Tech_Quantity = item["Tech_Quantity"].Text.Trim();
                        MDDLD.Mat_Comment = item["Mat_Comment"].Text.Trim();
                        MDDLD.Memo_Quantity = item["Memo_Quantity"].Text.Trim();

                        string DemandDate = item["DemandDate"].Text.Trim();

                        try
                        {

                            Convert.ToDateTime(DemandDate);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "需求日期，请输入正确的时间格式！";
                            RadNotificationAlert.Show();
                            return;
                        }
     
        

                        MDDLD.DemandDate = DemandDate;


                //RadTextBox RadTextBoxSpecial_Needs = item.FindControl("rtb_SpecialNeeds") as RadTextBox;
                   //     MDDLD.Special_Needs = RadTextBoxSpecial_Needs.Text.ToString();
          
                 
                        /*
                        RadComboBox RadDropDownListLingJian_Type = item.FindControl("RDDL_LingJian_Type") as RadComboBox;
                        MDDLD.LingJian_Type = RadDropDownListLingJian_Type.SelectedValue;


                        RadComboBox rcbAddr = item.FindControl("RadComboBoxShippingAddress") as RadComboBox;
                        MDDLD.Shipping_Address = rcbAddr.SelectedValue;

                        RadComboBox RadComboBoxUseDes = item.FindControl("RadComboBoxUseDes") as RadComboBox;
                        MDDLD.Use_Des = RadComboBoxUseDes.SelectedValue;

                        RadComboBox RadComboBoxSecretLevel = item.FindControl("RadComboBoxSecretLevel") as RadComboBox;
                        MDDLD.Secret_Level = RadComboBoxSecretLevel.SelectedValue;

                        RadComboBox RadComboBoxUrgencyDegree = item.FindControl("RadComboBoxUrgencyDegree") as RadComboBox;
                        MDDLD.Urgency_Degre = RadComboBoxUrgencyDegree.SelectedValue;

                        RadComboBox RadComboBoxCertification = item.FindControl("RadComboBoxCertification") as RadComboBox;
                        MDDLD.Certification = RadComboBoxCertification.SelectedValue;

                        RadComboBox RadComboBoxAttribute4 = item.FindControl("RadComboBoxAttribute4") as RadComboBox;
                        MDDLD.Attribute4 = RadComboBoxAttribute4.SelectedValue;
                         */
                        MDDLD.Special_Needs = item["Special_Needs"].Text.Trim();
                        string LingJian_Type = item["LingJian_Type"].Text.Trim();

                        strSQL = "select LingJian_Type_Code, LingJian_Type_Name from Sys_LingJian_Info where Is_Del = 'false'";
                        DataTable dtTemp = DBI.Execute(strSQL, true);
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (LingJian_Type == dtTemp.Rows[count]["LingJian_Type_Name"].ToString())
                            {
                                MDDLD.LingJian_Type = dtTemp.Rows[count]["LingJian_Type_Code"].ToString();
                                break;
                            }

                        }
                        if (MDDLD.LingJian_Type == null)
                        {
                            RadNotificationAlert.Text = "零件类型输入错误，正确的选项为：标准件,成品件,通用件,专用件,组件,其它，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                     

                        strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                                        " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                                        " where Sys_UserInfo_PWD.ID = '" + userid + "'";
                        dtTemp = DBI.Execute(strSQL, true);

                        string MaterialDept = dtTemp.Rows[0]["DeptCode"].ToString();

                         strSQL = "select KeyWord from Sys_Dict" + " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
                         " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + MaterialDept + "')";
                         dtTemp = DBI.Execute(strSQL, true);

                        MDDLD.Shipping_Address = item["Shipping_Address"].Text.Trim();
                        if (MDDLD.Shipping_Address != dtTemp.Rows[0]["KeyWord"].ToString())
                        {
                            RadNotificationAlert.Text = "配送地址输入不正确，正确的选项为：" + dtTemp.Rows[0]["KeyWord"].ToString();
                            RadNotificationAlert.Show();
                            return;

                        }

                      
                   
                     
                   
                        string Use_Des = item["Use_Des"].Text.Trim();

                        strSQL = " Select * From GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE' and ENABLED_FLAG='Y'";// +PackId + "'";
                        dtTemp = DBI.Execute(strSQL, true);
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (Use_Des == dtTemp.Rows[count]["DICT_NAME"].ToString())
                            {
                                MDDLD.Use_Des = dtTemp.Rows[count]["DICT_CODE"].ToString();
                                break;
                            }

                        }
                        if (MDDLD.Use_Des == null)
                        {
                            RadNotificationAlert.Text = "用途输入错误，正确的选项为：弹上/箭上,辅料,工装,中间料，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }

                        MDDLD.Secret_Level = item["Secret_Level"].Text.Trim();

                        strSQL = "SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)";
                        dtTemp = DBI.Execute(strSQL, true);
                        bool inputIsRight = false;
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (MDDLD.Secret_Level == dtTemp.Rows[count]["SecretLevel_Name"].ToString())
                            {
                                inputIsRight = true;
                                break;

                            }

                        }
                        if (!inputIsRight)
                        {
                            RadNotificationAlert.Text = "密级输入错误，正确的选项为：内部,秘密,机密，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                        string Urgency_Degre = item["Urgency_Degre"].Text.Trim();
                        strSQL = " Select * From GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL' and ENABLED_FLAG='Y'";// +PackId + "'";
                        dtTemp = DBI.Execute(strSQL, true);
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (Urgency_Degre == dtTemp.Rows[count]["DICT_NAME"].ToString())
                            {
                                MDDLD.Urgency_Degre = dtTemp.Rows[count]["DICT_CODE"].ToString();
                                break;
                            }

                        }
                        if( MDDLD.Urgency_Degre ==null)
                        {
                            RadNotificationAlert.Text = "紧急程度输入错误，正确的选项为：一般，急，特急，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                     
                     
                  

                        MDDLD.Certification = item["Certification"].Text.Trim();
                        if (MDDLD.Certification != "Y" && MDDLD.Certification != "N")
                        {
                            RadNotificationAlert.Text = "合格证列输入错误，正确的选项为：Y,N，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                        MDDLD.Attribute4 = item["Attribute4"].Text.Trim();
                        if (MDDLD.Attribute4 != "国产" && MDDLD.Attribute4 != "进口")
                        {
                            RadNotificationAlert.Text = "国产/进口列输入错误，正确的选项为：国产,进口，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                        MDDLD.Manufacturer = item["MANUFACTURER"].Text.Trim();
                  
                    
                      //  RadComboBox RadComboBoxMaterialDept = item.FindControl("RadComboBoxMaterialDept") as RadComboBox;
                        MDDLD.MaterialDept = RadComboBox_Dept.SelectedValue;

                    
                        try
                        {
                            if (this.hfBh.Value != null && this.hfBh.Value != "")
                            {
                                MDPID = Convert.ToInt32(this.hfBh.Value);
                            }
                            else
                            {
                                strSQL = @"exec Proc_Add_M_Demand_Plan_List_Technology " + userid + ",'" + MDDLD.TaskCode + "'," + PackId + "," + DraftID + "," + this.ViewState["submit_type"].ToString() + ",0,''";
                                DataTable dt1 = DBI.Execute(strSQL, true);
                                if (dt1.Rows.Count == 1)
                                {
                                    MDPID = Convert.ToInt32(dt1.Rows[0][0].ToString());
                                    this.hfBh.Value = dt1.Rows[0][0].ToString();
                                }
                                else
                                {
                                    RadNotificationAlert.Text = "失败！Proc_Add_M_Demand_Plan_List_Technology返回的记录数不唯一";
                                    RadNotificationAlert.Show();
                                    return;
                                }
                           }
                           MDDLD.MDPId = MDPID;


                          strSQL = " Insert Into M_Demand_DetailedList_Draft ( PackId, DraftId,taskid,MDPId,Quantity,VerCode,Stage,DemandDate,Special_Needs,MaterialDept,"
                              + " TaskCode,Drawing_No, Mat_Pro_Weight,Material_State, Combine_State,Material_Name, Material_Mark, CN_Material_State,ItemCode1, Mat_Unit, Mat_Rough_Weight,"
                              + "Rough_Size,Dinge_Size, Rough_Spec, DemandNumSum, NumCasesSum,Material_Tech_Condition,Material_Code,TDM_Description,Technics_Line,LingJian_Type,"
                              + "Import_Date,User_ID,Urgency_Degre ,Secret_Level ,Use_Des ,Shipping_Address ,Certification  , Manufacturer , Attribute4 ,"
                              + "Tech_Quantity,Technics_Comment,Memo_Quantity,Mat_Comment,"
                              + "ParentId,ParentId_For_Combine,Is_del)" + " Values ("
                              + MDDLD.PackId + "," + DraftID + "," + MDDLD.TaskId + "," + MDDLD.MDPId + "," + MDDLD.Quantity + "," + MDDLD.VerCode + "," 
                              + MDDLD.Stage + ",'" + MDDLD.DemandDate + "','" + MDDLD.Special_Needs + "','" + MDDLD.MaterialDept + "','" + MDDLD.TaskCode + "','"
                              + MDDLD.Drawing_No + "'," + MDDLD.Mat_Pro_Weight + ",0,0,'" + MDDLD.Material_Name + "','" + MDDLD.Material_Mark + "','" + MDDLD.CN_Material_State + "','"
                              + MDDLD.ItemCode1 + "','" + MDDLD.Mat_Unit + "'," + MDDLD.Mat_Rough_Weight + ",'" + MDDLD.Rough_Size + "','" + MDDLD.Dinge_Size + "','" + MDDLD.Rough_Spec + "',"
                              + MDDLD.DemandNumSum + "," + MDDLD.NumCasesSum + ",'" + MDDLD.Material_Tech_Condition + "','" + MDDLD.Material_Code + "','"
                              + MDDLD.TDM_Description + "','" + MDDLD.Technics_Line + "','" + MDDLD.LingJian_Type + "','" + MDDLD.Import_Date + "'," + userid + ",'"
                              + MDDLD.Urgency_Degre + "','" + MDDLD.Secret_Level + "','" + MDDLD.Use_Des + "','" + MDDLD.Shipping_Address + "','" + MDDLD.Certification + "','"
                              + MDDLD.Manufacturer + "','" + MDDLD.Attribute4 + "','" 
                              + MDDLD.Tech_Quantity + "','" + MDDLD.Technics_Comment + "','" + MDDLD.Memo_Quantity + "','" + MDDLD.Mat_Comment + "',"
                              +"0,0,0)" + " select @@identity";
                            DBI.GetSingleValue(strSQL);
                            // DBI.Execute(strSQL);
                        }
                        catch (Exception e1)
                        {
                            RadNotificationAlert.Text = "导入失败！第" + (i + 1).ToString() + "行，发生错误" + e1.Message.ToString();
                            RadNotificationAlert.Show();
                            return;
                        }
                    }
                }
          //  }
                  GridSource = Common.AddTableRowsID(GetDetailedListList());
                  RadGrid_DemandDetailedList.Rebind();

                  //  RadBtnSubmit.Visible = true;
                    RadNotificationAlert.Text = "导入成功！";
                    RadNotificationAlert.Show();
            }
            /*catch (Exception ex)
            {
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                return;
            }
             */
        }


  
        protected void btnDown_Click(object sender, EventArgs e)
        {
            int i = 0;
            DirectoryInfo info =
                new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Plan/物资导入模板");
            if (System.IO.Directory.Exists(info.ToString()))
            {
                foreach (FileInfo n in info.GetFiles())
                {
                    if (n.Name == "型号物资导入模板.xlsx")
                    {
                        i = 1;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(n.Name));
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                        Response.ContentType = "application/ms-excel";
                        this.EnableViewState = false;

                        Response.WriteFile(Server.MapPath(@"~\Plan\物资导入模板\") + n.Name);
                        Response.End();
                        return;
                    }
                }
            }
            if (i == 0)
            {
                RadNotificationAlert.Text = "没有找到模版";
                RadNotificationAlert.Show();
            }
        }




       protected void RadComboBoxMaterialDept_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString(); ;
            RadComboBox RadComboBoxShipping_Address = (cb.Parent.Parent as GridDataItem).FindControl("RadComboBoxShippingAddress") as RadComboBox;
          
           string strSQL = "select KeyWord from Sys_Dict" +
               " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
               " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + cb.SelectedItem.Value + "')";
           DataTable dt = DBI.Execute(strSQL, true);
           RadComboBoxShipping_Address.Items.Clear();         
           RadComboBoxShipping_Address.DataSource = dt;
           RadComboBoxShipping_Address.DataTextField = "KeyWord";
           RadComboBoxShipping_Address.DataValueField = "KeyWord";
           RadComboBoxShipping_Address.DataBind();
        }

  
  
        private void BindDeptUserAddress()
        {
            string userid = Session["UserId"].ToString();
            string strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                            " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                            " where Sys_UserInfo_PWD.ID = '" + userid + "'";
            DataTable dt = DBI.Execute(strSQL, true);

            RadComboBox_Dept.DataSource = dt;
            RadComboBox_Dept.DataTextField = "Dept";
            RadComboBox_Dept.DataValueField = "DeptCode";
            RadComboBox_Dept.DataBind();

        

            RadComboBox_User.DataSource = dt;
            RadComboBox_User.DataTextField = "UserName";
            RadComboBox_User.DataValueField = "UserID";
            RadComboBox_User.DataBind();


        }

        private void BindDDlUserInfo(string Dept_Id)
        {
            RadComboBox_User.ClearSelection();
            string strSQL = "SELECT * FROM [Sys_UserInfo_PWD] where Dept='" + Dept_Id + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            RadComboBox_User.DataSource = dt;
            RadComboBox_User.DataTextField = "UserName";
            RadComboBox_User.DataValueField = "Id";
            RadComboBox_User.DataBind();
        }
        protected void RadComboBox_Dept_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (RadComboBox_Dept.SelectedValue != "0")
            {
                BindDDlUserInfo(RadComboBox_Dept.SelectedValue);
            }
        }
   
    }
}