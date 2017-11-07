<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="ReturnApply.aspx.cs" Inherits="mms.OutOfStorageManagement.ReturnApply" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0)
			{

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="出库单管理-->退库" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
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
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_AllReturnApply">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var SigeConfrimID;
            function ShowConfrim(sender, args) {
                var grid = $find("<%=RadGrid1.ClientID%>");
                if (grid._selectedIndexes.length == 0) {
                    $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择退库行");
                    $find("<%=RadNotificationAlert.ClientID%>").show();
                    return;
                }

                $find("<%= RadWindowConfirm.ClientID %>").show();
                SigeConfrimID = sender.get_id();
                args.set_cancel(true);
            }
            function YesOrNoClicked(sender, args) {
                var oWnd = $find("<%=RadWindowConfirm.ClientID %>");
                if (sender.get_text() == "确定退库") {

                    var ReturnApplyuser = $find("<%=RTB_ReturnApplyUser.ClientID%>");
                    var returnnote = $find("<%=RTB_returnnote.ClientID %>");
                    var nnum = $find("<%=RTB_nnum.ClientID %>");
                    var itemnum = $find("<%=RTB_itemnum.ClientID %>");

                    if (ReturnApplyuser._text == "") {
                        $find("<%=RadNotificationAlert.ClientID%>").set_text("请输入退库人姓名");
                        $find("<%=RadNotificationAlert.ClientID%>").show();
                        return;
                    }
                    if (returnnote._text == "") {
                        $find("<%=RadNotificationAlert.ClientID%>").set_text("请输入退库原因");
                        $find("<%=RadNotificationAlert.ClientID%>").show();
                        return;
                    }
                    if (nnum._text == "") {
                        $find("<%=RadNotificationAlert.ClientID%>").set_text("请输入退库数量");
                        $find("<%=RadNotificationAlert.ClientID%>").show();
                        return;
                    }
                    if (itemnum._text == "") {
                        $find("<%=RadNotificationAlert.ClientID%>").set_text("请输入退库件数");
                        $find("<%=RadNotificationAlert.ClientID%>").show();
                        return;
                    }
                    oWnd.close();
                    $find(SigeConfrimID).click();
                } else {
                    oWnd.close();
                }
            }

        </script>
    </telerik:RadCodeBlock>
    <div style="width: 100%; margin: 0px auto;">
        <table style="width: 100%; float: left; text-align: left;">
            <tr>
                <td style="text-align: right;">出库单号：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_billno" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td style="text-align: right;">单据日期：</td>
                <td>
                    <telerik:RadDatePicker ID="RDP_Startbilldate" runat="server" Width="100px"></telerik:RadDatePicker>
                </td>
                <td>～</td>
                <td>
                    <telerik:RadDatePicker ID="RDP_Endbilldate" runat="server" Width="120px"></telerik:RadDatePicker>
                </td>
                <td>物资编码：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_invcode" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td style="text-align: right;">物资名称：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_invname" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td>退库状态：</td>
                <td>
                    <telerik:RadDropDownList ID="RDDL_ReturnApplyState" runat="server">
                        <Items>
                            <telerik:DropDownListItem Value="0" Text="全部" />
                            <telerik:DropDownListItem Value="1" Text="未退库" Selected="true" />
                            <telerik:DropDownListItem Value="2" Text="已退库" />
                        </Items>
                    </telerik:RadDropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">合格证号：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_hgz_no" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td style="text-align: right;">任务号：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_jc_rwh" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td style="text-align: right;">图号：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_jc_th" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td style="text-align: right;">质检单号：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_hgz_zjdbillno" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td style="text-align: right;">质疑单号：</td>
                <td>
                    <telerik:RadTextBox ID="RTB_hgz_zydh" runat="server" Width="100px"></telerik:RadTextBox></td>
                <td>
                    <telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%; margin: 0px auto;">
        <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True"
            OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">
            <AlternatingItemStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
            <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
            <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="6" ScrollHeight="600px"></Scrolling>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
 			<ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID">
	        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                <CommandItemTemplate>
                    <telerik:RadButton ID="RB_ReturnApplyConfrim" runat="server" Text="退库" CommandName="ReturnApply" OnClientClicking="ShowConfrim" CssClass="floatleft"></telerik:RadButton>
                    物流中心出库单列表
 					<telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                    <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                    <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                    <%--<telerik:GridTemplateColumn HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <HeaderTemplate>
                            <telerik:RadButton ID="RB_AllReturnApply" runat="server" OnCheckedChanged="RB_AllReturnApply_CheckedChanged" ButtonType="ToggleButton" ToggleType="CheckBox" Skin="Default"></telerik:RadButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadButton ID="RB_ReturnApply" runat="server" OnCheckedChanged="RB_ReturnApply_CheckedChanged" ButtonType="ToggleButton" ToggleType="CheckBox" Skin="Default"></telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="退库原因"  ItemStyle-Width="100px" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <telerik:RadTextBox ID="RTB_returnnote" runat="server" Width="90px" OnTextChanged="RTB_returnnote_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="退回数量"  ItemStyle-Width="100px" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <telerik:RadTextBox ID="RTB_nnum" runat="server" Width="90px" OnTextChanged="RTB_nnum_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="退回件数"  ItemStyle-Width="100px" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <telerik:RadTextBox ID="RTB_itemnum" runat="server" Width="90px" OnTextChanged="RTB_itemnum_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridBoundColumn DataField="crowno" HeaderText="出库单<br />行号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="billno" HeaderText="出库<br />单号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="billdate" HeaderText="单据<br />日期" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CanReturnnnum" HeaderText="可退<br />数量" ItemStyle-Width="100px" HeaderStyle-Width="100px" UniqueName="CanReturnnnum"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CanReturnitemnum" HeaderText="可退<br />件数" ItemStyle-Width="100px" HeaderStyle-Width="100px" UniqueName="CanReturnitemnum"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_jstype1" HeaderText="结算<br />类型" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="realnum" HeaderText="实发<br />数量" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="realdepnum" HeaderText="实发<br />辅数量" ItemStyle-Width="100px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="realitemnum" HeaderText="实发<br />件数" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="curators" HeaderText="保管员" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="outstock" HeaderText="出库仓库" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="planner" HeaderText="计划员" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="replacecorp" HeaderText="领用<br />单位" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="replaceuser" HeaderText="领用人" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="accountcorp" HeaderText="客户<br />单位" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="accountmode" HeaderText="结算<br />方式" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="distraddress" HeaderText="配送地" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_jyy" HeaderText="检验员" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_zzr" HeaderText="制单人" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="vnote" HeaderText="单据<br />头备注" ItemStyle-Width="160px" HeaderStyle-Width="160px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_jjcd" HeaderText="紧急<br />程度" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_phone" HeaderText="联系人/<br />联系电话" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="cgeneralbid" HeaderText="出库单<br />表体主键" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invcode" HeaderText="物资编码" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invname" HeaderText="物资名称" ItemStyle-Width="160px" HeaderStyle-Width="160px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invxhcpdh" HeaderText="型（牌）号/<br />产品代号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invscale" HeaderText="规格" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="techconditions" HeaderText="技术<br />条件" ItemStyle-Width="160px" HeaderStyle-Width="160px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invstatus" HeaderText="物资<br />状态" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invlevel" HeaderText="质量<br />等级" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invfurbatch" HeaderText="（炉）批号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invmeasname" HeaderText="计量<br />单位" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invdepmeasname" HeaderText="副计量<br />单位" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invspecial" HeaderText="特殊<br />记载" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="manufacturer" HeaderText="生产<br />厂家" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="manufacturdate" HeaderText="生产<br />日期" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="invbatch" HeaderText="批次号<br />（帐卡号）" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_cc" HeaderText="尺寸" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_no" HeaderText="合格<br />证号" ItemStyle-Width="140px" HeaderStyle-Width="140px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgzbz" HeaderText="单据体<br />备注" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_rwh" HeaderText="任务号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_gcxh" HeaderText="型号<br />工程" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_yzjd" HeaderText="研制<br />阶段" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_pzh" HeaderText="批组号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_use" HeaderText="用途" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_zxgc" HeaderText="专项<br />工程" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_price" HeaderText="出库<br />单价" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_money" HeaderText="出库<br />金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_invcl" HeaderText="存货<br />分类" ItemStyle-Width="160px" HeaderStyle-Width="160px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_th" HeaderText="图号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_ckjl" HeaderText="出库v结论" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_wzms" HeaderText="物资<br />描述" ItemStyle-Width="300px" HeaderStyle-Width="300px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jc_wzbs" HeaderText="物资<br />标识" ItemStyle-Width="260px" HeaderStyle-Width="260px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="dpaconclusion" HeaderText="DPA结论" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="specialinstr" HeaderText="特殊<br />说明" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ysconclusion" HeaderText="验收结论<br />（入库结论）" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_cqyslb" HeaderText="超期验收<br />类别" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_zjdbillno" HeaderText="质检<br />单号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_fyyxqf" HeaderText="复验有效<br />期自" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_fyyxqz" HeaderText="复验有效<br />期止" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_yxqx" HeaderText="存储<br />期止" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_zydh" HeaderText="质疑<br />单号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_ysxs" HeaderText="验收<br />形式" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_hjxjl" HeaderText="焊接性<br />结论" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="hgz_sqjyjl" HeaderText="水汽检验<br />结论" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nc_kth" HeaderText="课题号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rowstatus" HeaderText="行状态" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="isprint" HeaderText="是否打印<br />合格证" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_no" HeaderText="需求申请<br />编号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_revision" HeaderText="需求申请<br />版本号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="user_rq_no" HeaderText="用户需求<br />编号" ItemStyle-Width="160px" HeaderStyle-Width="160px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_id" HeaderText="需求系统<br />需求ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="securitylevel" HeaderText="密级" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="lastupdate" HeaderText="最后<br />更新<br />时间" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="khbm_info" HeaderText="客户<br />部门" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="istrans" HeaderText="是否<br />配送" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="iscutting" HeaderText="是否<br />下料" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="transdate" HeaderText="配送<br />时限" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="old_generalbid" HeaderText="原出库单<br />行ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="old_generalhid" HeaderText="原出库单<br />头ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_headid" HeaderText="需求<br />头ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="rq_bodyid" HeaderText="需求<br />行ID" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="request_sn_id" HeaderText="领料单<br />标识" ItemStyle-Width="160px" HeaderStyle-Width="160px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="request_sn" HeaderText="领料单<br />头标识" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="dhprice" HeaderText="到货<br />价" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jcbl" HeaderText="加成<br />比例" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jyfft" HeaderText="检验费<br />分摊" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="jyxhft" HeaderText="检验消耗<br />分摊" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="startdate" HeaderText="滞纳金<br />起算日期" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="npaidmny" HeaderText="累计实收<br />金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nbalancemny" HeaderText="剩余应收<br />金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nprepaidoffsetmny" HeaderText="预付冲抵<br />金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nshouldpaymny" HeaderText="应收<br />金额" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="lldsqdept" HeaderText="申请<br />部门" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="bathnewinvcode" HeaderText="存帐卡对应的<br />新物质编码" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="httsyq" HeaderText="合同特殊<br />要求" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nbReturnApplynum" HeaderText="签收<br />数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <%--签收确定--窗口--%>
    <telerik:RadWindow ID="RadWindowConfirm" runat="server" VisibleTitlebar="false"
        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="400px" Width="800px">
        <ContentTemplate>
            <div style="width:100%; margin-top: 30px; float: left;">
                <table style="margin: 0px auto; width: 760px; height: 300px;">
                    <tr>
                        <th colspan="6" style="text-align:center; font-size:14px;">退库确认</th>
                    </tr>
                    <tr>
                        <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc;">退库信息</th>
                    </tr>
                    <tr>
                        <td style="width: 100px; text-align: right;">退库人：</td>
                        <td style="width: 140px;">
                            <telerik:RadTextBox ID="RTB_ReturnApplyUser" runat="server" Width="120px"></telerik:RadTextBox></td>
                        <td style="text-align: right; width: 100px;">退库时间：</td>
                        <td style="width: 140px;">
                            <telerik:RadDatePicker ID="RDP_ApplicationTime" runat="server" Width="120px"></telerik:RadDatePicker>
                        </td>
                        <td style="text-align: right; width: 100px;">联系方式：</td>
                        <td style="width: 140px;">
                            <telerik:RadTextBox ID="RTB_ContactInformation" runat="server" Width="120px"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 100px; text-align: right;">退库原因：</td>
                        <td>
                            <telerik:RadTextBox ID="RTB_returnnote" runat="server" Width="120px"></telerik:RadTextBox></td>

                        <td style="width: 100px; text-align: right;">退库数量：</td>
                        <td>
                            <telerik:RadTextBox ID="RTB_nnum" runat="server" Width="120px"></telerik:RadTextBox></td>

                        <td style="width: 100px; text-align: right;">退回件数：</td>
                        <td>
                            <telerik:RadTextBox ID="RTB_itemnum" runat="server" Width="120px"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc;">业务审批流程</th>
                    </tr>
                    <tr>
                        <td style="text-align: right;">1.车间调度员：</td>
                        <td>
                            <telerik:RadDropDownList runat="server" ID="RDDL_DiaoDu" Width="120px"></telerik:RadDropDownList>
                        </td>
                        <td style="text-align: right;">2.型号计划员：</td>
                        <td>
                            <telerik:RadDropDownList runat="server" ID="RDDL_XingHao" Width="120px"></telerik:RadDropDownList>
                        </td>
                        <td style="text-align: right;">3.物资计划员：</td>
                        <td>
                            <telerik:RadDropDownList runat="server" ID="RDDL_WuZi" Width="120px"></telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: center;">
                            <telerik:RadButton ID="RadButton3" runat="server" Text="确定退库" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                                <Icon PrimaryIconCssClass="rbOk" />
                            </telerik:RadButton>
                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                            <telerik:RadButton ID="RadButton4" runat="server" Text="取消" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                                <Icon PrimaryIconCssClass="rbCancel" />
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <%--签收确定--窗口--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowApp" runat="server" Title="退库" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="1000px" Height="680px" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
</asp:Content>
