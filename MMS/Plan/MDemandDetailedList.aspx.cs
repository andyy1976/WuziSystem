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
    public partial class MDemandDetailedList : System.Web.UI.Page
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
                    InitTable.Columns.Add("Draft_Num");
                    InitTable.Columns.Add("Material_State");
                    InitTable.Columns.Add("Lasttime_Synchro_Time");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("Task_Type");
                    InitTable.Columns.Add("List_Maker");
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
        private string UserID = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                GridSource = Common.AddTableRowsID(GetDetailedListList());
                this.ViewState["lastSelectDeptCode"] = "";
                this.ViewState["lastSelectAccount"] = "";
            }
        }
        

        protected DataTable GetDetailedListList()
        {
            try
            {
                string strSQL;
                strSQL = "Select * From V_M_Draft_List";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单列表信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_MDemandDetailedList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MDemandDetailedList.DataSource = GridSource;
        }

        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            ((sender as RadButton).NamingContainer as GridItem).Selected = (sender as RadButton).Checked;
        }
        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            RadButton headerCheckBox = (sender as RadButton);
            foreach (GridDataItem dataItem in RadGrid_MDemandDetailedList.MasterTableView.Items)
            {
                (dataItem.FindControl("RadButtonItem") as RadButton).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }
        protected void RadGrid_MDemandDetailedList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            DataTable table = GridSource;
            GridDataItem dataitem = e.Item as GridDataItem;
            if (e.CommandName == "Detail")
            {
                string ID = table.Rows[e.Item.DataSetIndex]["ID"].ToString();
                string Model = table.Rows[e.Item.DataSetIndex]["Model"].ToString();
                string PlanCode = table.Rows[e.Item.DataSetIndex]["PlanCode"].ToString();
                string DraftCode = table.Rows[e.Item.DataSetIndex]["DraftCode"].ToString();
                string draftid = table.Rows[e.Item.DataSetIndex]["draftid"].ToString();
                Response.Redirect("~/Plan/MDemandDetails.aspx?PackId=" + ID + "&DraftCode=" + DraftCode + "&draftid=" + draftid + "&Model=" + Model + "&PlanCode=" + PlanCode + "&inFlag=2");
            }

            if (e.CommandName == "Synchron")
            {
                string PackID = ((e.Item) as GridDataItem).GetDataKeyValue("ID").ToString();
                string strSQL = " select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "'";
                if (DBI.GetSingleValue(strSQL) != "0")
                {
                    return;
                }
                try
                {
                    DBI.OpenConnection();
                    DBI.BeginTrans();

                    GetTask(PackID);
                    //UpdateMaterialState(PackID);

                    DBI.CommitTrans();

                    RadNotificationAlert.Text = "同步成功！";
                    RadNotificationAlert.Show();
                    GridSource = Common.AddTableRowsID(GetDetailedListList());
                    RadGrid_MDemandDetailedList.Rebind();
                }
                catch (Exception ex)
                {
                    DBI.RollbackTrans();
                    RadNotificationAlert.Text = "同步失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
                finally
                {
                    DBI.CloseConnection();
                }
            }
            if (e.CommandName == "Query")
            {
                string ID = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                Response.Redirect("~/Plan/M_Demand_DetailedList_DraftBOM.aspx?PackId=" + ID);
            }
        }

        protected class M_Draft_ListBody
        {
            public int ID { get; set; }
            public string Draft_Num { get; set; }
            public string Material_State { get; set; }
            public string Lasttime_Synchro_Time { get; set; }
            public string PackId { get; set; }
            public string Task_Type { get; set; }
            public string List_Maker { get; set; }
        }


        protected void RadGrid_MDemandDetailedList_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridDataItem)
            //{
            //    DataTable table = GridSource;
            //    GridDataItem dataitem = e.Item as GridDataItem;
            //    if (table.Rows[e.Item.DataSetIndex]["lstime"].ToString() == "未导入")
            //    {
            //        e.Item.FindControl("RadButtonDetail").Visible = false;
            //        RadButton btn = e.Item.FindControl("RadButtonSynchron") as RadButton;
            //        btn.Text = "同步SmarTeam";
            //    }
            //    else
            //    {
            //        RadButton btn = e.Item.FindControl("RadButtonSynchron") as RadButton;
            //        btn.Text = "查看BOM";
            //    }
            //}
        }

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
        }

        private string InsertMDDLD(M_Demand_DetailedList_Draft MDDLD)
        {
            string strSQL = " Insert into M_Demand_DetailedList_Draft"
                + " (VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, Material_Code, ParentId, Material_Spec, TDM_Descration"
                + " , Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, Technics_Comment, Material_Mark, ItemCode1, ItemCode2"
                + " , MaterialsNum, Mat_Unit, Lingjian_Type, Mat_Rough_Weight, Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec"
                + " , Rough_Size, MaterialsDes, StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity"
                + " , Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, User_ID)"
                + " values ('1','" + MDDLD.Class_Id + "','" + MDDLD.Object_Id + "','" + MDDLD.Stage + "','0'"
                + " ,'" + MDDLD.Material_Tech_Condition + "','" + MDDLD.Material_Code + "','" + MDDLD.ParentId + "','" + MDDLD.Material_Spec + "','" + MDDLD.TDM_Description + "'"
                + " ,'" + MDDLD.Material_Name + "','" + MDDLD.PackId + "','" + MDDLD.TaskId + "','" + MDDLD.DraftId + "','" + MDDLD.Drawing_No + "'"
                + " ,'" + MDDLD.Technics_Line + "','" + MDDLD.Technics_Comment + "','" + MDDLD.Material_Mark + "','" + MDDLD.ItemCode1 + "','" + MDDLD.ItemCode2 + "'"
                + " ,Null,'" + MDDLD.Mat_Unit + "','" + MDDLD.Lingjian_Type + "'," + (MDDLD.Mat_Rough_Weight == "" ? "Null" : "'" + MDDLD.Mat_Rough_Weight + "'") + "," + (MDDLD.Mat_Pro_Weight == "" ? "Null" : "'" + MDDLD.Mat_Pro_Weight + "'")
                + " ,'" + MDDLD.Mat_Weight + "','" + MDDLD.Mat_Efficiency + "','" + MDDLD.Mat_Comment + "','" + MDDLD.Mat_Technics + "','" + MDDLD.Rough_Spec + "'"
                + " ,'" + MDDLD.Rough_Size + "',''," + (MDDLD.StandAlone == "" ? "Null" : ("'" + MDDLD.StandAlone + "'")) + "," + (MDDLD.ThisTimeOperation == "" ? "Null" : "'" + MDDLD.ThisTimeOperation + "'") + "," + (MDDLD.PredictDeliveryDate == "" ? "Null" : "'" + MDDLD.PredictDeliveryDate + "'")
                + " ," + (MDDLD.DemandNumSum == "" ? "Null" : "'" + MDDLD.DemandNumSum + "'") + "," + (MDDLD.NumCasesSum == "" ? "Null" : "'" + MDDLD.NumCasesSum + "'") + ",Null," + (MDDLD.Quantity == "" ? "Null" : "'" + MDDLD.Quantity + "'") + ",'" + MDDLD.Tech_Quantity + "'"
                + " ,'" + MDDLD.Memo_Quantity + "','" + MDDLD.Test_Quantity + "','" + MDDLD.Required_Quantity + "','" + MDDLD.Other_Quantity + "','" + MDDLD.Ballon_No + "'"
                + " ,'" + MDDLD.Comment + "','false',GetDate(),'" + UserID + "')"
                + " select @@identity";

            return DBI.GetSingleValue(strSQL);
        }

        private void UpdateMaterialState(string PackID)
        {
            string strSQL = " Update M_Demand_DetailedList_Draft set Material_State = '3' where PackID = '" + PackID + "' and Material_State = '-1' and ID in (select ParentID from M_Demand_DetailedList_Draft)"
                + " Update M_Demand_DetailedList_Draft set Material_State = '4' where PackID = '" + PackID + "' and Material_State = '-1' and  ID not in (select ParentID from M_Demand_DetailedList_Draft)"
                + " and (Material_Name is null or Material_Name = '' or Material_Mark is null or Material_Mark = '' or ItemCode1 is null or ItemCode1 = ''"
                + " or Mat_Unit is null or Mat_Unit = '' or Quantity is null or Mat_Rough_Weight is null or Mat_Rough_Weight = '' or Mat_Pro_Weight is null or Mat_Pro_Weight = ''"
                + " or Rough_Size is null or Rough_Size = '')"
                + " if (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "') = 0 begin"
                + " Update M_Draft_List set Material_State = '0' where PackID = '" + PackID + "' end"
                + " else begin if (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "'"
                + " and ID not in (select ParenID from M_Demand_DetailedList_Draft) and (Material_State = '4' or Material_State = '5')))) = '0'"
                + " begin Update M_Draft_List set Material_State = '1' where PackID = '" + PackID + "' end else begin"
                + " Update M_Draft_List set Material_State = '2' where PackID = '" + PackID + "' end  end";
            DBI.Execute(strSQL);

        }

        private void GetTask(string PackID)
        {
            string strSQL = "";
            string Draft_Code = DBI.GetSingleValue(" Exec  [Proc_CodeBuildByCodeDes1] '材料清单编号','JZWZ'");
            strSQL = " Insert into [dbo].[M_Draft_List] (Draft_Code, Material_State, Lasttime_Synchro_Time, PackId, Task_Type, List_Maker)"
                + " values ('" + Draft_Code + "','0',GetDate(),'" + PackID + "','0','" + UserID + "') select @@identity";
            string DraftId = DBI.GetSingleValue(strSQL);

            strSQL = " select TaskId, PackId, ProductName, TaskDrawingCode, TaskCode, Stage, Unit, MatingNum, DefrayNum, ProductionNum, PlanFinishTime, IsSpread, LastChangeTime, ChangeTimes, IsDel from [dbo].[P_Pack_Task] where PackID = '" + PackID + "' and IsDel = 'false'";
            DataTable dt = DBI.Execute(strSQL, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string TaskID = dt.Rows[i]["TaskID"].ToString();
                string DrawingNo = dt.Rows[i]["TaskDrawingCode"].ToString();
                string Stage = dt.Rows[i]["Stage"].ToString();
                string MatingNum = dt.Rows[i]["MatingNum"].ToString();          //单机配套数量
                string ProductionNum = dt.Rows[i]["ProductionNum"].ToString();  //投产数量
                string PlanFinishTime = dt.Rows[i]["PlanFinishTime"].ToString() == "" ? "" : Convert.ToDateTime(dt.Rows[i]["PlanFinishTime"].ToString()).ToString("yyyy-MM-dd"); //计划交付时间
                string IsSpread = dt.Rows[i]["ProductionNum"].ToString().ToLower();

                strSQL = "select * from [dbo].[InterfaceData] where [CN_Drawing_No] = '" + DrawingNo + "' and State = '" + Stage + "'";
                DataTable dtGetByDrawingNoAndPhase = DBI.Execute(strSQL, true);
                if (dtGetByDrawingNoAndPhase.Rows.Count > 0)
                {
                    M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft();

                    MDDLD.Material_Code = (i + 1).ToString();
                    MDDLD.ParentId = "0";
                    MDDLD.PackId = PackID;
                    MDDLD.TaskId = TaskID;
                    MDDLD.DraftId = DraftId;

                    MDDLD.Class_Id = dtGetByDrawingNoAndPhase.Rows[0]["Class_Id"].ToString();
                    MDDLD.Object_Id = dtGetByDrawingNoAndPhase.Rows[0]["Object_Id"].ToString();
                    MDDLD.Stage = dtGetByDrawingNoAndPhase.Rows[0]["State"].ToString();
                    MDDLD.Material_Tech_Condition = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_Tech_Condition"].ToString();
                    MDDLD.Material_Spec = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_Spec"].ToString();
                    MDDLD.TDM_Description = dtGetByDrawingNoAndPhase.Rows[0]["TDM_Description"].ToString();
                    MDDLD.Material_Name = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_Name"].ToString();
                    MDDLD.Drawing_No = dtGetByDrawingNoAndPhase.Rows[0]["CN_Drawing_No"].ToString();
                    MDDLD.Technics_Line = dtGetByDrawingNoAndPhase.Rows[0]["CN_Technics_Line"].ToString();
                    MDDLD.Technics_Comment = dtGetByDrawingNoAndPhase.Rows[0]["CN_Technics_Comment"].ToString();
                    MDDLD.Material_Mark = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_Mark"].ToString();
                    MDDLD.ItemCode1 = dtGetByDrawingNoAndPhase.Rows[0]["CN_ItemCode1"].ToString();
                    MDDLD.ItemCode2 = dtGetByDrawingNoAndPhase.Rows[0]["CN_ItemCode2"].ToString();
                    MDDLD.Mat_Unit = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Unit"].ToString();
                    MDDLD.Lingjian_Type = dtGetByDrawingNoAndPhase.Rows[0]["CN_Lingjian_Type"].ToString();
                    MDDLD.Mat_Rough_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Rough_Weight"].ToString();
                    MDDLD.Mat_Pro_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Pro_Weight"].ToString();
                    MDDLD.Mat_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Weight"].ToString();
                    MDDLD.Mat_Efficiency = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Efficiency"].ToString();
                    MDDLD.Mat_Comment = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Comment"].ToString();
                    MDDLD.Mat_Technics = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Technics"].ToString();
                    MDDLD.Rough_Spec = dtGetByDrawingNoAndPhase.Rows[0]["CN_Rough_Spec"].ToString();
                    MDDLD.Rough_Size = dtGetByDrawingNoAndPhase.Rows[0]["CN_Rough_Size"].ToString();

                    MDDLD.Quantity = ProductionNum;
                    MDDLD.Tech_Quantity = "";
                    MDDLD.Memo_Quantity = "";
                    MDDLD.Test_Quantity = "";
                    MDDLD.Required_Quantity = "";
                    MDDLD.Other_Quantity = "";
                    MDDLD.Ballon_No = "";
                    MDDLD.Comment = "";

                    MDDLD.StandAlone = MatingNum;               //单机配套数量
                    MDDLD.ThisTimeOperation = ProductionNum;    //本次投产数量
                    MDDLD.PredictDeliveryDate = PlanFinishTime; //计划交付时间

                    MDDLD.DemandNumSum = "";//共计需求量（kg）
                    MDDLD.NumCasesSum = ProductionNum;  //共计需求件数

                    string ID = InsertMDDLD(MDDLD);
                    GetChildren(MDDLD.Object_Id, MDDLD.Class_Id, PackID, TaskID, DraftId, ID, MDDLD.Material_Code, MDDLD.NumCasesSum, "", "", IsSpread);
                }
            }
        }

        private void GetChildren(string ObjectID, string ClassID, string PackID, string TaskID, string DraftId, string ParentID, string Material_Code, string NumCasesSum, string ObjectID1, string ClassID1, string IsSpread)
        {
            string strSQL = " select * from InterfaceData where Object_ID1 = '" + ObjectID + "' and Class_ID1 = '" + ClassID + "'";
            DataTable dtGetchildren = DBI.Execute(strSQL, true);

            for (int i = 0; i < dtGetchildren.Rows.Count; i++)
            {
                M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft();

                MDDLD.Material_Code = Material_Code + "-" + (i + 1).ToString();
                MDDLD.ParentId = ParentID;
                MDDLD.PackId = PackID;
                MDDLD.TaskId = TaskID;
                MDDLD.DraftId = DraftId;

                MDDLD.Class_Id = dtGetchildren.Rows[i]["Class_Id"].ToString();
                MDDLD.Object_Id = dtGetchildren.Rows[i]["Object_Id"].ToString();
                MDDLD.Stage = dtGetchildren.Rows[i]["State"].ToString();
                MDDLD.Material_Tech_Condition = dtGetchildren.Rows[i]["CN_Material_Tech_Condition"].ToString();
                MDDLD.Material_Spec = dtGetchildren.Rows[i]["CN_Material_Spec"].ToString();
                MDDLD.TDM_Description = dtGetchildren.Rows[i]["TDM_Description"].ToString();
                MDDLD.Material_Name = dtGetchildren.Rows[i]["CN_Material_Name"].ToString();
                MDDLD.Drawing_No = dtGetchildren.Rows[i]["CN_Drawing_No"].ToString();
                MDDLD.Technics_Line = dtGetchildren.Rows[i]["CN_Technics_Line"].ToString();
                MDDLD.Technics_Comment = dtGetchildren.Rows[i]["CN_Technics_Comment"].ToString();
                MDDLD.Material_Mark = dtGetchildren.Rows[i]["CN_Material_Mark"].ToString();
                MDDLD.ItemCode1 = dtGetchildren.Rows[i]["CN_ItemCode1"].ToString();
                MDDLD.ItemCode2 = dtGetchildren.Rows[i]["CN_ItemCode2"].ToString();
                MDDLD.Mat_Unit = dtGetchildren.Rows[i]["CN_Mat_Unit"].ToString();
                MDDLD.Lingjian_Type = dtGetchildren.Rows[i]["CN_Lingjian_Type"].ToString();
                MDDLD.Mat_Rough_Weight = dtGetchildren.Rows[i]["CN_Mat_Rough_Weight"].ToString();
                MDDLD.Mat_Pro_Weight = dtGetchildren.Rows[i]["CN_Mat_Pro_Weight"].ToString();
                MDDLD.Mat_Weight = dtGetchildren.Rows[i]["CN_Mat_Weight"].ToString();
                MDDLD.Mat_Efficiency = dtGetchildren.Rows[i]["CN_Mat_Efficiency"].ToString();
                MDDLD.Mat_Comment = dtGetchildren.Rows[i]["CN_Mat_Comment"].ToString();
                MDDLD.Mat_Technics = dtGetchildren.Rows[i]["CN_Mat_Technics"].ToString();
                MDDLD.Rough_Spec = dtGetchildren.Rows[i]["CN_Rough_Spec"].ToString();
                MDDLD.Rough_Size = dtGetchildren.Rows[i]["CN_Rough_Size"].ToString();

                DataTable dtItemsCount = DBI.Execute("select * from InterfaceData where Object_ID = '" + ObjectID + "' and Class_ID = '" + ClassID + "' and Object_ID1 = '" + ObjectID1 + "' and Class_ID1 = '" + ClassID1 + "'", true);

                if (dtItemsCount.Rows.Count > 0)
                {
                    MDDLD.Quantity = dtGetchildren.Rows[i]["CN_Quantity"].ToString();
                    MDDLD.Tech_Quantity = dtGetchildren.Rows[i]["CN_Tech_Quantity"].ToString();
                    MDDLD.Memo_Quantity = dtGetchildren.Rows[i]["CN_Memo_Quantity"].ToString();
                    MDDLD.Test_Quantity = dtGetchildren.Rows[i]["CN_Test_Quantity"].ToString();
                    MDDLD.Required_Quantity = dtGetchildren.Rows[i]["CN_Required_Quantity"].ToString();
                    MDDLD.Other_Quantity = dtGetchildren.Rows[i]["CN_Other_Quantity"].ToString();
                    MDDLD.Ballon_No = dtGetchildren.Rows[i]["CN_Ballon_No"].ToString();
                    MDDLD.Comment = dtGetchildren.Rows[i]["CN_Comment"].ToString();
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

                MDDLD.StandAlone = "";               //单机配套数量
                MDDLD.ThisTimeOperation = "";    //本次投产数量
                MDDLD.PredictDeliveryDate = ""; //计划交付时间

                MDDLD.DemandNumSum = ""; //共计需求量（kg）
                MDDLD.NumCasesSum = "";  //共计需求件数

                if (MDDLD.Quantity != "" && NumCasesSum != "")
                {
                    try
                    {
                        Convert.ToInt32(MDDLD.Quantity);
                    }
                    catch
                    {
                        MDDLD.Quantity = "";
                    }
                    try
                    {
                        if (MDDLD.Quantity != "" && NumCasesSum != "")
                            MDDLD.NumCasesSum = (Convert.ToInt32(MDDLD.Quantity) * Convert.ToInt32(NumCasesSum)).ToString();  //共计需求件数
                    }
                    catch
                    {

                    }
                }

                string ID = InsertMDDLD(MDDLD);
                if (IsSpread == "true")
                {
                    GetChildren(dtGetchildren.Rows[i]["Object_ID"].ToString(), dtGetchildren.Rows[i]["Class_ID"].ToString(), PackID, TaskID, DraftId, ID,
                        MDDLD.Material_Code, MDDLD.NumCasesSum, dtGetchildren.Rows[i]["Object_ID1"].ToString(), dtGetchildren.Rows[i]["Class_ID1"].ToString(), IsSpread);
                }
            }
        }

    }
}