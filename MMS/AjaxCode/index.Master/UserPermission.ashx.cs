using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms.AjaxCode.index.Master
{
    /// <summary>
    /// UserPermission 的摘要说明
    /// </summary>
    public class UserPermission : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string UserAccount = "Admin";//context.Request["UserAccount"].ToString();
            Dal.SystemManagementTableAdapters.Get_UserPermissionTableAdapter ta = new Dal.SystemManagementTableAdapters.Get_UserPermissionTableAdapter();
            Dal.SystemManagement.Get_UserPermissionDataTable dt = ta.Get_UserPermission(@UserAccount);
            Dal.SystemManagement ds = new Dal.SystemManagement();
            ds.Tables.Add(dt);
            string data = ds.GetXml();
            context.Response.ContentType = "text/plain";
            context.Response.Write(data);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}