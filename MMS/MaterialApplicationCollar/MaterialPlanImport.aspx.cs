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

namespace mms.MaterialApplicationCollar
{
    public partial class MaterialPlanImport : System.Web.UI.Page
    {
        private static DataTable GridSource1;
        private  DataTable GridSource
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
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
    
            if (!IsPostBack)
            {
               // Common.CheckPermission(Session["UserName"].ToString(), "MDemandImport", this.Page); 

                string PackId = "";
              
                if (Request.QueryString["PackId"] != null && Request.QueryString["PackId"].ToString() != "")
                {
                    PackId = Request.QueryString["PackId"].ToString();

                
                }
  
                GridSource1 = new System.Data.DataTable();
                 
                     this.hfBh.Value = "";
                     this.hfTaskId.Value = "";
                    this.ViewState["submit_type"] = "4";
                    GridSource = Common.AddTableRowsID(GetDetailedListList());
                    Session["gds"] = null;         
                    this.span_apply_time1.InnerText = DateTime.Now.ToString("yyyy-MM-dd"); 

                    BindDeptUserAddress1();
      

                    this.ViewState["GridSource2"] = Common.AddTableRowsID(GetP_Pack_Task());

                    string strSQL = " select PlanCode, State , PlanName, Remark , isnull((select Model from Sys_Model where Convert(nvarchar(50),ID) = P_Pack.Model), P_Pack.Model) as Model from P_Pack where PackID = '" + PackId + "'";
                    DataTable dt = DBI.Execute(strSQL, true);
                    lblPlanCode.Text = dt.Rows[0]["PlanCode"].ToString();
                    lblModel.Text = dt.Rows[0]["Model"].ToString();
                    HFState.Value = dt.Rows[0]["State"].ToString();
                    if (dt.Rows[0]["State"].ToString() == "2")
                    {
                        table1.Visible = true;
                        lblPlanName.Text = dt.Rows[0]["PlanName"].ToString();
                        lblRemark.Text = dt.Rows[0]["Remark"].ToString();
                        RTB_PlanName.Visible = false;
                        RTB_Remark.Visible = false;
                    }
                    else
                    {
                        RTB_PlanName.Text = dt.Rows[0]["PlanName"].ToString();
                        RTB_Remark.Text = dt.Rows[0]["Remark"].ToString();
                    }
           
            }
        }


        protected DataTable GetP_Pack_Task()
        {
            string PaskID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            string strWhere = "";
            if (Session["PTWhere"] != null) { strWhere = Session["PTWhere"].ToString(); }
            string strSQL = " select * , case when Stage = '1' then 'M' when Stage = '2' then 'C' when Stage = '3' then 'S' else 'D' end as Stage1 from P_Pack_Task where IsDel = 'false' and PackId = '" + PaskID + "' " + strWhere + " order by TaskCode";
            DataTable dt = new DataTable();
            try
            {
                dt = DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "获取计划包列表失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }
            return dt;
        }

        protected void RadGridP_Pack_Task_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridP_Pack_Task.DataSource = this.ViewState["GridSource2"];
        }

        protected void RadGridP_Pack_Task_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TaskID"].ToString();
                string IsSpread = (this.ViewState["GridSource2"] as DataTable).Select("TaskID='" + ID + "'")[0]["IsSpread"].ToString().ToLower();
                RadioButtonList RBL = e.Item.FindControl("RBL_IsSpread") as RadioButtonList;
                if (RBL != null)
                {
                    RBL.CssClass = ID.ToString();
                    if (HFState.Value == "2")
                    {
                        RBL.Visible = false;
                        Label lbl = e.Item.FindControl("lbl_IsSpread") as Label;
                        lbl.Text = (IsSpread == "true" ? "是" : "否");
                    }
                    else
                    {
                        if (IsSpread != "" && IsSpread != null)
                        {
                            RBL.SelectedValue = IsSpread.ToLower();
                        }
                    }
                }
            }
            if (e.Item is GridCommandItem)
            {
                if (HFState.Value == "2")
                {
                    RadioButtonList RBL = e.Item.FindControl("RBL_IsSpreadAll") as RadioButtonList;
                    RBL.Visible = false;
                    Label lbl = e.Item.FindControl("lbl") as Label;
                    lbl.Visible = false;
                }
            }
        }

        protected void RBL_IsSpread_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = sender as RadioButtonList;
            string TaskID = rbl.CssClass;
            string IsSpread = rbl.SelectedValue;
            if (TaskID != "")
            {
                string strSQL = " Update P_Pack_Task set IsSpread = '" + IsSpread + "' where TaskID = '" + TaskID + "'";
                try
                {
                    DBI.Execute(strSQL);
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "修改展开状态失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
        }

        protected void RB_Query_Click(object sender, EventArgs e)
        {
            string ProductName = RTB_ProductName.Text.Trim();
            string DrawingNum = RTB_DrawingNum.Text.Trim();
            string TaskNum = RTB_TaskNum.Text.Trim();
            DateTime? StartDate = RDP_StartDate.SelectedDate;
            DateTime? EndDate = RDP_EndDate.SelectedDate;

            string strWhere = "";
            if (ProductName != "")
            {
                strWhere += " and ProductName like '%" + ProductName + "%'";
            }
            if (DrawingNum != "")
            {
                strWhere += " and TaskDrawingCode like '%" + DrawingNum + "%'";
            }
            if (TaskNum != "")
            {
                strWhere += " and TaskCode like '%" + TaskNum + "%'";
            }
            if (StartDate != null)
            {
                strWhere += " and PlanFinishTime >= '" + Convert.ToDateTime(StartDate).ToString("yyyy-MM-dd") + "'";
            }
            if (EndDate != null)
            {
                strWhere += " and PlanFinishTime <= '" + Convert.ToDateTime(EndDate).ToString("yyyy-MM-dd") + "'";

            }
            Session["PTWhere"] = strWhere;
            this.ViewState["GridSource2"] = GetP_Pack_Task();
            RadGridP_Pack_Task.Rebind();

        }

        protected void RB_Add_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RadGridP_Pack_Task.Items.Count; i++)
            {
                RadioButtonList RBL = RadGridP_Pack_Task.Items[i].FindControl("RBL_IsSpread") as RadioButtonList;
                if (RBL != null)
                {
                    if (RBL.SelectedValue == "" || RBL.SelectedValue == null)
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行没有选择是否展开";
                        RadNotificationAlert.Show();
                        return;
                    }
                }
            }

            string PackID = string.IsNullOrEmpty(Request.QueryString["PackID"]) ? "1" : Request.QueryString["PackID"].ToString();
            try
            {
                string strSQL = " Update P_Pack set State = '2' where PackID = '" + PackID + "'";
                DBI.Execute(strSQL);
                RadNotificationAlert.Text = "归档成功！";
                RadNotificationAlert.Show();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }
        }

        protected void RBL_IsSpreadAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (sender) as RadioButtonList;
            string value = rbl.SelectedItem.Value;
            for (int i = 0; i < RadGridP_Pack_Task.Items.Count; i++)
            {
                RadioButtonList rbl1 = RadGridP_Pack_Task.Items[i].FindControl("RBL_IsSpread") as RadioButtonList;
                if (value == "") { rbl1.SelectedIndex = -1; }
                else { rbl1.SelectedValue = value; }
            }
        }

        protected DataTable GetDetailedListList()
        {
            try
            {
                string strSQL = "";
                string TaskID = this.hfTaskId.Value;
                if (TaskID != "")
                {
                    strSQL =
                        " select * , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                        " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1" +
                        " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))" +
                        " union all select *, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
                        " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() +"' and TaskId='"+TaskID+ "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and is_del = 'false' and Material_State = '0' order by ID";
                }
                else
                {
                    strSQL =
                      " select * , 'false' as checked, case when is_del='1' then '取消提交' else case when Material_State = '7' then '取消提交' else '需重新提交' end  end as mstate" +
                      " , Convert(nvarchar(50), (select Convert(int,sum(NumCasesSum)) from M_Demand_Merge_List where Correspond_Draft_Code = Convert(nvarchar(50), M_Demand_DetailedList_Draft.ID))) as quantity1" +
                      " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and ((Is_del = 'false' and Material_State in ('2','7')) or (Is_del = 'true' and Material_State in ('1','2','6','7')))" +
                      " union all select *, 'false' as checked, '未提交' as mstate, '0' as quantity1" +
                      " from M_Demand_DetailedList_Draft where PackId = '" + Request.QueryString["PackId"].ToString() + "' and ParentId_For_Combine= 0" + " and Combine_State!= 2" + " and is_del = 'false' and Material_State = '0' order by ID";
 
                }
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取物资需求清单信息出错" + ex.Message.ToString());
            }
        }

        protected void RadGrid_DemandDetailedList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_DemandDetailedList.DataSource = GridSource;
        }
        protected void RadGrid_DemandDetailedList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            DataTable table = GridSource;
            GridDataItem dataitem = e.Item as GridDataItem;
            if (e.CommandName == "delete")
            {
                int ID =Convert.ToInt32( dataitem.GetDataKeyValue("ID"));
                string MDPId = table.Rows[e.Item.DataSetIndex]["MDPId"].ToString();
                DeletePlanDetailsRecord(MDPId, ID);
                GridSource = Common.AddTableRowsID(GetDetailedListList());
                RadGrid_DemandDetailedList.DataSource = GridSource;
            }
        }
        protected void DeletePlanDetailsRecord(string MDPId, int ID)
        {
            DBI.OpenConnection();
            try
            {
             //   if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            //int userid = Convert.ToInt32(Session["UserId"].ToString());
             //   string strSQL = @"exec Proc_DeleteTechnologyNoSubmit " + MDPId + "," + ID + "," + userid;
                string strSQL = "delete from M_Demand_DetailedList_Draft where ID=" + ID;
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


        protected void RadGrid_Importlist_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                if (Session["UserId"] == null)
                {
                    RadNotificationAlert.Text = "登录超时，请重新登录！" ;
                    RadNotificationAlert.Show();
                    return;
                }
                string userid = Session["UserId"].ToString();
                string strSQL = " select Sys_UserInfo_PWD.ID as UserID, Sys_DeptEnum.ID as DeptID,  DeptCode, Sys_DeptEnum.Dept, UserName, DomainAccount from Sys_UserInfo_PWD" +
                                " join Sys_DeptEnum on Convert(nvarchar(50),Sys_DeptEnum.ID) = Sys_UserInfo_PWD.Dept " +
                                " where Sys_UserInfo_PWD.ID = '" + userid + "'";
                DataTable dt = DBI.Execute(strSQL, true);
              //  string departCode = dt.Rows[0]["DeptCode"].ToString();
                RadComboBox RadComboBoxMaterialDept = e.Item.FindControl("RadComboBoxMaterialDept1") as RadComboBox;

                RadComboBoxMaterialDept.DataSource = dt;
                RadComboBoxMaterialDept.DataTextField = "Dept";
                RadComboBoxMaterialDept.DataValueField = "DeptCode";
                RadComboBoxMaterialDept.DataBind();
               // RadComboBoxMaterialDept.FindItemByValue(GridSource1.Select("ID='" + id + "'")[0]["MaterialDept"].ToString()).Selected = true;

                RadComboBox RDDL_LingJian_Type = e.Item.FindControl("RDDL_LingJian_Type") as RadComboBox;

                RDDL_LingJian_Type.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["LingJian_Type"].ToString()).Selected = true;
              
           //      RadComboBox RadComboBoxStage = e.Item.FindControl("RadComboBoxStage1") as RadComboBox;
             //    RadComboBoxStage.FindItemByText(GridSource1.Select("ID='" + id + "'")[0]["stage"].ToString()).Selected = true;

               
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
              /*   if(RadGridP_Pack_Task.MasterTableView.Items.Count>1)
                 {
                   if( RadGridP_Pack_Task.MasterTableView.GetSelectedItems().Count()<1)
                   {
                       File.Delete(filePath);
                       RadNotificationAlert.Text = "请先选择一条任务记录";
                       RadNotificationAlert.Show();
                       return;

                   }
                 }
                */
       
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
    

                          /*
                               case "产品图号":
                                    GridSource1.Columns[i].ColumnName = "DRAWING_NO";
                                    columnscount++;
                                    break;
                               
                          case "任务号":
                              GridSource1.Columns[i].ColumnName = "TaskCode";
                              columnscount++;
                              break;

                          case "产品编号":
                              GridSource1.Columns[i].ColumnName = "Material_Code";
                              columnscount++;
                              break;
                          case "研制阶段":
                              GridSource1.Columns[i].ColumnName = "stage";
                              columnscount++;
                              break;
                           case "共计需求件数":
                              GridSource1.Columns[i].ColumnName = "NumCasesSum";
                              columnscount++;
                              break;
                      */
                                case "产品名称":
                                    GridSource1.Columns[i].ColumnName = "TDM_Description";
                                    columnscount++;
                                    break;

                                case "技术条件":
                                    GridSource1.Columns[i].ColumnName = "Material_Tech_Condition";
                                    columnscount++;
                                    break;



                                case "工艺路线":
                                    GridSource1.Columns[i].ColumnName = "Technics_Line";
                                    columnscount++;
                                    break;

                                case "零件类型":
                                    GridSource1.Columns[i].ColumnName = "LingJian_Type";
                                    columnscount++;
                                    break;
                        



                                case "物资编码":
                                    GridSource1.Columns[i].ColumnName = "ItemCode1";
                                    columnscount++;
                                    break;

                                case "单件质量":
                                    GridSource1.Columns[i].ColumnName = "Mat_Rough_Weight";
                                    columnscount++;
                                    break;

                                case "每产品质量":
                                    GridSource1.Columns[i].ColumnName = "Mat_Pro_Weight";
                                    columnscount++;
                                    break;

                              
                                /*  
                                 case "物资名称":
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
                                   case "物资牌号":
                                       GridSource1.Columns[i].ColumnName = "Material_Mark";
                                       columnscount++;
                                       break;
                                 */

                              

                                case "物资尺寸":
                                       GridSource1.Columns[i].ColumnName = "ROUGH_SIZE";
                                       columnscount++;
                                       break;


                                   case "特殊需求":
                                       GridSource1.Columns[i].ColumnName = "Comment";
                                       columnscount++;
                                       break;

                                   case "需求时间":
                                       GridSource1.Columns[i].ColumnName = "DemandDate";
                                       columnscount++;
                                       break;
                          
                           
                            }
                        }
                        if (columnscount < 10)
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
                        GridSource1.Columns.Add("Material_Mark");

                        GridSource1.Columns.Add("DemandNumSum");
                        int rowsid = 1;
                        for (int i = 0; i < GridSource1.Rows.Count; i++)
                        {
                            string itemCode1 = GridSource1.Rows[i]["ItemCode1"].ToString();
                             if (itemCode1!= "")
                            {
                          
                                GridSource1.Rows[i]["ID"] = rowsid;
                                rowsid++;
                                Set_Txt_ByItemCode1(itemCode1,i);
                               /*
                                try
                                {
                                    double Mat_Rough_Weight = Convert.ToDouble(GridSource1.Rows[i]["Mat_Rough_Weight"]);
                                    double NumCasesSum = Convert.ToDouble(GridSource1.Rows[i]["NumCasesSum"]);
                                   GridSource1.Rows[i]["DemandNumSum"] = (Mat_Rough_Weight * NumCasesSum).ToString();
                                }
                                catch { GridSource1.Rows[i]["DemandNumSum"] = "0"; }
                                */
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
                GridSource1.Rows[i]["Material_Mark"] = dt.Rows[0]["Seg13"].ToString();
                switch (Seg5)
                {
                    case "YY01":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg21"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY02":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg15"].ToString();
                        break;
                    case "YY03":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg15"].ToString();
                        GridSource1.Rows[i]["ROUGH_SIZE"] = dt.Rows[0]["Seg16"].ToString();
                        break;
                    case "YY04":
                    case "YY05":
                    case "YY06":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY07":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg20"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    case "YY08":
                    case "YY09":
                        GridSource1.Rows[i]["Material_Name"] = dt.Rows[0]["Seg12"].ToString();
                        GridSource1.Rows[i]["MAT_UNIT"] = dt.Rows[0]["Seg7"].ToString();
                        GridSource1.Rows[i]["Rough_Spec"] = dt.Rows[0]["Seg14"].ToString();
                        break;
                    default:
                        GridSource1.Rows[i]["Material_Name"] = "";
                        GridSource1.Rows[i]["MAT_UNIT"] = "";
                        GridSource1.Rows[i]["Rough_Spec"] = "";
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
            string TaskID = "";
            if (RadGridP_Pack_Task.MasterTableView.Items.Count > 1)
            {

                GridDataItem[] dataItems = RadGridP_Pack_Task.MasterTableView.GetSelectedItems();
                if (dataItems.Count() < 1)
                {
                    RadNotificationAlert.Text = "请先选择一条任务记录";
                    RadNotificationAlert.Show();
                    return;

                }
                else if (dataItems.Count() > 1)
                {
                    RadNotificationAlert.Text = "请勿选择多条任务记录";
                    RadNotificationAlert.Show();
                    return;
                }
                else
                {
                    TaskID = dataItems[0].GetDataKeyValue("TaskID").ToString();
                }
            }
            else
            {
                GridDataItem dataItem = RadGridP_Pack_Task.MasterTableView.Items[0];
                TaskID = dataItem.GetDataKeyValue("TaskID").ToString();
            }
            this.hfTaskId.Value = TaskID;
            if (RadGridImport.Items.Count == 0)
            {
                RadNotificationAlert.Text = "失败！没有可导入数据";
                RadNotificationAlert.Show();
                return;
            }
         


            try
            {
                if (Session["UserId"] == null)
                {
                    RadNotificationAlert.Text = "登录超时，请重新登录！";
                    RadNotificationAlert.Show();
                    return;
                }
                string DBContractConn =
                    ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString
                        .ToString();
                DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
               
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string strSQL = "";
              
                int Submit_Type = Convert.ToInt32(this.ViewState["submit_type"].ToString());//1－工艺试验件；2－技术创新课题；3－车间备料

                int PackId = Convert.ToInt32(Request.QueryString["PackId"].ToString());

                M_Demand_DetailedList_Draft MDDLD = new M_Demand_DetailedList_Draft()
                {
                    Ballon_No = "",
                    Class_Id = null,
                    CN_Material_State = "",
                    Comment = "",
                    DemandDate = "",
                    DemandNumSum = null,
                    DraftId = null,
                    Drawing_No = "",

                    Import_Date = null,
                    Is_allow_merge = null,
                    Is_del = false,
                    ItemCode1 = "",
                    ItemCode2 = "",
                    JSGS_Des = "",
                    LingJian_Type = "",
                    Mat_Comment = "",
                    Mat_Efficiency = "",
                    Mat_Pro_Weight = "",
                    Mat_Rough_Weight = "",
                    Mat_Technics = "",
                    Mat_Unit = "",
                    Mat_Weight = "",
                    Material_Code = "",
                    Material_Mark = "",
                    Material_Name = "",
                    Material_Spec = "",
                    Material_State = 0,
                    Material_Tech_Condition = "",
                    MaterialDept = "",
                    MaterialsDes = "",
                    MaterialsNum = null,
                    MDML_Id = "",
                    MDPId = null,
                    Memo_Quantity = "",
                    MissingDescription = "",
                    NumCasesSum = null,
                    Object_Id = null,
                    Other_Quantity = "",
                    PackId = PackId,
                    ParentId = -1,
                    PredictDeliveryDate = null,
                    Quantity = "",
                    Required_Quantity = "",
                    Rough_Size = "",
                    Rough_Spec = "",
                    Stage = null,
                    StandAlone = null,   //单机配套数量
                    TaskCode = "",
                    TaskId = null,
                    TDM_Description = "",
                    Tech_Quantity = "",
                    Technics_Comment = "",
                    Technics_Line = "",
                    Test_Quantity = "",
                    ThisTimeOperation = null, //投产数量
                    User_ID = userid,
                    VerCode = 1
                };
            //    strSQL = " Select * From V_M_Draft_List where packid='" + PackId + "'";
                strSQL = " Select * From M_Demand_Plan_List where packid='" + PackId + "'";
                DataTable dt = DBI.Execute(strSQL, true);

                string DraftID = "";
                int MDPID = 0;
                if (dt.Rows.Count > 0)
                {
                    DraftID = dt.Rows[0]["draftid"].ToString();
                    this.hfBh.Value = dt.Rows[0]["Id"].ToString();
                }

                if (DraftID == "" || DraftID == null)
                {
                    string Draft_Code = DBI.GetSingleValue(" Exec  [Proc_CodeBuildByCodeDes1] '材料清单编号','JZWZ'");
                    strSQL = " Insert into [dbo].[M_Draft_List] (Draft_Code, Material_State, Lasttime_Synchro_Time, PackId, Task_Type, List_Maker)"
                        + " values ('" + Draft_Code + "','1',GetDate(),'" + PackId + "','0','" + userid + "') select @@identity";
                    DraftID = DBI.GetSingleValue(strSQL);

                
                }
               // if (DraftID != "" & DraftID != null)
           //     {
                //    MDDLD.DraftId = Convert.ToInt32(DraftID);
           //     }

         

             strSQL = " select TaskId, PackId, ProductName, TaskDrawingCode, TaskCode, Stage, Unit, MatingNum, DefrayNum, ProductionNum, PlanFinishTime"
                + " , IsSpread, LastChangeTime, ChangeTimes, IsDel"
                + " , (select top 1 case when IsGetBOM = 'true' then AreaCode else '' end from P_Pack left join Sys_Model on Sys_Model.Id = P_Pack.Model left join Sys_Area on Sys_Area.Id = Sys_Model.AreaId where PackId = '" + PackId + "') as AreaCode"
                + " from P_Pack_Task where PackID = '" + PackId +"' and TaskId='"+ TaskID+"' and IsDel = 'false'";
             dt = DBI.Execute(strSQL, true);

             

          //  for (int j = 0; j < dt.Rows.Count; j++)
          //  {
                MDDLD.Drawing_No = dt.Rows[0]["TaskDrawingCode"].ToString();
                MDDLD.Import_Date = DateTime.Now;
     
                MDDLD.NumCasesSum = Convert.ToDecimal(dt.Rows[0]["ProductionNum"].ToString());
                MDDLD.Quantity = dt.Rows[0]["ProductionNum"].ToString();

                MDDLD.Stage = Convert.ToInt32(dt.Rows[0]["Stage"].ToString());
                MDDLD.StandAlone = Convert.ToInt32(dt.Rows[0]["MatingNum"].ToString()); //单机配套数量
                MDDLD.TaskCode = dt.Rows[0]["TaskCode"].ToString();

                /*
                 MDDLD.TaskCode = item["TaskCode"].Text.Trim();
                if (MDDLD.TaskCode == "" || MDDLD.TaskCode == "&nbsp;")
                {
                    RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，任务号：请输入任务号";
                    RadNotificationAlert.Show();
                    return;
                }
                */
                MDDLD.TaskId = Convert.ToInt32(dt.Rows[0]["TaskID"].ToString());
                MDDLD.ThisTimeOperation = Convert.ToInt32(dt.Rows[0]["ProductionNum"].ToString()); //投产数量
                MDDLD.TDM_Description = dt.Rows[0]["ProductName"].ToString();
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
                        MDDLD.Material_Code = (i + 1).ToString();
                       /*
                        MDDLD.Mat_Weight = item["Mat_Weight"].Text.Trim(); ;
                        MDDLD.Mat_Efficiency = item["Mat_Efficiency"].Text.Trim(); ;
                        MDDLD.Mat_Comment = item["Mat_Comment"].Text.Trim(); ;
                        MDDLD.Mat_Technics = item["Mat_Technics"].Text.Trim(); ;
                        MDDLD.CN_Material_State = item["CN_Material_State"].Text.Trim(); 
                       
                        RadComboBox RadComboBoxStage = item.FindControl("RadComboBoxStage1") as RadComboBox;
                        MDDLD.Stage = Convert.ToInt32(RadComboBoxStage.SelectedValue);
                        MDDLD.Drawing_No = item["DRAWING_NO"].Text.Trim();
                        if (MDDLD.Drawing_No == "" || MDDLD.Drawing_No == "&nbsp;")
                        {
                           RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，产品图号：请输入产品图号";
                           RadNotificationAlert.Show();
                           return;
                        }
                        */
                        MDDLD.Material_Tech_Condition = item["Material_Tech_Condition"].Text.Trim();                    

                      //  MDDLD.TDM_Description = item["TDM_Description"].Text.Trim();

                        MDDLD.Material_Name = item["Material_Name"].Text.Trim();

                        MDDLD.Material_Mark = item["Material_Mark"].Text.Trim();
    
                        MDDLD.Technics_Line = item["Technics_Line"].Text.Trim();
                        MDDLD.Comment = item["Comment"].Text.Trim();
                    /*
                    if (Comment == "" || Comment == "&nbsp;")
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，备注：请输入特殊需求";
                        RadNotificationAlert.Show();
                        return;
                    }
                    */
                     

                        MDDLD.ItemCode1 = item["ItemCode1"].Text.Trim();



                        MDDLD.Mat_Unit = item["MAT_UNIT"].Text.Trim();

                        RadComboBox RadDropDownListLingJian_Type = item.FindControl("RDDL_LingJian_Type") as RadComboBox;
                        MDDLD.LingJian_Type = RadDropDownListLingJian_Type.SelectedValue;

                        MDDLD.Mat_Rough_Weight = item["Mat_Rough_Weight"].Text.Trim();
                        try
                        {
                            Convert.ToDecimal(MDDLD.Mat_Rough_Weight);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，单件质量：请输入数字";
                            RadNotificationAlert.Show();
                            return;
                        }

                        MDDLD.Mat_Pro_Weight = item["Mat_Pro_Weight"].Text.Trim();
                        try
                        {
                            Convert.ToDecimal(MDDLD.Mat_Pro_Weight);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，每产品质量：请输入数字";
                            RadNotificationAlert.Show();
                            return;
                        }


                        MDDLD.Rough_Spec = item["Rough_Spec"].Text.Trim();
                        MDDLD.Rough_Size = item["ROUGH_SIZE"].Text.Trim();
                        if (MDDLD.Rough_Size == "" || MDDLD.Rough_Size == "&nbsp;")
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，物资尺寸：请输入物资尺寸";
                            RadNotificationAlert.Show();
                            return;
                        }

                        /*
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
                        MDDLD.NumCasesSum = Convert.ToInt32(NumCasesSum);
                      
                   
                        
                             
                

            


                        string DemandNumSum = item["DemandNumSum"].Text.Trim();
                        try
                        {
                            Convert.ToDecimal(DemandNumSum);
                        }
                        catch
                        {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，共计需求数量（kg）：请输入数字";
                            RadNotificationAlert.Show();
                            return;
                        }
                        MDDLD.DemandNumSum = Convert.ToDecimal(DemandNumSum);
                         */
                        MDDLD.DemandNumSum = Convert.ToDecimal(MDDLD.Mat_Rough_Weight) * MDDLD.NumCasesSum;

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

                        MDDLD.DemandDate =DemandDate;
                        RadComboBox RadComboBoxMaterialDept = item.FindControl("RadComboBoxMaterialDept1") as RadComboBox;
                        MDDLD.MaterialDept = RadComboBoxMaterialDept.SelectedValue;

                       
                       
                    
                        try
                        {
                          if (this.hfBh.Value != null && this.hfBh.Value != "")
                            {
                                MDPID = Convert.ToInt32(this.hfBh.Value);
                            }
                            else
                            {
                                strSQL = @"exec Proc_Add_M_Demand_Plan_List_Technology " + userid + ",'" + MDDLD.TaskCode + "'," + PackId + "," + DraftID + "," + this.ViewState["submit_type"].ToString() + ",0,''";
                                DataTable dt1 = DBI.Execute(strSQL, true);
                                if (dt1.Rows.Count == 1)
                                {
                                    MDPID = Convert.ToInt32(dt1.Rows[0][0].ToString());
                                    this.hfBh.Value = dt1.Rows[0][0].ToString();
                                }
                                else
                                {
                                    RadNotificationAlert.Text = "失败！Proc_Add_M_Demand_Plan_List_Technology返回的记录数不唯一";
                                    RadNotificationAlert.Show();
                                    return;
                                }

                            }
                          MDDLD.MDPId = MDPID;
                         

                            strSQL = " Insert Into M_Demand_DetailedList_Draft ( PackId, DraftId,taskid,MDPId,Quantity,VerCode,Stage,DemandDate,Comment,MaterialDept,TaskCode,Drawing_No, Mat_Pro_Weight,Material_State, Combine_State,Material_Name, Material_Mark, ItemCode1, Mat_Unit, Mat_Rough_Weight, Rough_Size, Rough_Spec, DemandNumSum, NumCasesSum,Material_Tech_Condition,Material_Code,TDM_Description,Technics_Line,LingJian_Type,Import_Date,User_ID,ParentId,ParentId_For_Combine,Is_del)"
                              + " Values (" + MDDLD.PackId + "," + DraftID + "," + MDDLD.TaskId + "," + MDDLD.MDPId + "," + MDDLD.Quantity + ","+MDDLD.VerCode+"," + MDDLD.Stage + ",'" + MDDLD.DemandDate + "','" + MDDLD.Comment + "','" + MDDLD.MaterialDept + "','" + MDDLD.TaskCode + "','" + MDDLD.Drawing_No + "'," + MDDLD.Mat_Pro_Weight + ",0,0,'" +
                                       MDDLD.Material_Name + "','" + MDDLD.Material_Mark + "','" + MDDLD.ItemCode1 + "','" + MDDLD.Mat_Unit + "'," + MDDLD.Mat_Rough_Weight + ",'" + MDDLD.Rough_Size + "','" + MDDLD.Rough_Spec + "'," + MDDLD.DemandNumSum + "," + MDDLD.NumCasesSum + ",'" + MDDLD.Material_Tech_Condition + "','" + MDDLD.Material_Code + "','" + MDDLD.TDM_Description + "','" + MDDLD.Technics_Line + "','" + MDDLD.LingJian_Type + "','" + MDDLD.Import_Date + "'," + userid + ",0,0,0)" + " select @@identity";
                            DBI.GetSingleValue(strSQL);
                            // DBI.Execute(strSQL);
                        }
                        catch (Exception e1)
                        
                        {
                            RadNotificationAlert.Text = "导入失败！第" + (i + 1).ToString() + "行，发生错误" + e1.Message.ToString();
                            RadNotificationAlert.Show();
                            return;
                        }
                    }
                }
          //  }
    

              //  RadBtnSubmit.Visible = true;
                RadNotificationAlert.Text = "导入成功！";
                RadNotificationAlert.Show();

                GridSource = Common.AddTableRowsID(GetDetailedListList());
                RadGrid_DemandDetailedList.Rebind();
           
             

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
                new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "MaterialApplicationCollar/需求导入模板");
            if (System.IO.Directory.Exists(info.ToString()))
            {
                foreach (FileInfo n in info.GetFiles())
                {
                    if (n.Name == "型号物资导入模板.xlsx")
                    {
                        i = 1;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(n.Name));
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                        Response.ContentType = "application/ms-excel";
                        this.EnableViewState = false;

                        Response.WriteFile(Server.MapPath(@"~\MaterialApplicationCollar\需求导入模板\") + n.Name);
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

  
  
        private void BindDeptUserAddress1()
        {
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
            }
        }
   
    }
}