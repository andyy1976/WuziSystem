using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mms
{
    public partial class index : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Convert.ToString(Session["UserName"]) == null || Convert.ToString(Session["UserName"]) == "")
                {
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    //LbUserName.InnerText = Session["UserName"].ToString();
                    using (var db = new MMSDbDataContext())
                    {
                        var query = (from p in db.Sys_UserInfo_PWD
                            where p.UserAccount == Session["UserName"].ToString()
                            select p).SingleOrDefault();
                        LbUserName.InnerText = query.UserName;
                    }
                }
            }
        }

        protected void ExitBt_Click(object sender, ImageClickEventArgs e)
        {
            Session["UserName"] = null;
            Response.Redirect("~/Default.aspx?R=1");
        }
    }
}