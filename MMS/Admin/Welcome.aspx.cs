using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Camc.Web.Library;

namespace mms.Admin
{
    public partial class Welcome : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (Session["UserId"] == null) {  Response.Redirect("/Default.aspx"); }
            if (!IsPostBack)
            {                
                string userId = Session["UserId"].ToString();
                string strSQL = " select DeptCode, Dept from Sys_DeptEnum where ID = (select Dept from Sys_UserInfo_PWD where Id = '" + userId + "')";
                DataTable dt = DBI.Execute(strSQL, true);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["DeptCode"].ToString() == "A")
                    {
                        strSQL = " select PlanName, (select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and Material_State = '4' and Is_del = 'false') as MaterialState4"
                            + " , (select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and (Material_State = '5' or Material_State = '6') and Is_del = 'false') as MaterialState5 "
                            + " from P_Pack where IsDel = 'false'"
                            + " and( (select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and Material_State = '4' and Is_del = 'false') > 0"
                            + " or (select count(*) from M_Demand_DetailedList_Draft where PackId = P_Pack.PackId and (Material_State = '5' or Material_State = '6') and Is_del = 'false')  > 0)";
                        DataTable dtmddld = DBI.Execute(strSQL, true);
                        RadGridPlan.DataSource = dtmddld;
                        RadGridPlan.DataBind();
                    }
                }
            }
        }
    }
}