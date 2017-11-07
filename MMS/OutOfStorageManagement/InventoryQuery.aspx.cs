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
    public partial class InventoryQuery : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
    
            if (!IsPostBack)
            {
                string userId = Session["UserId"].ToString();
                Common.CheckPermission(Session["UserName"].ToString(), "InventoryQuery", this.Page);
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

                dt = new DataTable();
                Session["IQGridSource2"] = dt;
                Session["IQGridSource1"] = OutOfStorage("", "", "", "");
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = Session["IQGridSource1"] as DataTable;
        }

        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid2.DataSource = Session["IQGridSource2"] as DataTable;
        }

        protected void RB_Query2_Click(object sender, EventArgs e)
        {
            string startTime = RDP_Start1.SelectedDate.ToString();
            string endTime = RDP_End1.SelectedDate.ToString();
            string Material_Name = RTB_Material_Name1.Text.ToString();
            string ItemCode1 = RTB_ItemCode11.Text.ToString();
            string Type = RDDL_BD.SelectedValue;
            string strWhere = " and Type like '%" + Type + "%'";
            if (startTime != "")
            {
                try
                {
                    Convert.ToDateTime(startTime);
                    strWhere += " and OpeTime >= '" + startTime + "'";
                }
                catch { }
            }
            if (endTime != "")
            {
                try
                {
                    Convert.ToDateTime(endTime);
                    strWhere += " and OpeTime < '" + Convert.ToDateTime(endTime).AddDays(1).ToString("yyyy-MM-dd") + "'";
                }
                catch { }
            }
            if (Material_Name != "")
            {
                strWhere += " and Material_Name like '%" + Material_Name + "%'";
            }
            if (ItemCode1 != "")
            {
                strWhere += " and ItemCode1 like '%" + ItemCode1 + "%'";
            }

            Session["IQGridSource2"] = GetOutOfStorage(strWhere);
            RadGrid2.Rebind();
        }

        public DataTable GetOutOfStorage(string StrWhere)
        {

            DataTable dt = new DataTable();

            string strSQL = " select *, Case when Type = '1' then '入库' else '出库' end as Type1"
                + " , (select UserName from Sys_UserInfo_PWD where Sys_UserInfo_PWD.ID = OpeUserID) as UserName"
                + " from OutOfStorage where DeptID = '" + HF_DeptID.Value + "'";
            strSQL += StrWhere;
            strSQL += " order by ID";
            try
            {
                dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "获取出入库信息错误！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }

            return dt;
        }

        public DataTable OutOfStorage(string Start, string End, string Material_Name, string ItemCode1)
        {
            DataTable dt = new DataTable();

            string strSQL = " create table #test (Material_Name nvarchar(50), ItemCode1 nvarchar(50), InitialNumber decimal(18,2), StorageQuantity decimal(18,2), OutgoingQuantity decimal(18,2), FinalNumber decimal(18,2))";
            if (Start != "")
            {
                strSQL += " insert into #test (Material_Name, ItemCode1,InitialNumber)";
                strSQL += " select Material_Name, ItemCode1, sum(case when Type = '1' then Quantity else -Quantity end)";
                strSQL += " from OutOfStorage where DeptID = '" + HF_DeptID.Value + "' and Material_Name like '%" + Material_Name + "%' and ItemCode1 like '%" + ItemCode1 + "%' and OpeTime < '" + Start + "'";
                strSQL += " group by Material_Name, ItemCode1 having sum(case when Type = '1' then Quantity else -Quantity end) > 0";
            }

            strSQL += " insert into #test (Material_Name, ItemCode1,StorageQuantity)";
            strSQL += " select Material_Name, ItemCode1, sum(Quantity)";
            strSQL += " from OutOfStorage where DeptID = '" + HF_DeptID.Value + "' and Material_Name like '%" + Material_Name + "%' and ItemCode1 like '%" + ItemCode1 + "%'  and Type = '1'";
            if (Start != "")
            {
                strSQL += " and OpeTime >= '" + Start + "'";
            }
            if (End != "")
            {
                strSQL += " and OpeTime < '" + Convert.ToDateTime(End).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            strSQL += " group by Material_Name, ItemCode1 having sum(Quantity) > 0 ";

            strSQL += " insert into #test (Material_Name, ItemCode1,OutgoingQuantity)";
            strSQL += " select Material_Name, ItemCode1, sum(Quantity)";
            strSQL += " from OutOfStorage where DeptID = '" + HF_DeptID.Value + "' and Material_Name like '%" + Material_Name + "%' and ItemCode1 like '%" + ItemCode1 + "%' and Type='2'";
            if (Start != "")
            {
                strSQL += " and OpeTime >= '" + Start + "'";
            }
            if (End != "")
            {
                strSQL += " and OpeTime < '" + Convert.ToDateTime(End).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            strSQL += " group by Material_Name, ItemCode1 having sum(Quantity) > 0 ";

            strSQL += " insert into #test (Material_Name, ItemCode1,FinalNumber)";
            strSQL += " select Material_Name, ItemCode1, isnull(sum(InitialNumber),0) + isnull(sum(StorageQuantity),0) - isnull(sum(OutgoingQuantity),0) from #test group by Material_Name, ItemCode1";

            strSQL += " select Material_Name, ItemCode1 , isnull(sum(InitialNumber),0) as InitialNumber , isnull(sum(StorageQuantity),0) as StorageQuantity ";
            strSQL += " , isnull(sum(OutgoingQuantity),0) as OutgoingQuantity ,isnull(sum(FinalNumber),0) as FinalNumber from #test group by Material_Name, ItemCode1 order by Material_Name, ItemCode1";

            try
            {
                dt = DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                RadNotificationAlert.Text = "获取库存信息错误！" + ex.Message.ToString();
                RadNotificationAlert.Show();
            }

            return dt;
        }

        protected void RB_Query1_Click(object sender, EventArgs e)
        {
            string Material_Name = RTB_Material_Name1.Text.Trim();
            string ItemCode1 = RTB_ItemCode11.Text.Trim();
            string Start = RDP_Start1.SelectedDate.ToString();
            string end = RDP_End1.SelectedDate.ToString();
            try { Convert.ToDateTime(Start); }
            catch { Start = ""; }
            try { Convert.ToDateTime(end); }
            catch { end = ""; }
           
            Session["IQGridSource1"] = OutOfStorage(Start, end, Material_Name, ItemCode1);
            RadGrid1.Rebind();
        }
		
		 protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "库存信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "库存信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "库存信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }

        protected void RadButton_ExportExcel_Click2(object sender, EventArgs e)
        {
            RadGrid2.ExportSettings.FileName = "出入库流水信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid2.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click2(object sender, EventArgs e)
        {
            RadGrid2.ExportSettings.FileName = " 出入库流水信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid2.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click2(object sender, EventArgs e)
        {
            RadGrid2.ExportSettings.FileName = "出入库流水信息列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid2.ExportSettings.IgnorePaging = true;
            RadGrid2.MasterTableView.ExportToPdf();
            RadGrid2.ExportSettings.IgnorePaging = false;
        }
    }
}