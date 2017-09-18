using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Camc.Web.Library;
using mms.k2;

namespace mms
{
    /// <summary>
    /// 领料WebService
    /// </summary>
    public class K2BLL
    {
        private static string DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
        private DBInterface DBI = DBFactory.GetDBInterface(DBConn);

        /// <summary>
        /// 向流程平台提交领料申请
        /// </summary>
        /// <param name="MAID"></param>
        /// <returns></returns>
        public string StartNewProcess(string MAID)
        {
            var db = new MMSDbDataContext();
            var ma = db.MaterialApplication.SingleOrDefault(p => p.Id == Convert.ToInt32(MAID)); //(from p in db.MaterialApplication where p.Id == Convert.ToInt32(MAID) select p).SingleOrDefault();
            if (ma == null) return "失败！" ;
            var mdml = db.M_Demand_Merge_List.SingleOrDefault(p => p.ID == ma.Material_Id); // (from p in db.M_Demand_Merge_List where p.ID == ma.Material_Id select p).SingleOrDefault();
            var RqHeaderId="";
            if (mdml != null) { RqHeaderId = mdml.MDPId.ToString(); }
            var sysdept = db.Sys_DeptEnum.SingleOrDefault(p => p.DeptCode == ma.Dept);
            var rqDept = "";
            if (sysdept != null) { rqDept = sysdept.Dept; }

            SubmitInfoModel model = new SubmitInfoModel();

            model.Applicant = ma.Applicant;
            if(ma.ApplicationTime != null)
            model.ApplicationTime = Convert.ToDateTime(ma.ApplicationTime);
            model.AppState = Convert.ToInt32( ma.AppState);
            model.CN_Material_State = ma.CN_Material_State;
            model.ContactInformation = ma.ContactInformation;
            model.Dept = ma.Dept;
            model.DiaoDuApproveAccount = "TJ\\" + ma.DiaoDuApprove;
            model.Draft_Code = ma.Draft_Code;
            model.TuiKuContext = ma.TuiKuContext;
            model.Drawing_No = ma.Drawing_No;
           
            model.FeedingTime = ma.FeedingTime;
            model.Is_Del = Convert.ToBoolean(ma.Is_Del);
            if(ma.IsConfirm != null)
            model.IsConfirm =  Convert.ToBoolean( ma.IsConfirm);
            if(ma.IsDispatch != null)
            model.IsDispatch = Convert.ToBoolean(ma.IsDispatch);
            model.ItemCode = ma.ItemCode;
            model.Mat_Rough_Weight = ma.Mat_Rough_Weight;
            model.Mat_Unit = ma.Mat_Unit;
            if(ma.Material_Id != null)
            model.Material_Id =  Convert.ToInt32(ma.Material_Id);
            model.Material_Mark = ma.Material_Mark;
            model.Material_Name = ma.Material_Name;
            model.Material_Tech_Condition = ma.Material_Tech_Condition;
            model.MaterialType = ma.MaterialType;
            model.MMS_ID = ma.Id;
            model.PleaseTakeQuality = ma.PleaseTakeQuality;
            model.ProcessInstID = 0;
            if(ma.Quantity != null)
            model.Quantity = Convert.ToInt32(ma.Quantity);
            model.Remark = ma.Remark;
            model.ReturnReason = ma.ReturnReason;
            model.Rough_Size = ma.Rough_Size;
            model.Rough_Spec = ma.Rough_Spec;
            model.RqDept = rqDept;                                          
            model.RqHeaderId = RqHeaderId;
            model.RqLineId = ma.Material_Id.ToString();
            model.TaskCode = ma.TaskCode;
            model.TheMaterialWay = ma.TheMaterialWay;
            if (ma.Type != null)
            model.Type = Convert.ToInt32(ma.Type);
            model.UserAccount = "TJ\\" + ma.UserAccount;
            model.WuZiJiHuaYuanApprove = "TJ\\" + ma.WuZiJiHuaYuanApprove;
            model.XingHaoJiHuaYuanApprove = "TJ\\" + ma.XingHaoJiHuaYuanApprove;

            K2WebServiceForMMS k2mms = new K2WebServiceForMMS();
            bool result ;
            try
            {
                result = k2mms.StartNewProcess(model);
            }
            catch (Exception ex)
            {
              //  ma.Is_Del = true;
                db.SubmitChanges();
                return "不能与流程平台通信，<br />请联系管理员";
            }
            if (result == true)
            {
                ma.AppState = 2;
                db.SubmitChanges();
                return "";
            }
            else
            {
                return "提交流程平台失败！<br />请联系管理员";
            }
        }

        /// <summary>
        /// 跟踪领料流程进度
        /// </summary>
        /// <param name="maId"></param>
        /// <param name="userDomainAccount"></param>
        /// <returns></returns>
        public ApproveInfoHead GetApproveHeader(string maId, string userDomainAccount)
        {
            SubmitInfoModel model = new SubmitInfoModel();
            model.MMS_ID = Convert.ToInt32(maId);
            model.UserAccount ="TJ\\" + userDomainAccount;
            K2WebServiceForMMS k2mms = new K2WebServiceForMMS();
            ApproveInfoHead head = k2mms.GetApproveHeader(model);

            return head;
        }

        /// <summary>
        /// 获取领料审批过程
        /// </summary>
        /// <param name="maId"></param>
        /// <param name="userDomainAccount"></param>
        /// <returns></returns>
        public ApproveInfoBody[] GetApproveBody(string maId, string userDomainAccount)
        {
            SubmitInfoModel model = new SubmitInfoModel();
            model.MMS_ID = Convert.ToInt32(maId);
            model.UserAccount = "TJ\\" + userDomainAccount;
            K2WebServiceForMMS k2mms = new K2WebServiceForMMS();
            ApproveInfoBody[] body = k2mms.GetApproveBody(model);

            return body;
        }
    }
}