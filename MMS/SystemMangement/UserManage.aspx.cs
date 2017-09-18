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
    public partial class UserManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("RoleName");
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
                GridSource = Common.AddTableRowsID(GetUserList());
                //设置上一次部门过滤选择状态
                this.ViewState["lastSelectItem"] = "";
                Common.CheckPermission(userAccount, "Allow_Visit_UserManage_Page", this);
            }
        }

        protected DataTable GetUserList()
        {
            try
            {
                string strSQL;
                strSQL = "Select * From Sys_UserInfo";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取用户信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_UserManage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_UserManage.DataSource = GridSource;
        }

        protected void RadGrid_UserManage_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "PerformInsert")
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                DataTable ordersTable = GridSource;
                DataRow newRow = ordersTable.NewRow();
                //Set new values
                Hashtable newValues = new Hashtable();
                //The GridTableView will fill the values from all editable columns in the hash
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                try
                {
                    UserBody newType = new UserBody();
                    foreach (DictionaryEntry entry in newValues)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }

                    newType.UserAccount = newRow["UserAccount"].ToString();
                    newType.UserName = newRow["UserName"].ToString();
                    newType.DeptNo = newRow["DeptNo"].ToString();
                    
                    try
                    {
                        newType.ID = Convert.ToInt16(newRow["ID"].ToString());
                    }
                    catch
                    {
                        newType.ID = 0;
                    }

                    AddUserInfo(newType);

                    e.Canceled = false;
                    GridSource = GetUserList();
                    RadGrid_UserManage.DataSource = GridSource;

                }
                catch (Exception ex)
                {
                    throw new Exception("添加新用户失败！" + ex.Message.ToString());
                }
            }
            if (e.CommandName == "Update")
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
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
                    UserBody changeType = new UserBody();
                    foreach (DictionaryEntry entry in newValues)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changeType.ID = Convert.ToInt32(changedRows[0]["ID"]);
                    changeType.UserAccount = changedRows[0]["UserAccount"].ToString();
                    changeType.UserName = changedRows[0]["UserName"].ToString();
                    changeType.DeptNo = changedRows[0]["DeptNo"].ToString();

                    UpdateUserManage(changeType);

                    changedRows[0].EndEdit();

                    e.Canceled = false;
                    GridSource = GetUserList();
                    RadGrid_UserManage.DataSource = GridSource;
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
                    DeleteUser(userID);
                    //GridSource.Rows.Find(ID).Delete();
                    //GridSource.AcceptChanges();
                }
                GridSource = GetUserList();
                RadGrid_UserManage.DataSource = GridSource;
            }
        }

        protected void AddUserInfo(UserBody userbody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                //数据库操作
                strSQL = @"INSERT INTO [dbo].[Sys_UserInfo]
                               (UserAccount, UserName, DeptNo)
                               VALUES ('" + userbody.UserAccount + "', '" + userbody.UserName + "', '" + userbody.DeptNo + "')";
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
        protected void UpdateUserManage(UserBody userbody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                //更新数据库
                DBI.BeginTrans();
                //插入新任务并返回新的任务ID
                strSQL = @"UPDATE [dbo].[Sys_UserInfo]
                               SET [UserAccount] = '" + userbody.UserAccount + "'" +
                                  ", [UserName] = '" + userbody.UserName + "'" +
                                  ", [DeptNo] = '" + userbody.DeptNo + "'" +
                             " WHERE [ID] = '" + userbody.ID + "'";
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
        protected void DeleteUser(string userID)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = "Delete [dbo].[Sys_UserInfo] Where ID = '" + userID + "'";
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
        protected class UserBody
        {
            public int ID { get; set; }
            public string UserAccount { get; set; }
            public string UserName { get; set; }
            public string DeptNo { get; set; }
        }

        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            ((sender as RadButton).NamingContainer as GridItem).Selected = (sender as RadButton).Checked;
        }
        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            RadButton headerCheckBox = (sender as RadButton);
            foreach (GridDataItem dataItem in RadGrid_UserManage.MasterTableView.Items)
            {
                (dataItem.FindControl("RadButtonItem") as RadButton).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void RadGrid_UserManage_ItemDeleted(object sender, GridDeletedEventArgs e)
        {
            
        }

        protected void RadGrid_UserManage_ItemDataBound(object sender, GridItemEventArgs e)
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
                    RadGrid_UserManage.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                    RadGrid_UserManage.Columns.FindByUniqueName("CheckBoxTemplateColumn").Visible = false;
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
                    RadGrid_UserManage.Columns.FindByUniqueName("EditCommandColumn").Visible = true;
                    RadGrid_UserManage.Columns.FindByUniqueName("CheckBoxTemplateColumn").Visible = true;
                }
            }
        }
        protected DataTable GetOneUser(string ID)
        {
            string strSQL;
            try
            {
                strSQL = "Select * From [dbo].[Sys_UserInfo] Where ID like '%" + ID + "%'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取人员列表时出现异常" + e.Message.ToString());
            }
        }

        protected void RadGrid_UserManage_ItemCreated(object sender, GridItemEventArgs e)
        {
            
        }

        protected void RadComboBoxUser_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = string.Concat(e.Item.Text.ToLower().Split(' ')[0], "");
        }

        protected void RadComboBoxUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string selectName = e.Value.ToString();
            this.ViewState["lastSelectItem"] = selectName;
            GridSource = Common.AddTableRowsID(GetOneUser(selectName));
            this.RadGrid_UserManage.Rebind();
        }
    }
}