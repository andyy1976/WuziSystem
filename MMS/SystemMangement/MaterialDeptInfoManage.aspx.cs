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
    public partial class MaterialDeptInfoManage : System.Web.UI.Page
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
                    InitTable.Columns.Add("Technics_Line");
                    InitTable.Columns.Add("Dept_Id");
                    InitTable.Columns.Add("Technics_Line_Len");
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
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (Session["UserName"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            userAccount = Session["UserName"].ToString();
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "MaterialDeptInfoManage", this.Page);
                GridSource = GetMaterialDeptInfo();
            }
        }
        protected DataTable GetMaterialDeptInfo()
        {
            string strSQL;
            try
            {
                strSQL = " select ID, Technics_Line, Dept_Id, Convert(nvarchar(50),Technics_Line_Len) as Technics_Line_Len, Is_Del from Sys_MaterialDeptInfo";
                return Common.AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception e)
            {
                throw new Exception("获取数据时出现异常" + e.Message.ToString());
            }
        }
       
        protected void RadGridMaterialDeptInfo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridMaterialDeptInfo.DataSource = GridSource;
        }

        protected void RadGridMaterialDeptInfo_ItemCommand(object sender, GridCommandEventArgs e)
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
                    string Technics_Line = changeRow["Technics_Line"].ToString();
                    string Dept_Id = changeRow["Dept_Id"].ToString();
                    //string Technics_Line_Len = changeRow["Technics_Line_Len"].ToString();
                    string Is_Del = changeRow["Is_Del"].ToString();
                    if (Technics_Line == "")
                    {
                        RadNotificationAlert.Text = "失败！没有工艺路线";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    //if (Technics_Line_Len == "")
                    //{
                    //    RadNotificationAlert.Text = "失败！没有前缀长度";
                    //    RadNotificationAlert.Show();
                    //    e.Canceled = true;
                    //    return;
                    //}
                    //try
                    //{
                    //    Convert.ToInt32(Technics_Line_Len);
                    //}
                    //catch
                    //{
                    //    RadNotificationAlert.Text = "失败！前缀长度：请输入整数";
                    //    RadNotificationAlert.Show();
                    //    e.Canceled = true;
                    //    return;

                    //}
                    //strSQL = " select count(*) from Sys_MaterialDeptInfo where Technics_Line = '" + Technics_Line + "' and Dept_Id = '" + Dept_Id + "' and Technics_Line_Len = '" + Technics_Line_Len + "' and Is_Del = 'false' and Id <> '" + id + "'";
                    strSQL = " select count(*) from Sys_MaterialDeptInfo where Technics_Line = '" + Technics_Line + "' and Dept_Id = '" + Dept_Id + "' and Is_Del = 'false' and Id <> '" + id + "'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！已有相同的记录，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    strSQL = " Update Sys_MaterialDeptInfo set Technics_Line = '" + Technics_Line + "', Dept_Id = '" + Dept_Id + "'"
                        //+ " , Technics_Line_Len = '" + Technics_Line_Len + "' , Is_Del = '" + Is_Del + "' where ID = '" + id + "'";
                        + " , Is_Del = '" + Is_Del + "' where ID = '" + id + "'";
                    DBI.Execute(strSQL);

                    RadNotificationAlert.Text = "修改成功！";
                    RadNotificationAlert.Show();

                    GridSource = GetMaterialDeptInfo();
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
                    string Technics_Line = newRow["Technics_Line"].ToString();
                    string Dept_Id = newRow["Dept_Id"].ToString();
                    //string Technics_Line_Len = newRow["Technics_Line_Len"].ToString();
                    string Is_Del = newRow["Is_Del"].ToString();
                    if (Technics_Line == "")
                    {
                        RadNotificationAlert.Text = "失败！没有工艺路线";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    //if (Technics_Line_Len == "")
                    //{
                    //    RadNotificationAlert.Text = "失败！没有前缀长度";
                    //    RadNotificationAlert.Show();
                    //    e.Canceled = true;
                    //    return;
                    //}
                    //try
                    //{
                    //    Convert.ToInt32(Technics_Line_Len);
                    //}
                    //catch
                    //{
                    //    RadNotificationAlert.Text = "失败！前缀长度：请输入整数";
                    //    RadNotificationAlert.Show();
                    //    e.Canceled = true;
                    //    return;

                    //}
                    //strSQL = " select count(*) from Sys_MaterialDeptInfo where Technics_Line = '" + Technics_Line + "' and Dept_Id = '" + Dept_Id + "' and Technics_Line_Len = '" + Technics_Line_Len + "' and Is_Del = 'false'";
                    strSQL = " select count(*) from Sys_MaterialDeptInfo where Technics_Line = '" + Technics_Line + "' and Dept_Id = '" + Dept_Id + "' and Is_Del = 'false'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！已有相同的记录，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    strSQL = " Insert into Sys_MaterialDeptInfo (Technics_Line, Dept_Id, Technics_Line_Len, Is_Del)";
                    //strSQL += " values ('" + Technics_Line + "','" + Dept_Id + "','" + Technics_Line_Len + "','" + Is_Del + "')";
                    strSQL += " values ('" + Technics_Line + "','" + Dept_Id + "',Null,'" + Is_Del + "')";
                    DBI.Execute(strSQL);

                    RadNotificationAlert.Text = "添加成功！";
                    RadNotificationAlert.Show();

                    GridSource = GetMaterialDeptInfo();
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
            RadGridMaterialDeptInfo.ExportSettings.FileName = "工艺路线提取领料部门列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMaterialDeptInfo.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGridMaterialDeptInfo.ExportSettings.FileName = "工艺路线提取领料部门列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMaterialDeptInfo.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGridMaterialDeptInfo.ExportSettings.FileName = "工艺路线提取领料部门列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMaterialDeptInfo.ExportSettings.IgnorePaging = true;
            RadGridMaterialDeptInfo.MasterTableView.ExportToPdf();
            RadGridMaterialDeptInfo.ExportSettings.IgnorePaging = false;
        }
    }
}