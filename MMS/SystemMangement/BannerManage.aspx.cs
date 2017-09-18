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
    public partial class BannerManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("ParentId");
                    InitTable.Columns.Add("ItemName");
                    InitTable.Columns.Add("ItemUrl");
                    InitTable.Columns.Add("PermissionnId");
                    InitTable.Columns.Add("OrderNo");
                    InitTable.Columns.Add("IsDel");
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

        //声明全局数据
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
                GridSource = Common.AddTableRowsID(GetBannerItem());
                this.ViewState["lastSelectItem"] = "";
                Common.CheckPermission(userAccount, "Allow_Visit_BannerManage_Page", this);
            }
        }
        protected DataTable GetBannerItem()
        {
            string strSQL;
            try
            {
                strSQL = "Select * From [dbo].[Sys_BannerItem]";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取菜单表时出现异常" + e.Message.ToString());
            }
        }

        protected void RadGridBannerManage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridBannerManage.DataSource = GridSource;
        }
        /// <summary>
        /// 数据库操作-创建菜单
        /// </summary>
        protected void AddBannerItem(BannerItemBody banneritembody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                //数据库操作
                strSQL = @"INSERT INTO [dbo].[Sys_BannerItem]
                               ([ParentId]
                              ,[ItemName]
                              ,[ItemUrl]
                              ,[PermissionId]
                              ,[OrderId]
                              ,[Enable])
                               values ('" + banneritembody.ParentId + "','" + banneritembody.ItemName + 
                                        "','" + banneritembody.ItemUrl + "','" + banneritembody.PermissionId + 
                                        "','" + banneritembody.OrderNo + "','" + banneritembody.Enable + "')";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("数据库操作-创建菜单时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        /// <summary>
        /// 数据库操作-修改菜单
        /// </summary>
        protected void UpdateBannerItem(BannerItemBody bannerItemBody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                //更新数据库
                DBI.BeginTrans();
                //插入新任务并返回新的任务ID
                strSQL = @"UPDATE [dbo].[Sys_BannerItem]
                               SET [ParentId] = '" + bannerItemBody.ParentId + "'" +
                                  ", [ItemName] = '" + bannerItemBody.ItemName + "'" +
                                  ", [ItemUrl] = '" + bannerItemBody.ItemUrl + "'" +
                                  ", [PermissionId] = '" + bannerItemBody.PermissionId + "'" +
                                  ", [OrderId] = '" + bannerItemBody.OrderNo + "'" +
                                  ", [Enable] = '" + bannerItemBody.Enable + "'" +
                             " WHERE [ID] = '" + bannerItemBody.ID + "'";
                DBI.Execute(strSQL);
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                RadNotificationAlert.Text = "数据库操作-修改菜单时出现异常" + e.Message.ToString();
                RadNotificationAlert.Show();
            }
            finally
            {
                DBI.CloseConnection();
            }
        }
        //****************菜单对象*******************//
        protected class BannerItemBody
        {
            public int ID { get; set; }
            public int ParentId { get; set; }
            public string ItemName { get; set; }
            public string ItemUrl { get; set; }
            public int PermissionId { get; set; }
            public int OrderNo { get; set; }
            public bool Enable { get; set; }
        }

        protected void RadGridBannerManage_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            DataTable ordersTable = GridSource;
            DataRow newRow = ordersTable.NewRow();

            DataRow[] allValues = ordersTable.Select("", "RowsId", DataViewRowState.CurrentRows);

            if (allValues.Length > 0)
            {
                newRow["RowsId"] = Convert.ToInt32(ordersTable.Rows[allValues.Length - 1]["RowsId"]) + 1;
            }
            else
            {
                newRow["RowsId"] = 1; //the table is empty;
            }

            //Set new values
            Hashtable newValues = new Hashtable();
            //The GridTableView will fill the values from all editable columns in the hash
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            try
            {
                BannerItemBody newType = new BannerItemBody();
                foreach (DictionaryEntry entry in newValues)
                {
                    newRow[(string)entry.Key] = entry.Value;
                }

                newType.Enable = true;
                newType.ItemName = newRow["ItemName"].ToString();
                newType.ItemUrl = newRow["ItemUrl"].ToString();
                try
                {
                    newType.OrderNo = Convert.ToInt32(newRow["OrderNo"].ToString());
                }
                catch
                {
                    newType.OrderNo = 0;
                }
                try
                {
                    newType.ParentId = Convert.ToInt32(newRow["ParentId"].ToString());
                }
                catch
                {
                    newType.ParentId = 0;
                }
                try
                {
                    newType.PermissionId = Convert.ToInt32(newRow["PermissionId"].ToString());
                }
                catch
                {
                    newType.PermissionId = 0;
                }
                try
                {
                    newType.Enable = Convert.ToBoolean(newRow["Enable"].ToString());
                }
                catch
                {
                    newType.Enable = false;
                }

                AddBannerItem(newType);

                RadNotificationAlert.Text = "添加成功！";
                RadNotificationAlert.Show();

                e.Canceled = false;
                GridSource = GetBannerItem();
                RadGridBannerManage.DataSource = GridSource;
            }
            catch (Exception ex)
            {
                throw new Exception("添加新菜单失败！" + ex.Message.ToString());
            }
        }

        protected void RadGridBannerManage_ItemUpdated(object sender, GridCommandEventArgs e)
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
                BannerItemBody changeType = new BannerItemBody();
                foreach (DictionaryEntry entry in newValues)
                {
                    changedRows[0][(string)entry.Key] = entry.Value;
                }
                changeType.ID = Convert.ToInt32(changedRows[0]["ID"]);
                changeType.ItemName = changedRows[0]["ItemName"].ToString();
                changeType.ItemUrl = changedRows[0]["ItemUrl"].ToString();
                try
                {
                    changeType.OrderNo = Convert.ToInt32(changedRows[0]["OrderNo"].ToString());
                }
                catch
                {
                    changeType.OrderNo = 0;
                }
                try
                {
                    changeType.ParentId = Convert.ToInt32(changedRows[0]["ParentId"].ToString());
                }
                catch
                {
                    changeType.ParentId = 0;
                }
                try
                {
                    changeType.PermissionId = Convert.ToInt32(changedRows[0]["PermissionId"].ToString());
                }
                catch
                {
                    changeType.PermissionId = 0;
                }
                try
                {
                    changeType.Enable = Convert.ToBoolean(changedRows[0]["Enable"].ToString());
                }
                catch
                {
                    changeType.Enable = false;
                }

                UpdateBannerItem(changeType);

                RadNotificationAlert.Text = "修改成功！";
                RadNotificationAlert.Show();
                changedRows[0].EndEdit();

                e.Canceled = false;
                GridSource = GetBannerItem();
                RadGridBannerManage.DataSource = GridSource;
            }
            catch (Exception ex)
            {
                changedRows[0].CancelEdit();
                e.Canceled = true;
            }
        }

        protected void RadGridBannerManage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!Common.IsHasRight(userAccount, "Allow_Edit_BannerManage_Page"))
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButtonAddTask").Visible = false;
                }
                if (e.Item is GridDataItem)
                {
                    RadGridBannerManage.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                }
            }
            else
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButtonAddTask").Visible = true;
                }
                if (e.Item is GridDataItem)
                {
                    RadGridBannerManage.Columns.FindByUniqueName("EditCommandColumn").Visible = true;
                }
            }
        }

        protected DataTable GetOneBannerItem(string ID)
        {
            string strSQL;
            try
            {
                strSQL = "Select * From [dbo].[Sys_BannerItem] Where ID ='" + ID + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取人员列表时出现异常" + e.Message.ToString());
            }
        }
        protected void RadGridBannerManage_ItemCreated(object sender, GridItemEventArgs e)
        {
            
        }

        protected void RadComboBoxBannerItem_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = string.Concat(e.Item.Text.ToLower().Split(' ')[0], "");
        }

        protected void RadComboBoxBannerItem_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string selectName = e.Value.ToString();
            this.ViewState["lastSelectItem"] = selectName;
            GridSource = Common.AddTableRowsID(GetOneBannerItem(selectName));
            this.RadGridBannerManage.Rebind();
        }
    }
}