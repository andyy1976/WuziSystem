<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MaterialDeptInfoManage.aspx.cs" Inherits="mms.SystemMangement.MaterialDeptInfoManage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->材料部门信息管理" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridMaterialDeptInfo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMaterialDeptInfo" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>

    <telerik:RadGrid ID="RadGridMaterialDeptInfo" runat="server" CellSpacing="-1" Culture="zh-CN" 
        AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True"
         GroupPanelPosition="Top" OnNeedDataSource="RadGridMaterialDeptInfo_NeedDataSource"
         OnItemCommand="RadGridMaterialDeptInfo_ItemCommand">
        <AlternatingItemStyle HorizontalAlign="Center" />
        <ItemStyle HorizontalAlign="Center" />
        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
        <ClientSettings EnableRowHoverStyle="true" >
            <Selecting AllowRowSelect="true" />
            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3" ScrollHeight="600px"></Scrolling>
        </ClientSettings>
                        <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
                        </ExportSettings>
        <MasterTableView AutoGenerateColumns="False" DataKeyNames="Id" CommandItemDisplay="Top" EditMode="InPlace">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
    		 <CommandItemTemplate>
                工艺路线提取领料部门
                <telerik:RadButton ID="RadButton_AddNew" runat="server" Text="新建工艺路线" Font-Bold="true" CommandName="InitInsert" CssClass="floatleft"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
            </CommandItemTemplate>
            <CommandItemStyle HorizontalAlign="Center" Height="36px" Font-Size="16px" Font-Bold="true" />
            <Columns>
                <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ReadOnly="True"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Technics_Line" HeaderText="工艺路线前缀"></telerik:GridBoundColumn>
                <%--<telerik:GridBoundColumn DataField="Technics_Line_Len" HeaderText="前缀长度"></telerik:GridBoundColumn>--%>
                <telerik:GridDropDownColumn DataField="Dept_Id" DataSourceID="SqlDataSourceDept" ListValueField="ID" ListTextField="Dept" HeaderText="领料部门"></telerik:GridDropDownColumn>
                <telerik:GridCheckBoxColumn DataField="Is_Del" HeaderText="是否禁用"></telerik:GridCheckBoxColumn>
                <telerik:GridEditCommandColumn ButtonType="LinkButton" HeaderText="操作" 
                    HeaderStyle-Width="100px" EditText="编辑" UpdateText="修改" InsertText="新增" CancelText="取消">
                </telerik:GridEditCommandColumn>
            </Columns>
               </MasterTableView>
    </telerik:RadGrid>
    <asp:SqlDataSource ID="SqlDataSourceDept" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="SELECT [Dept], [ID] FROM [Sys_DeptEnum] WHERE Is_Shipping='True' and Is_Del = 'false' order by Dept"></asp:SqlDataSource>
     <telerik:RadNotification ID="RadNotificationAlert" runat="server" Position="Center"
        AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
    </asp:Content>
