<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="UserManagePWD.aspx.cs" Inherits="mms.SystemMangement.UserManagePWD" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                args.set_enableAjax(false);

            }
        }
        //<![CDATA[
        //删除确认窗口开始
        var deleteButtonID;
        function CustomRadWindowConfirm(sender, args) {
            var grid = $find("<%=RadGrid_UserManage_PWD.ClientID %>");
            var length = grid._selectedIndexes.length;
            if (length == 0) {
                $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择要删除的行！");
                $find("<%=RadNotificationAlert.ClientID%>").show();
                args.set_cancel(true);
            } else {
                $find("<%=confirmDeleteWindow.ClientID %>").show();
                deleteButtonID = sender.get_id();
                //Cancel the postback
                args.set_cancel(true);
            }
        }
        function YesOrNoClicked(sender, args) {
            var oWnd = $find("<%=confirmDeleteWindow.ClientID %>");
            oWnd.close();
            if (sender.get_text() == "是") {
                $find(deleteButtonID).click();
            }
        }
        //删除确认窗口结束
        function ShowRoleInfoManage() {
            window.radopen("/SystemMangement/RoleInfoManage.aspx", "RadWindowWinRole");
            return false;
        }

        function refreshGrid(arg) {
            if (arg) {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->员工信息管理" ClientIDMode="Static" />
    <div style="width: 100%; margin: 0px auto;">
        <div style="width: 100%; float: left;">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                <ClientEvents OnRequestStart="onRequestStart" />
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadGrid_UserManage_PWD">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_UserManage_PWD" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="RTB_UserName">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_UserManage_PWD" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_UserManage_PWD" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="RCB_Where">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_UserManage_PWD" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
            <div style="width:100%;">
                <table>
                    <tr>
                        <td>姓名或域帐号：</td>
                        <td>
                            <telerik:RadComboBox ID="RCB_Where" runat="server" AppendDataBoundItems="true" Width="300px" Filter="Contains" OnSelectedIndexChanged="RCB_Where_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:RadComboBoxItem Value="" Text="" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </div>
            
            <telerik:RadGrid ID="RadGrid_UserManage_PWD" runat="server" AllowPaging="True" PageSize="20" PagerStyle-AlwaysVisible="True"
                DataKeyNames="ID" Culture="zh-CN"
                GroupPanelPosition="Top" OnNeedDataSource="RadGrid_UserManage_PWD_NeedDataSource"
                OnItemCommand="RadGrid_UserManage_PWD_ItemCommand" OnItemDataBound="RadGrid_UserManage_PWD_ItemDataBound">
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <ItemStyle HorizontalAlign="Center" />
                <AlternatingItemStyle HorizontalAlign="Center" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3" ScrollHeight="600px"></Scrolling>
                </ClientSettings>
                <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
                </ExportSettings>
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                <CommandItemTemplate>
                        <telerik:RadButton ID="RadButton_AddNew" runat="server" Text="新建用户" Font-Bold="true" CommandName="InitInsert" CssClass="floatleft"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_Delete" runat="server" Text="删除用户" Font-Bold="true" CommandName="Delete" CssClass="floatleft"
                            OnClientClicking="CustomRadWindowConfirm">
                        </telerik:RadButton>
                        员工信息列表
                        <%-- <telerik:RadTextBox ID="RTB_UserName" runat="server" EmptyMessage="输入用户姓名查询" OnTextChanged="RTB_UserName_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                       <telerik:RadComboBox ID="RadComboBoxUserPWD" EmptyMessage="选择用户姓名" AllowCustomText="True" Filter="Contains" AutoPostBack="True"
                            runat="server" Culture="zh-CN" DataSourceID="SqlDataSourceUserPWD" DataTextField="UserName" DataValueField="ID" OnItemDataBound="RadComboBoxUserPWD_ItemDataBound"
                            OnSelectedIndexChanged="RadComboBoxUserPWD_SelectedIndexChanged">
                        </telerik:RadComboBox>--%>
                        <telerik:RadButton ID="RadButton_AddRole" runat="server" Text="角色管理" AutoPostBack="false" OnClientClicked="ShowRoleInfoManage" CssClass="floatright"></telerik:RadButton>
						 <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridClientSelectColumn ItemStyle-Width="20px"></telerik:GridClientSelectColumn>
                        <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ItemStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserAccount" HeaderText="登录账户" ItemStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DomainAccount" HeaderText="域帐号" ItemStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserName" HeaderText="姓名" ItemStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RoleName" HeaderText="角色" UniqueName="RoleName" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                        <telerik:GridDropDownColumn DataField="Dept" DataSourceID="SqlDataSourceDept" ListTextField="Dept" ListValueField="ID" HeaderText="部门" ItemStyle-Width="120px"></telerik:GridDropDownColumn>
                        <telerik:GridBoundColumn DataField="Phone" HeaderText="联系电话" ItemStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="Isdel" HeaderText="删除标记" ReadOnly="True" ItemStyle-Width="80px"/>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderText="编辑" ItemStyle-Width="60px"></telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings UserControlName="UserManagePWD.ascx" EditFormType="WebUserControl">
                    </EditFormSettings>
             
                </MasterTableView>
            </telerik:RadGrid>
            <%-- <asp:SqlDataSource ID="SqlDataSourceUserPWD" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="select '0' as ID, '全部' as UserName Union SELECT [ID], [UserName] FROM [Sys_UserInfo_PWD] where Isdel='false'"></asp:SqlDataSource>--%>
            <asp:SqlDataSource ID="SqlDataSourceDept" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="SELECT [ID], [Dept] FROM [Sys_DeptEnum] where Is_Del = 'false' order by Dept"></asp:SqlDataSource>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowWinRole" runat="server" Title="角色管理" Left="150px"
                        ReloadOnShow="true" ShowContentDuringLoad="true" VisibleTitlebar="true" VisibleStatusbar="false"
                        Behaviors="None" Modal="true" Width="600px" Height="500px" />
                </Windows>
            </telerik:RadWindowManager>
            <%-- 删除弹出窗口--开始--%>
            <telerik:RadWindow ID="confirmDeleteWindow" runat="server" VisibleTitlebar="false"
                VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                <ContentTemplate>
                    <div style="margin-top: 30px; float: left;">
                        <div style="width: 60px; padding-left: 15px; float: left;">
                            <img src="/Images/images/warnning1.jpg" alt="" />
                        </div>
                        <div style="width: 200px; float: left;">
                            <asp:Label ID="lblConfirm" Font-Size="14px" Text="确定要删除选定的记录吗？" runat="server" Font-Bold="true"
                                ForeColor="#25a0da" />
                            <br />
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <telerik:RadButton ID="btnYes" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                                <Icon PrimaryIconCssClass="rbOk" />
                            </telerik:RadButton>
                            &nbsp;
                            <telerik:RadButton ID="btnNo" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                                <Icon PrimaryIconCssClass="rbCancel" />
                            </telerik:RadButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <%-- 删除弹出窗口--结束--%>
            <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
            </telerik:RadNotification>
        </div>
    </div>
</asp:Content>
