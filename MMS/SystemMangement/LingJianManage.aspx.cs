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
    public partial class LingJianManage : System.Web.UI.Page
    {
        //声明全局数据
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
                    InitTable.Columns.Add("LingJian_Type_Code");
                    InitTable.Columns.Add("LingJian_Type_Name");
                    InitTable.Columns.Add("Is_BOM_Show");
                    InitTable.Columns.Add("Is_MDDLD_Show");
                    InitTable.Columns.Add("Is_Del");
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
        string DBConn;
        DBInterface DBI;
        private string userAccount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            userAccount = Session["UserName"].ToString();
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "LingJianManage", this.Page);
                GridSource = GetLingJianInfo();
            }
        }
        protected DataTable GetLingJianInfo()
        {
            string strSQL;
            try
            {
                strSQL = "Select Id, Convert(nvarchar(50), LingJian_Type_Code) as LingJian_Type_Code, LingJian_Type_Name, Is_BOM_Show, Is_MDDLD_Show, Is_Del From [dbo].[Sys_LingJian_Info]";
                return Common.AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("获取数零件类型信息表时出现异常" + e.Message.ToString());
            }
        }
       
        protected void RadGridLingJian_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridLingJian.DataSource = GridSource;
        }
        
        protected void RadGridLingJian_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string strSQL = "";
            if (e.CommandName == "Update")
            {
                GridEditableItem item = e.Item as GridEditableItem;
                string id = item.GetDataKeyValue("Id").ToString();

                DataRow changeRow = GridSource.Select("ID='" + id + "'")[0];

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, item);
                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        changeRow[(string)entry.Key] = entry.Value;
                    }
                    string LingJian_Type_Code = changeRow["LingJian_Type_Code"].ToString();
                    string LingJian_Type_Name = changeRow["LingJian_Type_Name"].ToString();
                    string Is_BOM_Show = changeRow["Is_BOM_Show"].ToString();
                    string Is_MDDLD_Show = changeRow["Is_MDDLD_Show"].ToString();
                    string Is_Del = changeRow["Is_Del"].ToString();
                    if (LingJian_Type_Code == "")
                    {
                        RadNotificationAlert.Text = "失败！没有零件类型编号";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (LingJian_Type_Name == "")
                    {
                        RadNotificationAlert.Text = "失败！没有零件类型名称";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    try
                    {
                        Convert.ToInt32(LingJian_Type_Code);
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！零件类型编号：请输入整数";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;

                    }
                    strSQL = " select count(*) from Sys_LingJian_Info where LingJian_Type_Code = '" + LingJian_Type_Code + "' and Is_Del = 'false' and Id <> '" + id + "'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！该零件类型编号已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    strSQL = " Update Sys_LingJian_Info set LingJian_Type_Code = '" + LingJian_Type_Code + "', LingJian_Type_Name = '" + LingJian_Type_Name + "'"
                        + " , Is_BOM_Show = '" + Is_BOM_Show + "' , Is_MDDLD_Show = '" + Is_MDDLD_Show + "', Is_Del = '" + Is_Del + "' where ID = '" + id + "'";
                    DBI.Execute(strSQL);

                    RadNotificationAlert.Text = "修改成功！";
                    RadNotificationAlert.Show();

                    GridSource = GetLingJianInfo();
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
                    string LingJian_Type_Code = newRow["LingJian_Type_Code"].ToString();
                    string LingJian_Type_Name = newRow["LingJian_Type_Name"].ToString();
                    string Is_BOM_Show = newRow["Is_BOM_Show"].ToString();
                    string Is_MDDLD_Show = newRow["Is_MDDLD_Show"].ToString();
                    string Is_Del = newRow["Is_Del"].ToString();

                    if (LingJian_Type_Code == "")
                    {
                        RadNotificationAlert.Text = "失败！没有零件类型编号";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (LingJian_Type_Name == "")
                    {
                        RadNotificationAlert.Text = "失败！没有零件类型名称";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    try
                    {
                        Convert.ToInt32(LingJian_Type_Code);
                    }
                    catch 
                    {
                        RadNotificationAlert.Text = "失败！零件类型编号：请输入整数";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    
                    }
                    strSQL = " select count(*) from Sys_LingJian_Info where LingJian_Type_Code = '" + LingJian_Type_Code + "' and Is_Del = 'false'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！该零件类型编号已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    strSQL = " Insert into Sys_LingJian_Info (LingJian_Type_Code, LingJian_Type_Name, Is_BOM_Show, Is_MDDLD_Show, Is_Del)";
                    strSQL += " values ('" + LingJian_Type_Code + "','" + LingJian_Type_Name + "','" + Is_BOM_Show + "','" + Is_MDDLD_Show + "','" + Is_Del + "')";
                    DBI.Execute(strSQL);

                    RadNotificationAlert.Text = "添加成功！";
                    RadNotificationAlert.Show();

                    GridSource = GetLingJianInfo();
                }
                catch (Exception ex)
                {
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
        }
      protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGridLingJian.ExportSettings.FileName = "零件信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridLingJian.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGridLingJian.ExportSettings.FileName = "零件信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridLingJian.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGridLingJian.ExportSettings.FileName = "零件信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridLingJian.ExportSettings.IgnorePaging = true;
            RadGridLingJian.MasterTableView.ExportToPdf();
            RadGridLingJian.ExportSettings.IgnorePaging = false;
        }
    }
}