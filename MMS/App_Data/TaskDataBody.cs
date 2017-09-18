using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms
{
    public class TaskDataBody
    {
        /// <summary>
        /// 任务标识
        /// </summary>
        private string _ID;
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>
        /// 型号
        /// </summary>
        private string _ProductModel;
        public string ProductModel
        {
            get { return _ProductModel; }
            set { _ProductModel = value; }
        }
        /// <summary>
        /// 任务号
        /// </summary>
        private string _TaskNum;
        public string TaskNum
        {
            get { return _TaskNum; }
            set { _TaskNum = value; }
        }

        /// <summary>
        /// 图号
        /// </summary>
        private string _DrawingNum;
        public string DrawingNum
        {
            get { return _DrawingNum; }
            set { _DrawingNum = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        private string _ProductName;
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }

        /// <summary>
        /// 设计数量（计划数量）
        /// </summary>
        private int _PlanAmount;
        public int PlanAmount
        {
            get { return _PlanAmount; }
            set { _PlanAmount = value; }
        }

        /// <summary>
        /// 投产数量（交付数量）
        /// </summary>
        private int _MakeAmount;
        public int MakeAmount
        {
            get { return _MakeAmount; }
            set { _MakeAmount = value; }
        }

        /// <summary>
        /// 进度
        /// </summary>
        private string _ProgressDate;
        public string ProgressDate
        {
            get { return _ProgressDate; }
            set { _ProgressDate = value; }
        }

        /// <summary>
        /// 完成数量
        /// </summary>
        private int _FinishAmount;
        public int FinishAmount
        {
            get { return _FinishAmount; }
            set { _FinishAmount = value; }
        }

        /// <summary>
        /// 完成时间
        /// </summary>
        private string _FinishDate;
        public string FinishDate
        {
            get { return _FinishDate; }
            set { _FinishDate = value; }
        }

        /// <summary>
        /// 材料
        /// </summary>
        private string _Material;
        public string Material
        {
            get { return _Material; }
            set { _Material = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        /// <summary>
        /// 工艺路线
        /// </summary>
        private string _CraftRoute;
        public string CraftRoute
        {
            get { return _CraftRoute; }
            set { _CraftRoute = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        private string _State;
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        /// <summary>
        /// 下发时间
        /// </summary>
        private string _DistributeDate;
        public string DistributeDate
        {
            get { return _DistributeDate; }
            set { _DistributeDate = value; }
        }

        /// <summary>
        /// 逻辑删除标记（true：可用，false：删除）
        /// </summary>
        private bool _Enable;
        public bool Enable
        {
            get { return _Enable; }
            set { _Enable = value; }
        }

        /// <summary>
        /// 部门
        /// </summary>
        private string _DeptCode;
        public string DeptCode
        {
            get { return _DeptCode; }
            set { _DeptCode = value; }
        }

        private string _DisposeType;
        /// <summary>
        /// 任务处理类型（新增，更改，删除）
        /// </summary>
        public string DisposeType
        {
            get { return _DisposeType; }
            set { _DisposeType = value; }
        }

        /// <summary>
        /// 生产任务导入时记录导入日志的编号
        /// </summary>
        private string _TaskCreatedLogID;
        public string TaskCreatedLogID
        {
            get { return _TaskCreatedLogID; }
            set { _TaskCreatedLogID = value; }
        }

        /// <summary>
        /// 修改生产任务时记录更改后的新生产记录的ID
        /// </summary>
        private string _UpdatedOld;
        public string UpdatedOld
        {
            get { return _UpdatedOld; }
            set { _UpdatedOld = value; }
        }

        private string _RecId;
        /// <summary>
        /// 北京任务对应ID
        /// </summary>
        public string RecId
        {
            get { return _RecId; }
            set { _RecId = value; }
        }
    }                
}