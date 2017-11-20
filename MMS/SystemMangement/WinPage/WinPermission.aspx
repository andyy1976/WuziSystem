<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WinPermission.aspx.cs" Inherits="mms.SystemMangement.WinPage.WinPermission" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                //<![CDATA[
                //删除确认窗口开始
                var deleteButtonID;
                function CustomRadWindowConfirm(sender, args) {
                    //Open the window
                    $find("<%=confirmDeleteWindow.ClientID %>").show();
                    deleteButtonID = sender.get_id();
                    //Cancel the postback
                    args.set_cancel(true);
                }
                function YesOrNoClicked(sender, args) {
                    var oWnd = $find("<%=confirmDeleteWindow.ClientID %>");
                    oWnd.close();
                    if (sender.get_text() == "是") {
                        $find(deleteButtonID).click();
                    }
                }
                //删除确认窗口结束
                //]]>
            </script>
        </telerik:RadCodeBlock>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid_Permission">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_Permission" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
        <telerik:RadGrid ID="RadGrid_Permission" runat="server" Culture="zh-CN" GroupPanelPosition="Top"
            OnNeedDataSource="RadGrid_Permission_NeedDataSource" Width="100%"
            OnItemCommand="RadGrid_Permission_ItemCommand" OnItemDataBound="RadGrid_Permission_ItemDataBound"
            AllowPaging="True" PageSize="10">
            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <telerik:RadButton ID="headerChkbox" OnCheckedChanged="ToggleSelectedState"
                                AutoPostBack="True" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadButton ID="RadButtonItem" runat="server" ButtonType="ToggleButton"
                                ToggleType="CheckBox" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True" />
                        </ItemTemplate>
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="序号" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PermissionSign" FilterControlAltText="Filter PermissionSign column" HeaderText="权限标示" SortExpression="PermissionSign" UniqueName="PermissionSign">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SignName" FilterControlAltText="Filter SignName column" HeaderText="权限名" SortExpression="SignName" UniqueName="SignName">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ParentId" DataType="System.Int32" FilterControlAltText="Filter ParentId column" HeaderText="父级ID" SortExpression="ParentId" UniqueName="ParentId">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderText="编辑" UniqueName="EditCommandColumn"
                         HeaderStyle-Width="80px">
                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings>
                    <EditColumn ButtonType="ImageButton" InsertText="新增" UpdateText="修改" CancelText="取消">
                    </EditColumn>
                </EditFormSettings>
                <CommandItemTemplate>
                    <telerik:RadButton ID="RadButton_AddNew" runat="server" Text="新建权限" Font-Bold="true" CommandName="InitInsert"></telerik:RadButton>
                    <telerik:RadButton ID="RadButton_Delete" runat="server" Text="删除权限" Font-Bold="true" CommandName="Delete"
                        OnClientClicking="CustomRadWindowConfirm">
                    </telerik:RadButton>
                    <telerik:RadComboBox ID="RadComboBoxPermission" runat="server" AllowCustomText="True" Filter="Contains"
                        AutoPostBack="True" Culture="zh-CN" DataSourceID="SqlDataSourcePermission" DataTextField="SignName"
                        DataValueField="ID" OnItemDataBound="RadComboBoxPermission_ItemDataBound" EmptyMessage="选择权限"
                        OnSelectedIndexChanged="RadComboBoxPermission_SelectedIndexChanged">
                    </telerik:RadComboBox>
                </CommandItemTemplate>
            </MasterTableView>
        </telerik:RadGrid>
        <asp:SqlDataSource ID="SqlDataSourcePermission" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="SELECT [ID], [SignName] FROM [Sys_Permission]"></asp:SqlDataSource>
        <%-- 删除弹出窗口--开始--%>
        <telerik:RadWindow ID="confirmDeleteWindow" runat="server" VisibleTitlebar="false"
            VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
            <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <div style="width: 60px; padding-left: 15px; float: left;">
                        <img src="Images/Warning.png" alt="" />
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
    </form>
</body>
</html>
