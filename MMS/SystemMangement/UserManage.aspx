<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="UserManage.aspx.cs" Inherits="mms.SystemMangement.UserManage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 1200px; margin:0px auto;">
        <div style="width: 1200px; float: left;">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid_UserManage">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_UserManage" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadGrid ID="RadGrid_UserManage" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN"
        GroupPanelPosition="Top" OnNeedDataSource="RadGrid_UserManage_NeedDataSource"
        OnItemCommand="RadGrid_UserManage_ItemCommand" OnItemDeleted="RadGrid_UserManage_ItemDeleted" OnItemDataBound="RadGrid_UserManage_ItemDataBound" OnItemCreated="RadGrid_UserManage_ItemCreated">
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
                <telerik:GridBoundColumn DataField="UserAccount" FilterControlAltText="Filter UserAccount column" HeaderText="账户" SortExpression="UserAccount" UniqueName="UserAccount">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column" HeaderText="用户名" SortExpression="UserName" UniqueName="UserName">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridDropDownColumn DataField="DeptNo" DataSourceID="SqlDataSourceDept" ListTextField="Dept" ListValueField="ID" HeaderText="部门">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridDropDownColumn>
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
                <telerik:RadButton ID="RadButton_AddNew" runat="server" Text="新建用户" Font-Bold="true" CommandName="InitInsert"></telerik:RadButton>
                <telerik:RadButton ID="RadButton_Delete" runat="server" Text="删除用户" Font-Bold="true" CommandName="Delete"
                    OnClientClicking="CustomRadWindowConfirm">
                </telerik:RadButton>
                <telerik:RadComboBox ID="RadComboBoxUser" runat="server" EmptyMessage="选择用户" AllowCustomText="True" Filter="Contains" AutoPostBack="True" Culture="zh-CN" DataSourceID="SqlDataSourceUser" DataTextField="UserName" DataValueField="ID" OnItemDataBound="RadComboBoxUser_ItemDataBound" OnSelectedIndexChanged="RadComboBoxUser_SelectedIndexChanged"></telerik:RadComboBox>
            </CommandItemTemplate>
        </MasterTableView>
    </telerik:RadGrid>
            <asp:SqlDataSource ID="SqlDataSourceUser" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="SELECT [ID], [UserName] FROM [Sys_UserInfo]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceDept" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" SelectCommand="SELECT [ID], [Dept] FROM [Sys_DeptEnum]"></asp:SqlDataSource>
    <%-- 删除弹出窗口--开始--%>
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
    <%-- 删除弹出窗口--结束--%>
            </div>
    </div>
</asp:Content>
