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
    public partial class MaterialImport : System.Web.UI.Page
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
  
                GridSource1 = new System.Data.DataTable();
           

                    string title = "", title1 = "";

                    title = "型号物资需求申请";
                    title1 = "型号物资需求未提交申请";

                    if (Request.QueryString["MDPId"] != null && Request.QueryString["MDPId"].ToString() != "")
                    {
                        this.hfBh.Value = Request.QueryString["MDPId"].ToString();
                      //  RadBtnSubmit.Visible = true;
                        //Get_M_Technology_ApplyByMDPId(Request.QueryString["MDPId"].ToString());
                    }
                    else
                    {
                     //  RadBtnSubmit.Visible = false;
                    }
                  
                    this.ViewState["submit_type"] = "4";
                 
                    GridSource = Common.AddTableRowsID(GetTechnologyTestList(this.hfBh.Value, "4"));
                 

                   

               

                    Session["gds"] = null;
           

              
                    this.span_apply_time1.InnerText = DateTime.Now.ToString("yyyy-MM-dd"); 
                    BindDeptUserAddress1();

           
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
                        " , case when Stage ='1' then 'M' when stage='2' then 'C' when Stage='3' then 'S' when Stage='4' then 'D' else Convert(nvarchar(50),Stage) end as Stage1"+
                        " , M_Demand_Merge_List.*" +
                        " from M_Demand_Merge_List join M_Demand_Plan_List on M_Demand_Plan_List.Id = M_Demand_Merge_List.MDPId" +
                        " left join GetBasicdata_T_Item as CUX_DM_URGENCY_LEVEL on CUX_DM_URGENCY_LEVEL.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                        " left join GetBasicdata_T_Item as CUX_DM_USAGE on CUX_DM_USAGE.DICT_CODE = M_Demand_Merge_List.Use_Des and CUX_DM_USAGE.DICT_CLASS='CUX_DM_USAGE'" +
                        " left join GetCustInfo_T_ACCT_SITE on Convert(nvarchar(50),GetCustInfo_T_ACCT_SITE.LOCATION_ID) = M_Demand_Merge_List.Shipping_Address" +
                        " where MDPId = '" + MDPId + "'";
                    
                    dt = DBI.Execute(strSQL, true);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception((Submit_Type == "1" ? "获取工艺试验件清单信息出错" : (Submit_Type == "2" ? "获取技术创新课题清单信息出错" : "获取车间备料物资需求清单信息出错")) +
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
                GridSource = Common.AddTableRowsID(GetTechnologyTestList(MDPId, this.ViewState["submit_type"].ToString()));
                RadGrid_TechnologyTestList.DataSource = GridSource;
            }
        }
        protected void DeleteTechnology(string MDPId, string ID)
        {
            DBI.OpenConnection();
            try
            {
                if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
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
   #region 从EXCEL文件导入辅料需求信息

        protected void RadGrid_Importlist_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
                string userid = Session["UserId"].ToString();
                string strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                                " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                                " where Sys_UserInfo_PWD.ID = '" + userid + "'";
                DataTable dt = DBI.Execute(strSQL, true);

                RadComboBox RadComboBoxMaterialDept = e.Item.FindControl("RadComboBoxMaterialDept1") as RadComboBox;

                RadComboBoxMaterialDept.DataSource = dt;
                RadComboBoxMaterialDept.DataTextField = "Dept";
                RadComboBoxMaterialDept.DataValueField = "DeptCode";
                RadComboBoxMaterialDept.DataBind();

                RadComboBox RadComboBoxShipping_Address = e.Item.FindControl("RadComboBoxShippingAddress1") as RadComboBox;
                RadComboBoxShipping_Address.CssClass = id;
                string departCode="";
            /*    if( GridSource1.Select("ID='" + id + "'")[0]["DeptCode"]!=null)
                {
                    departCode= GridSource1.Select("ID='" + id + "'")[0]["DeptCode"].ToString();
                }
                else
                {
             * */

                    departCode=dt.Rows[0]["DeptCode"].ToString();
              //  }
         
                strSQL = "select KeyWord from Sys_Dict" +
             " join Sys_Dept_ShipAddr on Sys_Dept_ShipAddr.Shipping_Addr_Id = '2-'+ Convert(nvarchar(50),Sys_Dict.KeyWordCode)" +
             " where TypeID = '2' and Dept_Id = (select ID from Sys_DeptEnum where DeptCode = '" + departCode + "')";
                DataTable dtAddress = DBI.Execute(strSQL, true);
                RadComboBoxShipping_Address.DataSource = dtAddress;
                RadComboBoxShipping_Address.DataTextField = "KeyWord";
                RadComboBoxShipping_Address.DataValueField = "KeyWord";
                RadComboBoxShipping_Address.DataBind();

               
                RadComboBoxShipping_Address.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["Shipping_Address"].ToString()).Selected = true;
               
          

                 RadComboBox RadComboBoxSecretLevel = e.Item.FindControl("RadComboBoxSecretLevel1") as RadComboBox;
                 RadComboBoxSecretLevel.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["Secret_Level"].ToString()).Selected = true;

                 

                 RadComboBox RadComboBoxCertification = e.Item.FindControl("RadComboBoxCertification1") as RadComboBox;
                 RadComboBoxCertification.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["Certification"].ToString()).Selected = true;
             
                 RadDropDownList RDDL_Project = e.Item.FindControl("RDDL_Project1") as RadDropDownList;
              
                 RDDL_Project.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["Project"].ToString()).Selected = true;
              
                 RadComboBox RadComboBoxStage = e.Item.FindControl("RadComboBoxStage1") as RadComboBox;
                 RadComboBoxStage.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["stage"].ToString()).Selected = true;

                 RadComboBox RadComboBoxAttribute4 = e.Item.FindControl("RadComboBoxAttribute4") as RadComboBox;
                 RadComboBoxAttribute4.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["Attribute4"].ToString()).Selected = true;

                 RadComboBox RadComboBoxUrgencyDegre = e.Item.FindControl("RadComboBoxUrgencyDegre1") as RadComboBox;
                 RadComboBoxUrgencyDegre.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["Urgency_Degre"].ToString()).Selected = true;

                 RadComboBox RadComboBoxUseDes = e.Item.FindControl("RadComboBoxUseDes1") as RadComboBox;
                 RadComboBoxUseDes.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["Use_Des"].ToString()).Selected = true;
               
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
                          
                                case "产品图号":
                                    GridSource1.Columns[i].ColumnName = "DRAWING_NO";
                                    columnscount++;
                                    break;
                                case "任务号":
                                    GridSource1.Columns[i].ColumnName = "TaskCode";
                                    columnscount++;
                                    break;
                                case "物资编码":
                                    GridSource1.Columns[i].ColumnName = "ItemCode1";
                                    columnscount++;
                                    break;
                                /*   case "物资名称":
                                       GridSource1.Columns[i].ColumnName = "Material_Name";
                                       columnscount++;
                                       break;
                                   case "物资规格":
                                       GridSource1.Columns[i].ColumnName = "Rough_Spec";
                                       columnscount++;
                                       break;
                                  
                                   case "计量单位":
                                       GridSource1.Columns[i].ColumnName = "MAT_UNIT";
                                       columnscount++;
                                       break;
                                 */
                                case "单价":
                                    GridSource1.Columns[i].ColumnName = "Unit_Price";
                                    columnscount++;
                                    break;
                                case "共计需求件数":
                                       GridSource1.Columns[i].ColumnName = "NumCasesSum";
                                       columnscount++;
                                       break;
                                   case "共计需求数量":
                                       GridSource1.Columns[i].ColumnName = "DemandNumSum";
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

                                case "物资尺寸":
                                    GridSource1.Columns[i].ColumnName = "ROUGH_SIZE";
                                    columnscount++;
                                    break;

                    
                                case "特殊需求":
                                    GridSource1.Columns[i].ColumnName = "Special_Needs";
                                    columnscount++;
                                    break;

                                case "需求时间":
                                    GridSource1.Columns[i].ColumnName = "DemandDate";
                                    columnscount++;
                                    break;

                                case "型号工程":
                                    GridSource1.Columns[i].ColumnName = "Project";
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

                                case "国产/进口":
                                    GridSource1.Columns[i].ColumnName = "Attribute4";
                                    columnscount++;
                                    break;

                                case "配送地址":
                                    GridSource1.Columns[i].ColumnName = "Shipping_Address";
                                    columnscount++;
                                    break;


                        
                            
                            }
                        }
                        if (columnscount < 17)
                        {
                            GridSource1 = new System.Data.DataTable();
                            RadGridImport.Rebind();
                            File.Delete(filePath);
                            RadNotificationAlert.Text = "失败！请参照Excel模板页面表头";
                            RadNotificationAlert.Show();
                            return;
                        }

                        GridSource1.Columns.Add("ID");
                        GridSource1.Columns.Add("Material_Name");
                        GridSource1.Columns.Add("MAT_UNIT");
                        GridSource1.Columns.Add("Rough_Spec");
                      //  GridSource1.Columns.Add("Unit_Price");
                        GridSource1.Columns.Add("Sum_Price");
                        int rowsid = 1;
                        for (int i = 0; i < GridSource1.Rows.Count; i++)
                        {
                            string itemCode1 = GridSource1.Rows[i]["ItemCode1"].ToString();
                            if (GridSource1.Rows[i]["DRAWING_NO"].ToString() != "" && itemCode1!= "")
                            {
                                GridSource1.Rows[i]["ID"] = rowsid;
                                rowsid++;
                                Set_Txt_ByItemCode1(itemCode1,i);
                                try
                                {
                                  double unitPrice=  Convert.ToDouble(GridSource1.Rows[i]["Unit_Price"]);
                                  double demandnumSum=  Convert.ToDouble(GridSource1.Rows[i]["DemandNumSum"]);
                                    GridSource1.Rows[i]["Sum_Price"] =( unitPrice*demandnumSum).ToString();
                                }
                                catch { GridSource1.Rows[i]["Sum_Price"] = "0"; }
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

        protected void Set_Txt_ByItemCode1(string ItemCode1,int i)
        {
   
            string strSQL = " select * from GetCommItem_T_Item where seg3 = '" + ItemCode1 + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            if (dt.Rows.Count > 0)
            {
                string Seg5 = dt.Rows[0]["Seg5"].ToString().Substring(0, 4);
                switch (Seg5)
                {
                    case "YY01":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg21"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg13"].ToString();
                     //   GridSource1.Rows[i]["Unit_Price"] = dt.Rows[0]["Seg31"].ToString();
                        break;
                    case "YY02":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg15"].ToString();
                    //    GridSource1.Rows[i]["Unit_Price"] = dt.Rows[0]["Seg25"].ToString();
                        break;
                    case "YY03":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg15"].ToString();
                    //    GridSource1.Rows[i]["Unit_Price"] = "0";
                        GridSource1.Rows[i]["ROUGH_SIZE"] = dt.Rows[0]["Seg16"].ToString();
                        break;
                    case "YY04":
                    case "YY05":
                    case "YY06":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                    //    GridSource1.Rows[i]["Unit_Price"] = dt.Rows[0]["Seg23"].ToString();
                        break;
                    case "YY07":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg20"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                    //    GridSource1.Rows[i]["Unit_Price"] = dt.Rows[0]["Seg25"].ToString();
                        break;
                    case "YY08":
                    case "YY09":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                   //     GridSource1.Rows[i]["Unit_Price"] = dt.Rows[0]["Seg21"].ToString();
                        break;
                    default:
                        GridSource1.Rows[i]["Material_Name"] = "";
                        GridSource1.Rows[i]["MAT_UNIT"] = "";
                        GridSource1.Rows[i]["Rough_Spec"] = "";
                    //    GridSource1.Rows[i]["Unit_Price"] = "";
                        break;
                }
            }
            else
            {
                RadNotificationAlert.Text = "物资编码不存在";
                RadNotificationAlert.Show();
                GridSource1.Rows[i].Delete();
                return;
            }
        }


        protected void RBClear_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < RadGridImport.Items.Count; i++)
                {
                    GridSource1.Rows.RemoveAt(i);
                }
                RadGridImport.Rebind();
                HFGridItemsCount.Value = RadGridImport.Items.Count.ToString();

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
         

            try
            {
           
                string DBContractConn =
                    ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString
                        .ToString();
                DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
                if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = "";
                int MDPId = 0;
                int Submit_Type = Convert.ToInt32(this.ViewState["submit_type"].ToString());//1－工艺试验件；2－技术创新；3－辅料

          
                for (int i = 0; i < RadGridImport.Items.Count; i++)
                {
                    if (RadGridImport.Items[i] is GridDataItem)
                    {
                        GridDataItem item = RadGridImport.Items[i] as GridDataItem;

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
                        /*
                        if (Material_Name == "" || Material_Name == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，物资名称：请输入物资名称";
                            RadNotificationAlert.Show();
                            return;
                        }
                        */
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
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，物资尺寸：请输入物资尺寸";
                            RadNotificationAlert.Show();
                            return;
                        }

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
                            Convert.ToInt32(DemandNumSum);
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
                            Convert.ToInt32(Unit_Price);
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


                        RadComboBox RadComboBoxMaterialDept = item.FindControl("RadComboBoxMaterialDept1") as RadComboBox;
                        string MaterialDept = RadComboBoxMaterialDept.SelectedValue; 

           
                        RadComboBox RadComboBoxUrgency_Degre = item.FindControl("RadComboBoxUrgencyDegre1") as RadComboBox;
                        string Urgency_Degre = RadComboBoxUrgency_Degre.SelectedValue; 

                 
                        RadComboBox RadComboBoxSecret_Level = item.FindControl("RadComboBoxSecretLevel1") as RadComboBox;
                        string Secret_Level = RadComboBoxSecret_Level.SelectedValue; 

                  

                        RadComboBox RadComboBoxStage = item.FindControl("RadComboBoxStage1") as RadComboBox;
                        string Stage = RadComboBoxStage.SelectedValue; 

                        RadComboBox RadComboBoxUseDes = item.FindControl("RadComboBoxUseDes1") as RadComboBox;
                        string Use_Des = RadComboBoxUseDes.SelectedValue;

                        RadComboBox RadComboBoxShipping_Address = item.FindControl("RadComboBoxShippingAddress1") as RadComboBox;
                        string Shipping_Address = RadComboBoxShipping_Address.SelectedItem.Text; 

                   
                        RadComboBox RadComboBoxCertification = item.FindControl("RadComboBoxCertification1") as RadComboBox;
                        string Certification = RadComboBoxCertification.SelectedValue;


                        RadComboBox RadComboBoxAttribute4 = item.FindControl("RadComboBoxAttribute4") as RadComboBox;
                        string Attribute4 = RadComboBoxAttribute4.SelectedValue;
                        
                        RadDropDownList RadDropDownListProject = item.FindControl("RDDL_Project1") as RadDropDownList;
                        string Project = RadDropDownListProject.SelectedValue;
                 
                        M_Demand_Merge_List mta = new M_Demand_Merge_List();
                        mta.Drawing_No = DRAWING_NO;
                        mta.TaskCode = TaskCode;
                        mta.DemandDate = Convert.ToDateTime(DemandDate);
                        mta.ItemCode1 = ItemCode1;
                        mta.NumCasesSum = Convert.ToDecimal(NumCasesSum);
                        mta.DemandNumSum = Convert.ToDecimal(DemandNumSum);
                        mta.Mat_Unit = MAT_UNIT;
                        mta.Quantity = Convert.ToInt32(NumCasesSum);
                        mta.Rough_Size = ROUGH_SIZE;
                        mta.Rough_Spec = Rough_Spec;
                        mta.Special_Needs = Special_Needs;

                        mta.MaterialDept = MaterialDept;
                        mta.Urgency_Degre = Urgency_Degre;
                        mta.Secret_Level = Secret_Level;
                        mta.Stage = Stage;
                        mta.Use_Des = Use_Des;
                        mta.Shipping_Address = Shipping_Address;
                        mta.Certification = Certification;
                        mta.Project = Project;

                        
                         mta.Attribute4 = Attribute4;
                           
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
                                 mta.TaskCode + "','" +
                                 mta.MaterialDept + "','" + mta.ItemCode1 + "','" + mta.DemandNumSum + "','" + mta.NumCasesSum +
                                 "','" +
                                 mta.Mat_Unit + "','" + mta.Quantity + "','" + mta.Rough_Size + "','" + mta.Rough_Spec + "','" +
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
              //  RadBtnSubmit.Visible = true;
                RadNotificationAlert.Text = "导入成功！";
                RadNotificationAlert.Show();
           
                GridSource = Common.AddTableRowsID(GetTechnologyTestList(MDPId.ToString(), Submit_Type.ToString()));
                RadGrid_TechnologyTestList.Rebind();
           
             

            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                return;
            }
        }

  
        protected void btnDown_Click(object sender, EventArgs e)
        {
            int i = 0;
            DirectoryInfo info =
                new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Plan/需求导入模板");
            if (System.IO.Directory.Exists(info.ToString()))
            {
                foreach (FileInfo n in info.GetFiles())
                {
                    if (n.Name == "需求导入模板.xlsx")
                    {
                        i = 1;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(n.Name));
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                        Response.ContentType = "application/ms-excel";
                        this.EnableViewState = false;

                        Response.WriteFile(Server.MapPath(@"~\Plan\需求导入模板\") + n.Name);
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




       protected void RadComboBoxMaterialDept_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string id = (cb.Parent.Parent as GridDataItem).GetDataKeyValue("ID").ToString(); ;
            RadComboBox RadComboBoxShipping_Address = (cb.Parent.Parent as GridDataItem).FindControl("RadComboBoxShippingAddress1") as RadComboBox;
          
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

       private void BindDDlAddress1(string DeptCode)
       {
          
       }



  
  
        private void BindDeptUserAddress1()
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
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

        private void BindDDlUserInfo1(string Dept_Id)
        {
            RadComboBox_User1.ClearSelection();
            string strSQL = "SELECT * FROM [Sys_UserInfo_PWD] where Dept='" + Dept_Id + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            RadComboBox_User1.DataSource = dt;
            RadComboBox_User1.DataTextField = "UserName";
            RadComboBox_User1.DataValueField = "Id";
            RadComboBox_User1.DataBind();
        }
        protected void RadComboBox_Dept_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (RadComboBox_Dept1.SelectedValue != "0")
            {
                BindDDlUserInfo1(RadComboBox_Dept1.SelectedValue);

                //Get_Sys_DeptEnumByUID(RadComboBox_Dept.SelectedValue);
            }
        }
        #endregion


    }
}