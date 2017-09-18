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
using System.Web.Security;

namespace mms.SystemMangement
{
    public partial class UserManagePWD1 : System.Web.UI.UserControl
    {
        private static string DBConn;
        private DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
        }

        public object DataItem { get; set; }

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        private void InitializeComponent()
        {
            this.DataBinding += new System.EventHandler(this.UserManagePWD_DataBinding);

        }

        protected void UserManagePWD_DataBinding(object sender, System.EventArgs e)
        {
            string id = DataBinder.Eval(DataItem, "ID").ToString();
            if (id == "")
            {
                RB_PWD1.Visible = false;
                RB_PWD2.Visible = false;
            }
            string strSQL = "";
            string dept = DataBinder.Eval(DataItem, "Dept").ToString();
            RCB_Dept.SelectedValue = dept;

            strSQL = " select a.*, UserId from Sys_RoleInfo as a left join Sys_UserInRole as b on a.ID = b.RoleId and UserId = '" + id + "' where a.Is_del='false'";
            DataTable dt = DBI.Execute(strSQL, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListItem li = new ListItem();
                li.Value = dt.Rows[i]["ID"].ToString();
                li.Text = dt.Rows[i]["RoleName"].ToString();
                if (dt.Rows[i]["UserID"].ToString() != "")
                {
                    li.Selected = true;
                }
                CBL_Role.Items.Add(li);
            }
        }
    }
}