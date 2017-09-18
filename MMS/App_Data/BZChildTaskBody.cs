using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductBarCodeManagementAndTrack
{
    public class BZChildTaskBody
    {
        private string _BarcodeFull;
        /// <summary>
        /// 条码
        /// </summary>
        public string BarcodeFull
        {
            get { return _BarcodeFull; }
            set { _BarcodeFull = value; }
        }

        private string _ChildProductName;
        /// <summary>
        /// 子任务名
        /// </summary>
        public string ChildProductName
        {
            get { return _ChildProductName; }
            set { _ChildProductName = value; }
        }

        private string _ChildDrawingNum;
        /// <summary>
        /// 子任务图号
        /// </summary>
        public string ChildDrawingNum
        {
            get { return _ChildDrawingNum; }
            set { _ChildDrawingNum = value; }
        }

        private string _ChildExpectFinishDate;
        /// <summary>
        /// 预计时间
        /// </summary>
        public string ChildExpectFinishDate
        {
            get { return _ChildExpectFinishDate; }
            set { _ChildExpectFinishDate = value; }
        }

        private string _ZhiKongCardNum;
        /// <summary>
        /// 质控卡号
        /// </summary>
        public string ZhiKongCardNum
        {
            get { return _ZhiKongCardNum; }
            set { _ZhiKongCardNum = value; }
        }


        private int _TotalNumber;
        /// <summary>
        /// 数量
        /// </summary>
        public int TotalNumber
        {
            get { return _TotalNumber; }
            set { _TotalNumber = value; }
        }

        private int _AllowSplitAmount;
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int AllowSplitAmount
        {
            get { return _AllowSplitAmount; }
            set { _AllowSplitAmount = value; }
        }

        private string _TaskID;
        /// <summary>
        /// 主任务ID
        /// </summary>
        public string TaskID
        {
            get { return _TaskID; }
            set { _TaskID = value; }
        }

        private DateTime _CreateTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

        private string _CreatePerson;
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatePerson
        {
            get { return _CreatePerson; }
            set { _CreatePerson = value; }
        }

        private string _PID;
        /// <summary>
        /// 父ID
        /// </summary>
        public string PID
        {
            get { return _PID; }
            set { _PID = value; }
        }

        private string _Enable;
        /// <summary>
        /// 逻辑删除标记
        /// </summary>
        public string Enable
        {
            get { return _Enable; }
            set { _Enable = value; }
        }

        private string _UpdatedID;
        /// <summary>
        /// 更新后原子任务ID
        /// </summary>
        public string UpdatedID
        {
            get { return _UpdatedID; }
            set { _UpdatedID = value; }
        }
    }
}