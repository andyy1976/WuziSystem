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
using System.Drawing;
namespace mms.Plan
{
    public partial class MDemandDetails : System.Web.UI.Page
    {
        //初始化Grid数据源
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
                    InitTable.Columns.Add("VerCode");
                    InitTable.Columns.Add("CLASS_ID");
                    InitTable.Columns.Add("OBJECT_ID");
                    InitTable.Columns.Add("STAGE");
                    InitTable.Columns.Add("MATERIAL_STATE");
                    InitTable.Columns.Add("MATERIAL_TECH_CONDITION");
                    InitTable.Columns.Add("MATERIAL_CODE");
                    InitTable.Columns.Add("ParentId");
                    InitTable.Columns.Add("MATERIAL_NAME");
                    InitTable.Columns.Add("TASK_NUM");
                    InitTable.Columns.Add("DRAWING_NO");
                    InitTable.Columns.Add("TECHNICS_LINE");
                    InitTable.Columns.Add("ItemCode1");
                    InitTable.Columns.Add("MaterialsNum");
                    InitTable.Columns.Add("MAT_UNIT");
                    InitTable.Columns.Add("LINGJIAN_TYPE");
                    InitTable.Columns.Add("MAT_ROUGH_WEIGHT");
                    InitTable.Columns.Add("MAT_PRO_WEIGHT");
                    InitTable.Columns.Add("MAT_WEIGHT");
                    InitTable.Columns.Add("MAT_EFFICIENCY");
                    InitTable.Columns.Add("MAT_COMMENT");
                    InitTable.Columns.Add("MAT_TECHNICS");
                    InitTable.Columns.Add("ROUCH_SPEC");
                    InitTable.Columns.Add("ROUGH_SIZE");
                    InitTable.Columns.Add("MaterialsDes");
                    InitTable.Columns.Add("StandAlone");
                    InitTable.Columns.Add("ThisTimeOperation");
                    InitTable.Columns.Add("PredictDeliveryDate");
                    InitTable.Columns.Add("DemandNumSum");
                    InitTable.Columns.Add("NumCasesSum");
                    InitTable.Columns.Add("DemandDate");
                    InitTable.Columns.Add("Quantity");
                    InitTable.Columns.Add("SparePart_num");
                    InitTable.Columns.Add("Process_num");
                    InitTable.Columns.Add("CanonicalForm_num");
                    InitTable.Columns.Add("MustChangePart_num");
                    InitTable.Columns.Add("Other");
                    InitTable.Columns.Add("Import_Date");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("JSGS_Des");
                    InitTable.Columns.Add("Is_del");
                    InitTable.Columns.Add("TaskCode");
                    InitTable.Columns.Add("MaterialDept");
                    InitTable.Columns.Add("MissingDescription");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("ParentId_For_Combine");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["ID"] };       //设置RowsId列为主键，用于datatable删除
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
        //public string[] IdArr;
       // public string itemCodeStr=",";
        protected void Page_Load(object sender, EventArgs e)
        {
        //    ((ScriptManager)Master.FindControl("ScriptManager1")).RegisterPostBackControl(RadBtnCombineMergeList);  
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "MDemandDetails", this.Page); 

                string PackId = "";
                string DraftCode = "";
                string draftid = "";
                string Model = "";
                string PlanCode = "";
                  if(Request.QueryString["PackId"]!=null && Request.QueryString["PackId"].ToString()!= "")
                {
                    PackId = Request.QueryString["PackId"].ToString();

                    string strSQL = " Select * From V_M_Draft_List where packid='" + PackId + "'";
                    DataTable dt = DBI.Execute(strSQL, true);

                    DraftCode = dt.Rows[0]["DraftCode"].ToString();
                    draftid = dt.Rows[0]["draftid"].ToString();
                    Model = dt.Rows[0]["model_1"].ToString();
                    PlanCode = dt.Rows[0]["PlanCode"].ToString();
                  

                    this.span_DraftCode.InnerText = DraftCode;
                    this.span_model.InnerText = Model;
                    this.span_plancode.InnerText = PlanCode;
                    this.span_PlanName.InnerText = dt.Rows[0]["PlanName"].ToString();
                    GridSource = Common.AddTableRowsID(GetDetailedListList());
                    this.ViewState["DraftId"] = draftid;
                    this.ViewState["DraftCode"] = DraftCode;
                    this.ViewState["PackId"] = PackId;
                    this.ViewState["Model"] = Model;
                    GetMaterialStateSum(DraftCode);
                    this.ViewState["flag"] = false;
                    this.hfFlag.Value = "0";
                    RadTabStrip1.Tabs[1].NavigateUrl = "MDemandMergeListChange.aspx?PackId=" + PackId;

                    Session["idStr"] = ",";
                    Session["otherStr"] = PackId + "," + draftid + "," + Model + "," + DraftCode;
                }
                //设置上一次部门过滤选择状态
                this.ViewState["lastSelectDeptCode"] = "";
                this.ViewState["lastSelectAccount"] = "";
            }
        }

        /// <summary>
        /// 获得物资需求提交状态  物资提交状态，0－未提交，1－已提交，2－已提交有更改可再次提交，3-有子节点，
        /// 4－缺失材料定额数据，5－错误数据类型，6－已提交有更改不可再次提交
        /// </summary>
        protected void GetMaterialStateSum(string DraftCode)
        {
            try
            {
                string strSQL = @"exec Proc_Sel_Material_State_Sum '" + DraftCode + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                lbl_state_0.InnerText = dt.Rows[0][0].ToString();
                lbl_state_1.InnerText= dt.Rows[0][1].ToString();
                lbl_state_2.InnerText = dt.Rows[0][2].ToString();
                lbl_state_4.InnerText = dt.Rows[0][3].ToString();
                lbl_state_5.InnerText = dt.Rows[0][4].ToString();
                lbl_state_6.InnerText = dt.Rows[0][5].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("保存物资需求清单详表合并数据出错" + ex.Message.ToString());
            }
        }

        protected DataTable GetDetailedListByItemCode(string ItemCode)
        {
            try
            {
                string strSQL =
                    " select * , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                    " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1" +
                    " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine = 0 " + " and ItemCode1='" + ItemCode + "' and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))" +
                    " union all select *, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
                    " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine = 0  " + " and ItemCode1='" + ItemCode + "' and is_del = 'false' and Material_State = '0' order by ID";

                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }

        protected DataTable GetDetailedListList()
        {
            try
            {
                string strSQL =
                    " select * , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                    " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1" +
                    " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))" +
                    " union all select *, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
                    " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and is_del = 'false' and Material_State = '0' order by ID";

                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }

        protected DataTable GetDetailedListByCombineParent(string ParentId_For_Combine)
        {
            try
            {
                string strSQL =
                    "select *, 'false' as checked, '已合并' as mstate, '0' as quantity1" +
                    " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine=" +ParentId_For_Combine+ " and is_del = 'false' and Material_State = '9' order by ID";

                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_MDemandDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!e.IsFromDetailTable)
            {
                RadGrid_MDemandDetails.DataSource = GridSource;
            }
        }

        protected void BuildMDemandMergeList(string idStr,string dateStr)
        {
            int userid = Convert.ToInt32(Session["UserId"].ToString());
            Session["idStr"] = idStr;
            Session["dateStr"] = dateStr;
            Session["otherStr"] = this.ViewState["PackId"].ToString() + "," + this.ViewState["DraftId"].ToString() + "," +
            this.ViewState["Model"].ToString() + "," + this.ViewState["DraftCode"].ToString();
            Response.Redirect("~/Plan/MDemandMergeList.aspx");
        }
        protected bool ValidMaterialState(string Id)
        {
            try
            {
                string strSQL = @"select Material_State from V_M_Demand_DetailedList_Draft where Id='"+Id+"'";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows[0][0].ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("查询物资需求清单详表数据出错" + ex.Message.ToString());
            }
        }
        
        /// <summary>
        /// 数据库操作-更新需求时间
        /// M_Demand_DetailedList_DraftBody M_Demand_DetailedList_DraftBody
        /// </summary>
        protected void UpdateDemandDate(string MDID, DateTime DemandDate)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = @"UPDATE [dbo].[M_Demand_DetailedList_Draft] SET [DemandDate] = '" + DemandDate + "' WHERE [ID] = '" + MDID + "'";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("数据库操作-更新需求时间时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        protected class M_Demand_DetailedList_DraftBody
        {
            public int ID { get; set; }
            public string VerCode { get; set; }
            public string CLASS_ID { get; set; }
            public string OBJECT_ID { get; set; }
            public string STAGE { get; set; }
            public string MATERIAL_STATE { get; set; }
            public string MATERIAL_TECH_CONDITION { get; set; }
            public string MATERIAL_CODE { get; set; }
            public string ParentId { get; set; }
            public string MATERIAL_NAME { get; set; }
            public string TASK_NUM { get; set; }
            public string DRAWING_NO { get; set; }
            public string TECHNICS_LINE { get; set; }
            public string ItemCode1 { get; set; }
            public string MaterialsNum { get; set; }
            public string MAT_UNIT { get; set; }
            public string LINGJIAN_TYPE { get; set; }
            public string MAT_ROUGH_WEIGHT { get; set; }
            public string MAT_PRO_WEIGHT { get; set; }
            public string MAT_WEIGHT { get; set; }
            public string MAT_EFFICIENCY { get; set; }
            public string MAT_COMMENT { get; set; }
            public string MAT_TECHNICS { get; set; }
            public string ROUCH_SPEC { get; set; }
            public string ROUGH_SIZE { get; set; }
            public string MaterialsDes { get; set; }
            public string StandAlone { get; set; }
            public string ThisTimeOperation { get; set; }
            public string PredictDeliveryDate { get; set; }
            public string DemandNumSum { get; set; }
            public string NumCasesSum { get; set; }
            public string DemandDate { get; set; }
            public string Product_num { get; set; }
            public string SparePart_num { get; set; }
            public string Process_num { get; set; }
            public string CanonicalForm_num { get; set; }
            public string MustChangePart_num { get; set; }
            public string Other { get; set; }
            public string Import_Date { get; set; }
            public string User_ID { get; set; }
        }


        protected void RadGrid_MDemandDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
           
            if (e.Item is GridDataItem)   
            
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                CheckBox cb = e.Item.FindControl("CheckBox1") as CheckBox;
                if (cb != null)
                {
                    if (GridSource.Select("ID='" + id + "'")[0]["checked"].ToString().ToLower() == "true")
                    {
                        cb.Checked = true;
                        e.Item.Selected = true;
                    }
                }
                string parentId_For_Combine = (e.Item as GridDataItem).GetDataKeyValue("ParentId_For_Combine").ToString();
                string combine_State = (e.Item as GridDataItem).GetDataKeyValue("Combine_State").ToString();
                if (parentId_For_Combine == "0" && combine_State=="1")
                {
                    e.Item.ForeColor = Color.Red;
                }
                else
                {
                    e.Item.ForeColor = Color.Green;
                }
               
            }
        }

        protected void RadGrid1_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
     
           // switch (e.DetailTableView.Name)
         //   {
              //  case "Child1":
                  //  {
                        string ParentId_For_Combine = dataItem.GetDataKeyValue("ID").ToString();
                        e.DetailTableView.DataSource = GetDetailedListByCombineParent(ParentId_For_Combine);
                      //  break;
                 //   }

      
          //  }
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              //  RadGrid_MDemandDetails.MasterTableView.Items[0].Expanded = true;
           //     RadGrid_MDemandDetails.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
            }
        }

        protected void WZBH_Query_Click(object sender, EventArgs e)
        {
            string ItemCode = "";
            if (this.RTB_ItemCode.Text.Trim() == "" )
            {
                RadNotificationAlert.Text = "请输入查询条件！";
                RadNotificationAlert.Show();
            }
            else
            {

                ItemCode = this.RTB_ItemCode.Text.Trim();


                GridSource = GetDetailedListByItemCode(ItemCode);
                RadGrid_MDemandDetails.DataSource = GridSource;
                RadGrid_MDemandDetails.Rebind();
            }

        }

        protected void RadBtn_Search_Click(object sender, EventArgs e)
        {
            string DraftCode = "";
            if (this.RadTxt_DraftCode.Text.Trim() == "" && this.span_DraftCode.InnerText == "") {
                RadNotificationAlert.Text = "请输入查询条件！";
                RadNotificationAlert.Show();
            }
            else {
                if (this.RadTxt_DraftCode.Text.Trim() != "" && this.span_DraftCode.InnerText != "")
                    DraftCode = this.RadTxt_DraftCode.Text.Trim();
                else if (this.RadTxt_DraftCode.Text.Trim() == "")
                    DraftCode = this.span_DraftCode.InnerText;
                else if (this.span_DraftCode.InnerText == "")
                    DraftCode = this.RadTxt_DraftCode.Text.Trim();
                GetMDraftList(DraftCode);
                GetMaterialStateSum(DraftCode);
                string state = this.ddlMState.SelectedValue;
                GridSource = GetDetailedListList();
                RadGrid_MDemandDetails.DataSource = GridSource;
                RadGrid_MDemandDetails.Rebind();
            }

        }

        protected void confirmWindowSubmitCombineServer(object sender, EventArgs e)
        {




        }
        protected void GetMDraftList(string DraftCode)
        {
            try
            {
                string strSQL = "select * from V_M_Draft_List where DraftCode='" + DraftCode + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count > 0) {
                    this.span_DraftCode.InnerText = DraftCode;
                    this.ViewState["DraftId"] = dt.Rows[0][6].ToString();
                    this.ViewState["DraftCode"] = DraftCode;
                    this.ViewState["PackId"] = dt.Rows[0][7].ToString();
                    this.ViewState["Model"] = dt.Rows[0][8].ToString();
                    this.span_model.InnerText = dt.Rows[0][8].ToString();
                    this.span_plancode.InnerText = dt.Rows[0][9].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单草稿信息出错" + ex.Message.ToString());
            }
        }
        public void GetMergeParameter(string idStr, string dateStr)
        {
            Session["idStr"] = idStr == "" ? idStr : idStr.Substring(0, idStr.Length - 1);
            Session["dateStr"] = dateStr == "" ? dateStr : dateStr.Substring(0, dateStr.Length - 1);
            Session["otherStr"] = this.ViewState["PackId"].ToString() + "," + this.ViewState["DraftId"].ToString() + "," +
                this.ViewState["Model"].ToString() + "," + this.ViewState["DraftCode"].ToString();
        }
        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string id = cb.CssClass.ToString();
            if (Session["idStr"].ToString().Substring(0,1) != ",") { Session["idStr"] = "," + Session["idStr"].ToString() + ","; }
            if (cb.Checked == true)
            {
                GridSource.Select("ID='" + id + "'")[0]["checked"] = "true";
                (cb.Parent.Parent as GridDataItem).Selected = true;
                Session["idStr"] += id + ",";
              
            }
            else
            {
                GridSource.Select("ID='" + id + "'")[0]["checked"] = "false";
                (cb.Parent.Parent as GridDataItem).Selected =false;
                if (Session["idStr"].ToString().IndexOf("," + id + ",") != -1)
                {
                    Session["idStr"] = Session["idStr"].ToString().Replace("," + id + ",", ",");
                }
              
            }
          //  sethfFlag();
        }
        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (Session["idStr"].ToString().Substring(0, 1) != ",") { Session["idStr"] = "," + Session["idStr"].ToString() + ","; }

            foreach (GridDataItem dataitem in RadGrid_MDemandDetails.MasterTableView.Items)
            {
                (dataitem.FindControl("CheckBox1") as CheckBox).Checked = cb.Checked;
                string id = dataitem.GetDataKeyValue("ID").ToString();
                GridSource.Select("ID='" + id + "'")[0]["checked"] = cb.Checked.ToString().ToLower();
                dataitem.Selected = cb.Checked;
                if (cb.Checked == true)
                {
                    if (Session["idStr"].ToString().IndexOf("," + id + ",") == -1)
                    {
                        Session["idStr"] += id + ",";
                    }
                  
                }
                else
                {
                    if (Session["idStr"].ToString().IndexOf("," + id + ",") != -1)
                    {
                        Session["idStr"] = Session["idStr"].ToString().Replace("," + id + ",", ",");
                    }

                }
            }
            //sethfFlag();
        }

        protected void sethfFlag()
        {
            if (GridSource.Select("checked='true'").Length == 0)
            {
                this.hfFlag.Value = "0";
            }
            else
            {
                this.hfFlag.Value = "1";
            }
        }

        protected void RB_Combine_Cancel_Click(object sender, EventArgs e)
        {
            GridDataItem[] dataItems=RadGrid_MDemandDetails.MasterTableView.GetSelectedItems();
            if (dataItems.Length == 1)
            {
                if (dataItems[0].GetDataKeyValue("Combine_State").ToString()== "1")
                {

                    string id = dataItems[0].GetDataKeyValue("ID").ToString();

                

                    string strSQL = " Update M_Demand_DetailedList_Draft set ParentId_For_Combine = 0 , Material_State = 0" + " where ParentId_For_Combine=" + id;
                    DBI.Execute(strSQL);
                    strSQL = " Update M_Demand_DetailedList_Draft set Material_State =10,Combine_State=2 where Id=" + id;
                    DBI.Execute(strSQL);

                    GridSource = GetDetailedListList();
                    RadGrid_MDemandDetails.DataSource = GridSource;
                    RadGrid_MDemandDetails.Rebind();
            

                }
            }
        }
          
        protected void RadBtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Plan/ShowPlan.aspx");
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                Response.Redirect("~/Plan/MDemandMergeListChange.aspx?PackId=" + Request.QueryString["PackId"].ToString());
            }
            if (e.Argument == "Rebind1")
            {
                Response.Redirect("/Plan/MDemandDetails.aspx?PackId=" + Request.QueryString["PackId"].ToString());
            }
            else
            {
                throw new Exception("刷新页面出错，请联系管理员！");
            }

        }

        private string[] GetAllNoSubmitId() {
            string[] res = new string[3];
            try
            {
                string strSQL = @"Select id From M_Demand_DetailedList_Draft where (Material_State=0 or Material_State=2) and is_del=0 and PackId='" + this.ViewState["PackId"].ToString() + "'";
                DataTable dt=DBI.Execute(strSQL,true);
                if (dt.Rows.Count > 0) {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        res[0] += dt.Rows[i]["Id"].ToString() + ",";
                        res[1] += DateTime.Now.ToShortDateString() + ",";
                    }
                    res[2] = this.ViewState["PackId"].ToString() + "," + this.ViewState["DraftId"].ToString() + "," +
                        this.ViewState["Model"].ToString() + "," + this.ViewState["DraftCode"].ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("数据库操作-获取数据时出现异常" + e.Message.ToString());
            }
            return res;
        }
        protected void chb_all_CheckedChanged(object sender, EventArgs e)
        {
            Session["idStr"] = ",";

            if (chb_all.Checked)
            {
                this.chb_all.Text = "反选";
                this.RadGrid_MDemandDetails.Columns[0].Visible = false;

                for (int i = 0; i < GridSource.Rows.Count; i++)
                {
                    GridSource.Rows[i]["checked"] = "true";
                    Session["idStr"] += GridSource.Rows[i]["ID"].ToString() + ",";
                }
                RadGrid_MDemandDetails.Rebind();
            }
            else {
                this.chb_all.Text = "全选";
                this.RadGrid_MDemandDetails.Columns[0].Visible = true;

                for (int i = 0; i < GridSource.Rows.Count; i++)
                {
                    GridSource.Rows[i]["checked"] = "false";
                }
                RadGrid_MDemandDetails.Rebind();
            }
            sethfFlag();
        }

    
    }
}