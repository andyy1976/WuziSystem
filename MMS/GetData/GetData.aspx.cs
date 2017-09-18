using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace mms.GetData
{
    public partial class GetData : System.Web.UI.Page
    {
        SOAHeaders SOAHeader = new SOAHeaders();
        protected void Page_Load(object sender, EventArgs e)
        {
            GetSOAHeader();
        }

        protected void Btn_ALL_Click(object sender, EventArgs e)
        {

        }

        protected void btn_GetExeInf_OnClick(object sender, EventArgs e)
        { 
            
        }

        protected void Btn_GetRqStatus_OnClick(object sender, EventArgs e)
        { 
            
        }

        protected void Btn_GetRcoStatus_OnClick(object sender, EventArgs e)
        { 
            
        }

        #region 读取XML文件获取SOAHeader和UserName、Password

        public SOAHeaders GetSOAHeader()
        {
            try
            {
                XmlDocument headerxml = new XmlDocument();

                headerxml.Load(Server.MapPath(@"~\SOAHeader.xml"));

                try
                {
                    SOAHeader.Responsibility = headerxml.GetElementsByTagName("Responsibility")[0].InnerText;
                }
                catch (Exception)
                {
                    SOAHeader.Responsibility = "CUX_SOA_ACCESS_RESP";
                }
                try
                {
                    SOAHeader.RespApplication = headerxml.GetElementsByTagName("RespApplication")[0].InnerText;
                }
                catch (Exception)
                {
                    SOAHeader.RespApplication = "CUX";
                }
                try
                {
                    SOAHeader.SecurityGroup = headerxml.GetElementsByTagName("SecurityGroup")[0].InnerText;
                }
                catch (Exception)
                {
                    SOAHeader.SecurityGroup = "STANDARD";
                }
                try
                {
                    SOAHeader.NLSLanguage = headerxml.GetElementsByTagName("NLSLanguage")[0].InnerText;
                }
                catch (Exception)
                {
                    SOAHeader.NLSLanguage = "AMERICAN";
                }
                try
                {
                    SOAHeader.Org_Id = headerxml.GetElementsByTagName("Org_Id")[0].InnerText;
                }
                catch (Exception)
                {
                    SOAHeader.Org_Id = "81";
                }
                try
                {

                    SOAHeader.UserName = headerxml.GetElementsByTagName("UserName")[0].InnerText;
                }
                catch (Exception)
                {
                    SOAHeader.UserName = "SOA_COMMON";
                }
                try
                {
                    SOAHeader.Password = headerxml.GetElementsByTagName("Password")[0].InnerText;
                }
                catch (Exception)
                {
                    SOAHeader.Password = "111111";
                }
                try
                {

                    SOAHeader.System_Code = headerxml.GetElementsByTagName("System_Code")[0].InnerText;
                }
                catch (Exception)
                {
                    SOAHeader.System_Code = "TJ-WZ";
                }
            }
            catch (Exception)
            {
                SOAHeader.Responsibility = "CUX_SOA_ACCESS_RESP";
                SOAHeader.RespApplication = "CUX";
                SOAHeader.SecurityGroup = "STANDARD";
                SOAHeader.NLSLanguage = "AMERICAN";
                SOAHeader.Org_Id = "81";

                SOAHeader.UserName = "SOA_COMMON";
                SOAHeader.Password = "111111";

                SOAHeader.System_Code = "TJ-WZ";
            }

            return SOAHeader;
        }

        #endregion

        #region GetRqStatus 需求申请提交状态变更信息

        protected void GetRQStatus()
        {
            var db = new MMSDbDataContext();
            //20	提供最新变化数据WS
            var sent = (from p in db.GetRqStatus_Sent orderby p.ID descending select p).Take(1).ToList();
            
        }

        #endregion
    }

    public class SOAHeaders
    {
        public string Responsibility { get; set; }
        public string RespApplication { get; set; }
        public string SecurityGroup { get; set; }
        public string NLSLanguage { get; set; }
        public string Org_Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string System_Code { get; set; }
    }
}