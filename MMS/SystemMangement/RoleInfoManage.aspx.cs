using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Camc.Web.Library;
using Telerik.Web.UI;
using System.Collections;

namespace mms.SystemMangement
{
    public partial class RoleInfoManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("RolsName");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["RowsId"] };       //设置RowsId列为主键，用于datatable删除
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
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true); }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                GridSource = GetRole();
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = GridSource;
        }

        public DataTable GetRole()
        {
            DataTable dt = new DataTable();

            string strSQL = " select * from Sys_RoleInfo where Is_del = 'false' order by RoleName";
            dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));

            return dt;
        }

        protected void RadGrid1_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string strSQL = "";
            if (e.CommandName == "delete")
            {
                GridDataItem item = e.Item as GridDataItem;
                string id = item.GetDataKeyValue("ID").ToString();

                strSQL = " if ((select count(*) from Sys_UserInfo_PWD where Dept = '" + id +"' and IsDel= 'false') = 0";
                strSQL += " and (select count(*) from Sys_RoleInPermission where RoleId = '" + id + "') = 0) begin";
                strSQL += " Update Sys_RoleInfo set Is_del = 'true' where ID = '" + id + "' delete Sys_RoleInPermission where RoleId = '" + id + "'";
                strSQL += " select '0' end else begin select '1' end";
                try
                {
                    if (DBI.GetSingleValue(strSQL).ToString() == "0")
                    {
                        GridSource.Rows.Remove(GridSource.Select("ID='" + id + "'")[0]);
                        RadGrid1.Rebind();
                        RadNotificationAlert.Text = "删除成功！";
                        RadNotificationAlert.Show();
                    }
                    else
                    {
                        RadNotificationAlert.Text = "失败！该角色已被引用，不可以删除";
                        RadNotificationAlert.Show();
                    }
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
            if (e.CommandName == "Update")
            {
                GridEditableItem item = e.Item as GridEditableItem;
                string id = item.GetDataKeyValue("ID").ToString();

                DataRow changeRow = GridSource.Select("ID='" + id + "'")[0];

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, item);
                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        changeRow[(string)entry.Key] = entry.Value;
                    }

                    string RoleName = changeRow["RoleName"].ToString();
                    if (RoleName == "")
                    {
                        RadNotificationAlert.Text = "失败！没有角色名称";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }
                    strSQL = " if (select count(*) from Sys_RoleInfo where RoleName = '" + RoleName + "' and ID <> '" + id + "'  and Is_del = 'false') = 0 begin";
                    strSQL += " Update Sys_RoleInfo set RoleName = '" + RoleName + "' where ID = '" + id + "'";
                    strSQL += " select '0' end else begin select '1' end";
                    if (DBI.GetSingleValue(strSQL).ToString() == "0")
                    {
                        RadNotificationAlert.Text = "修改成功！";
                        RadNotificationAlert.Show();
                    }
                    else
                    {
                        changeRow["RoleName"] = item.SavedOldValues["RoleName"].ToString();
                        RadNotificationAlert.Text = "失败！已有该角色，请更换另一个名称";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                    }
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
            if (e.CommandName == "PerformInsert")
            {
                GridEditableItem item = e.Item as GridEditableItem;
                DataTable ordersTable = GridSource;
                DataRow newRow = ordersTable.NewRow();

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, item);

                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }
                    string RoleName = newRow["RoleName"].ToString();
                    if (RoleName == "")
                    {
                        RadNotificationAlert.Text = "失败！没有角色名称";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }

                    strSQL = " if (select count(*) from Sys_RoleInfo where RoleName = '" + RoleName + "' and Is_del = 'false') = 0 begin"
                        + " Insert into Sys_RoleInfo (RoleName, Is_del) values ('" + RoleName + "','false')"
                        + " select '0' end else begin select '1' end";
                    if (DBI.GetSingleValue(strSQL).ToString() == "0")
                    {
                        RadNotificationAlert.Text = "添加成功！";
                        RadNotificationAlert.Show();

                        GridSource = GetRole();
                        RadGrid1.Rebind();
                    }
                    else
                    {
                        RadNotificationAlert.Text = "失败！已有该角色，请更换另一个名称";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }

                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！"+ ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
        }
    }
}