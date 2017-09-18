using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms.AjaxCode
{
    /// <summary>
    /// GetMaterialStateSum 的摘要说明
    /// </summary>
    public class GetMaterialStateSum : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string jzwzcode = context.Request["jzwzcode"].ToString();
            Dal.MDemandDetailedListDraft_DSTableAdapters.Proc_Sel_Material_State_SumTableAdapter ta =
                new Dal.MDemandDetailedListDraft_DSTableAdapters.Proc_Sel_Material_State_SumTableAdapter();
            Dal.MDemandDetailedListDraft_DS.Proc_Sel_Material_State_SumDataTable dt = ta.Proc_Sel_Material_State_Sum(@jzwzcode);
            Dal.MDemandDetailedListDraft_DS ds = new Dal.MDemandDetailedListDraft_DS();
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