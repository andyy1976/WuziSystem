using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Web.Security;
using System.Configuration;
using System.Linq;
using Camc.Web.Library;
using Telerik.Web.Data.Extensions;

namespace mms
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["R"] == null)
                    //DomainLongin();
            }
        }

        protected void LoginBut_Click(object sender, EventArgs e)
        {
            int? result = 0;
            int? UserId = 0;
            string UName = UserName.Text.ToString();

            if (UName == "")
            {
                RadNotificationAlert.Text = "请输入用户名！";
                RadNotificationAlert.Show();
                return;
            }

            string Pwd = PassWord.Text.ToString();
            string MD5PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(Pwd, "md5");

            Dal.AccountTableAdapters.QueriesTableAdapter ta = new Dal.AccountTableAdapters.QueriesTableAdapter();
            Convert.ToInt32(ta.Get_UerInfo(@UName, @MD5PassWord, ref result, ref UserId));
            if (result == -1)
            {
                Session["UserName"] = UName;
                Session["UserId"] = UserId;
                //Session.Timeout = 20;
                Response.Redirect("~/Admin/Welcome.aspx");
            }
            else if (result == -2)
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('登录失败，请联系管理员');");
                RadNotificationAlert.Text = "登录失败，请联系管理员";
                RadNotificationAlert.Show();
            }
            else
            {
                Response.Write("服务器错误");
            }
        }

        protected void AdLoginBut_Click(object sender, EventArgs e)
        {
            DomainLongin();
        }

        protected void DomainLongin()
        {
            string DomainAccount = User.Identity.Name.ToString();
            if (DomainAccount == "")
            {
                RadNotificationAlert.Text = "无法获取该电脑域帐号";
                RadNotificationAlert.Show();
                return;
            }
            if (DomainAccount.ToUpper().IndexOf("TJ\\") != -1)
            {
                DomainAccount = DomainAccount.Substring(3, DomainAccount.Length - 3);
            }
            var db=new MMSDbDataContext();
            var query =
                db.Sys_UserInfo_PWD.SingleOrDefault(p => p.IsDel != true && p.DomainAccount == DomainAccount);
            if (query!=null)
            {
                Session["UserName"] = query.UserAccount;
                Session["UserId"] = query.ID;
                //Session.Timeout = 20;
                Response.Redirect("~/Admin/Welcome.aspx");
            }
            else
            {
                RadNotificationAlert.Text = "本系统没有该域帐号！";
                RadNotificationAlert.Show();
            }
        }
    }
}