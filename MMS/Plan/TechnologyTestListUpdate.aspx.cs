﻿using System;
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

namespace mms.Plan
{
    public partial class TechnologyTestListUpdate : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                if (Request.QueryString["MDMLID"] != null || Request.QueryString["MDMLID"] != "")
                {
                    string mdmlid = Request.QueryString["MDMLID"].ToString();
                    DataTable dt = GetMDemandMergeList(mdmlid);
                    this.MDMLID.Value = mdmlid;
                    this.MDPLID.Value = dt.Rows[0]["MDPId"].ToString();
                    string strSQL = "";
                    string title = "";
                    string submit_Type = dt.Rows[0]["Submit_Type1"].ToString();

                    switch (submit_Type)
                    {
                        case "1":
                            title = "工艺试验件物资需求信息";
                            HiddenField.Value = "工艺试验件任务-->物资需求信息";

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
                            title = "技术创新物资需求信息";
                            HiddenField.Value = "技术创新任务-->物资需求信息";
                       
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
                            title = "车间备料物资需求信息";
                            HiddenField.Value = "车间备料任务-->物资需求信息";
                           // trAttribute4.Visible = true;

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
                    this.ViewState["Submit_Type"] = submit_Type;
                    this.b_title.InnerHtml = title;
                    
                    BindDDlDept();
                    BindDDlMaterialDept();
           
                    strSQL = " select Dept from Sys_UserInfo_PWD where ID = '" + dt.Rows[0]["User_ID"].ToString() +"'";
                    string dept = DBI.GetSingleValue(strSQL).ToString();
                    if (RadComboBox_Dept.FindItemByValue(dept) != null)
                    {
                        RadComboBox_Dept.FindItemByValue(dept).Selected = true;
                    }
                    BindDDlUserInfo(RadComboBox_Dept.SelectedValue);
                    if( RadComboBox_User.FindItemByValue(dt.Rows[0]["User_ID"].ToString())!=null)
                    {
                        RadComboBox_User.FindItemByValue(dt.Rows[0]["User_ID"].ToString()).Selected = true;
                    }
                    span_apply_time.SelectedDate = Convert.ToDateTime(dt.Rows[0]["Submit_Date"].ToString());
                    txt_TaskCode.Text = dt.Rows[0]["TaskCode"].ToString();
                    txt_Drawing_No.Text = dt.Rows[0]["Drawing_No"].ToString();
                    txt_ItemCode1.Text = dt.Rows[0]["ItemCode1"].ToString();
                    RDP_DemandDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["DemandDate"].ToString());
                    RTB_MaterialName.Text = dt.Rows[0]["Material_Name"].ToString();

                    RTB_TDM_Description.Text = dt.Rows[0]["TDM_Description"].ToString();
                    RTB_MaterialsDes.Text = dt.Rows[0]["MaterialsDes"].ToString();
                    RTB_Material_Mark.Text = dt.Rows[0]["Material_Mark"].ToString();
                    RTB_CN_Material_State.Text = dt.Rows[0]["CN_Material_State"].ToString();
                    RTB_Material_Tech_Condition.Text = dt.Rows[0]["Material_Tech_Condition"].ToString();

                    txt_NumCasesSum.Text = dt.Rows[0]["NumCasesSum"].ToString();
                    txt_DemandNumSum.Text = dt.Rows[0]["DemandNumSum"].ToString();
                    txt_Mat_Unit.Text = dt.Rows[0]["Mat_Unit"].ToString();
                    RTB_Rough_Size.Text = dt.Rows[0]["Rough_Size"].ToString();
                    RTB_Dinge_Size.Text = dt.Rows[0]["Dinge_Size"].ToString();
                    txt_Rough_Spec.Text = dt.Rows[0]["Rough_Spec"].ToString();
                    RTB_Mat_Rough_Weight.Text = dt.Rows[0]["Mat_Rough_Weight"].ToString();
                    RTB_Unit_Price.Text = dt.Rows[0]["Unit_Price"].ToString();
                    span_Sum_Price.Text = dt.Rows[0]["Sum_Price"].ToString();
                    strSQL = " SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)";
                    RadComboBoxSecretLevel.DataSource = DBI.Execute(strSQL, true);
                    RadComboBoxSecretLevel.DataValueField = "SecretLevel_Name";
                    RadComboBoxSecretLevel.DataTextField = "SecretLevel_Name";
                    RadComboBoxSecretLevel.DataBind();

                    if (RadComboBoxSecretLevel.FindItemByValue(dt.Rows[0]["Secret_Level"].ToString()) != null)
                    {
                        RadComboBoxSecretLevel.FindItemByValue(dt.Rows[0]["Secret_Level"].ToString()).Selected = true;
                    }
                    strSQL = " select * from Sys_Phase order by Code";
                    RadComboBoxStage.DataSource = DBI.Execute(strSQL, true);
                    RadComboBoxStage.DataValueField = "Code";
                    RadComboBoxStage.DataTextField = "Phase";
                    RadComboBoxStage.DataBind();

                    if (RadComboBoxStage.FindItemByValue(dt.Rows[0]["Stage"].ToString()) != null)
                    {
                        RadComboBoxStage.FindItemByValue(dt.Rows[0]["Stage"].ToString()).Selected = true; ;
                    }
                    strSQL = " select DICT_Code, DICT_Name from GetBasicdata_T_Item  where DICT_CLASS = 'CUX_DM_PROJECT' and ENABLED_FLAG = 'Y' order by DICT_Name";
                    RDDL_Project.DataSource = DBI.Execute(strSQL, true);
                    RDDL_Project.DataValueField = "DICT_Code";
                    RDDL_Project.DataTextField = "DICT_Name";
                    RDDL_Project.DataBind();
                    if (RDDL_Project.FindItemByValue(dt.Rows[0]["Project"].ToString()) != null)
                    {
                        RDDL_Project.FindItemByValue(dt.Rows[0]["Project"].ToString()).Selected = true;
                    }

                    strSQL = " select DICT_Code, DICT_Name from GetBasicdata_T_Item  where DICT_CLASS = 'CUX_DM_USAGE' and ENABLED_FLAG = 'Y' order by DICT_Name";
                    RadComboBoxUseDes.DataSource = DBI.Execute(strSQL, true);
                    RadComboBoxUseDes.DataValueField = "DICT_Code";
                    RadComboBoxUseDes.DataTextField = "DICT_Name";
                    RadComboBoxUseDes.DataBind();
                    if (RadComboBoxUseDes.FindItemByValue(dt.Rows[0]["Use_Des"].ToString()) != null)
                    {
                        RadComboBoxUseDes.FindItemByValue(dt.Rows[0]["Use_Des"].ToString()).Selected = true;
                    }
                    if (RadComboBoxCertification.FindItemByValue(dt.Rows[0]["Certification"].ToString()) != null)
                    {
                        RadComboBoxCertification.FindItemByValue(dt.Rows[0]["Certification"].ToString()).Selected = true;
                    }
                    rtb_SpecialNeeds.Text = dt.Rows[0]["Special_Needs"].ToString();

                    strSQL = " select DICT_Code, DICT_Name from GetBasicdata_T_Item  where DICT_CLASS = 'CUX_DM_URGENCY_LEVEL' and ENABLED_FLAG = 'Y' order by DICT_Name";
                    RadComboBoxUrgencyDegre.DataSource = DBI.Execute(strSQL, true);
                    RadComboBoxUrgencyDegre.DataValueField = "DICT_Code";
                    RadComboBoxUrgencyDegre.DataTextField = "DICT_Name";
                    RadComboBoxUrgencyDegre.DataBind();

                    if (RadComboBoxUrgencyDegre.FindItemByValue(dt.Rows[0]["Urgency_Degre"].ToString()) != null)
                    {
                        RadComboBoxUrgencyDegre.FindItemByValue(dt.Rows[0]["Urgency_Degre"].ToString()).Selected = true;
                    }
                    if (RadComboBoxMaterialDept.FindItemByValue(dt.Rows[0]["MaterialDept"].ToString()) != null)
                    {
                        RadComboBoxMaterialDept.FindItemByValue(dt.Rows[0]["MaterialDept"].ToString()).Selected = true;
                    }
                    BindDDlAddress(RadComboBoxMaterialDept.SelectedValue);
                    if (RadComboBoxShipping_Address.FindItemByText(dt.Rows[0]["Shipping_Address"].ToString()) != null)
                    {
                        RadComboBoxShipping_Address.FindItemByText(dt.Rows[0]["Shipping_Address"].ToString()).Selected = true;
                    }
                    if (dt.Rows[0]["Attribute4"].ToString() == "进口") {
                        RB_Attribute42.Checked = true;
                    }

                    if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }

                    strSQL = " select Dept from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "'";
                    string dept1 = DBI.GetSingleValue(strSQL).ToString();
                 
                    strSQL = " select DeptCode, Dept from Sys_DeptEnum where ID =  '" + dept1 + "'";
                 //   string userId = Session["UserId"].ToString();
                 //   strSQL = " select DeptCode, Dept from Sys_DeptEnum where ID = (select Dept from Sys_UserInfo_PWD where Id = '" + userId + "')";

                    dt = DBI.Execute(strSQL, true);
                    string departCode = "0";
                    if (dt.Rows.Count!= 0)
                    {
                        departCode = dt.Rows[0]["DeptCode"].ToString();
                        HF_DeptCode.Value = departCode;
                        if (departCode == "B")
                        {
                            RadBtnSubmit.Visible = false;
                            RadBtnSave.Text = "提交物流中心";

                        }
                    }
                }
                else
                {
                    Response.Redirect(Page.Request.UrlReferrer.ToString());
                }
            }
        }

        private void BindDDlDept()
        {
            string strSQL = "SELECT * FROM [Sys_DeptEnum] WHERE ([Is_Del] = 0) order by dept";
            DataTable dt = DBI.Execute(strSQL, true);
            RadComboBox_Dept.DataSource = dt;
            RadComboBox_Dept.DataTextField = "Dept";
            RadComboBox_Dept.DataValueField = "Id";
            RadComboBox_Dept.DataBind();
        }
        private void BindDDlUserInfo(string Dept_Id)
        {
            RadComboBox_User.ClearSelection();
            string strSQL = "SELECT * FROM [Sys_UserInfo_PWD] where Dept='" + Dept_Id + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            RadComboBox_User.DataSource = dt;
            RadComboBox_User.DataTextField = "UserName";
            RadComboBox_User.DataValueField = "Id";
            RadComboBox_User.DataBind();
        }
        protected void RadComboBox_Dept_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (RadComboBox_Dept.SelectedValue != "0")
            {
                BindDDlUserInfo(RadComboBox_Dept.SelectedValue);
            }
        }
        private void BindDDlMaterialDept()
        {
            string strSQL = "SELECT * from Sys_DeptEnum WHERE (Cust_Account_ID is not null) order by dept";
            DataTable dt = DBI.Execute(strSQL, true);
            RadComboBoxMaterialDept.DataSource = dt;
            RadComboBoxMaterialDept.DataTextField = "Dept";
            RadComboBoxMaterialDept.DataValueField = "DeptCode";
            RadComboBoxMaterialDept.DataBind();
            RadComboBoxMaterialDept.SelectedIndex = 0;
            BindDDlAddress(RadComboBoxMaterialDept.SelectedValue);
        }
        private void BindDDlAddress(string DeptCode)
        {
            RadComboBoxShipping_Address.Items.Clear();
            string strSQL = "select KeyWord from Sys_Dict" +
                " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
                " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + DeptCode + "')";
            DataTable dt = DBI.Execute(strSQL, true);
            RadComboBoxShipping_Address.DataSource = dt;
            RadComboBoxShipping_Address.DataTextField = "KeyWord";
            RadComboBoxShipping_Address.DataValueField = "KeyWord";
            RadComboBoxShipping_Address.DataBind();
        }

  
        protected void BindUrgencyDegre()
        {
            string strSQL = " select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'";
            RadComboBoxUrgencyDegre.DataSource = DBI.Execute(strSQL, true);
            RadComboBoxUrgencyDegre.DataTextField = "DICT_Name";
            RadComboBoxUrgencyDegre.DataValueField = "DICT_Code";
            RadComboBoxUrgencyDegre.DataBind();

        }


        protected int GetConfirmedQuantity(string MDMLID)
        {
           string strSQL = " select CONFIRMED_QUANTITY  from GetRqStatus_T_Item where SUBMISSION_STATUS='物流已确认' and USER_RQ_LINE_ID = '" + MDMLID + "'";
           DataTable dt= DBI.Execute(strSQL, true);
           int confirmedQuantity = 0;
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               confirmedQuantity+=Convert.ToInt32(dt.Rows[i]["CONFIRMED_QUANTITY"].ToString());
           }
           return confirmedQuantity;
        }

        protected DataTable GetMDemandMergeList(string MDMLID)
        {
            string strSQL = " select M_Demand_Merge_List.*, M_Demand_Plan_List.Submit_Type as Submit_Type1 from M_Demand_Merge_List join M_Demand_Plan_List on M_Demand_Plan_List.ID = M_Demand_Merge_List.MDPID where M_Demand_Merge_List.ID = '" + MDMLID + "'";
            return DBI.Execute(strSQL, true);
        }
        protected void RadBtnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = GetMDemandMergeList(Request.QueryString["MDMLID"].ToString());

            string ID = Request.QueryString["MDMLID"].ToString();
            string REQUESTER = RadComboBox_User.SelectedItem.Text.Trim();
            string REQUESTER_PHONE = dt.Rows[0]["REQUESTER_PHONE"].ToString();
            string Drawing_No = txt_Drawing_No.Text.Trim();
            string TaskCode = txt_TaskCode.Text.Trim();
            string DemandDate = "";
            try
            {
                DemandDate = Convert.ToDateTime(RDP_DemandDate.SelectedDate).ToString("yyyy-MM-dd");
            }
            catch
            {
                RadNotificationAlert.Text = "需求时间不能为空！"; RadNotificationAlert.Show(); return;
            }
            string Material_Name = RTB_MaterialName.Text.Trim();

            string Project = RDDL_Project.SelectedValue;
            string TDM_Description = RTB_TDM_Description.Text.Trim();
            string Material_Mark = RTB_Material_Mark.Text.Trim();
            string CN_Material_State = RTB_CN_Material_State.Text.Trim();
            string Material_Tech_Condition = RTB_Material_Tech_Condition.Text.Trim();
            string MaterialsDes = RTB_MaterialsDes.Text.Trim();


            string NumCasesSum = txt_NumCasesSum.Text.Trim();
            string DemandNumSum = txt_DemandNumSum.Text.Trim();
            string Mat_Unit = txt_Mat_Unit.Text.Trim();
            string Rough_Size = RTB_Rough_Size.Text.Trim();
            string Dinge_Size = RTB_Dinge_Size.Text.Trim();
            string Rough_Spec = txt_Rough_Spec.Text.Trim();
            string Unit_Price = RTB_Unit_Price.Text.Trim();
            string Mat_Rough_Weight = RTB_Mat_Rough_Weight.Text.Trim();
            string Sum_Price = span_Sum_Price.Text.Trim();
            string Secret_Level = RadComboBoxSecretLevel.SelectedValue;
            string Stage = RadComboBoxStage.SelectedValue;
            string Use_Des = RadComboBoxUseDes.SelectedValue;
            string Certification = RadComboBoxCertification.SelectedValue;
            string Special_Needs = rtb_SpecialNeeds.Text;
            string Urgency_Degre = RadComboBoxUrgencyDegre.SelectedValue;
            string MaterialDept = RadComboBoxMaterialDept.SelectedValue;
            string Shipping_Address = RadComboBoxShipping_Address.SelectedItem.Text;
            string MANUFACTURER = dt.Rows[0]["MANUFACTURER"].ToString();
            string SUBJECT = dt.Rows[0]["SUBJECT"].ToString();
            string UserID = Session["UserId"].ToString();         
            string Attribute4 = "";
            if (this.ViewState["Submit_Type"].ToString() == "3")
            {
                if (RB_Attribute41.Checked == true)
                {
                    Attribute4 = RB_Attribute41.Text.Trim();
                }
                else
                {
                    Attribute4 = RB_Attribute42.Text.Trim();
                }
            }
            string reason = RTB_Reason.Text.Trim();
            if (reason == "")
            {
                RadNotificationAlert.Text = "请输入变更原因!";
                RadNotificationAlert.Show();
                return;
            }

            if (REQUESTER == "") { RadNotificationAlert.Text = "申请人不能为空！"; RadNotificationAlert.Show(); return; }
            if (Drawing_No == "") { RadNotificationAlert.Text = "图号不能为空！"; RadNotificationAlert.Show(); return; }
            if (TaskCode == "") { RadNotificationAlert.Text = "任务号不能为空！"; RadNotificationAlert.Show(); return; }
            if (NumCasesSum == "") { RadNotificationAlert.Text = "需求件数不能为空！"; RadNotificationAlert.Show(); return; }
           
            if (DemandNumSum == "") {RadNotificationAlert.Text = "需求量不能为空！"; RadNotificationAlert.Show(); return;}
            int confirmedQuantiy = GetConfirmedQuantity(ID);
            if (Convert.ToDecimal(DemandNumSum) < confirmedQuantiy)
            {
                RadNotificationAlert.Text = "需求数量不能小于已发货数量！" + confirmedQuantiy.ToString();
                RadNotificationAlert.Show(); return;
            }
            if (Rough_Size == "") { RadNotificationAlert.Text = "物资尺寸不能为空！"; RadNotificationAlert.Show(); return; }

            if (Drawing_No != dt.Rows[0]["Drawing_No"].ToString() || TaskCode != dt.Rows[0]["TaskCode"].ToString() || Convert.ToDateTime(DemandDate) != Convert.ToDateTime(dt.Rows[0]["DemandDate"].ToString())
                || MaterialDept != dt.Rows[0]["MaterialDept"].ToString() || Convert.ToDecimal(NumCasesSum) != Convert.ToDecimal(dt.Rows[0]["NumCasesSum"].ToString())
                || Convert.ToDecimal(DemandNumSum) != Convert.ToDecimal(dt.Rows[0]["DemandNumSum"].ToString()) || Mat_Unit != dt.Rows[0]["Mat_Unit"].ToString()
                || Rough_Size != dt.Rows[0]["Rough_Size"].ToString() || Dinge_Size != dt.Rows[0]["Dinge_Size"].ToString() || Rough_Spec != dt.Rows[0]["Rough_Spec"].ToString()
                || Special_Needs != dt.Rows[0]["Special_Needs"].ToString() || Urgency_Degre != dt.Rows[0]["Urgency_Degre"].ToString()
                || Secret_Level != dt.Rows[0]["Secret_Level"].ToString() || Stage != dt.Rows[0]["Stage"].ToString() || Use_Des != dt.Rows[0]["Use_Des"].ToString()
                || Shipping_Address != dt.Rows[0]["Shipping_Address"].ToString() || Certification != dt.Rows[0]["Certification"].ToString()
                || Attribute4 != dt.Rows[0]["Attribute4"].ToString() || Material_Name != dt.Rows[0]["Material_Name"].ToString()
                || Project != dt.Rows[0]["Project"].ToString() || TDM_Description != dt.Rows[0]["TDM_Description"].ToString()
                || Material_Mark != dt.Rows[0]["Material_Mark"].ToString() || CN_Material_State != dt.Rows[0]["CN_Material_State"].ToString()
                || Material_Tech_Condition != dt.Rows[0]["Material_Tech_Condition"].ToString() || MaterialsDes != dt.Rows[0]["MaterialsDes"].ToString()
                ||Convert.ToDecimal(Unit_Price) != Convert.ToDecimal(dt.Rows[0]["Unit_Price"].ToString())|| Mat_Rough_Weight != dt.Rows[0]["Mat_Rough_Weight"].ToString())

            {
                if (Special_Needs != dt.Rows[0]["Special_Needs"].ToString() && Special_Needs == "")
                {
                    RadNotificationAlert.Text = "修改后的特殊需求不能为空";
                    RadNotificationAlert.Show();
                    return;
                }
                if (Shipping_Address != dt.Rows[0]["Shipping_Address"].ToString() && Shipping_Address == "")
                {
                    RadNotificationAlert.Text = "修改后的配送地址不能为空";
                    RadNotificationAlert.Show();
                    return;
                }

                try
                {
                    string strSQL = @"exec Proc_UpdateMDemandMergeList '" + ID + "','" +
                                 REQUESTER + "','" +
                                 REQUESTER_PHONE + "','" +
                                 Drawing_No + "','" +
                                 TaskCode + "','" +
                                 DemandDate + "','" +
                                 Material_Name + "','" +
                                 Project + "','" +
                                 TDM_Description + "','" +
                                 Material_Mark + "','" +
                                 CN_Material_State + "','" +
                                 Material_Tech_Condition + "','" +
                                 MaterialsDes + "','" +
                                 NumCasesSum + "','" +
                                 DemandNumSum + "','" +
                                 Mat_Unit + "','" +
                                 Rough_Size + "','" +
                                 Dinge_Size + "','" +
                                 Rough_Spec + "','" +
                                 Unit_Price + "','" +
                                 Mat_Rough_Weight + "','" +
                                 Sum_Price + "','" +
                                 Secret_Level + "','" +
                                 Stage + "','" +
                                 Use_Des + "','" +
                                 Certification + "','" +
                                 Special_Needs + "','" +
                                 Urgency_Degre + "','" +
                                 MaterialDept + "','" +
                                 Shipping_Address + "','" +
                                 MANUFACTURER + "','" +
                                 SUBJECT + "','" +
                                 UserID + "','" +
                                 reason.Replace(",", "，") + "'";
                    DataTable dtmd = DBI.Execute(strSQL, true);

                    if (HF_DeptCode.Value == "B")
                    {


                        string MDPLID = dtmd.Rows[0][0].ToString();
                        string MDPLCode = dtmd.Rows[0][1].ToString();

                        LogisticsCenterBLL bll = new LogisticsCenterBLL();
                        var result = bll.WriteRcoOrderRepeat(MDPLID);
                        if (result != "")
                        {
                            RadNotificationAlert.Text = "修改失败！<br />" + result;
                            RadNotificationAlert.Show();
                        }
                        else
                        {
                            RadNotificationAlert1.Text = "保存成功！";
                            RadNotificationAlert1.Show();
                          //  Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
                        }
                    }
                    else
                    {
                        //RadBtnSubmit.Visible = true;
                        RadNotificationAlert1.Text = "保存成功！";
                        RadNotificationAlert1.Show();
                    }
                  
                }
                catch (Exception ex)
                {
                    throw new Exception("保存需求更改信息出错" + ex.Message.ToString());
                }
         
            }
            else
            {
                RadNotificationAlert.Text = "修改后的数据没变化!!!";
                RadNotificationAlert.Show();
            }
        }


        protected void RadBtnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt = GetMDemandMergeList(Request.QueryString["MDMLID"].ToString());

            string ID = Request.QueryString["MDMLID"].ToString();
            string REQUESTER = RadComboBox_User.SelectedItem.Text.Trim();
            string REQUESTER_PHONE = dt.Rows[0]["REQUESTER_PHONE"].ToString();
            string Drawing_No = txt_Drawing_No.Text.Trim();
            string TaskCode = txt_TaskCode.Text.Trim();
            string DemandDate = "";
            try
            {
                DemandDate = Convert.ToDateTime(RDP_DemandDate.SelectedDate).ToString("yyyy-MM-dd");
            }
            catch
            {
                RadNotificationAlert.Text = "需求时间不能为空！"; RadNotificationAlert.Show(); return;
            }
            string Material_Name = RTB_MaterialName.Text.Trim();

            string Project = RDDL_Project.SelectedValue;
            string TDM_Description = RTB_TDM_Description.Text.Trim();
            string Material_Mark = RTB_Material_Mark.Text.Trim();
            string CN_Material_State = RTB_CN_Material_State.Text.Trim();
            string Material_Tech_Condition = RTB_Material_Tech_Condition.Text.Trim();
            string MaterialsDes = RTB_MaterialsDes.Text.Trim();


            string NumCasesSum = txt_NumCasesSum.Text.Trim();
            string DemandNumSum = txt_DemandNumSum.Text.Trim();
            string Mat_Unit = txt_Mat_Unit.Text.Trim();
            string Rough_Size = RTB_Rough_Size.Text.Trim();
            string Dinge_Size = RTB_Dinge_Size.Text.Trim();
            string Rough_Spec = txt_Rough_Spec.Text.Trim();
            string Unit_Price = RTB_Unit_Price.Text.Trim();
            string Mat_Rough_Weight = RTB_Mat_Rough_Weight.Text.Trim();
            string Sum_Price = span_Sum_Price.Text.Trim();
            string Secret_Level = RadComboBoxSecretLevel.SelectedValue;
            string Stage = RadComboBoxStage.SelectedValue;
            string Use_Des = RadComboBoxUseDes.SelectedValue;
            string Certification = RadComboBoxCertification.SelectedValue;
            string Special_Needs = rtb_SpecialNeeds.Text;
            string Urgency_Degre = RadComboBoxUrgencyDegre.SelectedValue;
            string MaterialDept = RadComboBoxMaterialDept.SelectedValue;
            string Shipping_Address = RadComboBoxShipping_Address.SelectedItem.Text;
            string MANUFACTURER = dt.Rows[0]["MANUFACTURER"].ToString();
            string SUBJECT = dt.Rows[0]["SUBJECT"].ToString();
            string UserID = Session["UserId"].ToString();
            string Attribute4 = "";
            if (this.ViewState["Submit_Type"].ToString() == "3")
            {
                if (RB_Attribute41.Checked == true)
                {
                    Attribute4 = RB_Attribute41.Text.Trim();
                }
                else
                {
                    Attribute4 = RB_Attribute42.Text.Trim();
                }
            }
            string reason = RTB_Reason.Text.Trim();
            if (reason == "")
            {
                RadNotificationAlert.Text = "请输入变更原因!";
                RadNotificationAlert.Show();
                return;
            }

            if (REQUESTER == "") { RadNotificationAlert.Text = "申请人不能为空！"; RadNotificationAlert.Show(); return; }
            if (Drawing_No == "") { RadNotificationAlert.Text = "图号不能为空！"; RadNotificationAlert.Show(); return; }
            if (TaskCode == "") { RadNotificationAlert.Text = "任务号不能为空！"; RadNotificationAlert.Show(); return; }
            if (NumCasesSum == "") { RadNotificationAlert.Text = "需求件数不能为空！"; RadNotificationAlert.Show(); return; }
           
            if (DemandNumSum == "") { RadNotificationAlert.Text = "需求量不能为空！"; RadNotificationAlert.Show(); return; }
            int confirmedQuantiy = GetConfirmedQuantity(ID);
            if (Convert.ToDecimal(DemandNumSum) < confirmedQuantiy)
            {
                RadNotificationAlert.Text = "需求数量不能小于已发货数量！" + confirmedQuantiy.ToString();
                RadNotificationAlert.Show(); return;
            }
            if (Rough_Size == "") { RadNotificationAlert.Text = "物资尺寸不能为空！"; RadNotificationAlert.Show(); return; }
            if (Drawing_No != dt.Rows[0]["Drawing_No"].ToString() || TaskCode != dt.Rows[0]["TaskCode"].ToString() || Convert.ToDateTime(DemandDate) != Convert.ToDateTime(dt.Rows[0]["DemandDate"].ToString())
               || MaterialDept != dt.Rows[0]["MaterialDept"].ToString() || Convert.ToDecimal(NumCasesSum) != Convert.ToDecimal(dt.Rows[0]["NumCasesSum"].ToString())
               || Convert.ToDecimal(DemandNumSum) != Convert.ToDecimal(dt.Rows[0]["DemandNumSum"].ToString()) || Mat_Unit != dt.Rows[0]["Mat_Unit"].ToString()
               || Rough_Size != dt.Rows[0]["Rough_Size"].ToString() || Dinge_Size != dt.Rows[0]["Dinge_Size"].ToString() || Rough_Spec != dt.Rows[0]["Rough_Spec"].ToString()
               || Special_Needs != dt.Rows[0]["Special_Needs"].ToString() || Urgency_Degre != dt.Rows[0]["Urgency_Degre"].ToString()
               || Secret_Level != dt.Rows[0]["Secret_Level"].ToString() || Stage != dt.Rows[0]["Stage"].ToString() || Use_Des != dt.Rows[0]["Use_Des"].ToString()
               || Shipping_Address != dt.Rows[0]["Shipping_Address"].ToString() || Certification != dt.Rows[0]["Certification"].ToString()
               || Attribute4 != dt.Rows[0]["Attribute4"].ToString() || Material_Name != dt.Rows[0]["Material_Name"].ToString()
               || Project != dt.Rows[0]["Project"].ToString() || TDM_Description != dt.Rows[0]["TDM_Description"].ToString()
               || Material_Mark != dt.Rows[0]["Material_Mark"].ToString() || CN_Material_State != dt.Rows[0]["CN_Material_State"].ToString()
               || Material_Tech_Condition != dt.Rows[0]["Material_Tech_Condition"].ToString() || MaterialsDes != dt.Rows[0]["MaterialsDes"].ToString()
               || Unit_Price != dt.Rows[0]["Unit_Price"].ToString() || Mat_Rough_Weight != dt.Rows[0]["Mat_Rough_Weight"].ToString())
            {
                if (Special_Needs != dt.Rows[0]["Special_Needs"].ToString() && Special_Needs == "")
                {
                    RadNotificationAlert.Text = "修改后的特殊需求不能为空";
                    RadNotificationAlert.Show();
                    return;
                }
                if (Shipping_Address != dt.Rows[0]["Shipping_Address"].ToString() && Shipping_Address == "")
                {
                    RadNotificationAlert.Text = "修改后的配送地址不能为空";
                    RadNotificationAlert.Show();
                    return;
                }
                string MDPLID = null;
                string MDPLCode = null;
                try
                {
                    string strSQL = @"exec Proc_UpdateMDemandMergeList '" + ID + "','" +
                                 REQUESTER + "','" +
                                 REQUESTER_PHONE + "','" +
                                 Drawing_No + "','" +
                                 TaskCode + "','" +
                                 DemandDate + "','" +
                                 Material_Name + "','" +
                                 Project + "','" +
                                 TDM_Description + "','" +
                                 Material_Mark + "','" +
                                 CN_Material_State + "','" +
                                 Material_Tech_Condition + "','" +
                                 MaterialsDes + "','" +
                                 NumCasesSum + "','" +
                                 DemandNumSum + "','" +
                                 Mat_Unit + "','" +
                                 Rough_Size + "','" +
                                 Dinge_Size + "','" +
                                 Rough_Spec + "','" +
                                 Unit_Price + "','" +
                                 Mat_Rough_Weight + "','" +
                                 Sum_Price + "','" +
                                 Secret_Level + "','" +
                                 Stage + "','" +
                                 Use_Des + "','" +
                                 Certification + "','" +
                                 Special_Needs + "','" +
                                 Urgency_Degre + "','" +
                                 MaterialDept + "','" +
                                 Shipping_Address + "','" +
                                 MANUFACTURER + "','" +
                                 SUBJECT + "','" +
                                 UserID + "','" +
                                 reason.Replace(",", "，") + "'";
                    DataTable dtmd = DBI.Execute(strSQL, true);
                    MDPLID = dtmd.Rows[0][0].ToString();
                    MDPLCode = dtmd.Rows[0][1].ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("保存需求更改信息出错" + ex.Message.ToString());
                }


                if (MDPLID != null && MDPLID != "")
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
                    if (this.ViewState["Submit_Type"].ToString() == "3")
                    {
                        approveAccount3 = RDDL_ApproveAccount3.SelectedValue;
                        if (approveAccount3 == "")
                        {
                            RadNotificationAlert.Text = "失败！没有选择物资综合计划员";
                            RadNotificationAlert.Show();
                            return;
                        }
                    }
                    string strSql = " Update M_Demand_Plan_List set DeptApproveAccountChange = '" + approveAccount1 + "', PlanOrTecApproveAccountChange = '" + approveAccount2 + "', MaterialPlanApproveAccountChange = '" + approveAccount3 + "' where ID = '" + MDPLID + "'";
                    DBI.Execute(strSql);

                    //ModifyTechnologySubmit(this.MDPLID.Value);
                    // WriteReqOrderRepeat(MDPLID.Value);
                    //Response.Redirect("/Plan/MDemandMergeListState.aspx?MDPID=" + MDPLID.Value);

                    K2PreBLL k2prebll = new K2PreBLL();
                    try
                    {
                        //  Convert.ToInt32(MDPId.Value);
                        var result = k2prebll.k2StartPreparesProgressChange(MDPLID, MDMLID.Value.ToString());

                        if (result == "")
                        {
                            RadNotificationAlert1.Text = "进入流程平台！";
                            RadNotificationAlert1.Show();
                            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow1();", true);
                        }
                        else
                        {
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
                else
                {
                    RadNotificationAlert.Text = "修改后的数据没变化!!!";
                    RadNotificationAlert.Show();

                }

            }
            else
            {
                RadNotificationAlert.Text = "存储过程Proc_UpdateMDemandMergeList执行出错!!!";
                RadNotificationAlert.Show();
            }
      
        }
        
   
    }
}

