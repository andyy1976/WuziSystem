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
    public partial class DeptManage1 : System.Web.UI.UserControl
    {
        string DBConn;
        DBInterface DBI;
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
            this.DataBinding += new System.EventHandler(this.DeptManagePWD_DataBinding);

        }

        protected void DeptManagePWD_DataBinding(object sender, System.EventArgs e)
        {
            string id = DataBinder.Eval(DataItem, "ID").ToString();

            string Cust_Account_ID = DataBinder.Eval(DataItem, "Cust_Account_ID").ToString();
            RDDL_Cust_Account_ID.SelectedValue = Cust_Account_ID;
            
            string strSQL = "";

            strSQL = " select KeyWordCode, KeyWord, Dept_Id from Sys_Dict " +
                " left join Sys_Dept_ShipAddr on Convert(nvarchar(50),Sys_Dict.TypeID) + '-' + Convert(nvarchar(50),Sys_Dict.KeyWordCode) = Sys_Dept_ShipAddr.Shipping_Addr_Id and Sys_Dept_ShipAddr.Dept_Id= '" + id + "'" +
                " where TypeID='2' and Sys_Dict.Is_Del = 'false'";
            DataTable dt = DBI.Execute(strSQL, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListItem li = new ListItem();
                li.Value = dt.Rows[i]["KeyWordCode"].ToString();
                li.Text = dt.Rows[i]["KeyWord"].ToString();
                if (dt.Rows[i]["Dept_Id"].ToString() != "")
                {
                    li.Selected = true;
                }
                CBL_Shipping_Address.Items.Add(li);
            }
        }

    }
}