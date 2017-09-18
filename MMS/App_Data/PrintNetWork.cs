using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Printing;
using CSharpActiveX;
using com.epson.pos.driver;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.Data;
using Camc.Web.Library;
using ThoughtWorks.QRCode.Codec.Util;
using ThoughtWorks.QRCode.Codec.Data;
using System.Configuration;
using System.Web.UI;

namespace mms.Banner
{
    public class PrintNetWork : IObjectSafety
    {
        private StatusAPI m_objAPI = new StatusAPI();

        private string jiaojie = string.Empty;
        private string jiaojieInOut = string.Empty;
        private DBInterface DBI;
        private static string DBConn;

        //外协厂家，车间，，时间，单号;
        //'IN','航亚,51车间,TJ\tj_mabaizhang,2015-05-12 12:08:13,000015WXJJD=;
        //47AS04-51-1002,箱体壁板1,1CG311-101,ZC047014001000000027,124,2015/5/20 10:00:00;
        //47AS04-51-1003,箱体壁板2,1CG311-102,ZC047014001000000028,5,2015/5/20 20:00:00;
        //47AS04-51-1004,箱体壁板3,1CG311-103,ZC047014001000000029,6,2015/5/20 2:00:00'
        #region IObjectSafety 成员

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;


        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) &&
                           (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) &&
                            (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        #endregion

        /// <summary>
        /// 通过获取的ID来进行打印
        /// </summary>
        private string MainID = string.Empty;


        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="PRINTER_NAME"></param>
        /// <param name="ID"></param>
        /// <param name="PrintType"></param>
        /// <returns></returns>
        public string PrintFn(string PRINTER_NAME, string ID, string PrintType)
        {
            Boolean isFinish;
            MainID = ID;
            PrintDocument pdPrint = new PrintDocument();
            string DocumentName = "";
            if (PrintType == "WXOut")
            {
                DocumentName = "外协交接单（出）";
                pdPrint.PrintPage += new PrintPageEventHandler(pdPrint_PrintPage_Out);
            }
            else if (PrintType == "WXIn")
            {
                DocumentName = "外协交接单（入）";
                pdPrint.PrintPage += new PrintPageEventHandler(pdPrint_PrintPage_In);
            }
            else if (PrintType == "DingZhiDan")
            {
                DocumentName = "订制单";
                pdPrint.PrintPage += new PrintPageEventHandler(pdPrint_PrintPageDingZhi);
            }
            else if (PrintType == "JiaoJieDan")
            {
                DocumentName = "交接单";
                pdPrint.PrintPage += new PrintPageEventHandler(pdPrint_PrintPageJJD);
            }
            pdPrint.PrinterSettings.PrinterName = PRINTER_NAME;

            try
            {
                if (m_objAPI.OpenMonPrinter(OpenType.TYPE_PRINTER, pdPrint.PrinterSettings.PrinterName) == ErrorCode.SUCCESS)
                {
                    if (pdPrint.PrinterSettings.IsValid)
                    {
                        pdPrint.DocumentName = DocumentName;
                        // Start printing.
                        pdPrint.Print();

                        // Check printing status.
                        isFinish = false;

                        // Perform the status check as long as the status is not ASB_PRINT_SUCCESS.
                        do
                        {
                            if (m_objAPI.Status.ToString().Contains(ASB.ASB_PRINT_SUCCESS.ToString()))
                                isFinish = true;

                        } while (!isFinish);
                        return "打印成功";
                        // Notify printing completion.
                    }
                    else
                    {
                        return "错误：10000，打印机正在打印中，请稍候！";
                    }
                    // Always close the Status Monitor after using the Status API.
                    if (m_objAPI.CloseMonPrinter() != ErrorCode.SUCCESS)
                    {
                        return "1";
                    }

                }
                else
                {
                    return "错误：10001，打开打印机失败！";
                }

            }
            catch
            {
                return "错误：10002，打开API失败！";
            }
        }
        #region 外协打印
        /// <summary>
        /// 打印外协交接单（出）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pdPrint_PrintPage_Out(object sender, PrintPageEventArgs e)
        {
            float x = 0, y = 0, lineOffset = 0;
            Font printFont = new Font("宋体", (float)20, FontStyle.Bold, GraphicsUnit.Point);
            DataTable ExportInfoDT = GetWXDingzhiSignHistoryInfo(MainID);
            DataTable DingzhidanDT = GetWXDingzhiSignHistoryByID(MainID);

            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point);
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            e.Graphics.DrawString("天津航天长征火箭制造有限公司", printFont, Brushes.Black, x, y);
            x = 20;
            y = 25;

            printFont = new Font("宋体", (float)20, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font

            e.Graphics.DrawString("交接单(出)", printFont, Brushes.Black, x, y);


            QRCodeEncoder encoder = new QRCodeEncoder();
            QRCodeDecoder decoder = new QRCodeDecoder();
            encoder.QRCodeVersion = 0;
            encoder.QRCodeScale = 2;
            //Bitmap bb = encoder.Encode(subrow[4]);
            Bitmap bb = encoder.Encode(DingzhidanDT.Rows[0]["WXJJDBarcode"].ToString());
            Image barc = bb;
            x = 170;
            y = 20;
            e.Graphics.DrawImage(barc, x, y);

            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += 40 + lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5 + 5;
            e.Graphics.DrawString("制造车间：" + DingzhidanDT.Rows[0]["DeptName"].ToString(), printFont, Brushes.Black, x, y);

            y += lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5 + 5;
            e.Graphics.DrawString("外协厂商：" + DingzhidanDT.Rows[0]["ReceiveWorkShop"].ToString(), printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5;
            e.Graphics.DrawString("编号:" + DingzhidanDT.Rows[0]["WXJJDBarcode"].ToString(), printFont, Brushes.Black, x, y);

            //printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;
            e.Graphics.DrawString("日期：" + DingzhidanDT.Rows[0]["CreateTime"].ToString(), printFont, Brushes.Black, x, y);


            //printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;

            e.Graphics.DrawString("----------------------------------------------", printFont, Brushes.Black, x, y);
            // Print the receipt text
            for (int i = 0; i < ExportInfoDT.Rows.Count; i++)
            {
                printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
                lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;

                //47AS04-51-1002,0
                //箱体壁板,1
                //1CG311-101,2
                //ZC047014001000000027,3
                //4 4
                //要求时间5

                x = 0;
                y += lineOffset;
                e.Graphics.DrawString(ExportInfoDT.Rows[i]["TaskNum"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString(ExportInfoDT.Rows[i]["ProductName"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString(ExportInfoDT.Rows[i]["DrawingNum"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString("要求时间:" + ExportInfoDT.Rows[i]["FinishDate"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;


                // y += lineOffset;
                e.Graphics.DrawString(ExportInfoDT.Rows[i]["Barcode"].ToString(), printFont, Brushes.Black, x, y);
                printFont = new Font("Times New Roman", (float)20, FontStyle.Regular, GraphicsUnit.Point);
                e.Graphics.DrawString("×" + ExportInfoDT.Rows[i]["MakeAmount"].ToString(), printFont, Brushes.Black, 150, y - 60);

                printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
                y += lineOffset;
                x = 0;
                e.Graphics.DrawString("----------------------------------------------", printFont, Brushes.Black, x, y);

            }
            printFont = new Font("宋体", (float)10, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("提交人：         接收人:", printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)7, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            y += lineOffset + 40;
            e.Graphics.DrawString("                                    (请签字)", printFont, Brushes.Black, x, y);
            e.HasMorePages = false;
        }
        /// <summary>
        /// 打印外协交接单（入）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pdPrint_PrintPage_In(object sender, PrintPageEventArgs e)
        {
            float x = 0, y = 0, lineOffset = 0;
            Font printFont = new Font("宋体", (float)20, FontStyle.Bold, GraphicsUnit.Point);
            DataTable ExportInfoDT = GetWXDingzhiSignReturnHistoryInfo(MainID);
            DataTable DingzhidanDT = GetWXDingzhiSignReturnHistoryByID(MainID);

            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point);
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            e.Graphics.DrawString("天津航天长征火箭制造有限公司", printFont, Brushes.Black, x, y);
            x = 20;
            y = 25;

            printFont = new Font("宋体", (float)20, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font

            e.Graphics.DrawString("交接单(入)", printFont, Brushes.Black, x, y);


            QRCodeEncoder encoder = new QRCodeEncoder();
            QRCodeDecoder decoder = new QRCodeDecoder();
            encoder.QRCodeVersion = 0;
            encoder.QRCodeScale = 2;
            //Bitmap bb = encoder.Encode(subrow[4]);
            Bitmap bb = encoder.Encode(DingzhidanDT.Rows[0]["WXJJDBarcode"].ToString());
            Image barc = bb;
            x = 170;
            y = 20;
            e.Graphics.DrawImage(barc, x, y);

            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += 40 + lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5 + 5;
            e.Graphics.DrawString("制造车间：" + DingzhidanDT.Rows[0]["DeptName"].ToString(), printFont, Brushes.Black, x, y);

            y += lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5 + 5;
            e.Graphics.DrawString("外协厂商：" + DingzhidanDT.Rows[0]["ReceiveWorkShop"].ToString(), printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5;
            e.Graphics.DrawString("编号:" + DingzhidanDT.Rows[0]["WXJJDBarcode"].ToString(), printFont, Brushes.Black, x, y);

            //printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;
            e.Graphics.DrawString("日期：" + DingzhidanDT.Rows[0]["CreateTime"].ToString(), printFont, Brushes.Black, x, y);


            //printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;

            e.Graphics.DrawString("----------------------------------------------", printFont, Brushes.Black, x, y);
            // Print the receipt text
            for (int i = 0; i < ExportInfoDT.Rows.Count; i++)
            {
                printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
                lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;

                //47AS04-51-1002,0
                //箱体壁板,1
                //1CG311-101,2
                //ZC047014001000000027,3
                //4 4
                //要求时间5

                x = 0;
                y += lineOffset;
                e.Graphics.DrawString(ExportInfoDT.Rows[i]["TaskNum"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString(ExportInfoDT.Rows[i]["ProductName"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString(ExportInfoDT.Rows[i]["DrawingNum"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString("要求时间:" + ExportInfoDT.Rows[i]["FinishDate"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;


                // y += lineOffset;
                e.Graphics.DrawString(ExportInfoDT.Rows[i]["Barcode"].ToString(), printFont, Brushes.Black, x, y);
                printFont = new Font("Times New Roman", (float)20, FontStyle.Regular, GraphicsUnit.Point);
                e.Graphics.DrawString("×" + ExportInfoDT.Rows[i]["MakeAmount"].ToString(), printFont, Brushes.Black, 150, y - 60);

                printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
                y += lineOffset;
                x = 0;
                e.Graphics.DrawString("----------------------------------------------", printFont, Brushes.Black, x, y);

            }
            printFont = new Font("宋体", (float)10, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("提交人：         接收人:", printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)7, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            y += lineOffset + 40;
            e.Graphics.DrawString("                                    (请签字)", printFont, Brushes.Black, x, y);
            e.HasMorePages = false;
        }

        /// <summary>
        /// 获取签收记录子表详情（签收 出）
        /// </summary>
        /// <param name="LogID"></param>
        /// <returns></returns>
        protected DataTable GetWXDingzhiSignHistoryInfo(string DingzhiDanID)
        {
            DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"select a.ID
                                  ,b.BarcodeFull as Barcode
                                  ,c.ProductName
                                  ,c.TaskNum
                                  ,c.DrawingNum
                                  ,b.AllowSplitAmount as MakeAmount
                                  ,a.OrderDate
                                  ,a.FinishDate
                                  ,a.WxCompany
                                  ,a.Remark
                                  ,case a.Urgent  when 1 then '是' else '否' end  as Urgent 
                                  ,case a.SignState when 0 then '未签收' when 1 then '已签收' when 2 then '已交回' end as State 
                                  from dbo.Sys_WX_ExportInfo as a 
                                  inner join dbo.BarCode as b on a.BarcodeID = b.ID  
                                  left join dbo.Task as c on b.TaskID = c.ID
                                  where a.DingzhiDanID = '" + DingzhiDanID + "' and b.Enable = '1' ";
                DataTable SignHistory = DBI.Execute(strSQL, true);

                return SignHistory;
            }
            catch (Exception e)
            {
                throw new Exception("获取签收记录子表详情出现异常" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取指定ID的外协签收历史记录（签收 出）
        /// </summary>
        /// <returns></returns>
        protected DataTable GetWXDingzhiSignHistoryByID(string ID)
        {
            DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            DataTable SignHistory = new DataTable();
            string strSQL;
            try
            {
                strSQL = @"SELECT *,(select DeptName from  DeptEnum where DeptCode=MakeWorkShop) as DeptName   FROM [dbo].[Sys_WX_JiaoJieDan_Out] where ID = '" + ID + "' ";

                SignHistory = DBI.Execute(strSQL, true);
                return SignHistory;
            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message.ToString());
            }
        }

        /// <summary>
        /// 获取指定ID的外协签收历史记录 （入）
        /// </summary>
        /// <returns></returns>
        protected DataTable GetWXDingzhiSignReturnHistoryByID(string ID)
        {
            DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            DataTable SignHistory = new DataTable();
            string strSQL;
            try
            {
                strSQL = @"SELECT *,(select DeptName from  DeptEnum where DeptCode=MakeWorkShop) as DeptName  FROM [dbo].[Sys_WX_JiaoJieDan_In] where ID = '" + ID + "' order by CreateTime desc";

                SignHistory = DBI.Execute(strSQL, true);
                return SignHistory;
            }
            catch (Exception e)
            {
                throw new Exception("" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取签收记录子表详情（交接 入）
        /// </summary>
        /// <param name="LogID"></param>
        /// <returns></returns>
        protected DataTable GetWXDingzhiSignReturnHistoryInfo(string DingzhiDanReturnID)
        {
            DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            DataTable SignHistory = new DataTable();
            string strSQL;
            try
            {
                strSQL = @"select a.ID
                                  ,b.BarcodeFull as Barcode
                                  ,c.ProductName
                                  ,c.TaskNum
                                  ,c.DrawingNum
                                  ,b.AllowSplitAmount as MakeAmount
                                  ,a.OrderDate
                                  ,a.FinishDate
                                  ,a.WxCompany
                                  ,a.Remark
                                  ,case a.Urgent  when 1 then '是' else '否' end  as Urgent 
                                  ,case a.SignState when 0 then '未签收' when 1 then '已签收' when 2 then '已交回' end as State 
                                  from dbo.Sys_WX_ExportInfo as a 
                                  inner join dbo.BarCode as b on a.BarcodeID = b.ID  
                                  left join dbo.Task as c on b.TaskID = c.ID
                                  where a.DingzhiDanReturnID = '" + DingzhiDanReturnID + "'  and b.Enable = '1'";
                SignHistory = DBI.Execute(strSQL, true);

                return SignHistory;
            }
            catch (Exception e)
            {
                throw new Exception("获取签收记录子表详情出现异常" + e.Message.ToString());
            }
        }
        #endregion

        #region 订制单打印
        /// <summary>
        /// 订制单打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pdPrint_PrintPageDingZhi(object sender, PrintPageEventArgs e)
        {
            float x = 0, y = 0, lineOffset = 0;
            Font printFont = new Font("宋体", (float)20, FontStyle.Bold, GraphicsUnit.Point);

            string[] subrow = GetPrintStr(MainID).Split('$');
            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point);

            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;

            e.Graphics.DrawString("天津航天长征火箭制造有限公司", printFont, Brushes.Black, x, y);
            QRCodeEncoder encoder = new QRCodeEncoder();
            QRCodeDecoder decoder = new QRCodeDecoder();
            encoder.QRCodeVersion = 0;
            encoder.QRCodeScale = 2;

            Bitmap bb = encoder.Encode(subrow[0]);
            Image barc = bb;
            x = 160;
            //y = 0 + lineOffset;
            e.Graphics.DrawImage(barc, x, 40);

            printFont = new Font("宋体", (float)20, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            e.Graphics.PageUnit = GraphicsUnit.Point;
            x = 30;
            y += 10 + lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5;
            e.Graphics.DrawString("订制单", printFont, Brushes.Black, x, y);

            //0 航亚,
            //1 51车间,
            //2 TJ\tj_mabaizhang,
            //3 2015-05-12 12:08:13.000,
            //4 000015WXJJD=;



            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += 20 + lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5 + 5;
            e.Graphics.DrawString("订制车间：" + subrow[1], printFont, Brushes.Black, x, y);

            y += lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5 + 5;
            e.Graphics.DrawString("执行车间：" + subrow[2], printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5;
            e.Graphics.DrawString("编号:" + subrow[0], printFont, Brushes.Black, x, y);

            //printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;
            e.Graphics.DrawString("订制日期：" + subrow[3], printFont, Brushes.Black, x, y);





            //printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;

            e.Graphics.DrawString("----------------------------------------------", printFont, Brushes.Black, x, y);
            // Print the receipt text


            printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;

            //47AS04-51-1002,
            //箱体壁板,
            //1CG311-101,
            //ZC047014001000000027,
            //4
            //要求时间

            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("订制性质：" + subrow[5], printFont, Brushes.Black, x, y);
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;
            e.Graphics.DrawString("送回时间：" + subrow[4], printFont, Brushes.Black, x, y);

            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("任务编号：" + subrow[6], printFont, Brushes.Black, x, y);

            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("名称：" + subrow[7], printFont, Brushes.Black, x, y);

            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("规格或图号：" + subrow[8], printFont, Brushes.Black, x, y);




            printFont = new Font("Times New Roman", (float)20, FontStyle.Regular, GraphicsUnit.Point);
            e.Graphics.DrawString("×" + subrow[9], printFont, Brushes.Black, 125, y - 20);

            printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("备注:" + subrow[10], printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
            y += lineOffset;
            x = 0;
            e.Graphics.DrawString("----------------------------------------------", printFont, Brushes.Black, x, y);




            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("订制车间", printFont, Brushes.Black, x, y);
            printFont = new Font("宋体", (float)10, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("调度组长：         检验组长:", printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset + 20;
            e.Graphics.DrawString("执行车间", printFont, Brushes.Black, x, y);
            printFont = new Font("宋体", (float)10, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("调度组长：         检验组长:", printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)7, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            y += lineOffset + 40;
            e.Graphics.DrawString("                                    (请签字)", printFont, Brushes.Black, x, y);
            e.HasMorePages = false;
        }
        /// <summary>
        /// 生成打印用的数据串
        /// </summary>
        /// <param name="DingZhiID"></param>
        /// <returns></returns>
        protected string GetPrintStr(string DingZhiID)
        {
            DataTable tempDT;
            DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = "Select * From [ProductBarCodeManagementAndTrack].[dbo].[User_DingZhiRecord] Where ID = '" + DingZhiID + "'";
                tempDT = DBI.Execute(strSQL, true);
            }
            catch (Exception e)
            {
                throw new Exception("获取报表数据源时出现异常" + e.Message.ToString());
            }
            int ID = string.IsNullOrEmpty(tempDT.Rows[0]["ID"].ToString()) ? 0 : Convert.ToInt32(tempDT.Rows[0]["ID"]);
            string Barcode = tempDT.Rows[0]["Barcode"].ToString();
            string CreateDept = string.IsNullOrEmpty(tempDT.Rows[0]["OrderWorkShop"].ToString()) ? null : Common.GetDeptCodeByCode(tempDT.Rows[0]["OrderWorkShop"].ToString());
            string ReceiveWorkShop = string.IsNullOrEmpty(tempDT.Rows[0]["DisposeWorkShop"].ToString()) ? null : Common.GetDeptCodeByCode(tempDT.Rows[0]["DisposeWorkShop"].ToString());
            string TaskNum = tempDT.Rows[0]["TaskNum"].ToString().Trim('\n').Trim('\r');
            string ProductName = tempDT.Rows[0]["ProductName"].ToString().Trim('\n').Trim('\r');
            string DrawingNum = tempDT.Rows[0]["DrawingNum"].ToString().Trim('\n').Trim('\r');
            string ProductAmount = tempDT.Rows[0]["Amount"].ToString().Trim('\n').Trim('\r');
            string DZProperty = tempDT.Rows[0]["DingZhiXingZhi"].ToString().Trim('\n').Trim('\r');
            string CreateDate = tempDT.Rows[0]["OrderDate"].ToString().Trim('\n').Trim('\r');
            string BackDate = tempDT.Rows[0]["AppointDate"].ToString().Trim('\n').Trim('\r');
            string Remark = tempDT.Rows[0]["Remark"].ToString();

            string finishStr = ID.ToString("000000") + "DZ=" + Barcode + "$" +
                                CreateDept + "$" +
                                ReceiveWorkShop + "$" +
                                CreateDate + "$" +
                                BackDate + "$" +
                                DZProperty + "$" +
                                TaskNum + "$" +
                                ProductName + "$" +
                                DrawingNum + "$" +
                                ProductAmount + "$" +
                                Remark;
            return finishStr;
        }
        #endregion

        #region 交接单打印
        /// <summary>
        /// 交接单打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pdPrint_PrintPageJJD(object sender, PrintPageEventArgs e)
        {
            float x = 0, y = 0, lineOffset = 0;
            Font printFont = new Font("宋体", (float)20, FontStyle.Bold, GraphicsUnit.Point);
            DataTable MainDt = GetJiaoJieDanMain(MainID);
            DataTable InfoDt = GetJiaoJieDanInfo(MainID);

            string JJDID = Convert.ToInt32(MainID).ToString("000000") + "JJ=";

            // Instantiate font objects used in printing.  

            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point);

            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;

            e.Graphics.DrawString("天津航天长征火箭制造有限公司", printFont, Brushes.Black, x, y);


            printFont = new Font("宋体", (float)20, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            e.Graphics.PageUnit = GraphicsUnit.Point;
            x = 30;
            y += +lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5;
            e.Graphics.DrawString("交接单", printFont, Brushes.Black, x, y);



            //编号0$制造车间1$接收车间2$创建时间3;任务号0$名称1$图号2$数量3$备注4;任务号$名称$图号$数量$备注
            //000023JJ=0$工艺技术处$55车间$2015年04月22日;47AS04-C-0002$测试产品1$CG1113-354$41$备注1;47AS04-C-0003$测试产品2$CG1113-355$10$备注2;

            QRCodeEncoder encoder = new QRCodeEncoder();
            QRCodeDecoder decoder = new QRCodeDecoder();
            encoder.QRCodeVersion = 0;
            encoder.QRCodeScale = 2;
            Bitmap bb = encoder.Encode(JJDID);
            Image barc = bb;
            x = 130;
            y = 20;
            e.Graphics.DrawImage(barc, x, y);

            printFont = new Font("宋体", (float)12, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += 20 + lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5 + 5;
            e.Graphics.DrawString("制造车间：" + MainDt.Rows[0]["MakeWorkShop"].ToString(), printFont, Brushes.Black, x, y);

            y += lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5 + 5;
            e.Graphics.DrawString("接收车间：" + MainDt.Rows[0]["ReceiveWorkShop"].ToString(), printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            lineOffset = printFont.GetHeight(e.Graphics) - (float)3.5;
            e.Graphics.DrawString("编号:" + JJDID, printFont, Brushes.Black, x, y);

            //printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;
            e.Graphics.DrawString("日期：" + MainDt.Rows[0]["CreatedTime"].ToString(), printFont, Brushes.Black, x, y);


            //printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;
            y += lineOffset;

            e.Graphics.DrawString("----------------------------------------------", printFont, Brushes.Black, x, y);
            // Print the receipt text


            for (int i = 0; i < InfoDt.Rows.Count; i++)
            {
                printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
                lineOffset = printFont.GetHeight(e.Graphics) + (float)3.5;


                //编号0$制造车间1$接收车间2$创建时间3;任务号0$名称1$图号2$数量3$备注4$条码号5;任务号$名称$图号$数量$备注
                //000023JJ=$工艺技术处$55车间$2015年04月22日;47AS04-C-0002$测试产品1$CG1113-354$41$备注$ZC0470140010000000271;47AS04-C-0003$测试产品2$CG1113-355$10$备注2$ZC0470140010000000271

                x = 0;
                y += lineOffset;
                e.Graphics.DrawString(InfoDt.Rows[i]["TaskNum"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString(InfoDt.Rows[i]["ProductName"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString(InfoDt.Rows[i]["DrawingNum"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString("合格证号:" + InfoDt.Rows[i]["CertificateID"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;
                e.Graphics.DrawString("备注:" + InfoDt.Rows[i]["Remark"].ToString(), printFont, Brushes.Black, x, y);
                y += lineOffset;

              

                e.Graphics.DrawString(InfoDt.Rows[i]["BarCode"].ToString(), printFont, Brushes.Black, x, y);




                printFont = new Font("Times New Roman", (float)20, FontStyle.Regular, GraphicsUnit.Point);
                e.Graphics.DrawString("×" + InfoDt.Rows[i]["ProductAmount"].ToString(), printFont, Brushes.Black, 135, y - 40);

                printFont = new Font("宋体", (float)10, FontStyle.Regular, GraphicsUnit.Point);
                y += lineOffset;
                x = 0;
                e.Graphics.DrawString("----------------------------------------------", printFont, Brushes.Black, x, y);

            }



            printFont = new Font("宋体", (float)10, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            x = 0;
            y += lineOffset;
            e.Graphics.DrawString("移交负责人：     接收负责人:", printFont, Brushes.Black, x, y);

            printFont = new Font("宋体", (float)7, FontStyle.Bold, GraphicsUnit.Point); // Substituted to FontA Font
            y += lineOffset + 40;
            e.Graphics.DrawString("                                    (请签字)", printFont, Brushes.Black, x, y);
            e.HasMorePages = false;
        }
        /// <summary>
        /// 获取交接单头
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        protected DataTable GetJiaoJieDanMain(string ID)
        {
            DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT ID,CreatedTime,
                            (Select DeptName From dbo.DeptEnum Where DeptCode = a.ReceiveWorkShop) as ReceiveWorkShop,
                            (Select DeptName From dbo.DeptEnum Where DeptCode = a.MakeWorkShop) as MakeWorkShop
                            FROM User_JiaoJieDan_Main a Where ID = '" + ID + "'";
                DataTable DT = DBI.Execute(strSQL, true);

                return DT;
            }
            catch (Exception e)
            {
                throw new Exception("获取签收记录子表详情出现异常" + e.Message.ToString());
            }
        }
        /// <summary>
        /// 获取交接单子列表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        protected DataTable GetJiaoJieDanInfo(string ID)
        {
            DBConn = ConfigurationManager.AppSettings["DBBarcodeManagement"].ToString();
            DBI = DBFactory.GetDBInterface(DBConn);
            string strSQL;
            try
            {
                strSQL = @"SELECT * FROM User_JiaoJieDan_Orders  where MainTableID='" + ID + "'";
                DataTable DT = DBI.Execute(strSQL, true);

                return DT;
            }
            catch (Exception e)
            {
                throw new Exception("获取签收记录子表详情出现异常" + e.Message.ToString());
            }
        }
        #endregion
    }

}