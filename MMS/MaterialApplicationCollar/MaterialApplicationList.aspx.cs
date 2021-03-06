﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Camc.Web.Library;
using Telerik.Web.UI;

namespace mms.MaterialApplicationCollar
{
    public partial class MaterialApplicationList : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["UserId"] == null) 
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
                return;
            }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                GetMA();
            }
        }

        public void GetMA()
        {
            if (Request.QueryString["MDMLID"] != null && Request.QueryString["MDMLID"].ToString() != "")
            {
                HFMDMLID.Value = Request.QueryString["MDMLID"].ToString();

                string strSQL = "select *,case when AppState = '1' then '未进入流程平台' when AppState='2' then '进入流程平台' when AppState='3' then '取消审批'"
                + " when AppState ='4' then '已审批已通过' when AppState = '5' then '已审批未通过' when AppState='6' then '已退回' else Convert(nvarchar(50),AppState) end as AppState1"
                + " , case when MaterialApplication.Type = '0' then '型号投产' when MaterialApplication.Type = '1' then '试验件' when MaterialApplication.Type='2' then '技术创新课题'  when MaterialApplication.Type='3' then '车间备料' when MaterialApplication.Type ='4' then '无需求' else Convert(nvarchar(50), MaterialApplication.Type) end as Type1"
                    + " from MaterialApplication where Is_Del='false' and Material_Id='" + HFMDMLID.Value ;
               
                strSQL += "' order by ID desc";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
                Session["MAQuery"] = dt;
            }
        }

        protected void RadGridMA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGridMA.DataSource = Session["MAQuery"];
        }

        protected void RadGridMA_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                if (RadGridMA.SelectedItems.Count > 0)
                {
                    string id = ((RadGridMA.SelectedItems[0]) as GridDataItem).GetDataKeyValue("ID").ToString();
                    string strSQL = " if (select AppState from MaterialApplication where ID = '" +id +"') in ('1','3','5','6') begin" +
                        " Update MaterialApplication set Is_Del = 'true' where Id = '" + id + "' select '" + id +"' end else begin select '0' end";
                    string result = DBI.GetSingleValue(strSQL);
                    if (result == "0")
                    {
                        RadNotificationAlert.Text = "失败！该申请单不能删除";
                        RadNotificationAlert.Show();
                    }
                    else
                    {
                        RadNotificationAlert.Text = "删除成功！";
                        RadNotificationAlert.Show();
                        GetMA();
                        RadGridMA.Rebind();
                    }
                }
                else
                {
                    RadNotificationAlert.Text = "失败！请选择删除行";
                    RadNotificationAlert.Show();
                }
            }
        }

        protected void RadGridMA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadGridMA.SelectedItems.Count > 0)
            {
                HFMAID.Value = ((RadGridMA.SelectedItems[0]) as GridDataItem).GetDataKeyValue("ID").ToString();
               // HFMDMLID.Value = ((RadGridMA.SelectedItems[0]) as GridDataItem)["Material_Id"].Text;
                HFAppSate1.Value = ((RadGridMA.SelectedItems[0]) as GridDataItem)["AppState1"].Text;
                string type1 = ((RadGridMA.SelectedItems[0]) as GridDataItem)["Type1"].Text;
                if (type1 == "型号投产")
                {
                    HFType.Value = "0";
                }
                else if (type1 == "试验件")
                {
                    HFType.Value = "1";
                }
                else if (type1 == "技术创新课题")
                {
                    HFType.Value = "2";
                }
                else if (type1 == "车间备料")
                {
                    HFType.Value = "3";
                }
                else if (type1 == "无需求")
                {
                    HFType.Value = "4";
                }
            }
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                GetMA();
                RadGridMA.Rebind();
            }
            else
            {
                throw new Exception("刷新页面出错，请按F5刷新！");
            }

        }

      
        protected void RadGridMA_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
                string AppState = (e.Item as GridDataItem)["AppState1"].Text.ToString();

                if (AppState == "未进入流程平台")
                {
                    RadButton rb = e.Item.FindControl("RB_K2") as RadButton;
                    if (rb != null)
                    {
                        rb.Visible = false;
                    }
                }
                if (AppState == "进入流程平台")
                {
                    try
                    {
                        k2.SubmitInfoModel model = new k2.SubmitInfoModel();
                        model.MMS_ID = Convert.ToInt32(id);
                        model.UserAccount = DBI.GetSingleValue("select DomainAccount from Sys_UserInfo_PWD where ID = '" +
                                       Session["UserId"].ToString() + "'");

                        k2.K2WebServiceForMMS k2mms = new k2.K2WebServiceForMMS();
                        k2.ApproveInfoHead head = k2mms.GetApproveHeader(model);

                        if (head != null)
                        {
                            if (head.AppState == 4)
                            {
                                string strSQL = " Update MaterialApplication set AppState = '" + head.AppState.ToString() + "' where ID = '" + id + "'" +
                                    " Insert into MaterialApplication_Log (MaterialApplicationId, Operation_UserId, Operation_Time, Operation_Remark)"
                                + " values ('" + HFMAID.Value + "', '" + Session["UserId"].ToString() + "',GetDate(),'流程平台结束，结果：' + '" + head.AppState + "')";
                                DBI.Execute(strSQL);

                                (e.Item as GridDataItem)["AppState1"].Text = "已审批已通过";
                            }
                            else if (head.AppState == 5)
                            {

                                string strSQL = " Update MaterialApplication set AppState = '" + head.AppState.ToString() + "',is_cancelData = 'true" + "' where ID = '" + id + "'" +
                                      " Insert into MaterialApplication_Log (MaterialApplicationId, Operation_UserId, Operation_Time, Operation_Remark)"
                                  + " values ('" + HFMAID.Value + "', '" + Session["UserId"].ToString() + "',GetDate(),'流程平台结束，结果：' + '" + head.AppState + "')";
                                DBI.Execute(strSQL);

                                (e.Item as GridDataItem)["AppState1"].Text = "已审批未通过";
                                string is_cancelData = (e.Item as GridDataItem)["is_cancelData"].Text.ToString();
                                if (is_cancelData == "False" || is_cancelData == null)
                                {
                                    string PleaseTakeQuality = (e.Item as GridDataItem)["PleaseTakeQuality"].Text.ToString();
                                    string Quantity = (e.Item as GridDataItem)["Quantity"].Text.ToString();

                                    strSQL =
                                             " Update M_Demand_Merge_List set Quantity_Applied = Quantity_Applied=-'" + Quantity + "',DemandNum_Applied=DemandNum_Applied-'" + PleaseTakeQuality +
                                            "',DemandNum_Left=DemandNum_Left+'" + PleaseTakeQuality + "',Quantity_Left=Quantity_Left+'" + Quantity +

                                                        "' where ID = '" + HFMDMLID.Value + "'";
                                    DBI.Execute(strSQL);
                                }

                            }   
                        }
                    }
                    catch
                    {
                        RadNotificationAlert.Text = "暂时不能与流程平台通讯，无法获取审批最新状态";
                        RadNotificationAlert.Show();
                    }
                }

                RadButton RB_K2 = e.Item.FindControl("RB_K2") as RadButton;
                if (RB_K2 != null)
                {
                    RB_K2.Attributes["onclick"] = String.Format("return ShowK2({0});", id);
                }
            }
            
        }
		protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGridMA.ExportSettings.FileName = "申请单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMA.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGridMA.ExportSettings.FileName = "申请单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMA.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGridMA.ExportSettings.FileName = "申请单列表" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGridMA.ExportSettings.IgnorePaging = true;
            RadGridMA.MasterTableView.ExportToPdf();
            RadGridMA.ExportSettings.IgnorePaging = false;
        }
    }
}
