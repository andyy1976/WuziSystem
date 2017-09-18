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
    public partial class ModelManage : System.Web.UI.Page
    {
        private static string DBConn;
        private DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); } 
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "ModelManage", this.Page);
                this.ViewState["GridSourceModel"] = GetModel();
                this.ViewState["GridSourceArea"] = GetArea();
            }
        }

        private DataTable GetArea()
        {
            string strSQL = "select * from Sys_Area where IsDel = 'false' order by AreaCode ";
            return Common.AddTableRowsID(DBI.Execute(strSQL, true));
        }

        private DataTable GetModel()
        {
            string strSQL = " select a.* , b.AreaName, b.AreaCode from Sys_Model as a left join Sys_Area as b on b.Id = a.AreaId where a.Is_Del='false' order by Model, AreaCode";
            return Common.AddTableRowsID(DBI.Execute(strSQL, true));
        }

        protected void RadGridModel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridModel.DataSource = this.ViewState["GridSourceModel"];
        }

        protected void RadGridModel_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string strSQL = "";
            if (e.CommandName == "Update")
            {
                GridEditableItem item = e.Item as GridEditableItem;
                string Id = item.GetDataKeyValue("Id").ToString();
                DataTable ordersTable = this.ViewState["GridSourceModel"] as DataTable;

                DataRow[] changedRows = ordersTable.Select("Id='" + Id + "'");

                if (changedRows.Length != 1)
                {
                    e.Canceled = true;
                    return;
                }

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, item);
                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    string Model = changedRows[0]["Model"].ToString();
                    string Area = changedRows[0]["AreaId"].ToString();
                    string IsGetBOM = changedRows[0]["IsGetBOM"].ToString();
                    if (Area == "0") { Area = ""; }

                    if (Model == "")
                    {
                        RadNotificationAlert.Text = "失败！没有录入型号";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }

                    strSQL = " if ('" + Area + "' != '' and (select count(*) from Sys_Area where Id = '" + Area + "' and IsDel = 'false') = 0) begin select '-1' end";
                    strSQL += " else begin if( select count(*) from Sys_Model where Is_Del = 'false' and ID <> '" + Id + "' and Model = '" + Model + "' and AreaId = '" + Area + "') = 0";
                    strSQL += " begin Update Sys_Model set Model = '" + Model + "', AreaId = '" + Area + "', IsGetBOM = '" + IsGetBOM + "' where ID = '" + Id + "' select '0' end";
                    strSQL += " else begin select '1' end end";
                    string result = DBI.GetSingleValue(strSQL);
                    if (result == "0")
                    {
                        RadNotificationAlert.Text = "修改成功！";
                        RadNotificationAlert.Show();

                        RadGridModel.Rebind();
                    }
                    else if (result == "-1")
                    {
                        RadNotificationAlert.Text = "失败！该地区不可用，请更换另一个";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }
                    else
                    {
                        RadNotificationAlert.Text = "失败！该型号已经存在";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！"+ex.Message.ToString();
                    RadNotificationAlert.Show();

                    e.Canceled = true;
                }
            }
            if (e.CommandName == "PerformInsert")
            {
                GridEditableItem item = e.Item as GridEditableItem;
                DataTable ordersTable = this.ViewState["GridSourceModel"] as DataTable;
                DataRow newRow = ordersTable.NewRow();

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, item);

                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }
                    string Model = newRow["Model"].ToString();
                    string Area = newRow["AreaId"].ToString();
                    string IsGetBOM = newRow["IsGetBOM"].ToString();
                    if (Model == "")
                    {
                        RadNotificationAlert.Text = "失败！没有录入型号";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }

                    strSQL = " if( select count(*) from Sys_Model where Is_Del = 'false' and Model = '" + Model + "' and AreaId = '" + Area + "') = 0";
                    strSQL += " begin Insert into Sys_Model (Model, AreaId, IsGetBOM, Is_Del) values ('" + Model + "', '" + Area + "','" + IsGetBOM + "' ,'false') select '0' end";
                    strSQL += " else begin select '1' end";
                    string result = DBI.GetSingleValue(strSQL);
                    if (result == "0")
                    {
                        RadNotificationAlert.Text = "新增成功！";
                        RadNotificationAlert.Show();
                        this.ViewState["GridSourceModel"] = GetModel();
                        RadGridModel.Rebind();
                    }
                    else {
                        RadNotificationAlert.Text = "失败！该型号已经存在";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();

                    e.Canceled = true;
                    return;
                }
            }
            if (e.CommandName == "delete")
            {
                GridEditableItem item = e.Item as GridEditableItem;
                string Id = item.GetDataKeyValue("Id").ToString();
                try
                {
                    strSQL = " if (select count(*) from P_Pack where Model = '" + Id + "' and IsDel = 'false') = 0 ";
                    strSQL += " begin delete Sys_Model where ID = '" + Id + "' select '0' end else begin select '1' end";
                    string result = DBI.GetSingleValue(strSQL);

                    if (result == "0")
                    {
                        (this.ViewState["GridSourceModel"] as DataTable).Select("Id='" + Id + "'")[0].Delete();
                        RadNotificationAlert.Text = "删除成功！";
                        RadNotificationAlert.Show();
                        RadGridModel.Rebind();
                    }
                    else
                    {
                        RadNotificationAlert.Text = "失败！该项已使用，不可以删除";
                        RadNotificationAlert.Show();
                    }


                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
        }

        protected void RadGridArea_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridArea.DataSource = this.ViewState["GridSourceArea"];
        }

        protected void RadGridArea_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string strSQL = "";
            if (e.CommandName == "PerformInsert")
            {
                DataRow newRow = (this.ViewState["GridSourceArea"] as DataTable).NewRow();
                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, (GridEditableItem)e.Item);
                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }
                    string AreaCode = newRow["AreaCode"].ToString();
                    string AreaName = newRow["AreaName"].ToString();
                    if (AreaCode == "")
                    {
                        RadNotificationAlert.Text = "失败！没有地区代码";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (AreaName == "")
                    {
                        RadNotificationAlert.Text = "失败！没有地区名称";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    strSQL = " select count(*) from Sys_Area where AreaCode ='" + AreaCode + "' and IsDel = 'false'";
                    if (DBI.GetSingleValue(strSQL) == "0")
                    {
                        strSQL = " insert into Sys_Area (AreaCode, AreaName, IsDel) values ('" + AreaCode + "','" + AreaName + "','false') select * from Sys_Area where IsDel = 'false' order by AreaCode ";

                        this.ViewState["GridSourceArea"] = Common.AddTableRowsID(DBI.Execute(strSQL, true));
                      
                        RadGridArea.Rebind();
                    }
                    else
                    {
                        RadNotificationAlert.Text = "失败！该地区代码已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                }
                catch (Exception ex) {
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                    e.Canceled = true;
                    return;
                }
            }
            if (e.CommandName == "Update")
            {
                GridEditableItem item = e.Item as GridEditableItem;
                string id = item.GetDataKeyValue("Id").ToString();

                DataTable ordersTable = this.ViewState["GridSourceArea"] as DataTable;

                DataRow[] changedRows = ordersTable.Select("Id='" + id + "'");

                if (changedRows.Length != 1)
                {
                    e.Canceled = true;
                    return;
                }

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, item);

             
                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        changedRows[0][(string)entry.Key] = entry.Value;
                    }
                    string AreaCode = changedRows[0]["AreaCode"].ToString();
                    string AreaName = changedRows[0]["AreaName"].ToString();

                    if (AreaCode == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入地区代码";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (AreaName == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入地区名称";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    strSQL = " if (select count(*) from Sys_Area where Id <> '" + id + "' and AreaCode = '" + AreaCode + "') = 0"
                        + " begin Update Sys_Area set AreaCode = '" + AreaCode + "', AreaName = '" + AreaName + "' where Id = '" + id + "' select '0' end"
                        + " else begin select '1' end";
                    if (DBI.GetSingleValue(strSQL) == "0")
                    {
                        RadNotificationAlert.Text = "修改成功！";
                        RadNotificationAlert.Show();

                        RadGridArea.Rebind();
                    }
                    else
                    {
                        RadNotificationAlert.Text = "失败！修改后的地区代码已经存在";
                        RadNotificationAlert.Show();

                        e.Canceled = true;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("修改失败！" + ex.Message.ToString());
                }
            }
            if (e.CommandName == "delete")
            {
                string id = ((GridDataItem)e.Item).GetDataKeyValue("Id").ToString();
                strSQL = "if  (select count(*) from Sys_Model where Is_Del = 'false' and AreaId = '" + id + "') = 0"
                    + " begin delete Sys_Area where Id = '" + id + "' select '0' end"
                    + " else begin select '1' end";
                if (DBI.GetSingleValue(strSQL) == "0")
                {
                    RadNotificationAlert.Text = "删除成功！";
                    RadNotificationAlert.Show();

                    (this.ViewState["GridSourceArea"] as DataTable).Select("Id='" + id + "'")[0].Delete();

                    RadGridArea.Rebind();
                }
                else
                {
                    RadNotificationAlert.Text = "失败！型号管理中已经使用，不可以删除";
                    RadNotificationAlert.Show();
                }
                
            }
        }
		
		protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGridModel.ExportSettings.FileName = "型号列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridModel.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGridModel.ExportSettings.FileName = "型号列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridModel.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGridModel.ExportSettings.FileName = "型号列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridModel.ExportSettings.IgnorePaging = true;
            RadGridModel.MasterTableView.ExportToPdf();
            RadGridModel.ExportSettings.IgnorePaging = false;
        }
    }
}