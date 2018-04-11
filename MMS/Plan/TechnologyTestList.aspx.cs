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
    public partial class TechnologyTestList : System.Web.UI.Page
    {
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
                    InitTable.Columns.Add("MDP_Code");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("DraftId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("Submit_Date");
                    InitTable.Columns.Add("Submit_Type");
                    InitTable.Columns.Add("Return_State");
                    InitTable.Columns.Add("subtype");
                    InitTable.Columns.Add("reState");
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
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }

                if (Request.QueryString["t"] != null && Request.QueryString["t"].ToString() != "")
                {
                    string title = "";
                    switch (Request.QueryString["t"].ToString())
                    {
                        case "1":
                            Common.CheckPermission(Session["UserName"].ToString(), "TechnologyTestList1", this.Page);
                            title = "新增工艺试验件";
                            HiddenField.Value = "物资需求-->工艺试验件任务"; 
                            break;
                        case "2":
                            Common.CheckPermission(Session["UserName"].ToString(), "TechnologyTestList2", this.Page);
                            title = "新增技术创新课题";
                            HiddenField.Value = "物资需求-->技术创新课题任务";
                            break;
                        case "3":
                            Common.CheckPermission(Session["UserName"].ToString(), "TechnologyTestList3", this.Page);
                            title = "新增车间备料任务";
                            HiddenField.Value = "物资需求-->车间备料任务";
                            break;
                    }
                    this.ViewState["submit_type"] = Request.QueryString["t"].ToString();
                    btnNewAdd.Text = title;
                    GridSource = Common.AddTableRowsID(GetTechnologyTestList(null));
                    btnNewAdd.Attributes["onclick"] = "return ShowTechnologyTestAdd(''," + Request.QueryString["t"].ToString() + ")";
                    RadTabStrip1.Tabs[1].NavigateUrl = "TechnologyTestListChange.aspx?t=" + Request.QueryString["t"].ToString();
                }
                else
                {
                    Response.Redirect(Page.Request.UrlReferrer.ToString());
                }
            }
        }

        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string MDP_Code = RTB_MDP_Code.Text.Trim();
            string startTime = RDPStart.SelectedDate.ToString();
            string endTime = RDPEnd.SelectedDate.ToString();
            string Submit_State = RDDL_AppState.SelectedItem.Value;
            Session["StrWhere"] = "";
            if (Submit_State != "")
            {
                Session["StrWhere"] += " and Submit_State = '" + Submit_State + "'";
            }
            if (MDP_Code != "")
            {
                Session["StrWhere"] += " and MDP_Code like '%" + MDP_Code + "%'";
            }
            try
            {
                Session["StrWhere"] += " and SUBMIT_DATE >= '" + Convert.ToDateTime(startTime).ToString() + "'";
            }
            catch { }
            try
            {
                Session["StrWhere"] += " and SUBMIT_DATE <= '" + Convert.ToDateTime(endTime).ToString() + "'";
            }
            catch { }

            GridSource = Common.AddTableRowsID(GetTechnologyTestList(Session["StrWhere"].ToString()));
            RadGrid_TechnologyTestList.Rebind();
        }

        protected DataTable GetTechnologyTestList(string strWhereCondition)
        {
            string stype = this.ViewState["submit_type"].ToString();
            try
            {
                string strSQL = "";
                string UserName = Session["UserName"].ToString();

                if (UserName == "Admin" || UserName == "admin")
                {
                    //strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,* from V_M_Demand_Plan_List where Submit_Type=" + stype;
                    strSQL = " select (ROW_NUMBER() OVER(ORDER BY Submit_Date desc)) AS rownum, M_Demand_Plan_List.* , UserAccount, UserName" +
                        " , case when submit_Type = '1' then '工艺试验件' when submit_Type = '2' then '技术创新课题' when submit_Type ='3' then '车间备料' else Convert(nvarchar(50),submit_Type) end as subtype" +
                        " , case when submit_State = '0' then '未提交' when submit_State = '1' then '进入流程平台' when submit_State = '2' then '已审批，已通过' when submit_State = '3' then '已审批，未通过' else '已提交物流' end as substate " +
                        " , (select Convert(nvarchar(50),count(*)) from M_Demand_Merge_List where MDPID = M_Demand_Plan_List.ID and Is_Submit = '1')" +
                        " + '/' + (select Convert(nvarchar(50),Count(*)) from M_Demand_Merge_List where MDPID = M_Demand_Plan_List.ID)  as SubmitCount" +
                        " from M_Demand_Plan_List left join Sys_UserInfo_PWD on M_Demand_Plan_List.User_ID = Sys_UserInfo_PWD.ID" +
                        " where Submit_Type = '" + stype + "' and M_Demand_Plan_List.ID in (select MDPID from M_Demand_Merge_List)" +
                        strWhereCondition + " and planType!='1' order by Submit_Date desc";
                }
                else
                {
                    //strSQL = "select (ROW_NUMBER() OVER(ORDER BY ID)) AS rownum,* from V_M_Demand_Plan_List where Submit_Type=" + stype+" and UserAccount='" + UserName + "'";
                    strSQL = " select (ROW_NUMBER() OVER(ORDER BY Submit_Date desc)) AS rownum, M_Demand_Plan_List.* , UserAccount, UserName" +
                        " , case when submit_Type = '1' then '工艺试验件' when submit_Type = '2' then '技术创新课题' when submit_Type ='3' then '车间备料' else Convert(nvarchar(50),submit_Type) end as subtype" +
                        " , case when submit_State = '0' then '未提交' when submit_State = '1' then '进入流程平台' when submit_State = '2' then '已审批，已通过' when submit_State = '3' then '已审批，未通过' else '已提交物流' end as substate " +
                        " ,  (select Convert(nvarchar(50),count(*)) from M_Demand_Merge_List where MDPID = M_Demand_Plan_List.ID and Is_Submit = '1')" +
                        " + '/' + (select Convert(nvarchar(50),Count(*)) from M_Demand_Merge_List where MDPID = M_Demand_Plan_List.ID)  as SubmitCount" +
                        " from M_Demand_Plan_List left join Sys_UserInfo_PWD on M_Demand_Plan_List.User_ID = Sys_UserInfo_PWD.ID" +
                        " where Submit_Type = '" + stype + "' and M_Demand_Plan_List.ID in (select MDPID from M_Demand_Merge_List) and UserAccount='" + UserName + "'" +
                        strWhereCondition + " and planType!='1' order by Submit_Date desc";
                }
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception((stype == "1" ? "获取工艺试验件清单信息出错" : (stype == "2" ? "获取技术创新课题清单信息出错" : "获取车间备料清单信息出错")) +
                                    ex.Message.ToString());
            }
        }

        protected void RadGrid_TechnologyTestList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_TechnologyTestList.DataSource = GridSource;
        }
        protected void RadGrid_TechnologyTestList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            DataTable table = GridSource;
            GridDataItem dataitem = e.Item as GridDataItem;
            if (e.CommandName == "Submit")
            {
                string ID = dataitem.GetDataKeyValue("ID").ToString();
                Response.Redirect("~/Plan/TechnologyTestAdd.aspx?MDPId=" + ID);
            }
        }
        protected void RadGrid_TechnologyTestList_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                Label lbltop = e.Item.FindControl("lbltop") as Label;
                if (lbltop != null)
                {
                    switch (Request.QueryString["t"].ToString())
                    {
                        case "1":
                            lbltop.Text = "工艺试验件任务";
                            break;

                        case "2":
                            lbltop.Text = "技术创新课题任务";
                            break;
                        case "3":
                            lbltop.Text = "车间备料任务";
                            break;
                    }
                }
            }
            if (e.Item is GridDataItem)
            {
                string MDPID = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                string Submit_State = GridSource.Select("ID='" + MDPID + "'")[0]["Submit_State"].ToString();
                string strSql = "";
                                
                DataTable table = GridSource;
                
                string MDP_Code = table.Rows[e.Item.DataSetIndex]["MDP_Code"].ToString();
                string SubmitType = this.ViewState["submit_type"].ToString();//1－工艺试验件；2－技术创新课题；3－车间备料
                RadButton btnDetails = e.Item.FindControl("RadButtonDetails") as RadButton;
                if (btnDetails != null)
                {
                    btnDetails.Attributes["onclick"] = "return ShowTechnologyTestListDetails('" + MDP_Code + "','" + MDPID + "','" + SubmitType + "','1')";
                }
                RadButton btnSubmit = e.Item.FindControl("RadBtnSubmit") as RadButton;
                if (btnSubmit != null)
                {
                    btnSubmit.Attributes["onclick"] = "return ShowTechnologyTestAdd('" + MDPID + "','" + SubmitType + "')";
                }
                if (Submit_State == "0")
                {
                    if (btnDetails != null)
                    {
                        btnDetails.Visible = false;
                    }
                    //btnChange.Visible = false;
                    if (btnSubmit != null)
                    {
                        btnSubmit.Visible = true;
                    }
                }
                else 
                {
                    if (btnDetails != null)
                    {
                        btnDetails.Visible = true;
                    }
                    //btnChange.Visible = true;
                    if (btnSubmit != null)
                    {
                        btnSubmit.Visible = false;
                    }
                }

                if (Submit_State == "1")
                {
                    string userDomainAccount = DBI.GetSingleValue("select DomainAccount from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "'");
                    K2PreBLL k2prebll = new K2PreBLL();
                    try
                    {
                        k2Pre.ApproveInfoHead head = k2prebll.K2PreGetApproveHeader(MDPID, userDomainAccount);
                        if (head != null)
                        {
                            if (head.AppState == 1 || head.AppState == 2)
                            {
                                strSql = " Update M_Demand_Plan_List set Submit_State = '" +
                                         (head.AppState + 1).ToString() + "' where ID = '" + MDPID + "'";
                                DBI.Execute(strSql);
                                GridSource.Select("ID='" + MDPID + "'")[0]["Submit_State"] =
                                    (head.AppState + 1).ToString();
                                if (head.AppState == 1)
                                {
                                    (e.Item as GridDataItem)["substate"].Text = "已审批，已通过";
                                }
                                else
                                {
                                    (e.Item as GridDataItem)["substate"].Text = "已审批，未通过";
                                }
                            }
                        }
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "暂时不能与流程平台通信，<br />不能获取流程信息";
                        RadNotificationAlert.Show();
                    }
                }                
            }
        }


        protected void RadAjaxManager1_OnAjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            GridSource = Common.AddTableRowsID(GetTechnologyTestList(null));
            RadGrid_TechnologyTestList.Rebind();
        }


        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyTestList.ExportSettings.FileName = "天津公司车间备料" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyTestList.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyTestList.ExportSettings.FileName = "天津公司车间备料" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyTestList.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyTestList.ExportSettings.FileName = "天津公司车间备料" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyTestList.ExportSettings.IgnorePaging = true;
            RadGrid_TechnologyTestList.MasterTableView.ExportToPdf();
            RadGrid_TechnologyTestList.ExportSettings.IgnorePaging = false;
        }
    }
}