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
using System.Collections;

namespace mms.SystemMangement
{
    public partial class PermissionManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("PermissionSign");
                    InitTable.Columns.Add("SignName");
                    InitTable.Columns.Add("ParentId");
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
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            userAccount = Session["UserName"].ToString();
            if (!IsPostBack)
            {
                GridSource = Common.AddTableRowsID(GetPermissionList());
                Common.CheckPermission(userAccount, "Allow_Visit_PermissionManage_Page", this);

                this.ViewState["lastSelectItem"] = "";
            }
        }

        protected DataTable GetPermissionList()
        {
            try
            {
                string strSQL;
                strSQL = "Select * From [dbo].[Sys_Permission]";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取用户信息出错" + ex.Message.ToString());
            }
        }

        protected void RadGrid_Permission_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid_Permission.DataSource = GridSource;
        }

        protected void RadGrid_Permission_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            if (e.CommandName == "PerformInsert")
            {
                DataTable ordersTable = GridSource;
                DataRow newRow = ordersTable.NewRow();

                //Set new values
                Hashtable newValues = new Hashtable();
                //The GridTableView will fill the values from all editable columns in the hash
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                try
                {
                    PermissionBody newType = new PermissionBody();
                    foreach (DictionaryEntry entry in newValues)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }

                    newType.PermissionSign = newRow["PermissionSign"].ToString();
                    newType.SignName = newRow["SignName"].ToString();

                    try
                    {
                        newType.ParentId = Convert.ToInt16(newRow["ParentId"].ToString());
                    }
                    catch
                    {
                        newType.ParentId = 0;
                    }

                    AddUserInfo(newType);

                    e.Canceled = false;
                    GridSource = GetPermissionList();
                    RadGrid_Permission.DataSource = GridSource;

                }
                catch (Exception ex)
                {
                    throw new Exception("添加新用户失败！" + ex.Message.ToString());
                }
            }
            if (e.CommandName == "Update")
            {
                string editID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString();
                DataTable ordersTable = GridSource;
                //Locate the changed row in the DataSource
                DataRow[] changedRows = ordersTable.Select("ID='" + editID + "'");
                if (changedRows.Length != 1)
                {
                    e.Canceled = true;
                    return;
                }
                //Update new values
                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
                try
                {
                    PermissionBody changeType = new PermissionBody();
                    foreach (DictionaryEntry entry in newValues)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changeType.ID = Convert.ToInt32(changedRows[0]["ID"]);
                    changeType.PermissionSign = changedRows[0]["PermissionSign"].ToString();
                    changeType.SignName = changedRows[0]["SignName"].ToString();
                    try
                    {
                        changeType.ParentId = Convert.ToInt32(changedRows[0]["ParentId"].ToString());
                    }
                    catch
                    {
                        changeType.ParentId = 0;
                    }

                    UpdateUserManage(changeType);

                    changedRows[0].EndEdit();

                    e.Canceled = false;
                    GridSource = GetPermissionList();
                    RadGrid_Permission.DataSource = GridSource;
                }
                catch (Exception ex)
                {
                    changedRows[0].CancelEdit();
                    e.Canceled = true;
                }
            }
            if (e.CommandName == "Delete")
            {
                DataTable table = GridSource;
                GridDataItem[] selectItems = e.Item.OwnerTableView.GetSelectedItems();
                foreach (GridDataItem item in selectItems)
                {
                    string userID = item.GetDataKeyValue("ID").ToString();
                    DeletePermission(userID);
                }
                GridSource = GetPermissionList();
                RadGrid_Permission.DataSource = GridSource;
            }
        }
        protected void AddUserInfo(PermissionBody permissionbody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                //数据库操作
                strSQL = @"INSERT INTO [dbo].[Sys_Permission]
                               (PermissionSign, SignName, ParentId)
                               VALUES ('" + permissionbody.PermissionSign + "', '" + permissionbody.SignName + "', '" + permissionbody.ParentId + "')";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("数据库操作-创建用户时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        /// <summary>
        /// 数据库操作-修改菜单
        /// </summary>
        protected void UpdateUserManage(PermissionBody permissionbody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                //更新数据库
                DBI.BeginTrans();
                //插入新任务并返回新的任务ID
                strSQL = @"UPDATE [dbo].[Sys_Permission]
                               SET [PermissionSign] = '" + permissionbody.PermissionSign + "'" +
                                  ", [SignName] = '" + permissionbody.SignName + "'" +
                                  ", [ParentId] = '" + permissionbody.ParentId + "'" +
                             " WHERE [ID] = '" + permissionbody.ID + "'";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("数据库操作-修改用户时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="roleID">人员ID</param>
        protected void DeletePermission(string ID)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = "Delete [dbo].[Sys_Permission] Where ID = '" + ID + "'";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("删除角色时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        protected class PermissionBody
        {
            public int ID { get; set; }
            public string PermissionSign { get; set; }
            public string SignName { get; set; }
            public int ParentId { get; set; }
        }
        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            ((sender as RadButton).NamingContainer as GridItem).Selected = (sender as RadButton).Checked;
        }
        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            RadButton headerCheckBox = (sender as RadButton);
            foreach (GridDataItem dataItem in RadGrid_Permission.MasterTableView.Items)
            {
                (dataItem.FindControl("RadButtonItem") as RadButton).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void RadGrid_Permission_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!Common.IsHasRight(userAccount, "Allow_Edit_DeptManage_Page"))
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButton_AddNew").Visible = false;
                    e.Item.FindControl("RadButton_Delete").Visible = false;
                }
                if (e.Item is GridDataItem)
                {
                    RadGrid_Permission.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                    RadGrid_Permission.Columns.FindByUniqueName("CheckBoxTemplateColumn").Visible = false;
                }
            }
            else
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButton_AddNew").Visible = true;
                    e.Item.FindControl("RadButton_Delete").Visible = true;
                }
                if (e.Item is GridDataItem)
                {
                    RadGrid_Permission.Columns.FindByUniqueName("EditCommandColumn").Visible = true;
                    RadGrid_Permission.Columns.FindByUniqueName("CheckBoxTemplateColumn").Visible = true;
                }
            }
        }

        protected DataTable GetOnePermission(string ID)
        {
            string strSQL;
            try
            {
                strSQL = "Select * From [dbo].[Sys_Permission] Where ID = '" + ID + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取人员列表时出现异常" + e.Message.ToString());
            }
        }

        protected void RadComboBoxPermission_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = string.Concat(e.Item.Text.ToLower().Split(' ')[0], "");
        }

        protected void RadComboBoxPermission_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string selectName = e.Value.ToString();
            this.ViewState["lastSelectItem"] = selectName;
            GridSource = Common.AddTableRowsID(GetOnePermission(selectName));
            this.RadGrid_Permission.Rebind();
        }
    }
}