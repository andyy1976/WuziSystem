<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="BannerManage.aspx.cs" Inherits="mms.SystemMangement.BannerManage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 1200px; margin:0px auto;">
        <div style="width: 1200px; float: left;">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGridBannerManage">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGridBannerManage" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadGrid ID="RadGridBannerManage" runat="server" AllowPaging="True" AutoGenerateColumns="False" Culture="zh-CN" GroupPanelPosition="Top" OnNeedDataSource="RadGridBannerManage_NeedDataSource"
         OnInsertCommand="RadGridBannerManage_InsertCommand" OnUpdateCommand="RadGridBannerManage_ItemUpdated" OnItemDataBound="RadGridBannerManage_ItemDataBound" OnItemCreated="RadGridBannerManage_ItemCreated">
        <MasterTableView DataKeyNames="ID" CommandItemDisplay="Top" >
            <Columns>
                <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="序号" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ParentId" DataType="System.Int32" FilterControlAltText="Filter ParentId column" HeaderText="父ID" SortExpression="ParentId" UniqueName="ParentId">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column" HeaderText="菜单名称" SortExpression="ItemName" UniqueName="ItemName">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ItemUrl" FilterControlAltText="Filter ItemUrl column" HeaderText="菜单路径" SortExpression="ItemUrl" UniqueName="ItemUrl">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PermissionId" DataType="System.Int32" FilterControlAltText="Filter PermissionId column" HeaderText="权限" SortExpression="PermissionId" UniqueName="PermissionId">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="OrderId" DataType="System.Int32" FilterControlAltText="Filter OrderId column" HeaderText="排序" SortExpression="OrderId" UniqueName="OrderId">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridCheckBoxColumn DataField="Enable" DataType="System.Boolean" FilterControlAltText="Filter Enable column" HeaderText="是否可用" SortExpression="Enable" UniqueName="Enable">
                </telerik:GridCheckBoxColumn>
                <telerik:GridEditCommandColumn ButtonType="LinkButton" HeaderText="编辑" UniqueName="EditCommandColumn"
                     HeaderStyle-Width="40px" EditText="编辑">
                    <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                </telerik:GridEditCommandColumn>
            </Columns>
            <EditFormSettings ColumnNumber="4" CaptionDataField="ItemName" CaptionFormatString="正在编辑菜单： {0}"
                InsertCaption="添加菜单" FormTableItemStyle-Height="30px" FormTableAlternatingItemStyle-Height="30px">
                <FormTableItemStyle Wrap="true"></FormTableItemStyle>
                <FormCaptionStyle CssClass="EditFormHeader" HorizontalAlign="Center"></FormCaptionStyle>
                <FormMainTableStyle GridLines="None" CellSpacing="0" CellPadding="0" BackColor="White"
                    Width="100%" />
                <FormTableStyle CellSpacing="0" CellPadding="0" BackColor="White" HorizontalAlign="Center" />
                <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>
                <EditColumn ButtonType="PushButton" InsertText="新增" UpdateText=" 修改 " CancelText=" 取消 ">
                </EditColumn>
                <FormTableButtonRowStyle HorizontalAlign="Center" Height="40px"></FormTableButtonRowStyle>
            </EditFormSettings>
            <ItemStyle HorizontalAlign="Center"></ItemStyle>

            <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
            <CommandItemTemplate>
                <telerik:RadButton ID="RadButtonAddTask" CssClass="GridCommandButton" runat="server"
                    Text="新建菜单" Font-Bold="true" CommandName="InitInsert">
                </telerik:RadButton>
                <telerik:RadComboBox ID="RadComboBoxBannerItem" runat="server" Culture="zh-CN"
                 EmptyMessage="选择菜单" AllowCustomText="True" Filter="Contains" AutoPostBack="True"
                 DataSourceID="SqlDataSourceBannerItem" DataTextField="ItemName" DataValueField="ID" 
                 OnItemDataBound="RadComboBoxBannerItem_ItemDataBound" 
                 OnSelectedIndexChanged="RadComboBoxBannerItem_SelectedIndexChanged">
                    
                </telerik:RadComboBox>
                
            </CommandItemTemplate>
        </MasterTableView>
    </telerik:RadGrid>
            <asp:SqlDataSource ID="SqlDataSourceBannerItem" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="SELECT [ID], [ItemName] FROM [Sys_BannerItem]"></asp:SqlDataSource>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Position="Center"
        AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
        </div>
    </div>
</asp:Content>
