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

    public partial class DeptSetAddress : System.Web.UI.Page
    {
        private static string DBConn;
        private DBInterface DBI;
        private string userAccount;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            userAccount = Session["UserName"].ToString();
            Address.Visible = false;
        }

        protected void RadComboBoxDept_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int DeptId = Convert.ToInt16(RadComboBoxDept.SelectedValue);
            string Addr_Id;
            try
            {
                string sqlstr = "select Shipping_Addr_Id from [dbo].[Sys_Dept_ShipAddr] where Dept_Id='" + DeptId + "'";
                Addr_Id = DBI.GetSingleValue(sqlstr);
            }
            catch (Exception ex)
            {
                throw new Exception("读取信息出错" + ex.Message.ToString());
            }
            Address.Visible = true;
            if (Addr_Id != null)
            {
                RadComboBoxDict.SelectedValue = Addr_Id;
            }
            else
            {
                RadComboBoxDict.SelectedIndex = 0;
            }
        }

        protected void RadButtonSave_Click(object sender, EventArgs e)
        {
            int DeptCode = Convert.ToInt16(RadComboBoxDept.SelectedValue);
            string Address = RadComboBoxDict.SelectedValue;
            try
            {
                string strSQL;
                strSQL = "insert into [dbo].[Sys_Dept_ShipAddr] (Dept_Id, Shipping_Addr_Id, Is_Del) values ('" + DeptCode + "', '" + Address + "', 'false')";
                DBI.Execute(strSQL);
                RadNotificationAlert.Text = "关联成功！";
                RadNotificationAlert.Show();
            }
            catch (Exception ex)
            {
                throw new Exception("保存信息出错" + ex.Message.ToString());
            }
        }
    }
}