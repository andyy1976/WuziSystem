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
    public partial class TechnologyTest : System.Web.UI.Page
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
                    InitTable.Columns.Add("rownum");
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("MDP_Code");
                    InitTable.Columns.Add("SecretLevel");
                    InitTable.Columns.Add("SpecialNeeds");
                    InitTable.Columns.Add("UrgencyDegre");
                    InitTable.Columns.Add("stage1");
                    InitTable.Columns.Add("UseDes");
                    InitTable.Columns.Add("Certification1");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("Drawing_No");
                    InitTable.Columns.Add("TaskCode");
                    InitTable.Columns.Add("MaterialDept");
                    InitTable.Columns.Add("ItemCode1");
                    InitTable.Columns.Add("DemandNumSum");
                    InitTable.Columns.Add("NumCasesSum");
                    InitTable.Columns.Add("Mat_Unit");
                    InitTable.Columns.Add("Quantity");
                    InitTable.Columns.Add("Rough_Size");
                    InitTable.Columns.Add("Dinge_Size");
                    InitTable.Columns.Add("Rough_Spec");
                    InitTable.Columns.Add("DemandDate");
                    InitTable.Columns.Add("Special_Needs");
                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Stage");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Unit_Price");
                    InitTable.Columns.Add("Sum_Price");
                    InitTable.Columns.Add("Submit_Type");
                    InitTable.Columns.Add("Is_Submit");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("Contact_Way");
                    InitTable.Columns.Add("Submit_Date");
                    InitTable.Columns.Add("Get_Quantity");
                    InitTable.Columns.Add("subtype");
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
        public string title = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                if (Request.QueryString["MDPID"] != null && Request.QueryString["MDPID"].ToString() != "")
                {
                    string MDPLID = Request.QueryString["MDPID"].ToString();
                    string strSql = " select * from M_Demand_Plan_List where ID = '" + MDPLID + "'";
                    DataTable dtmdpl = DBI.Execute(strSql, true);

                    this.span_gysyjCode.InnerText = dtmdpl.Rows[0]["MDP_Code"].ToString(); // Request.QueryString["MDP_Code"].ToString();

                    string stype = dtmdpl.Rows[0]["Submit_Type"].ToString(); //Request.QueryString["SubmitType"].ToString();
                    this.ViewState["SubmitType"] = stype;
                    //switch (stype)
                    //{
                    //    case "1":
                    //        this.span_title.InnerHtml = title = "工艺试验件"; break;
                    //    case "2":
                    //        this.span_title.InnerHtml = title = "技术创新课题"; break;
                    //    case "3":
                    //        this.span_title.InnerHtml = title = "车间备料"; break;
                    //}
                    //Page.Title = title;

                    //title = title + "－详细";
                    //this.span_title.InnerHtml = this.span_title.InnerHtml + "－详细";
                    
                    if (dtmdpl.Rows[0]["Submit_State"].ToString() == "2")
                    {
                        BtnSubmit.Visible = true;
                    }

                    if (dtmdpl.Rows[0]["Submit_State"].ToString() == "4")
                    {
                        Session["GridSource"] = GetMDMLSuc(MDPLID);
                        Session["GridSourceErr"] = GetMDMLErr(MDPLID);
                        if ((Session["GridSourceErr"] as DataTable).Rows.Count == 0)
                        {
                            Session["GridSourceErr"] = null;
                        }
                    }
                    else
                    {
                        Session["GridSource"] = GetMDMLAll(MDPLID);
                        Session["GridSourceErr"]= null;
                    }

                    string userDomainAccount = DBI.GetSingleValue("select DomainAccount from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "'");

                    K2PreBLL k2PreBll = new K2PreBLL();
                    try
                    {
                        k2Pre.ApproveInfoBody[] body = k2PreBll.k2PreGetApproveBody(MDPLID, userDomainAccount);
                        Session["GridSourceApp"] = body;
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "暂时不能与流程平台通信，<br />不能获取流程信息";
                        RadNotificationAlert.Show();
                    }
                }
                else
                {
                    Response.Redirect("~/Plan/TechnologyTestList.aspx");
                }                
            }
        }

        protected DataTable GetTechnologyTestList(string MDPID)
        {
            try
            {
                string strSQL =
                        " select (ROW_NUMBER() OVER (ORDER BY M_Demand_Merge_List.ID)) AS rownum, MDP_Code" +
                        " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes" + // , GetCustInfo_T_ACCT_SITE.ADDRESS as ADDRESS" +
                        " , CUX_DM_PROJECT.DICT_Name as Model" +
                        " , (select Phase from Sys_Phase where Code = M_Demand_Merge_List.Stage) as Stage1" +
                        " , M_Demand_Merge_List.*" +
                        " from M_Demand_Merge_List join M_Demand_Plan_List on M_Demand_Plan_List.Id = M_Demand_Merge_List.MDPId" +
                        " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                        " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                        " left join GetBasicdata_T_Item as CUX_DM_PROJECT on CUX_DM_PROJECT.DICT_CODE = M_Demand_Merge_List.Project and CUX_DM_PROJECT.DICT_CLASS='CUX_DM_PROJECT'" +
                        " where MDPId = '" + MDPID + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取" + title + "数据出错" + ex.Message.ToString());
            }
        }

        protected DataTable GetMDMLAll(string MDPLID)
        {
            DataTable dt = new DataTable();
            string strSql =
                       " select (ROW_NUMBER() OVER (ORDER BY M_Demand_Merge_List.ID)) AS rownum, MDP_Code" +
                       " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes" + // , GetCustInfo_T_ACCT_SITE.ADDRESS as ADDRESS" +
                       ", CUX_DM_PROJECT.DICT_Name as Model" +
                       " , (select Phase from Sys_Phase where Code = M_Demand_Merge_List.Stage) as Stage1" +
                       " , M_Demand_Merge_List.*" +
                       " from M_Demand_Merge_List join M_Demand_Plan_List on M_Demand_Plan_List.Id = M_Demand_Merge_List.MDPId" +
                       " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                       " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                       " left join GetBasicdata_T_Item as CUX_DM_PROJECT on CUX_DM_PROJECT.DICT_CODE = M_Demand_Merge_List.Project and CUX_DM_PROJECT.DICT_CLASS='CUX_DM_PROJECT'" +
                       " where MDPId = '" + MDPLID + "'";
            dt = DBI.Execute(strSql, true);
            return dt;
        }
        protected DataTable GetMDMLSuc(string MDPLID)
        {
            DataTable dt = new DataTable();
            string strSql = " select (ROW_NUMBER() OVER (ORDER BY M_Demand_Merge_List.ID)) AS rownum, MDP_Code" +
                       " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes" + // , GetCustInfo_T_ACCT_SITE.ADDRESS as ADDRESS" +
                       " , (select Phase from Sys_Phase where Code = M_Demand_Merge_List.Stage) as Stage1" +
                       " , isnull((select top 1 Submission_Status from GetRqStatus_T_Item where USER_RQ_LINE_ID = M_Demand_Merge_List.ID order by SUBMITED_SYNC_STATUS desc),'已提交') as State" +
                       " , M_Demand_Merge_List.*" +                      
                       " from M_Demand_Merge_List join M_Demand_Plan_List on M_Demand_Plan_List.Id = M_Demand_Merge_List.MDPId" +
                       " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                       " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +                      
                       " where MDPId = '" + MDPLID + "' and Is_Submit = 'true'";
            dt = DBI.Execute(strSql, true);
            return dt;
        }
        protected DataTable GetMDMLErr(string MDPLID)
        {
            DataTable dt = new DataTable();
            string strSql = " select (ROW_NUMBER() OVER (ORDER BY M_Demand_Merge_List.ID)) AS rownum, MDP_Code" +
                    " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes" + // , GetCustInfo_T_ACCT_SITE.ADDRESS as ADDRESS" +
                    " , (select Phase from Sys_Phase where Code = M_Demand_Merge_List.Stage) as Stage1" +
                    " , M_Demand_Merge_List.*" +
                     " , Err_Msg" +
                    " from M_Demand_Merge_List join M_Demand_Plan_List on M_Demand_Plan_List.Id = M_Demand_Merge_List.MDPId" +
                    " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                    " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                     " left join WriteReqOrder_RecList on WriteReqOrder_RecList.User_RQ_Line_ID = M_Demand_Merge_List.ID" +
                    " where MDPId = '" + MDPLID + "' and Is_Submit = 'false'";
            dt = DBI.Execute(strSql, true);
            return dt;
        }

        protected void RadGrid_TechnologyTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_TechnologyTest.DataSource = Session["GridSource"];
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyTest.ExportSettings.FileName = "物资需求清单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyTest.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyTest.ExportSettings.FileName = "物资需求清单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyTest.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_TechnologyTest.ExportSettings.FileName = "物资需求清单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_TechnologyTest.ExportSettings.IgnorePaging = true;
            RadGrid_TechnologyTest.MasterTableView.ExportToPdf();
            RadGrid_TechnologyTest.ExportSettings.IgnorePaging = false;
        }

        protected void RadGridApp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridApp.DataSource = Session["GridSourceApp"];
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string MDPLID = Request.QueryString["MDPID"].ToString();
            LogisticsCenterBLL bll = new LogisticsCenterBLL();
            string result = bll.WriteReqOrderRepeat(MDPLID);
            if (result == "")
            {
                RadNotificationAlert.Text = "提交成功！";
                RadNotificationAlert.Show();

                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);

                //BtnSubmit.Visible = false;

                //Session["GridSource"] = GetMDMLSuc(MDPLID);
                //Session["GridSourceErr"] = GetMDMLErr(MDPLID);
                //if ((Session["GridSourceErr"] as DataTable).Rows.Count == 0)
                //{
                //    Session["GridSourceErr"] = null;
                //}
                //RadGrid_TechnologyTest.Rebind();
                //RadGridErr.Rebind();

            }
            else
            {
                RadNotificationAlert.Text = result;
                RadNotificationAlert.Show();
            }
        }

        protected void RadGridErr_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridErr.DataSource = Session["GridSourceErr"];
        }
    }
}