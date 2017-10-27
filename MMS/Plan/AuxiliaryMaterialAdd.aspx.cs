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

namespace mms.Plan
{
    public partial class AuxiliaryMaterialAdd : System.Web.UI.Page
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
                if (Request.QueryString["MDPId"] != null && Request.QueryString["MDPId"].ToString() != "")
                {
                    this.hfBh.Value = Request.QueryString["MDPId"].ToString();
                    RadBtnSubmit.Visible = true;
                    Get_M_Technology_ApplyByMDPId(Request.QueryString["MDPId"].ToString());
                }
                else
                {
                    RadBtnSubmit.Visible = false;
                    txt_TaskCode.Enabled = true;
                    //txt_Drawing_No.Enabled = true;
                }
                GridSource = Common.AddTableRowsID(GetAuxiliaryMaterialList(this.hfBh.Value));
                BindDDlDept();
                this.span_apply_time.InnerText = DateTime.Now.ToString("yyyy-MM-dd"); //Get_Sys_DeptEnumByUID();
                DemandDate.SelectedDate = DateTime.Now;
                this.ViewState["lastSelectDeptCode"] = "";
                this.ViewState["lastSelectAccount"] = "";
            }
        }
        protected void Get_Sys_DeptEnumByUID(string Dept)
        {
            try
            {
                //int userid = Convert.ToInt32(Session["UserId"].ToString());
                //string UserName = Session["UserName"].ToString();
                string strSQL = "select * from V_Get_Sys_DeptEnumByUID where id='" + Dept + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count > 0)
                {
                    //this.span_proposer.InnerText = UserName;
                    //this.span_dept.InnerText = dt.Rows[0][0].ToString();
                    Session["DeptCode"] = dt.Rows[0][1].ToString();
                    this.span_MaterialDept.InnerText = dt.Rows[0][0].ToString();
                    this.span_Shipping_Address.InnerText = dt.Rows[0][2].ToString();
                    Session["Shipping_Addr"] = dt.Rows[0][3].ToString();
                    //this.span_apply_time.InnerText = DateTime.Now.ToString("yyyy-MM-dd");

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
                    //this.txt_Drawing_No.Text = dt.Rows[0]["Drawing_No"].ToString();
                    txt_TaskCode.Enabled = false;
                    //txt_Drawing_No.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取任务号出错" + ex.Message.ToString());
            }
        }
        private void SaveAuxiliaryMaterialInfo()
        {
            M_Demand_Merge_List mta = new M_Demand_Merge_List();
            mta.Drawing_No = txt_Drawing_No.Text;
            mta.TaskCode = txt_TaskCode.Text;
            mta.DemandDate = Convert.ToDateTime(DemandDate.SelectedDate);
            mta.MaterialDept = Session["DeptCode"].ToString();
            mta.ItemCode1 = txt_ItemCode1.Text;
            mta.NumCasesSum = Convert.ToDecimal(txt_NumCasesSum.Text);
            mta.DemandNumSum = 0;
            mta.Mat_Unit = txt_Mat_Unit.Text;//span_Mat_Unit.InnerText;
            mta.Quantity = Convert.ToInt32(txt_NumCasesSum.Text);//txt_Quantity
            mta.Rough_Size = txt_Rough_Size.Text;//span_Rough_Size.InnerText;
            mta.Rough_Spec = txt_Rough_Spec.Text;//span_Rough_Spec.InnerText;
            mta.Special_Needs = Convert.ToInt32(RadComboBoxSpecialNeeds.SelectedValue);
            mta.Urgency_Degre = Convert.ToInt32(RadComboBoxUrgencyDegre.SelectedValue);
            mta.Secret_Level = Convert.ToInt32(RadComboBoxSecretLevel.SelectedValue);
            mta.Stage = Convert.ToInt32(RadComboBoxStage.SelectedValue);
            mta.Use_Des = Convert.ToInt32(RadComboBoxUseDes.SelectedValue);
            mta.Shipping_Address = Session["Shipping_Addr"].ToString();
            mta.Certification = Convert.ToInt32(RadComboBoxCertification.SelectedValue);
            mta.Unit_Price = Convert.ToDecimal(span_Unit_Price.InnerText);
            mta.Sum_Price = Convert.ToDecimal(span_Sum_Price.InnerText);
            mta.Submit_Type = 3;//1－工艺试验件；2－技术创新课题；3-生产备料
            SaveTechnologyNoSubmit(mta);
            txt_ItemCode1.Text = "";
            txt_Drawing_No.Text = "";
            txt_NumCasesSum.Text = "";
            txt_DemandNumSum.Text = "";
            txt_Mat_Unit.Text = "";//span_Mat_Unit.InnerHtml = "";
            //txt_Quantity.Text = "";
            txt_Rough_Size.Text = "";//span_Rough_Size.InnerHtml = "";
            txt_Rough_Spec.Text = "";//span_Rough_Spec.InnerHtml = "";
            span_Unit_Price.InnerHtml = "";
            span_Sum_Price.InnerHtml = "";
            DemandDate.SelectedDate = DateTime.Now;
        }
        protected void RadBtnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string err = string.Empty;
                bool flag = true;
                if (string.IsNullOrEmpty(span_Shipping_Address.InnerText) || span_Shipping_Address.InnerText == "")
                {
                    flag = false; err = "“配送地址”不得为空！";
                }
                if (string.IsNullOrEmpty(span_MaterialDept.InnerText) || span_MaterialDept.InnerText == "")
                {
                    flag = false; err = "“领料部门”不得为空！";
                }
                if (!PublicFunClass.ValidIsNotDecimal(span_Sum_Price.InnerText) || span_Sum_Price.InnerText == "")
                {
                    flag = false; err = "“总价”不得为空！";
                }
                if (!PublicFunClass.ValidIsNotDecimal(span_Unit_Price.InnerText) || span_Unit_Price.InnerText == "")
                {
                    flag = false; err = "“单价”不得为空！";
                }
                //if (string.IsNullOrEmpty(span_Rough_Spec.InnerText) || span_Rough_Spec.InnerText == "")
                //{
                //    flag = false; err = "“坯料规格”不得为空！";
                //}
                //if (string.IsNullOrEmpty(span_Rough_Size.InnerText) || span_Rough_Size.InnerText == "")
                //{
                //    flag = false; err = "“物资尺寸”不得为空！";
                //}
                //if (string.IsNullOrEmpty(span_Mat_Unit.InnerText) || span_Mat_Unit.InnerText == "")
                //{
                //    flag = false; err = "“计量单位”不得为空！";
                //}
                if (Convert.ToInt32(txt_DemandNumSum.Text) == 0)
                {
                    flag = false; err = "“共计需求量”必须大于0！";
                }
                if (Convert.ToInt32(txt_NumCasesSum.Text) == 0)
                {
                    flag = false; err = "“共计需求件数”必须大于0！";
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
                if (!flag)
                {
                    RadNotificationAlert.Text = err;
                    RadNotificationAlert.Show();
                }
                else
                {
                    SaveAuxiliaryMaterialInfo();
                }
            }
        }
        protected void SaveTechnologyNoSubmit(M_Demand_Merge_List mta)
        {
            DBI.OpenConnection();
            try
            {
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = "";
                int MDPId = 0;
                if (this.hfBh.Value != null && this.hfBh.Value != "")
                {
                    mta.MDPId = MDPId = Convert.ToInt32(this.hfBh.Value);
                }
                else
                {
                    strSQL = @"exec Proc_Add_M_Demand_Plan_List_Technology " + userid + ",'" + mta.TaskCode + "',0,0,3,0,''";//辅料
                    DataTable dt = DBI.Execute(strSQL, true);
                    if (dt.Rows.Count == 1)
                    {
                        mta.MDPId = MDPId = Convert.ToInt32(dt.Rows[0][0].ToString());
                        this.hfBh.Value = dt.Rows[0][0].ToString();
                    }
                }
                strSQL = @"exec Proc_Save_Technology_Apply_NoSubmit '" + mta.MDPId + "','" + mta.Drawing_No + "','" + mta.TaskCode + "','" +
                    mta.MaterialDept + "','" + mta.ItemCode1 + "','" + mta.DemandNumSum + "','" + mta.NumCasesSum + "','" +
                    mta.Mat_Unit + "','" + mta.Quantity + "','" + mta.Rough_Size + "','" + mta.Rough_Spec + "','" +
                    mta.DemandDate + "','" + mta.Special_Needs + "','" + mta.Urgency_Degre + "','" + mta.Secret_Level + "','" +
                    mta.Stage + "','" + mta.Use_Des + "','" + mta.Shipping_Address + "','" + mta.Certification + "','" +
                    mta.Unit_Price + "','" + mta.Sum_Price + "','" + mta.Submit_Type + "','" + userid + "'";
                DBI.Execute(strSQL);
                RadBtnSubmit.Visible = true;
                RadNotificationAlert.Text = "保存成功！";
                RadNotificationAlert.Show();
                GridSource = Common.AddTableRowsID(GetAuxiliaryMaterialList(mta.MDPId.ToString()));
                RadGrid_AuxiliaryMaterialList.Rebind();
                Get_M_Technology_ApplyByMDPId(MDPId.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资清单信息出错" + ex.Message.ToString());
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
            public string Rough_Spec { get; set; }
            public string MaterialsDes { get; set; }
            public int Special_Needs { get; set; }
            public int Urgency_Degre { get; set; }
            public int Secret_Level { get; set; }
            public int Stage { get; set; }
            public int Use_Des { get; set; }
            public string Shipping_Address { get; set; }
            public int Certification { get; set; }
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
        }
        protected DataTable GetAuxiliaryMaterialList(string MDPId)
        {
            try
            {
                DataTable dt = new DataTable();
                if (MDPId != "" && MDPId != null)
                {
                    int userid = Convert.ToInt32(Session["UserId"].ToString());
                    string strSQL = "select * from V_M_Technology_Apply where Submit_Type=3 and Is_Submit=0 and User_ID=" + userid + " and MDPId=" + MDPId;//3－辅料
                    dt = DBI.Execute(strSQL, true);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资清单信息出错" + ex.Message.ToString());
            }
        }

        protected void RadGrid_AuxiliaryMaterialList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_AuxiliaryMaterialList.DataSource = GridSource;
        }
        protected void RadGrid_AuxiliaryMaterialList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            DataTable table = GridSource;
            GridDataItem dataitem = e.Item as GridDataItem;
            if (e.CommandName == "delete")
            {
                string ID = dataitem.GetDataKeyValue("ID").ToString();
                string MDPId = table.Rows[e.Item.DataSetIndex]["MDPId"].ToString();
                DeleteTechnology(MDPId, ID);
                GridSource = Common.AddTableRowsID(GetAuxiliaryMaterialList(MDPId));
                RadGrid_AuxiliaryMaterialList.DataSource = GridSource;
            }
        }
        protected void DeleteTechnology(string MDPId, string ID)
        {
            DBI.OpenConnection();
            try
            {
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = @"exec Proc_DeleteTechnologyNoSubmit " + MDPId + "," + ID + "," + userid;
                DataTable dt = DBI.Execute(strSQL,true);
                if (dt.Rows.Count == 0) {
                    this.hfBh.Value = "";
                    txt_TaskCode.Text = "";
                    txt_TaskCode.Enabled = true;
                }
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
                ModifyTechnologySubmit(this.hfBh.Value);
                RadNotificationAlert.Text = "提交成功！";
                RadNotificationAlert.Show();
                //Response.Redirect("~/Plan/AuxiliaryMaterialList.aspx");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "closeWindow();", true);
            }
        }
        protected void ModifyTechnologySubmit(string MDPId)
        {
            DBI.OpenConnection();
            try
            {
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

        protected void btn_ItemCodeOK_Click(object sender, EventArgs e)
        {
            string[] arr = { "kg", "尺寸", "规格", "100.00" };
            //decimal sum_price = Convert.ToDecimal(arr[3]) * Convert.ToDecimal(txt_NumCasesSum.Text);
            //span_Mat_Unit.InnerHtml = arr[0];
            //span_Rough_Size.InnerHtml = arr[1];
            //span_Rough_Spec.InnerHtml = arr[2];
            //span_Unit_Price.InnerHtml = arr[3];
            txt_Mat_Unit.Text = arr[0];
            txt_Rough_Size.Text = arr[1];
            txt_Rough_Spec.Text = arr[2];
            span_Unit_Price.InnerHtml = arr[3];
            //span_Sum_Price.InnerHtml = sum_price.ToString();
        }
        private void BindDDlDept()
        {
            string strSQL = "SELECT * FROM [Sys_DeptEnum] WHERE ([Is_Del] = 0)";
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
            RadComboBox_User.DataTextField = "UserAccount";
            RadComboBox_User.DataValueField = "Id";
            RadComboBox_User.DataBind();
        }
        protected void RadComboBox_Dept_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (RadComboBox_Dept.SelectedValue != "0")
            {
                BindDDlUserInfo(RadComboBox_Dept.SelectedValue);
                Get_Sys_DeptEnumByUID(RadComboBox_Dept.SelectedValue);
            }
        }

        //protected void txt_NumCasesSum_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        decimal sum_price = Convert.ToDecimal(span_Unit_Price.InnerText) * Convert.ToDecimal(txt_NumCasesSum.Text);
        //        span_Sum_Price.InnerHtml = sum_price.ToString();
        //    }
        //    catch
        //    {
        //    }
        //}
    }
}