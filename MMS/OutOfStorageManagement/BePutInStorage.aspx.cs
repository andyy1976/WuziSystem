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
    public partial class BePutInStorage : System.Web.UI.Page
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
                    InitTable.Columns.Add("RowsId");
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
            else {
                UserId = Session["UserId"].ToString();
            }
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "BePutInStorage", this.Page);
                Session["StrWhereModel"] = null;
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
                Session["BPISGridSource"] = GridSource;
                RTB_BarCode.Focus();
            }
        }

        protected void RTB_BarCode_TextChanged(object sender, EventArgs e)
        {
            //RTB_BarCode.Text = "该功能暂不能使用";
            //RTB_BarCode.ForeColor = Color.Red;
            //return;
            string BarCode = RTB_BarCode.Text;
            string billno = BarCode.Split('-')[0].ToString();

            string strSQL = " select * from ReleaseStockBill_T_Item where billno ='" + billno + "' ";
            DataTable dt = DBI.Execute(strSQL, true);
            if (dt.Rows.Count == 0)
            {
                RTB_BarCode.Text = "没有找到相关物资信息！";
                RTB_BarCode.ForeColor = Color.Red;
                RTB_BarCode.Focus();
                return;
            }
            DataTable dtgridSource = Session["BPISGridSource"] as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow newrow = dtgridSource.NewRow();
                newrow["RowsId"] = (dtgridSource.Rows.Count + 1).ToString();
                newrow["Material_Name"] = dt.Rows[i]["invname"].ToString();
                newrow["ItemCode1"] = dt.Rows[i]["invcode"].ToString();
                //newrow["Material_Type"]= dt.Rows[i]["invname"].ToString();
                newrow["Material_Mark"] = dt.Rows[i]["invxhcpdh"].ToString();
                newrow["Material_State"] = dt.Rows[i]["invstatus"].ToString();
                newrow["Material_Tech_Condition"] = dt.Rows[i]["techconditions"].ToString();
                newrow["Rough_Spec"] = dt.Rows[i]["invscale"].ToString();
                newrow["Rough_Size"] = dt.Rows[i]["jc_cc"].ToString();
                newrow["Mat_Unit"] = dt.Rows[i]["invmeasname"].ToString();
                //newrow["Mat_Rough_Weight"]= dt.Rows[i]["invname"].ToString();
                newrow["Quantity"] = dt.Rows[i]["realnum"].ToString();
                newrow["BarCode"] = BarCode;

                dtgridSource.Rows.Add(newrow);
            }
            Session["BPISGridSource"] = dtgridSource;
            RadGrid1.Rebind();
            RTB_BarCode.Text = "";
            RTB_BarCode.Focus();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = (Session["BPISGridSource"] as DataTable);
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "add")
            {
                if (RadGrid1.Items.Count > 0)
                {
                    for (int i = 0; i < RadGrid1.Items.Count; i++)
                    {
                        RadTextBox RTB_Quantity = RadGrid1.Items[i].FindControl("RTB_Quantity") as RadTextBox;
                        try
                        {
                            Convert.ToDecimal(RTB_Quantity.Text.Trim());
                        }
                        catch {
                            RadNotificationAlert.Text = "失败！第" + (i + 1).ToString() + "行，入库数量：请输入数字";
                            RadNotificationAlert.Show();
                            return;
                        }
                    }
                    DBI.OpenConnection();
                    try
                    {
                        DBI.BeginTrans();

                        for (int i = 0; i < RadGrid1.Items.Count; i++)
                        {
                            GridDataItem item = RadGrid1.Items[i] as GridDataItem;
                            RadTextBox RTB_Quantity = RadGrid1.Items[i].FindControl("RTB_Quantity") as RadTextBox;

                            string Type = "1";
                            string Reason = "入库";
                            string Quantity = RTB_Quantity.Text;

                            string Material_Name = item["Material_Name"].Text == "&nbsp;" ? "" : item["Material_Name"].Text;
                            string ItemCode1 = item["ItemCode1"].Text == "&nbsp;" ? "" : item["ItemCode1"].Text;
                            string Material_Type = item["Material_Type"].Text == "&nbsp;" ? "" : item["Material_Type"].Text;
                            string Material_Mark = item["Material_Mark"].Text == "&nbsp;" ? "" : item["Material_Mark"].Text;
                            string Material_State = item["Material_State"].Text == "&nbsp;" ? "" : item["Material_State"].Text;
                            string Material_Tech_Condition = item["Material_Tech_Condition"].Text == "&nbsp;" ? "" : item["Material_Tech_Condition"].Text;
                            string Rough_Spec = item["Rough_Spec"].Text == "&nbsp;" ? "" : item["Rough_Spec"].Text;
                            string Rough_Size = item["Rough_Size"].Text == "&nbsp;" ? "" : item["Rough_Size"].Text;
                            string Mat_Unit = item["Mat_Unit"].Text == "&nbsp;" ? "" : item["Mat_Unit"].Text;
                            string Mat_Rough_Weight = item["Mat_Rough_Weight"].Text == "&nbsp;" ? "" : item["Mat_Rough_Weight"].Text;
                            string BarCode = item["BarCode"].Text == "&nbsp;" ? "" : item["BarCode"].Text;

                            string strSQL = " Insert into OutOfStorage (DeptID,[Type], Reason, Quantity, Material_Name, ItemCode1, ItemCode2, MaterialDes, Material_Type, Material_Mark, Material_State"
                                + " , Material_Tech_Condition, Rough_Spec, Rough_Size, Mat_Unit, Mat_Rough_Weight, BarCode, SingleSerialNumber, MAID, OpeUserID, OpeTime)"
                                + " values ('" + HF_DeptID.Value + "','" + Type + "','" + Reason + "','" + Quantity + "','" + Material_Name + "','" + ItemCode1 + "','','','" + Material_Type + "','" + Material_Mark + "','" + Material_State + "'"
                                + " ,'" + Material_Tech_Condition + "','" + Rough_Spec + "','" + Rough_Size + "','" + Mat_Unit + "','" + Mat_Rough_Weight + "','" + BarCode + "','','','" + UserId + "',getDate())";
                            DBI.Execute(strSQL);
                        }

                        DBI.CommitTrans();

                        RadNotificationAlert.Text = "入库成功！";
                        RadNotificationAlert.Show();

                        GridSource.Clear();
                        Session["BPISGridSource"] = GridSource;
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
                else
                {
                    RadNotificationAlert.Text = "失败！没有入库物资信息！";
                    RadNotificationAlert.Show();
                }
               
            }
            else if (e.CommandName == "del")
            {
                string RowsId = (e.Item as GridDataItem).GetDataKeyValue("RowsId").ToString();

                DataRow dr = (Session["BPISGridSource"] as DataTable).Select("RowsId='" + RowsId + "'")[0];
                (Session["BPISGridSource"] as DataTable).Rows.Remove(dr);
                RadGrid1.Rebind();

                RadNotificationAlert.Text = "删除成功！";
                RadNotificationAlert.Show();
            }
        }

        protected void RB_Add1_Click(object sender, EventArgs e)
        {
            string Type = "1";
            string Reason = "入库";
            string Quantity = RTB_Quantity1.Text.Trim();
            try { Convert.ToDouble(Quantity); }
            catch
            {
                RadNotificationAlert.Text = "失败！入库数量：请输入数字！";
                RadNotificationAlert.Show();
                return;
            }

            string Material_Name = RTB_Material_Name.Text.Trim();
            string ItemCode1 =RTB_ItemCode1.Text.Trim();
            string Material_Type = RTB_Material_Type.Text.Trim();
            string Material_Mark = RTB_Material_Mark.Text.Trim();
            string Material_State = RTB_Material_State.Text.Trim();
            string Material_Tech_Condition = RTB_Material_Tech_Condition.Text.Trim();
            string Rough_Spec = RTB_Rough_Spec.Text.Trim();
            string Rough_Size =RTB_Rough_Size.Text.Trim();
            string Mat_Unit = RTB_Mat_Unit.Text.Trim();
            string Mat_Rough_Weight = RTB_Mat_Rough_Weight.Text.Trim();
            string BarCode = RTB_BarCode1.Text.Trim();

            string strSQL = " Insert into OutOfStorage (DeptID,[Type], Reason, Quantity, Material_Name, ItemCode1, ItemCode2, MaterialDes, Material_Type, Material_Mark, Material_State"
                + " , Material_Tech_Condition, Rough_Spec, Rough_Size, Mat_Unit, Mat_Rough_Weight, BarCode, SingleSerialNumber, MAID, OpeUserID, OpeTime)"
                + " values ('" + HF_DeptID.Value + "','" + Type + "','" + Reason + "','" + Quantity + "','" + Material_Name + "','" + ItemCode1 + "','','','" + Material_Type + "','" + Material_Mark + "','" + Material_State + "'"
                + " ,'" + Material_Tech_Condition + "','" + Rough_Spec + "','" + Rough_Size + "','" + Mat_Unit + "','" + Mat_Rough_Weight + "','" + BarCode + "','','','" + UserId + "',getDate())";
            DBI.Execute(strSQL);
            RadNotificationAlert.Text = "保存成功！";
            RadNotificationAlert.Show();
        }

        protected void RTB_ItemCode1_TextChanged(object sender, EventArgs e)
        {
            string ItemCode1 = RTB_ItemCode1.Text.Trim();
            

            string strSQL = " select * from GetCommItem_T_Item where seg3 = '" + ItemCode1 + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            if (dt.Rows.Count > 0)
            {
                string Seg5 = dt.Rows[0]["Seg5"].ToString().Substring(0, 4);
                RTB_Material_Type.Text = dt.Rows[0]["Seg6"].ToString().Substring(0, dt.Rows[0]["Seg6"].ToString().IndexOf("."));
                RTB_Material_State.Text = "";
                RTB_Material_Tech_Condition.Text = "";
                RTB_Mat_Rough_Weight.Text = "";

                switch (dt.Rows[0]["Seg5"].ToString().Substring(0, 4))
                {
                    case "YY01":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg21"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg13"].ToString();
                        RTB_Material_Mark.Text = "";
                        break;
                    case "YY02":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg15"].ToString();
                        RTB_Material_Mark.Text = "";
                        break;
                    case "YY03":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = dt.Rows[0]["Seg16"].ToString();
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg15"].ToString();
                        RTB_Material_Mark.Text = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY04":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Material_Mark.Text = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY05":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Material_Mark.Text = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY06":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Material_Mark.Text = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY07":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg20"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Material_Mark.Text = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY08":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Material_Mark.Text = dt.Rows[0]["Seg13"].ToString();
                        break;
                    case "YY09":
                        RTB_Material_Name.Text = dt.Rows[0]["Seg12"].ToString();
                        RTB_Mat_Unit.Text = dt.Rows[0]["Seg7"].ToString();
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = dt.Rows[0]["Seg14"].ToString();
                        RTB_Material_Mark.Text = dt.Rows[0]["Seg13"].ToString();
                        break;
                    default:
                        RTB_Material_Name.Text = "";
                        RTB_Mat_Unit.Text = "";
                        RTB_Rough_Size.Text = "";
                        RTB_Rough_Spec.Text = "";
                        RTB_Material_Mark.Text = "";
                        break;
                }
            }
        }
    }
}