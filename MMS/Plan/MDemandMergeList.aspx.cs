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
using System.Web.UI.WebControls.WebParts;

namespace mms.Plan
{
    public partial class MDemandMergeList : System.Web.UI.Page
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
                    InitTable.Columns.Add("Correspond_Draft_Code");
                    InitTable.Columns.Add("Drawing_No");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("TaskId");
                    InitTable.Columns.Add("DraftId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("TechnicsLine");
                    InitTable.Columns.Add("ItemCode1");
                    InitTable.Columns.Add("NumCasesSum");
                    InitTable.Columns.Add("DemandNumSum");
                    InitTable.Columns.Add("Mat_Unit");
                    InitTable.Columns.Add("Quantity");
                    InitTable.Columns.Add("Rough_Size");
                    InitTable.Columns.Add("Dinge_Size");
                    InitTable.Columns.Add("Rough_Spec");
                    InitTable.Columns.Add("MaterialsDes");
                    InitTable.Columns.Add("Special_Needs");
                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Stage");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Unit_Price");
                    InitTable.Columns.Add("Sum_Price");
                    InitTable.Columns.Add("Is_Submit");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("Submit_Date");
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
        private DataTable GridSource1
        {
            get
            {
                Object obj = this.ViewState["_gds1"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                else
                {
                    DataTable InitTable = new DataTable();
                    InitTable.Columns.Add("RowsId");
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("Change_Code");
                    InitTable.Columns.Add("Change_Evidence_Id");
                    InitTable.Columns.Add("Correspond_Draft_Code");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("Change_Date");
                    InitTable.Columns.Add("Change_State");
                    InitTable.Columns.Add("MReduce_Num");
                    InitTable.Columns.Add("User_ID");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["ID"] };       //设置RowsId列为主键，用于datatable删除
                    this.ViewState["_gds1"] = InitTable;
                    return InitTable;
                }
            }
            set
            {
                this.ViewState["_gds1"] = value;
                ((DataTable)this.ViewState["_gds1"]).PrimaryKey = new DataColumn[] { ((DataTable)this.ViewState["_gds1"]).Columns["ID"] };
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
                RDP_DemandDate.SelectedDate = DateTime.Today.AddMonths(3);

                string idStr=string.Empty;
                string dateStr = string.Empty;
                string[] otherStr=new string[4];
                if (Session["idStr"] != null && Session["idStr"].ToString() != "")
                {
                    if (Session["idStr"].ToString().Substring(0, 1) == "," && Session["idStr"].ToString().Length > 2)
                    {
                        Session["idStr"] = Session["idStr"].ToString().Substring(1, Session["idStr"].ToString().Length - 2);
                    }
                    idStr = Session["idStr"].ToString();                  
                    otherStr = Session["otherStr"].ToString().Split(new char[]{','});
                    string PackId = otherStr[0];
                    string DraftId = otherStr[1];
                    this.span_listNo.InnerText = otherStr[3];
                    this.span_model.InnerText = otherStr[2];
                    GridSource = Common.AddTableRowsID(GetTempMergeList(idStr, PackId, DraftId, dateStr));
                    GridSource1 = Common.AddTableRowsID(GetChangeRecord(idStr));
                }
                else {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "CloseWindow();", true);
                }
            }
        }

        protected DataTable GetTempMergeList(string idStr, string PackId, string DraftId, string dateStr)
        {
            try
            {
                string strSQL = @"exec Proc_Build_Merge_List '" + idStr + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }
        protected DataTable GetChangeRecord(string idStr)
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = @"exec Proc_Build_Change_List1 '" + idStr + "'";
           
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count > 0)
                {
                    this.divListTitle.Visible = true;
                    this.divListContent.Visible = true;
                }
                else {
                    this.divListTitle.Visible = false;
                    this.divListContent.Visible = false;
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("保存物资需求清单详表数据出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_MDemandMergelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MDemandMergelist.DataSource = GridSource;
        }
        protected void RadGrid_ChangeRecord_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_ChangeRecord.DataSource = GridSource1;
        }
        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            ((sender as RadButton).NamingContainer as GridItem).Selected = (sender as RadButton).Checked;
        }
        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            RadButton headerCheckBox = (sender as RadButton);
            foreach (GridDataItem dataItem in RadGrid_MDemandMergelist.MasterTableView.Items)
            {
                (dataItem.FindControl("RadButtonItem") as RadButton).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }
        
      

        protected class M_Draft_ListBody
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
            public string DemandDate { get; set; }
            public string Rough_Size { get; set; }
            public string Dinge_Size { get; set; }
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
        }

        protected void RB_Submit_Click(object sender, EventArgs e)
        {
            if (RDP_DemandDate.SelectedDate == null)
            {
                RadNotificationAlert.Text = "请选择需求时间！";
                RadNotificationAlert.Show();
                return;
            }
            RB_Submit.Visible = false;
            string MDPLID = SaveMergeInfo();
            try
            {
                if (MDPLID == "" || MDPLID == null)
                {
                    RadNotificationAlert.Text = "系统超时，请重新登录";
                    RadNotificationAlert.Show();

                }
                else
                {
                    Convert.ToDouble(MDPLID);
                    Response.Redirect("/Plan/MDemandMergeListState.aspx?MDPID=" + MDPLID);
                }
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = MDPLID + "<br />" + ex.Message;
                RadNotificationAlert.Show();
                RB_Submit.Enabled = true;
            }
        }

        private string SaveMergeInfo() {
            string MDPLID = "";
            string NeedsStr = "", DegreStr = "", LevelStr = "", DesStr = "", AddrStr = "", CertStr = "", ManufacturerStr = "", DemandDateStr = "";
           
            string[] otherStr = new string[4];
            string idStr = string.Empty;
            if ((Session["idStr"] != null && Session["idStr"].ToString() != "") &&
                (Session["otherStr"] != null && Session["otherStr"].ToString() != ""))
            {
                idStr = Session["idStr"].ToString();
                otherStr = Session["otherStr"].ToString().Split(new char[] { ',' });
                string PackId = otherStr[0];
                string DraftId = otherStr[1];
                for(int j=0;j<GridSource.Rows.Count;j++)
                {
                    string Special_Needs = GridSource.Rows[j]["Special_Needs"].ToString();
                    string Urgency_Degre = GridSource.Rows[j]["Urgency_Degre"].ToString();
                    string Secret_Level = GridSource.Rows[j]["Secret_Level"].ToString();
                    string Use_Des = GridSource.Rows[j]["Use_Des"].ToString();
                    string Shipping_Address = GridSource.Rows[j]["Shipping_Addr_Id"].ToString();
                    string CertificationVal = GridSource.Rows[j]["Certification"].ToString();
                    string Manufacturer = GridSource.Rows[j]["Manufacturer"].ToString();
                    string DemandDate = "";
                    if (RDP_DemandDate.SelectedDate == null)
                    {
                        DemandDate = DateTime.Today.AddMonths(3).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        DemandDate = Convert.ToDateTime(RDP_DemandDate.SelectedDate).ToString("yyyy-MM-dd");
                    }
                   
                    NeedsStr = NeedsStr + Special_Needs + ",";
                    DegreStr = DegreStr + Urgency_Degre + ",";
                    LevelStr = LevelStr + Secret_Level + ",";
                    DesStr = DesStr + Use_Des + ",";
                    AddrStr = AddrStr + Shipping_Address + ",";
                    CertStr = CertStr + CertificationVal + ",";
                    ManufacturerStr = ManufacturerStr + Manufacturer + ",";
                    DemandDateStr = DemandDateStr + DemandDate + ",";                 
                }
                NeedsStr = NeedsStr.Substring(0, NeedsStr.Length - 1);
                DegreStr = DegreStr.Substring(0, DegreStr.Length - 1);
                LevelStr = LevelStr.Substring(0, LevelStr.Length - 1);
                DesStr = DesStr.Substring(0, DesStr.Length - 1);
                AddrStr = AddrStr.Substring(0, AddrStr.Length - 1);
                CertStr = CertStr.Substring(0, CertStr.Length - 1);
                ManufacturerStr = ManufacturerStr.Substring(0, ManufacturerStr.Length - 1);
                DemandDateStr = DemandDateStr.Substring(0, DemandDateStr.Length - 1);
                MDPLID = SaveMDemandPlanAndChange(idStr, NeedsStr, DegreStr, LevelStr, DesStr, AddrStr, CertStr, PackId, DraftId, ManufacturerStr, DemandDateStr);
            }
            return MDPLID;
        }
        protected string SaveMDemandPlanAndChange(string DraftIdStr, string NeedsStr, string DegreStr, string LevelStr,
          string DesStr, string AddrStr, string CertStr, string PackId, string DraftId, string ManufacturerStr, string DemandDateStr)
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserId"].ToString());

                string strSQL = @"exec Proc_Save_M_Demand_Plan_And_Change1 '" + DraftIdStr + "','" + NeedsStr + "','" +
                    DegreStr + "','" + LevelStr + "','" + DesStr + "','" + AddrStr + "','" + CertStr + "', '" +
                    ManufacturerStr + "','" + userid + "'," + PackId + "," + DraftId + ",'" + DemandDateStr + "'";

                DataTable dt = DBI.Execute(strSQL, true);
                
                string MDPLID = "";
                if (dt.Rows.Count > 0)
                {
                    MDPLID = dt.Rows[0][0].ToString();
                }
              
                LogisticsCenterBLL bll = new LogisticsCenterBLL();
                string result = bll.WriteReqOrderRepeat(MDPLID);

                //提交失败的M_Demand_DetailedList_Draft表的修改
                strSQL = " Update M_Demand_DetailedList_Draft set Material_State = case when " +
                         " (select isnull(sum(NumCasesSum),0) from M_Demand_Merge_List where Is_Submit = '1' and Correspond_Draft_Code = convert(nvarchar(50),M_Demand_DetailedList_Draft.ID)) > 0" +
                         " or (select isnull(sum(DemandNumSum),0) from M_Demand_Merge_List where Is_Submit = '1' and Correspond_Draft_Code = convert(nvarchar(50),M_Demand_DetailedList_Draft.ID)) > 0" +
                         " then '2' else '0' end" +
                         " where Convert(nvarchar(50),ID) in (select Correspond_Draft_Code from M_Demand_Merge_List where MDPId = '" +
                         MDPLID + "' and Is_Submit = 'false')";
                DBI.Execute(strSQL);

                bll.WriteRcoOrderRepeat(MDPLID);
                if (result != "")
                {
                    return result;
                }

                return MDPLID;
            }
            catch (Exception e)
            {
                throw new Exception("数据库操作-操作物资需求清单详表时出现异常" + e.Message.ToString());
            }
        }

        private void BindDDlShipping_Addr(RadComboBox rcb, string DeptCode)
        {
            string strSQL = "select KeyWord from Sys_Dict" +
                " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
                " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + DeptCode + "')";
            DataTable dt = DBI.Execute(strSQL, true);
            rcb.DataSource = dt;
            rcb.DataTextField = "KeyWord";
            rcb.DataValueField = "KeyWord";
            rcb.DataBind();
        }
        protected void RadGrid_MDemandMergelist_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                string MaterialDept = (e.Item as GridDataItem).GetDataKeyValue("MaterialDept").ToString();
                DataTable table = GridSource;
                RadComboBox rcbAddr = e.Item.FindControl("RadComboBoxShippingAddress") as RadComboBox;
             
              //  rcbAddr.CssClass = id;
                BindDDlShipping_Addr(rcbAddr, MaterialDept);
                if (rcbAddr.FindItemByText(GridSource.Select("ID='" + id + "'")[0]["Shipping_Addr_Id"].ToString()) != null)
                {
                    rcbAddr.FindItemByText(GridSource.Select("ID='" + id + "'")[0]["Shipping_Addr_Id"].ToString()).Selected = true;
                }
                RadTextBox rtbSpecialNeeds = e.Item.FindControl("rtb_SpecialNeeds") as RadTextBox;
               // rtbSpecialNeeds.CssClass = id;
                if (GridSource.Select("ID='" + id + "'")[0]["Special_Needs"].ToString() != null && GridSource.Select("ID='" + id + "'")[0]["Special_Needs"].ToString()!="")
                {
                    rtbSpecialNeeds.Text = (GridSource.Select("ID='" + id + "'")[0]["Special_Needs"].ToString());
                }
                else
                {
                    GridSource.Select("Id='" + id + "'")[0]["Special_Needs"] = "无";
                    rtbSpecialNeeds.Text = "无";
                }

               RadComboBox RadComboBoxUseDes = e.Item.FindControl("RadComboBoxUseDes") as RadComboBox;
              if (RadComboBoxUseDes.FindItemByText(GridSource.Select("ID='" + id + "'")[0]["Use_Des"].ToString()) != null)
              {
                  RadComboBoxUseDes.FindItemByText(GridSource.Select("ID='" + id + "'")[0]["Use_Des"].ToString()).Selected = true;
               }
               RadComboBox RadComboBoxSecretLevel = e.Item.FindControl("RadComboBoxSecretLevel") as RadComboBox;
               if (RadComboBoxSecretLevel.FindItemByText("内部")!=null)
               {
                  RadComboBoxSecretLevel.FindItemByText("内部").Selected = true;
               }
               RadComboBox RadComboBoxUrgencyDegre = e.Item.FindControl("RadComboBoxUrgencyDegre") as RadComboBox;
               if (RadComboBoxSecretLevel.FindItemByText("一般") != null)
               {
                   RadComboBoxUrgencyDegre.FindItemByText("一般").Selected = true;
               }
               RadComboBox RadComboBoxCertification = e.Item.FindControl("RadComboBoxCertification") as RadComboBox;
   
               if (RadComboBoxCertification.FindItemByText(GridSource.Select("ID='" + id + "'")[0]["Certification"].ToString()) != null)
               {
                   RadComboBoxCertification.FindItemByText(GridSource.Select("ID='" + id + "'")[0]["Certification"].ToString()).Selected = true;
               }
        
            }
        }

        protected void RadComboBoxUrgencyDegre_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource.Select("Id='" + id + "'")[0]["Urgency_Degre"] = cb.SelectedItem.Value;
        }

        protected void RadComboBoxSecretLevel_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource.Select("Id='" + id + "'")[0]["Secret_Level"] = cb.SelectedItem.Value;
        }

        protected void RadComboBoxUseDes_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource.Select("Id='" + id + "'")[0]["Use_Des"] = cb.SelectedItem.Value;
        }

        protected void RadComboBoxShippingAddress_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource.Select("Id='" + id + "'")[0]["Shipping_Addr_Id"] = cb.SelectedItem.Value;
        }

        protected void RadComboBoxCertification_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource.Select("Id='" + id + "'")[0]["Certification"] = cb.SelectedItem.Value;
        }

        protected void rtb_SpecialNeeds_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource.Select("Id='" + id + "'")[0]["Special_Needs"] = rtb.Text;
        }

        protected void RTB_MANUFACTURER_TextChanged(object sender, EventArgs e)
        {
            RadTextBox rtb = sender as RadTextBox;
            string id = (rtb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString();
            GridSource.Select("ID='" + id + "'")[0]["MANUFACTURER"] = rtb.Text;
        }
    }
}