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
using System.Drawing;

namespace mms.OutOfStorageManagement
{
    public partial class ReturnLogisticsCenter : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        string UserId;
        private static DataTable InitTable;
        private DataTable GridSource
        {
            get
            {
                if (InitTable != null)
                {
                    return InitTable;
                }
                else
                {
                    InitTable = new DataTable();
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("Material_Name");
                    InitTable.Columns.Add("ItemCode1");
                    InitTable.Columns.Add("Material_Type");
                    InitTable.Columns.Add("Material_Mark");
                    InitTable.Columns.Add("Material_State");
                    InitTable.Columns.Add("Material_Tech_Condition");
                    InitTable.Columns.Add("Rough_Spec");
                    InitTable.Columns.Add("Rough_Size");
                    InitTable.Columns.Add("Mat_Unit");
                    InitTable.Columns.Add("Mat_Rough_Weight");
                    InitTable.Columns.Add("Quantity");
                    InitTable.Columns.Add("BarCode");
                    return InitTable;
                }
            }
            set
            {
                InitTable = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            else
            {
                UserId = Session["UserId"].ToString();
            }
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "ReturnLogisticsCenter", this.Page);
                string userId = Session["UserId"].ToString();
                string strSQL = " select Dept from Sys_UserInfo_PWD where Id = '" + userId + "'";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("/Admin/NoRights.aspx");
                }
                else
                {
                    HF_DeptID.Value = dt.Rows[0]["Dept"].ToString();
                }
                GridSource.Clear();
                Session["RLCGridSource"] = Common.AddTableRowsID(GridSource);
                RTB_BarCode.Focus();
                RadGrid2.DataSource = GridSource;
                RadGrid2.DataBind();
                Session["RLCStrWhere"] = null;
            }
        }

        protected void RTB_BarCode_TextChanged(object sender, EventArgs e)
        {
            string barcode = RTB_BarCode.Text.Trim();
            if (barcode == "")
            {
                RTB_BarCode.Focus();
                return;
            }

            DataTable dt = GetOutOfStorage(" and BarCode= '" + barcode + "'");
            if (dt.Rows.Count == 0)
            {
                RTB_BarCode.Text = "没有找到相关的物资信息";
                RTB_BarCode.ForeColor = Color.Red;
                return;
            }
            DataTable dtgridSource = Session["RLCGridSource"] as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dtgridSource.Select("ID='" + dt.Rows[i]["ID"].ToString() + "'").Length == 0)
                {
                    dtgridSource.ImportRow(dt.Rows[i]);
                }
            }
            Session["RLCGridSource"] = Common.AddTableRowsID(dtgridSource);
            RadGrid1.Rebind();
            RTB_BarCode.Text = "";
            RTB_BarCode.Focus();
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = Session["RLCGridSource"] as DataTable;
        }

        protected void RadGrid1_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string RowsId = (e.Item as GridDataItem).GetDataKeyValue("RowsId").ToString();

                DataRow dr = (Session["RLCGridSource"] as DataTable).Select("RowsId='" + RowsId + "'")[0];
                (Session["RLCGridSource"] as DataTable).Rows.Remove(dr);
                RadGrid1.Rebind();

                RadNotificationAlert.Text = "删除成功！";
                RadNotificationAlert.Show();
            }
            if (e.CommandName == "add")
            {
                for (int i = 0; i < RadGrid1.Items.Count; i++)
                {
                    GridDataItem item = RadGrid1.Items[i] as GridDataItem;
                    string RowsId = item["RowsId"].Text;
                    RadTextBox RTB_Quantity = item.FindControl("RTB_Quantity") as RadTextBox;

                    try
                    {
                        Convert.ToDouble(RTB_Quantity.Text.Trim());
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！第" + RowsId + "行：退库数量：请输入数字！";
                        RadNotificationAlert.Show();
                        return;
                    }

                    if (Convert.ToDouble(RTB_Quantity.Text.Trim()) > Convert.ToDouble(item["Quantity"].Text))
                    {
                        RadNotificationAlert.Text = "失败！第" + RowsId + "行：退库数量：不可以大于库存数量！";
                        RadNotificationAlert.Show();
                        return;
                    }
                }

                DBI.OpenConnection();
                try
                {
                    DBI.BeginTrans();
                    string result = "";
                    for (int i = 0; i < RadGrid1.Items.Count; i++)
                    {
                        GridDataItem item = RadGrid1.Items[i] as GridDataItem;
                        string id = item.GetDataKeyValue("ID").ToString();
                        RadTextBox RTB_Quantity = item.FindControl("RTB_Quantity") as RadTextBox;
                        string Quantity = RTB_Quantity.Text;
                        string RowsId = item["RowsId"].Text.Trim();

                        string quantity1 = UpdateOutOfStorageInsertOutOfStorage(id, Quantity);
                        if (quantity1 == "0")
                        {
                            result += result == "" ? RowsId : "、" + RowsId;
                        }

                    }
                    DBI.CommitTrans();

                    if (result == "") { result = "退库成功！"; }
                    else { result = "退库成功！第" + result + "行，库存不足，退库失败"; }
                    RadNotificationAlert.Text = result;
                    RadNotificationAlert.Show();

                    GridSource.Clear();
                    Session["RLCGridSource"] = Common.AddTableRowsID(GridSource);
                    RadGrid1.Rebind();
                }
                catch (Exception ex)
                {
                    DBI.RollbackTrans();
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
                finally
                {
                    DBI.CloseConnection();
                }
            }
        }

        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string Material_Name = RTB_Material_Name.Text.Trim();
            string ItemCode1 = RTB_ItemCode1.Text.Trim();
            string Material_Mark = RTB_Material_Mark.Text.Trim();
            string Rough_Spec = RTB_Rough_Spec.Text.Trim();
            string Rough_Size = RTB_Rough_Size.Text.Trim();

            Session["RLCStrWhere"] = " and Material_Name like '%" + Material_Name + "%' and ItemCode1 like '%" + ItemCode1 + "%'"
                + " and Material_Mark like '%" + Material_Mark + "%' and Rough_Spec like '%" + Rough_Spec + "%' and Rough_Size like '%" + Rough_Size + "%'".ToString();

            DataTable dt = GetOutOfStorage(Session["RLCStrWhere"].ToString());

            RadGrid2.DataSource = dt;
            RadGrid2.DataBind();
        }

        protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "add")
            {
                for (int i = 0; i < RadGrid2.SelectedItems.Count; i++)
                {
                    GridDataItem item = RadGrid2.SelectedItems[i] as GridDataItem;
                    string RowsId = item["RowsId"].Text.Trim();
                    RadTextBox RTB_Quantity2 = item.FindControl("RTB_Quantity2") as RadTextBox;
                    try
                    {
                        Convert.ToDouble(RTB_Quantity2.Text.Trim());
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "失败！第" + RowsId + "行，退库数量：请输入数字";
                        RadNotificationAlert.Show();
                        return;
                    }
                    if (Convert.ToDouble(RTB_Quantity2.Text.Trim()) > Convert.ToDouble(item["Quantity"].Text))
                    {
                        RadNotificationAlert.Text = "失败！第" + RowsId + "行，退库数量：不可以大于库存数量";
                        RadNotificationAlert.Show();
                        return;
                    }
                }

                DBI.OpenConnection();
                try
                {
                    DBI.BeginTrans();
                    string result = "";
                    for (int i = 0; i < RadGrid2.SelectedItems.Count; i++)
                    {
                        GridDataItem item = RadGrid2.SelectedItems[i] as GridDataItem;
                        string id = item.GetDataKeyValue("ID").ToString();
                        RadTextBox RTB_Quantity2 = item.FindControl("RTB_Quantity2") as RadTextBox;
                        string Quantity = RTB_Quantity2.Text.Trim();
                        string RowsId = item["RowsId"].Text.ToString();

                        string quantity1 = UpdateOutOfStorageInsertOutOfStorage(id, Quantity);
                        if (quantity1 == "0")
                        {
                            result += result == "" ? RowsId : "、" + RowsId;
                        }
                        
                    }
                    DBI.CommitTrans();

                    if (result == "") { result = "退库成功！"; }
                    else { result = "退库成功！第" + result + "行，库存不足，退库失败"; }
                    RadNotificationAlert.Text = result;
                    RadNotificationAlert.Show();

                    DataTable dt = GetOutOfStorage(Session["RLCStrWhere"].ToString());

                    RadGrid2.DataSource = dt;
                    RadGrid2.DataBind();
                }
                catch (Exception ex)
                {
                    DBI.RollbackTrans();
                    RadNotificationAlert.Text = "失败！" + ex.Message.ToString();
                    RadNotificationAlert.Show();
                }
                finally
                {
                    DBI.CloseConnection();
                }
            }
        }

        public DataTable GetOutOfStorage(string strWhere)
        {
            string strSQL = " select ID, DeptID, Type, Reason, (Quantity - isnull(Consume,0)) as Quantity, Material_Name, ItemCode1, ItemCode2, MaterialDes, Material_Type, Material_Mark, Material_State"
                 + " , Material_Tech_Condition, Rough_Spec, Rough_Size, Mat_Unit, Mat_Rough_Weight, BarCode, SingleSerialNumber, MAID, OpeUserID, OpeTime "
                 + " from OutOfStorage where DeptID = '" + HF_DeptID.Value + "' and Type = '1' and  Quantity > isnull(Consume,0) " + strWhere;
            DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
            return dt;
        }

        public string UpdateOutOfStorageInsertOutOfStorage(string id, string Quantity)
        {
            string strSQL = " declare @quantity decimal(18,2) select @quantity = (Quantity - convert(decimal(18,2),isnull(Consume,0))) from OutOfStorage where ID = '" + id + "'";
            strSQL += " if (convert(decimal(18,2),'" + Quantity +"') <= @quantity ) begin select @quantity = Convert(decimal(18,2),'" + Quantity +"')";
            strSQL += " Update OutOfStorage set Consume = (Convert(decimal(18,2),isnull(Consume,0)) + @quantity) where ID = '" + id + "'";
            strSQL += " Insert into OutOfStorage (DeptID,[Type], Reason, Quantity, Material_Name, ItemCode1, ItemCode2, MaterialDes, Material_Type, Material_Mark, Material_State"
                + " , Material_Tech_Condition, Rough_Spec, Rough_Size, Mat_Unit, Mat_Rough_Weight, BarCode, OpeUserID, OpeTime,RelID)"
                + " select DeptID, '2', '退库',  @quantity,Material_Name, ItemCode1, ItemCode2, MaterialDes, Material_Type, Material_Mark, Material_State"
                + " , Material_Tech_Condition, Rough_Spec, Rough_Size, Mat_Unit, Mat_Rough_Weight, BarCode,  '" + UserId + "', GetDate(), ID from OutOfStorage where Id = '" + id + "'";
            strSQL += " select '1' end else  begin select '0' end ";
            return  DBI.GetSingleValue(strSQL);
        }
    }
}