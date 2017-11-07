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

namespace mms.SystemMangement
{
    public partial class DeptManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("Dept");
                    InitTable.Columns.Add("DeptCode");
                    InitTable.Columns.Add("Shipping_Addr_Id");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["ID"] };       //设置RowsId列为主键，用于datatable删除
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
        private string userAccount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            userAccount = Session["UserName"].ToString();
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "DeptManage", this.Page);
                GridSource = GetDeptList();
            }
        }

        protected DataTable GetDeptList()
        {
            try
            {
                string strSQL = " select *  from Sys_DeptEnum"
                    + " order by Is_Del,  Dept";
                return Common.AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception ex)
            {
                throw new Exception("获取部门信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_DeptManage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_DeptManage.DataSource = GridSource;
        }

        protected void RadGrid_DeptManage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!Common.IsHasRight(userAccount, "RadButton_AddNew"))
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButton_AddNew").Visible = false;
                }
                if (e.Item is GridDataItem)
                {
                    RadGrid_DeptManage.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                    RadGrid_DeptManage.Columns.FindByUniqueName("DeleteColumn").Visible = false;
                }
            }
            else
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButton_AddNew").Visible = true;
                }
                if (e.Item is GridDataItem)
                {
                    RadGrid_DeptManage.Columns.FindByUniqueName("EditCommandColumn").Visible = true;
                    RadGrid_DeptManage.Columns.FindByUniqueName("DeleteColumn").Visible = true;
                }
            }
            if (Common.IsHasRight(userAccount, "RB_Addr"))
            {
                if (e.Item.FindControl("RB_Addr") != null) {
                    e.Item.FindControl("RB_Addr").Visible = true;
                }
            }

            if (e.Item is GridDataItem)
            {
                if (e.Item is Telerik.Web.UI.GridDataInsertItem) { }
                else
                {
                    string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                    string Cust_Account_ID = GridSource.Select("ID='" + id + "'")[0]["Cust_Account_ID"].ToString();
                    if (Cust_Account_ID != "" && Cust_Account_ID != "0")
                    {
                        string strSQL = " declare @sql nvarchar(max) = ''select @sql = @sql + Convert(nvarchar(50), KeyWord ) + ','" +
                            " from Sys_Dict where TypeID = '2' and  '2-' + Convert(nvarchar(50), KeyWordCode) in (select Shipping_Addr_ID from Sys_Dept_ShipAddr where Dept_Id= '" + id + "')" +
                            " select case when @sql = '' then '' else substring(@sql,1, len(@sql)-1) end";
                        string Address = DBI.GetSingleValue(strSQL).ToString();
                        (e.Item as GridDataItem)["Shipping_Address"].Text = Address;
                    }
                    else
                    {
                        (e.Item as GridDataItem)["Shipping_Address"].Text = "";
                    }
                }
            }
        }

        protected void RadGrid_DeptManage_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string strSQL = "";
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            RadTextBox RTB_DeptName = userControl == null ? new RadTextBox():userControl.FindControl("RTB_DeptName") as RadTextBox;
            RadTextBox RTB_DeptCode = userControl == null ? new RadTextBox() : userControl.FindControl("RTB_DeptCode") as RadTextBox;
            RadDropDownList RDDL_Cust_Account_ID = userControl == null ? new RadDropDownList() : userControl.FindControl("RDDL_Cust_Account_ID") as RadDropDownList;
            CheckBoxList CBL_Shipping_Address = userControl == null ? new CheckBoxList() : userControl.FindControl("CBL_Shipping_Address") as CheckBoxList;
                        
            if (e.CommandName == "PerformInsert")
            {
                DBI.OpenConnection();
                try
                {
                    DBI.BeginTrans();
                    string Dept = RTB_DeptName.Text.Trim();
                    string DeptCode = RTB_DeptCode.Text.Trim();
                    string Cust_Account_ID = RDDL_Cust_Account_ID.SelectedValue.ToString();

                    if (Dept == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入部门名称";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (DeptCode == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入部门编号";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;

                    }

                    strSQL = " select count(*) from Sys_DeptEnum where Dept = '" + Dept + "' and is_Del = 'false'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！部门名称已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    strSQL = " select count(*) from Sys_DeptEnum where DeptCode = '" + DeptCode + "' and is_Del = 'false'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！部门编号已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }

                    strSQL = "INSERT INTO [dbo].[Sys_DeptEnum](Dept,DeptCode, Is_Del,Cust_Account_ID) VALUES ('" + Dept + "','" + DeptCode + "','false'"
                        + " , case when  '" + Cust_Account_ID + "' = '0' then Null else '" + Cust_Account_ID + "' end) select @@identity";
                    string id = DBI.GetSingleValue(strSQL);
                    for (int i = 0; i < CBL_Shipping_Address.Items.Count; i++)
                    {
                        if (CBL_Shipping_Address.Items[i].Selected == true)
                        {
                            strSQL = " Insert into Sys_Dept_ShipAddr(Dept_ID,Shipping_Addr_Id, Is_Del) values('" + id + "','2-' + '" + CBL_Shipping_Address.Items[i].Value + "','false')";
                            DBI.Execute(strSQL);
                        }
                    }

                    RadNotificationAlert.Text = "添加成功！";
                    RadNotificationAlert.Show();

                    GridSource = GetDeptList();

                    DBI.CommitTrans();
                }
                catch (Exception ex)
                {
                    DBI.RollbackTrans();
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
                finally
                {
                    DBI.CloseConnection();
                }
            }
            if (e.CommandName == "Update")
            {
                string id = (e.Item as GridEditableItem).GetDataKeyValue("ID").ToString();
                DBI.OpenConnection();
                try
                {
                    DBI.BeginTrans();

                    string Dept = RTB_DeptName.Text.Trim();
                    string DeptCode = RTB_DeptCode.Text.Trim();
                    string Cust_Account_ID = RDDL_Cust_Account_ID.SelectedValue.ToString();

                    if (Dept == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入部门名称";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (DeptCode == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入部门编号";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;

                    }

                    strSQL = " select count(*) from Sys_DeptEnum where Dept = '" + Dept + "' and ID <> '" + id + "' and Is_Del = 'false'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！部门名称已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }

                    strSQL = " select count(*) from Sys_DeptEnum where DeptCode = '" + DeptCode + "' and ID <> '" + id + "' and Is_Del = 'false'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！部门编号已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }

                    strSQL = "UPDATE [dbo].[Sys_DeptEnum] SET [Dept] = '" + Dept + "',[DeptCode] = '" + DeptCode + "'" +
                         " , Cust_Account_ID = case when  '" + Cust_Account_ID + "' = '0' then Null else '" + Cust_Account_ID + "' end WHERE [ID] = '" + id + "'" +
                        " delete Sys_Dept_ShipAddr where Dept_Id='" + id + "'";
                    DBI.Execute(strSQL);
                    for (int i = 0; i < CBL_Shipping_Address.Items.Count; i++)
                    {
                        if (CBL_Shipping_Address.Items[i].Selected == true)
                        {
                            strSQL = " Insert into Sys_Dept_ShipAddr(Dept_ID,Shipping_Addr_Id, Is_Del) values('" + id + "','2-' + '" + CBL_Shipping_Address.Items[i].Value + "','false')";
                            DBI.Execute(strSQL);
                        }
                    }

                    RadNotificationAlert.Text = "更新成功！";
                    RadNotificationAlert.Show();
                    e.Canceled = false;
                    GridSource = GetDeptList();

                    DBI.CommitTrans();
                }
                catch (Exception ex)
                {
                    DBI.RollbackTrans();
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
                finally {
                    DBI.CloseConnection();
                }
            }
            if (e.CommandName == "Delete")
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();

                strSQL = "select count(*) from [dbo].[Sys_UserInfo_PWD] Where Dept = '" + id + "'";
                if (DBI.GetSingleValue(strSQL).ToString() != "0")
                {
                    RadNotificationAlert.Text = "失败！该部门已在员工管理中使用，无法删除";
                    RadNotificationAlert.Show();
                    return;
                }

                strSQL = "Update [dbo].[Sys_DeptEnum] set Is_del='true' Where ID = '" + id + "'";
                DBI.Execute(strSQL);

                GridSource = GetDeptList();
                RadGrid_DeptManage.DataSource = GridSource;

                RadNotificationAlert.Text = "删除成功！";
                RadNotificationAlert.Show();
            }
        }        

        
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                GridSource = GetDeptList();
                RadGrid_DeptManage.Rebind();
            }
            else
            {
                throw new Exception("刷新页面出错，请按F5刷新！");
            }            
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid_DeptManage.ExportSettings.FileName = "部门信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_DeptManage.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_DeptManage.ExportSettings.FileName = "部门信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_DeptManage.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_DeptManage.ExportSettings.FileName = "部门信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_DeptManage.ExportSettings.IgnorePaging = true;
            RadGrid_DeptManage.MasterTableView.ExportToPdf();
            RadGrid_DeptManage.ExportSettings.IgnorePaging = false;
        }
    }
}