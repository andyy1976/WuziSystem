<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="SetRoleToPermission.aspx.cs" Inherits="mms.SystemMangement.SetRoleToPermission" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .divControls {
            margin: 20px 0;
        }
    </style>
    <script>

        //删除确认窗口结束
        function OpenPermissionWindow() {
            window.radopen("WinPage/WinPermission.aspx", "RadWindowWinPermission");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->角色分配权限" ClientIDMode="Static" />
    <div style="width: 100%; margin: 0px auto;">
        <div style="width: 100%; float: left;">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            </telerik:RadAjaxManager>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                <%-- <telerik:RadButton ID="RadButtonSetPermission" runat="server" Text="配置权限" AutoPostBack="false" OnClientClicked="OpenPermissionWindow"></telerik:RadButton>--%>
                <div class="divTabPanel">
                    <telerik:RadTabStrip ID="RadTabStripRoles" runat="server" OnTabClick="RadTabStripRoles_TabClick" Skin="Default">
                    </telerik:RadTabStrip>
                </div>

                <div class="divSourceList">
                    <telerik:RadTreeList ID="RadTreeListSource" runat="server" DataKeyNames="ID" ParentDataKeyNames="ParentId"
                        AutoGenerateColumns="False" AllowMultiItemSelection="True" OnNeedDataSource="RadTreeListSource_NeedDataSource"
                        OnItemDataBound="RadTreeListSource_ItemDataBound" OnPreRender="RadTreeListSource_PreRender" Culture="zh-CN">
                        <ClientSettings>
                            <Scrolling AllowScroll="true" ScrollHeight="600px" UseStaticHeaders="true" />
                        </ClientSettings>
                        <HeaderStyle Font-Size="13px" />
                        <Columns>
                            <telerik:TreeListSelectColumn HeaderStyle-Width="40px">
                            </telerik:TreeListSelectColumn>
                            <telerik:TreeListBoundColumn DataField="RowsId" HeaderText="序号" UniqueName="RowsId"
                                HeaderStyle-Width="60px" />
                            <telerik:TreeListBoundColumn DataField="SignName" HeaderText="资源名称" UniqueName="SignName" />
                            <telerik:TreeListBoundColumn DataField="PermissionSign" HeaderText="资源标记" UniqueName="PermissionSign" />
                        </Columns>
                    </telerik:RadTreeList>
                </div>
                <div class="divControls">
                    <telerik:RadButton ID="RadButtonSaveConfig" runat="server" Text="保存设置"
                        OnClick="RadButtonSaveConfig_Click">
                    </telerik:RadButton>
                </div>
                <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                    AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
                </telerik:RadNotification>
            </telerik:RadAjaxPanel>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
            </telerik:RadAjaxLoadingPanel>
        </div>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowWinPermission" runat="server" Title="配置权限" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="true" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="680px" Height="450px" />
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>
