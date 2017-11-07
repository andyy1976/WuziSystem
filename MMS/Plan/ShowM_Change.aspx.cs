using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Camc.Web.Library;
using Telerik.Web.UI;
using System.Xml;

namespace mms.Plan
{
    public partial class ShowM_Change : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null || Session["UserId"] == null)
                {
                    Response.Redirect("/Default.aspx");
                }
                Common.CheckPermission(Session["UserName"].ToString(), "ShowM_Change", this.Page);   
                if (Request.QueryString["PackID"] == null)
                {
                    Response.Redirect("/Plan/ShowPlan.aspx");
                    return;
                }
                GetMCL();

                string DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
                DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);
                string strSQL =
                   " select Sys_Model.Model ,PlanName from P_Pack left join Sys_Model on Sys_Model.ID = P_Pack.Model where PackId = '" +
                   Request.QueryString["PackId"].ToString() + "'";
                DataTable dtpack = DBI.Execute(strSQL, true);
                lblModel.Text = dtpack.Rows[0]["Model"].ToString();
                lblPlanName.Text = dtpack.Rows[0]["PlanName"].ToString();
            }
        }

        public void GetMCL()
        {
            string PackId = Request.QueryString["PackID"].ToString();
            string DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
            DBInterface DBI = DBFactory.GetDBInterface(DBContractConn);

            string strSQL = " select ChangeList_Code, Drawing_No, Relevance_Dept, TDM_Description, Change_Reason,Remark, Reason,Change_UserName"
                + " ,CN_Drawing_No, Name, CN_Com_Drawing_No, CN_Edit_Comment, CN_Cur_Content, CN_Edit_Type, CN_Technics_Line, ChangePerson,Influence, Change_Date"
                + " ,case when CN_Edit_Type = '0' or CN_Edit_Type = '3' then '修改' when CN_Edit_Type = '1' or CN_Edit_Type = '2' then '新增' when CN_Edit_Type = '4' or CN_Edit_Type= '5' then '删除' else CN_Edit_Type end as CN_Edit_Type1"
                + " from M_Change_List left join M_Change_List_Detailed on M_Change_List.ID = M_Change_List_Detailed.MCLID"
                + " where PackID = '" + PackId + "' order by Change_Date desc";        
            this.ViewState["gs"] = Common.AddTableRowsID(DBI.Execute(strSQL, true));
        }

        protected void RadGridMCL_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGridMCL.DataSource = this.ViewState["gs"] as DataTable;
        }

        protected void RadGridMCL_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if(e.Item is GridDataItem)
            {
                string RowsId = ((e.Item) as GridDataItem).GetDataKeyValue("RowsId").ToString();
                DataRow datarow = (this.ViewState["gs"] as DataTable).Select("RowsId='" + RowsId + "'")[0];

                if (datarow["CN_EDIT_COMMENT"].ToString() != "" || datarow["CN_CUR_CONTENT"].ToString() != "")
                {
                    XmlDocument PropertyNameDoc = new XmlDocument();
                    XmlDocument PropertyNameDBDoc = new XmlDocument();
                    try
                    {
                        PropertyNameDoc.Load(Server.MapPath(@"~\Plan\PropertyName.xml"));
                        PropertyNameDBDoc.Load(Server.MapPath(@"~\Plan\PropertyNameDB.xml"));

                    }
                    catch { }

                    XmlDocument beforeDoc = new XmlDocument();
                    XmlDocument afterDoc = new XmlDocument();
                    try
                    {
                        beforeDoc.LoadXml(datarow["CN_EDIT_COMMENT"].ToString());
                        afterDoc.LoadXml(datarow["CN_CUR_CONTENT"].ToString());
                    }
                    catch { }

                    string tableStr = "<Table  border='1' style='border-right: thin solid; border-top: thin solid; border-left: thin solid;width: 100%; border-bottom: thin solid'>";
                    tableStr = tableStr + "<tr>";
                    tableStr = tableStr + "<td>更改内容</td>";
                    tableStr = tableStr + "<td>更改前</td>";
                    tableStr = tableStr + "<td>更改后</td>";
                    tableStr = tableStr + "</tr>";

                    string nodeName = "";
                    string nodeDes = "";
                    string valueBefore = "";
                    string valueAfter = "";

                    try
                    {
                        foreach (XmlNode tempNode in afterDoc.ChildNodes[1].ChildNodes)
                        {
                            nodeName = tempNode.Name.ToString();
                            try
                            {
                                if (beforeDoc.GetElementsByTagName(nodeName)[0].InnerText != null)
                                {
                                    valueBefore = beforeDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString();
                                }
                            }
                            catch (System.Exception)
                            {
                                valueBefore = "";
                            }
                            try
                            {
                                if (afterDoc.GetElementsByTagName(nodeName)[0].InnerText != null)
                                {
                                    valueAfter = afterDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString();
                                }
                            }
                            catch (System.Exception)
                            {
                                valueAfter = "";
                            }
                            try
                            {
                                if (PropertyNameDoc.GetElementsByTagName(nodeName)[0].InnerText != null)
                                {
                                    nodeDes = PropertyNameDoc.GetElementsByTagName(nodeName)[0].InnerText.ToString();
                                }
                                else { nodeDes = nodeName; }
                            }
                            catch { nodeDes = nodeName; }

                            if (valueBefore != valueAfter)
                            {
                                tableStr = tableStr + "<tr>";
                                tableStr = tableStr + "<td>" + nodeDes + "</td>";
                                tableStr = tableStr + "<td>" + valueBefore + "</td>";
                                tableStr = tableStr + "<td>" + valueAfter + "</td>";
                                tableStr = tableStr + "</tr>";
                            }
                        }
                    }
                    catch (System.Exception) { }

                    tableStr = tableStr + "</Table>";
                    (e.Item as GridDataItem)["ChangeContent"].Text = tableStr;
                }
            }
        }
    }
}