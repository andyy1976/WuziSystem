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

namespace mms.Plan
{
    public partial class MDemandDetailsCombine : System.Web.UI.Page
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
   //初始化Grid数据源
        private DataTable GridSource1
        {
            get
            {
                Object obj = this.ViewState["_gds1"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                else
                {
                    DataTable InitTable = new DataTable();
                    InitTable.Columns.Add("RowsId");
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("Correspond_Draft_Code");
                    InitTable.Columns.Add("Drawing_No");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("TaskId");
                    InitTable.Columns.Add("DraftId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("TechnicsLine");
                    InitTable.Columns.Add("ItemCode1");
                    InitTable.Columns.Add("NumCasesSum");
                    InitTable.Columns.Add("DemandNumSum");
                    InitTable.Columns.Add("Mat_Unit");
                    InitTable.Columns.Add("Quantity");
                    InitTable.Columns.Add("Rough_Size");
                    InitTable.Columns.Add("Rough_Spec");
                    InitTable.Columns.Add("MaterialsDes");
                    InitTable.Columns.Add("Special_Needs");
                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Stage");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Unit_Price");
                    InitTable.Columns.Add("Sum_Price");
                    InitTable.Columns.Add("Is_Submit");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("Submit_Date");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["ID"] };
                    this.ViewState["_gds1"] = InitTable;
                    return InitTable;
                }
            }
            set
            {
                this.ViewState["_gds1"] = value;
                ((DataTable)this.ViewState["_gds1"]).PrimaryKey = new DataColumn[] { ((DataTable)this.ViewState["_gds1"]).Columns["ID"] };
            }
        }

        private static string DBConn;
        private DBInterface DBI;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
               string PackId = "";
           
                string DraftCode = "";
                string draftid = "";
                string Model = "";
                string PlanCode = "";
                string idStr = string.Empty;
			    string dateStr = string.Empty;
                if(Request.QueryString["PackId"]!=null && Request.QueryString["PackId"].ToString()!= "")
                {
                    PackId = Request.QueryString["PackId"].ToString();
                   

                    string strSQL = " Select * From V_M_Draft_List where packid='" + PackId + "'";
                    DataTable dt = DBI.Execute(strSQL, true);

                    DraftCode = dt.Rows[0]["DraftCode"].ToString();
                    draftid = dt.Rows[0]["draftid"].ToString();
                    Model = dt.Rows[0]["model_1"].ToString();
                    PlanCode = dt.Rows[0]["PlanCode"].ToString();

                    if (Session["idStr"] != null && Session["idStr"].ToString() != "")
                    {
                        if (Session["idStr"].ToString().Substring(0, 1) == "," && Session["idStr"].ToString().Length > 2)
                        {
                            Session["idStr"] = Session["idStr"].ToString().Substring(1, Session["idStr"].ToString().Length - 2);
                        }
                        idStr = Session["idStr"].ToString();
                     
                        DataTable dt1 = GetDetailedListList(DraftCode, "", idStr);
                        GridSource = Common.AddTableRowsID(dt1);
                     //   DataTable dt2 = null;
                        DataTable dt2 = GetCombinedParentRecordList();
                        if (dt2.Rows.Count<=0)
                        {
                            dt2 = new DataTable();
                            dt2 = dt1.Clone();//克隆dt1的结构传递给dt2
                            DataRow dr = dt2.NewRow();
                            dr.ItemArray = dt1.Rows[0].ItemArray;
                            dt2.Rows.Add(dr);
                        }
                       

                       // GridSource1 = Common.AddTableRowsID(GetTempMergeList(idStr, PackId, draftid, dateStr));
                        GridSource1 = Common.AddTableRowsID(dt2);
                        this.ViewState["DraftId"] = draftid;
                        this.ViewState["DraftCode"] = DraftCode;
                        this.ViewState["PackId"] = PackId;
                        this.ViewState["Model"] = Model;
                        this.ViewState["flag"] = false;
             
                }  
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "CloseWindow1();", true);
                }
                  
                              
                }
      
            }
        }

    

        protected DataTable GetDetailedListList(string DraftCode,string state,string idStr)
        {
            try
            {
              /*  string itemCode1 = "";
                if (Request.QueryString["itemCode1"] != null && Request.QueryString["itemCode1"].ToString() != "")
                {
                    itemCode1 = Request.QueryString["itemCode1"].ToString();
                }
                   string strSQL =
             " select * , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
             " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1" +
             " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and (Id in (" + idStr + ")) " + " and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))" +
             " union all select *, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
             " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ((Id in (" + idStr + ")) or (ItemCode1='" + itemCode1 + "' and ParentId_For_Combine=1))" + " and is_del = 'false' and Material_State = '0' order by ID";
             */
                string strSQL =
                    " select * , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                    " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1" +
                    " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and (Id in (" + idStr + ")) "+" and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))" +
                    " union all select *, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
                    " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and (Id in (" + idStr + "))" + " and is_del = 'false' and Material_State = '0' order by ItemCode1";

          
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }

        protected DataTable GetCombinedParentRecordList()
        {
            try
            {
                string itemCode1 = "";
                if (Request.QueryString["itemCode1"] != null && Request.QueryString["itemCode1"].ToString() != "")
                {
                    itemCode1 = Request.QueryString["itemCode1"].ToString();
                }
              //  string strSQL =
              //      "select *, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
               //     " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ItemCode1='" + itemCode1 + "' and ParentId_For_Combine=1 and (Material_State =0 or Material_State=10) order by ID";
                string strSQL =
                "select top 1 *, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
                " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ItemCode1='" + itemCode1 + "' and Combine_State =2 order by ID";

                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }

        protected void RadGrid_MDemandDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MDemandDetails.DataSource = GridSource;
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
           /* if (e.Item is GridDataItem)
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
            */
        }

   protected DataTable GetTempMergeList(string idStr, string PackId, string DraftId, string dateStr)
        {
            try
            {
                string strSQL = @"exec Proc_Build_Merge_List '" + idStr + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }
      
	   protected void RadGrid_MDemandCombinelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MDemandCombinelist.DataSource = GridSource1;
        }
       
	   protected class M_Draft_ListBody
        {
            public int ID { get; set; }
            public string Correspond_Draft_Code { get; set; }
            public string Drawing_No { get; set; }
            public int PackId { get; set; }
            public int TaskId { get; set; }
            public int DraftId { get; set; }
            public int MDMId { get; set; }
            public int MDPId { get; set; }
            public string TechnicsLine { get; set; }
            public string ItemCode1 { get; set; }
            public decimal DemandNumSum { get; set; }
            public decimal NumCasesSum { get; set; }
            public string Mat_Unit { get; set; }
            public int Quantity { get; set; }
            public string DemandDate { get; set; }
            public string Rough_Size { get; set; }
            public string Rough_Spec { get; set; }
            public string MaterialsDes { get; set; }
            public int Special_Needs { get; set; }
            public int Urgency_Degre { get; set; }
            public int Secret_Level { get; set; }
            public int Stage { get; set; }
            public int Use_Des { get; set; }
            public string Shipping_Address { get; set; }
            public int Certification { get; set; }
            public decimal Unit_Price { get; set; }
            public decimal Sum_Price { get; set; }
            public bool Is_Submit { get; set; }
            public bool Is_Save { get; set; }
            public int User_ID { get; set; }
            public DateTime Submit_Date { get; set; }
        }
        protected void RB_Submit_Click(object sender, EventArgs e)
        {
          
            try
            {
      
                SaveCombineInfo();
         

              //  RadNotificationAlert.Text = "数据合并成功";
               // RadNotificationAlert.Show();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "info", "CloseRadWindow();", true);
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text =  ex.Message;
                RadNotificationAlert.Show();
            }
        }

      private void SaveCombineInfo()
     {


         if ((Session["UserId"]!=null) && (Session["idStr"] != null && Session["idStr"].ToString() != "") &&
          (Session["otherStr"] != null && Session["otherStr"].ToString() != ""))
      {
          int userid = Convert.ToInt32(Session["UserId"].ToString());
          string[] otherStr = new string[4];
          string idStr = string.Empty;
          idStr = Session["idStr"].ToString();
          if (idStr.Substring(0, 1) == "," && idStr.Length > 2)
          {
              idStr = idStr.Substring(1, Session["idStr"].ToString().Length - 2);
          }
          otherStr = Session["otherStr"].ToString().Split(new char[] { ',' });
          string PackId = otherStr[0];
          string DraftId = otherStr[1];

          string Combine_State = GridSource1.Rows[0]["Combine_State"].ToString();
          string Material_Name = GridSource1.Rows[0]["Material_Name"].ToString();
          string Material_Mark = GridSource1.Rows[0]["Material_Mark"].ToString();
          string ItemCode1 = GridSource1.Rows[0]["ItemCode1"].ToString();
          string Mat_Unit = GridSource1.Rows[0]["Mat_Unit"].ToString();
          string Mat_Rough_Weight = GridSource1.Rows[0]["Mat_Rough_Weight"].ToString();
          string Rough_Size = GridSource1.Rows[0]["Rough_Size"].ToString();

          string Rough_Spec = GridSource1.Rows[0]["Rough_Spec"].ToString();
          string DemandNumSum = GridSource1.Rows[0]["DemandNumSum"].ToString();
          string Quantity = GridSource1.Rows[0]["Quantity"].ToString();
          string NumCasesSum = GridSource1.Rows[0]["NumCasesSum"].ToString();
          string taskid = GridSource1.Rows[0]["TaskId"].ToString();
          string Stage = GridSource1.Rows[0]["Stage"].ToString();
          string Drawing_No = GridSource1.Rows[0]["Drawing_No"].ToString();
          string TaskCode = GridSource1.Rows[0]["TaskCode"].ToString();
          string DemandDate = GridSource1.Rows[0]["DemandDate"].ToString();
          string Mat_Pro_Weight = GridSource1.Rows[0]["Mat_Pro_Weight"].ToString();

          string Comment = GridSource1.Rows[0]["Comment"].ToString();
          string MaterialDept = GridSource1.Rows[0]["MaterialDept"].ToString();
          string LingJian_Type = GridSource1.Rows[0]["LingJian_Type"].ToString();
          string Material_Code = GridSource1.Rows[0]["Material_Code"].ToString();
          string TDM_Description = GridSource1.Rows[0]["TDM_Description"].ToString();
          string Import_Date = DateTime.Now.ToString("yyyy-MM-dd");

          string Technics_Line = GridSource1.Rows[0]["Technics_Line"].ToString();
          string Material_Tech_Condition = GridSource1.Rows[0]["Material_Tech_Condition"].ToString();
         
        
          string parentId_For_Combine = GridSource1.Rows[0]["ParentId_For_Combine"].ToString();

          
          string ID = GridSource1.Rows[0]["ID"].ToString();

        

          try
          {
              string strSQL = string.Empty;

              if (Combine_State == "2")
              {
                  strSQL = " Update M_Demand_DetailedList_Draft set ParentId_For_Combine = 0 ,Material_State = 0,Combine_State=1,Is_Del=0,Material_Name='" + Material_Name +
                   "',Material_Mark='" + Material_Mark + "',TaskCode='" + TaskCode + "',Drawing_No='" + Drawing_No + "',Mat_Pro_Weight='" + Mat_Pro_Weight + "',Mat_Unit='" + Mat_Unit + "',Mat_Rough_Weight='" + Mat_Rough_Weight + "',Rough_Size='" +
                   Rough_Size + "',Rough_Spec='" + Rough_Spec + "',DemandNumSum='" + DemandNumSum + "',NumCasesSum='" + NumCasesSum + "'where Id=" + ID + " select @@identity";

                  DBI.Execute(strSQL);
              }
              else
              {
              //    strSQL = " Insert Into M_Demand_DetailedList_Draft ( PackId, DraftId,taskid,TaskCode,Drawing_No, Mat_Pro_Weight,Material_State, Combine_State,Material_Name, Material_Mark, ItemCode1, Mat_Unit, Mat_Rough_Weight, Rough_Size, Rough_Spec, DemandNumSum, NumCasesSum,ParentId_For_Combine,Is_del)"
              //    + " Values (" + PackId + "," + DraftId + "," + taskid + ",'" + TaskCode + "','" + Drawing_No + "','"+ Mat_Pro_Weight+ "',0,1,'" +
                    //               Material_Name + "','" + Material_Mark + "','" + ItemCode1 + "','" + Mat_Unit
                      //             + "','" + Mat_Rough_Weight + "','" + Rough_Size + "','" + Rough_Spec + "'," + DemandNumSum + "," + NumCasesSum + ",0,0)" + " select @@identity";
              //   ID= DBI.GetSingleValue(strSQL);

             //    strSQL = " Insert Into M_Demand_DetailedList_Draft ( PackId, DraftId,taskid,MDPId,Quantity,Stage,DemandDate,Comment,MaterialDept,TaskCode,Drawing_No, Mat_Pro_Weight,Material_State, Combine_State,Material_Name, Material_Mark, ItemCode1, Mat_Unit, Mat_Rough_Weight, Rough_Size, Rough_Spec, DemandNumSum, NumCasesSum,Material_Tech_Condition,Material_Code,TDM_Description,Technics_Line,LingJian_Type,Import_Date,User_ID,ParentId,ParentId_For_Combine,Is_del)"
                  //       + " Values (" + PackId + "," + DraftId + "," + taskid + "," + MDPId + "," + Quantity + "," + Stage + ",'" + DemandDate + "','" + Comment + "','" + MaterialDept + "','" + TaskCode + "','" + Drawing_No + "'," + Mat_Pro_Weight + ",0,0,'" +
                      //            Material_Name + "','" + Material_Mark + "','" + ItemCode1 + "','" + Mat_Unit + "'," + Mat_Rough_Weight + ",'" + Rough_Size + "','" + Rough_Spec + "'," + mta.DemandNumSum + "," + mta.NumCasesSum + ",'" + mta.Material_Tech_Condition + "','" + mta.Material_Code + "','" + TDM_Description + "','" + TechnicsLine + "','" + LingJian_Type + "','" + Import_Date + "'," + userid + ",0,0,0)" + " select @@identity";
                 strSQL = " Insert Into M_Demand_DetailedList_Draft ( PackId, DraftId,taskid,Quantity,Stage,DemandDate,Comment,MaterialDept,TaskCode,Drawing_No, Mat_Pro_Weight,Material_State, Combine_State,Material_Name, Material_Mark, ItemCode1, Mat_Unit, Mat_Rough_Weight, Rough_Size, Rough_Spec, DemandNumSum, NumCasesSum,Material_Tech_Condition,Material_Code,TDM_Description,Technics_Line,LingJian_Type,Import_Date,User_ID,ParentId,ParentId_For_Combine,Is_del)"
                   + " Values (" + PackId + "," + DraftId + "," + taskid + "," +  Quantity + "," + Stage + ",'" + DemandDate + "','" + Comment + "','" + MaterialDept + "','" + TaskCode + "','" + Drawing_No + "'," + Mat_Pro_Weight + ",0,1,'" +
                            Material_Name + "','" + Material_Mark + "','" + ItemCode1 + "','" + Mat_Unit + "'," + Mat_Rough_Weight + ",'" + Rough_Size + "','" + Rough_Spec + "'," + DemandNumSum + "," + NumCasesSum + ",'" + Material_Tech_Condition + "','" + Material_Code + "','" + TDM_Description + "','" + Technics_Line + "','" + LingJian_Type + "','" + Import_Date + "'," + userid + ",0,0,0)" + " select @@identity";

                 ID = DBI.GetSingleValue(strSQL);

              }

              //  strSQL = " Update M_Demand_DetailedList_Draft set ParentId_For_Combine =" + id + ", Material_State = 9" + " where Id in (" + IdStr + ") and Id!=" + ID;

              strSQL = " Update M_Demand_DetailedList_Draft set ParentId_For_Combine =" + ID + ", Material_State = 9" + " where Id in (" + idStr + ")";
              DBI.Execute(strSQL);
          }
          catch (Exception e)
          {
              throw new Exception("数据库操作-操作物资需求清单详表时出现异常" + e.Message.ToString());

          }
        }
     }


 

 
        protected void RadGrid_MDemandCombinelist_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id =(e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
              //  string MaterialDept = (e.Item as GridDataItem).GetDataKeyValue("MaterialDept").ToString();
                DataTable table = GridSource1;
                         //  RadTextBox rbtSN = e.Item.FindControl("rtb_SpecialNeeds") as RadTextBox;
               // rbtSN.CssClass = id;
    
                RadTextBox rtbDemandNumSum = e.Item.FindControl("DemandNumSum") as RadTextBox;
                rtbDemandNumSum.Text = (GridSource1.Select("ID='" + id + "'")[0]["DemandNumSum"].ToString());

                RadTextBox rtbRough_Size = e.Item.FindControl("Rough_Size") as RadTextBox;
                rtbRough_Size.Text = (GridSource1.Select("ID='" + id + "'")[0]["Rough_Size"].ToString());

                RadTextBox rtbNumCasesSum = e.Item.FindControl("NumCasesSum") as RadTextBox;
                rtbNumCasesSum.Text = (GridSource1.Select("ID='" + id + "'")[0]["NumCasesSum"].ToString());



                RadTextBox rtbMat_Rough_Weight = e.Item.FindControl("Mat_Rough_Weight") as RadTextBox;
                rtbMat_Rough_Weight.Text = (GridSource1.Select("ID='" + id + "'")[0]["Mat_Rough_Weight"].ToString());


                RadTextBox rtbMat_Pro_Weight = e.Item.FindControl("Mat_Pro_Weight") as RadTextBox;
                rtbMat_Pro_Weight.Text = (GridSource1.Select("ID='" + id + "'")[0]["Mat_Pro_Weight"].ToString());

                RadTextBox rtbTaskCode = e.Item.FindControl("TaskCode") as RadTextBox;
                rtbTaskCode.Text = (GridSource1.Select("ID='" + id + "'")[0]["TaskCode"].ToString());

                RadTextBox rtbDrawing_No = e.Item.FindControl("Drawing_No") as RadTextBox;
                rtbDrawing_No.Text = (GridSource1.Select("ID='" + id + "'")[0]["Drawing_No"].ToString());
            }
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

        protected void DemandNumSum_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("ID='" + id + "'")[0]["DemandNumSum"] = rtb.Text;
        }

        protected void Mat_Mat_Pro_Weight_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("ID='" + id + "'")[0]["Mat_Pro_Weight"] = rtb.Text;
        }

        protected void Mat_Rough_Weight_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("ID='" + id + "'")[0]["Mat_Rough_Weight"] = rtb.Text;
        }
        protected void TaskCode_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("ID='" + id + "'")[0]["TaskCode"] = rtb.Text;
        }

        protected void Drawing_No_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("ID='" + id + "'")[0]["Drawing_No"] = rtb.Text;
        }

        protected void Rough_Size_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("ID='" + id + "'")[0]["Rough_Size"] = rtb.Text;
        }
        protected void NumCasesSum_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource1.Select("ID='" + id + "'")[0]["NumCasesSum"] = rtb.Text;
        }
          



        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                Response.Redirect("~/Plan/MDemandDetailsCombine.aspx?PackId=" + Request.QueryString["PackId"].ToString());
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
		
		 protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandDetails.ExportSettings.FileName = "物资需求清单合并记录" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandDetails.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandDetails.ExportSettings.FileName = "物资需求清单合并记录" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandDetails.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_MDemandDetails.ExportSettings.FileName = "物资需求清单合并记录" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_MDemandDetails.ExportSettings.IgnorePaging = true;
            RadGrid_MDemandDetails.MasterTableView.ExportToPdf();
            RadGrid_MDemandDetails.ExportSettings.IgnorePaging = false;
        }

    }
}