<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="LingJianManage.aspx.cs" Inherits="mms.SystemMangement.LingJianManage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->零件类型信息管理" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridLingJian">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridLingJian" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <input type="hidden" value="系统管理-->零件信息管理" />

    <telerik:RadGrid ID="RadGridLingJian" runat="server" AllowPaging="True" GroupPanelPosition="Top"
        OnNeedDataSource="RadGridLingJian_NeedDataSource"
        OnItemCommand="RadGridLingJian_ItemCommand">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <ItemStyle HorizontalAlign="Center" />
                        <AlternatingItemStyle HorizontalAlign="Center" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
                        </ExportSettings>
        <MasterTableView AutoGenerateColumns="False" DataKeyNames="Id" CommandItemDisplay="Top" EditMode="InPlace">
           <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
           <CommandItemTemplate>
                <telerik:RadButton ID="RadButton_AddNew" runat="server" Text="新建零件类型信息" Font-Bold="true" CommandName="InitInsert" CssClass="floatleft"></telerik:RadButton>
                零件类型信息列表
                <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
            </CommandItemTemplate>
            <Columns>
                <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ReadOnly="True" ></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="LingJian_Type_Code"  HeaderText="零件类型编号" ></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="LingJian_Type_Name"  HeaderText="零件类型名称"></telerik:GridBoundColumn>
                <telerik:GridCheckBoxColumn DataField="Is_Bom_Show"  HeaderText="是否在BOM显示"></telerik:GridCheckBoxColumn>
                <telerik:GridCheckBoxColumn DataField="Is_MDDLD_Show" HeaderText="是否取物资信息"></telerik:GridCheckBoxColumn>
                <telerik:GridCheckBoxColumn DataField="Is_Del" HeaderText="是否可用"></telerik:GridCheckBoxColumn>
                <telerik:GridEditCommandColumn ButtonType="LinkButton" HeaderText="操作" 
                    HeaderStyle-Width="100px" EditText="编辑" InsertText="新增" CancelText="取消" UpdateText="修改">
                </telerik:GridEditCommandColumn>
            </Columns>
         </MasterTableView>
    </telerik:RadGrid>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Position="Center"
        AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
</asp:Content>
