﻿<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="LogisticsCenterLibrary.aspx.cs" Inherits="mms.OutOfStorageManagement.LogisticsCenterLibrary" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function EnterKeyProcessing(sender, eventArgs) {
            var c = eventArgs.get_keyCode();
            if ((c == 13)) {
                eventArgs.set_cancel(true);
            }
        }

        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0)
			{

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资查询-->物流中心出库单" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
	</telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
             <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RB_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"></telerik:RadCodeBlock>
    <div style="width: 100%; margin: 0px auto;">
        <table style="width:100%; float:left; text-align:left;">
            <tr>
                <td style="text-align:right;">出库单号：</td>
                <td><telerik:RadTextBox ID="RTB_billno" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td style="text-align:right;">单据日期：</td>
                <td>
                    <telerik:RadDatePicker ID="RDP_Startbilldate" runat="server" Width="100px"  DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                    </telerik:RadDatePicker>
                </td>
                <td>～</td>
                <td>
                    <telerik:RadDatePicker ID="RDP_Endbilldate" runat="server" Width="120px"  DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                    </telerik:RadDatePicker>
                </td>
                <td>物资编码：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_invcode" runat="server" Width="100px">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                    </telerik:RadTextBox></td>
                <td style="text-align:right;">物资名称：</td>
                <td><telerik:RadTextBox ID="RTB_invname" runat="server" Width="100px">
                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
                    </telerik:RadTextBox></td>
            </tr>
            <tr>                
                <td style="text-align:right;">合格证号：</td>
                <td><telerik:RadTextBox ID="RTB_hgz_no" runat="server" Width="100px">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                    </telerik:RadTextBox></td>
                <td style="text-align:right;">任务号：</td>
                <td><telerik:RadTextBox ID="RTB_jc_rwh" runat="server" Width="100px">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                    </telerik:RadTextBox></td>
                <td style="text-align:right;">图号：</td>
                <td><telerik:RadTextBox ID="RTB_jc_th" runat="server" Width="100px">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                    </telerik:RadTextBox></td>
                <td style="text-align:right;">质检单号：</td>
                <td><telerik:RadTextBox ID="RTB_hgz_zjdbillno" runat="server" Width="100px">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                    </telerik:RadTextBox></td>
                <td style="text-align:right;">质疑单号：</td>
                <td><telerik:RadTextBox ID="RTB_hgz_zydh" runat="server" Width="100px">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                    </telerik:RadTextBox></td>
                <td><telerik:RadButton ID="RB_Search" runat="server" Text ="搜索" OnClick="RB_Search_Click"></telerik:RadButton></td>
            </tr>
        </table>
    </div>
    <div style="width: 100%; margin: 0px auto;">
        <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" PagerStyle-AlwaysVisible="True"
            OnNeedDataSource="RadGrid1_NeedDataSource">
            <AlternatingItemStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
            <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
            <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
             <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                          <MasterTableView  CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
						    物流中心出库单列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn DataField="billno" HeaderText="出库单号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="billdate" HeaderText="单据日期" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_jstype1" HeaderText="结算类型1" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="curators" HeaderText="保管员" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="outstock" HeaderText="出库仓库" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="planner" HeaderText="计划员" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="replacecorp" HeaderText="领用单位" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="replaceuser" HeaderText="领用人" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="accountcorp" HeaderText="客户单位" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="accountmode" HeaderText="结算方式" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="distraddress" HeaderText="配送地" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_jyy" HeaderText="检验员" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_zzr" HeaderText="制单人" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="vnote" HeaderText="单据头备注" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_jjcd" HeaderText="紧急程度" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_phone" HeaderText="联系人/联系电话" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="cgeneralbid" HeaderText="出库单表体主键" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invcode" HeaderText="物资编码" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invname" HeaderText="物资名称" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invxhcpdh" HeaderText="型（牌）号/产品代号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invscale" HeaderText="规格" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="techconditions" HeaderText="技术条件" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invstatus" HeaderText="物资状态" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invlevel" HeaderText="质量等级" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invfurbatch" HeaderText="（炉）批号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invmeasname" HeaderText="计量单位" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invdepmeasname" HeaderText="副计量单位" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invspecial" HeaderText="特殊记载" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="manufacturer" HeaderText="生产厂家" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="manufacturdate" HeaderText="生产日期" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invbatch" HeaderText="批次号（帐卡号）" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="realnum" HeaderText="实发数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="realdepnum" HeaderText="实发辅数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="realitemnum" HeaderText="实发件数" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_cc" HeaderText="需求尺寸" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_no" HeaderText="合格证号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgzbz" HeaderText="单据体备注" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_rwh" HeaderText="任务号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_gcxh" HeaderText="型号工程" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_yzjd" HeaderText="研制阶段" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_pzh" HeaderText="批组号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_use" HeaderText="用途" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_zxgc" HeaderText="专项工程" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_price" HeaderText="出库单价" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_money" HeaderText="出库金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_invcl" HeaderText="存货分类" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_th" HeaderText="图号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_ckjl" HeaderText="出库结论" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_wzms" HeaderText="物资描述" ItemStyle-Width="300px" HeaderStyle-Width="300px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_wzbs" HeaderText="物资标识" ItemStyle-Width="300px" HeaderStyle-Width="300px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="dpaconclusion" HeaderText="DPA结论" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="specialinstr" HeaderText="特殊说明" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ysconclusion" HeaderText="验收结论（入库结论）" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_cqyslb" HeaderText="超期验收类别" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_zjdbillno" HeaderText="质检单号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_fyyxqf" HeaderText="复验有效期自" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_fyyxqz" HeaderText="复验有效期止" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_yxqx" HeaderText="存储期止" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_zydh" HeaderText="质疑单号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_ysxs" HeaderText="验收形式" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_hjxjl" HeaderText="焊接性结论" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_sqjyjl" HeaderText="水汽检验结论" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nc_kth" HeaderText="课题号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rowstatus" HeaderText="行状态" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="isprint" HeaderText="是否打印合格证" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_no" HeaderText="需求申请编号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_revision" HeaderText="需求申请版本号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="user_rq_no" HeaderText="用户需求编号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_id" HeaderText="需求系统需求ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="securitylevel" HeaderText="密级" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="lastupdate" HeaderText="最后更新时间" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="khbm_info" HeaderText="客户部门" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="istrans" HeaderText="是否配送" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="iscutting" HeaderText="是否下料" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="transdate" HeaderText="配送时限" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="old_generalbid" HeaderText="原出库单行ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="old_generalhid" HeaderText="原出库单头ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_headid" HeaderText="需求头ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_bodyid" HeaderText="需求行ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="crowno" HeaderText="出库单行号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="request_sn_id" HeaderText="领料单标识" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="request_sn" HeaderText="领料单头标识" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="dhprice" HeaderText="到货价" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jcbl" HeaderText="加成比例" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jyfft" HeaderText="检验费分摊" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jyxhft" HeaderText="检验消耗分摊" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="startdate" HeaderText="滞纳金起算日期" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="npaidmny" HeaderText="累计实收金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nbalancemny" HeaderText="剩余应收金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nprepaidoffsetmny" HeaderText="预付冲抵金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nshouldpaymny" HeaderText="应收金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="lldsqdept" HeaderText="申请部门" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="bathnewinvcode" HeaderText="存帐卡对应的新物质编码" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="httsyq" HeaderText="合同特殊要求" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nbsignnum" HeaderText="签收数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
</asp:Content>
