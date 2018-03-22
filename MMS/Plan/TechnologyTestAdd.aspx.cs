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
using mms.PublicClass;
using System.Data.OleDb;
using System.IO;

namespace mms.Plan
{
    public partial class TechnologyTestAdd : System.Web.UI.Page
    {
        private static System.Data.DataTable GridSource1;
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
                    InitTable.Columns.Add("Dinge_Size");
                    InitTable.Columns.Add("DemandNumSum");
                    InitTable.Columns.Add("NumCasesSum");
                    InitTable.Columns.Add("Mat_Rough_Weight");
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
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
               

                GridSource1 = new System.Data.DataTable();
                if (Request.QueryString["SubmitType"] != null && Request.QueryString["SubmitType"].ToString() != "")
                {

                    string title = "", title1 = "";
                    string strSQL = "";
                    switch (Request.QueryString["SubmitType"].ToString())
                    { 
                        case "1":
                            title = "工艺试验件物资需求申请";
                            title1 = "工艺试验件物资需求未提交申请";
                            HiddenField.Value = "工艺试验件任务-->新增工艺试验件物资需求";

                            strSQL = " select UserName, DomainAccount from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '车间%' + '领导' and Is_Del ='false'";

                            RDDL_ApproveAccount1.DataSource = DBI.Execute(strSQL, true);
                            RDDL_ApproveAccount1.DataValueField = "DomainAccount";
                            RDDL_ApproveAccount1.DataTextField = "UserName";
                            RDDL_ApproveAccount1.DataBind();
                            lbl_ApproveAccount1.Text = "车间领导";
                            strSQL = " select UserName, DomainAccount from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%工艺处%型号%主管%' and Is_Del ='false'";
                            RDDL_ApproveAccount2.DataSource = DBI.Execute(strSQL, true);
                            RDDL_ApproveAccount2.DataValueField = "DomainAccount";
                            RDDL_ApproveAccount2.DataTextField = "UserName";
                            RDDL_ApproveAccount2.DataBind();
                            lbl_ApproveAccount2.Text = "工艺处型号主管";
                            break;
                        case "2":
                            title = "技术创新物资需求申请";
                            title1 = "技术创新物资需求未提交申请";
                            HiddenField.Value = "技术创新任务-->新增技术创新物资需求";
                       
                            strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%车%间%调%度%员%' and Is_Del ='false'";
                            RDDL_ApproveAccount1.DataSource = DBI.Execute(strSQL, true);
                            RDDL_ApproveAccount1.DataTextField = "UserName";
                            RDDL_ApproveAccount1.DataValueField = "DomainAccount";
                            RDDL_ApproveAccount1.DataBind();
                            lbl_ApproveAccount1.Text = "车间调度计划员";

                            strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%技%术%主%管%' and Is_Del ='false'";
                            RDDL_ApproveAccount2.DataSource = DBI.Execute(strSQL, true);
                            RDDL_ApproveAccount2.DataTextField = "UserName";
                            RDDL_ApproveAccount2.DataValueField = "DomainAccount";
                            RDDL_ApproveAccount2.DataBind();
                            lbl_ApproveAccount2.Text = "工艺技术处课题技术主管";
                            break;
                        case "3":
                            title = "车间备料物资需求申请";
                            title1 = "车间备料物资需求未提交申请";
                            HiddenField.Value = "车间备料任务-->新增车间备料物资需求";
                            //trAttribute4.Visible = true;

                            strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%车%间%调%度%员%' and Is_Del ='false'";
                            RDDL_ApproveAccount1.DataSource = DBI.Execute(strSQL, true);
                            RDDL_ApproveAccount1.DataTextField = "UserName";
                            RDDL_ApproveAccount1.DataValueField = "DomainAccount";
                            RDDL_ApproveAccount1.DataBind();
                            lbl_ApproveAccount1.Text = "车间调度计划员";

                            strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%型%号%计%划%员%' and Is_Del ='false'";
                            RDDL_ApproveAccount2.DataSource = DBI.Execute(strSQL, true);
                            RDDL_ApproveAccount2.DataTextField = "UserName";
                            RDDL_ApproveAccount2.DataValueField = "DomainAccount";
                            RDDL_ApproveAccount2.DataBind();
                            lbl_ApproveAccount2.Text = "型号主管计划员";

                            RDDL_ApproveAccount3.Visible = true;
                            lbl_ApproveAccount3.Visible = true;
                            strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%物%资%计%划%员%' and Is_Del ='false'";
                            RDDL_ApproveAccount3.DataSource = DBI.Execute(strSQL, true);
                            RDDL_ApproveAccount3.DataTextField = "UserName";
                            RDDL_ApproveAccount3.DataValueField = "DomainAccount";
                            RDDL_ApproveAccount3.DataBind();
                            lbl_ApproveAccount3.Text = "3、物资综合计划员";
                            break;
                    }
                    this.ViewState["submit_type"] = Request.QueryString["SubmitType"].ToString();
                    this.b_title.InnerHtml = title;
                    if (Request.QueryString["MDPId"] != null && Request.QueryString["MDPId"].ToString() != "")
                    {
                        this.hfBh.Value = Request.QueryString["MDPId"].ToString();
                        RadBtnSubmit.Visible = true;
                        Get_M_Technology_ApplyByMDPId(Request.QueryString["MDPId"].ToString());
                    }
                    else
                    {
                        RadBtnSubmit.Visible = false;
                    }
                    GridSource = Common.AddTableRowsID(GetTechnologyTestList(this.hfBh.Value, Request.QueryString["SubmitType"].ToString()));
   

                    BindDeptUserAddress();

                    this.span_apply_time.Text = DateTime.Now.ToString("yyyy-MM-dd"); 
                    DemandDate.SelectedDate = DateTime.Now.AddMonths(3);


                    strSQL = " select * from Sys_Phase order by Code";
                    RadComboBoxStage.DataSource = DBI.Execute(strSQL, true);
                    RadComboBoxStage.DataValueField = "Code";
                    RadComboBoxStage.DataTextField = "Phase";
                    RadComboBoxStage.DataBind();

                    strSQL =
                        " select DICT_Code, DICT_Name from GetBasicdata_T_Item  where DICT_CLASS = 'CUX_DM_PROJECT' and ENABLED_FLAG = 'Y' order by DICT_Name";
                    RDDL_Project.DataSource = DBI.Execute(strSQL, true);
                    RDDL_Project.DataValueField = "DICT_Code";
                    RDDL_Project.DataTextField = "DICT_Name";
                    RDDL_Project.DataBind();

                    if (RadComboBoxStage.FindItemByValue("3") != null)
                    {
                        RadComboBoxStage.FindItemByValue("3").Selected = true;
                    }

                    Session["gds"] = null;

                    //   strSQL = " select distinct dbo.Get_StrArrayStrOfIndex(Seg6,'.',1) as Seg6, substring(Seg5,1,4) as Type"
                    //    + " from [dbo].[GetCommItem_T_Item] order by substring(Seg5,1,4)";
                    strSQL = "select * from Sys_Wuzi_Type";

                    DataTable dt = DBI.Execute(strSQL, true);

                    RDDLMT.DataSource = dt;
                    RDDLMT.DataTextField = "Seg6";
                    RDDLMT.DataValueField = "Type";
                    RDDLMT.DataBind();

                   /*
                    strSQL = "select * from GetCommItem_T_Item where SEG10 = 'N'";
                    Session["gds"] = DBI.Execute(strSQL, true);
                    RadGrid1.DataSource = (Session["gds"] as DataTable);
                    */
                  //  Telerik.Web.UI.DropDownListItem li = new Telerik.Web.UI.DropDownListItem("物资编码查询", "ItemCode");
                    //RDDLMT.Items.Add(li);

          
                    this.span_apply_time1.InnerText = DateTime.Now.ToString("yyyy-MM-dd"); 
                    BindDeptUserAddress1();

                }
                else {
                    Response.Redirect(Page.Request.UrlReferrer.ToString());
                }
            }
        }
        protected void Get_Sys_DeptEnumByUID(string Dept)
        {
            try
            {
                string strSQL = "select * from V_Get_Sys_DeptEnumByUID where id='" + Dept + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count > 0)
                {
                    Session["DeptCode"] = dt.Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取部门信息出错" + ex.Message.ToString());
            }
        }
        protected void Get_M_Technology_ApplyByMDPId(string MDPId)
        {
            try
            {
                string strSQL = "select * from M_Demand_Plan_List where Id='" + MDPId + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count == 1)
                {
                    this.txt_TaskCode.Text = dt.Rows[0]["TaskCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取任务号出错" + ex.Message.ToString());
            }
        }
       
        protected void RadBtnSave_Click(object sender, EventArgs e)
        {
            if (IsValid) {
                string err = string.Empty;
                bool flag = true;
                //if (string.IsNullOrEmpty(span_Shipping_Address.InnerText) || span_Shipping_Address.InnerText == "")
                //{
                //    flag = false; err = "“配送地址”不得为空！";
                //}
                //if (string.IsNullOrEmpty(span_MaterialDept.InnerText) || span_MaterialDept.InnerText == "")
                //{
                //    flag = false; err = "“领料部门”不得为空！";
                //}
                //if (!PublicFunClass.ValidIsNotDecimal(span_Sum_Price.InnerText) || span_Sum_Price.InnerText == "")
                //{
                //    flag = false; err = "“总价”不得为空！";
                //}
                //if (!PublicFunClass.ValidIsNotDecimal(span_Unit_Price.InnerText) || span_Unit_Price.InnerText == "")
                //{
                //    flag = false; err = "“单价”不得为空！";
                //}
                //if (string.IsNullOrEmpty(span_Rough_Spec.InnerText) || span_Rough_Spec.InnerText == "")
                //{
                //    flag = false; err = "“坯料规格”不得为空！";
                //}
                if (string.IsNullOrEmpty(RTB_Rough_Size.Text.Trim()) || RTB_Rough_Size.Text.Trim() == "")
                {
                    flag = false; err = "“需求尺寸”不得为空！";
                }
                //if (string.IsNullOrEmpty(span_Mat_Unit.InnerText) || span_Mat_Unit.InnerText == "")
                //{
                //    flag = false; err = "“计量单位”不得为空！";
                //}
                if (Convert.ToDecimal(txt_DemandNumSum.Text) == 0)
                {
                    flag = false; 
                    err = "“共计需求量”必须大于0！";
                }
                if (Convert.ToInt32(txt_NumCasesSum.Text) == 0)
                {
                    flag = false;
                    err = "“共计需求件数”必须大于0！";
                }
                //if (string.IsNullOrEmpty(span_proposer.InnerText) || span_proposer.InnerText == "")
                //{
                //    flag = false; err = "“申请人”不得为空！";
                //}
                if (RadComboBox_User.SelectedValue == "")
                {
                    flag = false; err = "请选择申请人！";
                }
                if (RadComboBox_Dept.SelectedValue == "0")
                {
                    flag = false; err = "请选择部门！";
                }
                if (RDDL_Project.SelectedValue == "")
                {
                    flag = false; err = "请选择型号工程！";
                }

                if (!flag)
                {
                    RadNotificationAlert.Text = err;
                    RadNotificationAlert.Show();
                }
                else
                {
                    string ItemCode = txt_ItemCode1.Text.Trim();
                    string strSQL = " select count(*) from GetCommItem_T_Item where Seg3 = '" + ItemCode + "'";
                    if (DBI.GetSingleValue(strSQL) == "0")
                    {
                        RadNotificationAlert.Text = "失败！没有该物资编码，不能提交";
                        RadNotificationAlert.Show();
                        return;
                    }

                    SaveTechnologyTestInfo();
                }
            }
        }

        private void SaveTechnologyTestInfo()
        {
            M_Demand_Merge_List mta = new M_Demand_Merge_List();
            mta.Drawing_No = txt_Drawing_No.Text;
            mta.TaskCode = txt_TaskCode.Text;
            mta.DemandDate = Convert.ToDateTime(DemandDate.SelectedDate);
            mta.MaterialDept = RadComboBoxMaterialDept1.SelectedValue;//Session["DeptCode"].ToString();
            mta.ItemCode1 = txt_ItemCode1.Text;
            mta.NumCasesSum = Convert.ToDecimal(txt_NumCasesSum.Text);
            mta.DemandNumSum = Convert.ToDecimal(txt_DemandNumSum.Text);
            mta.Mat_Unit = txt_Mat_Unit.Text;//span_Mat_Unit.InnerText;
            mta.Quantity = Convert.ToInt32(txt_NumCasesSum.Text);//txt_Quantity
            mta.Rough_Size = RTB_Rough_Size.Text;//span_Rough_Size.InnerText;
            mta.Dinge_Size = RTB_Dinge_Size.Text;
            mta.Rough_Spec = txt_Rough_Spec.Text;//span_Rough_Spec.InnerText;
            mta.Mat_Rough_Weight = RTB_Mat_Rough_Weight.Text;
            mta.Special_Needs = rtb_SpecialNeeds.Text;
            mta.Urgency_Degre = RadComboBoxUrgencyDegree1.SelectedValue;
            mta.Secret_Level = RadComboBoxSecretLevel1.SelectedValue;
            mta.Stage = RadComboBoxStage.SelectedValue;
            mta.Use_Des = RadComboBoxUseDes1.SelectedValue;
            mta.Shipping_Address = RadComboBoxShipping_Address.SelectedItem.Text;//Session["Shipping_Addr"].ToString();
            mta.Certification = RadComboBoxCertification1.SelectedValue;
            mta.Project = RDDL_Project.SelectedValue;

            mta.Special_Needs = rtb_SpecialNeeds.Text.Trim();
            mta.Material_Mark = RTB_Material_Mark.Text.Trim();
            mta.CN_Material_State = RTB_CN_Material_State.Text.Trim();
            mta.Material_Tech_Condition = RTB_Material_Tech_Condition.Text.Trim(); ;        

            mta.Dinge_Size = RTB_Dinge_Size.Text;
            mta.TDM_Description = RTB_TDM_Description.Text;

            mta.MaterialsDes = RTB_MaterialsDes.Text; 
            if (Request.QueryString["SubmitType"].ToString() == "3")
            {
                if (RB_Attribute41.Checked == true)
                {
                    mta.Attribute4 = RB_Attribute41.Text;
                }
                else
                {
                    mta.Attribute4 = RB_Attribute42.Text;
                }
            }
            if (RTB_Unit_Price.Text != "")
            {
                mta.Unit_Price = Convert.ToDecimal(RTB_Unit_Price.Text); //span_Unit_Price.InnerText
            }
            if (span_Sum_Price.Text != "")
            {
                mta.Sum_Price = Convert.ToDecimal(span_Sum_Price.Text);
            }

            mta.Submit_Type = Convert.ToInt32(this.ViewState["submit_type"].ToString());//1－工艺试验件；2－技术创新课题；3－车间备料
            mta.Material_Name = RTB_Material_Name.Text.Trim();

            SaveTechnologyNoSubmit(mta);
            /*
            RTB_Material_Name.Text = "";
            RTB_Material_Mark.Text = "";
            RTB_CN_Material_State.Text = "";
            RTB_Material_Tech_Condition.Text = "";
           
            RTB_MaterialsDes.Text = "";
            RTB_TDM_Description.Text = "";
            txt_ItemCode1.Text = "";
            txt_Drawing_No.Text = "";
            txt_NumCasesSum.Text = "";
            txt_DemandNumSum.Text = "";
            RTB_Mat_Rough_Weight.Text = "";
            txt_Mat_Unit.Text = "";//span_Mat_Unit.InnerHtml = "";
            //txt_Quantity.Text = "";
            RTB_Rough_Size.Text = "";//span_Rough_Size.InnerHtml = "";
            RTB_Dinge_Size.Text = "";
            txt_Rough_Spec.Text = "";//span_Rough_Spec.InnerHtml = "";
            RTB_Unit_Price.Text = "";//span_Unit_Price.InnerHtml = "";
            span_Sum_Price.Text = "";
            rtb_SpecialNeeds.Text = "";
            DemandDate.SelectedDate = DateTime.Now;
             */
        }
        protected void SaveTechnologyNoSubmit(M_Demand_Merge_List mta)
        {
            DBI.OpenConnection();
            try
            {
                if (Session["UserId"] == null)
                {
                    throw new Exception("超时，请重新登录");
                }
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = "";
                int MDPId = 0;
                if (this.hfBh.Value != null && this.hfBh.Value != "") {
                    mta.MDPId = MDPId = Convert.ToInt32(this.hfBh.Value);
                }
                else {
                    strSQL = @"exec Proc_Add_M_Demand_Plan_List_Technology " + userid + ",'" + mta.TaskCode + "',0,0," + this.ViewState["submit_type"].ToString()+",0,''";
                    DataTable dt = DBI.Execute(strSQL, true);
                    if (dt.Rows.Count == 1)
                    {
                        mta.MDPId = MDPId = Convert.ToInt32(dt.Rows[0][0].ToString());
                        this.hfBh.Value = dt.Rows[0][0].ToString();
                    }
                }
                strSQL = @"exec Proc_Save_Technology_Apply_NoSubmit '" + mta.MDPId + "','" + mta.Drawing_No + "','" +
                         mta.TaskCode + "','" +mta.MaterialDept + "','" + mta.ItemCode1 + "','" + 
                         mta.DemandNumSum + "','" + mta.NumCasesSum +"','" +mta.Mat_Rough_Weight+"','"+
                         mta.Material_Mark + "','" + mta.CN_Material_State + "','" + mta.Material_Tech_Condition + "','" +
                         mta.TDM_Description + "','" + mta.MaterialsDes + "','" +
                         mta.Mat_Unit + "','" + mta.Quantity + "','" + mta.Rough_Size + "','" + mta.Dinge_Size + "','"+ mta.Rough_Spec + "','" +
                         mta.DemandDate + "','" + mta.Special_Needs + "','" + mta.Urgency_Degre + "','" +
                         mta.Secret_Level + "','" +
                         mta.Stage + "','" + mta.Use_Des + "','" + mta.Shipping_Address + "','" + mta.Certification +
                         "','" +
                         mta.Unit_Price + "','" + mta.Sum_Price + "','" + mta.Submit_Type + "','" + userid + "','" +
                         mta.Material_Name + "', '" + mta.Attribute4 + "','" + mta.Project + "'";
                DBI.Execute(strSQL);
                RadBtnSubmit.Visible = true;
                RadNotificationAlert.Text = "保存成功！";
                RadNotificationAlert.Show();
                GridSource = Common.AddTableRowsID(GetTechnologyTestList(mta.MDPId.ToString(), mta.Submit_Type.ToString()));
                RadGrid_TechnologyTestList.Rebind();
                Get_M_Technology_ApplyByMDPId(MDPId.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("获取工艺试验件清单信息出错" + ex.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        protected class M_Demand_Merge_List
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
            public DateTime DemandDate { get; set; }
            public string Rough_Size { get; set; }
            public string Dinge_Size { get; set; }
            public string Rough_Spec { get; set; }
            public string Mat_Rough_Weight { get; set; }
            public string MaterialsDes { get; set; }
            public string Special_Needs { get; set; }
            public string Urgency_Degre { get; set; }
            public string Secret_Level { get; set; }
            public string Stage { get; set; }
            public string Use_Des { get; set; }
            public string Shipping_Address { get; set; }
            public string Certification { get; set; }
            public decimal Unit_Price { get; set; }
            public decimal Sum_Price { get; set; }
            public bool Is_Submit { get; set; }
            public bool Is_Save { get; set; }
            public int User_ID { get; set; }
            public DateTime Submit_Date { get; set; }
            public string TaskCode { get; set; }
            public string MaterialDept { get; set; }
            public int Get_Quantity { get; set; }
            public int Submit_Type { get; set; }
            public string Material_Name { get; set; }
            public string Attribute4 { get; set; }
            public string Project { get; set; }
            public string CN_Material_State { get; set; }
            public string Material_Tech_Condition { get; set; }
            public string TDM_Description { get; set; }
            public string Material_Mark { get; set; }
        }
        protected DataTable GetTechnologyTestList(string MDPId, string Submit_Type)
        {
            try
            {
                DataTable dt=new DataTable();
                if (MDPId != "" && MDPId != null)
                {
                   // if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
                   // int userid = Convert.ToInt32(Session["UserId"].ToString());
                    //string strSQL = "select * from V_M_Technology_Apply where Submit_Type=" + Submit_Type + " and Is_Submit=0 and User_ID=" + userid + " and MDPId=" + MDPId;
                    string strSQL =
                        " select (ROW_NUMBER() OVER (ORDER BY M_Demand_Merge_List.ID)) AS rownum, MDP_Code" +
                        " , CUX_DM_URGENCY_LEVEL.DICT_Name as UrgencyDegre, CUX_DM_USAGE.DICT_Name as UseDes , GetCustInfo_T_ACCT_SITE.ADDRESS as ADDRESS" +
                        " , CUX_DM_PROJECT.DICT_Name as Model"+
                        " , case when Stage ='1' then 'M' when stage='2' then 'C' when Stage='3' then 'S' when Stage='4' then 'D' else Convert(nvarchar(50),Stage) end as Stage1"+
                        " , M_Demand_Merge_List.*" +

                        " from M_Demand_Merge_List join M_Demand_Plan_List on M_Demand_Plan_List.Id = M_Demand_Merge_List.MDPId" +
                        " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                        " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                        " left join GetBasicdata_T_Item as CUX_DM_PROJECT on CUX_DM_PROJECT.DICT_CODE = M_Demand_Merge_List.Project and CUX_DM_PROJECT.DICT_CLASS='CUX_DM_PROJECT'" +
                        " left join GetCustInfo_T_ACCT_SITE on Convert(nvarchar(50),GetCustInfo_T_ACCT_SITE.LOCATION_ID) = M_Demand_Merge_List.Shipping_Address" +
                        " where MDPId = '" + MDPId + "'";
                    
                    dt = DBI.Execute(strSQL, true);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception((Submit_Type == "1" ? "获取工艺试验件清单信息出错" : (Submit_Type == "2" ? "获取技术创新课题清单信息出错" : "获取车间备料清单信息出错")) +
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
            if (e.CommandName == "delete")
            {
                string ID = dataitem.GetDataKeyValue("ID").ToString();
                string MDPId = table.Rows[e.Item.DataSetIndex]["MDPId"].ToString();
                DeleteTechnology(MDPId, ID);
                if (GridSource.Rows.Count == 1)
                {
                    RadNotificationAlert1.Text = "记录全部删除，将退出当前页面";
                    RadNotificationAlert1.Show();
                }
                else
                {
                    GridSource = Common.AddTableRowsID(GetTechnologyTestList(MDPId, this.ViewState["submit_type"].ToString()));
                    RadGrid_TechnologyTestList.DataSource = GridSource;
                    RadGrid_TechnologyTestList.Rebind();
                }
            }
        }


        protected void DeleteTechnology(string MDPId, string ID)
        {
            DBI.OpenConnection();
            try
            {
                if (Session["UserId"] == null)
                {
                    RadNotificationAlert.Text = "登录超时，请重新登录！";
                    RadNotificationAlert.Show();
                    return;
                }
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = @"exec Proc_DeleteTechnologyNoSubmit " + MDPId + "," + ID + "," + userid;
                DBI.Execute(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("删除信息时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        protected void RadBtnSubmit_Click(object sender, EventArgs e)
        {
            if (this.hfBh.Value != null && this.hfBh.Value != "")
            {
                string approveAccount1 = RDDL_ApproveAccount1.SelectedValue;
                string approveAccount2 = RDDL_ApproveAccount2.SelectedValue;

                if (approveAccount1 == "")
                {
                    RadNotificationAlert.Text = "失败！没有选择" + lbl_ApproveAccount1.Text;
                    RadNotificationAlert.Show();
                    return;
                }
                if (approveAccount2 == "")
                {
                    RadNotificationAlert.Text = "失败！没有选择" + lbl_ApproveAccount2.Text;
                    RadNotificationAlert.Show();
                    return;
                }
                string approveAccount3 = null;
                if (Request.QueryString["SubmitType"].ToString() == "3")
                {
                    approveAccount3 = RDDL_ApproveAccount3.SelectedValue;
                    if (approveAccount3 == "")
                    {
                        RadNotificationAlert.Text = "失败！没有选择物资综合计划员";
                        RadNotificationAlert.Show();
                        return;
                    }
                }
                string strSql = " Update M_Demand_Plan_List set DeptApproveAccount = '" + approveAccount1 + "', PlanOrTecApproveAccount = '" + approveAccount2 + "', MaterialPlanApproveAccount = '" + approveAccount3 + "' where ID = '" + hfBh.Value.ToString() + "'";
                DBI.Execute(strSql);

                //ModifyTechnologySubmit(this.hfBh.Value);
                // WriteReqOrderRepeat(hfBh.Value);
                //Response.Redirect("/Plan/MDemandMergeListState.aspx?MDPID=" + hfBh.Value);

                K2PreBLL k2prebll = new K2PreBLL();
                try
                {
                    Convert.ToInt32(hfBh.Value);
                    var result = k2prebll.k2StartPreparesProgress(hfBh.Value.ToString());

                    if (result == "")
                    {
                      //  RadNotificationAlert.Text = "进入流程平台！";
                        //RadNotificationAlert.Show();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow1();", true);
                        RadNotificationAlert1.Text = "申请成功！进入流程平台";
                        RadNotificationAlert1.Show();
                    }
                    else {
                        RadNotificationAlert.Text = result;
                        RadNotificationAlert.Show();
                    }
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
        }
        protected void ModifyTechnologySubmit(string MDPId)
        {
            DBI.OpenConnection();
            try
            {
                if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = @"exec Proc_ModifyTechnologySubmit " + MDPId + "," + userid;
                DBI.Execute(strSQL);
            }
            catch (Exception e)
            {
                throw new Exception("更新信息时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        private void BindDeptUserAddress()
        {
            if (Session["UserId"] == null)
            {
                RadNotificationAlert.Text = "登录超时，请重新登录！";
                RadNotificationAlert.Show();
                return;
            }
            string userid = Session["UserId"].ToString();
            string strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                            " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                            " where Sys_UserInfo_PWD.ID = '" + userid + "'";
            DataTable dt = DBI.Execute(strSQL, true);

            RadComboBox_Dept.DataSource = dt;
            RadComboBox_Dept.DataTextField = "Dept";
            RadComboBox_Dept.DataValueField = "DeptCode";
            RadComboBox_Dept.DataBind();

            RadComboBoxMaterialDept1.DataSource = dt;
            RadComboBoxMaterialDept1.DataTextField = "Dept";
            RadComboBoxMaterialDept1.DataValueField = "DeptCode";
            RadComboBoxMaterialDept1.DataBind();

            RadComboBox_User.DataSource = dt;
            RadComboBox_User.DataTextField = "UserName";
            RadComboBox_User.DataValueField = "UserID";
            RadComboBox_User.DataBind();

            strSQL = "select KeyWord from Sys_Dict" +
                " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
                " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + dt.Rows[0]["DeptCode"].ToString() + "')";
            DataTable dtAddress = DBI.Execute(strSQL, true);
            RadComboBoxShipping_Address.DataSource = dtAddress;
            RadComboBoxShipping_Address.DataTextField = "KeyWord";
            RadComboBoxShipping_Address.DataValueField = "KeyWord";
            RadComboBoxShipping_Address.DataBind();

           
        }

      
        protected void RadComboBox_Dept_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (RadComboBox_Dept.SelectedValue != "0") {
                RadComboBox_User.ClearSelection();
                string strSQL = "SELECT * FROM [Sys_UserInfo_PWD] where Dept='" + RadComboBox_Dept.SelectedValue + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                RadComboBox_User.DataSource = dt;
                RadComboBox_User.DataTextField = "UserName";
                RadComboBox_User.DataValueField = "Id";
                RadComboBox_User.DataBind();
            }
        }
    
    
        #region 提交辅料需求清单
        public void WriteReqOrderRepeat(string MDPLID)
        {
            //取数据库
            var db = new MMSDbDataContext();
            var client = new Write_Req_Order.CUX_DM_REQ_SYNC_WS_PKG_PortTypeClient();
            var header = new Write_Req_Order.SOAHeader
            {
                Responsibility = "CUX_SOA_ACCESS_RESP",
                RespApplication = "CUX",
                SecurityGroup = "STANDARD",
                NLSLanguage = "AMERICAN",
                Org_Id = "81"
            };

            client.ClientCredentials.UserName.UserName = "SOA_COMMON";
            client.ClientCredentials.UserName.Password = "111111";

            var tempArray1 = (from p in db.WriteReqOrder_T_List
                              where p.USER_RQ_ID == Convert.ToDecimal(MDPLID)
                              orderby p.ID
                              select p).ToArray();

            var j = Math.Ceiling(Convert.ToDouble(tempArray1.Length) / Convert.ToDouble(200));
            for (int n = 0; n < j; n++)
            {
                string strSQL = " Insert into WriteReqOrder_Sent (System_Code, Total_Flag, Row_Count,Group_ID) values ('TJ-WZ','" + ((n == (j - 1)) ? "Y" : "N") + "','" + ((n == (j - 1)) ? (tempArray1.Length - (n * 200)) : 200) + "', (select isnull(max(Group_ID),0) + 1 from WriteReqOrder_Sent))";
                DBI.Execute(strSQL);

                var qSent = (from p in db.WriteReqOrder_Sent
                             orderby p.ID descending
                             select p).Take(1).ToList();

                var sentSum23 = new Write_Req_Order.APPSCUX_DM_REQ_SYNC_WS_PKG_SUM_23
                {
                    SYSTEM_CODE = qSent[0].SYSTEM_CODE,
                    TOTAL_FLAG = qSent[0].TOTAL_FLAG,
                    ROW_COUNT = qSent[0].ROW_COUNT,
                    ROW_COUNTSpecified = true,
                    GROUP_ID = qSent[0].GROUP_ID,
                    GROUP_IDSpecified = true
                };

                var sentList26 = new Write_Req_Order.APPSCUX_DM_REQ_SYNC_WS_PKG_RECO26[((n == (j - 1)) ? (tempArray1.Length - (n * 200)) : 200)];

                for (var i = n * 200; i < (n + 1) * 200 && i < tempArray1.Length; i++)
                {
                    sentList26[i] = new Write_Req_Order.APPSCUX_DM_REQ_SYNC_WS_PKG_RECO26
                    {
                        ORG_ID = tempArray1[i].ORG_ID,
                        ORG_IDSpecified = true,
                        RQ_NUMBER = "", //留空
                        USER_RQ_NUMBER = tempArray1[i].USER_RQ_NUMBER,
                        USER_RQ_ID = tempArray1[i].USER_RQ_ID,
                        USER_RQ_IDSpecified = true,
                        USER_RQ_LINE_ID = tempArray1[i].USER_RQ_LINE_ID,
                        USER_RQ_LINE_IDSpecified = true,
                        RQ_TYPE = tempArray1[i].RQ_TYPE,
                        ITEM_CODE = tempArray1[i].ITEM_CODE,
                        SPECIAL_REQUEST = tempArray1[i].SPECIAL_REQUEST,
                        QUANTITY = tempArray1[i].QUANTITY,
                        QUANTITYSpecified = tempArray1[i].QUANTITY != null,
                        PIECE = tempArray1[i].PIECE,
                        PIECESpecified = tempArray1[i].PIECE != null,
                        DIMENSION = tempArray1[i].DIMENSION,
                        MANUFACTURER = tempArray1[i].MANUFACTURER ?? "", //字段待确认
                        RQ_DATE = tempArray1[i].RQ_DATE,
                        RQ_DATESpecified = true,
                        URGENCY_LEVEL = tempArray1[i].URGENCY_LEVEL,
                        REQUESTER = tempArray1[i].REQUESTER,
                        REQUESTER_PHONE = tempArray1[i].REQUESTER_PHONE ?? "",
                        ITEM_REVISION = tempArray1[i].ITEM_REVISION,
                        USER_ITEM_DESCRIPTION = tempArray1[i].USER_ITEM_DESCRIPTION ?? "",
                        UNANIMOUS_BATCH = tempArray1[i].UNANIMOUS_BATCH ?? "",
                        SECURITY_LEVEL = tempArray1[i].SECURITY_LEVEL ?? "",
                        PROJECT = tempArray1[i].PROJECT,
                        PHASE = tempArray1[i].PHASE ?? "",
                        BATCH = tempArray1[i].BATCH ?? "",
                        BATCH_QTY = tempArray1[i].BATCH_QTY ?? 1, //字段待确认
                        BATCH_QTYSpecified = true,
                        USAGE = tempArray1[i].USAGE ?? "",
                        TASK = tempArray1[i].TASK ?? "",
                        SUBJECT = tempArray1[i].SUBJECT ?? "",
                        CUSTOMER_NAME = tempArray1[i].CUSTOMER_NAME ?? "",
                        CUSTOMER_ACCOUNT_NUMBER = tempArray1[i].CUSTOMER_ACCOUNT_NUMBER ?? "",
                        DELIVERY_ADDRESS = tempArray1[i].DELIVERY_ADDRESS ?? "",
                        PREPARER = tempArray1[i].PREPARER,
                        SUBMITED_BY = tempArray1[i].SUBMITED_BY,
                        SUBMITED_BYSpecified = tempArray1[i].SUBMITED_BY != null,
                        SUBMISSION_DATE = tempArray1[i].SUBMISSION_DATE,
                        SUBMISSION_DATESpecified = true,
                        SYSTEM_CODE = tempArray1[i].SYSTEM_CODE,
                        TRANSFER_TIME = tempArray1[i].TRANSFER_TIME,
                        TRANSFER_TIMESpecified = tempArray1[i].TRANSFER_TIME != null,
                        CUSTOMER_OF_CREATOR = tempArray1[i].CUSTOMER_OF_CREATOR ?? "",
                        GROUP_ID = qSent[0].GROUP_ID,
                        GROUP_IDSpecified = true,
                        CREATION_DATE = tempArray1[i].CREATION_DATE,
                        CREATION_DATESpecified = tempArray1[i].CREATION_DATE != null,
                        CREATED_BY = tempArray1[i].CREATED_BY,
                        CREATED_BYSpecified = tempArray1[i].CREATED_BY != null,
                        ATTRIBUTE_CATEGORY = tempArray1[i].ATTRIBUTE_CATEGORY ?? "",
                        ATTRIBUTE1 = tempArray1[i].ATTRIBUTE1 ?? "",
                        ATTRIBUTE2 = tempArray1[i].ATTRIBUTE2 ?? "",
                        ATTRIBUTE3 = tempArray1[i].ATTRIBUTE3 ?? "",
                        ATTRIBUTE4 = tempArray1[i].ATTRIBUTE4 ?? "",
                        ATTRIBUTE5 = tempArray1[i].ATTRIBUTE5 ?? "",
                        ATTRIBUTE6 = tempArray1[i].ATTRIBUTE6 ?? "",
                        ATTRIBUTE7 = tempArray1[i].ATTRIBUTE7 ?? "",
                        ATTRIBUTE8 = tempArray1[i].ATTRIBUTE8 ?? "",
                        ATTRIBUTE9 = tempArray1[i].ATTRIBUTE9 ?? "",
                        ATTRIBUTE10 = tempArray1[i].ATTRIBUTE10 ?? "",
                        ATTRIBUTE11 = tempArray1[i].ATTRIBUTE11 ?? "",
                        ATTRIBUTE12 = tempArray1[i].ATTRIBUTE12 ?? "",
                        ATTRIBUTE13 = tempArray1[i].ATTRIBUTE13 ?? "",
                        ATTRIBUTE14 = tempArray1[i].ATTRIBUTE14 ?? "",
                        ATTRIBUTE15 = tempArray1[i].ATTRIBUTE15 ?? ""
                    };
                }

                var input = new Write_Req_Order.InputParameters
                {
                    P_SUM_RECORD_INPUT = sentSum23,
                    P_RECORD_INPUT = sentList26
                };

                var returnSyncData = client.WRITE_REQ_ORDER(header, input);
                //提交物流中心不成功记录错误信息
                if (returnSyncData.X_RECORD_RESULT.STATUS == "E")
                {
                    var rec = new WriteReqOrder_Rec()
                    {
                        STATUS = returnSyncData.X_RECORD_RESULT.STATUS,
                        ERR_MSG = returnSyncData.X_RECORD_RESULT.ERR_MSG
                    };
                    db.WriteReqOrder_Rec.InsertOnSubmit(rec);
                    db.SubmitChanges();
                    var recid = rec.ID;

                    foreach (var list in returnSyncData.X_ERROR_RECORD)
                    {
                        var reclist = new WriteReqOrder_RecList()
                        {
                            REC_SUM_ID = recid,
                            ORG_ID = list.ORG_ID,
                            SYSTEM_CODE = list.SYSTEM_CODE,
                            USER_RQ_LINE_ID = list.USER_RQ_LINE_ID,
                            USER_RQ_ID = list.USER_RQ_ID,
                            ERR_MSG = list.ERR_MSG
                        };
                        db.WriteReqOrder_RecList.InsertOnSubmit(reclist);
                        var mdml = db.M_Demand_Merge_List.SingleOrDefault(p => Convert.ToDecimal(p.ID) == reclist.USER_RQ_LINE_ID);
                        mdml.Is_Submit = false;
                    }
                    db.SubmitChanges();
                }
                
                //GetErrInfStart();
               
            }
        }

        public void GetErrInfStart()
        {
            var dcQuery = new MMSDbDataContext();

            //从Sent表中取得systemCode,maxUpdateDate和lastMaxRowNum数据
            var query2 = (from p in dcQuery.GetErrInf_Sent
                          orderby p.ID descending
                          select new
                          {
                              systemCode = p.SYSTEM_CODE,
                              maxUpdateDate = p.MAX_UPDATE_DATE,
                              lastMaxRowNum = p.LAST_MAX_ROWNUMBER
                          }).ToList();


            var lastMaxRowNum = query2.Count > 0 ? query2[0].lastMaxRowNum ?? 0 : 0;
            var systemCode = query2.Count > 0 ? query2[0].systemCode : "TJ-WZ";
            var maxUpDateDate = query2.Count > 0 ? query2[0].maxUpdateDate : "2000-01-01 10:41:00";

            //执行取回数据函数
            GetErrInfRepeat(systemCode, maxUpDateDate, lastMaxRowNum);

            //从Rec表中取得WS返回的MaxRownumber数据，从CUST表中取得lastUpdateDate数据，并将其传入Sent表
            var query1 = (from p in dcQuery.GetErrInf_Rec
                          orderby p.ID descending
                          select new
                          {
                              maxRowNumber = p.MAX_ROWNUMBER
                          }).ToList();

            var query3 = (from p in dcQuery.GetErrInf_T_Item
                          select new
                          {
                              lastUpdateDate = p.LAST_UPDATE_DATE
                          }).Distinct().OrderByDescending(x => x.lastUpdateDate).ToList();

            var renewMaxRowNum = query1[0].maxRowNumber;
            var renewUpdateDate = query3.Count > 0 ? query3[0].lastUpdateDate : "2000-01-01 10:10:10";

            //更新Sent表中的lastUpdateDate和lastMaxRowNum数据
            var renewErrSent = new GetErrInf_Sent
            {
                SYSTEM_CODE = "TJ-WZ",
                LAST_MAX_ROWNUMBER = renewMaxRowNum,
                MAX_UPDATE_DATE = renewUpdateDate
            };
            dcQuery.GetErrInf_Sent.InsertOnSubmit(renewErrSent);
            try
            {
                dcQuery.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GetErrInfRepeat(string systemCode, string maxUpdateDate, decimal lastMaxRowNum)
        {
            var client =
                new Get_Err_Inf.CUX_DM_ERR_RELEASE_WS_PKG_PortTypeClient();
            var header = new Get_Err_Inf.SOAHeader
            {
                Responsibility = "CUX_SOA_ACCESS_RESP",
                RespApplication = "CUX",
                SecurityGroup = "STANDARD",
                NLSLanguage = "AMERICAN",
                Org_Id = "81"
            };
            client.ClientCredentials.UserName.UserName = "SOA_COMMON";
            client.ClientCredentials.UserName.Password = "111111";

            var reco63 = new Get_Err_Inf.APPSCUX_DM_ERR_RELEASE_WS_PKG_RE2
            {
                SYSTEM_CODE = systemCode,
                MAX_UPDATE_DATE = maxUpdateDate,
                LAST_MAX_ROWNUMBER = lastMaxRowNum,
                LAST_MAX_ROWNUMBERSpecified = true
            };

            var inputParameters = new Get_Err_Inf.InputParameters
            {
                P_RECORD_INPUT = reco63
            };

            var returnData = client.GET_ERR_INF(header, inputParameters);
            var dcUpdate = new MMSDbDataContext();
            var maxRowNumberQuery = (from p in dcUpdate.GetErrInf_Rec
                                     where p.MAX_ROWNUMBER != null
                                     orderby p.MAX_ROWNUMBER descending
                                     select new
                                     {
                                         lastRowNo = p.MAX_ROWNUMBER
                                     }).ToList();
            //将数据插入Rec表中
            var custRecord = returnData.X_RECORD_SUM;
            var j = new GetErrInf_Rec
            {
                STATUS = custRecord.STATUS,
                TOTAL_FLAG = custRecord.TOTAL_FLAG,
                ROW_ACCOUT = custRecord.ROW_COUNT,
                MAX_ROWNUMBER = custRecord.MAX_ROWNUMBER ?? maxRowNumberQuery[0].lastRowNo,
                ERR_MSG = custRecord.ERR_MSG,
                ATTRIBUTE1 = custRecord.ATTRIBUTE1,
                ATTRIBUTE2 = custRecord.ATTRIBUTE2,
                ATTRIBUTE3 = custRecord.ATTRIBUTE3,
                ATTRIBUTE4 = custRecord.ATTRIBUTE4,
                ATTRIBUTE5 = custRecord.ATTRIBUTE5,
                ATTRIBUTE6 = custRecord.ATTRIBUTE6,
                ATTRIBUTE7 = custRecord.ATTRIBUTE7,
                ATTRIBUTE8 = custRecord.ATTRIBUTE8,
                ATTRIBUTE9 = custRecord.ATTRIBUTE9,
                ATTRIBUTE10 = custRecord.ATTRIBUTE10
            };
            dcUpdate.GetErrInf_Rec.InsertOnSubmit(j);
            try
            {
                dcUpdate.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            //将数据插入Table表中
            var queryRecRowNum = (from p in dcUpdate.GetErrInf_Rec
                                  orderby p.ID descending
                                  select p).ToList();
            var tempRowNum = queryRecRowNum[0].ID;

            foreach (var custListItem in returnData.X_TABLE_ERR_INF)
            {
                var i = new GetErrInf_T_Item
                {
                    REC_ID = tempRowNum,
                    DATA_CLASS = custListItem.DATA_CLASS,
                    SOURCE_HEADER_ID = custListItem.SOURCE_HEADER_ID,
                    SOURCE_LINE_ID = custListItem.SOURCE_LINE_ID,
                    SOURCE_HEADER_NUMBER = custListItem.SOURCE_HEADER_NUMBER,
                    ERROR_MSG = custListItem.ERROR_MSG,
                    SYSTEM_CODE = custListItem.SYSTEM_CODE,
                    LAST_UPDATE_DATE = custListItem.LAST_UPDATE_DATE,
                    ATTRIBUTE_CATEGORY = custListItem.ATTRIBUTE_CATEGORY,
                    ATTRIBUTE1 = custListItem.ATTRIBUTE1,
                    ATTRIBUTE2 = custListItem.ATTRIBUTE2,
                    ATTRIBUTE3 = custListItem.ATTRIBUTE3,
                    ATTRIBUTE4 = custListItem.ATTRIBUTE4,
                    ATTRIBUTE5 = custListItem.ATTRIBUTE5,
                    ATTRIBUTE6 = custListItem.ATTRIBUTE6,
                    ATTRIBUTE7 = custListItem.ATTRIBUTE7,
                    ATTRIBUTE8 = custListItem.ATTRIBUTE8,
                    ATTRIBUTE9 = custListItem.ATTRIBUTE9,
                    ATTRIBUTE10 = custListItem.ATTRIBUTE10,
                    ATTRIBUTE11 = custListItem.ATTRIBUTE11,
                    ATTRIBUTE12 = custListItem.ATTRIBUTE12,
                    ATTRIBUTE13 = custListItem.ATTRIBUTE13,
                    ATTRIBUTE14 = custListItem.ATTRIBUTE14,
                    ATTRIBUTE15 = custListItem.ATTRIBUTE15
                };
                var q = (from p in dcUpdate.GetErrInf_T_Item
                         where p.SOURCE_HEADER_ID == i.SOURCE_HEADER_ID && p.SOURCE_LINE_ID == i.SOURCE_LINE_ID
                         orderby p.ID
                         select p).Take(1).ToList();
                if (q.Count == 0)
                {
                    dcUpdate.GetErrInf_T_Item.InsertOnSubmit(i);
                }
                else
                {
                    //使用ExeInfoRepo类更新T_Items表
                    var errrepo = new ErrInfoRepo();
                    var errinf = errrepo.GetErrInf((decimal)custListItem.SOURCE_HEADER_ID,
                        (decimal)custListItem.SOURCE_LINE_ID);
                    errinf.DATA_CLASS = custListItem.DATA_CLASS;
                    errinf.SOURCE_HEADER_ID = custListItem.SOURCE_HEADER_ID;
                    errinf.SOURCE_LINE_ID = custListItem.SOURCE_LINE_ID;
                    errinf.SOURCE_HEADER_NUMBER = custListItem.SOURCE_HEADER_NUMBER;
                    errinf.ERROR_MSG = custListItem.ERROR_MSG;
                    errinf.SYSTEM_CODE = custListItem.SYSTEM_CODE;
                    errinf.LAST_UPDATE_DATE = custListItem.LAST_UPDATE_DATE;
                    errinf.ATTRIBUTE_CATEGORY = custListItem.ATTRIBUTE_CATEGORY;
                    errinf.ATTRIBUTE1 = custListItem.ATTRIBUTE1;
                    errinf.ATTRIBUTE2 = custListItem.ATTRIBUTE2;
                    errinf.ATTRIBUTE3 = custListItem.ATTRIBUTE3;
                    errinf.ATTRIBUTE4 = custListItem.ATTRIBUTE4;
                    errinf.ATTRIBUTE5 = custListItem.ATTRIBUTE5;
                    errinf.ATTRIBUTE6 = custListItem.ATTRIBUTE6;
                    errinf.ATTRIBUTE7 = custListItem.ATTRIBUTE7;
                    errinf.ATTRIBUTE8 = custListItem.ATTRIBUTE8;
                    errinf.ATTRIBUTE9 = custListItem.ATTRIBUTE9;
                    errinf.ATTRIBUTE10 = custListItem.ATTRIBUTE10;
                    errinf.ATTRIBUTE11 = custListItem.ATTRIBUTE11;
                    errinf.ATTRIBUTE12 = custListItem.ATTRIBUTE12;
                    errinf.ATTRIBUTE13 = custListItem.ATTRIBUTE13;
                    errinf.ATTRIBUTE14 = custListItem.ATTRIBUTE14;
                    errinf.ATTRIBUTE15 = custListItem.ATTRIBUTE15;

                    errrepo.UpdateErrInfT(errinf);
                }
                try
                {
                    dcUpdate.SubmitChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #endregion

     

        #region 查询物资编码信息
        protected void confirmWindowClick(object sender, EventArgs e)
        {
           
            GridDataItem[] dataItems = RadGrid1.MasterTableView.GetSelectedItems();
            if (dataItems.Count() > 0)
            {
                txt_ItemCode1.Text = dataItems[0]["SEG3"].Text;
                txt_ItemCode1_OnTextChanged(sender, e);
            }

        }
        protected void txt_ItemCode1_OnTextChanged(object sender, EventArgs e)
        {
            string ItemCode1 = txt_ItemCode1.Text.Trim();
            lblMSG.Text = "";

            string strSQL = " select * from GetCommItem_T_Item where seg3 = '" + ItemCode1 + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                {
                    lblMSG.Text = "已失效！";
                }
                string Seg5 = dt.Rows[0]["Seg5"].ToString().Substring(0, 4);
                RTB_MaterialsDes.Text = dt.Rows[0]["SEG4"].ToString();
                RTB_Material_Mark.Text = dt.Rows[0]["SEG13"].ToString();

                switch (Seg5)
                {
                    case "YY01":
                        //RTB_Material_Mark.Text = "";
                        RTB_Material_Name.Text = dt.Rows[0]["Seg21"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                     
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg13"].ToString();
                        RTB_Unit_Price.Text = dt.Rows[0]["Seg31"].ToString();
                        break;
                    case "YY02":
                        //RTB_Material_Mark.Text = "";
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
            
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg15"].ToString();
                        RTB_Unit_Price.Text = dt.Rows[0]["Seg25"].ToString();
                        break;
                    case "YY03":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Dinge_Size.Text = dt.Rows[0]["Seg16"].ToString();
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg15"].ToString();
                        break;
                    case "YY04":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                    
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Unit_Price.Text = dt.Rows[0]["Seg23"].ToString();
                        break;
                    case "YY05":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                     
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Unit_Price.Text = dt.Rows[0]["Seg23"].ToString();
                        RTB_Material_Tech_Condition.Text = dt.Rows[0]["SEG16"].ToString();
                        break;
                    case "YY06":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                       
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Unit_Price.Text = dt.Rows[0]["Seg23"].ToString();
                        break;
                    case "YY07":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg20"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                      
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Unit_Price.Text = dt.Rows[0]["Seg25"].ToString();
                        break;
                    case "YY08":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                       
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Unit_Price.Text = dt.Rows[0]["Seg21"].ToString();
                        break;
                    case "YY09":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        txt_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        
                        txt_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Unit_Price.Text = dt.Rows[0]["Seg21"].ToString();
                   
                        break;
                    default:
                        break;
                }
                try
                {

                    if (txt_DemandNumSum.Text != "" && RTB_Unit_Price.Text!="")
                    {
                        Convert.ToDouble(RTB_Unit_Price.Text);
                        Convert.ToDouble(txt_DemandNumSum.Text);
                        span_Sum_Price.Text = (Convert.ToDouble(RTB_Unit_Price.Text) * Convert.ToDouble(txt_DemandNumSum.Text)).ToString();
                    }
                }
                catch
                { 
                    span_Sum_Price.Text = "0";
                    RTB_Unit_Price.Text = "0";
                }
            }
            else
            {
                lblMSG.Text = "不存在";
            }
        }

        protected void RDDLMT_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
         /*   string value = RDDLMT.SelectedValue.ToString();
            string prefix = RDDLMT.SelectedText.ToString() + ".";

            RDDLMT1.SelectedIndex = 0;
            RDDLMT2.SelectedIndex = 0;
            RDDLMT3.SelectedIndex = 0;
            RDDLMT4.SelectedIndex = 0;

            RDDLMT1.Items.Clear();
            RDDLMT2.Items.Clear();
            RDDLMT3.Items.Clear();
            RDDLMT4.Items.Clear();

            if (value == "ItemCode")
            {
                div1.Visible = false;
                div2.Visible = true;
            }
            else if (value != "")
            {
                DataTable dt = GetSeg6(prefix, "2");
                RDDLMT1.DataSource = dt;
                RDDLMT1.DataTextField = "Seg6";
                RDDLMT1.DataValueField = "RowsId";
                RDDLMT1.DataBind();

                div1.Visible = true;
                div2.Visible = false;
            }
            else
            {
                div1.Visible = false;
                div2.Visible = false;
            }*/
        }
        /*
        protected void RDDLMT1_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string prefix = RDDLMT.SelectedText.ToString() + "." + RDDLMT1.SelectedText.ToString() + ".";
            string value = RDDLMT1.SelectedValue.ToString();
            RDDLMT2.SelectedIndex = 0;
            RDDLMT3.SelectedIndex = 0;
            RDDLMT4.SelectedIndex = 0;
            RDDLMT2.Items.Clear();
            RDDLMT3.Items.Clear();
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                DataTable dt = GetSeg6(prefix, "3");
                RDDLMT2.DataSource = dt;
                RDDLMT2.DataTextField = "Seg6";
                RDDLMT2.DataValueField = "RowsId";
                RDDLMT2.DataBind();
            }
        }

        protected void RDDLMT2_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string prefix = RDDLMT.SelectedText.ToString() + "." + RDDLMT1.SelectedText.ToString() + "." + RDDLMT2.SelectedText.ToString() + ".";
            string value = RDDLMT2.SelectedValue.ToString();

            RDDLMT3.SelectedIndex = 0;
            RDDLMT4.SelectedIndex = 0;
            RDDLMT3.Items.Clear();
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                DataTable dt = GetSeg6(prefix, "4");
                RDDLMT3.DataSource = dt;
                RDDLMT3.DataTextField = "Seg6";
                RDDLMT3.DataValueField = "RowsId";
                RDDLMT3.DataBind();
            }
        }

        protected void RDDLMT3_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string prefix = RDDLMT.SelectedText.ToString() + "." + RDDLMT1.SelectedText.ToString() + "." + RDDLMT2.SelectedText.ToString() + "." + RDDLMT3.SelectedText.ToString() + ".";
            string value = RDDLMT2.SelectedValue.ToString();
  
            RDDLMT4.SelectedIndex = 0;
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                DataTable dt = GetSeg6(prefix, "5");
                RDDLMT4.DataSource = dt;
                RDDLMT4.DataTextField = "Seg6";
                RDDLMT4.DataValueField = "RowsId";
                RDDLMT4.DataBind();
            }
        }
*/
        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string strSQL = "select * from GetCommItem_T_Item where SEG10 = 'N'";
            string Material_Name = RTB_MaterialName.Text.Trim();
            string Material_Paihao = RTB_MaterialPaihao.Text.Trim();
            string Material_Guige = RTB_MaterialGuige.Text.Trim();
            string Material_Biaozhun = RTB_MaterialBiaozhun.Text.Trim();
            if (Material_Name != "")
            {
                strSQL += " and SEG4 like '%" + Material_Name + "%'";
            }
            if (Material_Paihao != "")
            {
                strSQL += " and SEG4 like '%牌号(" + Material_Paihao + "%'";
            }
            if (Material_Guige != "")
            {
                strSQL += " and SEG4 like '%规格(" + Material_Guige + "%'";
            }
            if (Material_Biaozhun != "")
            {
                strSQL += " and SEG4 like '%采用标准(%" + Material_Biaozhun + "%'";
            }
            string MTv = RDDLMT.SelectedValue.ToString();
            if (MTv == "")
            {

            }
           /* else if (MTv == "ItemCode")
            {
                string ItemCode = RTB_ItemCode.Text.Trim();
                strSQL += " and SEG3 like '%" + ItemCode + "%'"; ;
            }*/
            else
            {
                string MT = RDDLMT.SelectedText.ToString();
              //  string MT1 = RDDLMT1.SelectedText.ToString();
             //   string MT2 = RDDLMT2.SelectedText.ToString();
              //  string MT3 = RDDLMT3.SelectedText.ToString();
              //  string MT4 = RDDLMT4.SelectedText.ToString();

                string SEG6 = "";
                if (MT != "") { SEG6 += MT; }
            //    if (MT1 != "") { SEG6 += "." + MT1; }
            //    if (MT2 != "") { SEG6 += "." + MT2; }
              //  if (MT3 != "") { SEG6 += "." + MT3; }
               // if (MT4 != "") { SEG6 += "." + MT4; }
                strSQL += " and SEG6 like '" + SEG6 + "%'";
            }
            Session["gds"] = DBI.Execute(strSQL, true);
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = (Session["gds"] as DataTable);
        }

        private DataTable GetSeg6(string prefix, string level)
        {
            string strSQL = " (select '' as Seg6) union (select dbo.Get_StrArrayStrOfIndex(Seg6,'.'," + level + ") as Seg6"
                            + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%') order by Seg6";
            DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
            return dt;
        }

        #endregion

        #region 从EXCEL文件导入辅料需求信息

        protected void RadGrid_Importlist_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
              /*  string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                if (Session["UserId"] == null)
                {
                    RadNotificationAlert.Text = "登录超时，请重新登录！";
                    RadNotificationAlert.Show();
                    return;
                }
                string userid = Session["UserId"].ToString();
                string strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                                " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                                " where Sys_UserInfo_PWD.ID = '" + userid + "'";
                DataTable dt = DBI.Execute(strSQL, true);

                RadComboBox RadComboBoxMaterialDept = e.Item.FindControl("RadComboBoxMaterialDept") as RadComboBox;

                RadComboBoxMaterialDept.DataSource = dt;
                RadComboBoxMaterialDept.DataTextField = "Dept";
                RadComboBoxMaterialDept.DataValueField = "DeptCode";
                RadComboBoxMaterialDept.DataBind();

                RadComboBox RadComboBoxShipping_Address = e.Item.FindControl("RadComboBoxShippingAddress") as RadComboBox;
                RadComboBoxShipping_Address.CssClass = id;
                string departCode="";
                departCode=dt.Rows[0]["DeptCode"].ToString();
           
         
                strSQL = "select KeyWord from Sys_Dict" +
             " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
             " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + departCode + "')";
                DataTable dtAddress = DBI.Execute(strSQL, true);
                RadComboBoxShipping_Address.DataSource = dtAddress;
                RadComboBoxShipping_Address.DataTextField = "KeyWord";
                RadComboBoxShipping_Address.DataValueField = "KeyWord";
                RadComboBoxShipping_Address.DataBind();

               
                RadComboBoxShipping_Address.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Shipping_Address"].ToString()).Selected = true;
               
          

                 RadComboBox RadComboBoxSecretLevel = e.Item.FindControl("RadComboBoxSecretLevel") as RadComboBox;
                 RadComboBoxSecretLevel.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Secret_Level"].ToString()).Selected = true;
                // RadComboBoxSecretLevel.FindItemByText("内部").Selected = true;
                 

                 RadComboBox RadComboBoxCertification = e.Item.FindControl("RadComboBoxCertification") as RadComboBox;
                 RadComboBoxCertification.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Certification"].ToString()).Selected = true;
             
                 RadDropDownList RDDL_Project = e.Item.FindControl("RDDL_Project1") as RadDropDownList;

                 RDDL_Project.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Project"].ToString()).Selected = true;
              
                 RadComboBox RadComboBoxStage = e.Item.FindControl("RadComboBoxStage") as RadComboBox;
                 RadComboBoxStage.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["stage"].ToString()).Selected = true;

                 RadComboBox RadComboBoxAttribute4 = e.Item.FindControl("RadComboBoxAttribute4") as RadComboBox;
                 RadComboBoxAttribute4.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Attribute4"].ToString()).Selected = true;

                 RadComboBox RadComboBoxUrgencyDegree = e.Item.FindControl("RadComboBoxUrgencyDegree") as RadComboBox;
                 RadComboBoxUrgencyDegree.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Urgency_Degre"].ToString()).Selected = true;
             //    RadComboBoxUrgencyDegree.FindItemByText("一般").Selected = true;
                 RadComboBox RadComboBoxUseDes = e.Item.FindControl("RadComboBoxUseDes") as RadComboBox;
                 RadComboBoxUseDes.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["Use_Des"].ToString()).Selected = true;
         
               */
            }
        }

        protected void RadGridImport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridImport.DataSource = GridSource1;
        }

     
        protected void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            HFGridItemsCount.Value = "0";
            if (RadAsyncUpload1.UploadedFiles.Count == 1)
            {
                //导入文件存在服务器上
                string filderPath = Server.MapPath(@"~\Plan\ImportExcel\");
                if (!System.IO.Directory.Exists(filderPath))
                {
                    System.IO.Directory.CreateDirectory(filderPath);
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") +
                                  RadAsyncUpload1.UploadedFiles[0].FileName;
                string filePath = Path.Combine(filderPath, fileName);
                RadAsyncUpload1.UploadedFiles[0].SaveAs(filePath);

                HFFileName.Value = fileName;

                string conn = " Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath(@"~\Plan\ImportExcel\") +
                              "\\" + fileName + "; Extended Properties ='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                OleDbConnection thisconnection = new OleDbConnection(conn);

                try
                {
                    if (fileName != "")
                    {
                        thisconnection.Open();

                        string sql = " select * from [Sheet1$]";
                        OleDbDataAdapter command = new OleDbDataAdapter(sql, thisconnection);
                        DataSet ds = new DataSet();
                        command.Fill(ds, "[Sheet1$]");

                        thisconnection.Close();

                        GridSource1 = ds.Tables[0];
                        int columnscount = 0;
                        for (int i = 0; i < GridSource1.Columns.Count; i++)
                        {
                            switch (GridSource1.Columns[i].ColumnName.Trim())
                            {

                                case "型号工程":
                                    GridSource1.Columns[i].ColumnName = "Project";
                                    columnscount++;
                                    break;

                               
                                case "产品图号":
                                    GridSource1.Columns[i].ColumnName = "DRAWING_NO";
                                    columnscount++;
                                    break;
                                case "任务编号":
                                    GridSource1.Columns[i].ColumnName = "TaskCode";
                                    columnscount++;
                                    break;

                                case "产品名称":
                                    GridSource1.Columns[i].ColumnName = "TDM_Description";
                                    columnscount++;
                                    break;

                                case "物资名称":
                                    GridSource1.Columns[i].ColumnName = "Material_Name";
                                    columnscount++;
                                    break;
                                   case "物资牌号":
                                       GridSource1.Columns[i].ColumnName = "Material_Mark";
                                       columnscount++;
                                       break;

                                   case "供应状态":
                                       GridSource1.Columns[i].ColumnName = "CN_Material_State";
                                       columnscount++;
                                       break;
                                   case "技术标准":
                                       GridSource1.Columns[i].ColumnName = "Material_Tech_Condition";
                                       columnscount++;
                                       break;

                       

                                   case "规格":
                                       GridSource1.Columns[i].ColumnName = "Rough_Spec";
                                       columnscount++;
                                       break;
                                   case "需求尺寸":
                                       GridSource1.Columns[i].ColumnName = "ROUGH_SIZE";
                                       columnscount++;
                                       break;
                                   case "物资编码":
                                       GridSource1.Columns[i].ColumnName = "ItemCode1";
                                       columnscount++;
                                       break;


                                   case "坯料尺寸":
                                       GridSource1.Columns[i].ColumnName = "Dinge_Size";
                                       columnscount++;
                                       break;

                                   case "特殊需求":
                                       GridSource1.Columns[i].ColumnName = "Special_Needs";
                                       columnscount++;
                                       break;
                                  
                                 
                               
               
                                    
                                   case "需求件数":
                                       GridSource1.Columns[i].ColumnName = "NumCasesSum";
                                       columnscount++;
                                       break;
                                   case "单件质量":
                                       GridSource1.Columns[i].ColumnName = "Mat_Rough_Weight";
                                       columnscount++;
                                       break;
                                   case "计量单位":
                                       GridSource1.Columns[i].ColumnName = "MAT_UNIT";
                                       columnscount++;
                                       break;

                                case "需求数量":
                                       GridSource1.Columns[i].ColumnName = "DemandNumSum";
                                       columnscount++;
                                       break;
                                case "单价":
                                       GridSource1.Columns[i].ColumnName = "Unit_Price";
                                       columnscount++;
                                       break;
                                   /*    case "物资件数":
                                           GridSource1.Columns[i].ColumnName = "Quantity";
                                           columnscount++;
                                           break;
                                            case "总价":
                                           GridSource1.Columns[i].ColumnName = "Sum_Price";
                                           columnscount++;
                                           break;
                                    */

                           

                    
                         

                                case "需求时间":
                                    GridSource1.Columns[i].ColumnName = "DemandDate";
                                    columnscount++;
                                    break;

                           

                                case "紧急程度":
                                    GridSource1.Columns[i].ColumnName = "Urgency_Degre";
                                    columnscount++;
                                    break;
                                case "密级":
                                    GridSource1.Columns[i].ColumnName = "Secret_Level";
                                    columnscount++;
                                    break;

                                case "用途":
                                    GridSource1.Columns[i].ColumnName = "Use_Des";
                                    columnscount++;
                                    break;
                                case "研制阶段":
                                    GridSource1.Columns[i].ColumnName = "stage";
                                    columnscount++;
                                    break;

                                case "合格证":
                                    GridSource1.Columns[i].ColumnName = "Certification";
                                    columnscount++;
                                    break;

                                case "配送地址":
                                    GridSource1.Columns[i].ColumnName = "Shipping_Address";
                                    columnscount++;
                                    break;
                                case "国产/进口":
                                    GridSource1.Columns[i].ColumnName = "Attribute4";
                                    columnscount++;
                                    break;


                        
                            
                            }
                        }
                        if (columnscount < 24)
                        {
                            GridSource1 = new System.Data.DataTable();
                            RadGridImport.Rebind();
                            File.Delete(filePath);
                            RadNotificationAlert.Text = "失败！请参照Excel模板页面表头";
                            RadNotificationAlert.Show();
                            return;
                        }

                        GridSource1.Columns.Add("ID");
                       // GridSource1.Columns.Add("Material_Name");
                     //   GridSource1.Columns.Add("Material_Mark");
                      //  GridSource1.Columns.Add("CN_Material_State");
                     // GridSource1.Columns.Add("Material_Tech_Condition");
                    //    GridSource1.Columns.Add("MAT_UNIT");
                    //    GridSource1.Columns.Add("Rough_Spec");
                      //  GridSource1.Columns.Add("Unit_Price");
                        GridSource1.Columns.Add("Sum_Price");
                        GridSource1.Columns.Add("MaterialsDes");
                        int rowsid = 1;
                        for (int i = 0; i < GridSource1.Rows.Count; i++)
                        {
                            string itemCode1 = GridSource1.Rows[i]["ItemCode1"].ToString();
                            if (GridSource1.Rows[i]["DRAWING_NO"].ToString() != "" && itemCode1!= "")
                            {
                                GridSource1.Rows[i]["ID"] = rowsid;
                                rowsid++;
                                if (! Set_Txt_ByItemCode1(itemCode1, i))
                                {
                                    //RadNotificationAlert.Text = "第" + i.ToString() +"行物资编码不存在";
                                    //RadNotificationAlert.Show();
                                    //GridSource1.Rows[i].Delete();
                                  //  return;
                                }
                                try
                                {
                                   
                                  double unitPrice=  Convert.ToDouble(GridSource1.Rows[i]["Unit_Price"]);
                                  double demandnumSum=  Convert.ToDouble(GridSource1.Rows[i]["DemandNumSum"]);
                                  GridSource1.Rows[i]["Sum_Price"] =( unitPrice*demandnumSum).ToString();
                                }
                                catch
                                {
                                    RadNotificationAlert.Text = "单价、数量等必须为数字";
                                    RadNotificationAlert.Show();
                                    return;
                                   // GridSource1.Rows[i]["Sum_Price"] = "0"; 
                                }
                            }
                            else
                            {
                                GridSource1.Rows[i].Delete();
                            }
                        }
                

                        RadGridImport.Rebind();
                        HFGridItemsCount.Value = RadGridImport.Items.Count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    GridSource1 = new System.Data.DataTable();
                    RadGridImport.Rebind();
                    File.Delete(filePath);

                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
            else
            {
                GridSource1 = new System.Data.DataTable();
                RadGridImport.Rebind();

                RadNotificationAlert.Text = "请选择文件";
                RadNotificationAlert.Show();
                return;
            }
        }

        protected bool Set_Txt_ByItemCode1(string ItemCode1,int i)
        {
   
            string strSQL = " select * from GetCommItem_T_Item where seg3 = '" + ItemCode1 + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            if (dt.Rows.Count > 0)
            {
                string Seg5 = dt.Rows[0]["Seg5"].ToString().Substring(0, 4);
                GridSource1.Rows[i]["MaterialsDes"] = dt.Rows[0]["SEG4"].ToString();
             //   GridSource1.Rows[i]["Material_Mark"] = dt.Rows[0]["SEG13"].ToString();
              //  GridSource1.Rows[i]["CN_Material_State"] = "";
              //  GridSource1.Rows[i]["Material_Tech_Condition"] = "";
                switch (Seg5)
                {
                    case "YY01":
                     //   GridSource1.Rows[i]["Material_Mark"] = "";
                  //      GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg21"].ToString();
                    //    GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                    //    GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg13"].ToString();
                   
                        break;
                    case "YY02":
                     //   GridSource1.Rows[i]["Material_Mark"] = "";
                     //   GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                     //   GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                   //     GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg15"].ToString();

                        break;
                    case "YY03":
                     //   GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                     //   GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                      //  GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg15"].ToString();

                       // GridSource1.Rows[i]["DINGE_SIZE"] = dt.Rows[0]["Seg16"].ToString();
                        break;
                
                    case "YY05":
                      //  GridSource1.Rows[i]["Material_Tech_Condition"] = dt.Rows[0]["SEG16"].ToString();;
                    //    GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                     //   GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                 //       GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY04":
                    case "YY06":
                  //      GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                    //    GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                    //    GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY07":
                    //    GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg20"].ToString();
                     //   GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                //        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY08":
                    case "YY09":
                   //     GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                    //    GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                  //      GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    default:
                   //     GridSource1.Rows[i]["Material_Name"] = "";
                     //   GridSource1.Rows[i]["MAT_UNIT"] = "";
                     //   GridSource1.Rows[i]["Rough_Spec"] = "";
                        break;
                }
                return true;
            }
            else
            {
               // RadNotificationAlert.Text = "物资编码不存在";
              //  RadNotificationAlert.Show();
              //  GridSource1.Rows[i].Delete();
                return false;
            }
        }


        protected void RBClear_Click(object sender, EventArgs e)
        {
            try
            {
             //   for (int i = 0; i < RadGridImport.Items.Count; i++)
              //  {
                 //   GridSource1.Rows.RemoveAt(i);
             //   }
                GridSource1.Dispose();
                GridSource1 = new System.Data.DataTable();
                RadGridImport.Rebind();
                HFGridItemsCount.Value = RadGridImport.Items.Count.ToString();
                RadNotificationAlert.Text = "清空成功！";
                RadNotificationAlert.Show();

            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "清空失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                return;
            }

        }
        protected void RBDelete_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < RadGridImport.SelectedItems.Count; i++)
                {
                    GridSource1.Rows.RemoveAt(i);
                }
                RadGridImport.Rebind();
                HFGridItemsCount.Value = RadGridImport.Items.Count.ToString();

            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "删除失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                return;
            }

        }
        protected void RBImport_Click(object sender, EventArgs e)
        {

            if (RadGridImport.Items.Count == 0)
            {
                RadNotificationAlert.Text = "失败！没有可导入数据";
                RadNotificationAlert.Show();
                return;
            }
            if (Session["UserId"] == null)
            {
                RadNotificationAlert.Text = "登录超时，请重新登录！";
                RadNotificationAlert.Show();
                return;
            }

            //try
            {
           
                string DBContractConn =
                    ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString
                        .ToString();
                DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = "";
                int MDPId = 0;
                int Submit_Type = Convert.ToInt32(this.ViewState["submit_type"].ToString());//1－工艺试验件；2－技术创新课题；3－车间备料

                GridItemCollection gridItems=null;
                  if (RadGridImport.SelectedItems.Count > 0)
                  {
                      gridItems = RadGridImport.SelectedItems;
                  }
                  else
                  {
                      gridItems = RadGridImport.Items;
                  }
                  for (int i = 0; i < gridItems.Count; i++)
                 {
                    if (gridItems[i] is GridDataItem)
                    {
                        GridDataItem item = gridItems[i] as GridDataItem;
               
                        string DRAWING_NO = item["DRAWING_NO"].Text.Trim();
                        if (DRAWING_NO == "" || DRAWING_NO == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，产品图号：请输入产品图号";
                            RadNotificationAlert.Show();
                            return;
                        }

                        string TaskCode = item["TaskCode"].Text.Trim();
                        if (TaskCode == "" || TaskCode == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，任务号：请输入任务号";
                            RadNotificationAlert.Show();
                            return;
                        }

                        string ItemCode1 = item["ItemCode1"].Text.Trim();
                        /*
                        if (ItemCode1 == "" || ItemCode1 == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，物资编码：请输入物资编码";
                            RadNotificationAlert.Show();
                            return;
                        }
                        else
                        {
                            strSQL = " select count(*) from GetCommItem_T_Item where Seg3 = '" + ItemCode1 + "'";
                            if (DBI.GetSingleValue(strSQL) == "0")
                            {
                                RadNotificationAlert.Text = "失败！没有该物资编码，不能提交";
                                RadNotificationAlert.Show();
                                return;
                            }
                        }
                         */
                        string Material_Name = item["Material_Name"].Text.Trim();
                        string Mat_Rough_Weight = item["Mat_Rough_Weight"].Text.Trim();
                        string Rough_Spec = item["Rough_Spec"].Text.Trim();

             
  
                        /*
                        if (Rough_Spec == "" || Rough_Spec == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，物资规格：请输入物资规格";
                            RadNotificationAlert.Show();
                            return;
                        }
                        */

                        string MAT_UNIT = item["MAT_UNIT"].Text.Trim();
                        /*
                        if (MAT_UNIT == "" || MAT_UNIT == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，计量单位：请输入计量单位";
                            RadNotificationAlert.Show();
                            return;
                        }
                        */
                        string ROUGH_SIZE = item["ROUGH_SIZE"].Text.Trim();
                        if (ROUGH_SIZE == "" || ROUGH_SIZE == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，需求尺寸：请输入需求尺寸";
                            RadNotificationAlert.Show();
                            return;
                        }

                        string DINGE_SIZE = item["DINGE_SIZE"].Text.Trim();
                      /*  if (DINGE_SIZE == "" || DINGE_SIZE == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，定额需求尺寸：请输入需求尺寸";
                            RadNotificationAlert.Show();
                            return;
                        }*/

                        string NumCasesSum = item["NumCasesSum"].Text.Trim();
                        try
                        {
                            Convert.ToInt32(NumCasesSum);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，共计需求件数：请输入数字";
                            RadNotificationAlert.Show();
                            return;
                        }

                        string DemandNumSum = item["DemandNumSum"].Text.Trim();
                        try
                        {
                            Convert.ToDecimal(DemandNumSum);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，共计需求数量：请输入数字";
                            RadNotificationAlert.Show();
                            return;
                        }
                                        
                        string Unit_Price = item["Unit_Price"].Text.Trim();
                        try
                        {
                            Convert.ToDecimal(Unit_Price);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，单价：请输入数字";
                            RadNotificationAlert.Show();
                            return;
                        }

                        string Special_Needs = item["Special_Needs"].Text.Trim();
                        if (Special_Needs == "" || Special_Needs == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，特殊需求：请输入特殊需求";
                            RadNotificationAlert.Show();
                            return;
                        }

                        string DemandDate = item["DemandDate"].Text.Trim();
                        try
                        {
                            Convert.ToDateTime(DemandDate);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，需求时间：不是有效日期";
                            RadNotificationAlert.Show();
                            return;
                        }

                       
                     
             

                      //  RadComboBox RadComboBoxMaterialDept = item.FindControl("RadComboBoxMaterialDept") as RadComboBox;
                       

                        /*
                                     RadComboBox RadComboBoxUrgency_Degree = item.FindControl("RadComboBoxUrgencyDegree") as RadComboBox;
                                     string Urgency_Degre = RadComboBoxUrgency_Degree.SelectedValue; 

                 
                                     RadComboBox RadComboBoxSecret_Level = item.FindControl("RadComboBoxSecretLevel") as RadComboBox;
                                     string Secret_Level = RadComboBoxSecret_Level.SelectedValue; 

                  

                                     RadComboBox RadComboBoxStage = item.FindControl("RadComboBoxStage") as RadComboBox;
                                     string Stage = RadComboBoxStage.SelectedValue; 

                                     RadComboBox RadComboBoxUseDes = item.FindControl("RadComboBoxUseDes") as RadComboBox;
                                     string Use_Des = RadComboBoxUseDes.SelectedValue;

                                     RadComboBox RadComboBoxShipping_Address = item.FindControl("RadComboBoxShippingAddress") as RadComboBox;
                                     string Shipping_Address = RadComboBoxShipping_Address.SelectedItem.Text; 

                   
                                     RadComboBox RadComboBoxCertification = item.FindControl("RadComboBoxCertification") as RadComboBox;
                                     string Certification = RadComboBoxCertification.SelectedValue;


                                     RadComboBox RadComboBoxAttribute4 = item.FindControl("RadComboBoxAttribute4") as RadComboBox;
                                     string Attribute4 = RadComboBoxAttribute4.SelectedValue;
                        
                                     RadDropDownList RadDropDownListProject = item.FindControl("RDDL_Project1") as RadDropDownList;
                                     string Project = RadDropDownListProject.SelectedValue;
                              */


                        M_Demand_Merge_List mta = new M_Demand_Merge_List();

                        string Material_Mark = item["Material_Mark"].Text.Trim();
                        string CN_Material_State = item["CN_Material_State"].Text.Trim();
                        string Material_Tech_Condition = item["Material_Tech_Condition"].Text.Trim();
                        string Dinge_Size = item["Dinge_Size"].Text.Trim();
                        string TDM_Description = item["TDM_Description"].Text.Trim();
                        string MaterialsDes = item["MaterialsDes"].Text.Trim();
                        mta.Material_Mark = Material_Mark;
                        mta.CN_Material_State = CN_Material_State;
                        mta.Material_Tech_Condition = Material_Tech_Condition;
                        mta.Dinge_Size = Dinge_Size;
                        mta.MaterialsDes = MaterialsDes;
                        mta.TDM_Description = TDM_Description;
                        mta.Drawing_No = DRAWING_NO;
                        mta.TaskCode = TaskCode;
                        mta.DemandDate = Convert.ToDateTime(DemandDate);
                        mta.ItemCode1 = ItemCode1;
                        mta.NumCasesSum = Convert.ToDecimal(NumCasesSum);
                        mta.Mat_Rough_Weight = Mat_Rough_Weight;
                        mta.DemandNumSum = Convert.ToDecimal(DemandNumSum);
                        mta.Mat_Unit = MAT_UNIT;
                        mta.Quantity = Convert.ToInt32(NumCasesSum);
                        mta.Rough_Size = ROUGH_SIZE;
                        mta.Dinge_Size =DINGE_SIZE;
                        mta.Rough_Spec = Rough_Spec;
                        mta.Special_Needs = Special_Needs;

                      

                    
                        string Project = item["Project"].Text.Trim();

                        strSQL = "select DICT_Code, DICT_Name from GetBasicdata_T_Item  where DICT_CLASS = 'CUX_DM_PROJECT' and ENABLED_FLAG = 'Y' order by DICT_Name";
                        DataTable dtTemp = DBI.Execute(strSQL, true);
                        string projectStr = null;
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (Project == dtTemp.Rows[count]["DICT_Name"].ToString())
                            {
                                mta.Project = dtTemp.Rows[count]["DICT_Code"].ToString();
                                projectStr += dtTemp.Rows[count]["DICT_Name"].ToString()+"，";
                                break;
                            }

                        }
                        if (mta.Project == null)
                        {
                            RadNotificationAlert.Text = "型号输入错误，正确的选项为："+projectStr+"请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }

                        string stage = item["Stage"].Text.Trim();

                        strSQL = "select * from Sys_Phase order by Code";
                        dtTemp = DBI.Execute(strSQL, true);
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (stage == dtTemp.Rows[count]["Phase"].ToString())
                            {
                                mta.Stage = dtTemp.Rows[count]["Code"].ToString();
                                break;
                            }

                        }
                        if (mta.Stage == null)
                        {
                            RadNotificationAlert.Text = "研制阶段输入错误，正确的选项为：M,C,S,D，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                        mta.MaterialDept = RadComboBox_Dept.SelectedValue;
                      //  strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                                 //      " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                                   //    " where Sys_UserInfo_PWD.ID = '" + userid + "'";
                    //    dtTemp = DBI.Execute(strSQL, true);

                     //   string MaterialDept = dtTemp.Rows[0]["DeptCode"].ToString();
                        strSQL = "select KeyWord from Sys_Dict" + " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
                        " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + mta.MaterialDept + "')";
                        dtTemp = DBI.Execute(strSQL, true);

                        mta.Shipping_Address = item["Shipping_Address"].Text.Trim();
                        if (mta.Shipping_Address != dtTemp.Rows[0]["KeyWord"].ToString())
                        {
                            RadNotificationAlert.Text = "配送地址输入不正确，正确的选项为：" + dtTemp.Rows[0]["KeyWord"].ToString();
                            RadNotificationAlert.Show();
                            return;

                        }





                        string Use_Des = item["Use_Des"].Text.Trim();

                        strSQL = " Select * From GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE' and ENABLED_FLAG='Y'";// +PackId + "'";
                        dtTemp = DBI.Execute(strSQL, true);
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (Use_Des == dtTemp.Rows[count]["DICT_NAME"].ToString())
                            {
                                mta.Use_Des = dtTemp.Rows[count]["DICT_CODE"].ToString();
                                break;
                            }

                        }
                        if (mta.Use_Des == null)
                        {
                            RadNotificationAlert.Text = "用途输入错误，正确的选项为：弹上/箭上,辅料,工装,中间料，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }

                        mta.Secret_Level = item["Secret_Level"].Text.Trim();

                        strSQL = "SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)";
                        dtTemp = DBI.Execute(strSQL, true);
                        bool inputIsRight = false;
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (mta.Secret_Level == dtTemp.Rows[count]["SecretLevel_Name"].ToString())
                            {
                                inputIsRight = true;
                                break;

                            }

                        }
                        if (!inputIsRight)
                        {
                            RadNotificationAlert.Text = "密级输入错误，正确的选项为：内部,秘密,机密，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                        string Urgency_Degre = item["Urgency_Degre"].Text.Trim();
                        strSQL = " Select * From GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL' and ENABLED_FLAG='Y'";// +PackId + "'";
                        dtTemp = DBI.Execute(strSQL, true);
                        for (int count = 0; count < dtTemp.Rows.Count; count++)
                        {
                            if (Urgency_Degre == dtTemp.Rows[count]["DICT_NAME"].ToString())
                            {
                                mta.Urgency_Degre = dtTemp.Rows[count]["DICT_CODE"].ToString();
                                break;
                            }

                        }
                        if (mta.Urgency_Degre == null)
                        {
                            RadNotificationAlert.Text = "紧急程度输入错误，正确的选项为：一般，急，特急，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }




                        mta.Certification = item["Certification"].Text.Trim();
                        if (mta.Certification != "Y" && mta.Certification != "N")
                        {
                            RadNotificationAlert.Text = "合格证列输入错误，正确的选项为：Y,N，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                        mta.Attribute4 = item["Attribute4"].Text.Trim();
                        if (mta.Attribute4 != "国产" && mta.Attribute4 != "进口")
                        {
                            RadNotificationAlert.Text = "国产/进口列输入错误，正确的选项为：国产,进口，请选择其中之一！";
                            RadNotificationAlert.Show();
                            return;
                        }
                       
                 

                        
                    
                           
                        mta.Unit_Price = Convert.ToDecimal(Unit_Price); 
                        mta.Sum_Price = mta.Unit_Price*mta.Quantity;


                        mta.Submit_Type = Submit_Type;
                        mta.Material_Name = Material_Name;

                        if (this.hfBh.Value != null && this.hfBh.Value != "")
                        {
                            MDPId = Convert.ToInt32(this.hfBh.Value);
                        }
                        else
                        {
                            strSQL = @"exec Proc_Add_M_Demand_Plan_List_Technology " + userid + ",'" + mta.TaskCode + "',0,0," + this.ViewState["submit_type"].ToString() + ",0,''";
                            DataTable dt = DBI.Execute(strSQL, true);
                            if (dt.Rows.Count == 1)
                            {
                                MDPId = Convert.ToInt32(dt.Rows[0][0].ToString());
                                this.hfBh.Value = dt.Rows[0][0].ToString();
                            }
                            else
                            {
                                RadNotificationAlert.Text = "失败！Proc_Add_M_Demand_Plan_List_Technology返回的记录数不唯一";
                                RadNotificationAlert.Show();
                                return;
                            }

                        }
                        mta.MDPId = MDPId;

                        strSQL = @"exec Proc_Save_Technology_Apply_NoSubmit '" + mta.MDPId + "','" + mta.Drawing_No + "','" +
                                 mta.TaskCode + "','" + mta.MaterialDept + "','" + mta.ItemCode1 + "','" + mta.DemandNumSum + "','" +
                                 mta.NumCasesSum + "','" + mta.Mat_Rough_Weight + "','" +
                                 mta.Material_Mark + "','" + mta.CN_Material_State + "','" + mta.Material_Tech_Condition + "','" +
                                 mta.TDM_Description + "','" + mta.MaterialsDes + "','" +
                                 mta.Mat_Unit + "','" + mta.Quantity + "','" + mta.Rough_Size + "','" + mta.Dinge_Size + "','" + mta.Rough_Spec + "','" +
                                 mta.DemandDate + "','" + mta.Special_Needs + "','" + mta.Urgency_Degre + "','" +
                                 mta.Secret_Level + "','" +
                                 mta.Stage + "','" + mta.Use_Des + "','" + mta.Shipping_Address + "','" + mta.Certification +
                                 "','" +
                                 mta.Unit_Price + "','" + mta.Sum_Price + "','" + mta.Submit_Type + "','" + userid + "','" +
                                 mta.Material_Name + "', '" + mta.Attribute4 + "','" + mta.Project + "'";
                        DBI.Execute(strSQL);
                       
                        //  Get_M_Technology_ApplyByMDPId(MDPId.ToString());
                    }
                }
                RadBtnSubmit.Visible = true;
                RadNotificationAlert.Text = "导入成功！";
                RadNotificationAlert.Show();
           
                GridSource = Common.AddTableRowsID(GetTechnologyTestList(MDPId.ToString(), Submit_Type.ToString()));
                RadGrid_TechnologyTestList.DataSource = GridSource;
                RadGrid_TechnologyTestList.Rebind();
           
             

            }
          /* 
           catch (Exception ex)
            {
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                return;
            }
            */
        }

  
        protected void btnDown_Click(object sender, EventArgs e)
        {
            int i = 0;
            DirectoryInfo info =
                new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Plan/物资导入模板");
            if (System.IO.Directory.Exists(info.ToString()))
            {
                foreach (FileInfo n in info.GetFiles())
                {
                    if (n.Name == "车间物资导入模板.xlsx")
                    {
                        i = 1;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(n.Name));
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                        Response.ContentType = "application/ms-excel";
                        this.EnableViewState = false;

                        Response.WriteFile(Server.MapPath(@"~\Plan\物资导入模板\") + n.Name);
                        Response.End();
                        return;
                    }
                }
            }
            if (i == 0)
            {
                RadNotificationAlert.Text = "没有找到模版";
                RadNotificationAlert.Show();
            }
        }




       protected void RadComboBoxMaterialDept_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString(); ;
            RadComboBox RadComboBoxShipping_Address = (cb.Parent.Parent as GridDataItem).FindControl("RadComboBoxShippingAddress") as RadComboBox;
          
           string strSQL = "select KeyWord from Sys_Dict" +
               " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
               " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + cb.SelectedItem.Value + "')";
           DataTable dt = DBI.Execute(strSQL, true);
           RadComboBoxShipping_Address.Items.Clear();         
           RadComboBoxShipping_Address.DataSource = dt;
           RadComboBoxShipping_Address.DataTextField = "KeyWord";
           RadComboBoxShipping_Address.DataValueField = "KeyWord";
           RadComboBoxShipping_Address.DataBind();
        }

  
        private void BindDeptUserAddress1()
        {
            if (Session["UserId"] == null)
            {
                RadNotificationAlert.Text = "登录超时，请重新登录！";
                RadNotificationAlert.Show();
                return;
            }
            string userid = Session["UserId"].ToString();
            string strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                            " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                            " where Sys_UserInfo_PWD.ID = '" + userid + "'";
            DataTable dt = DBI.Execute(strSQL, true);

            RadComboBox_Dept1.DataSource = dt;
            RadComboBox_Dept1.DataTextField = "Dept";
            RadComboBox_Dept1.DataValueField = "DeptID";
            RadComboBox_Dept1.DataBind();

        

            RadComboBox_User1.DataSource = dt;
            RadComboBox_User1.DataTextField = "UserName";
            RadComboBox_User1.DataValueField = "UserID";
            RadComboBox_User1.DataBind();


        }

        protected void RadComboBox_Dept_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (RadComboBox_Dept1.SelectedValue != "0")
            {
                RadComboBox_User1.ClearSelection();
                string strSQL = "SELECT * FROM [Sys_UserInfo_PWD] where Dept='" + RadComboBox_Dept1.SelectedValue + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                RadComboBox_User1.DataSource = dt;
                RadComboBox_User1.DataTextField = "UserName";
                RadComboBox_User1.DataValueField = "Id";
                RadComboBox_User1.DataBind();
            }
        }
        #endregion


    }
}