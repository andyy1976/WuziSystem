using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Camc.Web.Library;
using System.Configuration;

namespace mms.MaterialApplicationCollar
{
    public partial class MaterialApplicationApprove : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ViewState["GridSource"] = new DataTable();
            if (!IsPostBack)
            {
             

                try
                {
                    string DBConn;
                    DBInterface DBI;
                    DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
                    DBI = DBFactory.GetDBInterface(DBConn);

                    string userDomainAccount = DBI.GetSingleValue("select DomainAccount from Sys_UserInfo_PWD where ID = '" +  Session["UserId"].ToString() + "'");
                    string maId = Request.QueryString["MAID"].ToString();

                    K2BLL k2Bll = new K2BLL();
                    k2.ApproveInfoBody[] body = k2Bll.GetApproveBody(maId, userDomainAccount);
                    this.ViewState["GridSource"] = body;
                }
                catch
                {
                    RadNotificationAlert.Text = "暂时不能与流程平台通讯，无法获取审批最新状态";
                    RadNotificationAlert.Show();
                }
            }
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.ViewState["GridSource"];
        }
    }
}