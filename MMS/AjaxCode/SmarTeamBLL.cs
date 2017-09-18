using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Camc.Web.Library;

namespace mms
{
    public class SmarTeamBLL
    {
        private SmarTeam.Items ST = new SmarTeam.Items();
        private static string DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
        private DBInterface DBI = DBFactory.GetDBInterface(DBConn);
        List<M_Demand_DetailedList_Draft> ListMDDLD = new List<M_Demand_DetailedList_Draft>();

        #region 获取SmarTeam数据
        public SynchronizationSmarTeam GetTask(int PackId, int UserID)
        {
            SynchronizationSmarTeam sst = new SynchronizationSmarTeam();
            ListMDDLD = new List<M_Demand_DetailedList_Draft>();
            sst.ErrMsg = "";
            sst.Mddld = ListMDDLD;

            string strSQL = "";
            strSQL = " select TaskId, PackId, ProductName, TaskDrawingCode, TaskCode, Stage, Unit, MatingNum, DefrayNum, ProductionNum, PlanFinishTime"
                + " , IsSpread, LastChangeTime, ChangeTimes, IsDel"
                + " , (select top 1 case when IsGetBOM = 'true' then AreaCode else '' end from P_Pack left join Sys_Model on Sys_Model.Id = P_Pack.Model left join Sys_Area on Sys_Area.Id = Sys_Model.AreaId where PackId = '" + PackId + "') as AreaCode"
                + " from P_Pack_Task where PackID = '" + PackId + "' and IsDel = 'false'";
            DataTable dt = DBI.Execute(strSQL, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string IsSpread = dt.Rows[i]["IsSpread"].ToString().ToLower();

                M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft()
                {
                    Ballon_No = "",
                    Class_Id = null,
                    CN_Material_State = "",
                    Comment = "",
                    DemandDate = "",
                    DemandNumSum = null,
                    DraftId = null,
                    Drawing_No =  dt.Rows[i]["TaskDrawingCode"].ToString(),
                    Id = ListMDDLD.Count,
                    Import_Date = DateTime.Now,
                    Is_allow_merge = null,
                    Is_del = false,
                    ItemCode1 = "",
                    ItemCode2 = "",
                    JSGS_Des = "",
                    LingJian_Type = "",
                    Mat_Comment = "",
                    Mat_Efficiency = "",
                    Mat_Pro_Weight = "",
                    Mat_Rough_Weight = "",
                    Mat_Technics = "",
                    Mat_Unit = "",
                    Mat_Weight = "",
                    Material_Code =  (i + 1).ToString(),
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
                    NumCasesSum = Convert.ToDecimal(dt.Rows[i]["ProductionNum"].ToString()),
                    Object_Id = null,
                    Other_Quantity = "",
                    PackId = PackId,
                    ParentId = -1,
                    PredictDeliveryDate = null,
                    Quantity = dt.Rows[i]["ProductionNum"].ToString(),
                    Required_Quantity = "",
                    Rough_Size = "",
                    Rough_Spec = "",
                    Stage = Convert.ToInt32(dt.Rows[i]["Stage"].ToString()),
                    StandAlone = Convert.ToInt32(dt.Rows[i]["MatingNum"].ToString()),   //单机配套数量
                    TaskCode = dt.Rows[i]["TaskCode"].ToString(),
                    TaskId = Convert.ToInt32(dt.Rows[i]["TaskID"].ToString()),
                    TDM_Description = "",
                    Tech_Quantity = "",
                    Technics_Comment = "",
                    Technics_Line = "",
                    Test_Quantity = "",
                    ThisTimeOperation = Convert.ToInt32(dt.Rows[i]["ProductionNum"].ToString()), //投产数量
                    User_ID = UserID,
                    VerCode = 1

                };

                if (dt.Rows[i]["PlanFinishTime"].ToString() != "")
                {                  
                    MDDLD.PredictDeliveryDate = Convert.ToDateTime(dt.Rows[i]["PlanFinishTime"].ToString());//计划交付时间
                }

                //SmarTeam接口数据
                DataTable dtGetByDrawingNoAndPhase = new DataTable();
                try
                {
                    dtGetByDrawingNoAndPhase =
                        ST.GetByDrawingNoAndPhase(MDDLD.Drawing_No.ToString(), MDDLD.Stage.ToString(), dt.Rows[i]["AreaCode"].ToString())
                            .Tables[0];
                }
                catch
                {
                    sst.ErrMsg = "不嫩与SmarTeam通信，<br />请联系管理员";
                    return sst;
                }

                if (dtGetByDrawingNoAndPhase.Rows.Count > 0)
                {
                    MDDLD.Class_Id = Convert.ToInt32(dtGetByDrawingNoAndPhase.Rows[0]["Class_Id"].ToString().Trim());
                    MDDLD.Object_Id = Convert.ToInt32(dtGetByDrawingNoAndPhase.Rows[0]["Object_Id"].ToString().Trim());
                    MDDLD.Stage = Convert.ToInt32(dtGetByDrawingNoAndPhase.Rows[0]["Phase"].ToString().Trim());
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
                    MDDLD.LingJian_Type = dtGetByDrawingNoAndPhase.Rows[0]["CN_LingJian_Type"].ToString().Trim();
                    MDDLD.Mat_Rough_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Rough_Weight"].ToString().Trim();
                    MDDLD.Mat_Pro_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Pro_Weight"].ToString().Trim();

                    MDDLD.Mat_Weight = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Weight"].ToString().Trim();
                    MDDLD.Mat_Efficiency = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Efficiency"].ToString().Trim();
                    MDDLD.Mat_Comment = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Comment"].ToString().Trim();
                    MDDLD.Mat_Technics = dtGetByDrawingNoAndPhase.Rows[0]["CN_Mat_Technics"].ToString().Trim();
                    MDDLD.Rough_Spec = dtGetByDrawingNoAndPhase.Rows[0]["CN_Rough_Spec"].ToString().Trim();
                    MDDLD.Rough_Size = dtGetByDrawingNoAndPhase.Rows[0]["CN_Rough_Size"].ToString().Trim();
                    MDDLD.CN_Material_State = dtGetByDrawingNoAndPhase.Rows[0]["CN_Material_State"].ToString();
                    
                    ListMDDLD.Add(MDDLD);

                   string errmsg =  GetChildren(MDDLD.Object_Id.ToString(), MDDLD.Class_Id.ToString(), MDDLD.PackId, MDDLD.TaskId, MDDLD.Id,
                        MDDLD.Material_Code, MDDLD.NumCasesSum.ToString(), IsSpread, MDDLD.TaskCode, UserID);
                   if (errmsg != "")
                   {
                       sst.ErrMsg = errmsg;
                       return sst;
                   }
                }
            }
            return sst;
        }

        private string GetChildren(string ObjectID, string ClassID, int? PackId, int? TaskId, int ParentID, string Material_Code, string NumCasesSum,
            string IsSpread, string TaskCode,  int UserID)
        {
            string errmsg = "";
            //SmarTeam接口数据
            DataTable dtGetChildren = new DataTable();
            try
            {
                dtGetChildren = ST.GetChildren(ClassID, ObjectID).Tables[0];
            }
            catch
            { 
                errmsg = "不嫩与SmarTeam通信，<br />请联系管理员";
                return errmsg;
            }

            for (int i = 0; i < dtGetChildren.Rows.Count; i++)
            {
                M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft()
                {
                    Ballon_No = "",
                    Class_Id = Convert.ToInt32(dtGetChildren.Rows[i]["Class_Id"].ToString().Trim()),
                    CN_Material_State = dtGetChildren.Rows[i]["CN_Material_State"].ToString(),
                    Comment = "",
                    DemandDate = "",
                    DemandNumSum = null,
                    DraftId = null,
                    Drawing_No = dtGetChildren.Rows[i]["CN_Drawing_No"].ToString().Trim(),
                    Id = ListMDDLD.Count,
                    Import_Date = DateTime.Now,
                    Is_allow_merge = null,
                    Is_del = false,
                    ItemCode1 = dtGetChildren.Rows[i]["CN_ItemCode1"].ToString().Trim(),
                    ItemCode2 = dtGetChildren.Rows[i]["CN_ItemCode2"].ToString().Trim(),
                    JSGS_Des = "",
                    LingJian_Type = dtGetChildren.Rows[i]["CN_LingJian_Type"].ToString().Trim(),
                    Mat_Comment = dtGetChildren.Rows[i]["CN_Mat_Comment"].ToString().Trim(),
                    Mat_Efficiency = dtGetChildren.Rows[i]["CN_Mat_Efficiency"].ToString().Trim(),
                    Mat_Pro_Weight = dtGetChildren.Rows[i]["CN_Mat_Pro_Weight"].ToString().Trim(),
                    Mat_Rough_Weight = dtGetChildren.Rows[i]["CN_Mat_Rough_Weight"].ToString().Trim(),
                    Mat_Technics = dtGetChildren.Rows[i]["CN_Mat_Technics"].ToString().Trim(),
                    Mat_Unit = dtGetChildren.Rows[i]["CN_Mat_Unit"].ToString().Trim(),
                    Mat_Weight = dtGetChildren.Rows[i]["CN_Mat_Weight"].ToString().Trim(),
                    Material_Code = Material_Code + "-" + (i + 1).ToString(),
                    Material_Mark = dtGetChildren.Rows[i]["CN_Material_Mark"].ToString().Trim(),
                    Material_Name = dtGetChildren.Rows[i]["CN_Material_Name"].ToString().Trim(),
                    Material_Spec =  dtGetChildren.Rows[i]["CN_Material_Spec"].ToString().Trim(),
                    Material_State = 0,
                    Material_Tech_Condition = dtGetChildren.Rows[i]["CN_Material_Tech_Condition"].ToString().Trim(),
                    MaterialDept = "",
                    MaterialsDes = "",
                    MaterialsNum = null,
                    MDML_Id = "",
                    MDPId = null,
                    Memo_Quantity = "",
                    MissingDescription = "",
                    NumCasesSum = null,
                    Object_Id = Convert.ToInt32(dtGetChildren.Rows[i]["Object_Id"].ToString().Trim()),
                    Other_Quantity = "",
                    PackId = PackId,
                    ParentId = ParentID,
                    PredictDeliveryDate = null,
                    Quantity = "",
                    Required_Quantity = "",
                    Rough_Size = dtGetChildren.Rows[i]["CN_Rough_Size"].ToString().Trim(),
                    Rough_Spec = dtGetChildren.Rows[i]["CN_Rough_Spec"].ToString().Trim(),
                    Stage = Convert.ToInt32(dtGetChildren.Rows[i]["Phase"].ToString().Trim()),
                    StandAlone =null,   //单机配套数量
                    TaskCode = TaskCode,
                    TaskId = TaskId,
                    TDM_Description = dtGetChildren.Rows[i]["TDM_Description"].ToString().Trim(),
                    Tech_Quantity = "",
                    Technics_Comment =  dtGetChildren.Rows[i]["CN_Technics_Comment"].ToString().Trim(),
                    Technics_Line = dtGetChildren.Rows[i]["CN_Technics_Line"].ToString().Trim(),
                    Test_Quantity = "",
                    ThisTimeOperation = null, //投产数量
                    User_ID = UserID,
                    VerCode = 1

                };

                //SmarTeam数据
                DataTable dtItemsCount = new DataTable();
                try
                {
                    dtItemsCount = ST.GetItemsCount(dtGetChildren.Rows[i]["CLASS_ID1"].ToString().Trim(), dtGetChildren.Rows[i]["OBJECT_ID1"].ToString().Trim(), dtGetChildren.Rows[i]["CLASS_ID"].ToString().Trim(), dtGetChildren.Rows[i]["OBJECT_ID"].ToString().Trim()).Tables[0];
                }
                catch
                {
                    errmsg = "不嫩与SmarTeam通信，<br />请联系管理员";
                    return errmsg;
                }

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
                
                if (MDDLD.Quantity != "" && NumCasesSum != "")
                {
                    try
                    {
                        Convert.ToDouble(MDDLD.Quantity);
                        MDDLD.NumCasesSum = Convert.ToDecimal((Convert.ToDouble(MDDLD.Quantity) * Convert.ToDouble(NumCasesSum)));  //共计需求件数
                    }
                    catch { }
                }
                ListMDDLD.Add(MDDLD);

                if (IsSpread == "true")
                {
                    errmsg = GetChildren(MDDLD.Object_Id.ToString(), MDDLD.Class_Id.ToString(), MDDLD.PackId, MDDLD.TaskId, MDDLD.Id,
                        MDDLD.Material_Code, MDDLD.NumCasesSum.ToString(), IsSpread, MDDLD.TaskCode, UserID);
                    if (errmsg != "")
                    {
                        return errmsg;
                    }                   
                }
            }
            return errmsg;
        }

        public class SynchronizationSmarTeam
        {
            public List<M_Demand_DetailedList_Draft> Mddld { get; set; }
            public string ErrMsg { get; set; }
        }

        #endregion
    }
}