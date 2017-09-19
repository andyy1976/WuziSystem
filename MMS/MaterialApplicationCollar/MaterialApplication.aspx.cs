using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Camc.Web.Library;
using System.Configuration;
using Telerik.Web.UI;

namespace mms.MaterialApplicationCollar
{
    public partial class MaterialApplication : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
               Common.CheckPermission(Session["UserName"].ToString(), "MaterialApplication", this.Page);
                
                string strSQL = "";
              //  strSQL = " select * from Sys_UserInfo_PWD where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and Dept = (select Dept from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "')" +
               //     " and ID in (select UserID from Sys_UserInRole where RoleID in (select ID from Sys_RoleInfo where RoleName like '%车%间%调%度%员%' and Is_Del ='false')) ";

                strSQL = " select * from V_Get_Sys_User_byRole where Isdel = 'false' and DomainAccount != '' and DomainAccount is not null and RoleName like '%车%间%调%度%员%' and Is_Del ='false'";
                DataTable dtdd = DBI.Execute(strSQL, true);
                RDDL_DiaoDu.DataSource = dtdd;
                RDDL_DiaoDu.DataTextField = "UserName";
                RDDL_DiaoDu.DataValueField = "DomainAccount";
                RDDL_DiaoDu.DataBind();


               // strSQL = " select * from Sys_UserInfo_PWD where ID = '" + Session["UserId"].ToString() + "'";
            //    DataTable dtuserinfo = DBI.Execute(strSQL, true);
             //   RTB_Applicant.Text = dtuserinfo.Rows[0]["UserName"].ToString();
             //   RTB_ContactInformation.Text = dtuserinfo.Rows[0]["Phone"].ToString();
                RTB_Applicant.Text = dtdd.Rows[0]["UserName"].ToString();
                RTB_ContactInformation.Text = dtdd.Rows[0]["Phone"].ToString();

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

                Session["gds"] = null;

               // strSQL = " select distinct dbo.Get_StrArrayStrOfIndex(Seg6,'.',1) as Seg6, substring(Seg5,1,4) as Type"
                 //   + " from [dbo].[GetCommItem_T_Item] order by substring(Seg5,1,4)";
                strSQL = "select * from Sys_Wuzi_Type";
                DataTable dt = DBI.Execute(strSQL, true);

                RDDLMT.DataSource = dt;
                RDDLMT.DataTextField = "Seg6";
                RDDLMT.DataValueField = "Type";
                RDDLMT.DataBind();
                

               /* strSQL = "select top 1000 SEG3,SEG4 * from GetCommItem_T_Item where SEG10 = 'N'";
                Session["gds"] = DBI.Execute(strSQL, true);
                RadGrid1.DataSource = (Session["gds"] as DataTable);
               */
                Telerik.Web.UI.DropDownListItem li = new Telerik.Web.UI.DropDownListItem("物资编码查询", "ItemCode");
                RDDLMT.Items.Add(li);

                RDP_ApplicationTime.SelectedDate = DateTime.Today;

            }
        }

        protected void confirmWindowClick(object sender, EventArgs e)
        {
            GridDataItem[] dataItems = RadGrid1.MasterTableView.GetSelectedItems();
            RTB_ItemCode.Text = dataItems[0]["SEG3"].Text;
            RTB_ItemCode_TextChanged(sender, e);
           
        }
        protected void RTB_ItemCode_TextChanged(object sender, EventArgs e)
        {
            string ItemCode1 = RTB_ItemCode.Text.Trim();
            lblMSG.Visible = false;

            string strSQL = " select * from GetCommItem_T_Item where seg3 = '" + ItemCode1 + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            if (dt.Rows.Count > 0)
            {
                string Seg5 = dt.Rows[0]["Seg5"].ToString().Substring(0, 4);
                RTB_Material_Mark.Text = dt.Rows[0]["SEG13"].ToString();
                switch (dt.Rows[0]["Seg5"].ToString().Substring(0, 4))
                {
                    case "YY01":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg21"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY02":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg15"].ToString();                        
                        break;
                    case "YY03":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = dt.Rows[0]["Seg16"].ToString();
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg15"].ToString();
                        break;
                    case "YY04":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY05":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY06":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY07":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg20"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY08":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY09":
                        if (dt.Rows[0]["Seg10"].ToString().ToUpper() == "Y")
                        { lblMSG.Text = "已失效！"; }
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        break;
                    default:
                        RTB_Material_Name.Text = "";
                        RTB_Mat_Unit.Text = "";
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = "";
                        break;
                }
            }
            else
            {
                lblMSG.Visible = true;
            }
        }

        protected void RB_Submit_Click(object sender, EventArgs e)
        {
            string deptCode = HF_DeptCode.Value.ToString();
            string Applicant = RTB_Applicant.Text.Trim();
            string ApplicationTime = RDP_ApplicationTime.SelectedDate.ToString();
            string ContactInformation = RTB_ContactInformation.Text.Trim();
            string TaskCode = RTB_TaskCode.Text.Trim();
            string DrawingNo = RTB_DrawingNo.Text.Trim();
            string Quantity = RTB_Quantity.Text.Trim();
            string PleaseTakeQuality = RTB_PleaseTakeQuality.Text.Trim();
            string TheMaterialWay = RDDL_TheMaterialWay.SelectedText.ToString();
            string FeedingTime = RDP_FeedingTime.SelectedDate.ToString();
            string IsDispatch = RB_IsDispatch.Checked.ToString();
            string IsConfirm = RB_IsConfirm.Checked.ToString();
            string Remark = RTB_Remark.Text.Trim();
            string ItemCode = RTB_ItemCode.Text.Trim();
            string Material_Name = RTB_Material_Name.Text.Trim();
            string Material_Mark = RTB_Material_Mark.Text.Trim();
            string CN_Material_State = RTB_CN_Material_State.Text.Trim();
            string Material_Tech_Condition = RTB_Material_Tech_Condition.Text.Trim();
            string Rough_Spec = RTB_Rough_Spec.Text.Trim();
            string Mat_Rough_Weight = RTB_Mat_Rough_Weight.Text.Trim();
            string Mat_Unit = RTB_Mat_Unit.Text.Trim();
            string Rough_Size = RTB_Rough_Size.Text.Trim();
            string DiaoDu = RDDL_DiaoDu.SelectedValue.ToString();
            string XingHao = RDDL_XingHao.SelectedValue.ToString();
            string WuZi = RDDL_WuZi.SelectedValue.ToString();

            if (lblMSG.Visible == true) { RadNotificationAlert.Text = "该物资编码不存在，不能提交"; RadNotificationAlert.Show(); return; }
            if (Applicant == "") { RadNotificationAlert.Text = "失败！请输入申请人"; RadNotificationAlert.Show(); return; }
            if (ApplicationTime == "") { RadNotificationAlert.Text = "失败！请输入申请时间"; RadNotificationAlert.Show(); return; }
            try { Convert.ToDateTime(ApplicationTime); }
            catch { RadNotificationAlert.Text = "失败！申请时间：不是有效日期"; RadNotificationAlert.Show(); return; }
            if (ContactInformation == "") { RadNotificationAlert.Text = "失败！请输入联系方式"; RadNotificationAlert.Show(); return; }
            if (TaskCode == "") { RadNotificationAlert.Text = "失败！请输入任务号"; RadNotificationAlert.Show(); return; }
            if (DrawingNo == "") { RadNotificationAlert.Text = "失败！请输入图号"; RadNotificationAlert.Show(); return; }
            if (Quantity == "") { RadNotificationAlert.Text = "失败！请输入申请件数"; RadNotificationAlert.Show(); return; }
            try { Convert.ToDouble(Quantity); }
            catch { RadNotificationAlert.Text = "失败！申请件数：请输入数字"; RadNotificationAlert.Show(); return; }
            if (PleaseTakeQuality != "")
            {
                try { Convert.ToDouble(PleaseTakeQuality); }
                catch { RadNotificationAlert.Text = "失败！申请数量：请输入数字"; RadNotificationAlert.Show(); return; } 
            }
            if (FeedingTime == "") { RadNotificationAlert.Text = "失败！请输入需求供货时间"; RadNotificationAlert.Show(); return; }
            try { Convert.ToDateTime(FeedingTime); }
            catch { { RadNotificationAlert.Text = "失败！需求供货时间：不是有效日期"; RadNotificationAlert.Show(); return; } }
            if (ItemCode == "") { RadNotificationAlert.Text = "失败！请输入物资编码"; RadNotificationAlert.Show(); return; }
            if (Material_Name == "") { RadNotificationAlert.Text = "失败！请输入物资名称"; RadNotificationAlert.Show(); return; }
            if (DiaoDu == "") { RadNotificationAlert.Text = "失败！请选择调度"; RadNotificationAlert.Show(); return; }
            if (XingHao == "") { RadNotificationAlert.Text = "失败！请输入型号计划员"; RadNotificationAlert.Show(); return; }
            if (WuZi == "") { RadNotificationAlert.Text = "失败！请输入物资计划员"; RadNotificationAlert.Show(); return; }
            
            string strSQL = "declare @id int";
            strSQL += " Insert into MaterialApplication (Type, Material_Id, Applicant, Dept, ApplicationTime, ContactInformation, TheMaterialWay, TaskCode, Drawing_No"
                + " , Draft_Code, Quantity, FeedingTime, IsDispatch, IsConfirm, Remark, MaterialType, Material_Name, Material_Mark, CN_Material_State, Material_Tech_Condition"
                + " , Rough_Spec, Mat_Rough_Weight, Mat_Unit, Rough_Size, PleaseTakeQuality, AppState, ReturnReason, Is_Del, ItemCode,DiaoDuApprove, XingHaoJiHuaYuanApprove, WuZiJiHuaYuanApprove)"
                + " values ('4',Null, '" + Applicant + "','" + deptCode + "','" + ApplicationTime + "','" + ContactInformation + "','" + TheMaterialWay + "','" + TaskCode + "','" + DrawingNo + "'"
                + " ,Null,'" + Quantity + "','" + FeedingTime + "','" + IsDispatch + "','" + IsConfirm + "','" + Remark + "'"
                + " ,Null,'" + Material_Name + "','" + Material_Mark + "','" + CN_Material_State + "','" + Material_Tech_Condition + "'"
                + " ,'" + Rough_Spec + "','" + Mat_Rough_Weight + "','" + Mat_Unit + "','" + Rough_Size + "','" + PleaseTakeQuality + "','1',Null,'false'"
                + " ,'" + ItemCode + "','" + DiaoDu + "','" + XingHao + "','" + WuZi + "') select @id = @@identity"
                + " Insert into MaterialApplication_Log (MaterialApplicationId, Operation_UserId, Operation_Time, Operation_Remark)"
                + " values (@id, '" + Session["UserId"].ToString() + "',GetDate(),'申请') select @id";

            string id = DBI.GetSingleValue(strSQL).ToString();
            
            K2BLL bll = new K2BLL();
            var result = bll.StartNewProcess(id);

            if (result == "")
            {
                strSQL = " Update MaterialApplication set AppState = '2' where ID = '" + id + "'";
                strSQL +=
                    " Insert into MaterialApplication_Log (MaterialApplicationId, Operation_UserID, Operation_Time, Operation_Remark)" +
                    " values('" + id + "','" + Session["UserId"].ToString() + "',GetDate() " +
                    " ,'进入流程平台:调度员：' + '" + DiaoDu + "' + '型号计划员：' + '" +
                    XingHao + "' +'物资计划员：' + '" +WuZi + "')";
                DBI.Execute(strSQL);
                RadNotificationAlert.Text = "申请成功！进入流程平台";
                RadNotificationAlert.Show();
                Clear();     
            }
            else
            {
                var db = new MMSDbDataContext();
                var ma = db.MaterialApplication.SingleOrDefault(p => p.Id.ToString() == id);
               // ma.Is_Del = true;
                db.SubmitChanges();
                RadNotificationAlert.Text = "提交失败！<br />" + result;
                RadNotificationAlert.Show();
            }
        }

        protected void RB_Save_Click(object sender, EventArgs e)
        {
            string deptCode = HF_DeptCode.Value.ToString();
            string Applicant = RTB_Applicant.Text.Trim();
            string ApplicationTime = RDP_ApplicationTime.SelectedDate.ToString();
            string ContactInformation = RTB_ContactInformation.Text.Trim();
            string TaskCode = RTB_TaskCode.Text.Trim();
            string DrawingNo = RTB_DrawingNo.Text.Trim();
            string Quantity = RTB_Quantity.Text.Trim();
            string PleaseTakeQuality = RTB_PleaseTakeQuality.Text.Trim();
            string TheMaterialWay = RDDL_TheMaterialWay.SelectedText.ToString();
            string FeedingTime = RDP_FeedingTime.SelectedDate.ToString();
            string IsDispatch = RB_IsDispatch.Checked.ToString();
            string IsConfirm = RB_IsConfirm.Checked.ToString();
            string Remark = RTB_Remark.Text.Trim();
            string ItemCode = RTB_ItemCode.Text.Trim();
            string Material_Name = RTB_Material_Name.Text.Trim();
            string Material_Mark = RTB_Material_Mark.Text.Trim();
            string CN_Material_State = RTB_CN_Material_State.Text.Trim();
            string Material_Tech_Condition = RTB_Material_Tech_Condition.Text.Trim();
            string Rough_Spec = RTB_Rough_Spec.Text.Trim();
            string Mat_Rough_Weight = RTB_Mat_Rough_Weight.Text.Trim();
            string Mat_Unit = RTB_Mat_Unit.Text.Trim();
            string Rough_Size = RTB_Rough_Size.Text.Trim();
            string DiaoDu = RDDL_DiaoDu.SelectedValue.ToString();
            string XingHao = RDDL_XingHao.SelectedValue.ToString();
            string WuZi = RDDL_WuZi.SelectedValue.ToString();

            if (lblMSG.Visible == true) { RadNotificationAlert.Text = "该物资编码不存在，不能保存"; RadNotificationAlert.Show(); return; }
            if (Applicant == "") { RadNotificationAlert.Text = "失败！请输入申请人"; RadNotificationAlert.Show(); return; }
            if (ApplicationTime == "") { RadNotificationAlert.Text = "失败！请输入申请时间"; RadNotificationAlert.Show(); return; }
            try { Convert.ToDateTime(ApplicationTime); }
            catch { RadNotificationAlert.Text = "失败！申请时间：不是有效日期"; RadNotificationAlert.Show(); return; }
            if (ContactInformation == "") { RadNotificationAlert.Text = "失败！请输入联系方式"; RadNotificationAlert.Show(); return; }
            if (TaskCode == "") { RadNotificationAlert.Text = "失败！请输入任务号"; RadNotificationAlert.Show(); return; }
            if (DrawingNo == "") { RadNotificationAlert.Text = "失败！请输入图号"; RadNotificationAlert.Show(); return; }
            if (Quantity == "") { RadNotificationAlert.Text = "失败！请输入申请件数"; RadNotificationAlert.Show(); return; }
            try { Convert.ToDouble(Quantity); }
            catch { RadNotificationAlert.Text = "失败！申请件数：请输入数字"; RadNotificationAlert.Show(); return; }
            if (PleaseTakeQuality != "")
            {
                try { Convert.ToDouble(PleaseTakeQuality); }
                catch { RadNotificationAlert.Text = "失败！申请数量：请输入数字"; RadNotificationAlert.Show(); return; }
            }
            if (FeedingTime == "") { RadNotificationAlert.Text = "失败！请输入需求供货时间"; RadNotificationAlert.Show(); return; }
            try { Convert.ToDateTime(FeedingTime); }
            catch { { RadNotificationAlert.Text = "失败！需求供货时间：不是有效日期"; RadNotificationAlert.Show(); return; } }
            if (ItemCode == "") { RadNotificationAlert.Text = "失败！请输入物资编码"; RadNotificationAlert.Show(); return; }
            if (Material_Name == "") { RadNotificationAlert.Text = "失败！请输入物资名称"; RadNotificationAlert.Show(); return; }
            if (DiaoDu == "") { RadNotificationAlert.Text = "失败！请选择调度"; RadNotificationAlert.Show(); return; }
            if (XingHao == "") { RadNotificationAlert.Text = "失败！请输入型号计划员"; RadNotificationAlert.Show(); return; }
            if (WuZi == "") { RadNotificationAlert.Text = "失败！请输入物资计划员"; RadNotificationAlert.Show(); return; }

            try
            {
            string strSQL = "declare @id int";
            strSQL += " Insert into MaterialApplication (Type, Material_Id, Applicant, Dept, ApplicationTime, ContactInformation, TheMaterialWay, TaskCode, Drawing_No"
                + " , Draft_Code, Quantity, FeedingTime, IsDispatch, IsConfirm, Remark, MaterialType, Material_Name, Material_Mark, CN_Material_State, Material_Tech_Condition"
                + " , Rough_Spec, Mat_Rough_Weight, Mat_Unit, Rough_Size, PleaseTakeQuality, AppState, ReturnReason, Is_Del, ItemCode,DiaoDuApprove, XingHaoJiHuaYuanApprove, WuZiJiHuaYuanApprove)"
                + " values ('4',Null, '" + Applicant + "','" + deptCode + "','" + ApplicationTime + "','" + ContactInformation + "','" + TheMaterialWay + "','" + TaskCode + "','" + DrawingNo + "'"
                + " ,Null,'" + Quantity + "','" + FeedingTime + "','" + IsDispatch + "','" + IsConfirm + "','" + Remark + "'"
                + " ,Null,'" + Material_Name + "','" + Material_Mark + "','" + CN_Material_State + "','" + Material_Tech_Condition + "'"
                + " ,'" + Rough_Spec + "','" + Mat_Rough_Weight + "','" + Mat_Unit + "','" + Rough_Size + "','" + PleaseTakeQuality + "','1',Null,'false'"
                + " ,'" + ItemCode + "','" + DiaoDu + "','" + XingHao + "','" + WuZi + "') select @id = @@identity"
                + " Insert into MaterialApplication_Log (MaterialApplicationId, Operation_UserId, Operation_Time, Operation_Remark)"
                + " values (@id, '" + Session["UserId"].ToString() + "',GetDate(),'编辑') select @id";

                RadNotificationAlert.Text = "保存数据成功!,";
                RadNotificationAlert.Show();

            }
            catch (Exception ex)
            {

                RadNotificationAlert.Text = "保存数据失败!" + ex.Message.ToString(); 
                RadNotificationAlert.Show();
            }
            
        }
        protected void RB_Clear_Click(object sender, EventArgs e)
        {
            Clear();              
        }

        protected void Clear()
        {
            RTB_Applicant.Text = "";
            RDP_ApplicationTime.SelectedDate = DateTime.Today;
            RTB_ContactInformation.Text = "";
            RTB_TaskCode.Text = "";
            RTB_DrawingNo.Text = "";
            RTB_Quantity.Text = "";
            RDDL_TheMaterialWay.SelectedIndex = 0;
            RDP_FeedingTime.SelectedDate = null;
            RTB_PleaseTakeQuality.Text = "";
            RB_IsDispatch.Checked = false;
            RB_IsConfirm.Checked = true;
            RTB_Remark.Text = "";
            RTB_ItemCode.Text = "";
            RTB_Material_Name.Text = "";
            RTB_Material_Mark.Text = "";
            RTB_CN_Material_State.Text = "";
            RTB_Material_Tech_Condition.Text = "";
            RTB_Rough_Spec.Text = "";
            RTB_Mat_Rough_Weight.Text = "";
            RTB_Mat_Unit.Text = "";
            RTB_Rough_Size.Text = "";
            RDDL_DiaoDu.SelectedIndex = 0;
            RDDL_XingHao.SelectedIndex = 0;
            RDDL_WuZi.SelectedIndex = 0;

            lblMSG.Visible = false;
        }

        #region 物资编码查询
        protected void RDDLMT_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            string value = RDDLMT.SelectedValue.ToString();
            string prefix = RDDLMT.SelectedText.ToString() + ".";
            string strSQL = "";

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
                strSQL = " select '' as Seg6 union select dbo.Get_StrArrayStrOfIndex(Seg6,'.',2) as Seg6"
                    + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%'";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
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
            }
        }

        protected void RDDLMT1_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string prefix = RDDLMT.SelectedText.ToString() + "." + RDDLMT1.SelectedText.ToString() + ".";
            string value = RDDLMT1.SelectedValue.ToString();
            string strSQL = "";

            RDDLMT2.SelectedIndex = 0;
            RDDLMT3.SelectedIndex = 0;
            RDDLMT4.SelectedIndex = 0;
            RDDLMT2.Items.Clear();
            RDDLMT3.Items.Clear();
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                strSQL = " select '' as Seg6 union select dbo.Get_StrArrayStrOfIndex(Seg6,'.',3) as Seg6"
                    + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%'";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
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
            string strSQL = "";

            RDDLMT3.SelectedIndex = 0;
            RDDLMT4.SelectedIndex = 0;
            RDDLMT3.Items.Clear();
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                strSQL = " select '' as Seg6 union select dbo.Get_StrArrayStrOfIndex(Seg6,'.',4) as Seg6"
                    + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%'";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
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
            string strSQL = "";

            RDDLMT4.SelectedIndex = 0;
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                strSQL = " select '' as Seg6 union select dbo.Get_StrArrayStrOfIndex(Seg6,'.',5) as Seg6"
                    + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%'";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
                RDDLMT4.DataSource = dt;
                RDDLMT4.DataTextField = "Seg6";
                RDDLMT4.DataValueField = "RowsId";
                RDDLMT4.DataBind();
            }
        }

        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string strSQL = "select * from GetCommItem_T_Item where SEG10 = 'N'";
            string Material_Name = RTB_Material_Name1.Text.Trim();      
            string Material_Paihao = RTB_Material_Paihao.Text.Trim();
            string Material_Guige = RTB_Material_Guige.Text.Trim();
            string Material_Biaozhun = RTB_Material_Biaozhun.Text.Trim();
            strSQL += " and SEG12 like '%" + Material_Name + "%'";
            strSQL += " and SEG13 like '%" + Material_Paihao + "%'";

            strSQL += " and SEG14 like '%" + Material_Guige + "%'";

            strSQL += " and SEG16 like '%" + Material_Biaozhun + "%'";
            string MTv = RDDLMT.SelectedValue.ToString();
            if (MTv == "")
            {

            }
            else if (MTv == "ItemCode")
            {
                string ItemCode = RTB_ItemCode.Text.Trim();
                strSQL += " and SEG3 like '%" + ItemCode + "%'"; ;
            }
            else
            {
                string MT = RDDLMT.SelectedText.ToString();
                string MT1 = RDDLMT1.SelectedText.ToString();
                string MT2 = RDDLMT2.SelectedText.ToString();
                string MT3 = RDDLMT3.SelectedText.ToString();
                string MT4 = RDDLMT4.SelectedText.ToString();

                string SEG6 = "";
                if (MT != "") { SEG6 += MT; }
                if (MT1 != "") { SEG6 += "." + MT1; }
                if (MT2 != "") { SEG6 += "." + MT2; }
                if (MT3 != "") { SEG6 += "." + MT3; }
                if (MT4 != "") { SEG6 += "." + MT4; }
                strSQL += " and SEG6 like '" + SEG6 + "%'";
            }
            Session["gds"] = DBI.Execute(strSQL, true);
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = (Session["gds"] as DataTable);
        }
        #endregion

    }
}