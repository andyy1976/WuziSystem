using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Camc.Web.Library;
using System.Configuration;

namespace mms.Plan
{
    public partial class WriteReqOrderRecList : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Response.Redirect("/Default.aspx"); }
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                Common.CheckPermission(Session["UserName"].ToString(), "WriteReqOrderRecList", this.Page);
                Session["GridSource"] = GetWriteReqOrderRecList("");
                 GetWriteRcoOrderRecList("");
            }
        }

        public DataTable GetWriteReqOrderRecList(string strWhere)
        {
            var strSql = "";
            strSql =
                " select WriteReqOrder_RecList.USER_RQ_LINE_ID, ERR_MSG , Material_Name, Rough_Size, Rough_Spec" +
                " , Special_Needs, a.Dict_Name as Urgency_Degre, Secret_Level,Use_Des, Shipping_Address, Certification, MANUFACTURER" +
                " from WriteReqOrder_RecList join M_Demand_Merge_List on M_Demand_Merge_List.ID = WriteReqOrder_RecList.USER_RQ_LINE_ID " +
                " left join GetBasicdata_T_Item as a on a.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and a.DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                " where 1 = 1 " + strWhere + " order by WriteReqOrder_RecList.ID desc";
           return Common.AddTableRowsID(DBI.Execute(strSql, true));
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = Session["GridSource"];
        }

        protected DataTable GetWriteRcoOrderRecList(string strWhere)
        {
            string strSql = "select USER_RCO_LINE_ID, Err_Msg " +
                        ", case	when Column_Changed = 'USER_RQ_NUMBER' then '用户需求编号' " +
                        " when  Column_Changed = 'SPECIAL_REQUEST' then '特殊要求'" +
                        " when Column_Changed = 'QUANTITY' then '需求数量'" +
                        " when Column_Changed = 'PIECE' then '件数'" +
                        " when Column_Changed = 'DIMENSION' then '尺寸'" +
                        " when Column_Changed = 'MANUFACTURER_ID' then '指定生产厂家'" +
                        " when Column_Changed = 'RQ_DATE' then '需求时间'" +
                        " when Column_Changed = 'URGENCY_LEVEL' then '紧急程度'" +
                        " when Column_Changed = 'REQUESTER' then '申请人'" +
                        " when Column_Changed = 'REQUESTER_PHONE' then '申请人联系电话'" +
                        " when Column_Changed = 'USER_ITEM_DESCRIPTION' then '无编码物资说明'" +
                        " when Column_Changed = 'UNANIMOUS_BATCH' then '同批次'" +
                        " when Column_Changed = 'SECURITY_LEVEL' then '密级'" +
                        " when Column_Changed = 'PROJECT' then '型号工程'" +
                        " when Column_Changed = 'PHASE' then '研制阶段'" +
                        " when Column_Changed = 'BATCH' then '批组号'" +
                        " when Column_Changed = 'BATCH_QTY' then '当量/发数'" +
                        " when Column_Changed = 'USAGE' then '用途'" +
                        " when Column_Changed = 'TASK' then '任务号'" +
                        " when Column_Changed = 'SUBJECT' then '课题号'" +
                        " when Column_Changed = 'CUSTOMER_ACCOUNT_ID' then '需求部门'" +
                        " when Column_Changed = 'DELIVERY_ADDRESS' then '配送地址'" +
                        " when Column_Changed = 'CUSTOMER_ID' then '需求单位'" +
                        " when Column_Changed = 'ATTRIBUTE2' then '指定采购部门'" +
                        " when Column_Changed = 'ATTRIBUTE3' then '开具合格证'" +
                        " else Column_Changed end" +
                        " as Column_Changed, Original_Value, Changed_Value" +
                        " , M_Demand_Merge_List.ID" +
                        " , Material_Name, Rough_Size, Rough_Spec" +
                        " , Special_Needs, a.Dict_Name as Urgency_Degre, Secret_Level, Use_Des, Shipping_Address, Certification, MANUFACTURER" +
                        " from WriteRcoOrder_RecList" +
                        " join M_Change_Record on M_Change_Record.ID = WriteRcoOrder_RecList.USER_RCO_LINE_ID" +
                        " join M_Demand_Merge_List on M_Demand_Merge_List.ID = M_Change_Record.MDMId" +
                        " left join GetBasicdata_T_Item as a on a.DICT_CODE = M_Demand_Merge_List.Urgency_Degre and a.DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                        " where 1 = 1" + strWhere + " order by WriteRcoOrder_RecList.Id desc";
            return Common.AddTableRowsID(DBI.Execute(strSql, true));
        }

        protected void RadGrid2_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid2.DataSource = Session["GridSourceRco"];
        }

        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string tasktype = RDDL_TaskType.SelectedValue.ToString();
            string start = RDP_Start.SelectedDate.ToString();
            string end = RDP_End.SelectedDate.ToString();
            string headerId = RTB_HeaderID.Text.Trim();
            string lineId = RTB_LineID.Text.Trim();

            string strWhere = "";
            if (tasktype != "")
            {
                strWhere += " and Submit_Type = '" + tasktype +"'";
            }
            if (start != "")
            {
                strWhere += " and Submit_Date >= '" + start + "'";
            }
            if (end != "")
            {
                strWhere += " and Submit_Date <= '" + Convert.ToDateTime(end).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (headerId != "")
            {
                strWhere += " and M_Demand_Merge_List.MDPID = '" + headerId + "'";
            }
            if(lineId != "")
            {
                strWhere += " and M_Demand_Merge_List.ID = '" + lineId + "'";
            }
            Session["GridSource"] = GetWriteReqOrderRecList(strWhere);
            RadGrid1.Rebind();
        }

        protected void RB_SearchRco_Click(object sender, EventArgs e)
        {
            string tasktype = RDDL_TaskTypeRco.SelectedValue.ToString();
            string start = RDP_StartRco.SelectedDate.ToString();
            string end = RDP_EndRco.SelectedDate.ToString();
            string rqlineid = RTB_RQ_LineId.Text.Trim();
            string rcoLineId = RTB_RCO_LintId.Text.Trim();

            string strWhere = "";
            if (tasktype != "")
            {
                strWhere += " and Submit_Type = '" + tasktype + "'";
            }
            if (start != "")
            {
                strWhere += " and Change_Date >= '" + start + "'";
            }
            if (end != "")
            {
                strWhere += " and Change_Date <= '" + Convert.ToDateTime(end).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (rqlineid != "")
            {
                strWhere += " and M_Change_Record.MDMID = '" + rqlineid + "'";
            }
            if (rcoLineId != "")
            {
                strWhere += " and M_Change_Record.ID = '" + rcoLineId + "'";
            }
            Session["GridSourceRco"] = GetWriteRcoOrderRecList(strWhere);
            RadGrid2.Rebind();
        }

        protected void RadButton_ExportExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心返回错误列表--变更" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadButton_ExportWord_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心返回错误列表--变更" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void RadButton_ExportPdf_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = "物流中心返回错误列表--变更" + DateTime.Now.ToString("yyyy-MM-dd");
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.MasterTableView.ExportToPdf();
            RadGrid1.ExportSettings.IgnorePaging = false;
        }
    }
}