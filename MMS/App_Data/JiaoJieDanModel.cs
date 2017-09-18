using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mms
{
    public class JiaoJieDanModel
    {

        public class JiaoJieDanMainTable
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

            private string _ReceiveWorkShop;
            /// <summary>
            /// 接收车间代号
            /// </summary>
            public string ReceiveWorkShop
            {
                get { return _ReceiveWorkShop; }
                set { _ReceiveWorkShop = value; }
            }

            private string _MakeWorkShop;
            /// <summary>
            /// 制造车间代号
            /// </summary>
            public string MakeWorkShop
            {
                get { return _MakeWorkShop; }
                set { _MakeWorkShop = value; }
            }

            private string _CreateUserAccount;
            /// <summary>
            /// 办理人帐号
            /// </summary>
            public string CreateUserAccount
            {
                get { return _CreateUserAccount; }
                set { _CreateUserAccount = value; }
            }

            private DateTime _CreatedTime;
            /// <summary>
            /// 提交时间
            /// </summary>
            public DateTime CreatedTime
            {
                get { return _CreatedTime; }
                set { _CreatedTime = value; }
            }

            private string _ReceiveUserAccount;
            /// <summary>
            /// 接收人帐号
            /// </summary>
            public string ReceiveUserAccount
            {
                get { return _ReceiveUserAccount; }
                set { _ReceiveUserAccount = value; }
            }

            private DateTime _ReceiveTime;
            /// <summary>
            /// 接收时间
            /// </summary>
            public DateTime ReceiveTime
            {
                get { return _ReceiveTime; }
                set { _ReceiveTime = value; }
            }

            private string _OperationState;
            /// <summary>
            /// 当前状态
            /// </summary>
            public string OperationState
            {
                get { return _OperationState; }
                set { _OperationState = value; }
            }
        }

        public class PartsOrder
        {
            private string _Barcode;
            /// <summary>
            /// 条码号
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
            /// 名称
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

            private string _CertificateID;
            /// <summary>
            /// 合格证
            /// </summary>
            public string CertificateID
            {
                get { return _CertificateID; }
                set { _CertificateID = value; }
            }

            private string _UnitName;
            /// <summary>
            /// 单位
            /// </summary>
            public string UnitName
            {
                get { return _UnitName; }
                set { _UnitName = value; }
            }

            private int _ProductAmount;
            /// <summary>
            /// 数量
            /// </summary>
            public int ProductAmount
            {
                get { return _ProductAmount; }
                set { _ProductAmount = value; }
            }

            private decimal _Cast;
            /// <summary>
            /// 金额
            /// </summary>
            public decimal Cast
            {
                get { return _Cast; }
                set { _Cast = value; }
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

            private string _MainTableID;
            /// <summary>
            /// 主表标识号
            /// </summary>
            public string MainTableID
            {
                get { return _MainTableID; }
                set { _MainTableID = value; }
            }
        }
    }
}