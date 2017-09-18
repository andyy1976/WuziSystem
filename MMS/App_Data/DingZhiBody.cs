using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms
{
    public class DingZhiBody
    {
        private string _ID;
        /// <summary>
        /// 标识
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _RecId;
        /// <summary>
        /// 北京订制单ID
        /// </summary>
        public string RecId
        {
            get { return _RecId; }
            set { _RecId = value; }
        }

        private string _Barcode;
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return _Barcode; }
            set { _Barcode = value; }
        }

        private string _TaskNum;
        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNum
        {
            get { return _TaskNum; }
            set { _TaskNum = value; }
        }

        private string _ProductName;
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }

        private string _DrawingNum;
        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNum
        {
            get { return _DrawingNum; }
            set { _DrawingNum = value; }
        }

        private string _Unit;
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        private string _Amount;
        /// <summary>
        /// 数量
        /// </summary>
        public string Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        private string _dingZhiXingZhi;
        /// <summary>
        /// 定制性质
        /// </summary>
        public string DingZhiXingZhi
        {
            get { return _dingZhiXingZhi; }
            set { _dingZhiXingZhi = value; }
        }

        private string _dingZhiDescription;
        /// <summary>
        /// 定制描述
        /// </summary>
        public string DingZhiDescription
        {
            get { return _dingZhiDescription; }
            set { _dingZhiDescription = value; }
        }

        private string _OrderWorkShop;
        /// <summary>
        /// 定制车间
        /// </summary>
        public string OrderWorkShop
        {
            get { return _OrderWorkShop; }
            set { _OrderWorkShop = value; }
        }

        private string _DisposeWorkShop;
        /// <summary>
        /// 执行车间
        /// </summary>
        public string DisposeWorkShop
        {
            get { return _DisposeWorkShop; }
            set { _DisposeWorkShop = value; }
        }

        private DateTime _OrderDate;
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; }
        }

        private string _AppointDate;
        /// <summary>
        /// 送回时间
        /// </summary>
        public string AppointDate
        {
            get { return _AppointDate; }
            set { _AppointDate = value; }
        }
        /// <summary>
        /// 签收时间
        /// </summary>
        private DateTime acceptDate;

        public DateTime AcceptDate
        {
            get { return acceptDate; }
            set { acceptDate = value; }
        }

        
        
        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
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

        private string _State;
        /// <summary>
        /// 订制单状态，0：新建，1：已提交，3：待确认，4：已签收
        /// </summary>
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        private bool _Enable;
        /// <summary>
        /// 逻辑删除标记
        /// </summary>
        public bool Enable
        {
            get { return _Enable; }
            set { _Enable = value; }
        }

        private string _BackReason;
        /// <summary>
        /// 退回理由
        /// </summary>
        public string BackReason
        {
            get { return _BackReason; }
            set { _BackReason = value; }
        }
    }
}