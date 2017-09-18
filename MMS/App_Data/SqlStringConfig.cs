using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductBarCodeManagementAndTrack
{
    public class SqlStringConfig
    {

        /// <summary>
        /// 获取指定ID的产品计划
        /// </summary>
        /// <param name="OBJECT_ID"></param>
        /// <returns></returns>
        public static string SQLPLANBYID(string OBJECT_ID)
        {
            string SQLPLANBYID = @"select * from dbo.PLANRELEASE where OBJECT_ID = '" + OBJECT_ID + "' and DATAAREAID = '211'";
            return SQLPLANBYID;
        }
        
        /// <summary>
        /// 获取产品计划列表
        /// </summary>
        /// <returns></returns>
        public static string SQLPLANLIST(string where)
        {
            string SQLPLANLIST;
            SQLPLANLIST = @"select OBJECT_ID,A.RECID,CAST(CN_YEAR as int) as CN_YEAR,PRODUCTDESCRIPTION,
                                          case when ORDERTYPE='0' then '正式计划'
                                               else '非正式计划'
                                               end as ORDERTYPE,
                                         CONVERT(VARCHAR(12),CREATION_DATE,111) as CREATION_DATE,CREATERNAME,
                                          case when HASREADFLAG='0' then '未处理' 
                                               when HASREADFLAG='1' then '已处理' 
                                               end as HASREADFLAG,
                                          case when B.RECEIVER='0' then '未签收' 
                                               when B.RECEIVER<>'0' then '已签收' 
                                               end as SignStatus,B.RECEIVER  
                                          from PLANRELEASE as A  left join PLANRECEIVE as B on A.OBJECT_ID = B.OBJECT_ID1 "+where;
            return SQLPLANLIST;
        }

        /// <summary>
        /// 获取指定ID产品计划签收部门列表
        /// </summary>
        /// <param name="OBJECT_ID1"></param>
        /// <returns></returns>
        public static string SQLPLANRECEIVEBYID(string OBJECT_ID1)
        {
            string SQLPLANRECEIVEBYID = @"select * from dbo.PLANRECEIVE where OBJECT_ID1 = '" + OBJECT_ID1 + "'  and DATAAREAID = '211'";
            return SQLPLANRECEIVEBYID;
        }

        /// <summary>
        ///获取指定ID产品计划生产明细
        /// </summary>
        /// <param name="OBJECT_ID2"></param>
        /// <returns></returns>
        public static string SQLRELEASEDPLANSBYID(string OBJECT_ID2)
        {
            string SQLRELEASEDPLANSBYID = @"select PLANTASK_NO,DRAWING_NO,PART_NAME,DESIGNER_QUANTITY,
                                                   TECHNICS_QUANTITY,PLAN_QUANTITY,HAND_QUANTITY,UNITID,
                                                   TECHNICS_LINE,convert(varchar(12),PLAN_DATE,111) as PLAN_DATE,
                                                   case when URGENT='0' then '否'
                                                        else '是' 
                                                        end as URGENT,
                                                   case DEPTNAME when '051' then '51车间' when '053' then '53车间' when '055' then '55车间' when '056' then '56车间' when '057' then '57车间' when '058' then '58车间' end as DEPTNAME,
                                                   case when PLANTASK_TYPE='1' then '新增'
                                                        when PLANTASK_TYPE='2' then '修改'
                                                        when PLANTASK_TYPE='3' then '删除'
                                                        end as PLANTASK_TYPE,
                                                   PLANTASK_MEMO 
                                                   from dbo.RELEASEDPLANS where OBJECT_ID2 =  '" + OBJECT_ID2 + "'  and DATAAREAID = '211' ";
            return SQLRELEASEDPLANSBYID;
        }


        /// <summary>
        /// 查询生产计划明细
        /// </summary>
        /// <param name="OBJECT_ID2"></param>
        /// <returns></returns>
        public static string SQLGETRELEASEDPLAN(string OBJECT_ID2,string deptcode) 
        {

            string SQLGETRELEASEDPLAN = @"select *
                                                   from dbo.RELEASEDPLANS where OBJECT_ID2 =  '" + OBJECT_ID2 + "'  and DATAAREAID = '211' and DEPTNAME = '" + deptcode + "'";
            return SQLGETRELEASEDPLAN;
        
        }

        /// <summary>
        /// 获取产品计划处理类型
        /// </summary>
        /// <param name="HASREADFLAG"></param>
        /// <returns></returns>
        public static string SQLGETHASREADFLAG(string DataField,string DataTable) 
        {
            string SQLGETHASREADFLAG = @"select  case when HASREADFLAG=0 then '未处理'
                                                   when HASREADFLAG=1 then '已处理' 
                                                   end as HASREADFLAG from PLANRELEASE group by HASREADFLAG";   
            return SQLGETHASREADFLAG;      
        }

        /// <summary>
        /// 获取月度计划列表
        /// </summary>
        /// <returns></returns>
        public static string SQLMONTHLYTASKRELEASE(string where) 
        {
            string SQLMONTHLYTASKRELEASE;
            SQLMONTHLYTASKRELEASE = @"select OBJECT_ID,A.RECID,MAINDEPTCODE,CN_YEAR,CN_MONTH,
                                                CONVERT(VARCHAR(12),CREATION_DATE,111) AS CREATION_DATE,CREATERNAME,
                                                CASE WHEN HASREADFLAG='0' THEN '未处理'
                                                    WHEN HASREADFLAG='1' THEN '已处理' 
                                                    END AS HASREADFLAG,
                                                case when B.RECEIVE_DATE='1900/1/1 0:00:00' THEN '未签收'
                                                     else '已签收' end as SignStatus
                                                from dbo.TASKRELEASE as A left join TASKRECEIVE as B on A.OBJECT_ID=B.OBJECT_ID1 "
                                            +where;
            return SQLMONTHLYTASKRELEASE;
        }

        /// <summary>
        /// 根据ID获取月度计划签收部门列表
        /// </summary>
        /// <param name="OBJECT_ID1"></param>
        /// <returns></returns>
        public static string SQLMONTHLYTASKRECEIVEBYID(string OBJECT_ID1)
        {
            string SQLMONTHLYTASKRELEASE = @"select * from dbo.TASKRECEIVE where OBJECT_ID1 = '" + OBJECT_ID1 + "'  and DATAAREAID = '211'";
            return SQLMONTHLYTASKRELEASE;   
        }

        /// <summary>
        /// 获取月度计划生产明细
        /// </summary>
        /// <param name="OBJECT_ID2"></param>
        /// <returns></returns>
        public static string SQLMONTHLYTASKRELEASELIST(string OBJECT_ID2) 
        {
            string SQLMONTHLYTASKRELEASELIST = @"select PLANTASK_NO,DRAWING_NO,PART_NAME,TECHNICS_LINE,HAND_QUANTITY,
                                                        UNITID,CONVERT(VARCHAR(12),HAND_DATE,111) AS HAND_DATE,TASKTYPE,
                                                        PLANTASKTYPE,PLANTASK_MEMO,RECEIVENOTE,case DEPTNAME when '051' then '51车间' when '053' then '53车间' when '055' then '55车间' when '056' then '56车间' when '057' then '57车间' when '058' then '58车间' end as DEPTNAME   
                                                        from RELEASEDTASKS  where OBJECT_ID2 = '" + OBJECT_ID2 + "' and DATAAREAID = '211' ";
            return SQLMONTHLYTASKRELEASELIST;
        }

        /// <summary>
        /// 获取签收后的产品计划列表
        /// </summary>
        /// <returns></returns>
        public static string SQLTTA_PREPRODTABLE(string where) 
        {
            string SQLTTA_PREPRODTABLE = @"select RECID,CLASSID,OBJECTID,PLANTASKNUM,DRAWINGNUM,PARTNAME,DESIGNQTY,TECHNICSQTY,
                                                    a.PLAN_QUANTITY,HANDQTY,a.UNITID,TECHNICSLINE,CONVERT(VARCHAR(12),HANDDATE,111) AS HANDDATE,
                                                    CASE WHEN ISURGENT='0' THEN '否' WHEN ISURGENT='1' THEN '是' END AS ISURGENT,
                                                    PLANTASKMEMO,a.DEPARTMENT,RELEASEDQTY,INITFINISHEDQTY,REPORTFINISHED,CURRENTQTY,
                                                    CAL_SIGNDATETIME,PLANMODIFIEDDATE,a.CAL_PUBLISHDATETIME,TASKTYPE,createdby,CREATEDDATETIME  
                                                    from TTA_PREPRODTABLE as a "
                                                    + where;
            return SQLTTA_PREPRODTABLE;
        }

        /// <summary>
        /// 查看生产计划内容
        /// </summary>
        /// <param name="PLANTASKNUM"></param>
        /// <returns></returns>
        public static string SQLGetProdtableById(string RECID) 
        {
            string SQLGetProdtableById = @"select RECID,CLASSID,OBJECTID,PLANTASKNUM,DRAWINGNUM,PARTNAME,DESIGNQTY,TECHNICSQTY,
                                                    a.PLAN_QUANTITY,HANDQTY,a.UNITID,TECHNICSLINE,CONVERT(VARCHAR(12),HANDDATE,111) AS HANDDATE,
                                                    CASE WHEN ISURGENT='0' THEN '否' WHEN ISURGENT='1' THEN '是' END AS ISURGENT,
                                                    PLANTASKMEMO,a.DEPARTMENT,RELEASEDQTY,INITFINISHEDQTY,REPORTFINISHED,CURRENTQTY,
                                                    CAL_SIGNDATETIME,PLANMODIFIEDDATE,a.CAL_PUBLISHDATETIME,createdby,PLANMODIFIEDDATE,PRODUCTID 
                                                    from TTA_PREPRODTABLE as a "
                                                        + " where RECID = '" + RECID + "'";
            return SQLGetProdtableById; 
        }

        /// <summary>
        /// 获取指定ID厂月计划列表
        /// </summary>
        /// <returns></returns>
        public static string SQLTTA_MonthlyPlanById(string PLANTASKNUM)
        {
            string SQLTTA_MonthlyPlanById = @"select PLANTASK_NO,DRAWING_NO,PARTNAME,HAND_QUANTITY,UNITID,TECHNICS_LINE,HAND_DATE,PLANTASK_MEMO
                                                    from TTA_PREMONTHPRODTABLE  as a 
                                                    where 
                                                    a.PLANTASK_NO = '" + PLANTASKNUM + "'";
            return SQLTTA_MonthlyPlanById;
        }


//        /// <summary>
//        /// 查看生产计划内容
//        /// </summary>
//        /// <param name="PLANTASKNUM"></param>
//        /// <returns></returns>
//        public static string SQLTTA_PLANCONTENT(string PLANTASKNUM) 
//        {
//            string SQLTTA_PLANCONTENT = @"select * from (select a.RECID,CLASSID,OBJECTID,PLANTASKNUM,DRAWINGNUM,PARTNAME,DESIGNQTY,TECHNICSQTY,
//                                                    a.PLAN_QUANTITY,HANDQTY,a.UNITID,TECHNICSLINE,CONVERT(VARCHAR(12),HANDDATE,111) AS HANDDATE,
//                                                    CASE WHEN ISURGENT='0' THEN '否' WHEN ISURGENT='1' THEN '是' END AS ISURGENT,
//                                                    PLANTASKMEMO,a.DEPARTMENT,RELEASEDQTY,INITFINISHEDQTY,REPORTFINISHED,CURRENTQTY,
//                                                    CAL_SIGNDATETIME,c.RECEIVER,a.PRODUCTID,a.PLANMODIFIEDDATE,a.CAL_PUBLISHDATETIME
//                                                    from TTA_PREPRODTABLE  as a left join RELEASEDPLANS as b on a.OBJECTID = b.OBJECT_ID1
//                                                                                left join TASKRECEIVE as c on b.OBJECT_ID2 = c.OBJECT_ID1
//                                                                                where a.PLANTASKNUM = '"+ PLANTASKNUM ;
//                   SQLTTA_PLANCONTENT += "') as A group by RECID,CLASSID,OBJECTID,PLANTASKNUM,DRAWINGNUM,PARTNAME,DESIGNQTY,TECHNICSQTY,PLAN_QUANTITY,HANDQTY,UNITID,TECHNICSLINE,HANDDATE,";
//                   SQLTTA_PLANCONTENT += "ISURGENT,PLANTASKMEMO,a.DEPARTMENT,RELEASEDQTY,INITFINISHEDQTY,REPORTFINISHED,CURRENTQTY,CAL_SIGNDATETIME,RECEIVER,PRODUCTID,PLANMODIFIEDDATE,CAL_PUBLISHDATETIME";
//            return SQLTTA_PLANCONTENT;
        
//        }

        /// <summary>
        /// 获取月度计划列表
        /// </summary>
        /// <returns></returns>
        public static string SQLMonthPlanReceive(string where) 
        {
            string SQLMonthPlanReceive = @"select CLASSID1,OBJECTID1,PLANTASK_NO,RECID,DRAWING_NO,PARTNAME,TECHNICS_LINE,CONVERT(VARCHAR(12),HAND_DATE,111) AS HAND_DATE,
                                            case when URGENT='0' then '否' when URGENT='1' then '是' end as URGENT,
                                            CN_YEAR,CN_MONTH,HAND_QUANTITY, case when HAVETOPROD='0' then '未生成' when HAVETOPROD='1' then '已生成' end as HAVETOPROD,
                                            case [TASKTYPE] when '0' then '固定' when '1' then '在制' when '2' then '力争' when '3' then '自主' end as TASKTYPE,DATAAREAID,CREATEDDATETIME  from TTA_PREMONTHPRODTABLE  
                                            " + where;
            return SQLMonthPlanReceive;
        }

        /// <summary>
        /// 根据PLANTASKNUM和DRAWINGNUM获取产品计划的RECID
        /// </summary>
        /// <param name="PLANTASKNUM"></param>
        /// <param name="DRAWINGNUM"></param>
        /// <returns></returns>
        public static string SQLGetProductPlanByMonthlyPlan(string PLANTASKNUM, string DRAWINGNUM)
        {
            string SQLGetProductPlanByMonthlyPlan = @"select RECID,CLASSID,OBJECTID,PLANTASKNUM,DRAWINGNUM,PARTNAME,DESIGNQTY,TECHNICSQTY,
                                                    a.PLAN_QUANTITY,HANDQTY,a.UNITID,TECHNICSLINE,CONVERT(VARCHAR(12),HANDDATE,111) AS HANDDATE,
                                                    CASE WHEN ISURGENT='0' THEN '否' WHEN ISURGENT='1' THEN '是' END AS ISURGENT,
                                                    PLANTASKMEMO,a.DEPARTMENT,RELEASEDQTY,INITFINISHEDQTY,REPORTFINISHED,CURRENTQTY,
                                                    CAL_SIGNDATETIME,PLANMODIFIEDDATE,a.CAL_PUBLISHDATETIME,TASKTYPE,createdby 
                                                    from TTA_PREPRODTABLE as a "
                                                    + " where PLANTASKNUM =  '" + PLANTASKNUM + "' and DRAWINGNUM = '" + DRAWINGNUM + "'";
            return SQLGetProductPlanByMonthlyPlan;
        }

        /// <summary>
        /// 根据指定ID获取月度计划详细信息
        /// </summary>
        /// <returns></returns>
        public static string SQLMonthPlanReceiveContent(string CLASSID1, string OBJECTID1, string CN_YEAR, string CN_MONTH, string RECID, string DATAAREAID) 
        {
            string SQLMonthPlanReceiveContent = @"select PLANTASK_NO,DRAWING_NO,PARTNAME,DEPTNAME,
	                                                TECHNICS_LINE,HAND_QUANTITY,REPORTFINISHED,
	                                                HAND_DATE,case URGENT when '0' then '否' when '1' then '是' end as URGENT,
                                                    case TASKTYPE when '0' then '固定' when '1' then '在制' when '2' then '力争'
                                                    when '3' then '自主' end as TASKTYPE,
                                                    case PLANTASKTYPE when '0' then '新增' when '1' then '修改' when '2' then '删除' 
                                                    when '3' then '重提' end as PLANTASKTYPE,
	                                                MODIFIEDDATETIME,MODIFIEDBY,
	                                                CREATEDDATETIME,CREATEDBY from TTA_PREMONTHPRODTABLE   where "
                                                   + "CLASSID1 = '" + CLASSID1 + "' "
                                                   + "and OBJECTID1 = '" + OBJECTID1 + "' "
                                                   + "and DATAAREAID = '" + DATAAREAID + "' "
                                                   + "and CN_YEAR = '" + CN_YEAR + "' "
                                                   + "and CN_MONTH = '" + CN_MONTH + "' "
                                                   + "and RECID = '" + RECID + "' "
                                                   ;
            return SQLMonthPlanReceiveContent;
        }

        /// <summary>
        /// 获取车间作业计划列表
        /// </summary>
        /// <returns></returns>
        public static string SQLWorkShopTaskList(string where)
        {
            string SQLWorkShopTaskList = "select [DIMENSION],[PLANTASKNUM],[ITEMID],[NAME],CAST(round([QTYSCHED],2,3) as decimal(9,0)) as QTYSCHED,[TECHNICSLINE], "
                + "CONVERT(VARCHAR(12),[DLVDATE],111) AS [DLVDATE] , case [TASKTYPE] when '0' then '固定' when '1' then '在制' when '2' then '力争' when '3' then '自主' end as TASKTYPE, "
                + "[PRODSTATUS],[PLANTASKMEMO],[RECID],CREATEDDATETIME "
                + "from [PRODTABLE] "
                + where
                ;        
            return SQLWorkShopTaskList;
        }

        /// <summary>
        /// 获取指定ID车间作业计划详细信息
        /// </summary>
        /// <param name="PLANTASK_NO"></param>
        /// <returns></returns>
        public static string SQLWorkShopTaskContent(string PLANTASK_NO) 
        {
            string SQLWorkShopTaskContent = "select [PLANTASKNUM],[ITEMID],[NAME],CAST(round([QTYSCHED],2,3) as decimal(9,0)) as QTYSCHED,[TECHNICSLINE], "
                + "substring((convert(nvarchar,[DLVDATE],120)),1,10) as [DLVDATE], case [TASKTYPE] when '0' then '固定' when '1' then '在制' when '2' then '力争' when '3' then '自主' end as TASKTYPE, "
                + "[PRODSTATUS],[PLANTASKMEMO],[RECID] "
                + "from [PRODTABLE] "
                + "where [DATAAREAID] = '211' "
                + "and [DIMENSION] = '' ";
            return SQLWorkShopTaskContent;
        }

        /// <summary>
        ///   查询指定任务ID的订制单列表
        /// </summary>
        /// <param name="PLANTASKNUM"></param>
        /// <returns></returns>
        public static string SQLGetDingzhi(string RECID) 
        {
            string SQLGetDingzhi = "select a.PlanTaskNum,b.CAL_DwgNo,b.CAL_DwgName, "
                + "b.oprid, b.BSC_MainDept,substring((convert(nvarchar,b.TransDate,120)),1,10) as TransDate, "
                + "b.OutDept, b.CAL_OutFlagDescription, CAST(round(b.QtyInput,2,3) as decimal(9,0)) as [QtyInput], b.BSC_Remark, b.createdBy, b.[RECID], b.CREATEDDATETIME, "
                + "case b.[CAL_SENDORRECEIVE] when '0' then '新建（申请）' when '1' then '已提出' when '3' then '待确认' when '4' then '已签收' end  as [CAL_SENDORRECEIVE]"
                + "from PRODTABLE a,ProdJournalRoute b "
                + "where a.[PRODID] = b.[PRODID] "
               // + "and b.[DIMENSION] = '" + CurrentDepID + "' "
                + "and a.[DATAAREAID] = '211' "
                + "and b.[OUTFLAG] = '1' "
                + "and a.[RECID] = '" + RECID + "' "
                + " ORDER BY b.CREATEDDATETIME DESC"
                ;

            return SQLGetDingzhi;
        }

        /// <summary>
        /// 根据指定RECID获取详细车间作业计划表
        /// </summary>
        /// <param name="RECID">记录号</param>
        public static string SQLGetTaskListInfo(string RECID)
        {
            string SQLGetTaskListInfo = "select [name],[DIMENSION],[PLANTASKNUM],[ITEMID],[NAME],CAST(round([QTYSCHED],2,3) as decimal(9,0)) as QTYSCHED,[TECHNICSLINE], "
                + "substring((convert(nvarchar,[DLVDATE],120)),1,10) as [DLVDATE], case [TASKTYPE] when '0' then '固定' when '1' then '在制' when '2' then '力争' when '3' then '自主' end as TASKTYPE, "
                + "[PRODSTATUS],[PLANTASKMEMO],[RECID] "
                + "from [PRODTABLE] "
                + "where [DATAAREAID] = '211' "
                + "and [RECID] = '" + RECID + "' "
                ;        
            return SQLGetTaskListInfo;
        }


        /// <summary>
        /// 获取指定订制单RECID的订制单信息
        /// </summary>
        /// <param name="RECID"></param>
        /// <returns></returns>
        public static string SQLGetDingzhiByRecID(string RECID) 
        {
            string SQLGetDingzhiByRecID = "select a.name,a.PlanTaskNum,b.CAL_DwgNo,b.CAL_DwgName, "
                + "b.oprid, b.BSC_MainDept,substring((convert(nvarchar,b.TransDate,120)),1,10) as TransDate, substring((convert(nvarchar,b.CAL_PLANENDDATE,120)),1,10) as CAL_PLANENDDATE, "
                + "b.OutDept, b.CAL_OutFlagDescription, CAST(round(b.QtyInput,2,3) as decimal(9,0)) as [QtyInput], b.BSC_Remark, b.createdBy, b.[RECID], "
                + "case b.[CAL_SENDORRECEIVE] when '0' then '新建（申请）' when '1' then '已提出' when '3' then '待确认' when '4' then '已签收' end  as [CAL_SENDORRECEIVE]"
                + "from PRODTABLE a,ProdJournalRoute b "
                + "where a.[PRODID] = b.[PRODID] "
                //+ "and b.[DIMENSION] = '" + CurrentDepID + "' "
                + "and a.[DATAAREAID] = '211' "
                + "and b.[OUTFLAG] = '1' "
                + "and b.[RECID] = '" + RECID + "' ";
            return SQLGetDingzhiByRecID;
        }

        /// <summary>
        /// 查询指定任务RECID的订制单列表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string SQLGetDingzhdListByPID(string ID) 
        {
            string SQLGetDingzhdListByPID = @"select a.PlanTaskNum,b.CAL_DwgNo,b.CAL_DwgName, "
                 + "b.oprid, b.BSC_MainDept,substring((convert(nvarchar,b.TransDate,120)),1,10) as TransDate, "
                 + "b.OutDept, b.CAL_OutFlagDescription, CAST(round(b.QtyInput,2,3) as decimal(9,0)) as QtyInput, b.BSC_Remark, b.createdBy, b.[RECID], "
                 + "case b.[CAL_SENDORRECEIVE] when '0' then '新建（申请）' when '1' then '已提出' when '3' then '待确认' when '4' then '已签收' end as CAL_SENDORRECEIVE "
                 + "from PRODTABLE a,ProdJournalRoute b "
                 + "where a.[PRODID] = b.[PRODID] "
                 + "and a.[DATAAREAID] = '211' "
                 + "and b.[OUTFLAG] = '1' "
                 + "and a.[RECID] = '" + ID + "' ";
            return SQLGetDingzhdListByPID;      
        }

        /// <summary>
        /// 获取车间订制单列表
        /// </summary>
        /// <returns></returns>
        public static string SQLGetDingzhdList(string where) 
        {
            string SQLGetDingzhdList = @"select a.PlanTaskNum,b.CAL_DwgNo,b.CAL_DwgName, "
                 + "b.oprid, case b.BSC_MainDept when '051' then '51车间' when '053' then '53车间' when '055' then '55车间' when '056' then '56车间' when '057' then '57车间' when '058' then '58车间' end as BSC_MainDept,CONVERT(VARCHAR(12),TransDate,111) AS TransDate, CONVERT(VARCHAR(12),CAL_PLANENDDATE,111) AS CAL_PLANENDDATE,  "
                 + "case b.OutDept when '051' then '51车间' when '053' then '53车间' when '055' then '55车间' when '056' then '56车间' when '057' then '57车间' when '058' then '58车间' end as OutDept, b.CAL_OutFlagDescription, CAST(round(b.QtyInput,2,3) as decimal(9,0)) as QtyInput, b.BSC_Remark, b.createdBy, b.[RECID], b.CREATEDDATETIME, "
                 + "case b.[CAL_SENDORRECEIVE] when '0' then '新建（申请）' when '1' then '已提出' when '3' then '待确认' when '4' then '已签收' end as CAL_SENDORRECEIVE "
                 + "from PRODTABLE a,ProdJournalRoute b "
                 + where
                ;
            return SQLGetDingzhdList;
        }

        /// <summary>
        /// 获取车间承制单列表
        /// </summary>
        /// <returns></returns>
        public static string SQLGetChejianChengzhd(string where) 
        {
            string SQLGetChejianChengzhd = "select a.PlanTaskNum,b.CAL_DwgNo,b.CAL_DwgName, "
                            + "b.oprid, case b.BSC_MainDept when '051' then '51车间' when '053' then '53车间' when '055' then '55车间' when '056' then '56车间' when '057' then '57车间' when '058' then '58车间' end as BSC_MainDept,CONVERT(VARCHAR(12),TransDate,111) AS TransDate , "
                            + "case b.OutDept when '051' then '51车间' when '053' then '53车间' when '055' then '55车间' when '056' then '56车间' when '057' then '57车间' when '058' then '58车间' end as OutDept, b.CAL_OutFlagDescription, CAST(round(b.QtyInput,2,3) as decimal(9,0)) as QtyInput, b.BSC_Remark, b.createdBy, b.[RECID], "
                            + "case b.[CAL_SENDORRECEIVE] when '1' then '未签收' when '3' then '待确认' when '4' then '已签收' end as CAL_SENDORRECEIVE, "
                            + "case b.[OPRFINISHSTATUS] when '0' then '未完成' when '1' then '已完成' when '2' then '已确认' end as OPRFINISHSTATUS, b.CREATEDDATETIME "
                            + "from PRODTABLE a,ProdJournalRoute b "
                            + where
                            ;

            return SQLGetChejianChengzhd;
        }

        /// <summary>
        /// 返回订制单
        /// </summary>
        /// <param name="RECID"></param>
        /// <returns></returns>
        public static string SQLGetDingzhFinish(string RECID) 
        {
            string SQLGetDingzhFinish = "select b.oprid, b.BSC_MainDept, b.TransDate, b.OutDept, b.CAL_OutFlagDescription, b.QtyInput, b.BSC_Remark, b.createdBy, b.[RECID] "
               + "from PRODTABLE a,ProdJournalRoute b "
               + "where a.[PRODID] = b.[PRODID] "
               + "and a.[DATAAREAID] = '211' "
               + "and b.[OUTFLAG] = '1' "
               + "and b.[RECID] = '" + RECID + "' "
               ;
            return SQLGetDingzhFinish;
        }

        /// <summary>
        /// 返回订制单汇报详细信息
        /// </summary>
        /// <param name="RECID"></param>
        /// <returns></returns>
        public static string SQLGetDingzhiReportDetails(string RECID) 
        {
            string strSQL = @"SELECT [AUTUALENDDATE],CAST(round([HOURS],2,3) as decimal(9,1)) as [HOURS],
						CAST(round([QTYGOOD],2,3) as decimal(9,0)) as [QTYGOOD],
						CAST(round([QTYERROR],2,3) as decimal(9,0)) as [QTYERROR],
						CAST(round([QTYLACK],2,3) as decimal(9,0)) as [QTYLACK],
						CAST(round([QTYFLOAT],2,3) as decimal(9,0)) as [QTYFLOAT],
						case [POSTED] when '0' then '否' when '1' then '是' end as POSTED, 
						[CAL_ZHIKONGKAHAO],[CAL_QUALIFIEDCARDNO],[CAL_FANGXINGDANHAO],[REMARK], [CREATEDBY],[CAL_FACTORYGROUPID],[CAL_INSPECTREPORTNO] 
						FROM [CopyOfDynamicsAx1].[dbo].[CAL_PRODJOURNALSELF_SUB] where refrecid ='" + RECID + "'";
            return strSQL;
        
        }


        public static string GetDingzhdRecid(string RECID)
        {
            string strGetDingzhdRecid = @"select b.RECID from ProdJournalTable as a 
                                            left join ProdJournalRoute as b
                                             on a.JOURNALID = b.JOURNALID
                                             WHERE a.RECID ='" + RECID + "'";
            return strGetDingzhdRecid;
        }
    }
}