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
    public partial class DictionaryManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("TypeDes");
                    InitTable.Columns.Add("KeyWordCode");
                    InitTable.Columns.Add("KeyWord");
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
                GridSource = Common.AddTableRowsID(GetDictList());
                Common.CheckPermission(userAccount, "Allow_Visit_DictManage_Page", this);
                this.ViewState["lastSelectItem"] = "";
            }
        }
        protected DataTable GetDictList()
        {
            try
            {
                string strSQL;
                strSQL = "Select * From VI_DictInfo";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获取数据字典信息出错" + ex.Message.ToString());
            }
        }

        protected void RadGridDictionary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridDictionary.DataSource = GridSource;
        }

        protected void RadGridDictionary_ItemCommand(object sender, GridCommandEventArgs e)
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
                    DictBody newType = new DictBody();
                    foreach (DictionaryEntry entry in newValues)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }

                    newType.KeyWord = newRow["KeyWord"].ToString();
                    try
                    {
                        newType.TypeID = Convert.ToInt32(newRow["TypeID"].ToString());
                    }
                    catch
                    {
                        newType.TypeID = 0;
                    }
                    try
                    {
                        newType.KeyWordCode = Convert.ToInt32(newRow["KeyWordCode"].ToString());
                    }
                    catch
                    {
                        newType.KeyWordCode = 0;
                    }
                    AddRoleInfo(newType);

                    RadNotificationAlert.Text = "添加成功！";
                    RadNotificationAlert.Show();
                    GridSource = GetDictList();
                    RadGridDictionary.DataSource = GridSource;

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
                    DictBody changeType = new DictBody();
                    foreach (DictionaryEntry entry in newValues)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    changeType.ID = Convert.ToInt32(changedRows[0]["ID"]);
                    changeType.KeyWord = changedRows[0]["KeyWord"].ToString();
                    try
                    {
                        changeType.KeyWordCode = Convert.ToInt32(changedRows[0]["KeyWordCode"].ToString());
                    }
                    catch
                    {
                        changeType.KeyWordCode = 0;
                    }
                    try
                    {
                        changeType.TypeID = Convert.ToInt32(changedRows[0]["TypeID"].ToString());
                    }
                    catch
                    {
                        changeType.TypeID = 0;
                    }

                    UpdateDictManage(changeType);

                    changedRows[0].EndEdit();
                    RadNotificationAlert.Text = "更新成功！";
                    RadNotificationAlert.Show();
                    e.Canceled = false;
                    RadGridDictionary.Rebind();
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
                DeleteDict(roleID);
                GridSource = GetDictList();
                RadGridDictionary.DataSource = GridSource;
            }
        }
        protected void AddRoleInfo(DictBody dictbody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = @"INSERT INTO [dbo].[Sys_Dict](TypeID, KeyWordCode, KeyWord) VALUES ('" + dictbody.TypeID + 
                    "','" + dictbody.KeyWordCode + "','" + dictbody.KeyWord + "')";
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
        protected void UpdateDictManage(DictBody dictbody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                //更新数据库
                DBI.BeginTrans();
                //插入新任务并返回新的任务ID
                strSQL = @"UPDATE [dbo].[Sys_Dict] SET TypeID = '" + dictbody.TypeID + 
                    "', KeyWordCode='" + dictbody.KeyWordCode + "', KeyWord='" + dictbody.KeyWord + 
                    "' WHERE [ID] = '" + dictbody.ID + "'";
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
        protected void DeleteDict(string DictID)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                strSQL = "Delete from [dbo].[Sys_Dict] Where ID = '" + DictID + "'";
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
        protected class DictBody
        {
            public int ID { get; set; }
            public int TypeID { get; set; }
            public int KeyWordCode { get; set; }
            public string TypeDes { get; set; }
            public string KeyWord { get; set; }
        }

        protected void RadGridDictionary_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!Common.IsHasRight(userAccount, "Allow_Edit_DictManage_Page"))
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButton_AddNew").Visible = false;
                }
                if (e.Item is GridDataItem)
                {
                    RadGridDictionary.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                    RadGridDictionary.Columns.FindByUniqueName("DeleteColumn").Visible = false;
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
                    RadGridDictionary.Columns.FindByUniqueName("EditCommandColumn").Visible = true;
                    RadGridDictionary.Columns.FindByUniqueName("DeleteColumn").Visible = true;
                }
            }
        }
        protected DataTable GetOneDict(string ID)
        {
            string strSQL;
            try
            {
                strSQL = "Select * From VI_DictInfo Where ID like '%" + ID + "%'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取人员列表时出现异常" + e.Message.ToString());
            }
        }

        protected void RadGridDictionary_ItemCreated(object sender, GridItemEventArgs e)
        {
            
        }

        protected void RadComboBoxDict_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = string.Concat(e.Item.Text.ToLower().Split(' ')[0], "");
        }

        protected void RadComboBoxDict_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string selectName = e.Value.ToString();
            this.ViewState["lastSelectItem"] = selectName;
            GridSource = Common.AddTableRowsID(GetOneDict(selectName));
            this.RadGridDictionary.Rebind();
        }
    }
}