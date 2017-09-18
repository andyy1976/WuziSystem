using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Camc.Web.Library;
using System.Configuration;
using System.Data;
using Telerik.Web.UI;


namespace mms.Plan
{
    public partial class QueryItemCode : System.Web.UI.Page
    {
        string DBConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            if (!IsPostBack)
            {
                string strSQL = " select distinct dbo.Get_StrArrayStrOfIndex(Seg6,'.',1) as Seg6, substring(Seg5,1,4) as Type"
                    + " from [dbo].[GetCommItem_T_Item] order by substring(Seg5,1,4)";
                DataTable dt = DBI.Execute(strSQL, true);

                RDDLMT.DataSource = dt;
                RDDLMT.DataTextField = "Seg6";
                RDDLMT.DataValueField = "Type";
                RDDLMT.DataBind();

                Telerik.Web.UI.DropDownListItem li = new Telerik.Web.UI.DropDownListItem("物资编码查询", "ItemCode");
                RDDLMT.Items.Add(li);
            }
        }

        protected void RDDLMT_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            string value = RDDLMT.SelectedValue.ToString();
            string prefix = RDDLMT.SelectedText.ToString() + ".";
            string strSQL = "";

            RDDLMT1.SelectedIndex = 0;
            RDDLMT2.SelectedIndex = 0;
            RDDLMT3.SelectedIndex = 0;
            RDDLMT4.SelectedIndex = 0;

            RDDLMT1.Items.Clear();
            RDDLMT2.Items.Clear();
            RDDLMT3.Items.Clear();
            RDDLMT4.Items.Clear();

            if (value == "ItemCode")
            {
                div1.Visible = false;
                div2.Visible = true;
            }
            else if (value != "")
            {
                strSQL = " select '' as Seg6 union select dbo.Get_StrArrayStrOfIndex(Seg6,'.',2) as Seg6"
                    + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%'";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
                RDDLMT1.DataSource = dt;
                RDDLMT1.DataTextField = "Seg6";
                RDDLMT1.DataValueField = "RowsId";
                RDDLMT1.DataBind();

                div1.Visible = true;
                div2.Visible = false;
            }
            else
            {
                div1.Visible = false;
                div2.Visible = false;
            }
        }

        protected void RDDLMT1_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string prefix = RDDLMT.SelectedText.ToString() + "." + RDDLMT1.SelectedText.ToString() + ".";
            string value = RDDLMT1.SelectedValue.ToString();
            string strSQL = "";

            RDDLMT2.SelectedIndex = 0;
            RDDLMT3.SelectedIndex = 0;
            RDDLMT4.SelectedIndex = 0;
            RDDLMT2.Items.Clear();
            RDDLMT3.Items.Clear();
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                strSQL = " select '' as Seg6 union select dbo.Get_StrArrayStrOfIndex(Seg6,'.',3) as Seg6"
                    + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%'";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
                RDDLMT2.DataSource = dt;
                RDDLMT2.DataTextField = "Seg6";
                RDDLMT2.DataValueField = "RowsId";
                RDDLMT2.DataBind();
            }
        }

        protected void RDDLMT2_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string prefix = RDDLMT.SelectedText.ToString() + "." + RDDLMT1.SelectedText.ToString() + "." + RDDLMT2.SelectedText.ToString() + ".";
            string value = RDDLMT2.SelectedValue.ToString();
            string strSQL = "";

            RDDLMT3.SelectedIndex = 0;
            RDDLMT4.SelectedIndex = 0;
            RDDLMT3.Items.Clear();
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                strSQL = " select '' as Seg6 union select dbo.Get_StrArrayStrOfIndex(Seg6,'.',4) as Seg6"
                    + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%'";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
                RDDLMT3.DataSource = dt;
                RDDLMT3.DataTextField = "Seg6";
                RDDLMT3.DataValueField = "RowsId";
                RDDLMT3.DataBind();
            }
        }

        protected void RDDLMT3_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            string prefix = RDDLMT.SelectedText.ToString() + "." + RDDLMT1.SelectedText.ToString() + "." + RDDLMT2.SelectedText.ToString() + "." + RDDLMT3.SelectedText.ToString() + ".";
            string value = RDDLMT2.SelectedValue.ToString();
            string strSQL = "";

            RDDLMT4.SelectedIndex = 0;
            RDDLMT4.Items.Clear();

            if (value != "")
            {
                strSQL = " select '' as Seg6 union select dbo.Get_StrArrayStrOfIndex(Seg6,'.',5) as Seg6"
                    + " from [dbo].[GetCommItem_T_Item] where Seg6 like '" + prefix + "%'";
                DataTable dt = Common.AddTableRowsID(DBI.Execute(strSQL, true));
                RDDLMT4.DataSource = dt;
                RDDLMT4.DataTextField = "Seg6";
                RDDLMT4.DataValueField = "RowsId";
                RDDLMT4.DataBind();
            }
        }

        protected void RB_Search_Click(object sender, EventArgs e)
        {
            string strSQL = "select * from GetCommItem_T_Item where SEG10 = 'N'";
            
            string Material_Name = RTB_Material_Name.Text.Trim();
            string Material_Paihao = RTB_Material_Paihao.Text.Trim();
            string Material_Guige = RTB_Material_Guige.Text.Trim();
            string Material_Biaozhun = RTB_Material_Biaozhun.Text.Trim();
            strSQL += " and SEG12 like '%" + Material_Name + "%'";
            strSQL += " and SEG13 like '%" + Material_Paihao + "%'";

            strSQL += " and SEG14 like '%" + Material_Guige + "%'";

            strSQL += " and SEG16 like '%" + Material_Biaozhun + "%'";
            string MTv = RDDLMT.SelectedValue.ToString();
            if (MTv == "")
            {

            }
            else if (MTv == "ItemCode")
            {
                string ItemCode = RTB_ItemCode.Text.Trim();
                strSQL += " and SEG3 like '%" + ItemCode + "%'"; ;
            }
            else
            {
                string MT = RDDLMT.SelectedText.ToString();
                string MT1 = RDDLMT1.SelectedText.ToString();
                string MT2 = RDDLMT2.SelectedText.ToString();
                string MT3 = RDDLMT3.SelectedText.ToString();
                string MT4 = RDDLMT4.SelectedText.ToString();

                string SEG6 = "";
                if (MT != "") { SEG6 += MT; }
                if (MT1 != "") { SEG6 += "." + MT1; }
                if (MT2 != "") { SEG6 += "." + MT2; }
                if (MT3 != "") { SEG6 += "." + MT3; }
                if (MT4 != "") { SEG6 += "." + MT4; }
                strSQL += " and SEG6 like '" + SEG6 + "%'";
            }
            Session["gds"] = DBI.Execute(strSQL, true);
            RadGrid1.Rebind();
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = (Session["gds"] as DataTable);
        }
    }
}