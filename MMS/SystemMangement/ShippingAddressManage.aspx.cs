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
    public partial class ShippingAddressManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("KeyWordCode");
                    InitTable.Columns.Add("KeyWord");
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
                GridSource = GetShipAddr();
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = GridSource;
        }

        public DataTable GetShipAddr()
        {
            DataTable dt = new DataTable();

            string strSQL = " select * from Sys_Dict where TypeId = '2' and Is_Del = 'false' order by KeyWord";
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

                strSQL = " if (select count(*) from Sys_Dept_ShipAddr where Shipping_Addr_ID = ('2-' + (select Convert(nvarchar(50),KeyWordCode) from Sys_Dict where ID = '" + id + "'))";
                strSQL += " and Dept_ID in (select ID from Sys_DeptEnum where Is_del='false') and Is_del = 'false') = 0 begin";
                strSQL += " Update Sys_Dict set Is_del = 'true' where ID = '" + id + "'";
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
                        RadNotificationAlert.Text = "失败！该地址已部门管理引用，不可以删除";
                        RadNotificationAlert.Show();
                    }
                }
                catch(Exception ex)
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

                    string KeyWord = changeRow["KeyWord"].ToString();
                    if (KeyWord == "")
                    {
                        RadNotificationAlert.Text = "失败！没有配送地址";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }
                    strSQL = " if (select count(*) from Sys_Dict where ID <> '" + id + "' and TypeID = '2' and KeyWord = '" + KeyWord + "') = 0 begin";
                    strSQL += " Update Sys_Dict set KeyWord = '" + KeyWord + "' where ID = '" + id + "'";
                    strSQL += " select '0' end else begin select '1' end";
                    if (DBI.GetSingleValue(strSQL).ToString() == "0")
                    {
                        RadNotificationAlert.Text = "修改成功！";
                        RadNotificationAlert.Show();
                    }
                    else
                    {
                        changeRow["KeyWord"] = item.SavedOldValues["KeyWord"].ToString();
                        RadNotificationAlert.Text = "失败！已有相同配送地址，请更换另一个名称";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
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
                    string KeyWord = newRow["KeyWord"].ToString();
                    if (KeyWord == "")
                    {
                        RadNotificationAlert.Text = "失败！没有配送地址";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }

                    strSQL = " if (select count(*) from Sys_Dict where TypeID = '2' and KeyWord = '" + KeyWord + "' and Is_Del = 'false') = 0 begin";
                    strSQL += " insert into Sys_Dict (TypeID, KeyWordCode, KeyWord, Is_Del)  values ('2'";
                    strSQL += " , (select max(isnull(KeyWordCode,0)) + 1 from Sys_Dict where TypeID = '2') ,'" + KeyWord + "','false')";
                    strSQL += " select '0' end else begin select '1' end";
                    if (DBI.GetSingleValue(strSQL).ToString() == "0")
                    {
                        RadNotificationAlert.Text = "添加成功！";
                        RadNotificationAlert.Show();

                        GridSource = GetShipAddr();
                        RadGrid1.Rebind();
                    }
                    else
                    {
                        RadNotificationAlert.Text = "失败！已有该配送地址，请更换另一个名称";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }

                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
        }
    }
}