using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using Telerik.Web.UI;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Configuration;
using Camc.Web.Library;
using DataTable = System.Data.DataTable;

namespace mms.Plan
{
    public partial class PlanImport : System.Web.UI.Page
    {
        private static System.Data.DataTable GridSource;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
            }
          
            if (!IsPostBack)
            {
                string DBContractConn =
                    ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString
                        .ToString();
                DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);

                GridSource = new System.Data.DataTable();
                string strSQL =
                    " select Model + '(' + AreaCode + '-' + AreaName + ')' as Model, Sys_Model.ID from Sys_Model join Sys_Area on Sys_Area.Id = Sys_Model.AreaId where Is_Del = 'false' order by Model, AreaCode";
                RDDL_Model.DataSource = DBI.Execute(strSQL, true);
                RDDL_Model.DataTextField = "Model";
                RDDL_Model.DataValueField = "ID";
                RDDL_Model.DataBind();

                strSQL = " select * from Sys_Phase order by Code";
                RDDL_Stage.DataSource = DBI.Execute(strSQL, true);
                RDDL_Stage.DataValueField = "Code";
                RDDL_Stage.DataTextField = "Phase";
                RDDL_Stage.DataBind();

                if (RDDL_Stage.FindItemByValue("3") != null)
                {
                    RDDL_Stage.FindItemByValue("3").Selected = true;
                }
            }
        }

        protected void RadGridP_Pack_Task_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                System.Web.UI.WebControls.Label hfield = e.Item.FindControl("HiddenField") as System.Web.UI.WebControls.Label;
                if (Request.QueryString["type"].ToString() == "1")
                {
                    hfield.Text = "企业备料计划任务列表";

                }
                else
                {
                    hfield.Text = "型号投产计划任务列表";
                }
               
            }
        }
        protected void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            HFGridItemsCount.Value = "0";
            if (RadAsyncUpload1.UploadedFiles.Count == 1)
            {
                //导入文件存在服务器上
                string filderPath = Server.MapPath(@"~\Plan\ImportExcel\");
                if (!System.IO.Directory.Exists(filderPath))
                {
                    System.IO.Directory.CreateDirectory(filderPath);
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") +
                                  RadAsyncUpload1.UploadedFiles[0].FileName;
                string filePath = Path.Combine(filderPath, fileName);
                RadAsyncUpload1.UploadedFiles[0].SaveAs(filePath);

                HFFileName.Value = fileName;

                string conn = " Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath(@"~\Plan\ImportExcel\") +
                              "\\" + fileName + "; Extended Properties ='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                OleDbConnection thisconnection = new OleDbConnection(conn);

                try
                {
                    if (fileName != "")
                    {
                        thisconnection.Open();

                        string sql = " select * from [Sheet1$]";
                        OleDbDataAdapter command = new OleDbDataAdapter(sql, thisconnection);
                        DataSet ds = new DataSet();
                        command.Fill(ds, "[Sheet1$]");

                        thisconnection.Close();

                        GridSource = ds.Tables[0];
                        int columnscount = 0;
                        for (int i = 0; i < GridSource.Columns.Count; i++)
                        {
                            switch (GridSource.Columns[i].ColumnName.Trim())
                            {
                                case "产品名称":
                                    GridSource.Columns[i].ColumnName = "ProductName";
                                    columnscount++;
                                    break;
                                case "产品图号":
                                    GridSource.Columns[i].ColumnName = "TaskDrawingCode";
                                    columnscount++;
                                    break;
                                case "任务号":
                                    GridSource.Columns[i].ColumnName = "TaskCode";
                                    columnscount++;
                                    break;
                                case "计量单位":
                                    GridSource.Columns[i].ColumnName = "Unit";
                                    columnscount++;
                                    break;
                                case "单机配套数量":
                                    GridSource.Columns[i].ColumnName = "MatingNum";
                                    columnscount++;
                                    break;
                                case "交付总数量":
                                    GridSource.Columns[i].ColumnName = "DefrayNum";
                                    columnscount++;
                                    break;
                                case "本次投产数量":
                                    GridSource.Columns[i].ColumnName = "ProductionNum";
                                    columnscount++;
                                    break;
                                case "计划交付时间":
                                    GridSource.Columns[i].ColumnName = "PlanFinishTime";
                                    columnscount++;
                                    break;
                            }
                        }
                        if (columnscount < 8)
                        {
                            GridSource = new System.Data.DataTable();
                            RadGridPack.Rebind();
                            File.Delete(filePath);
                            RadNotificationAlert.Text = "失败！请参照页面表头";
                            RadNotificationAlert.Show();
                            return;
                        }

                        GridSource.Columns.Add("RowsID");
                        int rowsid = 1;
                        for (int i = 0; i < GridSource.Rows.Count; i++)
                        {
                            if (GridSource.Rows[i]["ProductName"].ToString() != "")
                            {
                                GridSource.Rows[i]["RowsID"] = rowsid;
                                rowsid++;
                            }
                            else
                            {
                                GridSource.Rows[i].Delete();
                            }
                        }

                        RadGridPack.Rebind();
                        HFGridItemsCount.Value = RadGridPack.Items.Count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    GridSource = new System.Data.DataTable();
                    RadGridPack.Rebind();
                    File.Delete(filePath);

                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
            }
            else
            {
                GridSource = new System.Data.DataTable();
                RadGridPack.Rebind();

                RadNotificationAlert.Text = "请选择文件";
                RadNotificationAlert.Show();
                return;
            }
        }

        protected void RadGridPack_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridPack.DataSource = GridSource;
        }

        protected void RB_Temporary_Click(object sender, EventArgs e)
        {
            if (RTB_Name.Text == "")
            {
                RadNotificationAlert.Text = "失败！请输入计划包名称";
                RadNotificationAlert.Show();
                return;
            }
            if (GridSource.Rows.Count == 0)
            {
                RadNotificationAlert.Text = "失败！没有可导入数据";
                RadNotificationAlert.Show();
                return;
            }

            InsertPackTask("1");
        }


        protected void RBAdd_Click(object sender, EventArgs e)
        {
            if (RTB_Name.Text == "")
            {
                RadNotificationAlert.Text = "失败！请输入计划包名称";
                RadNotificationAlert.Show();
                return;
            }
            if (RadGridPack.Items.Count == 0)
            {
                RadNotificationAlert.Text = "失败！没有可导入数据";
                RadNotificationAlert.Show();
                return;
            }
            for (int i = 0; i < RadGridPack.Items.Count; i++)
            {
                RadioButtonList rbl = RadGridPack.Items[i].FindControl("RBL_IsSpread") as RadioButtonList;
                if (rbl != null)
                {
                    if (rbl.SelectedValue == "" || rbl.SelectedValue == null)
                    {
                        RadNotificationAlert.Text = "不可以归档！第" + (i + 1).ToString() + "行，没有选择是否展开";
                        RadNotificationAlert.Show();
                        return;
                    }
                }
            }

            InsertPackTask("2");
        }

        private void InsertPackTask(string PackState)
        {
            string DBContractConn =
                ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString
                    .ToString();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
            string planName = RTB_Name.Text.Trim();
            string remark = RTB_Remark.Text.Trim();
            for (int i = 0; i < RadGridPack.Items.Count; i++)
            {
                if (RadGridPack.Items[i] is GridDataItem)
                {
                    GridDataItem item = RadGridPack.Items[i] as GridDataItem;

                    string ProductName = item["ProductName"].Text.Trim();
                    string TaskDrawingCode = item["TaskDrawingCode"].Text.Trim();
                    string TaskCode = item["TaskCode"].Text.Trim();
                    string Unit = item["Unit"].Text.Trim();
                    string MatingNum = item["MatingNum"].Text.Trim();
                    string DefrayNum = item["DefrayNum"].Text.Trim();
                    string ProductionNum = item["ProductionNum"].Text.Trim();
                    string PlanFinishTime = item["PlanFinishTime"].Text.Trim();

                    if (ProductName == "" || ProductName == "&nbsp;")
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，产品名称：请输入产品名称";
                        RadNotificationAlert.Show();
                        return;
                    }
                    if (TaskDrawingCode == "" || TaskDrawingCode == "&nbsp;")
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，产品图号：请输入产品图号";
                        RadNotificationAlert.Show();
                        return;
                    }
                    if (TaskCode == "" || TaskCode == "&nbsp;")
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，任务号：请输入任务号";
                        RadNotificationAlert.Show();
                        return;
                    }
                    if (Unit == "" || Unit == "&nbsp;")
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，计量单位：请输入计量单位";
                        RadNotificationAlert.Show();
                        return;
                    }
                    try
                    {
                        Convert.ToInt32(MatingNum);
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，单机配套数量：请输入数字";
                        RadNotificationAlert.Show();
                        return;
                    }
                    try
                    {
                        Convert.ToInt32(DefrayNum);
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，交付总数量：请输入数字";
                        RadNotificationAlert.Show();
                        return;
                    }
                    try
                    {
                        Convert.ToInt32(ProductionNum);
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，本次投产数量：请输入数字";
                        RadNotificationAlert.Show();
                        return;
                    }
                    try
                    {
                        Convert.ToDateTime(PlanFinishTime);
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，计划交付时间：不是有效日期";
                        RadNotificationAlert.Show();
                        return;
                    }
                }
            }

            string UserID = Session["UserId"].ToString();

            string strSQL = "";
            try
            {
                string Model = RDDL_Model.SelectedItem.Value.ToString();
                string Stage = RDDL_Stage.SelectedItem.Value;
                string PlanCode = DBI.GetSingleValue(" Exec [Proc_CodeBuildByCodeDes1] '型号投产计划编号','XH'");
                string Type = Request.QueryString["Type"].ToString();
                strSQL =
                    " insert into P_Pack (Model, PlanCode, ImportFileName, ImportStaffId, ImportTime,DraftStatus, InvertoryStatus, IsDel, state, PlanName, Remark,Type) values";
                strSQL += " ('" + Model + "', '" + PlanCode + "','" + HFFileName.Value + "', '" + UserID +
                          "', Getdate(),'1','1','false','" + PackState + "','" + planName + "','" + remark + "',"+Type+")";
                strSQL += " select @@identity";
                string PackID = DBI.GetSingleValue(strSQL);

                for (int i = 0; i < RadGridPack.Items.Count; i++)
                {
                    if (RadGridPack.Items[i] is GridDataItem)
                    {
                        GridDataItem item = RadGridPack.Items[i] as GridDataItem;
                        string ProductName = item["ProductName"].Text.Trim();
                        string TaskDrawingCode = item["TaskDrawingCode"].Text.Trim();
                        string TaskCode = item["TaskCode"].Text.Trim();
                        string Unit = item["Unit"].Text.Trim();
                        string MatingNum = item["MatingNum"].Text.Trim();
                        string DefrayNum = item["DefrayNum"].Text.Trim();
                        string ProductionNum = item["ProductionNum"].Text.Trim();
                        string PlanFinishTime = item["PlanFinishTime"].Text.Trim();
                        RadioButtonList rbl = item.FindControl("RBL_IsSpread") as RadioButtonList;
                        string IsSpread = rbl.SelectedValue.ToString();

                        strSQL = " Insert into P_Pack_Task (PackId, ProductName, TaskDrawingCode, TaskCode, Stage, Unit, MatingNum"
                                 +
                                 " , DefrayNum , ProductionNum,PlanFinishTime, LastChangeTime, ChangeTimes, IsDel, IsSpread)"
                                 + " Values ('" + PackID + "','" + ProductName + "','" + TaskDrawingCode + "','" +
                                 TaskCode + "','" + Stage + "','" + Unit + "','" + MatingNum
                                 + "','" + DefrayNum + "','" + ProductionNum + "','" + PlanFinishTime +
                                 "',Getdate(),'1','false', " + (IsSpread == "" ? "Null" : "'" + IsSpread + "'") + ")";

                        DBI.Execute(strSQL);
                    }
                }
                RadNotificationAlert.Text = "导入成功！";
                RadNotificationAlert.Show();

                //Response.Redirect("/Plan/ShowPlan.aspx");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                RadNotificationAlert.Show();
                return;
            }
        }

        protected void btnDown_Click(object sender, EventArgs e)
        {
            int i = 0;
            DirectoryInfo info =
                new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "Plan/计划导入模板");
            if (System.IO.Directory.Exists(info.ToString()))
            {
                foreach (FileInfo n in info.GetFiles())
                {
                    if (n.Name == "计划导入模板.xlsx")
                    {
                        i = 1;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(n.Name));
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                        Response.ContentType = "application/ms-excel";
                        this.EnableViewState = false;

                        Response.WriteFile(Server.MapPath(@"~\Plan\计划导入模板\") + n.Name);
                        Response.End();
                        return;
                    }
                }
            }
            if (i == 0)
            {
                RadNotificationAlert.Text = "没有找到模版";
                RadNotificationAlert.Show();
            }
        }

        protected void RBL_IsSpreadAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (sender) as RadioButtonList;
            string value = rbl.SelectedItem.Value;
            for (int i = 0; i < RadGridPack.Items.Count; i++)
            {
                RadioButtonList rbl1 = RadGridPack.Items[i].FindControl("RBL_IsSpread") as RadioButtonList;
                if (value == "")
                {
                    rbl1.SelectedIndex = -1;
                }
                else
                {
                    rbl1.SelectedValue = value;
                }
            }
        }
    }
}