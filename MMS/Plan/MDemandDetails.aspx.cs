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
                    InitTable.Columns.Add("Special_Needs");
                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Manufacturer");
                    InitTable.Columns.Add("DemandDateStr");
                    
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
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

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
                    strSQL = " select LingJian_Type_Code, LingJian_Type_Name from Sys_LingJian_Info where Is_Del = 'false'";
                    DataTable dtlingJianInfo = DBI.Execute(strSQL, true);
                    RDDL_LingJian_Type.DataSource = dtlingJianInfo;
                    RDDL_LingJian_Type.DataTextField = "LingJian_Type_Name";
                    RDDL_LingJian_Type.DataValueField = "LingJian_Type_Code";
                    RDDL_LingJian_Type.DataBind();

                    GridSource = Common.AddTableRowsID(GetDetailedListByItemCode("","","",""));
                    this.ViewState["DraftId"] = draftid;
                    this.ViewState["DraftCode"] = DraftCode;
                    this.ViewState["PackId"] = PackId;
                    this.ViewState["Model"] = Model;
                    GetMaterialStateSum(DraftCode);
                    this.ViewState["flag"] = false;
                    this.hfFlag.Value = "0";
                    RadTabStrip1.Tabs[1].NavigateUrl = "MDemandMergeListChange.aspx?PackId=" + PackId + "&fromPage=0";

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

        protected DataTable GetDetailedListByItemCode(string ItemCode, string lingjiantype, string drawingNum, string techline)
        {
            try
            {
                string strSQL = " select * , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                  " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1, " +
                  "case when LingJian_Type='1' then '标准件' else case when LingJian_Type='2' then '成品件'  else case when LingJian_Type='3' then '通用件' else case when LingJian_Type='4' then '专用件' else case when LingJian_Type='5' then '组件'   else '其它' end  end end end end as LingJian_Type1 from M_Demand_DetailedList_Draft ";

                strSQL += " where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine = 0 " + " and ItemCode1 like '%" + ItemCode + "%' and Drawing_No like '%" + drawingNum + "%' and Technics_Line like '%" + techline + "%' and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))";

                if (lingjiantype == "6")
                {
                    strSQL += " and LingJian_Type not in (1,2,3,4,5)";
                }
                else
                {
                    strSQL += " and LingJian_Type like '%" + lingjiantype + "%'";
                }
               


                strSQL += " union all select *, 'false' as checked, '未提交' as mstate, '0' as quantity1, "+
                         "case when LingJian_Type='1' then '标准件' else case when LingJian_Type='2' then '成品件'  else case when LingJian_Type='3' then '通用件' else case when LingJian_Type='4' then '专用件' else case when LingJian_Type='5' then '组件'   else '其它' end  end end end end as LingJian_Type1 from M_Demand_DetailedList_Draft ";

                strSQL += " where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine = 0  " + " and ItemCode1 like'%" + ItemCode + "%' and Drawing_No like '%" + drawingNum + "%' and Technics_Line like '%" + techline + "%' and is_del = 'false' and Material_State = '0'";
                if (lingjiantype == "6")
                {
                      strSQL += " and LingJian_Type not in (1,2,3,4,5)";
                }
                else
                {
                      strSQL += " and LingJian_Type like '%" + lingjiantype + "%'";
                }
                strSQL += "order by ID";
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
                    " select a.* , 'false' as checked, case when a.Is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                    " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), a.ID))) as quantity1, b.LingJian_Type_Name as LingJian_Type1";
                strSQL += " from M_Demand_DetailedList_Draft as a left join Sys_LingJian_Info as b on b.LingJian_Type_Code = a.LingJian_Type";
                strSQL += " where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and ((a.Is_del = 'false' and Material_State in ('2','7')) or (a.Is_del = 'true' and Material_State in ('1','2','6','7')))" +
                    " union all select a.*, 'false' as checked, '未提交' as mstate, '0' as quantity1,b.LingJian_Type_Name as LingJian_Type1";
                strSQL += " from M_Demand_DetailedList_Draft as a left join Sys_LingJian_Info as b on b.LingJian_Type_Code = a.LingJian_Type";
                strSQL += " where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and a.Is_del = 'false' and Material_State = '0'";

                strSQL += "order by a.ID";

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
            }
        }
        protected void WZBH_Query_Click(object sender, EventArgs e)
        {
            
           /* if (this.RTB_ItemCode.Text.Trim() == "" )
            {
                RadNotificationAlert.Text = "请输入查询条件！";
                RadNotificationAlert.Show();
            }
            else
            {
            */
    
                string ItemCode = this.RTB_ItemCode.Text.Trim();
                string lingjiantype = RDDL_LingJian_Type.SelectedItem.Value;
                string drawingNum = this.RTB_Drawing_No.Text.Trim();
                string techline = this.Rad_TechLine.Text.Trim();
                GridSource = GetDetailedListByItemCode(ItemCode, lingjiantype,drawingNum,techline);
                RadGrid_MDemandDetails.DataSource = GridSource;
                RadGrid_MDemandDetails.Rebind();
         //   }

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
            sethfFlag();
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
            sethfFlag();
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

        protected void RadGrid_MDemandDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
                Session["idStr"] = ",";
    
            string id = null;
            foreach (GridDataItem dataitem in RadGrid_MDemandDetails.SelectedItems)
            {
                 id = dataitem.GetDataKeyValue("ID").ToString();
              
               
                    if (Session["idStr"].ToString().IndexOf("," + id + ",") == -1)
                    {
                        Session["idStr"] += id + ",";
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
                Response.Redirect("~/Plan/MDemandMergeListChange.aspx?PackId=" + Request.QueryString["PackId"].ToString()+"&fromPage=0");
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
            else
            {
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

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandDetails.ExportSettings.FileName = "型号物资需求清单-" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandDetails.MasterTableView.ExportToExcel();
        }
        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandDetails.ExportSettings.FileName = "型号物资需求清单" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandDetails.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandDetails.ExportSettings.FileName = "型号物资需求清单" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandDetails.ExportSettings.IgnorePaging = true;
            RadGrid_MDemandDetails.MasterTableView.ExportToPdf();
            RadGrid_MDemandDetails.ExportSettings.IgnorePaging = false;
        }
    }
}