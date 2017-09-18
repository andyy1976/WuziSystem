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

namespace mms.SystemMangement.WinPage
{
    public partial class WinRole : System.Web.UI.Page
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
                GridSource = Common.AddTableRowsID(GetRoleList());
                this.ViewState["lastSelectItem"] = "";
                Common.CheckPermission(userAccount, "Allow_Visit_RoleManage_Page", this);
            }
        }
        protected DataTable GetRoleList()
        {
            try
            {
                string strSQL;
                strSQL = "Select * From Sys_RoleInfo";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取用户角色信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_RoleManage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_RoleManage.DataSource = GridSource;
        }

        protected void RadGrid_RoleManage_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "PerformInsert")
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                DataTable ordersTable = GridSource;
                DataRow newRow = ordersTable.NewRow();

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                try
                {
                    RoleBody newType = new RoleBody();
                    foreach (DictionaryEntry entry in newValues)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }

                    newType.RoleName = newRow["RoleName"].ToString();
                    AddRoleInfo(newType);

                    RadNotificationAlert.Text = "添加成功！";
                    RadNotificationAlert.Show();
                    GridSource = GetRoleList();
                    RadGrid_RoleManage.DataSource = GridSource;

                }
                catch (Exception ex)
                {
                    throw new Exception("添加新用户角色失败！" + ex.Message.ToString());
                }

            }
            if (e.CommandName == "Update")
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                string editID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString();
                DataTable ordersTable = GridSource;
                DataRow[] changedRows = ordersTable.Select("ID='" + editID + "'");
                if (changedRows.Length != 1)
                {
                    e.Canceled = true;
                    return;
                }
                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);
                try
                {
                    RoleBody changeType = new RoleBody();
                    foreach (DictionaryEntry entry in newValues)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changeType.ID = Convert.ToInt32(changedRows[0]["ID"]);
                    changeType.RoleName = changedRows[0]["RoleName"].ToString();

                    UpdateRoleManage(changeType);

                    changedRows[0].EndEdit();
                    RadNotificationAlert.Text = "更新成功！";
                    RadNotificationAlert.Show();
                    e.Canceled = false;
                    RadGrid_RoleManage.Rebind();
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
                GridDataItem dataitem = e.Item as GridDataItem;
                string roleID = dataitem.GetDataKeyValue("ID").ToString();
                DeleteRole(roleID);
                GridSource = GetRoleList();
                RadGrid_RoleManage.DataSource = GridSource;
            }
        }

        protected void AddRoleInfo(RoleBody RoleBody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = @"INSERT INTO [dbo].[Sys_RoleInfo](RoleName) VALUES ('" + RoleBody.RoleName + "')";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("数据库操作-创建用户角色时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        /// <summary>
        /// 数据库操作-修改菜单
        /// </summary>
        protected void UpdateRoleManage(RoleBody RoleBody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                //更新数据库
                DBI.BeginTrans();
                //插入新任务并返回新的任务ID
                strSQL = @"UPDATE [dbo].[Sys_RoleInfo] SET [RoleName] = '" + RoleBody.RoleName + "' WHERE [ID] = '" + RoleBody.ID + "'";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("数据库操作-修改用户角色时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleID">角色ID</param>
        protected void DeleteRole(string roleID)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = "Delete from [dbo].[Sys_RoleInfo] Where ID = '" + roleID + "'";
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
        protected class RoleBody
        {
            public int ID { get; set; }
            public string RoleName { get; set; }
        }

        protected void RadGrid_RoleManage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!Common.IsHasRight(userAccount, "Allow_Edit_RoleManage_Page"))
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButton_AddNew").Visible = false;
                }
                if (e.Item is GridDataItem)
                {
                    RadGrid_RoleManage.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                    RadGrid_RoleManage.Columns.FindByUniqueName("DeleteColumn").Visible = false;
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
                    RadGrid_RoleManage.Columns.FindByUniqueName("EditCommandColumn").Visible = true;
                    RadGrid_RoleManage.Columns.FindByUniqueName("DeleteColumn").Visible = true;
                }
            }
        }
        protected DataTable GetOneRole(string ID)
        {
            string strSQL;
            try
            {
                strSQL = "Select * From [dbo].[Sys_RoleInfo] Where ID like '%" + ID + "%'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取人员列表时出现异常" + e.Message.ToString());
            }
        }
        protected void RadComboBoxRole_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = string.Concat(e.Item.Text.ToLower().Split(' ')[0], "");
        }

        protected void RadComboBoxRole_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string selectName = e.Value.ToString();
            this.ViewState["lastSelectItem"] = selectName;
            GridSource = Common.AddTableRowsID(GetOneRole(selectName));
            this.RadGrid_RoleManage.Rebind();
        }

        protected void RadGrid_RoleManage_ItemCreated(object sender, GridItemEventArgs e)
        {


        }
    }
}