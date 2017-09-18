using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms.AjaxCode.index.Master
{
    /// <summary>
    /// Get_FirstItem 的摘要说明
    /// </summary>
    public class Get_FirstItem : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Dal.SystemManagementTableAdapters.Get_FirstItemTableAdapter ta = new Dal.SystemManagementTableAdapters.Get_FirstItemTableAdapter();
            Dal.SystemManagement.Get_FirstItemDataTable dt = ta.Get_FirstItem();
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