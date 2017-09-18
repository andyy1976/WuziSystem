using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mms.BLL;

namespace mms.SystemMangement
{
    public partial class SyncUserFromAD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 点击同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSynch_Click(object sender, EventArgs e)
        {
            string serverName = "TJ";
            string port = "389";
            string userName = "tj_yaoy";
            string password = txtPwd.Text;
            UserService userService = new UserService();
            bool isSuccess = userService.SynchUser(serverName, port, userName, password);
            var str= isSuccess ? ("同步完成") : ("同步失败，请检查输入的信息是否正确");
        }
    }
}