using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Camc.Web.Library;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;

namespace mms.SystemMangement
{
    public partial class SetRoleToPermission : System.Web.UI.Page
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

        //初始化列表标记
        private bool isReload
        {
            get
            {
                object o = ViewState["isReload"];
                if (o == null)
                {
                    return false;
                }
                return (bool)o;
            }
            set
            {
                ViewState["isReload"] = value;
            }
        }

        //声明全局数据
        private static string DBConn;
        private DBInterface DBI;
        private string userAccount;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (Session["UserName"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            userAccount = Session["UserName"].ToString();
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "SetRoleToPermission", this.Page);
                this.RadTabStripRoles.DataSource = GetRoleList();
                this.RadTabStripRoles.DataTextField = "RoleName";
                this.RadTabStripRoles.DataValueField = "ID";
                this.RadTabStripRoles.DataBind();
                this.RadTabStripRoles.SelectedIndex = 0;
                //展开树的1级
                //RadTreeListSource.ExpandedIndexes.Add(new TreeListHierarchyIndex { LevelIndex = 0, NestedLevel = 0 });
                RadTreeListSource.ExpandAllItems();
                if (!Common.IsHasRight(userAccount, "RadButtonSaveConfig"))
                {
                    RadButtonSaveConfig.Visible = false;
                }
                else
                {
                    RadButtonSaveConfig.Visible = true;
                }
            }
        }
        protected DataTable GetRoleList()
        {
            try
            {
                string strSQL;
                strSQL = "Select * From Sys_RoleInfo where Is_Del = 'false'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取角色信息出错" + ex.Message.ToString());
            }
        }
        protected DataTable GetSysSourceList()
        {
            try
            {
                string strSQL;
                strSQL = "Select * From [dbo].[Sys_Permission] order by PermType";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取资源信息出错" + ex.Message.ToString());
            }
        }
        protected DataTable GetSysSourceList(string roleID)
        {
            try
            {
                string strSQL = @"Select b.ID, SignName, PermissionSign, ParentId From [dbo].[Sys_RoleInPermission] a
                                join [dbo].[Sys_Permission] b on a.PermissionID = b.ID Where a.RoleID = '" + roleID + "' order by PermType";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取角色权限信息出错" + ex.Message.ToString());
            }
        }
        //数据源
        protected void RadTreeListSource_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            RadTreeListSource.DataSource = GetSysSourceList();
        }

        //标签切换时
        protected void RadTabStripRoles_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            RadTreeListSource.ClearSelectedItems();
            isReload = false;
            RadTreeListSource.Rebind();
        }

        //根据角色权限勾选
        protected void RadTreeListSource_ItemDataBound(object sender, Telerik.Web.UI.TreeListItemDataBoundEventArgs e)
        {
            string roleID = this.RadTabStripRoles.SelectedTab.Value.ToString();
            DataTable dtRoleHasSource = GetSysSourceList(roleID);
            if (e.Item is TreeListDataItem)
            {
                if (!isReload)
                {
                    TreeListDataItem item = e.Item as TreeListDataItem;
                    if (dtRoleHasSource.Select("ID=" + item.GetDataKeyValue("ID")).Length > 0)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        //树完成
        protected void RadTreeListSource_PreRender(object sender, EventArgs e)
        {
            isReload = true;
        }

        //保存设置
        protected void RadButtonSaveConfig_Click(object sender, EventArgs e)
        {
            RadTreeListSource.ExpandAllItems();
            string strSQL;
            string roleID = this.RadTabStripRoles.SelectedTab.Value.ToString();
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = "Delete From [dbo].[Sys_RoleInPermission] Where [RoleID] = '" + roleID + "'";
                DBI.Execute(strSQL);
                if (RadTreeListSource.SelectedItems.Count > 0)
                {
                    foreach (TreeListDataItem item in RadTreeListSource.SelectedItems)
                    {
                        strSQL = "Insert Into [dbo].[Sys_RoleInPermission] ([RoleID],[PermissionID])" +
                            " Values ('" + roleID + "', '" + item.GetDataKeyValue("ID").ToString() + "')";
                        DBI.Execute(strSQL);
                    }
                }
                DBI.CommitTrans();
                RadNotificationAlert.Text = "保存成功！";
                RadNotificationAlert.Show();
            }
            catch (Exception ex)
            {
                DBI.RollbackTrans();
                throw new Exception("保存设置时出现异常" + ex.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }

        }
    }
}