<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinRole.aspx.cs" Inherits="mms.SystemMangement.WinPage.WinRole" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="divViewPanel">
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
                    <telerik:AjaxSetting AjaxControlID="RadGrid_RoleManage">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_RoleManage" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    var deleteButtonID;
                    function CustomRadWindowConfirm(sender, args) {
                        $find("<%=confirmDeleteWindow.ClientID %>").show();
                        deleteButtonID = sender.get_id();
                        args.set_cancel(true);
                    }
                    function YesOrNoClicked(sender, args) {
                        var oWnd = $find("<%=confirmDeleteWindow.ClientID %>");
                        oWnd.close();
                        if (sender.get_text() == "是") {
                            $find(deleteButtonID).click();
                        }
                    }
                    function CloseWindow() {
                        window.close();
                    }
                </script>
            </telerik:RadCodeBlock>
            <telerik:RadGrid ID="RadGrid_RoleManage" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top" OnNeedDataSource="RadGrid_RoleManage_NeedDataSource" OnItemCommand="RadGrid_RoleManage_ItemCommand" OnItemDataBound="RadGrid_RoleManage_ItemDataBound" OnItemCreated="RadGrid_RoleManage_ItemCreated">
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="序号" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RoleName" FilterControlAltText="Filter RoleName column" HeaderText="角色名称" SortExpression="RoleName" UniqueName="RoleName">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text="" />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderText="编辑" UniqueName="EditCommandColumn"
                             HeaderStyle-Width="80px">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                        </telerik:GridEditCommandColumn>
                        <telerik:GridTemplateColumn HeaderText="删除"  HeaderStyle-Width="80px" UniqueName="DeleteColumn">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadButton ID="RadButtonDelete" runat="server" Text="删除" OnClientClicking="CustomRadWindowConfirm" CommandName="Delete"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <EditFormSettings>
                        <EditColumn ButtonType="ImageButton" InsertText="新增" UpdateText="修改" CancelText="取消">
                        </EditColumn>

                    </EditFormSettings>
                    <CommandItemTemplate>
                        <telerik:RadButton ID="RadButton_AddNew" runat="server" Text="新增角色" Font-Bold="true" CommandName="InitInsert"></telerik:RadButton>

                    </CommandItemTemplate>
                </MasterTableView>
            </telerik:RadGrid>
            
            <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                AutoCloseDelay="1000" Width="240" Title="提示" EnableRoundedCorners="true">
            </telerik:RadNotification>
            
            <telerik:RadWindow ID="confirmDeleteWindow" runat="server" VisibleTitlebar="false"
                VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                <ContentTemplate>
                    <div style="margin-top: 30px; float: left;">
                        <div style="width: 60px; padding-left: 15px; float: left;">
                            <img src="../Images/images/warnning1.jpg" alt="" />
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
        </div>
        <%--<telerik:RadButton ID="RadButtonClose" runat="server" Text="关闭" AutoPostBack="false" OnClientClicking="CloseWindow"></telerik:RadButton>--%>
    </div>
    </form>
</body>
</html>
