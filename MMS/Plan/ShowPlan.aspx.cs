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
    public partial class ShowPlan : System.Web.UI.Page
    {
        static string DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
        DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
        private static string UserID;
        private List<M_Demand_DetailedList_Draft> ListMDDLD;

        //SmarTeam接口
        private SmarTeam.Items ST = new SmarTeam.Items();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            UserID = Session["UserId"].ToString();
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "ShowPlan", this.Page);
                Session["P_PackWhere"] = null;
                Session["GridSource"] = GetP_Pack();
            }
        }

        protected DataTable GetP_Pack()
        {
            string strSQL = " select P_Pack.PackID, isnull(Sys_Model.Model, P_Pack.Model) as Model, PlanCode, PlanName, State,Type"
                + " , isnull(UserName,ImportStaffId) as UserName , Convert(varchar(100),ImportTime,111) as ImportTime , Draft_Code"
                + " ,(select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and Material_State in ('0','1','2','4','5','6')) as AllCount"
                + " ,(select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and Material_State = '5') as ErrorCount"
                + " ,(select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and Material_State in ('4','6')) as DeficiencyCount"
                + " ,(select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and Material_State in ('0','2','4','5','6')) as NoSubmitCount"
                + " ,(select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and Material_State = '1') as SubmitCount"
                + " ,(select isnull(max(VerCode),(select isnull(max(VerCode),0) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId)) from M_Change_List where PackId = P_Pack.PackId) as MaxVerCode"
                + " from P_Pack"
                + " left join Sys_Model on Convert(nvarchar(50),Sys_Model.ID) = P_Pack.Model"
                + " left join Sys_UserInfo_PWD on Sys_UserInfo_PWD.ID = P_Pack.ImportStaffId"
                + " left join M_Draft_List on M_Draft_List.PackId = P_Pack.PackId"
                + " where P_Pack.Isdel = 'false' and P_Pack.Type=0";
            if (Session["P_PackWhere"] != null)
            {
                strSQL += Session["P_PackWhere"].ToString();
            }
            strSQL += " Order By ImportTime desc, P_Pack.PackId desc";
            return Common.AddTableRowsID(DBI.Execute(strSQL, true));
        }

        protected void RadGridP_Pack_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridP_Pack.DataSource = Session["GridSource"];
        }

        protected void RadGridP_Pack_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                RadButton RB_Add = e.Item.FindControl("RB_Add") as RadButton;
                if (RB_Add != null)
                {
                    if (Common.IsHasRight(Session["UserName"].ToString(), "RB_Add"))
                    {
                        RB_Add.Visible = true;
                    }
                }
            }
            if (e.Item is GridDataItem)
            {
                string PackID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackID"].ToString();
                DataRow datarow = (Session["GridSource"]as DataTable).Select("PackID='" + PackID + "'")[0];
               
                RadButton RB_PlanName = e.Item.FindControl("RB_PlanName") as RadButton;
                RadButton RB_State = e.Item.FindControl("RB_State") as RadButton;
                RadButton RB_NotSynchron = e.Item.FindControl("RB_NotSynchron") as RadButton;
                RadButton RB_Synchronization = e.Item.FindControl("RB_Synchronization") as RadButton;
                RadButton RB_Synchronization1 = e.Item.FindControl("RB_Synchronization1") as RadButton;
                RadButton RB_Change = e.Item.FindControl("RB_Change") as RadButton;
                RadButton RB_ChangeList = e.Item.FindControl("RB_ChangeList") as RadButton;
                RadButton RB_Draft = e.Item.FindControl("RB_Draft") as RadButton;
                RadButton RB_MergeListUpdate = e.Item.FindControl("RB_MergeListUpdate") as RadButton;
                RadButton RB_Delete = e.Item.FindControl("RB_Delete") as RadButton;

                //计划包名称
                if (RB_PlanName != null)
                {
                    RB_PlanName.Text = datarow["PlanName"].ToString();
                    RB_PlanName.Attributes["onclick"] = String.Format("return ShowP_Pack_Task({0});", PackID);
                }
                //计划包状态
                if (RB_State != null)
                {
                    //1:暂存，2：归档
                    if (datarow["State"].ToString() == "1")
                    {
                        RB_State.Image.ImageUrl = "/Images/images/zt_zc.png";
                        //删除 暂存的可以删除，归档的不可以删除
                        if (RB_Delete != null)
                        {
                            RB_Delete.Visible = true;
                        }
                       
                        if (RB_NotSynchron != null)
                        {
                            RB_NotSynchron.Text = "未归档";
                            RB_NotSynchron.ForeColor = Color.Gray;
                            RB_NotSynchron.Visible = true;
                            RB_NotSynchron.Enabled = false;
                            RB_NotSynchron.ToolTip = "";
                        }
                     
                        if (RB_Change != null)
                        {
                            RB_Change.Text = "未归档";
                            RB_Change.ForeColor = Color.Gray;
                            RB_Change.ToolTip = "";
                        }
                        if (RB_ChangeList != null)
                        {
                            RB_ChangeList.Text = "未归档";
                            RB_ChangeList.ForeColor = Color.Gray;
                            RB_ChangeList.ToolTip = "";
                        }
                        if (RB_Draft != null)
                        {
                            RB_Draft.Text = "未归档";
                            RB_Draft.ForeColor = Color.Gray;
                            RB_Draft.ToolTip = "";
                        }
                        if (RB_MergeListUpdate != null)
                        {
                            RB_MergeListUpdate.Text = "未归档";
                            RB_MergeListUpdate.ForeColor = Color.Gray;
                            RB_MergeListUpdate.ToolTip = "";
                        }
                    }
                    else {
                        RB_State.Image.ImageUrl = "/Images/images/zt_gd.png";

                        //未同步
                        if (datarow["Draft_Code"].ToString() == "")
                        {
                            //BOM管理
                            if (RB_NotSynchron != null)
                            {
                                RB_NotSynchron.Visible = true;
                                RB_NotSynchron.Style.Add("cursor", "pointer");
                            }
                            if (RB_Synchronization != null)
                            {
                                RB_Synchronization.Visible = false;
                            }
                            if (RB_Synchronization1 != null)
                            {
                                RB_Synchronization1.Visible = false;
                            }
                            //材料定额变更管理
                            if (RB_Change != null)
                            {
                                RB_Change.Text = "未同步";
                                RB_Change.ForeColor = Color.Gray;
                                RB_Change.ToolTip = "";
                            }
                            //变更查询
                            if (RB_ChangeList != null)
                            {
                                RB_ChangeList.Text = "未同步";
                                RB_ChangeList.ForeColor = Color.Gray;
                                RB_ChangeList.ToolTip = "";
                            }
                            //物资需求清单管理
                            if (RB_Draft != null)
                            {
                                RB_Draft.Text = "未同步";
                                RB_Draft.ForeColor = Color.Gray;
                                RB_Draft.ToolTip = "";
                            }
                            //需求变更
                            if (RB_MergeListUpdate != null)
                            {
                                RB_MergeListUpdate.Text = "未同步";
                                RB_MergeListUpdate.ForeColor = Color.Gray;
                                RB_MergeListUpdate.ToolTip = "";
                            }
                        }
                        //已同步
                        else
                        {
                            //BOM管理
                            if (RB_NotSynchron != null)
                            {
                                RB_NotSynchron.Visible = false;
                            }
                            if (RB_Synchronization != null)
                            {
                                RB_Synchronization.Visible = true;
                                RB_Synchronization.Attributes["onclick"] = string.Format("return ShowSmarTeam({0})", PackID);
                                RB_Synchronization.Style.Add("cursor", "pointer");
                            }
                            if (RB_Synchronization1 != null)
                            {
                                RB_Synchronization1.Text = "【" + datarow["ALLCount"].ToString() + "；";
                                if (datarow["ErrorCount"].ToString() != "0") { RB_Synchronization1.Text += datarow["ErrorCount"].ToString() + "错；"; }
                                if (datarow["DeficiencyCount"].ToString() != "0") { RB_Synchronization1.Text += datarow["DeficiencyCount"].ToString() + "缺；"; }
                                RB_Synchronization1.Text += "】";
                                RB_Synchronization1.Attributes["onclick"] = string.Format("return ShowSmarTeam({0})", PackID);
                                RB_Synchronization1.Visible = true;
                                RB_Synchronization1.Style.Add("cursor", "pointer");
                            }
                            //材料定额变更管理
                            if (RB_Change != null)
                            {
                                RB_Change.Attributes["onclick"] = string.Format("return ChangeMaterialQuota({0})", PackID);
                                RB_Change.Visible = true;
                                RB_Change.Style.Add("cursor", "pointer");
                            }
                            //变更查询
                            if (RB_ChangeList != null)
                            {
                                if (datarow["MaxVerCode"].ToString() == "0" || datarow["MaxVerCode"].ToString() == "1")
                                {
                                    RB_ChangeList.Text = "未变更";
                                    RB_ChangeList.ForeColor = Color.Gray;
                                    RB_ChangeList.ToolTip = "";
                                }
                                else
                                {
                                    string change = "";
                                    try
                                    {
                                        change = (Convert.ToInt32(datarow["MaxVerCode"]) - 1).ToString();
                                    }
                                    catch { change = datarow["MaxVerCode"].ToString(); }
                                    RB_ChangeList.Text = "已变更" + change + "次";
                                    RB_ChangeList.Attributes["onclick"] = string.Format("return ShowM_Change({0})", PackID);
                                    RB_ChangeList.Style.Add("cursor", "pointer");
                                }
                            }
                            //物资需求清单管理
                            if (RB_Draft != null)
                            {
                                if (datarow["AllCount"].ToString() != "0" && datarow["NoSubmitCount"].ToString() == "0")
                                {
                                    RB_Draft.Text = "已完成";
                                }
                                else
                                {
                                    RB_Draft.Text = "【" + datarow["SubmitCount"].ToString() + "已提；" + datarow["NoSubmitCount"].ToString() + "未提】";
                                }
                                RB_Draft.Attributes["onclick"] = string.Format("return ShowMDemandDetails({0})", PackID);
                                RB_Draft.ForeColor = Color.Blue;
                                RB_Draft.ToolTip = "点击查询物资清单";
                                RB_Draft.Style.Add("cursor", "pointer");
                            }
                            //需求变更
                            if (RB_MergeListUpdate != null)
                            {
                                if (datarow["SubmitCount"].ToString() == "0")
                                {
                                    RB_MergeListUpdate.Text = "未提交";
                                    RB_MergeListUpdate.ForeColor = Color.Gray;
                                    RB_MergeListUpdate.ToolTip = "";
                                }
                                else
                                {
                                    RB_MergeListUpdate.Attributes["onclick"] = string.Format("return ShowMDemandMergeListUpdate({0})", PackID);
                                    RB_MergeListUpdate.Style.Add("cursor", "pointer");
                                }
                            }
                        }
                    }
                    RB_State.Attributes["onclick"] = String.Format("return ShowP_Pack_Task({0});", PackID);
                }

                
            }
        }

        protected void RadGridP_Pack_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Synchron")
            {
                if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
                //System.Threading.Thread.Sleep(5000);
                string PackID = ((e.Item) as GridDataItem).GetDataKeyValue("PackID").ToString();
                string strSQL = " select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "'";
                if (DBI.GetSingleValue(strSQL) != "0")
                {
                    RadNotificationAlert.Text = "已同步，此处不可以再同步，请去变更管理";
                    RadNotificationAlert.Show();
                    return;
                }

                //ListMDDLD = new List<M_Demand_DetailedList_Draft>();
                //获取SmarTeam数据
                //var result = GetTask(PackID);
                //if (result != "")
                //{
                //    RadNotificationAlert.Text = result;
                //    RadNotificationAlert.Show();
                //    return;
                //}
                var db = new MMSDbDataContext();
                SmarTeamBLL bll = new SmarTeamBLL();
                var result = bll.GetTask(Convert.ToInt32(PackID), Convert.ToInt32(Session["UserId"]));
                if (result.ErrMsg != "")
                {
                    RadNotificationAlert.Text = result.ErrMsg;
                    RadNotificationAlert.Show();
                    return;
                }

                try
                {
                    DBI.OpenConnection();
                    DBI.BeginTrans();
                    //将取回来的SmarTeam数据插入数据库中
                    if (result.Mddld.Count == 0)
                    //if (ListMDDLD.Count == 0)
                    {
                        RadNotificationAlert.Text = "失败！StarTeam中没有相关数据";
                        RadNotificationAlert.Show();
                    }
                    else
                    {
                        //for (int i = 0; i < ListMDDLD.Count; i++)
                        //{
                        //    if (ListMDDLD[i].ParentId == "-1") { ListMDDLD[i].ParentId = "0"; }
                        //    else { ListMDDLD[i].ParentId = ListMDDLD[Convert.ToInt32(ListMDDLD[i].ParentId)].Id; }
                        //    string id = InsertMDDLD(ListMDDLD[i]);
                        //    ListMDDLD[i].Id = id;
                        //}
                        for (int i = 0; i < result.Mddld.Count; i++)
                        {
                            if (result.Mddld[i].ParentId == -1) { result.Mddld[i].ParentId = 0; }
                            else { result.Mddld[i].ParentId = result.Mddld[Convert.ToInt32(result.Mddld[i].ParentId)].Id; }
                            db.M_Demand_DetailedList_Draft.InsertOnSubmit(result.Mddld[i]);
                            db.SubmitChanges();
                        }
                       

                        string Draft_Code = DBI.GetSingleValue(" Exec [Proc_CodeBuildByCodeDes1] '材料清单编号','JZWZ'");
                        strSQL = " declare @draftId nvarchar(50) Insert into [dbo].[M_Draft_List] (Draft_Code, Material_State, Lasttime_Synchro_Time, PackId, Task_Type, List_Maker)"
                            + " values ('" + Draft_Code + "','0',GetDate(),'" + PackID + "','0','" + UserID + "') select @draftId = @@identity"
                            + " Update M_Demand_DetailedList_Draft set DraftId = @draftId where PackId = '" + PackID + "'";
                        DBI.Execute(strSQL);

                        //计算共计需求数量（kg)
                        UpdateDemandNumSum(PackID);

                        DBI.CommitTrans();

                        RadNotificationAlert.Text = "同步成功！";
                        RadNotificationAlert.Show();
                        Session["GridSource"] = GetP_Pack();
                        RadGridP_Pack.Rebind();
                    }
                }
                catch (Exception ex)
                {
                    strSQL = " delete M_Draft_List where PackId = '" + PackID + "'";
                    DBI.Execute(strSQL);

                    DBI.RollbackTrans();
                    RadNotificationAlert.Text = "同步失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
                finally
                {
                    DBI.CloseConnection();
                }
            }
            if (e.CommandName == "Draft")
            {
                string PackID = ((GridDataItem)e.Item).GetDataKeyValue("PackID").ToString();
                DataTable dt = GetMDraftList(PackID);
                string Model = dt.Rows[0]["model_1"].ToString();
                string PlanCode = dt.Rows[0]["PlanCode"].ToString();
                string DraftCode = dt.Rows[0]["DraftCode"].ToString();
                string draftid = dt.Rows[0]["draftid"].ToString();
                RadButton RB_Draft = e.Item.FindControl("RB_Draft") as RadButton;
               
               // Response.Redirect("~/Plan/MDemandDetails.aspx?PackId=" + PackID + "&DraftCode=" + DraftCode + "&draftid=" + draftid + "&Model=" + Model + "&PlanCode=" + PlanCode + "&inFlag=1");
                Response.Redirect("~/Plan/MDemandDetailsTreeList.aspx?PackId=" + PackID + "&DraftCode=" + DraftCode + "&draftid=" + draftid + "&Model=" + Model + "&PlanCode=" + PlanCode + "&inFlag=1");
                    
            }
            if (e.CommandName == "Delete")
            {
                string PackId = ((GridDataItem)e.Item).GetDataKeyValue("PackID").ToString();
                string strSQL = "select count(*) from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Material_State = '1'";
                if (DBI.GetSingleValue(strSQL) == "0")
                {
                    strSQL = " Update P_Pack set IsDel = 'true' where PackId = '" + PackId + "'";
                    DBI.Execute(strSQL);
                    (Session["GridSource"]as DataTable).Select("PackID='" + PackId + "'")[0].Delete();
                    RadGridP_Pack.Rebind();
                    RadNotificationAlert.Text = "删除成功！";
                    RadNotificationAlert.Show();
                }
                else
                {
                    RadNotificationAlert.Text = "失败！已经提交物资需求，不可以删除";
                    RadNotificationAlert.Show();
                }
            }
        }

        protected void RB_Query_Click(object sender, EventArgs e)
        {
            string Model = RTB_Model.Text.Trim();
            string DS = RDDL_DS.SelectedItem.Value;
            string ID = RDDL_ID.SelectedItem.Value;
            string UserName = RTB_UserName.Text.Trim();
            DateTime? StartDate = RDP_Start.SelectedDate;
            DateTime? EndDate = RDP_End.SelectedDate;
        /*    string TaskCode = RTB_TaskCode.Text.Trim();
            string DrawingNo = RTB_DrawingNo.Text.Trim();
         */
            string PackageName = RTB_PackageName.Text.Trim();
            string strWhere = "";
            if (Model != "")
            {
                strWhere += " and Sys_Model.Model like '%" + Model + "%'";
            }
            if (DS != "")
            {
                strWhere += " and DraftStatus = '" + DS + "'";
            }
            if (ID != "")
            {
                strWhere += " and InvertoryStatus = '" + ID + "'";
            }
            if (UserName != "")
            {
                strWhere += " and UserName like '%" + UserName + "%'";
            }
            if (StartDate != null)
            {
                strWhere += " and ImportTime >= '" + Convert.ToDateTime(StartDate).ToString("yyyy-MM-dd") + "'";
            }
            if (EndDate != null)
            {
                strWhere += " and ImportTime <= '" + Convert.ToDateTime(EndDate).ToString("yyyy-MM-dd") + "'";
            }

            if (PackageName != "")
            {
                strWhere += " and P_Pack.PlanName like '%" + PackageName + "%'";
            }
            /*
            if (TaskCode != "")
            {
                strWhere += " and P_Pack.PackId in (select PackId from P_Pack_Task where TaskCode like '%" + TaskCode + "%')";
            }
            if (DrawingNo != "")
            {
                strWhere += " and P_Pack.PackId in (select PackId from M_Demand_DetailedList_Draft where Drawing_No like '%" + DrawingNo + "%')";
            }
             */
            Session["P_PackWhere"] = strWhere;
            Session["GridSource"]= GetP_Pack();
            RadGridP_Pack.Rebind();
        }

        #region M_Demand_DetailedList_Draft属性
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

        #region 获取SmarTeam数据
        private string GetTask(string PackId)
        {
            string strSQL = "";
            strSQL = " select TaskId, PackId, ProductName, TaskDrawingCode, TaskCode, Stage, Unit, MatingNum, DefrayNum, ProductionNum, PlanFinishTime"
                + " , IsSpread, LastChangeTime, ChangeTimes, IsDel"
                + " , (select top 1 case when IsGetBOM = 'true' then AreaCode else '' end from P_Pack left join Sys_Model on Sys_Model.Id = P_Pack.Model left join Sys_Area on Sys_Area.Id = Sys_Model.AreaId where PackId = '" + PackId + "') as AreaCode"
                + " from P_Pack_Task where PackID = '" + PackId + "' and IsDel = 'false'";
            DataTable dt = DBI.Execute(strSQL, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string IsSpread = dt.Rows[i]["IsSpread"].ToString().ToLower();

                M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft();

                MDDLD.Id = ListMDDLD.Count.ToString();
                MDDLD.Material_Spec = "";
                MDDLD.MaterialsNum = "";
                MDDLD.Is_allow_merge = "";
                MDDLD.DemandDate = "";

                MDDLD.VerCode = "1";
                MDDLD.Material_State = "0";

                MDDLD.MaterialsDes = "";

                MDDLD.PackId = PackId;
                MDDLD.DraftId = "";
                MDDLD.TaskId = dt.Rows[i]["TaskID"].ToString();
                MDDLD.TaskCode = dt.Rows[i]["TaskCode"].ToString();
                MDDLD.Material_Code = (i + 1).ToString();
                MDDLD.Drawing_No = dt.Rows[i]["TaskDrawingCode"].ToString();
                MDDLD.Stage = dt.Rows[i]["Stage"].ToString();

                MDDLD.ParentId = "-1";
                MDDLD.NumCasesSum = "";
                MDDLD.DemandNumSum = "";
                MDDLD.MaterailDept = "";
                MDDLD.MissingDescription = "";

                MDDLD.StandAlone = dt.Rows[i]["MatingNum"].ToString();          //单机配套数量
                MDDLD.ThisTimeOperation = dt.Rows[i]["ProductionNum"].ToString();  //投产数量
                MDDLD.PredictDeliveryDate = dt.Rows[i]["PlanFinishTime"].ToString() == "" ? "" : Convert.ToDateTime(dt.Rows[i]["PlanFinishTime"].ToString()).ToString("yyyy-MM-dd"); //计划交付时间

                MDDLD.Import_Date = "";
                MDDLD.User_ID = UserID;

                //SmarTeam接口数据
                DataTable dtGetByDrawingNoAndPhase = new DataTable();
                try
                {
                    dtGetByDrawingNoAndPhase =
                        ST.GetByDrawingNoAndPhase(MDDLD.Drawing_No, MDDLD.Stage, dt.Rows[i]["AreaCode"].ToString())
                            .Tables[0];
                }
                catch
                {
                    return "同步失败！<br />不能与SmarTeam通信，<br />请联系管理员。";
                }

                if (dtGetByDrawingNoAndPhase.Rows.Count > 0)
                {
                    MDDLD.Class_Id = dtGetByDrawingNoAndPhase.Rows[0]["Class_Id"].ToString().Trim();
                    MDDLD.Object_Id = dtGetByDrawingNoAndPhase.Rows[0]["Object_Id"].ToString().Trim();
                    MDDLD.Stage = dtGetByDrawingNoAndPhase.Rows[0]["Phase"].ToString().Trim();
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
                    MDDLD.CN_Material_State = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_State"].ToString();

                    MDDLD.Quantity = MDDLD.ThisTimeOperation;
                    MDDLD.Tech_Quantity = "";
                    MDDLD.Memo_Quantity = "";
                    MDDLD.Test_Quantity = "";
                    MDDLD.Required_Quantity = "";
                    MDDLD.Other_Quantity = "";
                    MDDLD.Ballon_No = "";
                    MDDLD.Comment = "";

                    MDDLD.NumCasesSum = MDDLD.ThisTimeOperation;  //共计需求件数

                    ListMDDLD.Add(MDDLD);

                    GetChildren(MDDLD.Object_Id, MDDLD.Class_Id, MDDLD.PackId, MDDLD.TaskId, MDDLD.Id, MDDLD.Material_Code, MDDLD.NumCasesSum, IsSpread, MDDLD.TaskCode);
                }
            }
            return "";
        }

        private void GetChildren(string ObjectID, string ClassID, string PackId, string TaskId, string ParentID, string Material_Code, string NumCasesSum, string IsSpread, string TaskCode)
        {
            //SmarTeam接口数据
            DataTable dtGetChildren = ST.GetChildren(ClassID, ObjectID).Tables[0];

            for (int i = 0; i < dtGetChildren.Rows.Count; i++)
            {
                M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft();

                MDDLD.Id = ListMDDLD.Count.ToString();
                MDDLD.Class_Id = dtGetChildren.Rows[i]["Class_Id"].ToString().Trim();
                MDDLD.Object_Id = dtGetChildren.Rows[i]["Object_Id"].ToString().Trim();
                MDDLD.Stage = dtGetChildren.Rows[i]["Phase"].ToString().Trim();
                MDDLD.Material_Tech_Condition = dtGetChildren.Rows[i]["CN_Material_Tech_Condition"].ToString().Trim();
                MDDLD.Material_Spec = dtGetChildren.Rows[i]["CN_Material_Spec"].ToString().Trim();
                MDDLD.TDM_Description = dtGetChildren.Rows[i]["TDM_Description"].ToString().Trim();
                MDDLD.Material_Name = dtGetChildren.Rows[i]["CN_Material_Name"].ToString().Trim();
                MDDLD.Drawing_No = dtGetChildren.Rows[i]["CN_Drawing_No"].ToString().Trim();
                MDDLD.Technics_Line = dtGetChildren.Rows[i]["CN_Technics_Line"].ToString().Trim();
                MDDLD.Technics_Comment = dtGetChildren.Rows[i]["CN_Technics_Comment"].ToString().Trim();
                MDDLD.Material_Mark = dtGetChildren.Rows[i]["CN_Material_Mark"].ToString().Trim();
                MDDLD.ItemCode1 = dtGetChildren.Rows[i]["CN_ItemCode1"].ToString().Trim();
                MDDLD.ItemCode2 = dtGetChildren.Rows[i]["CN_ItemCode2"].ToString().Trim();
                MDDLD.Mat_Unit = dtGetChildren.Rows[i]["CN_Mat_Unit"].ToString().Trim();
                MDDLD.Lingjian_Type = dtGetChildren.Rows[i]["CN_LingJian_Type"].ToString().Trim();
                MDDLD.Mat_Rough_Weight = dtGetChildren.Rows[i]["CN_Mat_Rough_Weight"].ToString().Trim();
                MDDLD.Mat_Pro_Weight = dtGetChildren.Rows[i]["CN_Mat_Pro_Weight"].ToString().Trim();
                MDDLD.Mat_Weight = dtGetChildren.Rows[i]["CN_Mat_Weight"].ToString().Trim();
                MDDLD.Mat_Efficiency = dtGetChildren.Rows[i]["CN_Mat_Efficiency"].ToString().Trim();
                MDDLD.Mat_Comment = dtGetChildren.Rows[i]["CN_Mat_Comment"].ToString().Trim();
                MDDLD.Mat_Technics = dtGetChildren.Rows[i]["CN_Mat_Technics"].ToString().Trim();
                MDDLD.Rough_Spec = dtGetChildren.Rows[i]["CN_Rough_Spec"].ToString().Trim();
                MDDLD.Rough_Size = dtGetChildren.Rows[i]["CN_Rough_Size"].ToString().Trim();
                MDDLD.CN_Material_State = dtGetChildren.Rows[i]["CN_Material_State"].ToString();

                //SmarTeam数据
                DataTable dtItemsCount = ST.GetItemsCount(dtGetChildren.Rows[i]["CLASS_ID1"].ToString().Trim(), dtGetChildren.Rows[i]["OBJECT_ID1"].ToString().Trim(), dtGetChildren.Rows[i]["CLASS_ID"].ToString().Trim(), dtGetChildren.Rows[i]["OBJECT_ID"].ToString().Trim()).Tables[0];

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

                MDDLD.Material_Spec = "";
                MDDLD.MaterialsNum = "";
                MDDLD.Is_allow_merge = "";
                MDDLD.DemandDate = "";

                MDDLD.VerCode = "1";
                MDDLD.Material_State = "0";

                MDDLD.MaterialsDes = "";

                MDDLD.PackId = PackId;
                MDDLD.DraftId = "";
                MDDLD.TaskId = TaskId;
                MDDLD.TaskCode = TaskCode;
                MDDLD.Material_Code = Material_Code + "-" + (i + 1).ToString();

                MDDLD.ParentId = ParentID;
                MDDLD.NumCasesSum = "";  //共计需求件数
                MDDLD.DemandNumSum = ""; //共计需求量（kg）
                MDDLD.MaterailDept = "";
                MDDLD.MissingDescription = "";

                MDDLD.StandAlone = "";          //单机配套数量
                MDDLD.ThisTimeOperation = "";  //投产数量
                MDDLD.PredictDeliveryDate = ""; //计划交付时间

                MDDLD.Import_Date = "";
                MDDLD.User_ID = UserID;


                if (MDDLD.Quantity != "" && NumCasesSum != "")
                {
                    try
                    {
                        Convert.ToDouble(MDDLD.Quantity);
                        MDDLD.NumCasesSum = (Convert.ToDouble(MDDLD.Quantity) * Convert.ToDouble(NumCasesSum)).ToString();  //共计需求件数
                    }
                    catch { }
                }
                ListMDDLD.Add(MDDLD);

                if (IsSpread == "true")
                {
                    GetChildren(MDDLD.Object_Id, MDDLD.Class_Id, MDDLD.PackId, MDDLD.TaskId, MDDLD.Id,
                        MDDLD.Material_Code, MDDLD.NumCasesSum, IsSpread, MDDLD.TaskCode);
                }
            }
        }
        #endregion

        #region 将SmarTeam数据插入数据库，修改M_Demand_DetailedList_Draft表材料状态，计算共计需求量（kg）
        private string InsertMDDLD(M_Demand_DetailedList_Draft MDDLD)
        {
            string strSQL = " Insert into M_Demand_DetailedList_Draft"
                + " (VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, Material_Code, ParentId, Material_Spec, TDM_Description"
                + " , Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, Technics_Comment, Material_Mark, ItemCode1, ItemCode2"
                + " , MaterialsNum, Mat_Unit, Lingjian_Type, Mat_Rough_Weight, Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec"
                + " , Rough_Size, MaterialsDes, StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity"
                + " , Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, User_ID, Is_del"
                + " ,TaskCode, MaterialDept, MissingDescription, CN_Material_State)"
                + " values ('1','" + MDDLD.Class_Id + "','" + MDDLD.Object_Id + "','" + MDDLD.Stage + "','0'"
                + " ,'" + MDDLD.Material_Tech_Condition + "','" + MDDLD.Material_Code + "','" + MDDLD.ParentId + "','" + MDDLD.Material_Spec + "','" + MDDLD.TDM_Description + "'"
                + " ,'" + MDDLD.Material_Name + "','" + MDDLD.PackId + "','" + MDDLD.TaskId + "','" + MDDLD.DraftId + "','" + MDDLD.Drawing_No + "'"
                + " ,'" + MDDLD.Technics_Line + "','" + MDDLD.Technics_Comment + "','" + MDDLD.Material_Mark + "','" + MDDLD.ItemCode1 + "','" + MDDLD.ItemCode2 + "'"
                + " ,Null,'" + MDDLD.Mat_Unit + "','" + MDDLD.Lingjian_Type + "'," + (MDDLD.Mat_Rough_Weight == "" ? "Null" : "'" + MDDLD.Mat_Rough_Weight + "'") + "," + (MDDLD.Mat_Pro_Weight == "" ? "Null" : "'" + MDDLD.Mat_Pro_Weight + "'")
                + " ,'" + MDDLD.Mat_Weight + "','" + MDDLD.Mat_Efficiency + "','" + MDDLD.Mat_Comment + "','" + MDDLD.Mat_Technics + "','" + MDDLD.Rough_Spec + "'"
                + " ,'" + MDDLD.Rough_Size + "',''," + (MDDLD.StandAlone == "" ? "Null" : ("'" + MDDLD.StandAlone + "'")) + "," + (MDDLD.ThisTimeOperation == "" ? "Null" : "'" + MDDLD.ThisTimeOperation + "'") + "," + (MDDLD.PredictDeliveryDate == "" ? "Null" : "'" + MDDLD.PredictDeliveryDate + "'")
                + " ," + (MDDLD.DemandNumSum == "" ? "Null" : "'" + MDDLD.DemandNumSum + "'") + "," + (MDDLD.NumCasesSum == "" ? "Null" : "'" + MDDLD.NumCasesSum + "'") + ",Null," + (MDDLD.Quantity == "" ? "Null" : "'" + MDDLD.Quantity + "'") + ",'" + MDDLD.Tech_Quantity + "'"
                + " ,'" + MDDLD.Memo_Quantity + "','" + MDDLD.Test_Quantity + "','" + MDDLD.Required_Quantity + "','" + MDDLD.Other_Quantity + "','" + MDDLD.Ballon_No + "'"
                + " ,'" + MDDLD.Comment + "','false',GetDate(),'" + UserID + "','false'"
                + " ,'" + MDDLD.TaskCode + "', '" + MDDLD.MaterailDept + "','" + MDDLD.MissingDescription + "','" + MDDLD.CN_Material_State + "')"
                + " select @@identity";

            return DBI.GetSingleValue(strSQL);
        }
        //修改M_Demand_DetailedList_Draft表材料状态，计算共计需求量（kg）
        private void UpdateDemandNumSum(string PackID)
        {
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
            
            //不需要材料定额的(根据零件类型判断是否需要材料定额)，状态为3
            strSQL = " Update M_Demand_DetailedList_Draft set Material_State = '3'";
            strSQL += " where PackID = '" + PackID + "' and Replace(LingJian_Type,' ','') not in (select LingJian_Type_Code from Sys_LingJian_Info where Is_MDDLD_Show = 'true')";
            //需要材料定额的，修改物资领料部门
            //strSQL += " Update M_Demand_DetailedList_Draft set MaterialDept = substring(Technics_Line ,5,2)";
            //strSQL += " where PackID = '" + PackID + "' and Material_State = '0'";
            //strSQL += " and (Technics_Line like '100-51%' or Technics_Line like '100-53%' or Technics_Line like '100-55%' or Technics_Line like '100-56%' or Technics_Line like '100-57%' or Technics_Line like '100-58%')";
            //strSQL += " Update M_Demand_DetailedList_Draft set MaterialDept = substring(Technics_Line ,1,2)";
            //strSQL += " where PackID = '" + PackID + "'  and Material_State = '0'";
            //strSQL += " and (Technics_Line like '51-%' or Technics_Line like '53-%' or Technics_Line like '55-%' or Technics_Line like '56-%' or Technics_Line like '57-%' or Technics_Line like '58-%')";

            strSQL += " update M_Demand_DetailedList_Draft set MaterialDept=dbo.F_SearchMaterialDept(Technics_Line) where PackId = '" + PackID + "' and Material_State=0";
            
            //物资领料部门为空的，不需要材料定额信息，状态为3
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '3' where PackID = '" + PackID + "' and Material_State = '0' and MaterialDept = ''";
            
            //需要材料定额的，缺失材料定额数据， 状态为4
            //缺失牌号
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '牌号'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (Material_Mark is null or REPLACE(Material_Mark,' ','') = '')";
            //缺失物资名称
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '物资名称'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (Material_Name is null or REPLACE(Material_Name,' ','')= '') ";
            //缺失物资编码
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '物资编码'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (ItemCode1 is null or REPLACE(ItemCode1,' ','') = '')";
            //缺失计量单位
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '计量单位'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (Mat_Unit is null or REPLACE(Mat_Unit,' ','') = '')";
            //缺失物资件数
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '物资件数'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (Quantity is null or Quantity = '')";
            //缺失单件质量
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '单件质量'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (Mat_Rough_Weight is null or REPLACE(Mat_Rough_Weight,' ','') = '')";
            //缺失每产品质量
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '每产品质量'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (Mat_Pro_Weight is null or REPLACE(Mat_Pro_Weight,' ','') = '')";
            //缺失物资规格
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '物资规格'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (Rough_Spec is null or REPLACE(Rough_Spec,' ','') = '')";
            //缺失物资尺寸
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '4', MissingDescription = MissingDescription + case when MissingDescription = '' then '缺失' else '、' end + '物资尺寸'";
            strSQL += " where PackID = '" + PackID + "' and Material_State <> '3' and (Rough_Size is null or REPLACE(Rough_Size,' ','') = '')";
           
            //需要材料定额的，定额材料数据不规范的， 状态为5
            //单件质量、产品数量不是数字的，为不规范数据
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription = MissingDescription + case when MissingDescription = '' then '' else '，' end + '单件质量不是数字'"
                + " where PackID = '" + PackID + "' and Material_State <> '3' and IsNumeric (Replace(Mat_Rough_Weight,' ','')) = 0";
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription = MissingDescription + case when MissingDescription = '' then '' else '，' end + '产品数量不是数字'"
                + " where PackID = '" + PackID + "' and Material_State <> '3' and IsNumeric (Replace(Quantity,' ','')) = 0";
            //物资名称中含‘棒’，物资规格不是‘φ’+数字的，或者物资尺寸不是以‘L=’开头的
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription  = MissingDescription + case when MissingDescription = '' then '' else '，' end + '物资规格不是‘φ+数字’' "
                + " where PackID = '" + PackID + "' and Material_State <> '3' and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Spec,' ',''),1,1) != 'φ' or IsNumeric (substring(Replace(Rough_Spec,' ',''),2,len(Replace(Rough_Spec,' ','')))) = 0)";
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription  = MissingDescription + case when MissingDescription = '' then '' else '，' end + '物资尺寸不是以‘L=’开头' "
                + " where PackID = '" + PackID + "' and Material_State <> '3' and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Size,' ',''),1,2) <> 'l=' and substring(Replace(Rough_Size,' ',''),1,2) <> 'L=')";

            DBI.Execute(strSQL);

            //计算物资需求量（kg)
            //物资名称中不含‘棒’的，物资需求量（kg） = 单件质量 * 共计需求件数
            strSQL = " Update M_Demand_DetailedList_Draft set DemandNumSum = Mat_Rough_Weight * NumCasesSum";
            strSQL += " where PackID = '" + PackID + "' and Material_State = '0' and Material_Name not like '%棒%'";
            //物资名称中含‘棒’的，物资需求量（kg）的计算            
            //物资名称中含‘棒’，物资尺寸为‘L=' +数字的，物资需求量（kg） = 单件质量 * 共计需求件数
            strSQL += " Update M_Demand_DetailedList_Draft set DemandNumSum = Mat_Rough_Weight * NumCasesSum"
                + " where PackID = '" + PackID + "' and Material_State = '0' and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Size,' ',''),1,2) = 'l=' or substring(Replace(Rough_Size,' ',''),1,2) = 'L=') and  IsNumeric (substring(Replace(Rough_Size,' ',''),3,len(Replace(Rough_Size,' ','')))) = 1";
            //物资名称中含‘棒’，物资尺寸以‘L=’+非数字
            strSQL += " select ID, Rough_Size, Rough_Spec, NumCasesSum, Mat_Rough_Weight from M_Demand_DetailedList_Draft"
                + " where PackID = '" + PackID + "' and Material_State = '0' and Material_Name like '%棒%'"
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

                            if (Convert.ToDouble(dt.Rows[i]["Rough_Spec"].ToString().Substring(1,dt.Rows[i]["Rough_Spec"].ToString().Length -1)) < pt1)
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
            //物资尺寸不能被系统识别
            strSQL = " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription  = MissingDescription + case when MissingDescription = '' then '' else '，' end + '物资尺寸系统不能识别' where ID in " + UpdateID;

            //修改P_Pack 材料定额状态
            //没有材料信息
            strSQL += " if (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and Material_State = '0') = 0 begin"
            + " Update M_Draft_List set Material_State = '0' where PackID = '" + PackID + "'  Update P_Pack set DraftStatus = '1' where PackId = '" + PackID + "' end"
                //待补全
            + " else begin if (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and (Material_State = '4' or Material_State = '5' or Material_State = '6')) > 0"
            + " begin Update M_Draft_List set Material_State = '2' where PackID = '" + PackID + "' Update P_Pack set DraftStatus = '2' where PackID = '" + PackID + "' end"
                //完成
            + " else begin Update M_Draft_List set Material_State = '1' where PackID = '" + PackID + "' Update P_Pack set DraftStatus = '3' where PackID = '" + PackID + "' end end";

            DBI.Execute(strSQL);
        }
        #endregion

        protected DataTable GetMDraftList(string packid)
        {
            try
            {
                string strSQL = "Select * From V_M_Draft_List where packid='" + packid + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                Session["P_PackWhere"] = null;
                Session["GridSource"]= GetP_Pack();
                RadGridP_Pack.Rebind();
            }
            else
            {
                throw new Exception("刷新页面出错，请按F5刷新！");
            }
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGridP_Pack.ExportSettings.FileName = " 型号投产计划包列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridP_Pack.MasterTableView.ExportToExcel();
        }
    }
}