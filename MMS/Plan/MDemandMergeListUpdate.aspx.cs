using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Camc.Web.Library;
using System.Configuration;
using mms;
using Telerik.Web.UI;
using System.Drawing;

namespace mms.Plan
{
    public partial class MDemandMergeListUpdate : System.Web.UI.Page
    {
        string DBContractConn;
        DBInterface DBI;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) { Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true); }
            DBContractConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ConnectionString.ToString();
            DBI = DBFactory.GetDBInterface(DBContractConn);

            if (!IsPostBack)
            {
                string MDMLID = Request.QueryString["MDMLID"].ToString();
                string strSQL = " select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'";
                DataTable dt = DBI.Execute(strSQL, true);
                RDDL_Urgency_Degre.DataSource = dt;
                RDDL_Urgency_Degre.DataTextField = "DICT_NAME";
                RDDL_Urgency_Degre.DataValueField = "DICT_CODE";
                RDDL_Urgency_Degre.DataBind();

                strSQL = "SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)";
                dt = DBI.Execute(strSQL, true);
                RDDL_Secret_Level.DataSource = dt;
                RDDL_Secret_Level.DataTextField = "SecretLevel_Name";
                RDDL_Secret_Level.DataValueField = "SecretLevel_Name";
                RDDL_Secret_Level.DataBind();

                strSQL = "select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE'";
                dt = DBI.Execute(strSQL, true);
                RDDL_Use_Des.DataSource = dt;
                RDDL_Use_Des.DataTextField = "DICT_NAME";
                RDDL_Use_Des.DataValueField = "DICT_CODE";
                RDDL_Use_Des.DataBind();

                GetMDML(MDMLID);
                GetManufactur("");
            }
        }

        protected void GetMDML(string id)
        {
            string strSQL = " select *, (select Dept from Sys_DeptEnum where DeptCode = M_Demand_Merge_List.MaterialDept) as Dept  from M_Demand_Merge_List where ID = '" + id + "'";
            DataTable dt = DBI.Execute(strSQL, true);
             
            lbl_ID.Text = id;
            hf_MDPLID.Value = dt.Rows[0]["MDPID"].ToString();
            lbl_TaskCode.Text = dt.Rows[0]["TaskCode"].ToString();
            lbl_DrawingNo.Text = dt.Rows[0]["Drawing_No"].ToString();
            lbl_Material_Name.Text = dt.Rows[0]["Material_Name"].ToString();
            lbl_ItemCode1.Text = dt.Rows[0]["ItemCode1"].ToString();
            lbl_MaterialDept.Text = dt.Rows[0]["Dept"].ToString();
            lbl_Rough_Spec.Text = dt.Rows[0]["Rough_Spec"].ToString();
            lbl_Dinge_Size.Text = dt.Rows[0]["Dinge_Size"].ToString();
            RTB_ROUGH_SIZE.Text = dt.Rows[0]["Rough_Size"].ToString();
            lbl_Mat_Unit.Text = dt.Rows[0]["Mat_Unit"].ToString();
            RTB_Mat_Rough_Weight.Text = dt.Rows[0]["Mat_Rough_Weight"].ToString();
            RTB_Special_Needs.Text = dt.Rows[0]["Special_Needs"].ToString();
            lbl_NumCasesSum.Text = Convert.ToDouble(dt.Rows[0]["NumCasesSum"].ToString()).ToString();
            lbl_DemandNumSum.Text = Convert.ToDouble(dt.Rows[0]["DemandNumSum"].ToString()).ToString();
            if (RDDL_Urgency_Degre.FindItemByValue(dt.Rows[0]["Urgency_Degre"].ToString()) != null)
            {
                RDDL_Urgency_Degre.FindItemByValue(dt.Rows[0]["Urgency_Degre"].ToString()).Selected = true;
            }
            if (RDDL_Secret_Level.FindItemByValue(dt.Rows[0]["Secret_Level"].ToString()) != null)
            {
                RDDL_Secret_Level.FindItemByValue(dt.Rows[0]["Secret_Level"].ToString()).Selected = true;
            }
            if (RDDL_Use_Des.FindItemByValue(dt.Rows[0]["Use_Des"].ToString()) != null)
            {
                RDDL_Use_Des.FindItemByValue(dt.Rows[0]["Use_Des"].ToString()).Selected = true;
            }
            if (RDDL_Certification.FindItemByText(dt.Rows[0]["Certification"].ToString()) != null)
            {
                RDDL_Certification.FindItemByText(dt.Rows[0]["Certification"].ToString()).Selected = true;
            }
            RDP_DemandDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["DemandDate"].ToString());
            RTB_MANUFACTURER.Text = dt.Rows[0]["MANUFACTURER"].ToString();

            string DeptCode = dt.Rows[0]["MaterialDept"].ToString();
            strSQL = " select '' as KeyWord , '' as keyWordCode union select KeyWord , keyWordCode from Sys_Dict where TypeID = '2' " +
                        " and  '2-' + Convert(nvarchar(50), KeyWordCode) in (select Shipping_Addr_ID from Sys_Dept_ShipAddr where Dept_Id= (select ID from Sys_DeptEnum where DeptCode='" + DeptCode + "')) order by KeyWord";
            DataTable dtaddr = DBI.Execute(strSQL, true);
            RDDL_Shipping_Address.DataSource = dtaddr;
            RDDL_Shipping_Address.DataTextField = "KeyWord";
            RDDL_Shipping_Address.DataValueField = "KeyWordCode";
            RDDL_Shipping_Address.DataBind();
            if (RDDL_Shipping_Address.FindItemByText(dt.Rows[0]["Shipping_Address"].ToString()) != null)
            {
                RDDL_Shipping_Address.FindItemByText(dt.Rows[0]["Shipping_Address"].ToString()).Selected = true;
            }
        }

        protected void RB_Update_Click(object sender, EventArgs e)
        {
            string strSQL = " select *, (select Dept from Sys_DeptEnum where DeptCode = M_Demand_Merge_List.MaterialDept) as Dept  from M_Demand_Merge_List where ID = '" + lbl_ID.Text + "'";
            DataTable dt = DBI.Execute(strSQL, true);

            if(RDP_DemandDate.SelectedDate == null)
            {
                RadNotificationAlert.Text = "请选择需求时间！";
                RadNotificationAlert.Show();
                return;
            }
            try
            {
                Convert.ToDateTime(RDP_DemandDate.SelectedDate);
            }
            catch {
                RadNotificationAlert.Text = "请正确填写需求时间！";
                RadNotificationAlert.Show();
                return;
            }
            if (RTB_Reason.Text == "")
            {
                RadNotificationAlert.Text = "请输入变更原因！";
                RadNotificationAlert.Show();
                return;
            }

            if (RTB_Special_Needs.Text.Trim() != dt.Rows[0]["Special_Needs"].ToString() || RDDL_Urgency_Degre.SelectedValue.ToString() != dt.Rows[0]["Urgency_Degre"].ToString() 
                || RDDL_Secret_Level.SelectedText.ToString() != dt.Rows[0]["Secret_Level"].ToString() || RDDL_Use_Des.SelectedValue.ToString() != dt.Rows[0]["Use_Des"].ToString()
                || RDDL_Shipping_Address.SelectedText.ToString() != dt.Rows[0]["Shipping_Address"].ToString() || RDDL_Certification.SelectedValue.ToString() != dt.Rows[0]["Certification"].ToString()
                || Convert.ToDateTime(RDP_DemandDate.SelectedDate).ToString("yyyy-MM-dd") != Convert.ToDateTime(dt.Rows[0]["DemandDate"].ToString()).ToString("yyyy-MM-dd")
                ||Convert.ToDecimal(lbl_NumCasesSum.Text.Trim()) != Convert.ToDecimal(dt.Rows[0]["NumCasesSum"].ToString()) || Convert.ToDecimal(lbl_DemandNumSum.Text.Trim()) != Convert.ToDecimal(dt.Rows[0]["DemandNumSum"].ToString())
                || lbl_Mat_Unit.Text.Trim() != dt.Rows[0]["Mat_Unit"].ToString() || RTB_ROUGH_SIZE.Text.Trim() != dt.Rows[0]["Rough_Size"].ToString()
                || RTB_MANUFACTURER.Text.Trim() != dt.Rows[0]["MANUFACTURER"].ToString() || RTB_Mat_Rough_Weight.Text.Trim() != dt.Rows[0]["Mat_Rough_Weight"].ToString())
            {
                string ID = lbl_ID.Text;
                string REQUESTER = dt.Rows[0]["REQUESTER"].ToString();
                string REQUESTER_PHONE = dt.Rows[0]["REQUESTER_PHONE"].ToString();
                string Drawing_No = dt.Rows[0]["Drawing_No"].ToString();
                string TaskCode = dt.Rows[0]["TaskCode"].ToString();
                string DemandDate = Convert.ToDateTime(RDP_DemandDate.SelectedDate).ToString("yyyy-MM-dd");
                string Material_Name = dt.Rows[0]["Material_Name"].ToString();
              
                string Dinge_Size = dt.Rows[0]["Dinge_Size"].ToString();
                string Rough_Spec = dt.Rows[0]["Rough_Spec"].ToString();
                string Unit_Price = dt.Rows[0]["Unit_Price"].ToString();
               
                string Sum_Price = dt.Rows[0]["Sum_Price"].ToString();
                string Secret_Level = RDDL_Secret_Level.SelectedText.ToString();
                string Stage = dt.Rows[0]["Stage"].ToString();
                string Use_Des = RDDL_Use_Des.SelectedValue.ToString();
                string Certification = RDDL_Certification.SelectedValue.ToString();
                string Special_Needs = RTB_Special_Needs.Text.Trim();
                string Urgency_Degre = RDDL_Urgency_Degre.SelectedValue.ToString();
                string NumCasesSum = lbl_NumCasesSum.Text.Trim();
               
                string Rough_Size = RTB_ROUGH_SIZE.Text.Trim();
                string DemandNumSum = lbl_DemandNumSum.Text.Trim();
                int confirmedQuantiy = GetConfirmedQuantity(ID);
                if (Convert.ToDecimal(DemandNumSum) < confirmedQuantiy)
                {
                    RadNotificationAlert.Text = "需求数量不能小于已发货数量！" + confirmedQuantiy.ToString();
                    RadNotificationAlert.Show(); return;
                }
                string Mat_Unit = lbl_Mat_Unit.Text.Trim();

                string Mat_Rough_Weight = RTB_Mat_Rough_Weight.Text.Trim();
                string MaterialDept = dt.Rows[0]["MaterialDept"].ToString();
                string Shipping_Address = RDDL_Shipping_Address.SelectedText.ToString();
                string MANUFACTURER = RTB_MANUFACTURER.Text.Trim();
                string SUBJECT = dt.Rows[0]["SUBJECT"].ToString();
                string UserID = null;

                string Project = dt.Rows[0]["Project"].ToString(); ;
                string TDM_Description = dt.Rows[0]["TDM_Description"].ToString(); ;
                string Material_Mark = dt.Rows[0]["Material_Mark"].ToString(); ;
                string CN_Material_State = dt.Rows[0]["CN_Material_State"].ToString(); ;
                string Material_Tech_Condition = dt.Rows[0]["Material_Tech_Condition"].ToString(); ;
                string MaterialsDes = dt.Rows[0]["MaterialsDes"].ToString(); ;

                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "info", "CloseWindow();", true);
                }
                string Reason = RTB_Reason.Text.Trim();

                if (Special_Needs != dt.Rows[0]["Special_Needs"].ToString() && Special_Needs == "")
                {
                    RadNotificationAlert.Text = "修改后的特殊需求不能为空";
                    RadNotificationAlert.Show();
                    return;
                }
                if (Shipping_Address != dt.Rows[0]["Shipping_Address"].ToString() && Shipping_Address == "")
                {
                    RadNotificationAlert.Text = "修改后的配送地址不能为空";
                    RadNotificationAlert.Show();
                    return;
                }

                //strSQL = " select SEG2 from GetCommManu_T_Item where SEG5 = '" + MANUFACTURER + "'";
                //MANUFACTURER = DBI.GetSingleValue(strSQL);

                strSQL = @"exec Proc_UpdateMDemandMergeList '" + ID + "','" +
                         REQUESTER + "','" +
                         REQUESTER_PHONE + "','" +
                         Drawing_No + "','" +
                         TaskCode + "','" +
                         DemandDate + "','" +
                         Material_Name + "','" +
                         Project + "','" +
                         TDM_Description + "','" +
                         Material_Mark + "','" +
                         CN_Material_State + "','" +
                         Material_Tech_Condition + "','" +
                         MaterialsDes + "','" +
                         NumCasesSum + "','" +
                         DemandNumSum + "','" +
                         Mat_Unit + "','" +
                         Rough_Size + "','" +
                         Dinge_Size + "','" +
                         Rough_Spec + "','" +
                         Unit_Price + "','" +
                         Mat_Rough_Weight + "','" +
                         Sum_Price + "','" +
                         Secret_Level + "','" +
                         Stage + "','" +
                         Use_Des + "','" +
                         Certification + "','" +
                         Special_Needs + "','" +
                         Urgency_Degre + "','" +
                         MaterialDept + "','" +
                         Shipping_Address + "','" +
                         MANUFACTURER + "','" +
                         SUBJECT + "','" +
                         UserID + "','" +
                         Reason.Replace(",", "，") + "'";
                DataTable dtmd = DBI.Execute(strSQL, true);
                string MDPLID = dtmd.Rows[0][0].ToString();
                string MDPCode = dtmd.Rows[0][1].ToString();

              

                LogisticsCenterBLL bll = new LogisticsCenterBLL();
                var result = bll.WriteRcoOrderRepeat(MDPLID);
                if (result == "")
                {
                    RadNotificationAlert1.Text = "保存成功！";
                    RadNotificationAlert1.Show();
                }
                else
                {
                    RadNotificationAlert.Text = "修改失败！<br />" + result;
                    RadNotificationAlert.Show();
                }

            }
            else
            {
                RadNotificationAlert.Text = "修改后的数据没变化，不再提交物流中心";
                RadNotificationAlert.Show();
            }
        }

        protected int GetConfirmedQuantity(string MDMLID)
        {
            string strSQL = " select CONFIRMED_QUANTITY  from GetRqStatus_T_Item where SUBMISSION_STATUS='物流已确认' and USER_RQ_LINE_ID = '" + MDMLID + "'";
            DataTable dt = DBI.Execute(strSQL, true);
            int confirmedQuantity = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                confirmedQuantity += Convert.ToInt32(dt.Rows[i]["CONFIRMED_QUANTITY"].ToString());
            }
            return confirmedQuantity;
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
                               where p.MDPLID == MDPLID
                               select p).ToArray();

            for (int n = 0; n < qSentHeader.Length; n++)
            {
                var qsentList = (from p in db.WriteRcoOrder_SentLine
                                 where p.USER_RCO_HEADER_ID == qSentHeader[n].USER_RCO_HEADER_ID
                                 orderby p.ID
                                 select p).ToArray();

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
                    GROUP_ID = qSentHeader[n].GROUP_ID,
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

                    sentreco34[0].GROUP_ID = qSent[0].GROUP_ID;

                    var input = new Write_Rco_Order.InputParameters
                    {
                        P_SUM_RECORD_INPUT = sentSum29,
                        P_HEADER_INPUT = sentreco34,
                        P_LINE_INPUT = sentreco35
                    };

                    var returnData = client.WRITE_RCO_ORDER(header, input);

                    if (returnData.X_RECORD_RESULT.STATUS == "E")
                    {
                        WriteRcoOrder_Rec rec = new WriteRcoOrder_Rec()
                        {
                            STATUS = returnData.X_RECORD_RESULT.STATUS,
                            ERR_MSG = returnData.X_RECORD_RESULT.ERR_MSG
                        };
                        db.WriteRcoOrder_Rec.InsertOnSubmit(rec);
                        var recId = rec.ID;
                        foreach (var item in returnData.X_ERROR_RECORD)
                        {
                            WriteRcoOrder_RecList reclist = new WriteRcoOrder_RecList()
                            {
                                REC_SUM_ID = recId,
                                ORG_ID = item.ORG_ID,
                                SYSTEM_CODE = item.SYSTEM_CODE,
                                USER_RCO_HEADER_ID = item.USER_RCO_HEADER_ID,
                                USER_RCO_LINE_ID = item.USER_RCO_LINE_ID,
                                ERR_MSG = item.ERR_MSG
                            };
                            db.WriteRcoOrder_RecList.InsertOnSubmit(reclist);
                        }
                        db.SubmitChanges();



                        //foreach (var ERROR_RECORD in returnData.X_ERROR_RECORD)
                        //{
                        //    WriteRcoOrder_RecList reclist = new WriteRcoOrder_RecList() { 
                        //        REC_SUM_ID = 0, //不对的
                        //        ORG_ID = ERROR_RECORD.ORG_ID,
                        //        SYSTEM_CODE= ERROR_RECORD.SYSTEM_CODE,
                        //        USER_RCO_HEADER_ID = ERROR_RECORD.USER_RCO_HEADER_ID,
                        //        USER_RCO_LINE_ID = sentreco35[0].USER_RCO_LINE_ID, //返回值是空的ERROR_RECORD.USER_RCO_LINE_ID,
                        //        ERR_MSG = ERROR_RECORD.ERR_MSG
                        //    };
                        //    db.WriteRcoOrder_RecList.InsertOnSubmit(reclist);

                        //    string MCRID = qsentList[i].GROUP_ID.ToString();


                        //    if (sentreco35[0].COLUMN_CHANGED == "PIECE")
                        //    {
                        //        strSQL =
                        //            " Update M_Demand_DetailedList_Draft " +
                        //            " set Material_State = case when (select Change_State from M_Change_Record where ID = '" +
                        //            MCRID + "') = '2' then '7' else '2' end" +
                        //            " where Convert(nvarchar(50),ID) = (select Correspond_Draft_Code from M_Demand_Merge_List where Convert(nvarchar(50),ID) = (select MDMLId from M_Change_Record where ID = '" +
                        //            MCRID + "'))" +
                        //            " Update M_Demand_Merge_List set NumCasesSum = '" + sentreco35[0].ORIGINAL_VALUE +
                        //            "' where Convert(nvarchar(50),ID) = (select MDMLId from M_Change_Record where ID = '" + MCRID + "')";
                        //        DBI.Execute(strSQL);
                        //    }
                        //    else if (sentreco35[0].COLUMN_CHANGED == "QUANTITY")
                        //    {
                        //        strSQL =
                        //             " Update M_Demand_DetailedList_Draft " +
                        //            " set Material_State = case when (select Change_State from M_Change_Record where ID = '" +
                        //            MCRID + "') = '2' then '7' else '2' end" +
                        //            " where Convert(nvarchar(50),ID) = (select Correspond_Draft_Code from M_Demand_Merge_List where Convert(nvarchar(50),ID) = (select MDMLId from M_Change_Record where ID = '" +
                        //            MCRID + "'))" +
                        //            " Update M_Demand_Merge_List set DemandNumSum = '" + sentreco35[0].ORIGINAL_VALUE +
                        //            "' where Convert(nvarchar(50),ID) = (select MDMLId from M_Change_Record where ID = '" + MCRID + "')";
                        //        DBI.Execute(strSQL);
                        //    }
                        //}

                    }
                }
            }
            db.SubmitChanges();
        }

        #endregion

        protected void RadGrid_MANUFACTURER_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            RadGrid_MANUFACTURER.DataSource = this.ViewState["GridMANUFACTURER"];
        }

        protected void GetManufactur(string strWhere)
        {
            string strSQL = " select seg4, seg5 from GetCommManu_T_Item where 1= 1"+ strWhere + " order by Seg4";
            DataTable dt = DBI.Execute(strSQL, true);
            this.ViewState["GridMANUFACTURER"] = dt;
            
        }

        protected void RB_SM_Click(object sender, EventArgs e)
        {
            string seg4 = RTB_Seg4.Text.Trim();
            string seg5 = RTB_Seg5.Text.Trim();

            string strWhere = " and Seg4 like '%" + seg4 + "%' and Seg5 like '%" + seg5 + "%'";
            GetManufactur(strWhere);
            RadGrid_MANUFACTURER.Rebind();
        }
    }
}