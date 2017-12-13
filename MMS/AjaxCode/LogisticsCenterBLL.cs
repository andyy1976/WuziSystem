using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Camc.Web.Library;
using mms.WriteStockBillService;

namespace mms
{
    /// <summary>
    /// 物资需求信息WebService
    /// </summary>
    public class LogisticsCenterBLL
    {

        private static string DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
        private DBInterface DBI = DBFactory.GetDBInterface(DBConn);

        #region 提交物料需求清单WriteReqOrderRepeat
        /// <summary>
        /// 提交物料需求清单，成功返回空值，否则返回错误信息
        /// </summary>
        /// <param name="MDPLID"></param>
        /// <returns></returns>
        public string WriteReqOrderRepeat(string MDPLID)
        {
            //取数据库
            var db = new MMSDbDataContext();
            var client = new Write_Req_Order.CUX_DM_REQ_SYNC_WS_PKG_PortTypeClient();
            var header = new Write_Req_Order.SOAHeader
            {
                Responsibility = "CUX_SOA_ACCESS_RESP",
                RespApplication = "CUX",
                SecurityGroup = "STANDARD",
                NLSLanguage = "AMERICAN",
                Org_Id = "81"
            };

            client.ClientCredentials.UserName.UserName = "SOA_COMMON";
            client.ClientCredentials.UserName.Password = "111111";
            
            db.ExecuteCommand("Update M_Demand_Merge_List set Is_Submit = 'true' where MDPID = '" + MDPLID + "' Update M_Demand_Plan_List set Submit_State = '4' where Id = '" + MDPLID + "'");

            var submitcount = 200;

            var tempArray1 = (from p in db.WriteReqOrder_T_List
                              where p.USER_RQ_ID == Convert.ToDecimal(MDPLID)
                              orderby p.ID
                              select p).ToArray();

            var j = Math.Ceiling(Convert.ToDouble(tempArray1.Length) / submitcount);
            for (int n = 0; n < j; n++)
            {

                var Total_Flag = (n == (j - 1)) ? "Y" : "N";
                var Row_Count = (n == (j - 1)) ? (tempArray1.Length - (n * submitcount)) : submitcount;

                string strSQL = " Insert into WriteReqOrder_Sent (System_Code, Total_Flag, Row_Count,Group_ID) values ('TJ-WZ','" + Total_Flag + "','" + Row_Count + "', (select isnull(max(Group_ID),0) + 1 from WriteReqOrder_Sent)) select @@identity";
                var sentId = DBI.GetSingleValue(strSQL);

                var qSent = (from p in db.WriteReqOrder_Sent where p.ID.ToString() == sentId
                             orderby p.ID descending
                             select p).Take(1).ToList();

                var sentSum23 = new Write_Req_Order.APPSCUX_DM_REQ_SYNC_WS_PKG_SUM_23
                {
                    SYSTEM_CODE = qSent[0].SYSTEM_CODE,
                    TOTAL_FLAG = qSent[0].TOTAL_FLAG,
                    ROW_COUNT = qSent[0].ROW_COUNT,
                    ROW_COUNTSpecified = true,
                    GROUP_ID = qSent[0].GROUP_ID,
                    GROUP_IDSpecified = true
                };

                var sentList26 = new Write_Req_Order.APPSCUX_DM_REQ_SYNC_WS_PKG_RECO26[((n == (j - 1)) ? (tempArray1.Length - (n * submitcount)) : submitcount)];

                for (var i = n * submitcount; i < (n + 1) * submitcount && i < tempArray1.Length; i++)
                {
                    var tlist =
                            db.WriteReqOrder_T_List.SingleOrDefault(
                                p => p.ID == tempArray1[i].ID);
                    tlist.GROUP_ID = qSent[0].GROUP_ID;

                    sentList26[i - n * submitcount] = new Write_Req_Order.APPSCUX_DM_REQ_SYNC_WS_PKG_RECO26
                    {
                        ORG_ID = tempArray1[i].ORG_ID,
                        ORG_IDSpecified = true,
                        RQ_NUMBER = "", //留空
                        USER_RQ_NUMBER = tempArray1[i].USER_RQ_NUMBER,
                        USER_RQ_ID = tempArray1[i].USER_RQ_ID,
                        USER_RQ_IDSpecified = true,
                        USER_RQ_LINE_ID = tempArray1[i].USER_RQ_LINE_ID,
                        USER_RQ_LINE_IDSpecified = true,
                        RQ_TYPE = tempArray1[i].RQ_TYPE,
                        ITEM_CODE = tempArray1[i].ITEM_CODE,
                        SPECIAL_REQUEST = tempArray1[i].SPECIAL_REQUEST,
                        QUANTITY = tempArray1[i].QUANTITY,
                        QUANTITYSpecified = tempArray1[i].QUANTITY != null,
                        PIECE = tempArray1[i].PIECE,
                        PIECESpecified = tempArray1[i].PIECE != null,
                        DIMENSION = tempArray1[i].DIMENSION,
                        MANUFACTURER = tempArray1[i].MANUFACTURER ?? "", //字段待确认
                        RQ_DATE = tempArray1[i].RQ_DATE,
                        RQ_DATESpecified = true,
                        URGENCY_LEVEL = tempArray1[i].URGENCY_LEVEL,
                        REQUESTER = tempArray1[i].REQUESTER,
                        REQUESTER_PHONE = tempArray1[i].REQUESTER_PHONE ?? "",
                        ITEM_REVISION = tempArray1[i].ITEM_REVISION,
                        USER_ITEM_DESCRIPTION = tempArray1[i].USER_ITEM_DESCRIPTION ?? "",
                        UNANIMOUS_BATCH = tempArray1[i].UNANIMOUS_BATCH ?? "",
                        SECURITY_LEVEL = tempArray1[i].SECURITY_LEVEL ?? "",
                        PROJECT = tempArray1[i].PROJECT,
                        PHASE = tempArray1[i].PHASE ?? "",
                        BATCH = tempArray1[i].BATCH ?? "",
                        BATCH_QTY = tempArray1[i].BATCH_QTY ?? 1, //字段待确认
                        BATCH_QTYSpecified = true,
                        USAGE = tempArray1[i].USAGE ?? "",
                        TASK = tempArray1[i].TASK ?? "",
                        SUBJECT = tempArray1[i].SUBJECT ?? "",
                        CUSTOMER_NAME = tempArray1[i].CUSTOMER_NAME ?? "",
                        CUSTOMER_ACCOUNT_NUMBER = tempArray1[i].CUSTOMER_ACCOUNT_NUMBER ?? "",
                        DELIVERY_ADDRESS = tempArray1[i].DELIVERY_ADDRESS ?? "",
                        PREPARER = tempArray1[i].PREPARER,
                        SUBMITED_BY = tempArray1[i].SUBMITED_BY,
                        SUBMITED_BYSpecified = tempArray1[i].SUBMITED_BY != null,
                        SUBMISSION_DATE = tempArray1[i].SUBMISSION_DATE,
                        SUBMISSION_DATESpecified = true,
                        SYSTEM_CODE = tempArray1[i].SYSTEM_CODE,
                        TRANSFER_TIME = tempArray1[i].TRANSFER_TIME,
                        TRANSFER_TIMESpecified = tempArray1[i].TRANSFER_TIME != null,
                        CUSTOMER_OF_CREATOR = tempArray1[i].CUSTOMER_OF_CREATOR ?? "",
                        GROUP_ID = qSent[0].GROUP_ID,
                        GROUP_IDSpecified = true,
                        CREATION_DATE = tempArray1[i].CREATION_DATE,
                        CREATION_DATESpecified = tempArray1[i].CREATION_DATE != null,
                        CREATED_BY = tempArray1[i].CREATED_BY,
                        CREATED_BYSpecified = tempArray1[i].CREATED_BY != null,
                        ATTRIBUTE_CATEGORY = tempArray1[i].ATTRIBUTE_CATEGORY ?? "",
                        ATTRIBUTE1 = tempArray1[i].ATTRIBUTE1 ?? "",
                        ATTRIBUTE2 = tempArray1[i].ATTRIBUTE2 ?? "",
                        ATTRIBUTE3 = tempArray1[i].ATTRIBUTE3 ?? "",
                        ATTRIBUTE4 = tempArray1[i].ATTRIBUTE4 ?? "",
                        ATTRIBUTE5 = tempArray1[i].ATTRIBUTE5 ?? "",
                        ATTRIBUTE6 = tempArray1[i].ATTRIBUTE6 ?? "",
                        ATTRIBUTE7 = tempArray1[i].ATTRIBUTE7 ?? "",
                        ATTRIBUTE8 = tempArray1[i].ATTRIBUTE8 ?? "",
                        ATTRIBUTE9 = tempArray1[i].ATTRIBUTE9 ?? "",
                        ATTRIBUTE10 = tempArray1[i].ATTRIBUTE10 ?? "",
                        ATTRIBUTE11 = tempArray1[i].ATTRIBUTE11 ?? "",
                        ATTRIBUTE12 = tempArray1[i].ATTRIBUTE12 ?? "",
                        ATTRIBUTE13 = tempArray1[i].ATTRIBUTE13 ?? "",
                        ATTRIBUTE14 = tempArray1[i].ATTRIBUTE14 ?? "",
                        ATTRIBUTE15 = tempArray1[i].ATTRIBUTE15 ?? ""
                    };
                }
                db.SubmitChanges();

                var input = new Write_Req_Order.InputParameters
                {
                    P_SUM_RECORD_INPUT = sentSum23,
                    P_RECORD_INPUT = sentList26
                };
                Write_Req_Order.OutputParameters returnSyncData = new Write_Req_Order.OutputParameters();
                
                try
                {
                    returnSyncData = client.WRITE_REQ_ORDER(header, input);
                }
                catch
                {
                    db.ExecuteCommand("Update M_Demand_Merge_List set Is_Submit = 'false' where MDPID = '" + MDPLID + "' Update M_Demand_Plan_List set Submit_State = case when Submit_Type = '0' then '0' else'2' end where Id = '" + MDPLID + "'");
                    return "不能与物流中心通信，<br />请联系管理员。";
                }
                    //提交物流中心不成功记录错误信息
                if (returnSyncData.X_RECORD_RESULT.STATUS == "E")
                {
                    var rec = new WriteReqOrder_Rec()
                    {
                        STATUS = returnSyncData.X_RECORD_RESULT.STATUS,
                        ERR_MSG = returnSyncData.X_RECORD_RESULT.ERR_MSG,
                        GROUP_ID = qSent[0].GROUP_ID.ToString()
                    };
                    db.WriteReqOrder_Rec.InsertOnSubmit(rec);
                    db.SubmitChanges();
                    var recid = rec.ID;

                    strSQL = "Update M_Demand_Merge_List  set Is_submit = 'false' where ID in ( select USER_RQ_LINE_ID from WriteReqOrder_T_List where GROUP_ID = '" + qSent[0].GROUP_ID.ToString() + "')";
                    db.ExecuteCommand(strSQL);

                    //记录错误信息
                    foreach (var list in returnSyncData.X_ERROR_RECORD)
                    {
                        var reclist = new WriteReqOrder_RecList()
                        {
                            REC_SUM_ID = recid,
                            ORG_ID = list.ORG_ID,
                            SYSTEM_CODE = list.SYSTEM_CODE,
                            USER_RQ_LINE_ID = list.USER_RQ_LINE_ID,
                            USER_RQ_ID = list.USER_RQ_ID,
                            ERR_MSG = list.ERR_MSG
                        };
                        db.WriteReqOrder_RecList.InsertOnSubmit(reclist);
                        //修改提交失败的需求信息的提交状态
                        
                    }
                    db.SubmitChanges();
                }
            }
            return "";
        }

        #endregion

        #region 提交变更单WriteRcoOrderRepeat
        /// <summary>
        /// 提交变更单，成功返回空值，否则返回错误信息
        /// </summary>
        /// <param name="MDPLID"></param>
        /// <returns></returns>
        public string WriteRcoOrderRepeat(string MDPLID)
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
                        " Insert into WriteRcoOrder_Sent values ('TJ-WZ','Y','2',(select isnull(max(Group_ID),0) + 1 from WriteRcoOrder_Sent)) select @@identity";
                    var sentid = DBI.GetSingleValue(strSQL);

                    var qSent = (from p in db.WriteRcoOrder_Sent where p.ID.ToString() == sentid
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

                    Write_Rco_Order.OutputParameters returnData = new Write_Rco_Order.OutputParameters();
                    try
                    {
                        returnData = client.WRITE_RCO_ORDER(header, input);
                    }
                    catch
                    {
                        var mcr =
                            (from p in db.M_Change_Record where p.MDPId.ToString() == MDPLID select p).ToList();
                        foreach (var item in mcr)
                        {
                            var mdmlid = item.MDMId;
                            var columnChanged = item.Column_Changed;
                            var originalValue = item.Original_Value;
                       
                            var mdml = db.M_Demand_Merge_List.SingleOrDefault(p => p.ID == mdmlid);
                            switch (columnChanged)
                            {
                                case "USER_RQ_NUMBER"://用户需求编号
                                    mdml.USER_RQ_NUMBER = originalValue;
                                    break;
                                case "SPECIAL_REQUEST"://特殊需求
                                    mdml.Special_Needs = originalValue;
                                    mdml.SPECIAL_REQUEST = originalValue;
                                    break;
                                case "QUANTITY"://需求数量
                                    mdml.DemandNumSum = Convert.ToDecimal(originalValue);
                                    mdml.QUANTITY1 = Convert.ToDecimal(originalValue);
                                    break;
                                case "PIECE"://件数
                                    mdml.NumCasesSum = Convert.ToDecimal(originalValue);
                                    mdml.PIECE = Convert.ToDecimal(originalValue);
                                    break;
                                case "DIMENSION"://尺寸
                                    mdml.Rough_Size = originalValue;
                                    mdml.DIMENSION = originalValue;
                                    break;
                                case "MANUFACTURER_ID"://指定生产厂家
                                    mdml.MANUFACTURER = originalValue;
                                    break;
                                case "RQ_DATE"://需求时间
                                    mdml.DemandDate = Convert.ToDateTime(originalValue);
                                    mdml.RQ_DATE = Convert.ToDateTime(originalValue);
                                    break;
                                case "URGENCY_LEVEL"://紧急程度
                                    mdml.Urgency_Degre = originalValue;
                                    mdml.URGENCY_LEVEL = originalValue;
                                    break;
                                case "REQUESTER"://申请人
                                    mdml.REQUESTER = originalValue;
                                    break;
                                case "REQUESTER_PHONE"://申请人联系电话
                                    mdml.REQUESTER_PHONE = originalValue;
                                    break;
                                case "USER_ITEM_DESCRIPTION"://无编码物资说明
                                    mdml.USER_ITEM_DESCRIPTION = originalValue;
                                    break;
                                case "UNANIMOUS_BATCH"://同批次
                                    mdml.UNANIMOUS_BATCH = originalValue;
                                    break;
                                case "SECURITY_LEVEL"://密级
                                    mdml.SECURITY_LEVEL = originalValue;
                                    mdml.Secret_Level = originalValue;
                                    break;
                                case "PROJECT"://型号工程
                                    mdml.PROJECT = originalValue;
                                    break;
                                case "PHASE"://研制阶段
                                    mdml.PHASE = originalValue;
                                    mdml.Stage = originalValue;
                                    break;
                                case "BATCH"://批组号
                                    mdml.BATCH = originalValue;
                                    break;
                                case "BATCH_QTY"://当量/发数
                                    mdml.BATCH_QTY = Convert.ToDecimal(originalValue);
                                    break;
                                case "USAGE"://用途
                                    mdml.Use_Des = originalValue;
                                    mdml.USAGE = originalValue;
                                    break;
                                case "TASK"://任务号
                                    mdml.TaskCode = originalValue;
                                    mdml.TASK = originalValue;
                                    break;
                                case "SUBJECT"://课题号
                                    mdml.SUBJECT = originalValue;
                                    break;
                                case "CUSTOMER_ACCOUNT_ID"://需求部门
                                    mdml.CUSTOMER_ACCOUNT_NUMBER = originalValue;
                                    break;
                                case "DELIVERY_ADDRESS"://配送地址
                                    mdml.DELIVERY_ADDRESS = originalValue;
                                    mdml.Shipping_Address = originalValue;
                                    break;
                                case "CUSTOMER_ID"://需求单位
                                    mdml.CUSTOMER_NAME = originalValue;
                                    break;
                                case "ATTRIBUTE2"://指定采购部门
                                    mdml.ATTRIBUTE2 = originalValue;
                                    break;
                                case "ATTRIBUTE3"://开具合格证
                                    mdml.ATTRIBUTE3 = originalValue;
                                    mdml.Certification = originalValue;
                                    break;
                            }
                            db.SubmitChanges();
                        }

                        return "不能与物流中心通信，<br />请联系管理员。";
                    }

                    if (returnData.X_RECORD_RESULT.STATUS == "E")
                    {
                        WriteRcoOrder_Rec rec = new WriteRcoOrder_Rec()
                        {
                            STATUS = returnData.X_RECORD_RESULT.STATUS,
                            ERR_MSG = returnData.X_RECORD_RESULT.ERR_MSG
                        };
                        db.WriteRcoOrder_Rec.InsertOnSubmit(rec);
                        db.SubmitChanges();
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

                            var mcr = db.M_Change_Record.SingleOrDefault(p=> p.Id == item.USER_RCO_LINE_ID);// (from p in db.M_Change_Record where p.Id == item.USER_RCO_LINE_ID select p)
                            var mdmlId = mcr.MDMId;
                            var columnChanged = mcr.Column_Changed;
                            var originalValue = mcr.Original_Value;

                            var mdml = db.M_Demand_Merge_List.SingleOrDefault(p => p.ID == mdmlId);
                            switch (columnChanged)
                            {
                                case "USER_RQ_NUMBER"://用户需求编号
                                    mdml.USER_RQ_NUMBER = originalValue;
                                    break;
                                case "SPECIAL_REQUEST"://特殊需求
                                    mdml.Special_Needs = originalValue;
                                    mdml.SPECIAL_REQUEST = originalValue;
                                    break;
                                case "QUANTITY"://需求数量
                                    mdml.DemandNumSum = Convert.ToDecimal(originalValue);
                                    mdml.QUANTITY1 = Convert.ToDecimal(originalValue);
                                    break;
                                case "PIECE"://件数
                                    mdml.NumCasesSum = Convert.ToDecimal(originalValue);
                                    mdml.PIECE = Convert.ToDecimal(originalValue);
                                    break;
                                case "DIMENSION"://尺寸
                                    mdml.Rough_Size = originalValue;//需求尺寸
                                    mdml.DIMENSION = originalValue;
                                    break;
                                case "MANUFACTURER_ID"://指定生产厂家
                                    mdml.MANUFACTURER = originalValue;
                                    break;
                                case "RQ_DATE"://需求时间
                                    mdml.DemandDate = Convert.ToDateTime(originalValue);
                                    mdml.RQ_DATE = Convert.ToDateTime(originalValue);
                                    break;
                                case "URGENCY_LEVEL"://紧急程度
                                    mdml.Urgency_Degre = originalValue;
                                    mdml.URGENCY_LEVEL = originalValue;
                                    break;
                                case "REQUESTER"://申请人
                                    mdml.REQUESTER = originalValue;
                                    break;
                                case "REQUESTER_PHONE"://申请人联系电话
                                    mdml.REQUESTER_PHONE = originalValue;
                                    break;
                                case "USER_ITEM_DESCRIPTION"://无编码物资说明
                                    mdml.USER_ITEM_DESCRIPTION = originalValue;
                                    break;
                                case "UNANIMOUS_BATCH"://同批次
                                    mdml.UNANIMOUS_BATCH = originalValue;
                                    break;
                                case "SECURITY_LEVEL"://密级
                                    mdml.SECURITY_LEVEL = originalValue;
                                    mdml.Secret_Level = originalValue;
                                    break;
                                case "PROJECT"://型号工程
                                    mdml.PROJECT = originalValue;
                                    break;
                                case "PHASE"://研制阶段
                                    mdml.PHASE = originalValue;
                                    mdml.Stage = originalValue;
                                    break;
                                case "BATCH"://批组号
                                    mdml.BATCH = originalValue;
                                    break;
                                case "BATCH_QTY"://当量/发数
                                    mdml.BATCH_QTY = Convert.ToDecimal(originalValue);
                                    break;
                                case "USAGE"://用途
                                    mdml.Use_Des = originalValue;
                                    mdml.USAGE = originalValue;
                                    break;
                                case "TASK"://任务号
                                    mdml.TaskCode = originalValue;
                                    mdml.TASK = originalValue;
                                    break;
                                case "SUBJECT"://课题号
                                    mdml.SUBJECT = originalValue;
                                    break;
                                case "CUSTOMER_ACCOUNT_ID"://需求部门
                                    mdml.CUSTOMER_ACCOUNT_NUMBER = originalValue;
                                    break;
                                case "DELIVERY_ADDRESS"://配送地址
                                    mdml.DELIVERY_ADDRESS = originalValue;
                                    mdml.Shipping_Address = originalValue;
                                    break;
                                case "CUSTOMER_ID"://需求单位
                                    mdml.CUSTOMER_NAME = originalValue;
                                    break;
                                case "ATTRIBUTE2"://指定采购部门
                                    mdml.ATTRIBUTE2 = originalValue;
                                    break;
                                case "ATTRIBUTE3"://开具合格证
                                    mdml.ATTRIBUTE3 = originalValue;
                                    mdml.Certification = originalValue;
                                    break;
                            }
                            db.SubmitChanges();
                        }
                    }
                }
            }
            db.SubmitChanges();

            return "";
        }

        #endregion
        
        #region 获取错误信息 GetErrInfRepeat
        public void GetErrInfRepeat(string systemCode, string maxUpdateDate, decimal lastMaxRowNum)
        {
            var client =
                new Get_Err_Inf.CUX_DM_ERR_RELEASE_WS_PKG_PortTypeClient();
            var header = new Get_Err_Inf.SOAHeader
            {
                Responsibility = "CUX_SOA_ACCESS_RESP",
                RespApplication = "CUX",
                SecurityGroup = "STANDARD",
                NLSLanguage = "AMERICAN",
                Org_Id = "81"
            };
            client.ClientCredentials.UserName.UserName = "SOA_COMMON";
            client.ClientCredentials.UserName.Password = "111111";

            var reco63 = new Get_Err_Inf.APPSCUX_DM_ERR_RELEASE_WS_PKG_RE2
            {
                SYSTEM_CODE = systemCode,
                MAX_UPDATE_DATE = maxUpdateDate,
                LAST_MAX_ROWNUMBER = lastMaxRowNum,
                LAST_MAX_ROWNUMBERSpecified = true
            };

            var inputParameters = new Get_Err_Inf.InputParameters
            {
                P_RECORD_INPUT = reco63
            };

            var returnData = client.GET_ERR_INF(header, inputParameters);
            var dcUpdate = new MMSDbDataContext();
            var maxRowNumberQuery = (from p in dcUpdate.GetErrInf_Rec
                                     where p.MAX_ROWNUMBER != null
                                     orderby p.MAX_ROWNUMBER descending
                                     select new
                                     {
                                         lastRowNo = p.MAX_ROWNUMBER
                                     }).ToList();
            //将数据插入Rec表中
            var custRecord = returnData.X_RECORD_SUM;
            var j = new GetErrInf_Rec
            {
                STATUS = custRecord.STATUS,
                TOTAL_FLAG = custRecord.TOTAL_FLAG,
                ROW_ACCOUT = custRecord.ROW_COUNT,
                MAX_ROWNUMBER = custRecord.MAX_ROWNUMBER ?? maxRowNumberQuery[0].lastRowNo,
                ERR_MSG = custRecord.ERR_MSG,
                ATTRIBUTE1 = custRecord.ATTRIBUTE1,
                ATTRIBUTE2 = custRecord.ATTRIBUTE2,
                ATTRIBUTE3 = custRecord.ATTRIBUTE3,
                ATTRIBUTE4 = custRecord.ATTRIBUTE4,
                ATTRIBUTE5 = custRecord.ATTRIBUTE5,
                ATTRIBUTE6 = custRecord.ATTRIBUTE6,
                ATTRIBUTE7 = custRecord.ATTRIBUTE7,
                ATTRIBUTE8 = custRecord.ATTRIBUTE8,
                ATTRIBUTE9 = custRecord.ATTRIBUTE9,
                ATTRIBUTE10 = custRecord.ATTRIBUTE10
            };
            dcUpdate.GetErrInf_Rec.InsertOnSubmit(j);
            try
            {
                dcUpdate.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            //将数据插入Table表中
            var queryRecRowNum = (from p in dcUpdate.GetErrInf_Rec
                                  orderby p.ID descending
                                  select p).ToList();
            var tempRowNum = queryRecRowNum[0].ID;

            foreach (var custListItem in returnData.X_TABLE_ERR_INF)
            {
                var i = new GetErrInf_T_Item
                {
                    REC_ID = tempRowNum,
                    DATA_CLASS = custListItem.DATA_CLASS,
                    SOURCE_HEADER_ID = custListItem.SOURCE_HEADER_ID,
                    SOURCE_LINE_ID = custListItem.SOURCE_LINE_ID,
                    SOURCE_HEADER_NUMBER = custListItem.SOURCE_HEADER_NUMBER,
                    ERROR_MSG = custListItem.ERROR_MSG,
                    SYSTEM_CODE = custListItem.SYSTEM_CODE,
                    LAST_UPDATE_DATE = custListItem.LAST_UPDATE_DATE,
                    ATTRIBUTE_CATEGORY = custListItem.ATTRIBUTE_CATEGORY,
                    ATTRIBUTE1 = custListItem.ATTRIBUTE1,
                    ATTRIBUTE2 = custListItem.ATTRIBUTE2,
                    ATTRIBUTE3 = custListItem.ATTRIBUTE3,
                    ATTRIBUTE4 = custListItem.ATTRIBUTE4,
                    ATTRIBUTE5 = custListItem.ATTRIBUTE5,
                    ATTRIBUTE6 = custListItem.ATTRIBUTE6,
                    ATTRIBUTE7 = custListItem.ATTRIBUTE7,
                    ATTRIBUTE8 = custListItem.ATTRIBUTE8,
                    ATTRIBUTE9 = custListItem.ATTRIBUTE9,
                    ATTRIBUTE10 = custListItem.ATTRIBUTE10,
                    ATTRIBUTE11 = custListItem.ATTRIBUTE11,
                    ATTRIBUTE12 = custListItem.ATTRIBUTE12,
                    ATTRIBUTE13 = custListItem.ATTRIBUTE13,
                    ATTRIBUTE14 = custListItem.ATTRIBUTE14,
                    ATTRIBUTE15 = custListItem.ATTRIBUTE15
                };
                var q = (from p in dcUpdate.GetErrInf_T_Item
                         where p.SOURCE_HEADER_ID == i.SOURCE_HEADER_ID && p.SOURCE_LINE_ID == i.SOURCE_LINE_ID
                         orderby p.ID
                         select p).Take(1).ToList();
                if (q.Count == 0)
                {
                    dcUpdate.GetErrInf_T_Item.InsertOnSubmit(i);
                }
                else
                {
                    //使用ExeInfoRepo类更新T_Items表
                    var errrepo = new ErrInfoRepo();
                    var errinf = errrepo.GetErrInf((decimal)custListItem.SOURCE_HEADER_ID,
                        (decimal)custListItem.SOURCE_LINE_ID);
                    errinf.DATA_CLASS = custListItem.DATA_CLASS;
                    errinf.SOURCE_HEADER_ID = custListItem.SOURCE_HEADER_ID;
                    errinf.SOURCE_LINE_ID = custListItem.SOURCE_LINE_ID;
                    errinf.SOURCE_HEADER_NUMBER = custListItem.SOURCE_HEADER_NUMBER;
                    errinf.ERROR_MSG = custListItem.ERROR_MSG;
                    errinf.SYSTEM_CODE = custListItem.SYSTEM_CODE;
                    errinf.LAST_UPDATE_DATE = custListItem.LAST_UPDATE_DATE;
                    errinf.ATTRIBUTE_CATEGORY = custListItem.ATTRIBUTE_CATEGORY;
                    errinf.ATTRIBUTE1 = custListItem.ATTRIBUTE1;
                    errinf.ATTRIBUTE2 = custListItem.ATTRIBUTE2;
                    errinf.ATTRIBUTE3 = custListItem.ATTRIBUTE3;
                    errinf.ATTRIBUTE4 = custListItem.ATTRIBUTE4;
                    errinf.ATTRIBUTE5 = custListItem.ATTRIBUTE5;
                    errinf.ATTRIBUTE6 = custListItem.ATTRIBUTE6;
                    errinf.ATTRIBUTE7 = custListItem.ATTRIBUTE7;
                    errinf.ATTRIBUTE8 = custListItem.ATTRIBUTE8;
                    errinf.ATTRIBUTE9 = custListItem.ATTRIBUTE9;
                    errinf.ATTRIBUTE10 = custListItem.ATTRIBUTE10;
                    errinf.ATTRIBUTE11 = custListItem.ATTRIBUTE11;
                    errinf.ATTRIBUTE12 = custListItem.ATTRIBUTE12;
                    errinf.ATTRIBUTE13 = custListItem.ATTRIBUTE13;
                    errinf.ATTRIBUTE14 = custListItem.ATTRIBUTE14;
                    errinf.ATTRIBUTE15 = custListItem.ATTRIBUTE15;

                    errrepo.UpdateErrInfT(errinf);
                }
                try
                {
                    dcUpdate.SubmitChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        #endregion

        #region 反馈签收信息 WriteStockBill
        /// <summary>
        /// 签收出库单，返回是数字为签收失败数，否则为错误信息
        /// </summary>
        /// <param name="sentID"></param>
        /// <returns></returns>
        public string WriteStockBill(int sentID)
        {
            int ecount = 0;
            var db = new MMSDbDataContext();

            WriteStockBillServicePortTypeClient client = new WriteStockBillServicePortTypeClient();

            var sent = (from p in db.stockbill_Sent where p.ID == sentID select p).Take(1).ToList();
            var item = (from p in db.stockbill_T_Item where p.stockbill_SentID == sentID.ToString() select p).ToList();

            WriteStockBillParamBillInfo[] signbilllist = new WriteStockBillParamBillInfo[item.Count];

            for (int i = 0; i < item.Count; i++)
            {
                signbilllist[i] = new WriteStockBillParamBillInfo
                {
                    cgeneralbid = item[i].cgeneralbid,
                    jc_jstype = item[i].jc_jstype,
                    lastupdate = Convert.ToDateTime(item[i].lastupdate).ToString("yyyy-MM-dd HH:mm:ss"),
                    signuser = item[i].signuser,
                    userSysBillBid = item[i].userSysBillBid.ToString(),
                    userSysBillHid = item[i].userSysBillHid.ToString(),
                    vdefdou1 = item[i].vdefdou1,
                    vdefdou10 = item[i].vdefdou10,
                    vdefdou10Specified = true,
                    vdefdou1Specified = true,
                    vdefdou2 = item[i].vdefdou2,
                    vdefdou2Specified = true,
                    vdefdou3 = item[i].vdefdou3,
                    vdefdou3Specified = true,
                    vdefdou4 = item[i].vdefdou4,
                    vdefdou4Specified = true,
                    vdefdou5 = item[i].vdefdou5,
                    vdefdou5Specified = true,
                    vdefdou6 = item[i].vdefdou6,
                    vdefdou6Specified = true,
                    vdefdou7 = item[i].vdefdou7,
                    vdefdou7Specified = true,
                    vdefdou8 = item[i].vdefdou8,
                    vdefdou8Specified = true,
                    vdefdou9 = item[i].vdefdou9,
                    vdefdou9Specified = true,
                    vdefstr1 = item[i].vdefstr1,
                    vdefstr10 = item[i].vdefstr10,
                    vdefstr2 = item[i].vdefstr2,
                    vdefstr3 = item[i].vdefstr3,
                    vdefstr4 = item[i].vdefstr4,
                    vdefstr5 = item[i].vdefstr5,
                    vdefstr6 = item[i].vdefstr6,
                    vdefstr7 = item[i].vdefstr7,
                    vdefstr8 = item[i].vdefstr8,
                    vdefstr9 = item[i].vdefstr9
                };
            }

            WriteStockBillParam billParam = new WriteStockBillParam
            {
                customerSysCode = sent[0].customerSysCode,// "TJ_WZ",
                customerSyspwd = sent[0].customerSyspwd, // "TJ_WZ",
                customerSysIp = sent[0].customerSysIp,// "10.20.232.48",
                customerSysPort = sent[0].customerSysPort,// "80",
                row_Count = item.Count,// signbilllist.Length,
                row_CountSpecified = true,
                writeStockBillRequestBillInfo = signbilllist,
                vdefDouble1 = sent[0].vdefDouble1,
                vdefDouble1Specified = true,
                vdefDouble2 = sent[0].vdefDouble2,
                vdefDouble2Specified = true,
                vdefDouble3 = sent[0].vdefDouble3,
                vdefDouble3Specified = true,
                vdefInteger1 = sent[0].vdefInteger1,
                vdefInteger1Specified = true,
                vdefInteger2 = sent[0].vdefInteger2,
                vdefInteger2Specified = true,
                vdefInteger3 = sent[0].vdefInteger3,
                vdefInteger3Specified = true,
                vdefString1 = sent[0].vdefString1,
                vdefString2 = sent[0].vdefString2,
                vdefString3 = sent[0].vdefString3
            };
            WriteStockBillResponse[] writeStockBillResponses;

            try
            {
                writeStockBillResponses = client.writeStockBillInfo(billParam);
            }
            catch
            {
                db.ExecuteCommand("Update stockbill_T_Item set State = '0' where stockbill_SentID = '" + sentID + "'");
            
                return "不能与物流中心通信，<br />请联系管理员！";
            }

            for (int i = 0; i < writeStockBillResponses.Length; i++)
            {
                WriteStockBillResponse s = writeStockBillResponses[i];
                stockbill_Rec rec = new stockbill_Rec()
                {
                    cgeneralBid = s.cgeneralBid,
                    errorInfo = s.errorInfo,
                    lastUpdate = Convert.ToDateTime(s.lastUpdate),
                    returnResult = s.returnResult,

                    userSysBillBid = s.userSysBillBid,
                    userSysBillHid = s.userSysBillHid,
                    vdefDouble1 = s.vdefDouble1,
                    vdefDouble10 = s.vdefDouble10,
                    vdefDouble2 = s.vdefDouble2,
                    vdefDouble3 = s.vdefDouble3,
                    vdefDouble4 = s.vdefDouble4,
                    vdefDouble5 = s.vdefDouble5,
                    vdefDouble6 = s.vdefDouble6,
                    vdefDouble7 = s.vdefDouble7,
                    vdefDouble8 = s.vdefDouble8,
                    vdefDouble9 = s.vdefDouble9,
                    vdefString1 = s.vdefString1,
                    vdefString10 = s.vdefString10,
                    vdefString2 = s.vdefString2,
                    vdefString3 = s.vdefString3,
                    vdefString4 = s.vdefString4,
                    vdefString5 = s.vdefString5,
                    vdefString6 = s.vdefString6,
                    vdefString7 = s.vdefString7,
                    vdefString8 = s.vdefString8,
                    vdefString9 = s.vdefString9
                };

                db.stockbill_Rec.InsertOnSubmit(rec);
                if (s.returnResult == "E")
                {
                    stockbill_T_Item updateitem = db.stockbill_T_Item.SingleOrDefault(up => up.ID == Convert.ToInt32(s.userSysBillBid));
                    updateitem.State = 0;

                    ecount++;
                }
                db.SubmitChanges();
            }
            return ecount.ToString();
        }

        #endregion
    }

    public class ErrInfoRepo
    {
        public GetErrInf_T_Item GetErrInf(decimal id1, decimal id2)
        {
            var db = new MMSDbDataContext();
            db.ObjectTrackingEnabled = false;
            return
                db.GetErrInf_T_Item.Where(p => p.SOURCE_HEADER_ID == id1)
                    .SingleOrDefault(p => p.SOURCE_LINE_ID == id2);
        }

        public void UpdateErrInfT(GetErrInf_T_Item errinf)
        {
            var db = new MMSDbDataContext();
            db.GetErrInf_T_Item.Attach(errinf);
            db.SubmitChanges();
        }
    }
}