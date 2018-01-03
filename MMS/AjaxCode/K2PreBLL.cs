using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Camc.Web.Library;
using mms;
using mms.k2Pre;

namespace mms
{
    /// <summary>
    /// 备料WebService
    /// </summary>
    public class K2PreBLL
    {
        private static string DBConn = ConfigurationManager.ConnectionStrings["MaterialManagerSystemConnectionString"].ToString();
        private DBInterface DBI = DBFactory.GetDBInterface(DBConn);

        /// <summary>
        /// 物资需求传入流程平台
        /// </summary>
        /// <param name="MDPLID"></param>
        public string k2StartPreparesProgress(string MDPLID)
        {
            string strSql = "select M_Demand_Plan_List.*, DomainAccount, UserName from M_Demand_Plan_List" +
                " join Sys_UserInfo_PWD on Sys_UserInfo_PWD.ID = M_Demand_Plan_List.User_ID where M_Demand_Plan_List.ID = '" + MDPLID + "'";
            DataTable dtmdpl = DBI.Execute(strSql, true);
            strSql = " select b.DICT_Name as UseDes, M_Demand_Merge_List.*, Dept, a.DICT_Name as Urgency" +
                  ",case when Stage ='1' then 'M' when stage='2' then 'C' when Stage='3' then 'S' when Stage='4' then 'D' else Convert(nvarchar(50),Stage) end as Stage1 " +

                " from M_Demand_Merge_List" +
                " left join Sys_DeptEnum on Sys_DeptEnum.DeptCode = M_Demand_Merge_List.MaterialDept" +
                " left join GetBasicdata_T_Item as a on M_Demand_Merge_List.Urgency_Degre = a.DICT_Code and a.DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                " left join GetBasicdata_T_Item as b on M_Demand_Merge_List.Use_Des = b.DICT_Code and b.DICT_CLASS='CUX_DM_USAGE'" +
                " where MDPID='" + MDPLID + "'";
            DataTable dtmdml = DBI.Execute(strSql, true);
        
            PreparesMarerialsBodyModel[] listbodymodel = new PreparesMarerialsBodyModel[dtmdml.Rows.Count];
            PreparesMarerialsHeadModel headmodel = new PreparesMarerialsHeadModel()
            {
                RequestType = 1,
                DeptApproveAccount = "TJ\\" + dtmdpl.Rows[0]["DeptApproveAccount"].ToString(),                                             //车间
                PlanApproveAccount = "TJ\\" + dtmdpl.Rows[0]["PlanOrTecApproveAccount"].ToString(),                    //工艺处（工艺处、生产处有一个就可以）
                ProcessInstID = 0,                                                                                               //流程ID
                RequestID = Convert.ToInt32(MDPLID),
                SubmitDate = Convert.ToDateTime(dtmdpl.Rows[0]["Submit_Date"].ToString()).ToString("yyyy-MM-dd"),
                TecApproveAccount = dtmdpl.Rows[0]["Submit_Type"].ToString() == "3" ? "TJ\\" + dtmdpl.Rows[0]["MaterialPlanApproveAccount"].ToString() : "",    //物资综合计划员
                UserAccount = "TJ\\" + dtmdpl.Rows[0]["DomainAccount"].ToString(),
                UserName = dtmdpl.Rows[0]["UserName"].ToString(),
                AppState = dtmdpl.Rows[0]["Submit_Type"].ToString()
                //Is_Del = dtmdpl.Rows[0][""].ToString()
            };
            for (int i = 0; i < dtmdml.Rows.Count; i++)
            {
                PreparesMarerialsBodyModel bodymodel = new PreparesMarerialsBodyModel()
                {
                    Certification = dtmdml.Rows[i]["Certification"].ToString(),
                    Country = dtmdml.Rows[i]["ATTRIBUTE4"].ToString(),                                                         ////国产进口
                    DemandDate = dtmdml.Rows[i]["DemandDate"].ToString(),
                    DemandNumSum = dtmdml.Rows[i]["DemandNumSum"].ToString(),
                    DrawingNum = dtmdml.Rows[i]["Drawing_No"].ToString(),
                    ItemCode = dtmdml.Rows[i]["ItemCode1"].ToString(),
                    Material_Name = dtmdml.Rows[i]["Material_Name"].ToString(),
                    MaterialDept = dtmdml.Rows[i]["Dept"].ToString(),
                    MatUnit = dtmdml.Rows[i]["Mat_Unit"].ToString(),
                    ProcessInstID = 0,                                                           //流程ID
                    Quantity = dtmdml.Rows[i]["NumCasesSum"].ToString(),
                    RoughSize = dtmdml.Rows[i]["Rough_Size"].ToString(),
                //    DingeSize = dtmdml.Rows[i]["Dinge_Size"].ToString(),
                    RoughSpec = dtmdml.Rows[i]["Rough_Spec"].ToString(),
                    SecretLevel = dtmdml.Rows[i]["Secret_Level"].ToString(),
                    SpecialNeeds = dtmdml.Rows[i]["Special_Needs"].ToString(),
                    Stage = dtmdml.Rows[i]["Stage1"].ToString(),
                    SubjectNum = dtmdml.Rows[i]["SUBJECT"].ToString(),
                    TaskNum = dtmdml.Rows[i]["TaskCode"].ToString(),
                    Urgency = dtmdml.Rows[i]["Urgency"].ToString(),
                    UseDes = dtmdml.Rows[i]["UseDes"].ToString()
                };
                listbodymodel[i] = bodymodel;
            }

            K2WebServiceForMMSPre mms = new K2WebServiceForMMSPre();
            bool result;
            try
            {
                result = mms.StartPreparesProgress(headmodel, listbodymodel);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("未能解析此远程名称") == -1)
                {
                    return "提交流程平台错误，请联系流程平台管理员";
                }
                else
                {
                    return "不能与流程平台通信，<br />请联系管理员";
                }
            }
            if (result == true)
            {
                strSql = " Update M_Demand_Plan_List set Submit_State='1' where ID ='" + MDPLID + "'";
                DBI.Execute(strSql);
                return "";
            }
            else
            {
                return "提交流程平台失败！<br />请联系管理员";
            }
        }

        /// <summary>
        /// 物资需求传入流程平台
        /// </summary>
        /// <param name="MDPLID"></param>
        public string k2StartPreparesProgressChange(string MDPLID, string MDMLID)
        {
            string strSql = "select M_Demand_Plan_List.*, DomainAccount, UserName from M_Demand_Plan_List" +
                " join Sys_UserInfo_PWD on Sys_UserInfo_PWD.ID = M_Demand_Plan_List.User_ID where M_Demand_Plan_List.ID = '" + MDPLID + "'";
            DataTable dtmdpl = DBI.Execute(strSql, true);
            strSql = " select b.DICT_Name as UseDes, M_Demand_Merge_List.*, Dept, a.DICT_Name as Urgency" +
                      ", case when Stage ='1' then 'M' when stage='2' then 'C' when Stage='3' then 'S' when Stage='4' then 'D' else Convert(nvarchar(50),Stage) end as Stage1  " +
                " from M_Demand_Merge_List" +
                " left join Sys_DeptEnum on Sys_DeptEnum.DeptCode = M_Demand_Merge_List.MaterialDept" +
                " left join GetBasicdata_T_Item as a on M_Demand_Merge_List.Urgency_Degre = a.DICT_Code and a.DICT_CLASS='CUX_DM_URGENCY_LEVEL'" +
                " left join GetBasicdata_T_Item as b on M_Demand_Merge_List.Use_Des = b.DICT_Code and b.DICT_CLASS='CUX_DM_USAGE'" +
                " where MDPID='" + MDPLID + "'and M_Demand_Merge_List.ID='" + MDMLID + "'";
            DataTable dtmdml = DBI.Execute(strSql, true);

            PreparesMarerialsBodyModel[] listbodymodel = new PreparesMarerialsBodyModel[dtmdml.Rows.Count];
            PreparesMarerialsHeadModel headmodel = new PreparesMarerialsHeadModel()
            {
                RequestType=0,
                DeptApproveAccount = "TJ\\" + dtmdpl.Rows[0]["DeptApproveAccountChange"].ToString(),                                             //车间
                PlanApproveAccount = "TJ\\" + dtmdpl.Rows[0]["PlanOrTecApproveAccountChange"].ToString(),                    //工艺处（工艺处、生产处有一个就可以）
                ProcessInstID = 0,                                                                                               //流程ID
                RequestID = Convert.ToInt32(MDPLID),
                SubmitDate = Convert.ToDateTime(dtmdpl.Rows[0]["Submit_Date"].ToString()).ToString("yyyy-MM-dd"),
                TecApproveAccount = dtmdpl.Rows[0]["Submit_Type"].ToString() == "3" ? "TJ\\" + dtmdpl.Rows[0]["MaterialPlanApproveAccountChange"].ToString() : "",    //物资综合计划员
                UserAccount = "TJ\\" + dtmdpl.Rows[0]["DomainAccount"].ToString(),
                UserName = dtmdpl.Rows[0]["UserName"].ToString(),
                AppState = dtmdpl.Rows[0]["Submit_Type"].ToString()
                //Is_Del = dtmdpl.Rows[0][""].ToString()
            };
            for (int i = 0; i < dtmdml.Rows.Count; i++)
            {
                PreparesMarerialsBodyModel bodymodel = new PreparesMarerialsBodyModel()
                {
                    Certification = dtmdml.Rows[i]["Certification"].ToString(),
                    Country = dtmdml.Rows[i]["ATTRIBUTE4"].ToString(),                                                         ////国产进口
                    DemandDate = dtmdml.Rows[i]["DemandDate"].ToString(),
                    DemandNumSum = dtmdml.Rows[i]["DemandNumSum"].ToString(),
                    DrawingNum = dtmdml.Rows[i]["Drawing_No"].ToString(),
                    ItemCode = dtmdml.Rows[i]["ItemCode1"].ToString(),
                    Material_Name = dtmdml.Rows[i]["Material_Name"].ToString(),
                    MaterialDept = dtmdml.Rows[i]["Dept"].ToString(),
                    MatUnit = dtmdml.Rows[i]["Mat_Unit"].ToString(),
                    ProcessInstID = 0,                                                           //流程ID
                    Quantity = dtmdml.Rows[i]["NumCasesSum"].ToString(),
                    RoughSize = dtmdml.Rows[i]["Rough_Size"].ToString(),
                 //   DingeSize = dtmdml.Rows[i]["Dinge_Size"].ToString(),
                    RoughSpec = dtmdml.Rows[i]["Rough_Spec"].ToString(),
                    SecretLevel = dtmdml.Rows[i]["Secret_Level"].ToString(),
                    SpecialNeeds = dtmdml.Rows[i]["Special_Needs"].ToString(),
                    Stage = dtmdml.Rows[i]["Stage1"].ToString(),
                    SubjectNum = dtmdml.Rows[i]["SUBJECT"].ToString(),
                    TaskNum = dtmdml.Rows[i]["TaskCode"].ToString(),
                    Urgency = dtmdml.Rows[i]["Urgency"].ToString(),
                    UseDes = dtmdml.Rows[i]["UseDes"].ToString()
                };
                listbodymodel[i] = bodymodel;
            }

            K2WebServiceForMMSPre mms = new K2WebServiceForMMSPre();
            bool result;
            try
            {
                result = mms.StartPreparesProgress(headmodel, listbodymodel);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("未能解析此远程名称") == -1)
                {
                    return "提交流程平台错误，请联系流程平台管理员";
                }
                else
                {
                    return "不能与流程平台通信，<br />请联系管理员";
                }
            }
            if (result == true)
            {
                strSql = " Update M_Demand_Plan_List set Submit_State='1' where ID ='" + MDPLID + "'";
                DBI.Execute(strSql);
                return "";
            }
            else
            {
                return "提交流程平台失败！<br />请联系管理员";
            }
        }
        /// <summary>
        /// 删除流程
        /// </summary>
        /// <param name="submitInfo">只需要传入MMS_ID和UserAccount</param>
        /// <param name="onlyProcess">传true只删除流程，传false删除流程和数据 </param>
        public void K2DeleteProcessInst(string MDPLID, string userDomainAccount, bool onlyProcess)
        {
            PreparesMarerialsHeadModel headmodel = new PreparesMarerialsHeadModel()
            {
                RequestID = Convert.ToInt32(MDPLID),
                UserAccount = "TJ\\" + userDomainAccount
            };
            K2WebServiceForMMSPre mmspre = new K2WebServiceForMMSPre();
            mmspre.DeleteProcessInst(headmodel, onlyProcess);
        }

        /// <summary>
        /// 获取审批结果 0:已提交，1：已结束已通过，2：已结束未通过
        /// </summary>
        /// <param name="MDPLID"></param>
        /// <returns> </returns>
        public ApproveInfoHead K2PreGetApproveHeader(string MDPLID, string userDomainAccount)
        {
            PreparesMarerialsHeadModel headmodel = new PreparesMarerialsHeadModel()
            {
                RequestID = Convert.ToInt32(MDPLID),
                UserAccount = userDomainAccount
            };
            K2WebServiceForMMSPre mmspre = new K2WebServiceForMMSPre();
            return mmspre.GetApproveHeader(headmodel);
        }

        /// <summary>
        /// 获取审批流程
        /// </summary>
        /// <param name="MDPLID"></param>
        public ApproveInfoBody[] k2PreGetApproveBody(string MDPLID, string userDomainAccount)
        {
            PreparesMarerialsHeadModel headmodel = new PreparesMarerialsHeadModel()
            {
                RequestID = Convert.ToInt32(MDPLID),
                UserAccount = userDomainAccount
            };
            K2WebServiceForMMSPre mmspre = new K2WebServiceForMMSPre();
            var result =mmspre.GetApproveBody(headmodel);
            return result;
        }
    }
}