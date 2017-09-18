using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Collections;
using Camc.Web.Library;
using System.Configuration;

namespace mms.Plan
{
    public partial class MDemandChangeSubmit : System.Web.UI.Page
    {
        private DataTable GridSource
        {
            get
            {
                Object obj = this.ViewState["_gds"];
                if (obj != null)
                {
                    return (DataTable)obj;
                }
                else
                {
                    DataTable InitTable = new DataTable();
                    InitTable.Columns.Add("RowsId");
                    InitTable.Columns.Add("ID");
                    InitTable.Columns.Add("Correspond_Draft_Code");
                    InitTable.Columns.Add("Drawing_No");
                    InitTable.Columns.Add("PackId");
                    InitTable.Columns.Add("TaskId");
                    InitTable.Columns.Add("DraftId");
                    InitTable.Columns.Add("MDMId");
                    InitTable.Columns.Add("MDPId");
                    InitTable.Columns.Add("TechnicsLine");
                    InitTable.Columns.Add("ItemCode1");
                    InitTable.Columns.Add("NumCasesSum");
                    InitTable.Columns.Add("DemandNumSum");
                    InitTable.Columns.Add("Mat_Unit");
                    InitTable.Columns.Add("Quantity");
                    InitTable.Columns.Add("Rough_Size");
                    InitTable.Columns.Add("Rough_Spec");
                    InitTable.Columns.Add("MaterialsDes");
                    InitTable.Columns.Add("Special_Needs");
                    InitTable.Columns.Add("Urgency_Degre");
                    InitTable.Columns.Add("Secret_Level");
                    InitTable.Columns.Add("Stage");
                    InitTable.Columns.Add("Use_Des");
                    InitTable.Columns.Add("Shipping_Address");
                    InitTable.Columns.Add("Certification");
                    InitTable.Columns.Add("Unit_Price");
                    InitTable.Columns.Add("Sum_Price");
                    InitTable.Columns.Add("Is_Submit");
                    InitTable.Columns.Add("User_ID");
                    InitTable.Columns.Add("Submit_Date");
                    InitTable.PrimaryKey = new DataColumn[] { InitTable.Columns["ID"] };
                    this.ViewState["_gds"] = InitTable;
                    return InitTable;
                }
            }
            set
            {
                this.ViewState["_gds"] = value;
                ((DataTable)this.ViewState["_gds"]).PrimaryKey = new DataColumn[] { ((DataTable)this.ViewState["_gds"]).Columns["ID"] };
            }
        }
        private static string DBConn;
        private DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);

            if (!IsPostBack)
            {
                string PackId = string.Empty;
                string DraftIdStr = string.Empty;
                string StateStr = string.Empty;
                if ((Request.QueryString["PackId"] != null && Request.QueryString["PackId"].ToString() != "") &&
                    (Session["DraftIdStr"] != null && Session["DraftIdStr"].ToString() != ""))
                     //&&(Session["StateStr"] != null && Session["StateStr"].ToString() != "")
                {
                    PackId = Request.QueryString["PackId"].ToString();
                    Session["PackId"] = Request.QueryString["PackId"].ToString();
                    DraftIdStr = Session["DraftIdStr"].ToString();
                    //StateStr = Session["StateStr"].ToString();
                    GridSource = Common.AddTableRowsID(Get_XHRW_Change_Record(PackId, DraftIdStr));
                    this.ViewState["lastSelectDeptCode"] = "";
                    this.ViewState["lastSelectAccount"] = "";
                }
                else
                {
                   // Response.Redirect("~/Plan/MDemandDetails.aspx");
                    Response.Redirect("~/Plan/MDemandDetailsTreeList.aspx");
                }
            }
        }
        /// <summary>
        /// 获得型号任务变更信息
        /// </summary>
        /// <param name="PackId">包ID</param>
        /// <param name="DraftIdStr">ID字符串</param>
        /// <param name="StateStr">状态字符串, string StateStr</param>
        protected DataTable Get_XHRW_Change_Record(string PackId, string DraftIdStr)
        {
            try
            {
                string strSQL = @"exec Proc_Get_XHRW_Change_Record '" + PackId + "','" + DraftIdStr + "',''";
                return DBI.Execute(strSQL, true);
            }
            catch (Exception ex)
            {
                throw new Exception("获得型号任务变更信息出错" + ex.Message.ToString());
            }
        }
        protected void RadGrid_MDemandMergelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MDemandMergelist.DataSource = GridSource;
        }

        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            ((sender as RadButton).NamingContainer as GridItem).Selected = (sender as RadButton).Checked;
        }
        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            RadButton headerCheckBox = (sender as RadButton);
            foreach (GridDataItem dataItem in RadGrid_MDemandMergelist.MasterTableView.Items)
            {
                (dataItem.FindControl("RadButtonItem") as RadButton).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }

        protected void Save_XHRW_Change_Record()
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserId"].ToString());
                string PackId = Session["PackId"].ToString();
                string DraftIdStr = Session["DraftIdStr"].ToString();
                string StateStr = Session["StateStr"].ToString();
                //string strSQL = @"exec Proc_Save_XHRW_Change_Record '" + PackId + "','" + DraftIdStr + "','" + StateStr + "','" + userid + "'";
                string strSQL = @"exec Proc_Save_M_Demand_Plan_And_Change1 '" + DraftIdStr + "','','','','','',''," + userid + "," + PackId + ",'',''";
                DataTable dt = DBI.Execute(strSQL,true);
                string MDPLID = dt.Rows[0][0].ToString();
                WriteRcoOrderRepeat(MDPLID);
            }
            catch (Exception e)
            {
                throw new Exception("数据库操作出现异常" + e.Message.ToString());
            }
        }

        #region 提交变更单

        public void WriteRcoOrderRepeat(string MDPLID)
        {
            var db = new MMSDbDataContext();
            var client = new Write_Rco_Order.CUX_DM_RCO_SYNC_WS_PKG_PortTypeClient();
            var header = new Write_Rco_Order.SOAHeader
            {
                Responsibility = "CUX_SOA_ACCESS_RESP",
                RespApplication = "CUX",
                SecurityGroup = "STANDARD",
                NLSLanguage = "AMERICAN",
                Org_Id = "81"
            };

            client.ClientCredentials.UserName.UserName = "SOA_COMMON";
            client.ClientCredentials.UserName.Password = "111111";

            var qSentHeader = (from p in db.WriteRcoOrder_SentHeader
                               where p.USER_RCO_HEADER_NO == Session["MDPCode"].ToString()
                               select p).ToArray();

            for (int n = 0; n < qSentHeader.Length; n++)
            {
                var qsentList = (from p in db.WriteRcoOrder_SentLine
                                 where p.USER_RCO_HEADER_ID == qSentHeader[n].USER_RCO_HEADER_ID
                                 orderby p.ID
                                 select p).ToArray();

                string strSQL =
                        " Insert into WriteRcoOrder_Sent values ('TJ-WZ','Y','2',(select isnull(max(Group_ID),0) + 1 from WriteRcoOrder_Sent))";
                DBI.Execute(strSQL);

                var qSent = (from p in db.WriteRcoOrder_Sent
                             orderby p.ID descending
                             select p).Take(1).ToList();

                var sentSum29 = new Write_Rco_Order.APPSCUX_DM_RCO_SYNC_WS_PKG_SUM_29
                {
                    SYSTEM_CODE = qSent[0].SYSTEM_CODE,
                    TOTAL_FLAG = qSent[0].TOTAL_FLAG,
                    ROW_COUNT = qSent[0].ROW_COUNT,
                    ROW_COUNTSpecified = true,
                    GROUP_ID = qSent[0].GROUP_ID,
                    GROUP_IDSpecified = true
                };

                var sentreco34 = new Write_Rco_Order.APPSCUX_DM_RCO_SYNC_WS_PKG_RECO34[1];

                sentreco34[0] = new Write_Rco_Order.APPSCUX_DM_RCO_SYNC_WS_PKG_RECO34
                {
                    RCO_HEADER_NO = qSentHeader[n].RCO_HEADER_NO,
                    USER_RCO_HEADER_ID = qSentHeader[n].USER_RCO_HEADER_ID,
                    USER_RCO_HEADER_IDSpecified = true,
                    USER_RCO_HEADER_NO = qSentHeader[n].USER_RCO_HEADER_NO,
                    ORG_ID = qSentHeader[n].ORG_ID,
                    ORG_IDSpecified = true,
                    RCO_STATUS = qSentHeader[n].RCO_STATUS,
                    USER_RQ_ID = qSentHeader[n].USER_RQ_ID,
                    USER_RQ_IDSpecified = true,
                    USER_RQ_LINE_ID = qSentHeader[n].USER_RQ_LINE_ID,
                    USER_RQ_LINE_IDSpecified = true,
                    REASON = qSentHeader[n].REASON,
                    REQUESTER = qSentHeader[n].REQUESTER,
                    REQUESTER_PHONE = qSentHeader[n].REQUESTER_PHONE,
                    PREPARER = qSentHeader[n].PREPARER,
                    CREATION_DATE = qSentHeader[n].CREATION_DATE,
                    CREATION_DATESpecified = qSentHeader[n].CREATION_DATE != null,
                    CREATED_BY = qSentHeader[n].CREATED_BY,
                    CREATED_BYSpecified = false, //预留字段，无需传输值
                    CUSTOMER_OF_CREATOR = qSentHeader[n].CUSTOMER_OF_CREATOR, //预留，无需传值
                    SUBMITTED_BY = qSentHeader[n].SUBMITTED_BY,
                    SUBMITTED_BYSpecified = false, //预留字段，无需传输值
                    SUBMISSION_DATE = qSentHeader[n].SUBMISSION_DATE,
                    SUBMISSION_DATESpecified = qSentHeader[n].SUBMISSION_DATE != null,
                    CHANGE_OPTION = qSentHeader[n].CHANGE_OPTION,
                    SYSTEM_CODE = qSentHeader[n].SYSTEM_CODE,
                    TRANSFER_TIME = qSentHeader[n].TRANSFER_TIME,
                    TRANSFER_TIMESpecified = qSentHeader[n].TRANSFER_TIME != null,
                    GROUP_ID = sentSum29.GROUP_ID,
                    GROUP_IDSpecified = true,
                    ATTRIBUTE_CATEGORY = qSentHeader[n].ATTRIBUTE_CATEGORY,
                    ATTRIBUTE1 = qSentHeader[n].ATTRIBUTE1,
                    ATTRIBUTE2 = qSentHeader[n].ATTRIBUTE2,
                    ATTRIBUTE3 = qSentHeader[n].ATTRIBUTE3,
                    ATTRIBUTE4 = qSentHeader[n].ATTRIBUTE4,
                    ATTRIBUTE5 = qSentHeader[n].ATTRIBUTE5,
                    ATTRIBUTE6 = qSentHeader[n].ATTRIBUTE6,
                    ATTRIBUTE7 = qSentHeader[n].ATTRIBUTE7,
                    ATTRIBUTE8 = qSentHeader[n].ATTRIBUTE8,
                    ATTRIBUTE9 = qSentHeader[n].ATTRIBUTE9,
                    ATTRIBUTE10 = qSentHeader[n].ATTRIBUTE10,
                    ATTRIBUTE11 = qSentHeader[n].ATTRIBUTE11,
                    ATTRIBUTE12 = qSentHeader[n].ATTRIBUTE12,
                    ATTRIBUTE13 = qSentHeader[n].ATTRIBUTE13,
                    ATTRIBUTE14 = qSentHeader[n].ATTRIBUTE14,
                    ATTRIBUTE15 = qSentHeader[n].ATTRIBUTE15
                };

                for (int i = 0; i < qsentList.Length; i++)
                {

                    var sentreco35 = new Write_Rco_Order.APPSCUX_DM_RCO_SYNC_WS_PKG_RECO35[1];

                    sentreco35[0] = new Write_Rco_Order.APPSCUX_DM_RCO_SYNC_WS_PKG_RECO35
                    {
                        USER_RCO_LINE_ID = qsentList[i].USER_RCO_LINE_ID,
                        USER_RCO_LINE_IDSpecified = true,
                        USER_RCO_HEADER_ID = qsentList[i].USER_RCO_HEADER_ID,
                        USER_RCO_HEADER_IDSpecified = true,
                        LINE_NUM = qsentList[i].LINE_NUM,
                        LINE_NUMSpecified = true,
                        COLUMN_CHANGED = qsentList[i].COLUMN_CHANGED,
                        ORIGINAL_VALUE = qsentList[i].ORIGINAL_VALUE,
                        CHANGED_VALUE = qsentList[i].CHANGED_VALUE,
                        SYSTEM_CODE = qsentList[i].SYSTEM_CODE,
                        TRANSFER_TIME = qsentList[i].TRANSFER_TIME,
                        TRANSFER_TIMESpecified = false,
                        GROUP_ID = sentSum29.GROUP_ID,
                        GROUP_IDSpecified = true,
                        ATTRIBUTE_CATEGORY = qsentList[i].ATTRIBUTE_CATEGORY,
                        ATTRIBUTE1 = qsentList[i].ATTRIBUTE1,
                        ATTRIBUTE2 = qsentList[i].ATTRIBUTE2,
                        ATTRIBUTE3 = qsentList[i].ATTRIBUTE3,
                        ATTRIBUTE4 = qsentList[i].ATTRIBUTE4,
                        ATTRIBUTE5 = qsentList[i].ATTRIBUTE5,
                        ATTRIBUTE6 = qsentList[i].ATTRIBUTE6,
                        ATTRIBUTE7 = qsentList[i].ATTRIBUTE7,
                        ATTRIBUTE8 = qsentList[i].ATTRIBUTE8,
                        ATTRIBUTE9 = qsentList[i].ATTRIBUTE9,
                        ATTRIBUTE10 = qsentList[i].ATTRIBUTE10,
                        ATTRIBUTE11 = qsentList[i].ATTRIBUTE11,
                        ATTRIBUTE12 = qsentList[i].ATTRIBUTE12,
                        ATTRIBUTE13 = qsentList[i].ATTRIBUTE13,
                        ATTRIBUTE14 = qsentList[i].ATTRIBUTE14,
                        ATTRIBUTE15 = qsentList[i].ATTRIBUTE15
                    };

                    var input = new Write_Rco_Order.InputParameters
                    {
                        P_SUM_RECORD_INPUT = sentSum29,
                        P_HEADER_INPUT = sentreco34,
                        P_LINE_INPUT = sentreco35
                    };

                    var returnData = client.WRITE_RCO_ORDER(header, input);
                    var aa = returnData;
                    if (returnData.X_RECORD_RESULT.STATUS == "E")
                    {
                        foreach (var ERROR_RECORD in returnData.X_ERROR_RECORD)
                        {
                            WriteRcoOrder_RecList reclist = new WriteRcoOrder_RecList()
                            {
                                REC_SUM_ID = 0, //不对的
                                ORG_ID = ERROR_RECORD.ORG_ID,
                                SYSTEM_CODE = ERROR_RECORD.SYSTEM_CODE,
                                USER_RCO_HEADER_ID = ERROR_RECORD.USER_RCO_HEADER_ID,
                                USER_RCO_LINE_ID = sentreco35[0].USER_RCO_LINE_ID, //返回值是空的ERROR_RECORD.USER_RCO_LINE_ID,
                                ERR_MSG = ERROR_RECORD.ERR_MSG
                            };
                            db.WriteRcoOrder_RecList.InsertOnSubmit(reclist);

                            string MCRID = qsentList[i].GROUP_ID.ToString();


                            if (sentreco35[0].COLUMN_CHANGED == "PIECE")
                            {
                                strSQL =
                                    " Update M_Demand_DetailedList_Draft " +
                                    " set Material_State = case when (select Change_State from M_Change_Record where ID = '" +
                                    MCRID + "') = '2' then '7' else '2' end" +
                                    " where Convert(nvarchar(50),ID) = (select Correspond_Draft_Code from M_Demand_Merge_List where Convert(nvarchar(50),ID) = (select MDMLId from M_Change_Record where ID = '" +
                                    MCRID + "'))" +
                                    " Update M_Demand_Merge_List set NumCasesSum = '" + sentreco35[0].ORIGINAL_VALUE +
                                    "' where Convert(nvarchar(50),ID) = (select MDMLId from M_Change_Record where ID = '" + MCRID + "')";
                                DBI.Execute(strSQL);
                            }
                            else if (sentreco35[0].COLUMN_CHANGED == "QUANTITY")
                            {
                                strSQL =
                                     " Update M_Demand_DetailedList_Draft " +
                                    " set Material_State = case when (select Change_State from M_Change_Record where ID = '" +
                                    MCRID + "') = '2' then '7' else '2' end" +
                                    " where Convert(nvarchar(50),ID) = (select Correspond_Draft_Code from M_Demand_Merge_List where Convert(nvarchar(50),ID) = (select MDMLId from M_Change_Record where ID = '" +
                                    MCRID + "'))" +
                                    " Update M_Demand_Merge_List set DemandNumSum = '" + sentreco35[0].ORIGINAL_VALUE +
                                    "' where Convert(nvarchar(50),ID) = (select MDMLId from M_Change_Record where ID = '" + MCRID + "')";
                                DBI.Execute(strSQL);
                            }
                        }

                    }
                }
            }
            db.SubmitChanges();
        }

        #endregion


        protected class M_Draft_ListBody
        {
            public int ID { get; set; }
            public string Correspond_Draft_Code { get; set; }
            public string Drawing_No { get; set; }
            public int PackId { get; set; }
            public int TaskId { get; set; }
            public int DraftId { get; set; }
            public int MDMId { get; set; }
            public int MDPId { get; set; }
            public string TechnicsLine { get; set; }
            public string ItemCode1 { get; set; }
            public decimal DemandNumSum { get; set; }
            public decimal NumCasesSum { get; set; }
            public string Mat_Unit { get; set; }
            public int Quantity { get; set; }
            public string DemandDate { get; set; }
            public string Rough_Size { get; set; }
            public string Rough_Spec { get; set; }
            public string MaterialsDes { get; set; }
            public int Special_Needs { get; set; }
            public int Urgency_Degre { get; set; }
            public int Secret_Level { get; set; }
            public int Stage { get; set; }
            public int Use_Des { get; set; }
            public string Shipping_Address { get; set; }
            public int Certification { get; set; }
            public decimal Unit_Price { get; set; }
            public decimal Sum_Price { get; set; }
            public bool Is_Submit { get; set; }
            public bool Is_Save { get; set; }
            public int User_ID { get; set; }
            public DateTime Submit_Date { get; set; }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            Save_XHRW_Change_Record();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "CloseRadWindow();", true);
        }


        //protected void RadGrid_MDemandMergelist_ItemDataBound(object sender, GridItemEventArgs e)
        //{
        //    if (e.Item is GridDataItem)
        //    {
        //        DataTable table = GridSource;
        //        string Shipping_Addr_Id = table.Rows[e.Item.DataSetIndex]["Shipping_Addr_Id"].ToString();
        //        RadComboBox rcb = e.Item.FindControl("RadComboBoxShippingAddress") as RadComboBox;
        //        rcb.SelectedValue = Shipping_Addr_Id;
        //    }
        //}
    }
}