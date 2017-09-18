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
    public partial class SetUserToRole : System.Web.UI.Page
    {
        private static string DBConn;
        private DBInterface DBI;
        private string userAccount;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();//
            DBI = DBFactory.GetDBInterface(DBConn);
            userAccount = Session["UserName"].ToString();
            Common.CheckPermission(userAccount, "Allow_Visit_UserRole_Page", this);
            if (!Common.IsHasRight(userAccount, "Allow_Edit_UserRole_Page"))
            {
                rbtn_save.Visible = false;
            }
            else
            {
                rbtn_save.Visible = true;
            }
            Action.Visible = false;
            if (!IsPostBack)
            {
                
            }
        }


        protected void rbtn_save_Click(object sender, EventArgs e)
        {
            string PID;
            string sqlstr;
            try
            {
                sqlstr = "select ID from [dbo].[Sys_UserInRole] where UserID='"
                    + rcob_user.SelectedItem.Value + "'";
                PID = DBI.GetSingleValue(sqlstr);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            if (PID == null)
            {
                try
                {
                    sqlstr = "insert into [dbo].[Sys_UserInRole] (UserID,RoleID) values ('" + rcob_user.SelectedValue + "','" + rcob_role.SelectedValue + "')";
                    DBI.Execute(sqlstr);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            else
            {
                try
                {
                    sqlstr = "update [dbo].[Sys_UserInRole] set UserID='"
                        + rcob_user.SelectedValue + "',RoleID='"
                        + rcob_role.SelectedValue + "' where ID='" + PID + "'";
                    DBI.Execute(sqlstr);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
                finally
                {
                    RadNotificationAlert.Text = "保存成功！";
                    RadNotificationAlert.Show();
                }
            }
        }

        protected void rcob_user_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Action.Visible = true;
            string RID;
            try
            {
                string sqlstr = "select RoleID from [dbo].[Sys_UserInRole] where UserID='" + rcob_user.SelectedValue + "'";
                RID = DBI.GetSingleValue(sqlstr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            if (RID != null)
            {
                
                rcob_role.SelectedValue = RID.ToString();
            }
        }
    }
}