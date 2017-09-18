<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="DeptSetAddress.aspx.cs" Inherits="mms.SystemMangement.DeptSetAddress" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>部门配置配送地址</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="HiddenField" type="hidden" value="系统管理-->部门配置配送地址" />
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
            <telerik:AjaxSetting AjaxControlID="RadComboBoxDept">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Address" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadButtonSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" Runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <div id="Dept">
        <label>选择部门：</label>
  
        <telerik:RadComboBox ID="RadComboBoxDept" runat="server" Culture="zh-CN" DataSourceID="SqlDataSourceDept"
             DataTextField="Dept" DataValueField="ID" OnSelectedIndexChanged="RadComboBoxDept_SelectedIndexChanged"
             AutoPostBack="true" EmptyMessage="请选择部门" AllowCustomText="true">

        </telerik:RadComboBox>
    </div>
    <div id="Address" runat="server">
        <label>选择地址：</label>
        <telerik:RadComboBox ID="RadComboBoxDict" runat="server" Culture="zh-CN" DataSourceID="SqlDataSourceDict"
             DataTextField="KeyWord" DataValueField="AddrId" EmptyMessage="请选择配送地址" AllowCustomText="true">

        </telerik:RadComboBox>
    </div>
    <asp:SqlDataSource ID="SqlDataSourceDept" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="select * from Sys_DeptEnum where Is_Shipping='True'"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceDict" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="select CONVERT(nvarchar, TypeID) + '-' + CONVERT(nvarchar, KeyWordCode) as AddrId, KeyWord from Sys_Dict where TypeID='2'"></asp:SqlDataSource>
    <telerik:RadButton ID="RadButtonSave" runat="server" Text="保存" OnClick="RadButtonSave_Click"></telerik:RadButton>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
    </asp:Content>
