﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Camc.Web.Library;
using mms.k2;

namespace mms.MaterialApplicationCollar
{
    public partial class MaterialAppWindow : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null) 
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
                return;
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                string strSQL = "";
                HFType.Value = Request.QueryString["Type"].ToString();
                if (HFType.Value == "0")
                {
                    lbltitle.Text = "型号投产物资领用申请";
                }
                else if (HFType.Value == "1")
                {
                    lbltitle.Text = "试验件物资领用申请";
                }
                else if (HFType.Value == "2")
                {
                    lbltitle.Text = "课题物资领用申请";
                }
                else if (HFType.Value == "3")
                {
                    lbltitle.Text = "车间备料物资领用申请";
                }
                else if (HFType.Value == "4")
                {
                    lbltitle.Text = "无需求物料申请";
                }

              //  strSQL = " select * from Sys_UserInfo_PWD where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and Dept = (select Dept from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "')" +
               //     " and ID in (select UserID from Sys_UserInRole where RoleID in (select ID from Sys_RoleInfo where RoleName like '%车%间%调%度%员%' and Is_Del ='false')) ";

                strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%车%间%调%度%员%' and Is_Del ='false'";
                DataTable dtdd = DBI.Execute(strSQL, true);
                RDDL_DiaoDu.DataSource = dtdd;
                RDDL_DiaoDu.DataTextField = "UserName";
                RDDL_DiaoDu.DataValueField = "DomainAccount";
                RDDL_DiaoDu.DataBind();
                //新增
                if (Request.QueryString["MAID"] == null)
                {
                    strSQL = " select * from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "'";
                    DataTable dtuserinfo = DBI.Execute(strSQL, true);
                    RTB_Applicant.Text = dtuserinfo.Rows[0]["UserName"].ToString();
                    RTB_ContactInformation.Text = dtuserinfo.Rows[0]["Phone"].ToString();

             	   Session["UserAccount"] = dtuserinfo.Rows[0]["UserAccount"].ToString();
                }
                //  strSQL = " select * from Sys_UserInfo_PWD where IsDel = 'false' and DomainAccount != '' and DomainAccount is not null" +
                //    " and ID in (select UserId from Sys_UserInRole where RoleID in (select ID from Sys_RoleInfo where RoleName like '%型%号%计%划%员%'))";
                strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%型%号%计%划%员%' and Is_Del ='false'";

                DataTable dtxh = DBI.Execute(strSQL, true);
                RDDL_XingHao.DataSource = dtxh;
                RDDL_XingHao.DataTextField = "UserName";
                RDDL_XingHao.DataValueField = "DomainAccount";
                RDDL_XingHao.DataBind();

  
                //   strSQL = " select * from Sys_UserInfo_PWD where IsDel = 'false' and DomainAccount != '' and DomainAccount is not null" +
                //      " and ID in (select UserId from Sys_UserInRole where RoleID in (select ID from Sys_RoleInfo where RoleName like '%物%资%计%划%员%'))";
                strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%物%资%计%划%员%' and Is_Del ='false'";
                DataTable dtwz = DBI.Execute(strSQL, true);
                RDDL_WuZi.DataSource = dtwz;
                RDDL_WuZi.DataTextField = "UserName";
                RDDL_WuZi.DataValueField = "DomainAccount";
                RDDL_WuZi.DataBind();

                string userId = Session["UserId"].ToString();
                strSQL = " select DeptCode, Dept from Sys_DeptEnum where ID = (select Dept from Sys_UserInfo_PWD where Id = '" + userId + "')";
                DataTable dtdept = DBI.Execute(strSQL, true);
                lbl_Dept.Text = dtdept.Rows[0]["Dept"].ToString();
                HF_DeptCode.Value = dtdept.Rows[0]["DeptCode"].ToString();

                strSQL = " select DICT_Code, DICT_Name from GetBasicdata_T_Item  where DICT_CLASS = 'CUX_DM_USAGE' and ENABLED_FLAG = 'Y' order by DICT_Name";
                RadComboBoxUseDes.DataSource = DBI.Execute(strSQL, true);
                RadComboBoxUseDes.DataValueField = "DICT_Code";
                RadComboBoxUseDes.DataTextField = "DICT_Name";
                RadComboBoxUseDes.DataBind();
                strSQL = " SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)";
                RadComboBoxSecretLevel.DataSource = DBI.Execute(strSQL, true);
                RadComboBoxSecretLevel.DataValueField = "SecretLevel_Name";
                RadComboBoxSecretLevel.DataTextField = "SecretLevel_Name";
                RadComboBoxSecretLevel.DataBind();
                //修改
                if (Request.QueryString["MAID"] != null)
                {
                    HFMAID.Value = Request.QueryString["MAID"].ToString();


                    //strSQL = " select * , (select MDPID from M_Demand_Merge_List where ID = MaterialApplication.Material_Id) as MDPLID from MaterialApplication where ID = '" + HFMAID.Value + "'";
                    strSQL = " select * ,M_Demand_Merge_List.DemandNum_Left,M_Demand_Merge_List.Quantity_Left,M_Demand_Merge_List.MDPID as MDPLID,M_Demand_Merge_List.Special_Needs from MaterialApplication left join M_Demand_Merge_List on M_Demand_Merge_List.ID = MaterialApplication.Material_Id where MaterialApplication.ID = '" + HFMAID.Value + "'";
                    DataTable dtma = DBI.Execute(strSQL, true);
                    HFMDMLID.Value = dtma.Rows[0]["Material_ID"].ToString();
                    HFMDPLID.Value = dtma.Rows[0]["MDPLID"].ToString();
                    Session["UserAccount"] = dtma.Rows[0]["UserAccount"].ToString();
                    RTB_Applicant.Text = dtma.Rows[0]["Applicant"].ToString();
                    RDP_ApplicationTime.SelectedDate = null;
                    if (dtma.Rows[0]["ApplicationTime"].ToString() != "" && Convert.ToDateTime(dtma.Rows[0]["ApplicationTime"].ToString()) >= RDP_ApplicationTime.MinDate)
                    {
                        RDP_ApplicationTime.SelectedDate = Convert.ToDateTime(dtma.Rows[0]["ApplicationTime"].ToString());
                    }
                    RTB_ContactInformation.Text = dtma.Rows[0]["ContactInformation"].ToString();
                    RTB_TaskCode.Text = dtma.Rows[0]["TaskCode"].ToString();
                    RTB_DrawingNo.Text = dtma.Rows[0]["Drawing_No"].ToString();
                    RTB_Quantity.Text = dtma.Rows[0]["Quantity"].ToString();
                    RTB_PleaseTakeQuality.Text = dtma.Rows[0]["PleaseTakeQuality"].ToString();
                    RadComboBoxIsApply.SelectedValue = dtma.Rows[0]["Is_Apply"].ToString();
                    type.Value=dtma.Rows[0]["Type"].ToString();
                    if (type.Value != "4")
                    {
                        DemandNum_Left.Value = (Convert.ToDecimal(dtma.Rows[0]["DemandNum_Left"].ToString()) + Convert.ToDecimal(RTB_PleaseTakeQuality.Text)).ToString();
                        Quantity_Left.Value = (Convert.ToDecimal(dtma.Rows[0]["Quantity_Left"].ToString()) + Convert.ToDecimal(RTB_Quantity.Text)).ToString();
                        DemandNum_Applied.Value = (Convert.ToDecimal(dtma.Rows[0]["DemandNum_Applied"].ToString()) - Convert.ToDecimal(RTB_PleaseTakeQuality.Text)).ToString();
                        Quantity_Applied.Value = (Convert.ToDecimal(dtma.Rows[0]["Quantity_Applied"].ToString()) - Convert.ToDecimal(RTB_Quantity.Text)).ToString();
                    }
                    RDDL_TheMaterialWay.SelectedIndex = 0;
                    if (RDDL_TheMaterialWay.FindItemByText(dtma.Rows[0]["TheMaterialWay"].ToString()) != null)
                    {
                        RDDL_TheMaterialWay.FindItemByText(dtma.Rows[0]["TheMaterialWay"].ToString()).Selected = true;
                    }
                    RDP_FeedingTime.SelectedDate = null;
                    if (dtma.Rows[0]["FeedingTime"].ToString() != "" && Convert.ToDateTime(dtma.Rows[0]["FeedingTime"].ToString()) >= RDP_FeedingTime.MinDate)
                    {
                        RDP_FeedingTime.SelectedDate = Convert.ToDateTime(dtma.Rows[0]["FeedingTime"].ToString());
                    }
                    

                    RTB_Remark.Text = dtma.Rows[0]["Remark"].ToString();
                    RB_IsDispatch.Checked = Convert.ToBoolean(dtma.Rows[0]["IsDispatch"].ToString());
                    RB_IsConfirm.Checked = Convert.ToBoolean(dtma.Rows[0]["IsConfirm"].ToString());

                    lbl_ItemCode.Text = dtma.Rows[0]["ItemCode"].ToString();
                    lbl_Material_Name.Text = dtma.Rows[0]["Material_Name"].ToString();
                    lbl_Material_Mark.Text = dtma.Rows[0]["Material_Mark"].ToString();
                    lbl_CN_Material_State.Text = dtma.Rows[0]["CN_Material_State"].ToString();
                    lbl_Material_Tech_Condition.Text = dtma.Rows[0]["Material_Tech_Condition"].ToString();
                    lbl_Rough_Spec.Text = dtma.Rows[0]["Rough_Spec"].ToString();
                    lbl_Mat_Rough_Weight.Text = dtma.Rows[0]["Mat_Rough_Weight"].ToString();
                    lb1_MaterialsDes.Text = dtma.Rows[0]["MaterialsDes"].ToString();
                    lbl_Mat_Unit.Text = dtma.Rows[0]["Mat_Unit"].ToString();
                    RTB_Rough_Size.Text = dtma.Rows[0]["Rough_Size"].ToString();
                    lbl_Dinge_Size.Text = dtma.Rows[0]["Dinge_Size"].ToString();
                   
                  
                    if (RadComboBoxSecretLevel.FindItemByValue(dtma.Rows[0]["SECURITY_LEVEL"].ToString()) != null)
                    {
                        RadComboBoxSecretLevel.FindItemByValue(dtma.Rows[0]["SECURITY_LEVEL"].ToString()).Selected = true;
                    }
                    if (RadComboBoxUseDes.FindItemByValue(dtma.Rows[0]["USAGE"].ToString()) != null)
                    {
                        RadComboBoxUseDes.FindItemByValue(dtma.Rows[0]["USAGE"].ToString()).Selected = true;
                    }
                    if (RDDL_DiaoDu.FindItemByValue(dtma.Rows[0]["DiaoDuApprove"].ToString()) != null)
                    {
                        RDDL_DiaoDu.FindItemByValue(dtma.Rows[0]["DiaoDuApprove"].ToString()).Selected = true;
                    }
                    if (RDDL_XingHao.FindItemByValue(dtma.Rows[0]["XingHaoJiHuaYuanApprove"].ToString()) != null)
                    {
                        RDDL_XingHao.FindItemByValue(dtma.Rows[0]["XingHaoJiHuaYuanApprove"].ToString()).Selected = true;
                    }
                    if (RDDL_WuZi.FindItemByValue(dtma.Rows[0]["WuZiJiHuaYuanApprove"].ToString()) != null)
                    {
                        RDDL_WuZi.FindItemByValue(dtma.Rows[0]["WuZiJiHuaYuanApprove"].ToString()).Selected = true;
                    }

                }
                //新增
                else
                {
                    RDP_ApplicationTime.SelectedDate = DateTime.Now;
                    if (Request.QueryString["MDMLID"] != null && Request.QueryString["MDMLID"].ToString() != "")
                    {

                        HFMDMLID.Value = Request.QueryString["MDMLID"].ToString();

                        strSQL = " select * from M_Demand_Merge_List where ID = '" + HFMDMLID.Value + "'";
                        DataTable dt = DBI.Execute(strSQL, true);

                        HFMDPLID.Value = dt.Rows[0]["MDPID"].ToString();
                        if (RadComboBoxSecretLevel.FindItemByValue(dt.Rows[0]["SECURITY_LEVEL"].ToString()) != null)
                        {
                            RadComboBoxSecretLevel.FindItemByValue(dt.Rows[0]["SECURITY_LEVEL"].ToString()).Selected = true;
                        }
                        if (RadComboBoxUseDes.FindItemByValue(dt.Rows[0]["Use_Des"].ToString()) != null)
                        {
                            RadComboBoxUseDes.FindItemByValue(dt.Rows[0]["Use_Des"].ToString()).Selected = true;
                        }
                        lbl_ItemCode.Text = dt.Rows[0]["ItemCode1"].ToString();
                        lbl_Material_Name.Text = dt.Rows[0]["Material_Name"].ToString();
                        lbl_Material_Mark.Text = dt.Rows[0]["Material_Mark"].ToString();

                        lbl_CN_Material_State.Text = dt.Rows[0]["CN_Material_State"].ToString();
                        lbl_Material_Tech_Condition.Text = dt.Rows[0]["Material_Tech_Condition"].ToString();
                        lbl_Rough_Spec.Text = dt.Rows[0]["Rough_Spec"].ToString();

                        lbl_Mat_Rough_Weight.Text = dt.Rows[0]["Mat_Rough_Weight"].ToString();
                        lb1_MaterialsDes.Text = dt.Rows[0]["MaterialsDes"].ToString();
                        lbl_Mat_Unit.Text = dt.Rows[0]["Mat_Unit"].ToString();
                        RTB_Rough_Size.Text = dt.Rows[0]["Rough_Size"].ToString();
                        lbl_Dinge_Size.Text = dt.Rows[0]["Dinge_Size"].ToString();

                        //lbl_AppQuantity.Text = Convert.ToDouble(dt.Rows[0]["AppQuantity"].ToString()).ToString();
                        RTB_Quantity.Text = dt.Rows[0]["Quantity_Left"].ToString();
                        RTB_PleaseTakeQuality.Text = dt.Rows[0]["DemandNum_Left"].ToString();
                        DemandNum_Left.Value = dt.Rows[0]["DemandNum_Left"].ToString();
                        Quantity_Left.Value=dt.Rows[0]["Quantity_Left"].ToString();
                        DemandNum_Applied.Value = dt.Rows[0]["DemandNum_Applied"].ToString();
                        Quantity_Applied.Value = dt.Rows[0]["Quantity_Applied"].ToString();
                        RTB_TaskCode.Text = dt.Rows[0]["TaskCode"].ToString();
                        RTB_DrawingNo.Text = dt.Rows[0]["Drawing_No"].ToString();
                        RTB_Remark.Text = dt.Rows[0]["Special_Needs"].ToString(); //dtma.Rows[0]["Remark"].ToString();//by fyc

                        if (HFType.Value == "0")
                        {
                            string Correspond_Draft_Code = dt.Rows[0]["Correspond_Draft_Code"].ToString();
                            string mddldid = Correspond_Draft_Code.Split(',')[0];
                            strSQL = " select TDM_Description, Material_Name, Material_Mark, CN_Material_State, Material_Tech_Condition, Mat_Rough_Weight ,MaterialsDes,Dinge_Size from M_Demand_DetailedList_Draft where Id = '" + mddldid + "'";
                            DataTable dtmddld = DBI.Execute(strSQL, true);
                            lbl_Dinge_Size.Text = dtmddld.Rows[0]["Dinge_Size"].ToString();
                            lbl_Material_Mark.Text = dtmddld.Rows[0]["Material_Mark"].ToString();
                            lbl_CN_Material_State.Text = dtmddld.Rows[0]["CN_Material_State"].ToString();
                            lbl_Material_Tech_Condition.Text = dtmddld.Rows[0]["Material_Tech_Condition"].ToString();
                            lbl_Mat_Rough_Weight.Text = dtmddld.Rows[0]["Mat_Rough_Weight"].ToString();
                            lb1_MaterialsDes.Text = dtmddld.Rows[0]["MaterialsDes"].ToString();
                        }
                    }
                
                }

                if (lbl_Material_Name.Text.IndexOf('棒') != -1)
                {
                    DataTable dt = DBI.Execute("select * from Sys_ComputationalFormula", true);
                    if (dt.Rows.Count > 0)
                    {
                        string Parameter1 = dt.Rows[0]["Parameter1"].ToString();
                        try
                        {
                            double Parameter = Convert.ToDouble(Parameter1);
                            string Rough_Spec = lbl_Rough_Spec.Text;
                            if (Rough_Spec.Length > 0)
                            {
                                string spec = Rough_Spec.Substring(1, Rough_Spec.Length - 1);
                                try
                                {
                                    double value = Convert.ToDouble(spec);
                                    if (value < Parameter)
                                    {
                                        RDDL_TheMaterialWay.SelectedIndex = 1;
                                        RDDL_TheMaterialWay.Enabled = false;
                                    }
                                }
                                catch { }
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        protected void RB_Submit_Click(object sender, EventArgs e)
        {
            string Applicant = RTB_Applicant.Text.Trim();
            string Dept = HF_DeptCode.Value;
            string ApplicationTime = RDP_ApplicationTime.SelectedDate.ToString();
            string ContactInformation = RTB_ContactInformation.Text.Trim();
            string TaskCode = RTB_TaskCode.Text.Trim();
            string DrawingNo = RTB_DrawingNo.Text.Trim();
            string Quantity = RTB_Quantity.Text.Trim();
            string PleaseTakeQuality = RTB_PleaseTakeQuality.Text.Trim();
            string TheMaterialWay = RDDL_TheMaterialWay.SelectedItem.Text;
            string FeedingTime = RDP_FeedingTime.SelectedDate.ToString();
            string IsDispatch = RB_IsDispatch.Checked.ToString();
            string IsConfirm = RB_IsConfirm.Checked.ToString();
            string Remark = RTB_Remark.Text.Trim();
            string ItemCode = lbl_ItemCode.Text.Trim();
            string Material_Name = lbl_Material_Name.Text;
            string Material_Mark = lbl_Material_Mark.Text;
            string CN_Material_State = lbl_CN_Material_State.Text;
            string Material_Tech_Condition = lbl_Material_Tech_Condition.Text;
            string Rough_Spec = lbl_Rough_Spec.Text;
            string Mat_Rough_Weight = lbl_Mat_Rough_Weight.Text;
            string MaterialsDes = lb1_MaterialsDes.Text;
            string Mat_Unit = lbl_Mat_Unit.Text;
            string Rough_Size = RTB_Rough_Size.Text;
            string Dinge_Size = lbl_Dinge_Size.Text.Trim();
            var diaodu = RDDL_DiaoDu.SelectedValue;
            var xinghao = RDDL_XingHao.SelectedValue;
            var wuzi = RDDL_WuZi.SelectedValue;
            var securityLevel = RadComboBoxSecretLevel.SelectedValue;
            var Usage = RadComboBoxUseDes.SelectedValue;
            var isApply = RadComboBoxIsApply.SelectedValue;
            if (Applicant == "")
            {
                RadNotificationAlert.Text = "请输入申请人！";
                RadNotificationAlert.Show();
                return;
            }
            if (Dept == "")
            {
                RadNotificationAlert.Text = "请输入部门！";
                RadNotificationAlert.Show();
                return;
            }
            if (ApplicationTime == "")
            {
                RadNotificationAlert.Text = "请输入申请时间！";
                RadNotificationAlert.Show();
                return;
            }

            if (FeedingTime == "")
            {
                RadNotificationAlert.Text = "请输入需求供料时间！";
                RadNotificationAlert.Show();
                return;
            }
            if (ContactInformation == "")
            {
                RadNotificationAlert.Text = "请输入联系方式！";
                RadNotificationAlert.Show();
                return;
            }
            if (diaodu == "")
            {
                RadNotificationAlert.Text = "请选择 车间调度员";
                RadNotificationAlert.Show();
                return;
            }
            if (xinghao == "")
            {
                RadNotificationAlert.Text = "请选择 型号计划员";
                RadNotificationAlert.Show();
                return;
            }
            if (wuzi == "")
            {
                RadNotificationAlert.Text = "请选择 物资计划员";
                RadNotificationAlert.Show();
                return;                
            }
            if (Quantity == "")
            {
                RadNotificationAlert.Text = "请输入申请件数";
                RadNotificationAlert.Show();
                return;
            }
            if (PleaseTakeQuality == "")
            {
                RadNotificationAlert.Text = "请输入申请数量";
                RadNotificationAlert.Show();
                return;
            }

            if (Quantity_Left.Value != "")
            {

                if (Convert.ToDecimal(Quantity) > Convert.ToDecimal(Quantity_Left.Value))
                {

                    RadNotificationAlert.Text = "申请件数不能大于剩余件数";
                    RadNotificationAlert.Show();
                    return;
                }
            }

            if (DemandNum_Left.Value != "")
            {

                if (Convert.ToDecimal(PleaseTakeQuality) > Convert.ToDecimal(DemandNum_Left.Value))
                {

                    RadNotificationAlert.Text = "申请数量不能大于剩余数量";
                    RadNotificationAlert.Show();
                    return;
                }
            }
            try
            {
                string strSQL = "";
                bool isNewCreate = false;
                if (HFMAID.Value == "")
                {
                    isNewCreate = true;
                    strSQL = "declare @id int";
                    strSQL += " Insert into MaterialApplication (Type, Material_Id, Applicant, Dept, ApplicationTime, ContactInformation, TheMaterialWay, TaskCode, Drawing_No"
                        + " , Draft_Code, Quantity, FeedingTime, IsDispatch, IsConfirm, Remark, MaterialType, Material_Name, Material_Mark, CN_Material_State, Material_Tech_Condition"
                        + " , SECURITY_LEVEL,USAGE,Is_Apply"
                        + " , Rough_Spec, Mat_Rough_Weight,MaterialsDes, Mat_Unit,Rough_Size,Dinge_Size,PleaseTakeQuality, AppState, ReturnReason, Is_Del, ItemCode,DiaoDuApprove, XingHaoJiHuaYuanApprove, WuZiJiHuaYuanApprove, UserId, UserAccount)"
                        + " values ('" + HFType.Value + "','" + HFMDMLID.Value + "', '" + Applicant + "','" + Dept + "','" + ApplicationTime + "','" + ContactInformation + "','" + TheMaterialWay + "','" + TaskCode + "','" + DrawingNo + "'"
                        + " ,Null,'" + Quantity + "','" + FeedingTime + "','" + IsDispatch + "','" + IsConfirm + "','" + Remark + "'"
                        + " ,Null,'" + Material_Name + "','" + Material_Mark + "','" + CN_Material_State + "','" + Material_Tech_Condition + "'"
                        + " ,'" + securityLevel+"','"+Usage+"','"+isApply+"'"
                        + " ,'" + Rough_Spec + "','" + Mat_Rough_Weight + "','" + MaterialsDes + "','" + Mat_Unit + "','" + Rough_Size + "','" +Dinge_Size+"','"+ PleaseTakeQuality + "','1',Null,'false'"
                        + " ,'" + ItemCode + "','" +diaodu + "','" + xinghao + "','" + wuzi + "','" + Session["UserId"].ToString() + "'"
                        + " ,(select DomainAccount from Sys_UserInfo_PWD where Id = '" + Session["UserId"].ToString() + "'))" +
                        " select @id = @@identity"
                        + " Insert into MaterialApplication_Log (MaterialApplicationId, Operation_UserId, Operation_Time, Operation_Remark)"
                        + " values (@id, '" + Session["UserId"].ToString() + "',GetDate(),'申请') select @id";

                    string id = DBI.GetSingleValue(strSQL).ToString();

                    HFMAID.Value = id;
                }
                else
                {
                    strSQL = " declare @sql nvarchar(max) select @sql = '' select @sql = @sql + (case when Applicant = '" + Applicant + "' then '' else '申请人=[原]' + Applicant + '[新]' + '" + Applicant + "' end) "
                        + " + (case when ApplicationTime =Convert(datetime,'" + ApplicationTime + "') then '' else '申请时间=[原]' + CONVERT(varchar(100), ApplicationTime, 23) + '[新]' + '" + ApplicationTime + "' end)"
                        + " + (case when ContactInformation = '" + ContactInformation + "' then '' else '联系方式=[原]' + ContactInformation + '[新]' + '" + ContactInformation + "' end)"
                        + " + (case when TheMaterialWay = '" + TheMaterialWay + "' then '' else '领料方式=[原]' + TheMaterialWay + '[新]' + '" + TheMaterialWay + "' end)"
                        + " + (case when Quantity = Convert(int,'" + Quantity + "') then '' else '申请数量=[原]' + Convert(nvarchar(50),Quantity) + '[新]' + '" + Quantity + "' end)"
                        + " + (case when FeedingTime =Convert(datetime,'" + FeedingTime + "') then '' else '要求供料时间=[原]' + CONVERT(varchar(100), FeedingTime, 23) + '[新]' + '" + FeedingTime + "' end)"
                        + " + (case when IsDispatch = '" + IsDispatch + "' then '' else '申请数量=[原]' + Convert(nvarchar(50),IsDispatch) + '[新]' + '" + IsDispatch + "' end)"
                        + " + (case when IsConfirm = '" + IsConfirm + "' then '' else '申请数量=[原]' + Convert(nvarchar(50),IsConfirm) + '[新]' + '" + IsConfirm + "' end)"
                        + " + (case when Remark = '" + Remark + "' then '' else '申请数量=[原]' + Remark + '[新]' + '" + Remark + "' end)"
                        + " from MaterialApplication  where ID = '" + HFMAID.Value + "'";
                    strSQL += " Update MaterialApplication set Applicant = '" + Applicant + "' , ApplicationTime = '" + ApplicationTime + "', ContactInformation = '" + ContactInformation + "'"
                        + " ,TheMaterialWay = '" + TheMaterialWay + "', Quantity = '" + Quantity + "', FeedingTime = '" + FeedingTime + "', IsDispatch = '" + IsDispatch + "'"
                        + " , IsConfirm = '" + IsConfirm + "', Remark = '" + Remark + "', ReturnReason = '', AppState = '1'"
                        + " , DiaoDuApprove = '" + diaodu + "', XingHaoJiHuaYuanApprove = '" + xinghao + "', WuZiJiHuaYuanApprove = '" + wuzi + "', TaskCode = '" + TaskCode
                        + "', PleaseTakeQuality = '" + PleaseTakeQuality + "'"
                        + ", SECURITY_LEVEL = '" + securityLevel + "'"
                        + ", USAGE = '" + Usage + "'"
                        + ", Is_Apply = '" + isApply + "'"
                        + " where ID = '" + HFMAID.Value + "'";
                    strSQL += " Insert into MaterialApplication_Log (MaterialApplicationId, Operation_UserId, Operation_Time, Operation_Remark)"
                        + " values ('" + HFMAID.Value + "', '" + Session["UserId"].ToString() + "',GetDate(),'重新提交' + @sql)";
                    DBI.Execute(strSQL);

                }
                /*
               string demandNum_Applied = (Convert.ToDecimal(DemandNum_Applied.Value) + Convert.ToDecimal(PleaseTakeQuality)).ToString();
                string quantity_Applied = (Convert.ToDecimal(Quantity_Applied.Value) + Convert.ToDecimal(Quantity)).ToString();
                string demandNum_Left = (Convert.ToDecimal(DemandNum_Left.Value) - Convert.ToDecimal(PleaseTakeQuality)).ToString();
                string quantity_Left = (Convert.ToDecimal(Quantity_Left.Value) - Convert.ToDecimal(Quantity)).ToString();
                strSQL =
                     " Update M_Demand_Merge_List set Quantity_Applied = '" + quantity_Applied + "',DemandNum_Applied='" + demandNum_Applied +
                    "',DemandNum_Left='" + demandNum_Left + "',Quantity_Left='" + quantity_Left +

                                "' where ID = '" + HFMDMLID.Value + "'";


                DBI.Execute(strSQL);
               */
                K2BLL k2Bll = new K2BLL();
                var result = k2Bll.StartNewProcess(HFMAID.Value);
               if (result == "")
                {
                    strSQL =
                        " Insert into MaterialApplication_Log (MaterialApplicationId, Operation_UserID, Operation_Time, Operation_Remark)" +
                        " values('" + HFMAID.Value + "','" + Session["UserId"].ToString() + "',GetDate() " +
                        " ,'进入流程平台:调度员：' + '" + diaodu + "' + '型号计划员：' + '" + xinghao + "' +'物资计划员：' + '" + wuzi + "')";
                    DBI.Execute(strSQL);
                    if (type.Value != "4")
                    {

                        string demandNum_Applied = (Convert.ToDecimal(DemandNum_Applied.Value) + Convert.ToDecimal(PleaseTakeQuality)).ToString();
                        string quantity_Applied = (Convert.ToDecimal(Quantity_Applied.Value) + Convert.ToDecimal(Quantity)).ToString();
                        string demandNum_Left = (Convert.ToDecimal(DemandNum_Left.Value) - Convert.ToDecimal(PleaseTakeQuality)).ToString();
                        string quantity_Left = (Convert.ToDecimal(Quantity_Left.Value) - Convert.ToDecimal(Quantity)).ToString();
                        strSQL =
                             " Update M_Demand_Merge_List set Quantity_Applied = '" + quantity_Applied + "',DemandNum_Applied='" + demandNum_Applied +
                            "',DemandNum_Left='" + demandNum_Left + "',Quantity_Left='" + quantity_Left +

                                        "' where ID = '" + HFMDMLID.Value + "'";


                        DBI.Execute(strSQL);
                    }
                    RadNotificationAlert1.Text = "申请成功！进入流程平台";
                    RadNotificationAlert1.Show();
                    
                }
               else
                {
                    if (isNewCreate)
                    {
                         strSQL = " Update MaterialApplication set Is_Del = 'true' where Id = '" + HFMAID.Value + "'";
                         DBI.Execute(strSQL);
                    }
                    //进入流程平台失败
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
     //   private System.Timers.Timer _timer = null;
        protected void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
          //  _timer.Stop();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
        }
    }
}