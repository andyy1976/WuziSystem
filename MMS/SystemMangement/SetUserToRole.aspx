<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="SetUserToRole.aspx.cs" Inherits="mms.SystemMangement.SetUserToRole" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 1200px; margin: 0px auto;">
        <div style="width: 1200px; float: left;">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                 <%--<AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="rcob_user">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="rcob_role" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>--%>
            </telerik:RadAjaxManager>
            <telerik:RadComboBox ID="rcob_user" runat="server" Culture="zh-CN" AllowCustomText="true"
                 DataSourceID="SqlDataSourceUser" DataTextField="UserName" DataValueField="ID"
                 EmptyMessage="选择用户" Filter="Contains" AutoPostBack="True" OnSelectedIndexChanged="rcob_user_SelectedIndexChanged">

            </telerik:RadComboBox>
            <asp:SqlDataSource runat="server" ID="SqlDataSourceUser" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>' SelectCommand="SELECT [ID], [UserName] FROM [Sys_UserInfo]"></asp:SqlDataSource>
            <div id="Action" runat="server" pid="">
                <telerik:RadComboBox ID="rcob_role" runat="server" Culture="zh-CN" DataSourceID="SqlDataSourceRole" DataTextField="RoleName"
                     DataValueField="ID" EmptyMessage="选择角色"></telerik:RadComboBox>
                <asp:SqlDataSource ID="SqlDataSourceRole" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="SELECT [ID], [RoleName] FROM [Sys_RoleInfo]"></asp:SqlDataSource>
                <telerik:RadButton ID="rbtn_save" runat="server" Text="保 存" OnClick="rbtn_save_Click"></telerik:RadButton>
            </div>
        </div>
        <telerik:RadNotification ID="RadNotificationAlert" Position="Center" runat="server"></telerik:RadNotification>
    </div>
</asp:Content>

