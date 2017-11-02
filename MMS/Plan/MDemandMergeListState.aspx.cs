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
using System.Drawing;

namespace mms.Plan
{
    public partial class MDemandMergeListState : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string DBConn =
                    ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
                DBInterface DBI = DBFactory.GetDBInterface(DBConn);

                string strSQL = " select a.*, b.DICT_Name as Urgency_Degre1, c.DICT_Name as Use_Des1 , isnull(Sys_Phase.Phase, Stage) as Phase1 from M_Demand_Merge_List as a " +
                    " left join GetBasicdata_T_Item as b on b.DICT_CODE = a.Urgency_level and b.DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                    " left join Sys_Phase on Sys_Phase.Code = a.Stage" +
                    " where MDPID = '" + Request.QueryString["MDPID"].ToString() + "' and Is_Submit = '1'";
                Session["GridSource"] = DBI.Execute(strSQL, true);

                strSQL = "select a.*, isnull((select ERR_MSG from WriteReqOrder_Rec where GROUP_ID = (select GROUP_ID from WriteReqOrder_T_List where USER_RQ_LINE_ID = a.ID)),'') + isnull(WriteReqOrder_RecList.Err_Msg,'')" +
                    " , b.DICT_Name as Urgency_Degre1, c.DICT_Name as Use_Des1 , isnull(Sys_Phase.Phase, Stage) as Phase1" +
                    " from M_Demand_Merge_List as a" +
                    " left join WriteReqOrder_RecList on WriteReqOrder_RecList.User_RQ_Line_ID = a.ID" +
                    " left join GetBasicdata_T_Item as b on b.DICT_CODE = a.Urgency_level and b.DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                    " left join GetBasicdata_T_Item as c on  c.DICT_CODE = a.Use_Des and  c.DICT_CLASS='CUX_DM_USAGE'" +
                    " left join Sys_Phase on Sys_Phase.Code = a.Stage" +
                    " where MDPID = '" + Request.QueryString["MDPID"].ToString() +"' and a.Is_Submit = 'false'";
                Session["GridSourceFailed"] = DBI.Execute(strSQL, true);
            }
        }

        protected void RadGrid1_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = (Session["GridSource"] as DataTable);
        }

        protected void RadGridFailed_ItemCommand(object sender, GridCommandEventArgs e)
        {
            RadGridFailed.DataSource = Session["GridSourceFailed"];
        }
    }
}