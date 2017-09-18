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
    public partial class DictIndexManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("TypeID");
                    InitTable.Columns.Add("TypeDes");
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
                GridSource = Common.AddTableRowsID(GetDictIndex());
                this.ViewState["lastSelectItem"] = "";
                Common.CheckPermission(userAccount, "Allow_Visit_BannerManage_Page", this);
            }
        }
        protected DataTable GetDictIndex()
        {
            string strSQL;
            try
            {
                strSQL = "Select * From [dbo].[Sys_DictIndex]";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取数据字典索引表时出现异常" + e.Message.ToString());
            }
        }

        protected void RadGridDictIndex_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridDictIndex.DataSource = GridSource;
        }
        //****************菜单对象*******************//
        protected class DictIndexBody
        {
            public int ID { get; set; }
            public int TypeID { get; set; }
            public string TypeDes { get; set; }
        }
        /// <summary>
        /// 数据库操作-创建菜单
        /// </summary>
        protected void AddDictIndexItem(DictIndexBody dictidexbody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                //数据库操作
                strSQL = @"INSERT INTO [dbo].[Sys_DictIndex]
                               ([TypeID]
                              ,[TypeDes])
                               values ('" + dictidexbody.TypeID + "','" + dictidexbody.TypeDes + "')";
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
        protected void UpdateDictIndex(DictIndexBody dictidexbody)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                //更新数据库
                DBI.BeginTrans();
                //插入新任务并返回新的任务ID
                strSQL = @"UPDATE [dbo].[Sys_DictIndex]
                               SET [TypeID] = '" + dictidexbody.TypeID + "'" +
                                  ", [TypeDes] = '" + dictidexbody.TypeDes + "'" +
                             " WHERE [ID] = '" + dictidexbody.ID + "'";
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
        protected int ValidDictIndex(string DictID)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                strSQL = "select count(TypeID) from [dbo].[Sys_Dict] Where ID = '" + DictID + "'";
                return Convert.ToInt32(DBI.GetSingleValue(strSQL));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptID">部门ID</param>
        protected void DeleteDictIndex(string deptID)
        {
            string strSQL;
            DBI.OpenConnection();
            try
            {
                DBI.BeginTrans();
                int sum = ValidDictIndex(deptID);
                if (sum == 0)
                {

                    strSQL = "Delete from [dbo].[Sys_DictIndex] Where ID = '" + deptID + "'";
                    DBI.Execute(strSQL);
                }
                else
                {
                    RadNotificationAlert.Title = "该部门已经在其他表中使用，无法删除！";
                    RadNotificationAlert.AutoCloseDelay = 4000;
                    RadNotificationAlert.Show();
                }
                DBI.CommitTrans();
            }
            catch (Exception e)
            {
                DBI.RollbackTrans();
                throw new Exception("删除部门时出现异常" + e.Message.ToString());
            }
            finally
            {
                DBI.CloseConnection();
            }
        }

        protected void RadGridDictIndex_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            DataTable ordersTable = GridSource;
            DataRow newRow = ordersTable.NewRow();

            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            try
            {
                DictIndexBody newType = new DictIndexBody();
                foreach (DictionaryEntry entry in newValues)
                {
                    newRow[(string)entry.Key] = entry.Value;
                }

                newType.TypeDes = newRow["TypeDes"].ToString();
                try
                {
                    newType.TypeID = Convert.ToInt32(newRow["TypeID"].ToString());
                }
                catch
                {
                    newType.TypeID = 0;
                }

                AddDictIndexItem(newType);

                RadNotificationAlert.Text = "添加成功！";
                RadNotificationAlert.Show();
                GridSource = GetDictIndex();
                RadGridDictIndex.DataSource = GridSource;

            }
            catch (Exception ex)
            {
                throw new Exception("添加新部门失败！" + ex.Message.ToString());
            }
        }

        protected void RadGridDictIndex_ItemUpdated(object sender, GridCommandEventArgs e)
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
                DictIndexBody changeType = new DictIndexBody();
                foreach (DictionaryEntry entry in newValues)
                {
                    changedRows[0][(string)entry.Key] = entry.Value;
                }
                changeType.ID = Convert.ToInt32(changedRows[0]["ID"]);
                changeType.TypeDes = changedRows[0]["TypeDes"].ToString();
                changeType.TypeID = Convert.ToInt16(changedRows[0]["TypeID"].ToString());

                UpdateDictIndex(changeType);

                changedRows[0].EndEdit();
                RadNotificationAlert.Text = "更新成功！";
                RadNotificationAlert.Show();
                e.Canceled = false;
                RadGridDictIndex.Rebind();
            }
            catch (Exception ex)
            {
                changedRows[0].CancelEdit();
                e.Canceled = true;
            }
        }

        protected void RadGridDictIndex_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable table = GridSource;
                GridDataItem dataitem = e.Item as GridDataItem;
                DataTable Record = ((DataTable)RadGridDictIndex.DataSource);
                string deptID = Record.Rows[e.Item.DataSetIndex]["TypeID"].ToString();
                DeleteDictIndex(deptID);
                GridSource = GetDictIndex();
                RadGridDictIndex.DataSource = GridSource;
            }
        }

        protected void RadGridDictIndex_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!Common.IsHasRight(userAccount, "Allow_Edit_DictIndexManage_Page"))
            {
                if (e.Item is GridCommandItem)
                {
                    e.Item.FindControl("RadButton_AddNew").Visible = false;
                }
                if (e.Item is GridDataItem)
                {
                    RadGridDictIndex.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                    RadGridDictIndex.Columns.FindByUniqueName("DeleteColumn").Visible = false;
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
                    RadGridDictIndex.Columns.FindByUniqueName("EditCommandColumn").Visible = true;
                    RadGridDictIndex.Columns.FindByUniqueName("DeleteColumn").Visible = true;
                }
            }
        }

        protected DataTable GetOneDictIndex(string TypeDes)
        {
            string strSQL;
            try
            {
                strSQL = "Select * From [dbo].[Sys_DictIndex] Where ID = '" + TypeDes + "'";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取人员列表时出现异常" + e.Message.ToString());
            }
        }
        protected void RadGridDictIndex_ItemCreated(object sender, GridItemEventArgs e)
        {
            
        }

        protected void RadComboBoxTypeDes_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = string.Concat(e.Item.Text.ToLower().Split(' ')[0], "");
        }

        protected void RadComboBoxTypeDes_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string selectName = e.Value.ToString();
            this.ViewState["lastSelectItem"] = selectName;
            GridSource = Common.AddTableRowsID(GetOneDictIndex(selectName));
            this.RadGridDictIndex.Rebind();
        }

       
    }
}