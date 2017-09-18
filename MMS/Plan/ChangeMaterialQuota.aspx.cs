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
    public partial class ChangeMaterialQuota : System.Web.UI.Page
    {
        string DBContractConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
            DBI = DBFactory.GetDBInterface(DBContractConn);
            if (!IsPostBack)
            {
                if (Common.IsHasRight(Session["UserName"].ToString(), "RB_SynchronChange"))
                {
                    RB_SynchronChange.Visible = true;
                }

                RadTabStrip1.Tabs.FindTabByValue("0").NavigateUrl = "/Plan/ShowSmarTeam.aspx?PackId=" + Request.QueryString["PackId"].ToString();
                RadTabStrip1.Tabs.FindTabByValue("1").NavigateUrl = "/Plan/ShowSmarTeam.aspx?PackId=" + Request.QueryString["PackId"].ToString() + "&Tabs=1";
                RadTabStrip1.Tabs.FindTabByValue("2").NavigateUrl = "/Plan/ShowSmarTeam.aspx?PackId=" + Request.QueryString["PackId"].ToString() + "&Tabs=2";

                string packId = Request.QueryString["PackId"].ToString();

                string strSQL = " select * from Sys_M_Change_Reason where Enable = 'true'";
                DataTable dtchangereason = DBI.Execute(strSQL, true);
                RBL1.DataSource = dtchangereason;
                RBL1.DataValueField = "ID";
                RBL1.DataTextField = "Reason";
                RBL1.DataBind();

                strSQL =
                    " select Sys_Model.Model ,PlanName from P_Pack left join Sys_Model on Sys_Model.ID = P_Pack.Model where PackId = '" +
                    Request.QueryString["PackId"].ToString() + "'";
                DataTable dtpack = DBI.Execute(strSQL, true);
                lblModel.Text = dtpack.Rows[0]["Model"].ToString();
                lblPlanName.Text = dtpack.Rows[0]["PlanName"].ToString();

                this.ViewState["GridSourceChange"] = new DataTable();
            }
        }

        #region 更改单更改材料定额

        protected void RTB_Search_Click(object sender, EventArgs e)
        {
            string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            string strSQL = "";
            string CN_CHANGEREPORT_NO = RTB_CN_CHANGEREPORT_NO.Text.Trim();
            if (CN_CHANGEREPORT_NO == "")
            {
                RadNotificationAlert.Text = "没有更改单号！";
                RadNotificationAlert.Show();
                return;
            }
            strSQL = " select count(*) from M_Change_List where ChangeList_Code = '" + CN_CHANGEREPORT_NO + "' and PackId = '" + PackID + "'";
            string result = DBI.GetSingleValue(strSQL);
            if (result == "0")
            {
                lblSecrchCNResult.Text = "";
                Label1.Text = "确定要更改吗？";
            }
            else
            {
                lblSecrchCNResult.Text = CN_CHANGEREPORT_NO + "：已经变更" + result + "次"; ;
                Label1.Text = "该更改单已经更改，<br />是否重新更改？";
            }

            SmarTeam.Items ST = new SmarTeam.Items();
            STLookupTables.LookUpTables STLUT = new STLookupTables.LookUpTables();

            DBContractConn = ConfigurationManager.AppSettings["DBST211"].ToString();
            DBI = DBFactory.GetDBInterface(DBContractConn);

            //通过更改单获取更改单object_ID
            strSQL = " select top 1 a.OBJECT_ID"
                + " from tn_documentation a,TN_CHANGE_REPORT b,TN_ITEMS c,TN_LINK_00378 d"
                + " where a.OBJECT_ID=d.OBJECT_ID2 and d.OBJECT_ID1=c.OBJECT_ID and a.OBJECT_ID=b.OBJECT_ID"
                + " and a.CLASS_ID=801 and a.STATE=3 and a.CN_CHANGEREPORT_NO like '%GZGXX%'"
                + " and a.CN_CHANGEREPORT_NO = '" + CN_CHANGEREPORT_NO + "'"
                + " order by a.MODIFICATION_DATE desc";

            DataTable dt = new DataTable();
            try
            {
                dt = DBI.Execute(strSQL, true);
            }
            catch
            {
                RadNotificationAlert.Text = "不能与SmarTeam通信，<br />请联系管理员。";
                RadNotificationAlert.Show();
                return;
            }
            lbl_CN_CHANGEREPORT_NO.Text = CN_CHANGEREPORT_NO;
            if (dt.Rows.Count > 0)
            {
                string ObjectID = dt.Rows[0]["OBJECT_ID"].ToString();
                //通过更改单Object_ID获取更改单信息
                strSQL = " select top 1 c.CN_DRAWING_NO as C_CN_DRAWING_NO,c.TDM_DESCRIPTION as C_TDM_DESCRIPTION,a.*,b.*,C.CN_FIRSTUSE_PRODUCT, CN_TECHNICS_LINE"
                + " from tn_documentation a,TN_CHANGE_REPORT b,TN_ITEMS c,TN_LINK_00378 d"
                + " where a.OBJECT_ID=d.OBJECT_ID2 and d.OBJECT_ID1=c.OBJECT_ID"
                + " and a.Object_Id=" + ObjectID
                + " and a.OBJECT_ID=b.OBJECT_ID and a.CLASS_ID=801 and a.STATE=3 and a.CN_CHANGEREPORT_NO like '%GZGXX%' order by a.MODIFICATION_DATE desc";

                DataTable dtggd = DBI.Execute(strSQL, true);
                if (dtggd.Rows.Count > 0)
                {
                    lbl_CN_CHANGEREPORT_NO.Text = dtggd.Rows[0]["CN_CHANGEREPORT_NO"].ToString();
                    lbl_C_CN_DRAWING_NO.Text = dtggd.Rows[0]["C_CN_DRAWING_NO"].ToString();
                    lbl_C_TDM_DESCRIPTION.Text = dtggd.Rows[0]["C_TDM_DESCRIPTION"].ToString();
                    lbl_CN_CHANGE_REASON.Text = dtggd.Rows[0]["CN_CHANGE_REASON"].ToString();
                    lbl_Change_UserName.Text = STLUT.GetUserName(dtggd.Rows[0]["USER_OBJECT_ID"].ToString());
                    lbl_TECHNICS_LINE.Text = dtggd.Rows[0]["CN_TECHNICS_LINE"].ToString();
                    RTB_Remark.Text = dtggd.Rows[0]["CN_CHANGE_REASON"].ToString();
                }

                //通过更改单Object_ID获取更改单详细信息
                strSQL = "select b.CN_DRAWING_NO as CN_COM_DRAWING_NO,B.CN_TECHNICS_LINE,a.*"
                + " ,case when CN_EDIT_TYPE = '0' or CN_EDIT_TYPE = '3' then '修改' when CN_EDIT_TYPE = '1' or CN_EDIT_TYPE = '2' then '新增' when CN_EDIT_TYPE = '4' or CN_EDIT_TYPE= '5' then '删除' else CN_EDIT_TYPE end as CN_EDIT_TYPE1"
                + " from TN_EDIT_AUDITS a,TN_ITEMS b where a.CN_COM_OBJECT_ID=b.OBJECT_ID and a.CN_CHANGE_NO=" + ObjectID
                + " order by a.CREATION_DATE";
                this.ViewState["GridSourceChange"] = DBI.Execute(strSQL, true);

                (this.ViewState["GridSourceChange"] as DataTable).Columns.Add("RowsId");
                (this.ViewState["GridSourceChange"] as DataTable).Columns.Add("ChangeContent");
                (this.ViewState["GridSourceChange"] as DataTable).Columns.Add("SetData");
                (this.ViewState["GridSourceChange"] as DataTable).Columns.Add("WhereData");
                (this.ViewState["GridSourceChange"] as DataTable).Columns.Add("Name");
                (this.ViewState["GridSourceChange"] as DataTable).Columns.Add("ChangePerson");
                (this.ViewState["GridSourceChange"] as DataTable).Columns.Add("PackState");

                XmlDocument PropertyNameDoc = new XmlDocument();
                XmlDocument PropertyNameDBDoc = new XmlDocument();
                try
                {
                    PropertyNameDoc.Load(Server.MapPath(@"~\Plan\PropertyName.xml"));
                    PropertyNameDBDoc.Load(Server.MapPath(@"~\Plan\PropertyNameDB.xml"));

                }
                catch { }

                for (int i = 0; i < (this.ViewState["GridSourceChange"] as DataTable).Rows.Count; i++)
                {
                    (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["RowsId"] = (i + 1).ToString();
                    (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["ChangeContent"] = "";
                    (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["SetData"] = "";
                    (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["WhereData"] = "";
                    (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["PackState"] = "";
                    (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_TECHNICS_LINE"] = ST.GetById((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_CLASS_ID"].ToString(), (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_OBJECT_ID"].ToString()).Tables[0].Rows[0]["CN_TECHNICS_LINE"].ToString();
                    try
                    {
                        (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["Name"] = ST.GetById((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_CLASS_ID"].ToString(), (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_OBJECT_ID"].ToString()).Tables[0].Rows[0]["TDM_DESCRIPTION"].ToString();
                    }
                    catch { }
                    try
                    {
                        (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["ChangePerson"] = STLUT.GetUserName((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["USER_OBJECT_ID"].ToString());
                    }
                    catch { }

                    //如果不是修改不需要执行下列程序
                    if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() != "0" && (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() != "3")
                    { continue; }

                    XmlDocument beforeDoc = new XmlDocument();
                    XmlDocument afterDoc = new XmlDocument();
                    try
                    {
                        beforeDoc.LoadXml((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_COMMENT"].ToString());
                        afterDoc.LoadXml((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_CUR_CONTENT"].ToString());
                    }
                    catch { }

                    string tableStr = "<Table  border='1' style='border-right: thin solid; border-top: thin solid; border-left: thin solid;width: 100%; border-bottom: thin solid'>";
                    tableStr = tableStr + "<tr>";
                    tableStr = tableStr + "<td>更改内容</td>";
                    tableStr = tableStr + "<td>更改前</td>";
                    tableStr = tableStr + "<td>更改后</td>";
                    tableStr = tableStr + "</tr>";

                    string nodeName = "";
                    string nodeDes = "";
                    string valueBefore = "";
                    string valueAfter = "";

                    try
                    {
                        foreach (XmlNode tempNode in afterDoc.ChildNodes[1].ChildNodes)
                        {
                            nodeName = tempNode.Name.ToString();
                            try
                            {
                                if (beforeDoc.GetElementsByTagName(nodeName)[0].InnerText != null)
                                {
                                    valueBefore = beforeDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString();
                                }
                            }
                            catch (System.Exception)
                            {
                                valueBefore = "";
                            }
                            try
                            {
                                if (afterDoc.GetElementsByTagName(nodeName)[0].InnerText != null)
                                {
                                    valueAfter = afterDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString();
                                }
                            }
                            catch (System.Exception)
                            {
                                valueAfter = "";
                            }
                            try
                            {
                                if (PropertyNameDoc.GetElementsByTagName(nodeName)[0].InnerText != null)
                                {
                                    nodeDes = PropertyNameDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString();
                                }
                                else { nodeDes = nodeName; }
                            }
                            catch { nodeDes = nodeName; }

                            if (valueBefore != valueAfter)
                            {
                                tableStr = tableStr + "<tr>";
                                tableStr = tableStr + "<td>" + nodeDes + "</td>";
                                tableStr = tableStr + "<td>" + valueBefore + "</td>";
                                tableStr = tableStr + "<td>" + valueAfter + "</td>";
                                tableStr = tableStr + "</tr>";

                                try
                                {
                                    if (PropertyNameDBDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString() != null)
                                    {
                                        (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["SetData"] += " , " + PropertyNameDBDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString() + " = '" + valueAfter + "'";

                                        if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["WhereData"].ToString() == "")
                                        {
                                            (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["WhereData"] += " and (" + PropertyNameDBDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString() + " <> '" + valueAfter + "'";
                                        }
                                        else
                                        {
                                            (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["WhereData"] += " or " + PropertyNameDBDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString() + " <> '" + valueAfter + "'";
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    catch (System.Exception) { }

                    tableStr = tableStr + "</Table>";
                    (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["ChangeContent"] = tableStr;
                    if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["WhereData"].ToString() != "") { (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["WhereData"] += " )"; }
                }

                //判断本计划包中的状态
                DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
                DBI = DBFactory.GetDBInterface(DBContractConn);
                //CN_Com_Class_Id 所属Class_Id, CN_Com_OBJECT_ID 所属OBJECT_ID
                for (int i = 0; i < (this.ViewState["GridSourceChange"] as DataTable).Rows.Count; i++)
                {
                    string class_Id = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Class_Id"].ToString();
                    string Object_Id = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Object_Id"].ToString();
                    if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "0" || (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "3" || (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "4" || (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "5")
                    {
                        strSQL = " if (select count(*) from M_Demand_DetailedList_Draft"
                            + " where PackId = '" + PackID + "' and Object_ID = '" + Object_Id + "' and Class_ID = '" + class_Id + "' and Is_Del = 'false') = 0 begin select '不存在', '' end"
                            + " else begin declare @sql nvarchar(max) , @id nvarchar(max) select @sql = '', @id = '' select @sql = @sql +  Material_Code + '：' + TDM_Description + ':' + case when Material_State in (1,2,6,7) then '已提交' else '未提交' end + ','"
                            + " , @id = @id + Convert(nvarchar(50),ID) + ',' from M_Demand_DetailedList_Draft where PackId = '" + PackID + "' and Object_ID = '" + Object_Id + "' and Class_ID = '" + class_Id + "' and Is_Del = 'false' order by Id desc select @sql, @id end"
                            + "";
                        string Material_Code_Name = DBI.GetSingleValue(strSQL);
                        if (Material_Code_Name != "不存在")
                        {
                            Material_Code_Name = Material_Code_Name.Substring(0, Material_Code_Name.Length - 2);
                            if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "4" || (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "5")
                            {
                                Material_Code_Name += "及其子节点";
                            }
                        }
                        (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["PackState"] = Material_Code_Name;
                    }
                    else if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "1" || (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "2")
                    {
                        DataTable dtGetParents = ST.GetParents(class_Id, Object_Id).Tables[0];
                        string Material_Code_Name = "";
                        for (int j = 0; j < dtGetParents.Rows.Count; j++)
                        {
                            string PClass_ID = dtGetParents.Rows[j]["Class_Id"].ToString();
                            string PObject_ID = dtGetParents.Rows[j]["Object_ID"].ToString();
                            strSQL = " if (select count(*) from M_Demand_DetailedList_Draft"
                            + " where PackId = '" + PackID + "' and Object_ID = '" + PObject_ID + "' and Class_ID = '" + PClass_ID + "' and Is_Del = 'false') = 0 begin select '不存在' end"
                            + " else begin declare @sql nvarchar(max)  select @sql = '' select @sql = @sql +  Material_Code + '：' + TDM_Description + ','"
                            + " from M_Demand_DetailedList_Draft where PackId = '" + PackID + "' and Object_ID = '" + PObject_ID + "' and Class_ID = '" + PClass_ID + "' and Is_Del = 'false' order by Id desc select @sql end"
                            + "";
                            DataTable dtMDDLD = DBI.Execute(strSQL, true);
                            if (dtMDDLD.Rows[0][0].ToString() != "不存在")
                            {
                                Material_Code_Name += dtMDDLD.Rows[0][0].ToString();
                            }
                        }
                        if (Material_Code_Name == "")
                        {
                            (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["PackState"] = "没有找到父节点无法增加";
                        }
                        else
                        {
                            (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["PackState"] = Material_Code_Name + "节点下增加";
                        }
                    }
                }
                RadGridChange.Rebind();
            }
            else
            {
                lblSecrchCNResult.Text = "更改单: " + CN_CHANGEREPORT_NO + "，不存在！"; ;
            }
        }

        protected void RadGridChange_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridChange.DataSource = (this.ViewState["GridSourceChange"] as DataTable);
        }

        protected void RB_SynchronChange_Click(object sender, EventArgs e)
        {
            if (RTB_Remark.Text.Trim() == "")
            {
                RadNotificationAlert.Text = "请输入更改说明！";
                RadNotificationAlert.Show();
                return;
            }
            if (RBL1.SelectedIndex == -1)
            {
                RadNotificationAlert.Text = "请选择更改原因！";
                RadNotificationAlert.Show();
                return;
            }

            string PackId = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            string User_ID = (Session["UserId"] == null ? "" : Session["UserId"].ToString());

            string strSQL = "";

            try
            {
                DBI.OpenConnection();
                DBI.BeginTrans();

                strSQL = " Insert into M_Change_List (PackID, VerCode, ChangeList_Code, Drawing_No, Remark,Reason, Change_UserID, Change_Date, Relevance_Dept, TDM_Description, Change_Reason, Change_UserName)";
                strSQL += " values ('" + PackId + "', (select isnull(max(VerCode),1) + 1 from M_Change_List where PackId = '" + PackId + "'),'" + lbl_CN_CHANGEREPORT_NO.Text.Trim() + "'";
                strSQL += " ,'" + lbl_C_CN_DRAWING_NO.Text.Trim() + "','" + RTB_Remark.Text.Trim() + "','" + RBL1.SelectedItem.Text.ToString() + "','1',GetDate(),'" + lbl_TECHNICS_LINE.Text.Trim() + "','" + lbl_C_TDM_DESCRIPTION.Text.Trim() + "'"
                    + " ,'" + lbl_CN_CHANGE_REASON.Text.Trim() + "','" + lbl_Change_UserName.Text.Trim() + "') declare @mclid nvarchar(50) select @mclid = @@identity";
                strSQL += " select ID, VerCode from M_Change_List where ID = @mclid ";
                DataTable dtmcl = DBI.Execute(strSQL, true);

                string verCode = dtmcl.Rows[0]["VerCode"].ToString();
                string MCLId = dtmcl.Rows[0]["ID"].ToString();

                for (int i = 0; i < (this.ViewState["GridSourceChange"] as DataTable).Rows.Count; i++)
                {
                    string CN_Drawing_No = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_DRAWING_NO"].ToString();
                    string Name = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["Name"].ToString();
                    string CN_Com_Drawing_No = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Com_Drawing_No"].ToString();
                    string CN_Edit_Comment = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Edit_Comment"].ToString();
                    string CN_Cur_Content = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Cur_Content"].ToString();
                    string CN_Edit_Type = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Edit_Type"].ToString();
                    string CN_TECHNICS_LINE = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Technics_Line"].ToString();
                    string ChangePerson = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["ChangePerson"].ToString();
                    string class_Id = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Class_Id"].ToString();
                    string Object_Id = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_Object_Id"].ToString();
                    string Influence = (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["PackState"].ToString();
                    //插入M_Change_List_Detailed
                    strSQL = " Insert into M_Change_List_Detailed (MCLID, CN_Drawing_No, Name, CN_Com_Drawing_No, CN_Edit_Comment"
                        + " , CN_Cur_Content, CN_Edit_Type, CN_TECHNICS_LINE, ChangePerson, Class_ID, Object_ID, Influence)"
                        + " values ('" + MCLId + "', '" + CN_Drawing_No + "','" + Name + "','" + CN_Com_Drawing_No + "','" + CN_Edit_Comment + "','" + CN_Cur_Content + "'"
                        + " ,'" + CN_Edit_Type + "','" + CN_TECHNICS_LINE + "','" + ChangePerson + "','" + class_Id + "','" + Object_Id + "'"
                        + " , case when isnumeric('" + Influence.Substring(0, 1) + "') = 0 then '无' else '" + Influence + "' end)";
                    DBI.Execute(strSQL);

                    //修改
                    if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "0" || (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "3")
                    {
                        if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["SetData"].ToString() != "")
                        {
                            strSQL = " insert into M_Change_Flow (MDDLD_Id, Change_State, VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, "
                                + " Material_Code, ParentId, Material_Spec, TDM_Description, Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, "
                                + " Technics_Comment, Material_Mark, ItemCode1, ItemCode2, MaterialsNum, Mat_Unit, LingJian_Type, Mat_Rough_Weight, "
                                + " Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec, Rough_Size, MaterialsDes, "
                                + " StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity, "
                                + " Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, "
                                + " User_ID, JSGS_Des, Is_del, TaskCode, MaterialDept, MissingDescription, M_Change_List_ID, CN_Material_State)"
                                + " select Id, Null, VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, "
                                + " Material_Code, ParentId, Material_Spec, TDM_Description, Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, "
                                + " Technics_Comment, Material_Mark, ItemCode1, ItemCode2, MaterialsNum, Mat_Unit, LingJian_Type, Mat_Rough_Weight, "
                                + " Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec, Rough_Size, MaterialsDes, "
                                + " StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity, "
                                + " Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, "
                                + " User_ID, JSGS_Des, Is_del, TaskCode, MaterialDept, MissingDescription, '" + MCLId + "', CN_Material_State"
                                + " from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Object_Id = '" + Object_Id + "' and Class_Id = '" + class_Id + "' and Is_del = 'false' " + (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["WhereData"].ToString()
                                + " Update M_Demand_DetailedList_Draft set VerCode = '" + verCode + "', User_ID = '" + User_ID + "' " + (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["SetData"].ToString()
                                //修改共计需求件数
                                + " , NumCasesSum = case when ParentId = 0 then NumCasesSum else case when isnumeric(Quantity) = 1 and isnumeric((select NumCasesSum from M_Demand_DetailedList_Draft as a where a.ID = M_Demand_DetailedList_Draft.ParentId)) = 1"
                                + " then Convert(decimal(18,4), Quantity) * Convert(decimal(18,4),(select NumCasesSum from M_Demand_DetailedList_Draft as a where a.ID = M_Demand_DetailedList_Draft.ParentId)) else Null end end"
                                + " where PackId = '" + PackId + "' and Object_Id = '" + Object_Id + "' and Class_Id = '" + class_Id + "' and Is_del = 'false' " + (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["WhereData"].ToString();

                            //如果共计需求件数有变化，子节点的共计需求件数跟着变 
                            strSQL += " if exists(select * from tempdb..sysobjects where id =OBJECT_ID('tempdb..#updateID'))"
                                + " begin drop table #updateID end"
                                + " create table #updateID (ID int)  insert into #updateID select ID from M_Demand_DetailedList_Draft"
                                + " where PackId = '" + PackId + "' and Object_Id = '" + Object_Id + "' and Class_Id = '" + class_Id + "' and Is_del = 'false' and VerCode = '" + verCode + "'"
                                + " and NumCasesSum <> (select top 1 NumCasesSum from M_Change_Flow where MDDLD_Id = M_Demand_DetailedList_Draft.ID order by ID desc)"
                                + " while (select count(*) from M_Demand_DetailedList_Draft where ParentID in (select ID from #updateID) and ID not in (select ID from #updateID) and Is_Del = 'false') > 0 "
                                + " begin  insert into #updateID select ID from M_Demand_DetailedList_Draft where ParentID in (select ID from #updateID) and ID not in (select ID from #updateID) and Is_Del = 'false' end"
                                + " delete #updateID where ID in (select ID from M_Demand_DetailedList_Draft"
                                + " where PackId = '" + PackId + "' and Object_Id = '" + Object_Id + "' and Class_Id = '" + class_Id + "' and Is_del = 'false' and VerCode = '" + verCode + "')"

                                //需求件数有变化的子节点
                                + " insert into M_Change_Flow (MDDLD_Id, Change_State, VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, "
                                + " Material_Code, ParentId, Material_Spec, TDM_Description, Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, "
                                + " Technics_Comment, Material_Mark, ItemCode1, ItemCode2, MaterialsNum, Mat_Unit, LingJian_Type, Mat_Rough_Weight, "
                                + " Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec, Rough_Size, MaterialsDes, "
                                + " StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity, "
                                + " Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, "
                                + " User_ID, JSGS_Des, Is_del, TaskCode, MaterialDept, MissingDescription, M_Change_List_ID, CN_Material_State)"
                                + " select Id, Null, VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, "
                                + " Material_Code, ParentId, Material_Spec, TDM_Description, Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, "
                                + " Technics_Comment, Material_Mark, ItemCode1, ItemCode2, MaterialsNum, Mat_Unit, LingJian_Type, Mat_Rough_Weight, "
                                + " Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec, Rough_Size, MaterialsDes, "
                                + " StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity, "
                                + " Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, "
                                + " User_ID, JSGS_Des, Is_del, TaskCode, MaterialDept, MissingDescription, '" + MCLId + "', CN_Material_State"
                                + " from M_Demand_DetailedList_Draft where ID in (select * from #updateID)"
                                + " Update M_Demand_DetailedList_Draft set VerCode = '" + verCode + "', User_ID = '" + User_ID + "'"
                                //修改子节点共计需求件数
                                + " , NumCasesSum = case when ParentId = 0 then NumCasesSum else case when isnumeric(Quantity) = 1 and isnumeric((select NumCasesSum from M_Demand_DetailedList_Draft as a where a.ID = M_Demand_DetailedList_Draft.ParentId)) = 1"
                                + " then Convert(decimal(18,4), Quantity) * Convert(decimal(18,4),(select NumCasesSum from M_Demand_DetailedList_Draft as a where a.ID = M_Demand_DetailedList_Draft.ParentId)) else Null end end"
                                + " where ID in (select * from #updateID)";
                            DBI.Execute(strSQL);

                        }
                    }
                    //删除
                    else if ((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "5" || (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "4")
                    {
                        //删除的节点及其子节点
                        strSQL = " if exists(select * from tempdb..sysobjects where id =OBJECT_ID('tempdb..#deleteID'))"
                            + " begin drop table #deleteID end"
                            + " create table #deleteID (ID int) delete #deleteID insert into #deleteID select ID from  M_Demand_DetailedList_Draft"
                            + " where PackId = '" + PackId + "' and Object_Id = '" + Object_Id + "' and Class_Id = '" + class_Id + "' and Is_del = 'false'"
                            + " while (select count(*) from M_Demand_DetailedList_Draft where ParentID in (select ID from #deleteID) and ID not in (select ID from #deleteID) and Is_Del = 'false') > 0 "
                            + " begin insert into #deleteID select ID from M_Demand_DetailedList_Draft where ParentID in (select ID from #deleteID) and ID not in (select ID from #deleteID) and Is_Del = 'false' end";

                        strSQL += " insert into M_Change_Flow (MDDLD_Id, Change_State, VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, "
                                + " Material_Code, ParentId, Material_Spec, TDM_Description, Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, "
                                + " Technics_Comment, Material_Mark, ItemCode1, ItemCode2, MaterialsNum, Mat_Unit, LingJian_Type, Mat_Rough_Weight, "
                                + " Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec, Rough_Size, MaterialsDes, "
                                + " StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity, "
                                + " Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, "
                                + " User_ID, JSGS_Des, Is_del, TaskCode, MaterialDept, MissingDescription, M_Change_List_ID, CN_Material_State)"
                                + " select Id, Null, VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, "
                                + " Material_Code, ParentId, Material_Spec, TDM_Description, Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, "
                                + " Technics_Comment, Material_Mark, ItemCode1, ItemCode2, MaterialsNum, Mat_Unit, LingJian_Type, Mat_Rough_Weight, "
                                + " Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec, Rough_Size, MaterialsDes, "
                                + " StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity, "
                                + " Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, "
                                + " User_ID, JSGS_Des, Is_del, TaskCode, MaterialDept, MissingDescription, '" + MCLId + "', CN_Material_State"
                                + " from M_Demand_DetailedList_Draft where ID in (select ID from #deleteID)"
                                + " Update M_Demand_DetailedList_Draft set VerCode = '" + verCode + "', User_ID = '" + User_ID + "', Is_del = 'true' where ID in (select ID from #deleteID)";
                        DBI.Execute(strSQL);
                    }
                    //新增
                    else if (((this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "1" || (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["CN_EDIT_TYPE"].ToString() == "2") && (this.ViewState["GridSourceChange"] as DataTable).Rows[i]["PackState"].ToString() != "没有找到父节点无法增加")
                    {
                        //SmarTeam接口
                        SmarTeam.Items ST = new SmarTeam.Items();
                        DataTable dtGetParents = ST.GetParents(class_Id, Object_Id).Tables[0];

                        for (int j = 0; j < dtGetParents.Rows.Count; j++)
                        {
                            string PClass_ID = dtGetParents.Rows[j]["Class_Id"].ToString();
                            string PObject_ID = dtGetParents.Rows[j]["Object_ID"].ToString();

                            strSQL = " select count(*) from M_Demand_DetailedList_Draft where PackId = '" + PackId + "' and Object_Id = '" + PObject_ID + "' and Class_ID='" + PClass_ID + "' and Is_del = 'false'";
                            if (DBI.GetSingleValue(strSQL).ToString() != "0")
                            {
                                DataTable dtGetChildren = ST.GetChildren(dtGetParents.Rows[j]["Class_ID"].ToString(), dtGetParents.Rows[j]["Object_ID"].ToString()).Tables[0];
                                DataRow datarow = dtGetChildren.Select("OBJECT_ID='" + Object_Id + "' and CLASS_ID='" + class_Id + "' ")[0];
                                DataTable dtItemsCount = ST.GetItemsCount(PClass_ID, PObject_ID, class_Id, Object_Id).Tables[0];
                                string Quantity = "", Tech_Quantity = "", Memo_Quantity = "", Test_Quantity = "", Required_Quantity = "", Other_Quantity = "", Ballon_No = "", Comment = "";

                                if (dtItemsCount.Rows.Count > 0)
                                {
                                    Quantity = dtItemsCount.Rows[0]["CN_Quantity"].ToString().Trim();
                                    Tech_Quantity = dtItemsCount.Rows[0]["CN_Tech_Quantity"].ToString().Trim();
                                    Memo_Quantity = dtItemsCount.Rows[0]["CN_Memo_Quantity"].ToString().Trim();
                                    Test_Quantity = dtItemsCount.Rows[0]["CN_Test_Quantity"].ToString().Trim();
                                    Required_Quantity = dtItemsCount.Rows[0]["CN_Required_Quantity"].ToString().Trim();
                                    Other_Quantity = dtItemsCount.Rows[0]["CN_Other_Quantity"].ToString().Trim();
                                    Ballon_No = dtItemsCount.Rows[0]["CN_Ballon_No"].ToString().Trim();
                                    Comment = dtItemsCount.Rows[0]["CN_Comment"].ToString().Trim();

                                }
                                strSQL = " Insert into M_Demand_DetailedList_Draft"
                                    + " (VerCode, Class_Id, Object_Id, Stage, Material_State, Material_Tech_Condition, Material_Code, ParentId, Material_Spec, TDM_Description"
                                    + " , Material_Name, PackId, TaskId, DraftId, Drawing_No, Technics_Line, Technics_Comment, Material_Mark, ItemCode1, ItemCode2"
                                    + " , MaterialsNum, Mat_Unit, Lingjian_Type, Mat_Rough_Weight, Mat_Pro_Weight, Mat_Weight, Mat_Efficiency, Mat_Comment, Mat_Technics, Rough_Spec"
                                    + " , Rough_Size, MaterialsDes, StandAlone, ThisTimeOperation, PredictDeliveryDate, DemandNumSum, NumCasesSum, DemandDate, Quantity, Tech_Quantity"
                                    + " , Memo_Quantity, Test_Quantity, Required_Quantity, Other_Quantity, Ballon_No, Comment, Is_allow_merge, Import_Date, User_ID, Is_del"
                                    + " ,TaskCode, MaterialDept, MissingDescription, CN_Material_State)"
                                    + " select '" + verCode + "','" + class_Id + "','" + Object_Id + "','" + datarow["Phase"].ToString() + "','0'"
                                    + " ,'" + datarow["CN_Material_Tech_Condition"].ToString() + "'"
                                    + " , Material_Code + '-' +  Convert(nvarchar(50),(select count(*) + 1 from M_Demand_DetailedList_Draft where ParentID = a.ID))"
                                    + " , ID,'" + datarow["CN_Material_Spec"].ToString() + "','" + datarow["TDM_Description"].ToString() + "'"
                                    + " ,'" + datarow["CN_Material_Name"].ToString() + "','" + PackId + "',TaskID, DraftId,'" + datarow["CN_Drawing_No"].ToString() + "'"
                                    + " ,'" + datarow["CN_Technics_Line"].ToString() + "','" + datarow["CN_Technics_Comment"].ToString() + "','" + datarow["CN_Material_Mark"].ToString() + "','" + datarow["CN_ItemCode1"].ToString() + "','" + datarow["CN_ItemCode2"].ToString() + "'"
                                    + " ,Null,'" + datarow["CN_Mat_Unit"].ToString() + "','" + datarow["CN_Lingjian_Type"].ToString() + "','" + datarow["CN_Mat_Rough_Weight"].ToString() + "','" + datarow["CN_Mat_Pro_Weight"].ToString() + "'"
                                    + " ,'" + datarow["CN_Mat_Weight"].ToString() + "','" + datarow["CN_Mat_Efficiency"].ToString() + "','" + datarow["CN_Mat_Comment"].ToString() + "','" + datarow["CN_Mat_Technics"].ToString() + "','" + datarow["CN_Rough_Spec"].ToString() + "'"
                                    + " ,'" + datarow["CN_Rough_Size"].ToString() + "','', Null , Null, Null"
                                    + " , Null, case when isnumeric(NumCasesSum) =1 and isnumeric('" + Quantity + "') =1 then NumCasesSum * Convert(decimal(18,4),'" + Quantity + "') else Null end, Null, '" + Quantity + "', '" + Tech_Quantity + "'"
                                    + " ,'" + Memo_Quantity + "','" + Test_Quantity + "','" + Required_Quantity + "','" + Other_Quantity + "','" + Ballon_No + "'"
                                    + " ,'" + Comment + "','false',GetDate(),'" + User_ID + "','false'"
                                    + " ,TaskCode , '','' ,'" + datarow["CN_Material_State"].ToString() + "' from M_Demand_DetailedList_Draft as a where PackId = '" + PackId + "' and Object_Id = '" + PObject_ID + "' and Class_Id = '" + PClass_ID + "' and Is_Del = 'false'"
                                    + "";
                                DBI.Execute(strSQL);
                            }
                        }
                    }
                }
                UpdateDemandNumSum(PackId, verCode);

                DBI.CommitTrans();
                RadNotificationAlert.Text = "更改成功！";
                RadNotificationAlert.Show();
                //---20160101-lrw-begin---------------------------------

                strSQL = " declare @sql nvarchar(max) select @sql = '' select @sql = @sql + Convert(nvarchar(50),ID) + ',' from M_Demand_DetailedList_Draft "
                    + " where PackId = '" + PackId + "' and VerCode = '" + verCode + "' and (Material_State = '2' or (Material_State in ('1','2','6','7') and Is_Del = 'true')) "
                    + " select case when @sql = '' then '' else SUBSTRING(@sql,1,len(@sql)-1) end";
                Session["DraftIdStr"] = DBI.GetSingleValue(strSQL);
                if (Session["DraftIdStr"] != null && Session["DraftIdStr"].ToString() != "")
                {
                    RadButtonBuild.Attributes["onclick"] = "return ShowMDemandChangeSubmit('" + PackId + "')";
                    RadButtonBuild.Visible = true;
                }
                else {
                    RadButtonBuild.Visible = false;
                }
                //---20160101-lrw-end-----------------------------------
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                DBI.RollbackTrans();
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        //修改更改单变化的M_Demand_DetailesList_Draft表材料状态，计算共计需求量（kg）
        private void UpdateDemandNumSum(string PackID, string VerCode)
        {
            string strSQL = "";
            DataTable dtcf = DBI.Execute(" select Top 1 * from Sys_ComputationalFormula", true);
            double pt1 = 0, pt2 = 0, pt3 = 0;
            if (dtcf.Rows.Count > 0)
            {
                pt1 = Convert.ToDouble(dtcf.Rows[0]["Parameter1"].ToString());//直径
                pt2 = Convert.ToDouble(dtcf.Rows[0]["Parameter2"].ToString());//总长度
                pt3 = Convert.ToDouble(dtcf.Rows[0]["Parameter3"].ToString());//加持余量
            }
            //先将缺项说明清空//将状态4、5的置为0，状态2、5、6、7的置为1，重置材料状态
            strSQL = " Update M_Demand_DetailedList_Draft set MissingDescription = '' "
                + " , Material_State = case when Material_State in ('4','5') then '0' when Material_State in ('2','6','7') then '1' else Material_State end"
                + " where  PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'";
            //原来不需要材料定额，现在改为需要材料定额（根据零件类型判断是否需要材料等额) , 原状态为3的改为状态0
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = 0"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false' and Material_State = '3'"
                + " and Replace(LingJian_Type,' ','') in (select LingJian_Type_Code from Sys_LingJian_Info where Is_MDDLD_Show = 'true' and Is_Del = 'false')";
            //需要材料定额的，修改物资领料部门            
            strSQL += " update M_Demand_DetailedList_Draft set MaterialDept=dbo.F_SearchMaterialDept(Technics_Line) where PackId = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'";

            //不需要材料定额的(根据零件类型、领料部门判断是否需要材料定额)，未提交的状态为3，已提交的状态为7
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6, 7) then '7' else '3' end";
            strSQL += " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Replace(LingJian_Type,' ','') not in (select LingJian_Type_Code from Sys_LingJian_Info where Is_MDDLD_Show = 'true' and Is_Del = 'false')"
                + " or (MaterialDept = '' or MaterialDept is Null))";

            //缺失材料定额数据， 未提交的状态为4，提交的状态为6
            //缺失牌号
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '牌号'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and (Material_Mark is null or REPLACE(Material_Mark,' ','') = '')";
            //缺失物资名称
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资名称'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and (Material_Name is null or REPLACE(Material_Name,' ','')= '') ";
            //缺失物资编码
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资编码'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and (ItemCode1 is null or REPLACE(ItemCode1,' ','') = '')";
            //缺失计量单位
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '计量单位'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and (Mat_Unit is null or REPLACE(Mat_Unit,' ','') = '')";
            //缺失物资件数
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资件数'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and Quantity is null";
            //缺失单件质量
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '单件质量'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and (Mat_Rough_Weight is null or REPLACE(Mat_Rough_Weight,' ','') = '')";
            //缺失每产品质量
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '每产品质量'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and (Mat_Pro_Weight is null or REPLACE(Mat_Pro_Weight,' ','') = '')";
            //缺失物资规格
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资规格'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + "  and (Material_State <> '3' and Material_State <> '7') and (Rough_Spec is null or REPLACE(Rough_Spec,' ','') = '')";
            //缺失物资尺寸
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '4' end, MissingDescription = case when MissingDescription = '' then '缺失' else '、' end + '物资尺寸'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and (Rough_Size is null or REPLACE(Rough_Size,' ','') = '')";
            //单件质量不是数字的，为不规范数据，未提交的状态为5，提交的状态为6
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '5' end, MissingDescription = case when MissingDescription = '' then '' else '，' end + '单件质量不是数字'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and IsNumeric (Replace(Mat_Rough_Weight,' ','')) = 0";
            //产品数量不是数字的，为不规范数据，未提交的状态为5，提交的状态为6
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State in (1, 2, 6) then '6' else '5' end, MissingDescription = case when MissingDescription = '' then '' else '，' end + '产品数量不是数字'"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and IsNumeric (Replace(Quantity,' ','')) = 0";
            //没有共计需求件数的， 为不规范数据，未提交的状态为5，提交的状态为6
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '5', MissingDescription = MissingDescription + case when MissingDescription = '' then '' else '，' end + ''"
               + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false'"
                + " and (Material_State <> '3' and Material_State <> '7') and NumCasesSum is null";
            //物资名称中含‘棒’的,物资规格不是‘φ’+数字的，或者物资尺寸不是以‘L=’开头的,为不规范，未提交的状态为5，提交的状态为6
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State = '0' then '5' else '6' end, MissingDescription  = case when MissingDescription = '' then '' else '，' end + '物资规格不是‘φ+数字’' "
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false' and (Material_State = '0' or Material_State = '1' or Material_State = '2')"
                + " and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Spec,' ',''),1,1) != 'φ' or IsNumeric (substring(Replace(Rough_Spec,' ',''),2,len(Replace(Rough_Spec,' ','')))) = 0)";
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State = '0' then '5' else '6' end, MissingDescription  = case when MissingDescription = '' then '' else '，' end + '物资尺寸不是以‘L=’开头' "
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false' and (Material_State = '0' or Material_State = '1' or Material_State = '2')"
                + " and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Size,' ',''),1,2) <> 'l=' and substring(Replace(Rough_Size,' ',''),1,2) <> 'L=')";
            DBI.Execute(strSQL);

            //计算物资需求量（kg)
            //物资名称中不含‘棒’的，物资需求量（kg） = 单件质量 * 共计需求件数
            strSQL = " Update M_Demand_DetailedList_Draft set DemandNumSum = Mat_Rough_Weight * NumCasesSum"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false' and (Material_State = '0' or Material_State = '1' or Material_State = '2')"
                + " and Replace(LingJian_Type,' ','') in (select LingJian_Type_Code from Sys_LingJian_Info where Is_MDDLD_Show = 'true' and Is_Del = 'false') and Material_Name not like '%棒%'";
            //物资名称中含‘棒’的，物资需求量（kg）的计算            
            //物资名称中含‘棒’
            strSQL += " select ID, Rough_Size, Rough_Spec, NumCasesSum, Mat_Rough_Weight from M_Demand_DetailedList_Draft"
                + " where PackID = '" + PackID + "' and VerCode = '" + VerCode + "' and Is_del = 'false' and (Material_State = '0' or Material_State = '1' or Material_State = '2')"
                + " and Material_Name like '%棒%'"
                + " and (substring(Replace(Rough_Size,' ',''),1,2) = 'l=' or substring(Replace(Rough_Size,' ',''),1,2) = 'L=') and  IsNumeric (substring(Replace(Rough_Size,' ',''),3,len(Replace(Rough_Size,' ','')))) = 0 ";

            DataTable dt = DBI.Execute(strSQL, true);

            string UpdateID = "('0'"; //不规范的ID序列
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ID = dt.Rows[i]["ID"].ToString().Trim();
                //size 去掉“L=”之后的字符串
                string size = dt.Rows[i]["Rough_Size"].ToString().Replace(" ", "").Substring(2, dt.Rows[i]["Rough_Size"].ToString().Replace(" ", "").Length - 2);
                double spec = Convert.ToDouble(dt.Rows[i]["Rough_Spec"].ToString().Replace(" ", "").Substring(1, dt.Rows[i]["Rough_Spec"].ToString().Replace(" ", "").Length - 1));
                double NumCasesSum = Convert.ToDouble(dt.Rows[i]["NumCasesSum"].ToString());
                double Mat_Rough_Weight = Convert.ToDouble(dt.Rows[i]["Mat_Rough_Weight"].ToString());
                double DemandNumSum = 0;
                string JSGS_Des = "";
                if (size == "")
                {
                    UpdateID += " ,'" + ID + "'";
                    continue;
                }
                else
                {
                    //直径小于pt1的计算
                    if (spec < pt1)
                    {
                        //直径小于pt1的尺寸只能是"L="+数字，否则为不规范
                        try
                        {
                            double size1 = Convert.ToDouble(size);
                            double sumsize = size1 * NumCasesSum;
                            if (pt2 != 0)
                            {
                                sumsize = sumsize + Math.Ceiling(sumsize / pt2) * pt3;
                                DemandNumSum = sumsize * size1 / Mat_Rough_Weight;
                                JSGS_Des = "直径小于" + pt1.ToString() + ",总长度每增加" + pt2.ToString() + "增加一个夹持量" + pt3.ToString();
                            }
                            else
                            {
                                DemandNumSum = size1 * NumCasesSum * Mat_Rough_Weight;
                            }
                        }
                        catch
                        {
                            UpdateID += ",'" + ID + "'";
                            continue;
                        }
                    }
                    //直径大于等于pt1的计算
                    else
                    {
                        try
                        {
                            double size1 = Convert.ToDouble(size);
                            DemandNumSum = size1 * NumCasesSum * Mat_Rough_Weight;
                        }
                        catch
                        {
                            if (size.Split('+').Length == 2)
                            {
                                try
                                {
                                    double size1 = Convert.ToDouble(size.Split('+')[0]);
                                    double size2 = Convert.ToDouble(size.Split('+')[1]);
                                    DemandNumSum = ((size1 + size2) / size1) * NumCasesSum * Mat_Rough_Weight;
                                    JSGS_Des = "直径大于等于" + pt1.ToString() + ",每一个增加一个加持余量" + size2.ToString();
                                }
                                catch
                                {
                                    UpdateID += ",'" + ID + "'";
                                    continue;
                                }
                            }
                            else
                            {
                                UpdateID += ",'" + ID + "'";
                                continue;
                            }
                        }
                    }
                }
                if (DemandNumSum != 0)
                {
                    strSQL = " Update M_Demand_DetailedList_Draft set DemandNumSum = '" + DemandNumSum.ToString() + "'";
                    strSQL += " ,JSGS_Des = '" + JSGS_Des + "' where ID = '" + ID + "'";
                    DBI.Execute(strSQL);
                }
            }
            UpdateID += " )";
            strSQL = " Update M_Demand_DetailedList_Draft set Material_State = case when Material_State = '0' then '5' else '6' end, MissingDescription  = case when MissingDescription = '' then '' else '，' end + '物资尺寸系统不能识别' where ID in " + UpdateID;
            //已提交Material_State = '1'的，物资编码、规格、尺寸、计量单位、单件质量，共计需求件数，共计需求量 有变化的状态改为2
            //根据物流中心接口说明，物资尺寸、需求数量、件数 有变化需要重新提交状态改为2 (单件质量影响需求数量，所以也重新提交)
            //计量单位暂不处理
            strSQL += " Update M_Demand_DetailedList_Draft set Material_State = '2' "
                + " where PackId= '" + PackID + "' and VerCode = '" + VerCode + "' and Material_State = '1'"
                + " and checksum (ItemCode1, Rough_Spec, Rough_Size,Mat_Rough_Weight, NumCasesSum, DemandNumSum) not in "
                + " (select top 1 checksum(ItemCode1, Rough_Spec, Rough_Size,Mat_Rough_Weight, NumCasesSum, DemandNumSum) "
                + " from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50),M_Demand_DetailedList_Draft.ID) order by ID desc)";

            //M_Draft_List的材料定额状态  //P_Pack 草稿状态
            //没有材料信息
            strSQL += " if (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and Is_del = 'false' and (Material_State = '0' or  Material_State = '1' or Material_State = '2')) = 0  begin"
            + " Update M_Draft_List set Material_State = '0' where PackID = '" + PackID + "' Update P_Pack set DraftStatus = '1' where PackId = '" + PackID + "' end"
                //待补全
            + " else begin if (select count(*) from M_Demand_DetailedList_Draft where PackID = '" + PackID + "' and Is_del = 'false' and (Material_State = '4' or Material_State = '5' or Material_State = '6')) > 0"
            + " begin Update M_Draft_List set Material_State = '2' where PackID = '" + PackID + "' Update P_Pack set DraftStatus = '2' where PackID = '" + PackID + "' end"
                //完成
            + " else begin Update M_Draft_List set Material_State = '1' where PackID = '" + PackID + "'  Update P_Pack set DraftStatus = '3' where PackID = '" + PackID + "' end end";

            DBI.Execute(strSQL);
        }


        #endregion
    }
}