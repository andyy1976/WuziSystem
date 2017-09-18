using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms
{
    public class ScanLogBody
    {

        private string _ID;
        /// <summary>
        /// 扫描记录标识
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


        private string _Barcode;
        /// <summary>
        /// 条码标识
        /// </summary>
        public string Barcode
        {
            get { return _Barcode; }
            set { _Barcode = value; }
        }

        private DateTime _ScanTime;
        /// <summary>
        /// 扫描时间
        /// </summary>
        public DateTime ScanTime
        {
            get { return _ScanTime; }
            set { _ScanTime = value; }
        }

        private string _StationID;
        /// <summary>
        /// 工位标识
        /// </summary>
        public string StationID
        {
            get { return _StationID; }
            set { _StationID = value; }
        }

        private string _ScanIP;
        /// <summary>
        /// 扫描IP
        /// </summary>
        public string ScanIP
        {
            get { return _ScanIP; }
            set { _ScanIP = value; }
        }

        private string _ScanPerson;
        /// <summary>
        /// 扫描人
        /// </summary>
        public string ScanPerson
        {
            get { return _ScanPerson; }
            set { _ScanPerson = value; }
        }

        private string _CertificateID;
        /// <summary>
        /// 成品合格证号
        /// </summary>
        public string CertificateID
        {
            get { return _CertificateID; }
            set { _CertificateID = value; }
        }

        private string _WorkState;
        /// <summary>
        /// 开工完工标识.0：开工，1：完工，2：暂停 3，4，5：交接单 提 签 驳 6，7，8：订制单 提 签 驳
        /// </summary>
        public string WorkState
        {
            get { return _WorkState; }
            set { _WorkState = value; }
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

        private string _PauseTypeID;
        /// <summary>
        /// 暂停类型ID
        /// </summary>
        public string PauseTypeID
        {
            get { return _PauseTypeID; }
            set { _PauseTypeID = value; }
        }

        private string _NotFinishCertificateID;
        /// <summary>
        /// 半成品合格证
        /// </summary>
        public string NotFinishCertificateID
        {
            get { return _NotFinishCertificateID; }
            set { _NotFinishCertificateID = value; }
        }

    }
}