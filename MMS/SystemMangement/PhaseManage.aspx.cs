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
    public partial class PhaseManage : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "PhaseManage", this.Page);
                this.ViewState["GridSource"] = GetPhase();
            }
        }

        private DataTable GetPhase()
        {
            string strSQL = " select Sys_Phase.*, GetBasicdata_T_Item.DICT_NAME from Sys_Phase " +
                " left join GetBasicdata_T_Item on GetBasicdata_T_Item.DICT_CODE = Sys_Phase.Basicdata_Dict_Code and GetBasicdata_T_Item.DICT_CLASS='CUX_DM_PHASE' order by Code";
            return Common.AddTableRowsID(DBI.Execute(strSQL, true));
        }

        protected void RadGridPhase_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridPhase.DataSource = this.ViewState["GridSource"];
        }

        protected void RadGridPhase_ItemCommand(object sender, GridCommandEventArgs e)
        {
             string strSQL = "";
             if (e.CommandName == "Update")
             {
                 GridEditableItem item = e.Item as GridEditableItem;
                 string Id = item.GetDataKeyValue("Id").ToString();
                 DataTable ordersTable = this.ViewState["GridSource"] as DataTable;

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
                     string Code = changedRows[0]["Code"].ToString();
                     string Phase = changedRows[0]["Phase"].ToString();
                     string Basicdata_DICT_Code = changedRows[0]["Basicdata_DICT_Code"].ToString();

                     if (Code == "")
                     {
                         RadNotificationAlert.Text = "失败！请输入序号";
                         RadNotificationAlert.Show();
                         e.Canceled = true;
                         return;
                     }
                     try { Convert.ToInt32(Code); }
                     catch {
                         RadNotificationAlert.Text = "失败！序号：请输入整数";
                         RadNotificationAlert.Show();
                         e.Canceled = true;
                         return;
                     }
                     if (Phase == "")
                     {
                         RadNotificationAlert.Text = "失败！请输入阶段代码";
                         RadNotificationAlert.Show();
                         e.Canceled = true;
                         return;
                     }
                     if (Basicdata_DICT_Code == "")
                     {
                         RadNotificationAlert.Text = "失败！请选择物资基础库中的名称";
                         RadNotificationAlert.Show();
                         e.Canceled = true;
                         return;
                     }

                     strSQL = " select count(*) from Sys_Phase where Code = '" + Code + "' and ID != '" + Id + "'";
                     if (DBI.GetSingleValue(strSQL).ToString() != "0")
                     {
                         RadNotificationAlert.Text = "失败！序号已经存在，请更换另一个";
                         RadNotificationAlert.Show();
                         e.Canceled = true;
                         return;
                     }
                     strSQL = " select count(*) from Sys_Phase where Phase = '" + Code + "' and ID != '" + Id + "'";
                     if (DBI.GetSingleValue(strSQL).ToString() != "0")
                     {
                         RadNotificationAlert.Text = "失败！阶段代码已经存在，请更换另一个";
                         RadNotificationAlert.Show();
                         e.Canceled = true;
                         return;
                     }
                     strSQL = " Update Sys_Phase set Code = '" + Code + "', Phase = '" + Phase + "', Basicdata_DICT_Code = '" + Basicdata_DICT_Code + "' where ID= '" + Id + "'";
                     DBI.Execute(strSQL);

                     RadNotificationAlert.Text = "修改成功！";
                     RadNotificationAlert.Show();
                     RadGridPhase.Rebind();
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
                DataTable ordersTable = this.ViewState["GridSource"] as DataTable;
                DataRow newRow = ordersTable.NewRow();

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, item);

                try
                {
                    foreach (DictionaryEntry entry in newValues)
                    {
                        newRow[(string)entry.Key] = entry.Value;
                    }
                    string Code = newRow["Code"].ToString();
                    string Phase = newRow["Phase"].ToString();
                    string Basicdata_DICT_Code = newRow["Basicdata_DICT_Code"].ToString();
                    if (Code == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入序号";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    try { Convert.ToInt32(Code); }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！序号：请输入整数";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (Phase == "")
                    {
                        RadNotificationAlert.Text = "失败！请输入阶段代码";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    if (Basicdata_DICT_Code == "")
                    {
                        RadNotificationAlert.Text = "失败！请选择物资基础库中的名称";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }

                    strSQL = " select count(*) from Sys_Phase where Code = '" + Code + "'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！序号已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }
                    strSQL = " select count(*) from Sys_Phase where Phase = '" + Code + "'";
                    if (DBI.GetSingleValue(strSQL).ToString() != "0")
                    {
                        RadNotificationAlert.Text = "失败！阶段代码已经存在，请更换另一个";
                        RadNotificationAlert.Show();
                        e.Canceled = true;
                        return;
                    }

                    strSQL = " insert into Sys_Phase (Code , Phase, Basicdata_DICT_Code) values ('" + Code + "','" + Phase + "','" + Basicdata_DICT_Code + "')";
                    DBI.Execute(strSQL);

                    RadNotificationAlert.Text = "添加成功！";
                    RadNotificationAlert.Show();
                    this.ViewState["GridSource"] = GetPhase();                    
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
                    strSQL = " delete Sys_Phase where ID = '" + Id + "'";
                    DBI.Execute(strSQL);

                    RadNotificationAlert.Text = "删除成功！";
                    RadNotificationAlert.Show();
                    this.ViewState["GridSource"] = GetPhase();          
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
            RadGridPhase.ExportSettings.FileName = "研制阶段列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridPhase.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGridPhase.ExportSettings.FileName = "研制阶段列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridPhase.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGridPhase.ExportSettings.FileName = "研制阶段列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridPhase.ExportSettings.IgnorePaging = true;
            RadGridPhase.MasterTableView.ExportToPdf();
            RadGridPhase.ExportSettings.IgnorePaging = false;
        }
    }
}