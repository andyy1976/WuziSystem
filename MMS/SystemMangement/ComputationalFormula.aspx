<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="ComputationalFormula.aspx.cs" Inherits="mms.SystemMangement.ComputationalFormula" %>

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
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->夹持量参数管理" ClientIDMode="Static" />
    <div style="width: 100%; margin: 0px auto;">
        <div style="width: 100%; float: left;">
            <div class="divContant">
                <div class="divViewPanel">
                    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
                <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="RadGrid_ComputationalFormula">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ComputationalFormula" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>

                    <telerik:RadGrid ID="RadGrid_ComputationalFormula" runat="server" AllowPaging="True" DataKeyNames="ID" GroupPanelPosition="Top"
                        OnNeedDataSource="RadGrid_ComputationalFormula_NeedDataSource" OnItemCommand="RadGrid_ComputationalFormula_ItemCommand">                       
                        <AlternatingItemStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true">
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                        <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
                        </ExportSettings>
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top" EditMode="InPlace">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                                夹持量参数列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsID" HeaderText="序号" ReadOnly="True" ItemStyle-Width="40px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Remark" HeaderText="说明" ReadOnly="True" ItemStyle-Width="400px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Parameter1" HeaderText="参考直径" ItemStyle-Width="160px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Parameter2" HeaderText="参考长度" ItemStyle-Width="160px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Parameter3" HeaderText="夹持量" ItemStyle-Width="160px"></telerik:GridBoundColumn>
                                <telerik:GridEditCommandColumn ButtonType="LinkButton" HeaderText="编辑" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" UpdateText="修改"></telerik:GridEditCommandColumn>                               
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    </div>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
</asp:Content>
