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
    public partial class ComputationalFormula : System.Web.UI.Page
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
                    InitTable.Columns.Add("Dept");
                    InitTable.Columns.Add("DeptCode");
                    InitTable.Columns.Add("Shipping_Addr_Id");
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
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            userAccount = Session["UserName"].ToString();
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "ComputationalFormula", this.Page);
                GridSource = GetComputationalFormula();
            }
        }

        private DataTable GetComputationalFormula()
        {
            string strSQL = " select * from Sys_ComputationalFormula";
            return Common.AddTableRowsID(DBI.Execute(strSQL, true));
        }

        protected void RadGrid_ComputationalFormula_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid_ComputationalFormula.DataSource = GridSource;
        }

        protected void RadGrid_ComputationalFormula_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
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
                    string Parameter1 = changeRow["Parameter1"].ToString();
                    string Parameter2 = changeRow["Parameter2"].ToString();
                    string Parameter3 = changeRow["Parameter3"].ToString();
                    if (Parameter1 == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入参数1";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (Parameter2 == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入参数2";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (Parameter3 == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入参数3";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    try
                    {
                        Convert.ToDouble(Parameter1);
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！参数1：请输入数字";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    try
                    {
                        Convert.ToDouble(Parameter2);
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！参数2：请输入数字";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    try
                    {
                        Convert.ToDouble(Parameter3);
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！参数3：请输入数字";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    string strSQL = " Update Sys_ComputationalFormula set Parameter1 = '" + Parameter1 + "', Parameter2 = '" + Parameter2 + "' , Parameter3 = '" + Parameter3 + "' where Id = '" + id + "'";
                    DBI.Execute(strSQL);
                    RadNotificationAlert.Text = "修改成功！";
                    RadNotificationAlert.Show();
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
            RadGrid_ComputationalFormula.ExportSettings.FileName = "夹持量参数列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_ComputationalFormula.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid_ComputationalFormula.ExportSettings.FileName = "夹持量参数列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_ComputationalFormula.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid_ComputationalFormula.ExportSettings.FileName = "夹持量参数列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid_ComputationalFormula.ExportSettings.IgnorePaging = true;
            RadGrid_ComputationalFormula.MasterTableView.ExportToPdf();
            RadGrid_ComputationalFormula.ExportSettings.IgnorePaging = false;
        }
    }
}