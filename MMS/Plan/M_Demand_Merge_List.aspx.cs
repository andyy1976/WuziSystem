using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Camc.Web.Library;

namespace mms.Plan
{
    public partial class M_Demand_Merge_List : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                this.ViewState["GridSource"] = GetM_Demand_Merge_List("");
            }
        }

        protected DataTable GetM_Demand_Merge_List(string strWhere) {
            DataTable dt = new DataTable();
            string PackId = Request.QueryString["PackId"].ToString();
            string strSQL = " select a.ID, Correspond_Draft_Code, a.Drawing_No, Task as Task_Code, TechnicsLine" +
                " , ISNULL((select Dept from Sys_DeptEnum where DeptCode = a.MaterialDept), a.MaterialDept) as Material_Dept" +
                " ,a.MaterialDept ,a.ItemCode1 ,Material_Name ,a.DemandNumSum ,a.NumCasesSum ,a.Special_Needs" +
                " , isnull((select top 1 DICT_NAME from GetBasicdata_T_Item where DICT_CLASS = 'CUX_DM_URGENCY_LEVEL' and DICT_CODE = a.Urgency_Degre), a.Urgency_Degre) as Urgency_Degre" +
                " , Secret_Level" +
                " , isnull((select top 1 DICT_NAME from GetBasicdata_T_Item where DICT_CLASS = 'CUX_DM_USAGE' and DICT_CODE = a.Use_Des), a.Use_Des) as Use_Des" +
                " ,isnull((select top 1 ADDRESS from GetCustInfo_T_ACCT_SITE where Convert(nvarchar(50),LOCATION_ID) = a.Shipping_Address), a.Shipping_Address) as Shipping_Address" +
                " , Certification , a.DemandDate" +
                " from M_Demand_Merge_List as a" +
                " join M_Demand_DetailedList_Draft on CONVERT(nvarchar(50), M_Demand_DetailedList_Draft.Id) = a.Correspond_Draft_Code" +
                " where a.PackId ='" + PackId + "'" +
                strWhere;
            
            dt = DBI.Execute(strSQL, true);
            return dt;
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = (this.ViewState["GridSource"] as DataTable);
        }
    }
}