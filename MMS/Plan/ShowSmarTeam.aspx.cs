using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Camc.Web.Library;
using System.Configuration;
using System.Data;
using Telerik.Web.UI;
using System.Drawing;
using System.Xml;
using System.Collections.Specialized;

namespace mms.Plan
{
    public partial class ShouSmarTeam : System.Web.UI.Page
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
                Common.CheckPermission(Session["UserName"].ToString(), "ShouSmarTeam", this.Page);               
                Session["SSTBOMWhere"] = null;
                Session["SSTDefectWhere"] = null;
                Session["SSTSubmitStateWhere"] = null;

                GridBind();

                if (Request.QueryString["Tabs"] != null)
                {
                    if (Request.QueryString["Tabs"].ToString() == "1" || Request.QueryString["Tabs"].ToString() == "2")
                    {
                        RadTabStrip1.SelectedIndex = Convert.ToInt32(Request.QueryString["Tabs"].ToString());
                        RadMultiPage1.SelectedIndex = Convert.ToInt32(Request.QueryString["Tabs"].ToString());
                    }
                }

                RadTabStrip1.Tabs.FindTabByValue("3").NavigateUrl = "/Plan/ChangeMaterialQuota.aspx?PackId=" + Request.QueryString["PackId"].ToString() + "&fromPage=0"; 

                string strSQL = " select LingJian_Type_Code, LingJian_Type_Name from Sys_LingJian_Info where Is_Del = 'false'";
                DataTable dtlingJianInfo = DBI.Execute(strSQL, true);
                RDDL_LingJian_Type_BOM.DataSource = dtlingJianInfo;
                RDDL_LingJian_Type_BOM.DataTextField = "LingJian_Type_Name";
                RDDL_LingJian_Type_BOM.DataValueField = "LingJian_Type_Code";
                RDDL_LingJian_Type_BOM.DataBind();
                RDDL_LingJian_Type_Defect.DataSource = dtlingJianInfo;
                RDDL_LingJian_Type_Defect.DataTextField = "LingJian_Type_Name";
                RDDL_LingJian_Type_Defect.DataValueField = "LingJian_Type_Code";
                RDDL_LingJian_Type_Defect.DataBind();
                RDDL_LingJian_Type_SubmitState.DataSource = dtlingJianInfo;
                RDDL_LingJian_Type_SubmitState.DataTextField = "LingJian_Type_Name";
                RDDL_LingJian_Type_SubmitState.DataValueField = "LingJian_Type_Code";
                RDDL_LingJian_Type_SubmitState.DataBind();

                strSQL =
                    " select Sys_Model.Model ,PlanName from P_Pack left join Sys_Model on Sys_Model.ID = P_Pack.Model where PackId = '" +
                    Request.QueryString["PackId"].ToString() + "'";
                DataTable dtpack = DBI.Execute(strSQL, true);
                lblModel.Text = dtpack.Rows[0]["Model"].ToString();
                lblPlanName.Text = dtpack.Rows[0]["PlanName"].ToString();
            }
        }

        #region 页面控件绑定数据源
        private DataTable GetBOM(string lingjiantype)
        {
           
            string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            string strSQL = " select a.Id, VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, Material_Code, Material_Spec, TDM_Description, Material_Name, PackId";
            strSQL += " , TaskId, DraftId, Drawing_No, Technics_Line, Technics_Comment, Material_Mark, ItemCode1, ItemCode2, MaterialsNum, Mat_Unit, LingJian_Type, Mat_Rough_Weight, Mat_Pro_Weight";
            strSQL += " , Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec, Rough_Size, MaterialsDes, StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum";
            strSQL += ", DemandDate, Quantity, Tech_Quantity, Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, User_ID, JSGS_Des";
            strSQL += " , a.Is_del, TaskCode, MaterialDept, MissingDescription, MDPId, CN_Material_State, b.LingJian_Type_Name as LingJian_Type1";
            if (Session["SSTBOMWhere"] != null)
            {
                strSQL += " ,0 as ParentId";
            }
            else
            {
                strSQL += " ,ParentId";
            }
            if (lingjiantype == "" || lingjiantype == "1" || lingjiantype == "2" || lingjiantype == "3" || lingjiantype == "4" || lingjiantype == "5")
            {
                strSQL += " from M_Demand_DetailedList_Draft as a left join Sys_LingJian_Info as b on b.LingJian_Type_Code = a.LingJian_Type";
            }
            else
            {
                strSQL += " from M_Demand_DetailedList_Draft as a left join Sys_LingJian_Info as b on b.LingJian_Type_Code='6' ";
            }
            strSQL += " where PackID = '" + PackID + "' and Is_Bom_Show = 'true' and a.Is_del = 'false'";
            if (Session["SSTBOMWhere"] != null)
            {
                strSQL += Session["SSTBOMWhere"].ToString();
            }
            strSQL += " order by Id";
            return DBI.Execute(strSQL, true);
        }

        private DataTable GetDefect()
        {
            string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            string strSQL = " create table #test ( Id int, ParentId int, Material_Code nvarchar(max),TDM_Description nvarchar(max),LingJian_Type nvarchar(max),"
                + " Material_Name nvarchar(max),Drawing_No nvarchar(max),Technics_Line nvarchar(max),MaterialDept nvarchar(max), ItemCode1 nvarchar(max),Quantity nvarchar(max),Mat_Unit nvarchar(max),"
                + " Mat_Rough_Weight nvarchar(50),Mat_Pro_Weight nvarchar(50),Rough_Size nvarchar(50),Rough_Spec nvarchar(50),DemandNumSum nvarchar(50),"
                + " NumCasesSum nvarchar(max),DemandDate nvarchar(max),i int, MissingDescription nvarchar(max), Material_Mark nvarchar(max))  "
                + " insert into #test select ID ,ParentID,Material_Code ,TDM_Description ,LingJian_Type ,Material_Name ,Drawing_No ,Technics_Line ,MaterialDept,ItemCode1 ,"
                + " Quantity ,Mat_Unit ,Mat_Rough_Weight ,Mat_Pro_Weight ,Rough_Size ,Rough_Spec ,DemandNumSum ,NumCasesSum ,DemandDate ,'1' ,MissingDescription, Material_Mark"
                + " from [dbo].[M_Demand_DetailedList_Draft] "
                + " where packid = '" + PackID + "' and (Material_State = '4' or Material_State = '5' or Material_State = '6') and Is_del = 'false'";
            if (Session["SSTDefectWhere"] == null)
            {
                strSQL += " declare @i int select @i = max(i) from #test"
                 + " while (select count(*) from [dbo].[M_Demand_DetailedList_Draft] where ID in (select ParentID from #test where i = @i)) <> 0"
                 + " begin insert into #test "
                 + " select ID ,ParentID ,Material_Code ,TDM_Description ,LingJian_Type ,Material_Name ,Drawing_No ,Technics_Line ,MaterialDept ,ItemCode1 ,"
                 + " Quantity ,Mat_Unit ,Mat_Rough_Weight ,Mat_Pro_Weight ,Rough_Size ,Rough_Spec ,DemandNumSum ,NumCasesSum ,DemandDate ,@i + 1, '',''"
                 + " from [dbo].[M_Demand_DetailedList_Draft]"
                 + " where packid = '" + PackID + "' and ID in (select ParentID from #test where i = @i) and ID not in (select ID from #test) and Is_del = 'false'"
                 + " select @i = @i + 1 end "
                 + " select a.*, LingJian_Type_Name as LingJian_Type1 from #test as a left join Sys_LingJian_Info as b on b.LingJian_Type_Code = a.LingJian_Type order by Id";
            }
            else
            {
                strSQL += Session["SSTDefectWhere"].ToString();
                strSQL += " select a.Id , 0 as ParentId , Material_Code ,TDM_Description ,LingJian_Type ,"
                + " Material_Name ,Drawing_No ,Technics_Line ,MaterialDept , ItemCode1 ,Quantity ,Mat_Unit ,"
                + " Mat_Rough_Weight ,Mat_Pro_Weight ,Rough_Size ,Rough_Spec ,DemandNumSum ,"
                + " NumCasesSum ,DemandDate ,i , MissingDescription , Material_Mark "
                + " ,LingJian_Type_Name as LingJian_Type1 from #test as a left join Sys_LingJian_Info as b on b.LingJian_Type_Code = a.LingJian_Type order by Id";
            }
            return DBI.Execute(strSQL, true);
        }

        private DataTable GetSubmitState()
        {
            string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            string strSQL = " create table #test1 ( Id int, ParentId int, Material_Code nvarchar(max),TDM_Description nvarchar(max),LingJian_Type nvarchar(max),"
                + " Material_Name nvarchar(max),Drawing_No nvarchar(max),Technics_Line nvarchar(max),MaterialDept nvarchar(max), ItemCode1 nvarchar(max),Quantity nvarchar(max),Mat_Unit nvarchar(max),"
                + " Mat_Rough_Weight nvarchar(50),Mat_Pro_Weight nvarchar(50),Rough_Size nvarchar(50),Rough_Spec nvarchar(50),DemandNumSum nvarchar(50),"
                + " NumCasesSum nvarchar(max),DemandDate nvarchar(max),i int, MissingDescription nvarchar(max), Material_Mark nvarchar(max), Material_State1 nvarchar(50))  "
                + " insert into #test1 select ID ,ParentID,Material_Code ,TDM_Description ,LingJian_Type ,Material_Name ,Drawing_No ,Technics_Line ,MaterialDept,ItemCode1 ,"
                + " Quantity ,Mat_Unit ,Mat_Rough_Weight ,Mat_Pro_Weight ,Rough_Size ,Rough_Spec ,DemandNumSum ,NumCasesSum ,DemandDate ,'1' ,MissingDescription, Material_Mark "
                + " ,case when Material_state = '0' or Material_state = '2' then '未提交' when Material_state = '1' then '已提交'  else '' end"
                + " from [dbo].[M_Demand_DetailedList_Draft] "
                + " where packid = '" + PackID + "' and (Material_State = '0' or Material_State = '1' or Material_State = '2') and Is_del = 'false'";
            if (Session["SSTSubmitStateWhere"] == null)
            {
                strSQL += " declare @i int select @i = max(i) from #test1"
                    + " while (select count(*) from [dbo].[M_Demand_DetailedList_Draft] where ID in (select ParentID from #test1 where i = @i)) <> 0"
                    + " begin insert into #test1 "
                    + " select ID ,ParentID ,Material_Code ,TDM_Description ,LingJian_Type ,Material_Name ,Drawing_No ,Technics_Line ,MaterialDept ,ItemCode1 ,"
                    + " Quantity ,Mat_Unit ,Mat_Rough_Weight ,Mat_Pro_Weight ,Rough_Size ,Rough_Spec ,DemandNumSum ,NumCasesSum ,DemandDate ,@i + 1, '','',''"
                    + " from [dbo].[M_Demand_DetailedList_Draft]"
                    + " where packid = '" + PackID + "' and ID in (select ParentID from #test1 where i = @i) and ID not in (select ID from #test1) and Is_del = 'false'"
                    + " select @i = @i + 1 end "
                    + " select a.*, LingJian_Type_Name as LingJian_Type1 from #test1 as a left join Sys_LingJian_Info as b on b.LingJian_Type_Code = a.LingJian_Type order by Id";
            }
            else
            {
                strSQL += Session["SSTSubmitStateWhere"].ToString();
                strSQL += " select a.Id , 0 as ParentId , Material_Code ,TDM_Description ,LingJian_Type ,"
                    + " Material_Name ,Drawing_No ,Technics_Line ,MaterialDept , ItemCode1 ,Quantity ,Mat_Unit ,"
                    + " Mat_Rough_Weight ,Mat_Pro_Weight ,Rough_Size ,Rough_Spec ,DemandNumSum ,"
                    + " NumCasesSum ,DemandDate ,i , MissingDescription , Material_Mark , Material_State1 "
                    + " ,LingJian_Type_Name as LingJian_Type1 from #test1 as a left join Sys_LingJian_Info as b on b.LingJian_Type_Code = a.LingJian_Type order by Id";
            }
            return DBI.Execute(strSQL, true);
        }

        private void GridBind()
        {
            Session["GridSourceBOM"] = GetBOM("");
            if ((Session["GridSourceBOM"] as DataTable).Rows.Count < 50)
            {
                RTL_BOM.ExpandAllItems();
            }
            else
            {
                RTL_BOM.ExpandedIndexes.Add(new TreeListHierarchyIndex { LevelIndex = 0, NestedLevel = 0 });
            }

            Session["GridSourceDefect"] = GetDefect();
            if ((Session["GridSourceDefect"] as DataTable).Rows.Count < 50)
            {
                RTL_Defect.ExpandAllItems();
            }
            else
            {
                RTL_Defect.ExpandedIndexes.Add(new TreeListHierarchyIndex { LevelIndex = 0, NestedLevel = 0 });
            }

            Session["GridSourceSubmit"] = GetSubmitState();
            if ((Session["GridSourceSubmit"] as DataTable).Rows.Count < 50)
            {
                RTL_SubmitState.ExpandAllItems();
            }
            else
            {
                RTL_SubmitState.ExpandToLevel(2);
            }

            string PackId = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();

            string strSQL = " declare @Material_State0 int,@Material_State1 int,@Material_State2 int,"
            + " @Material_State4 int,@Material_State5 int,@Material_State6 int"
            + " select @Material_State0 = count(*) from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Material_State = '0' and Is_del = 'false'"
            + " select @Material_State1 = count(*) from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Material_State = '1' and Is_del = 'false'"
            + " select @Material_State2 = count(*) from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Material_State = '2' and Is_del = 'false'"
            + " select @Material_State4 = count(*) from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Material_State = '4' and Is_del = 'false'"
            + " select @Material_State5 = count(*) from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Material_State = '5' and Is_del = 'false'"
            + " select @Material_State6 = count(*) from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Material_State = '6' and Is_del = 'false'"
            + " select @Material_State0 as Material_State0, @Material_State1 as Material_State1, @Material_State2 as Material_State2"
            + " , @Material_State4 as Material_State4, @Material_State5 as Material_State5, @Material_State6 as Material_State6";

            DataTable dt = DBI.Execute(strSQL, true);
            Label10.Text = dt.Rows[0]["Material_State0"].ToString();
            Label11.Text = dt.Rows[0]["Material_State1"].ToString();
            Label12.Text = dt.Rows[0]["Material_State2"].ToString();
            Label14.Text = dt.Rows[0]["Material_State4"].ToString();
            Label15.Text = dt.Rows[0]["Material_State5"].ToString();
            Label16.Text = dt.Rows[0]["Material_State6"].ToString();
            Label24.Text = dt.Rows[0]["Material_State4"].ToString();
            Label25.Text = dt.Rows[0]["Material_State5"].ToString();
            Label26.Text = dt.Rows[0]["Material_State6"].ToString();
            Label30.Text = dt.Rows[0]["Material_State0"].ToString();
            Label31.Text = dt.Rows[0]["Material_State1"].ToString();
            Label32.Text = dt.Rows[0]["Material_State2"].ToString();
            Label1All.Text = (Convert.ToDouble(Label10.Text) + Convert.ToDouble(Label11.Text) + Convert.ToDouble(Label12.Text) + Convert.ToDouble(Label14.Text) + Convert.ToDouble(Label15.Text) + Convert.ToDouble(Label16.Text)).ToString();
        }

        protected void RTL_BOM_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            RTL_BOM.DataSource = Session["GridSourceBOM"];
        }

        protected void RTL_Defect_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            RTL_Defect.DataSource = Session["GridSourceDefect"];
        }

        protected void RTL_SubmitState_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            RTL_SubmitState.DataSource = Session["GridSourceSubmit"];
        }

        protected void RTL_SubmitState_ItemDataBound(object sender, Telerik.Web.UI.TreeListItemDataBoundEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                TreeListDataItem item = e.Item as TreeListDataItem;

                if (item["Material_State1"].Text == "已提交")
                {
                    item.ForeColor = Color.Green;
                }
                else if (item["Material_State1"].Text == "未提交")
                {
                    item.ForeColor = Color.Red;
                }
            }
        }
        #endregion

        #region 直接同步确定额和不规范项
        //重新同步全部缺定额项和不规范项
        protected void RB_SynchronAll_Click(object sender, EventArgs e)
        {
            try
            {
                DBI.OpenConnection();
                DBI.BeginTrans();
                string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
                string strSQL = " select ID from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and (Material_State = '4' or Material_State = '5' or Material_State = '6')";
                DataTable dt = DBI.Execute(strSQL, true);
                string listID = "('0'";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string id = dt.Rows[i]["ID"].ToString();
                    if (ReSynchron(id) == "同步")
                    {
                        listID += ",'" + id + "'";
                    }
                }
                listID += ")";
                UpdateMaterialStateAndDemandNumSum(listID);
                DBI.CommitTrans();
                RadNotificationAlert.Text = "同步成功！";
                RadNotificationAlert.Show();
                GridBind();
            }
            catch (Exception ex)
            {
                DBI.RollbackTrans();
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        #endregion

        #region M_Demand_DetailedList_Draft 属性

        public class M_Demand_DetailedList_Draft
        {
            public string Id { get; set; }
            public string VerCode { get; set; }
            public string Class_Id { get; set; }
            public string Object_Id { get; set; }
            public string Stage { get; set; }
            public string Material_State { get; set; }
            public string Material_Tech_Condition { get; set; }
            public string Material_Code { get; set; }
            public string ParentId { get; set; }
            public string Material_Spec { get; set; }
            public string TDM_Description { get; set; }
            public string Material_Name { get; set; }
            public string PackId { get; set; }
            public string TaskId { get; set; }
            public string DraftId { get; set; }
            public string Drawing_No { get; set; }
            public string Technics_Line { get; set; }
            public string Technics_Comment { get; set; }
            public string Material_Mark { get; set; }
            public string ItemCode1 { get; set; }
            public string ItemCode2 { get; set; }
            public string MaterialsNum { get; set; }
            public string Mat_Unit { get; set; }
            public string Lingjian_Type { get; set; }
            public string Mat_Rough_Weight { get; set; }
            public string Mat_Pro_Weight { get; set; }
            public string Mat_Weight { get; set; }
            public string Mat_Efficiency { get; set; }
            public string Mat_Comment { get; set; }
            public string Mat_Technics { get; set; }
            public string Rough_Spec { get; set; }
            public string Rough_Size { get; set; }
            public string MaterialsDes { get; set; }
            public string StandAlone { get; set; }
            public string ThisTimeOperation { get; set; }
            public string PredictDeliveryDate { get; set; }
            public string DemandNumSum { get; set; }
            public string NumCasesSum { get; set; }
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
            public string Import_Date { get; set; }
            public string User_ID { get; set; }
            public string TaskCode { get; set; }
            public string MaterailDept { get; set; }
            public string MissingDescription { get; set; }
            public string CN_Material_State { get; set; }
        }

        #endregion

        #region 缺定额和不规范：同步SmarTeam修改表MDDLD  修改MDDLD物料状态，共计需求量（kg）
        private string ReSynchron(string id)
        {
            string strSQL = " declare @Object_Id nvarchar(50), @Class_Id nvarchar(50) , @pObject_Id nvarchar(50), @pClass_Id nvarchar(50), @Drawing_No nvarchar(50), @Stage nvarchar(50), @NumCasesSum decimal(18,4), @pNumCasesSum decimal(18,4), @ParentId int";
            strSQL += " select @Object_Id = Object_ID, @Class_Id = Class_ID , @Drawing_No = Drawing_No , @Stage = Stage, @NumCasesSum = NumCasesSum , @ParentId = ParentId from M_Demand_DetailedList_Draft where ID = '" + id + "'"; ;
            strSQL += " select @pObject_Id = Object_ID , @pClass_Id = Class_ID, @pNumCasesSum = NumCasesSum from M_Demand_DetailedList_Draft where ID = @ParentId ";
            strSQL += " select @Object_Id as Object_Id, @Class_Id as Class_Id, @pObject_Id  as pObject_Id, @pClass_Id as pClass_Id,  @Drawing_No as Drawing_No, @Stage as Stage, @NumCasesSum as NumCasesSum , @pNumCasesSum as pNumCasesSum , @ParentId as ParentId";
            DataTable dt = DBI.Execute(strSQL, true);
            string Drawing_No = dt.Rows[0]["Drawing_No"].ToString();
            string Stage = dt.Rows[0]["Stage"].ToString();
            string Class_Id = dt.Rows[0]["Class_Id"].ToString();
            string Object_Id = dt.Rows[0]["Object_Id"].ToString();
            string NumCasesSum = dt.Rows[0]["NumCasesSum"].ToString();
            string pClass_Id = dt.Rows[0]["pClass_Id"].ToString();
            string pObject_Id = dt.Rows[0]["pObject_Id"].ToString();
            string pNumCasesSum = dt.Rows[0]["pNumCasesSum"].ToString();
            string ParentId = dt.Rows[0]["ParentId"].ToString();

            //模拟接口
            strSQL = "select * from [dbo].[InterfaceData] where [CN_Drawing_No] = '" + Drawing_No + "' and State = '" + Stage + "'";
            DataTable dtGetByDrawingNoAndPhase = DBI.Execute(strSQL, true);

            //SmarTeam接口数据
            //DataTable dtGetByDrawingNoAndPhase = ST.GetByDrawingNoAndPhase(Drawing_No, Stage, "").Tables[0];

            if (dtGetByDrawingNoAndPhase.Rows.Count > 0)
            {
                M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft();

                MDDLD.Class_Id = dtGetByDrawingNoAndPhase.Rows[0]["Class_Id"].ToString().Trim();
                MDDLD.Object_Id = dtGetByDrawingNoAndPhase.Rows[0]["Object_Id"].ToString().Trim();
                MDDLD.Stage = dtGetByDrawingNoAndPhase.Rows[0]["State"].ToString().Trim();
                MDDLD.Material_Tech_Condition = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_Tech_Condition"].ToString().Trim();
                MDDLD.Material_Spec = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_Spec"].ToString().Trim();
                MDDLD.TDM_Description = dtGetByDrawingNoAndPhase.Rows[0]["TDM_Description"].ToString().Trim();
                MDDLD.Material_Name = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_Name"].ToString().Trim();
                MDDLD.Drawing_No = dtGetByDrawingNoAndPhase.Rows[0]["CN_Drawing_No"].ToString().Trim();
                MDDLD.Technics_Line = dtGetByDrawingNoAndPhase.Rows[0]["CN_Technics_Line"].ToString().Trim();
                MDDLD.Technics_Comment = dtGetByDrawingNoAndPhase.Rows[0]["CN_Technics_Comment"].ToString().Trim();
                MDDLD.Material_Mark = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_Mark"].ToString().Trim();
                MDDLD.ItemCode1 = dtGetByDrawingNoAndPhase.Rows[0]["CN_ItemCode1"].ToString().Trim();
                MDDLD.ItemCode2 = dtGetByDrawingNoAndPhase.Rows[0]["CN_ItemCode2"].ToString().Trim();
                MDDLD.Mat_Unit = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Unit"].ToString().Trim();
                MDDLD.Lingjian_Type = dtGetByDrawingNoAndPhase.Rows[0]["CN_LingJian_Type"].ToString().Trim();
                MDDLD.Mat_Rough_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Rough_Weight"].ToString().Trim();
                MDDLD.Mat_Pro_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Pro_Weight"].ToString().Trim();
                MDDLD.Mat_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Weight"].ToString().Trim();
                MDDLD.Mat_Efficiency = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Efficiency"].ToString().Trim();
                MDDLD.Mat_Comment = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Comment"].ToString().Trim();
                MDDLD.Mat_Technics = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Technics"].ToString().Trim();
                MDDLD.Rough_Spec = dtGetByDrawingNoAndPhase.Rows[0]["CN_Rough_Spec"].ToString().Trim();
                MDDLD.Rough_Size = dtGetByDrawingNoAndPhase.Rows[0]["CN_Rough_Size"].ToString().Trim();
                MDDLD.Id = id;

                if (ParentId == "0")
                {
                    MDDLD.Quantity = NumCasesSum;
                    MDDLD.NumCasesSum = NumCasesSum;
                }
                else
                {
                    //获取父节点是否有变化,如果没有变化继续一下操作，否则删除

                    //模拟接口数据
                    strSQL = "select * from InterfaceData where Class_ID = (select Class_ID1 from InterfaceData where Class_ID = '" + MDDLD.Class_Id + "' and OBJECT_ID = '" + MDDLD.Object_Id + "')";
                    strSQL += " and Object_ID = (select Object_ID1 from InterfaceData where Class_ID = '" + MDDLD.Class_Id + "' and OBJECT_ID = '" + MDDLD.Object_Id + "')";
                    DataTable dtGetParents = DBI.Execute(strSQL, true);

                    //SmarTeam接口数据
                    //DataTable dtGetParents = ST.GetParents(MDDLD.Class_Id, MDDLD.Object_Id);

                    bool IsOP = false;
                    DataRow drGetParents = dtGetParents.NewRow();
                    for (int j = 0; j < dtGetParents.Rows.Count; j++)
                    {
                        if (dtGetParents.Rows[j]["Class_ID"].ToString() == pClass_Id && dtGetParents.Rows[j]["Object_ID"].ToString() == pObject_Id)
                        {
                            drGetParents = dtGetParents.Rows[j];
                            IsOP = true;
                            break;
                        }
                    }

                    if (IsOP == false)
                    {
                        strSQL = " Update M_Demand_DetailedList_Draft set IsDel = 'true' where ID = '" + id + "'";
                        DBI.Execute(strSQL);
                        return "同步";
                    }


                    //模拟接口数据
                    DataTable dtItemsCount = DBI.Execute("select * from InterfaceData where Object_ID1 = '" + pObject_Id + "' and Class_ID1 = '" + pClass_Id + "' and  Object_ID = '" + Object_Id + "' and Class_ID = '" + Class_Id + "'", true);

                    //SmarTeam数据
                    //DataTable dtItemsCount = ST.GetItemsCount(pClass_Id, pObject_Id, Class_Id, Object_Id).Tables[0]; ;

                    if (dtItemsCount.Rows.Count > 0)
                    {
                        MDDLD.Quantity = dtItemsCount.Rows[0]["CN_Quantity"].ToString().Trim();
                        MDDLD.Tech_Quantity = dtItemsCount.Rows[0]["CN_Tech_Quantity"].ToString().Trim();
                        MDDLD.Memo_Quantity = dtItemsCount.Rows[0]["CN_Memo_Quantity"].ToString().Trim();
                        MDDLD.Test_Quantity = dtItemsCount.Rows[0]["CN_Test_Quantity"].ToString().Trim();
                        MDDLD.Required_Quantity = dtItemsCount.Rows[0]["CN_Required_Quantity"].ToString().Trim();
                        MDDLD.Other_Quantity = dtItemsCount.Rows[0]["CN_Other_Quantity"].ToString().Trim();
                        MDDLD.Ballon_No = dtItemsCount.Rows[0]["CN_Ballon_No"].ToString().Trim();
                        MDDLD.Comment = dtItemsCount.Rows[0]["CN_Comment"].ToString().Trim();
                    }
                    else
                    {
                        MDDLD.Quantity = "";
                        MDDLD.Tech_Quantity = "";
                        MDDLD.Memo_Quantity = "";
                        MDDLD.Test_Quantity = "";
                        MDDLD.Required_Quantity = "";
                        MDDLD.Other_Quantity = "";
                        MDDLD.Ballon_No = "";
                        MDDLD.Comment = "";
                    }

                    if (MDDLD.Quantity != "" && pNumCasesSum != "")
                    {
                        try
                        {
                            Convert.ToInt32(MDDLD.Quantity);
                            MDDLD.NumCasesSum = (Convert.ToInt32(MDDLD.Quantity) * Convert.ToInt32(Convert.ToDouble(pNumCasesSum))).ToString();  //共计需求件数
                        }
                        catch
                        {
                            MDDLD.Quantity = "";
                        }
                    }
                }

                UpdateMDDLD(MDDLD);
            }
            else
            {
                strSQL = " Update M_Demand_DetailedList_Draft set IsDel = 'true' where ID = '" + id + "'";
                DBI.Execute(strSQL);
            }

            return "同步";
        }

        private void UpdateMDDLD(M_Demand_DetailedList_Draft MDDLD)
        {
            string strSQL = " Update M_Demand_DetailedList_Draft set Class_Id = '" + MDDLD.Class_Id + "', Object_Id = '" + MDDLD.Object_Id + "', Stage = '" + MDDLD.Stage + "' ";
            strSQL += " ,Material_Tech_Condition = '" + MDDLD.Material_Tech_Condition + "', Material_Spec = '" + MDDLD.Material_Spec + "', TDM_Description= '" + MDDLD.TDM_Description + "'";
            strSQL += " ,Material_Name = '" + MDDLD.Material_Name + "', Drawing_No = '" + MDDLD.Drawing_No + "', Technics_Line = '" + MDDLD.Technics_Line + "'";
            strSQL += " ,Technics_Comment = '" + MDDLD.Technics_Comment + "',Material_Mark = '" + MDDLD.Material_Mark + "', ItemCode1 = '" + MDDLD.ItemCode1 + "' ";
            strSQL += " ,ItemCode2 = '" + MDDLD.ItemCode2 + "', Mat_Unit = '" + MDDLD.Mat_Unit + "',Lingjian_Type = '" + MDDLD.Lingjian_Type + "'";
            strSQL += " ,Mat_Rough_Weight = '" + MDDLD.Mat_Rough_Weight + "', Mat_Pro_Weight = '" + MDDLD.Mat_Pro_Weight + "', Mat_Weight = '" + MDDLD.Mat_Weight + "'";
            strSQL += " , Mat_Efficiency = '" + MDDLD.Mat_Efficiency + "', Mat_Comment = '" + MDDLD.Mat_Comment + "',Mat_Technics = '" + MDDLD.Mat_Technics + "' ";
            strSQL += " ,Rough_Spec = '" + MDDLD.Rough_Spec + "', Rough_Size = '" + MDDLD.Rough_Size + "',Quantity = '" + MDDLD.Quantity + "'";
            strSQL += " ,NumCasesSum = '" + MDDLD.NumCasesSum + "', Material_State = '0', DemandNumSum = null, MissingDescription = '', MaterialDept = '' ";
            strSQL += " where ID = '" + MDDLD.Id + "'";
            DBI.Execute(strSQL);
        }

        private void UpdateMaterialStateAndDemandNumSum(string ListID)
        {
            string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();

            string strSQL = "";

            DataTable dtcf = DBI.Execute(" select Top 1 * from [dbo].[Sys_ComputationalFormula]", true);
            double pt1 = 0, pt2 = 0;
            if (dtcf.Rows.Count > 0)
            {
                pt1 = Convert.ToDouble(dtcf.Rows[0]["Parameter1"].ToString());
                pt2 = Convert.ToDouble(dtcf.Rows[0]["Parameter2"].ToString());
            }
            //默认全部未提交状态

            //修改物资材料定额状态 

            //零件类型不等于4的，不需要材料定额信息，状态为3
            strSQL = " Update M_Demand_DetailedList_Draft set Material_State = '3'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') <> '4'"; ;
            //零件类型等于4的，判断物资领料部门
            strSQL += " Update M_Demand_DetailedList_Draft set MaterialDept = substring(Technics_Line ,5,2)";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4'";
            strSQL += " and (Technics_Line like '100-51%' or Technics_Line like '100-53%' or Technics_Line like '100-55%' or Technics_Line like '100-56%' or Technics_Line like '100-57%' or Technics_Line like '100-58%')";
            strSQL += " Update M_Demand_DetailedList_Draft set MaterialDept = substring(Technics_Line ,1,2)";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4'";
            strSQL += " and (Technics_Line like '51-%' or Technics_Line like '53-%' or Technics_Line like '55-%' or Technics_Line like '56-%' or Technics_Line like '57-%' or Technics_Line like '58-%')";
            //物资领料部门为空的，不需要材料定额信息，状态为3
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '3' where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and MaterialDept = ''";
            //零件类型等于4的，缺失材料定额数据， 状态为4
            //缺失牌号
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '牌号'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and (Material_Mark is null or REPLACE(Material_Mark,' ','') = '')";
            //缺失物资名称
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资名称'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and (Material_Name is null or REPLACE(Material_Name,' ','')= '') ";
            //缺失物资编码
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资编码'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and (ItemCode1 is null or REPLACE(ItemCode1,' ','') = '')";
            //缺失计量单位
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '计量单位'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and (Mat_Unit is null or REPLACE(Mat_Unit,' ','') = '')";
            //缺失物资件数
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资件数'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and Quantity is null";
            //缺失单件质量
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '单件质量'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and (Mat_Rough_Weight is null or REPLACE(Mat_Rough_Weight,' ','') = '')";
            //缺失每产品质量
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '每产品质量'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and (Mat_Pro_Weight is null or REPLACE(Mat_Pro_Weight,' ','') = '')";
            //缺失物资规格
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资规格'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and (Rough_Spec is null or REPLACE(Rough_Spec,' ','') = '')";
            //缺失物资尺寸
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资尺寸'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and (Rough_Size is null or REPLACE(Rough_Size,' ','') = '')";
            //单件质量不是数字的，为不规范数据
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription = case when MissingDescription = '' then '' else '，' end + '单件质量不是数字'";
            strSQL += " where ID in " + ListID + " and Replace(LingJian_Type,' ','') = '4' and Material_State = '0' and IsNumeric (Replace(Mat_Rough_Weight,' ','')) = 0";

            DBI.Execute(strSQL);

            //计算物资需求量（kg)
            //物资名称中不含‘棒’的，物资需求量（kg） = 单件质量 * 共计需求件数
            strSQL = " Update M_Demand_DetailedList_Draft set DemandNumSum = Mat_Rough_Weight * NumCasesSum";
            strSQL += " where ID in " + ListID + " and Material_State = '0' and Replace(LingJian_Type,' ','') = '4' and Material_Name not like '%棒%'";
            //物资名称中含‘棒’的，物资需求量（kg）的计算
            //物资定额信息不能被系统识别的，物资规格不是‘φ’+数字的，或者物资尺寸不是以‘L=’开头的
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription  = case when MissingDescription = '' then '' else '，' end + '物资规格不是‘φ+数字’'"
                + " where ID in " + ListID + " and Material_State = '0' and Replace(LingJian_Type,' ','') = '4' and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Spec,' ',''),1,1) != 'φ' or IsNumeric (substring(Replace(Rough_Spec,' ',''),2,len(Replace(Rough_Spec,' ','')))) = 0)";
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription  = case when MissingDescription = '' then '' else '，' end + '物资尺寸不是以‘L=’开头' "
                + " where ID in " + ListID + " and Material_State = '0' and Replace(LingJian_Type,' ','') = '4' and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Size,' ',''),1,2) <> 'l=' and substring(Replace(Rough_Size,' ',''),1,2) <> 'L=')";
            //物资名称中含‘棒’，物资尺寸为‘L=' +数字的，物资需求量（kg） = 单件质量 * 共计需求件数
            strSQL += " Update M_Demand_DetailedList_Draft set DemandNumSum = Mat_Rough_Weight * NumCasesSum"
                + " where ID in " + ListID + " and Material_State = '0' and Replace(LingJian_Type,' ','') = '4' and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Size,' ',''),1,2) = 'l=' or substring(Replace(Rough_Size,' ',''),1,2) = 'L=') and  IsNumeric (substring(Replace(Rough_Size,' ',''),3,len(Replace(Rough_Size,' ','')))) = 1";
            //物资名称中含‘棒’，物资尺寸以‘L=’+数字+字符
            strSQL += " select ID, Rough_Size, Rough_Spec, NumCasesSum, Mat_Rough_Weight from M_Demand_DetailedList_Draft"
                + " where ID in " + ListID + " and Material_State = '0' and Replace(LingJian_Type,' ','') = '4' and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Size,' ',''),1,2) = 'l=' or substring(Replace(Rough_Size,' ',''),1,2) = 'L=') and  IsNumeric (substring(Replace(Rough_Size,' ',''),3,len(Replace(Rough_Size,' ','')))) = 0 ";

            DataTable dt = DBI.Execute(strSQL, true);

            string UpdateID = "('0'";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ID = dt.Rows[i]["ID"].ToString().Trim();
                string size = dt.Rows[i]["Rough_Size"].ToString().Replace(" ", "").Substring(2, dt.Rows[i]["Rough_Size"].ToString().Replace(" ", "").Length - 2);
                if (size == "")
                {
                    UpdateID += " ,'" + ID + "'";
                }
                else
                {
                    if (size.Split('+').Length == 2)
                    {
                        try
                        {
                            double size1 = Convert.ToDouble(size.Split('+')[0]);
                            double size2 = Convert.ToDouble(size.Split('+')[1]);
                            double NumCasesSum = Convert.ToDouble(dt.Rows[i]["NumCasesSum"].ToString());
                            double Mat_Rough_Weight = Convert.ToDouble(dt.Rows[i]["Mat_Rough_Weight"].ToString());

                            if (Convert.ToDouble(dt.Rows[i]["Rough_Spec"].ToString().Substring(1, dt.Rows[i]["Rough_Spec"].ToString().Length - 1)) < pt1)
                            {
                                double sizesum = size1 * NumCasesSum + Math.Ceiling((size1 * NumCasesSum) / pt2) * size2;
                                double DemandNumSum = sizesum * size1 / Mat_Rough_Weight;
                                strSQL = " Update M_Demand_DetailedList_Draft set DemandNumSum = '" + DemandNumSum.ToString() + "'";
                                strSQL += " ,JSGS_Des = '直径小于" + pt1.ToString() + ",总长度每增加" + pt2.ToString() + "增加一个夹持量" + "' where ID = '" + ID + "'";
                                DBI.Execute(strSQL);
                            }
                            else
                            {
                                strSQL = " Update M_Demand_DetailedList_Draft set DemandNumSum =  Mat_Rough_Weight * NumCasesSum";
                                strSQL += " ,JSGS_Des = '直径小于" + pt1.ToString() + ",总长度每增加" + pt2.ToString() + "增加一个夹持量" + "' where ID = '" + ID + "'";
                                DBI.Execute(strSQL);
                            }
                        }
                        catch
                        {
                            UpdateID += " ,'" + ID + "'";
                        }
                    }
                    else
                    {
                        UpdateID += " ,'" + ID + "'";
                    }
                }
            }
            UpdateID += " )";
            strSQL = " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription  = case when MissingDescription = '' then '' else '，' end + '物资尺寸系统不能识别' where ID in " + UpdateID;

            //M_Draft_List的材料定额状态
            //没有材料信息
            strSQL += " if (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and Material_State = '0') = 0 begin"
            + " Update M_Draft_List set Material_State = '0' where PackID = '" + PackID + "' end"
                //待补全
            + " else begin if (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and (Material_State = '4' or Material_State = '5' or Material_State = '6')) > 0"
            + " begin Update M_Draft_List set Material_State = '2' where PackID = '" + PackID + "' end"
                //完成
            + " else begin Update M_Draft_List set Material_State = '1' where PackID = '" + PackID + "'  end end";

            //P_Pack 草稿状态
            strSQL += " if(select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and LingJian_Type = '4' and Material_State='0') = "
                + " (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and LingJian_Type = '4') begin"
                + " Update P_Pack set DraftStatus ='3' where PackID = '" + PackID + "' end"
                + " else begin Update P_Pack set DraftStatus ='2' where PackID = '" + PackID + "' end";

            DBI.Execute(strSQL);
        }

        #endregion

        protected void RB_Expand_Click(object sender, EventArgs e)
        {
            RadButton RB = sender as RadButton;
            string selectedTab = RadTabStrip1.SelectedTab.Value.ToString();

            if (selectedTab == "0")
            {
                RTL_BOM.ExpandAllItems();
            }
            else if (selectedTab == "1")
            {
                RTL_Defect.ExpandAllItems();
            }
            else if (selectedTab == "2")
            {
                RTL_SubmitState.ExpandAllItems();
            }
        }

        protected void RB_Search_BOM_Click(object sender, EventArgs e)
        {
            string TDM_Description = RTB_TDM_Description_BOM.Text.Trim();
            string lingjiantype = RDDL_LingJian_Type_BOM.SelectedItem.Value;
            string drawingNo = RTB_Drawing_No_BOM.Text.Trim();
            string MaterialDept = RTB_MaterialDept_BOM.Text.Trim();


            if (TDM_Description == "" && lingjiantype == "" && drawingNo == "" && MaterialDept == "")
            { Session["SSTBOMWhere"] = null; }
            else
            {
                string where = " and TDM_Description like '%" + TDM_Description + "%'";
                if (lingjiantype == "6")
                {
                    where += " and LingJian_Type not in (1,2,3,4,5)";
                }
                else
                {
                    where += " and LingJian_Type like '%" + lingjiantype + "%'";
                }
                where += " and Drawing_No like '%" + drawingNo + "%'";
                where += " and MaterialDept like '%" + MaterialDept + "%'";
                Session["SSTBOMWhere"] = where;
            }

            Session["GridSourceBOM"] = GetBOM(lingjiantype);
            RTL_BOM.Rebind();
        }

        protected void RB_Search_Defect_Click(object sender, EventArgs e)
        {
            string TDM_Description = RTB_TDM_Description_Defect.Text.Trim();
            string lingjiantype = RDDL_LingJian_Type_Defect.SelectedItem.Value;
            string drawingNo = RTB_Drawing_No_Defect.Text.Trim();


            if (TDM_Description == "" && lingjiantype == "" && drawingNo == "")
            { Session["SSTDefectWhere"] = null; }
            else
            {
                string where = " and TDM_Description like '%" + TDM_Description + "%'";
                where += " and LingJian_Type like '%" + lingjiantype + "%'";
                where += " and Drawing_No like '%" + drawingNo + "%'";
                Session["SSTDefectWhere"] = where;
            }

            Session["GridSourceDefect"] = GetDefect();
            RTL_Defect.Rebind();
        }

        protected void RB_SubmitState_Click(object sender, EventArgs e)
        {
            string TDM_Description = RTB_TDM_Description_SubmitState.Text.Trim();
            string lingjiantype = RDDL_LingJian_Type_SubmitState.SelectedItem.Value;
            string drawingNo = RTB_Drawing_No_SubmitState.Text.Trim();
            string SubmitState = RDDL_SubmitState.SelectedItem.Value;

            if (TDM_Description == "" && lingjiantype == "" && drawingNo == "" && SubmitState == "")
            { Session["SSTSubmitStateWhere"] = null; }
            else
            {
                string where = " and TDM_Description like '%" + TDM_Description + "%'";
                where += " and LingJian_Type like '%" + lingjiantype + "%'";
                where += " and Drawing_No like '%" + drawingNo + "%'";
                if (SubmitState == "1")
                {
                    where += " and Material_State in ('0','2')";
                }
                else if (SubmitState == "2")
                {
                    where += " and Material_State = '1'";
                }
                Session["SSTSubmitStateWhere"] = where;
            }

            Session["GridSourceSubmit"] = GetSubmitState();
            RTL_SubmitState.Rebind();
        }
    }
}