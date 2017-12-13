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
    public partial class MDemandDetailsTreeList : System.Web.UI.Page
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
                    InitTable.Columns.Add("DINGE_SIZE");
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
                string strSQL = " select M_Demand_DetailedList_Draft.* , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                  " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1, " +

                  "CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes ,"+
               //  " GetCustInfo_T_ACCT_SITE.ADDRESS as ADDRESS, " +
              //    "case when Stage ='1' then 'M' when stage='2' then 'C' when Stage='3' then 'S' when Stage='4' then 'D' else Convert(nvarchar(50),Stage) end as Stage1, " +

                  "case when LingJian_Type='1' then '标准件' else case when LingJian_Type='2' then '成品件'  else case when LingJian_Type='3' then '通用件' else case when LingJian_Type='4' then '专用件' else case when LingJian_Type='5' then '组件'   else '其它' end  end end end end as LingJian_Type1 from M_Demand_DetailedList_Draft " +

                  " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_DetailedList_Draft.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                  " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_DetailedList_Draft.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'";
                    //   " left join GetCustInfo_T_ACCT_SITE on Convert(nvarchar(50),GetCustInfo_T_ACCT_SITE.LOCATION_ID) = M_Demand_DetailedList_Draft.Shipping_Address";

                  strSQL += " where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine = 0 " + " and ItemCode1 like '%" + ItemCode + "%' and Drawing_No like '%" + drawingNum + "%' and Technics_Line like '%" + techline + "%' and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))";

                if (lingjiantype == "6")
                {
                    strSQL += " and LingJian_Type not in (1,2,3,4,5)";
                }
                else
                {
                    strSQL += " and LingJian_Type like '%" + lingjiantype + "%'";
                }



                strSQL += " union all select M_Demand_DetailedList_Draft.*, 'false' as checked, '未提交' as mstate, '0' as quantity1, " +

                  "CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes ," +
                    //"GetCustInfo_T_ACCT_SITE.ADDRESS as ADDRESS, " +
                    //     "case when Stage ='1' then 'M' when stage='2' then 'C' when Stage='3' then 'S' when Stage='4' then 'D' else Convert(nvarchar(50),Stage) end as Stage1, " +

                  "case when LingJian_Type='1' then '标准件' else case when LingJian_Type='2' then '成品件'  else case when LingJian_Type='3' then '通用件' else case when LingJian_Type='4' then '专用件' else case when LingJian_Type='5' then '组件'   else '其它' end  end end end end as LingJian_Type1 from M_Demand_DetailedList_Draft " +

                  " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_DetailedList_Draft.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                  " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_DetailedList_Draft.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'";
              //    " left join GetCustInfo_T_ACCT_SITE on Convert(nvarchar(50),GetCustInfo_T_ACCT_SITE.LOCATION_ID) = M_Demand_DetailedList_Draft.Shipping_Address";

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

        protected void RadTreeList1_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            RadTreeList1.DataSource = GridSource;
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
        


        protected void RadTreeList_MDemandDetails_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
           
            if (e.Item is TreeListDataItem)   
            
            {
                TreeListDataItem item = e.Item as TreeListDataItem;

                if (item["ParentId_For_Combine"].Text == "0" && item["Combine_State"].Text == "1")
                {
                        item.ForeColor = Color.Red;
                }
            }
        }

        protected void RadTreeList1_ChildItemsDataBind(object source, TreeListChildItemsDataBindEventArgs e)
        {
            string  ParentId_For_Combine = e.ParentDataKeyValues["ID"].ToString();
            e.ChildItemsDataSource = GetDetailedListByCombineParent(ParentId_For_Combine);
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
                RadTreeList1.DataSource = GridSource;
                RadTreeList1.Rebind();
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
                if ((cb.Parent.Parent as TreeListDataItem)["ParentId_For_Combine"].Text != "0")
                {
                    cb.Checked = false;
                    (cb.Parent.Parent as TreeListDataItem).Selected = false;
                    RadNotificationAlert.Text = "请勿选择已被合并的数据记录";
                    RadNotificationAlert.Show();

                }
                else
                {
                    cb.Checked = true;
                   (cb.Parent.Parent as TreeListDataItem).Selected = true;
                    Session["idStr"] += id + ",";
                }

            }
            else
            {
                cb.Checked = false;
                (cb.Parent.Parent as TreeListDataItem).Selected = false;
                if (Session["idStr"].ToString().IndexOf("," + id + ",") != -1)
                {
                    Session["idStr"] = Session["idStr"].ToString().Replace("," + id + ",", ",");
                }
              
            }

        }
        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (Session["idStr"].ToString().Substring(0, 1) != ",") { Session["idStr"] = "," + Session["idStr"].ToString() + ","; }

            foreach (TreeListDataItem dataitem in RadTreeList1.Items)
            {
                (dataitem.FindControl("CheckBox1") as CheckBox).Checked = cb.Checked;
                string id = dataitem.GetDataKeyValue("ID").ToString();
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

        }

        protected void RadTreeList1_SelectedIndexChanged(object sender, EventArgs e)
        {
                Session["idStr"] = ",";
    
            string id = null;
            foreach (TreeListDataItem dataitem in RadTreeList1.SelectedItems)
            {
                 id = dataitem.GetDataKeyValue("ID").ToString();
              
               
                    if (Session["idStr"].ToString().IndexOf("," + id + ",") == -1)
                    {
                        Session["idStr"] += id + ",";
                    }

               
            }

        }


        protected void RB_Combine_Cllick(object sender, EventArgs e)
        {

            if (RadTreeList1.SelectedItems.Count ==0)
            {
                   RadNotificationAlert.Text = "请选择要合并的数据";
                     RadNotificationAlert.Show();
            }
            else 
            {
                var combineparentid = RadTreeList1.SelectedItems[0]["ParentId_For_Combine"].Text;
               // var combineparentid = RadTreeList1.SelectedItems[0].GetDataKeyValue("ParentId_For_Combine").ToString();
                  if (combineparentid !="0") 
                  {
                         RadNotificationAlert.Text = "请务选择已经合并过的数据记录";
                         RadNotificationAlert.Show();
                   }     
                   else               
                   {
                       string temp = RadTreeList1.SelectedItems[0]["ItemCode1"].Text;
                      // var temp = RadTreeList1.SelectedItems[0].GetDataKeyValue("ItemCode1").ToString();
                        for (var i = 1; i < RadTreeList1.SelectedItems.Count; i++)
                        {
                            combineparentid = RadTreeList1.SelectedItems[i]["ParentId_For_Combine"].Text;
                            //combineparentid = RadTreeList1.SelectedItems[i].GetDataKeyValue("ParentId_For_Combine").ToString();
                            if (combineparentid != "0")
                            {
                                    RadNotificationAlert.Text = "请务选择已经合并过的数据记录";
                                     RadNotificationAlert.Show();
                                     return;
                            }
                            var itemCode1 = RadTreeList1.SelectedItems[i]["ItemCode1"].Text;
                           // var itemCode1 = RadTreeList1.SelectedItems[i].GetDataKeyValue("ItemCode1").ToString();
                            if (itemCode1 != temp) 
                            {
                                  RadNotificationAlert.Text = "请选择物资编码相同的数据";
                                  RadNotificationAlert.Show();
                                 return;
                            }
                      }
                      RadBtnCombineMergeList.Attributes["onclick"] = "return ShowMDemandCombineList()";
                  
                  }                     
                            
            }
        }


        protected void RB_Combine_Cancel_Click(object sender, EventArgs e)
        {

          /* 
           if (RadTreeList1.SelectedItems.Count == 0)
            {
                RadNotificationAlert.Text = "请选择一条以红颜色标记的合并过的数据记录";
                RadNotificationAlert.Show();
            }
            else if (RadTreeList1.SelectedItems.Count>1)
            {
                RadNotificationAlert.Text = "请勿选择两条（含）以上的数据记录";
                RadNotificationAlert.Show();
            }
           */
         //       var combineparentid = RadTreeList1.SelectedItems[0].GetDataKeyValue("ParentId_For_Combine").ToString();
        //        var combine_State = RadTreeList1.SelectedItems[0].GetDataKeyValue("Combine_State").ToString();
             //   if (combine_State == "1" && combineparentid == "0")
            if (RadTreeList1.SelectedItems.Count ==1)
            {              
                    string id = RadTreeList1.SelectedItems[0].GetDataKeyValue("ID").ToString();
                    
                    string strSQL = " Update M_Demand_DetailedList_Draft set ParentId_For_Combine = 0 , Material_State = 0" + " where ParentId_For_Combine=" + id;
                    DBI.Execute(strSQL);
                    strSQL = " Update M_Demand_DetailedList_Draft set Material_State =10,Combine_State=2 where Id=" + id;
                    DBI.Execute(strSQL);

                    GridSource = GetDetailedListByItemCode("", "","","");
                    Session["idStr"] = ",";
                    RadTreeList1.DataSource = GridSource;
                    RadTreeList1.Rebind();
            
                    
               }
             //   else
              //  {
                  //    RadNotificationAlert.Text = "请选择一条以红颜色标记的合并过的数据记录";
                 //     RadNotificationAlert.Show();
           //     }

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
            else if (e.Argument == "Rebind1")
            {
                Response.Redirect("/Plan/MDemandDetailsTreeList.aspx?PackId=" + Request.QueryString["PackId"].ToString());
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
                this.RadTreeList1.Columns[0].Visible = false;

                for (int i = 0; i < GridSource.Rows.Count; i++)
                {
                    GridSource.Rows[i]["checked"] = "true";
                    Session["idStr"] += GridSource.Rows[i]["ID"].ToString() + ",";
                }

                foreach (TreeListDataItem dataitem in RadTreeList1.Items)
                {
                    dataitem.Selected = true;
                }
                RadTreeList1.Rebind();
            }
            else
            {
                this.chb_all.Text = "全选";
                this.RadTreeList1.Columns[0].Visible = true;

                for (int i = 0; i < GridSource.Rows.Count; i++)
                {
                    GridSource.Rows[i]["checked"] = "false";
                }

                foreach (TreeListDataItem dataitem in RadTreeList1.Items)
                {
                    dataitem.Selected = false;
                }
                RadTreeList1.Rebind();
            }
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
       //   RadTreeList1.ExportSettings.FileName = "型号物资需求清单-" + DateTime.Now.ToString("yyyy-MM-dd");
           RadTreeList1.ExportSettings.FileName = "ModelMaterialRequirements-" + DateTime.Now.ToString("yyyy-MM-dd");
        //    RadTreeList1.ExportToExcel();
            RadTreeList1.Columns[0].Visible = false;
            //RadTreeList1.Columns[1].Visible = false;
            //RadTreeList1.Columns[2].Visible = false;
            RadTreeList1.ExportSettings.IgnorePaging = true;
          
           // RadTreeList1.ExportSettings.ExportMode = (TreeListExportMode)1;
          //  RadTreeList1.ShowFooter = false;
            //RadTreeList1.Rebind();
           
          //  RadTreeList1.ExportSettings.Excel.Format = (TreeListExcelFormat)Enum.Parse(typeof(TreeListExcelFormat), "Xlsx");
            RadTreeList1.ExportToExcel();
        } 
        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadTreeList1.ExportSettings.FileName = "型号物资需求清单" + DateTime.Now.ToString("yyyy-MM-dd");
            RadTreeList1.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadTreeList1.ExportSettings.FileName = "型号物资需求清单" + DateTime.Now.ToString("yyyy-MM-dd");
            RadTreeList1.ExportSettings.IgnorePaging = true;
            RadTreeList1.ExportToPdf();
        }
    }
}