<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShippingAddressManage.aspx.cs" Inherits="mms.SystemMangement.ShippingAddressManage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .floatleft {
            float: left;
        }
        .floatright {
            float: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                var deleteid;
                function confirmWindow(sender, args) {
                    $find("<%=RadWindowDelete.ClientID%>").show();
                    deleteid = sender.get_id();
                    args.set_cancel(true);
                }
                function YesOrNoClickedDelete(sender, args) {
                    $find("<%=RadWindowDelete.ClientID%>").close();
                    if (sender.get_text() == "是") {
                        $find(deleteid).click();
                    }
                }
                function CloseWindow(args) {                 
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    var oArg = new Object();
                    oWindow.BrowserWindow.refreshGrid(args);
                    oWindow.close(oArg);
                }
            </script>
        </telerik:RadCodeBlock>
        <div style="width: 470px; margin: 0px auto;">
            <div style="width: 470px; float: left;">
                <telerik:RadButton ID="RB_Close" runat="server" Text="关闭" ButtonType="LinkButton" AutoPostBack="false" CssClass="floatright" OnClientClicking="CloseWindow"></telerik:RadButton>
            </div>
            <div style="width: 470px; float: left;">
                <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15"
                    OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand" Skin="Silk">
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <ItemStyle HorizontalAlign="Center" />
                    <AlternatingItemStyle HorizontalAlign="Center" />
                    <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                    <ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true">
                        <Selecting AllowRowSelect="true" />
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top"  EditMode="InPlace" DataKeyNames="ID">
                        <Columns>
                            <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ItemStyle-Width="40px" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="KeyWord" HeaderText="配送地址" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                            <telerik:GridEditCommandColumn ButtonType="LinkButton" HeaderText="编辑" UniqueName="EditCommandColumn"
                                HeaderStyle-Width="100px" InsertText="新增" EditText="编辑" UpdateText="修改" CancelText="取消">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridTemplateColumn ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <telerik:RadButton ID="RB_Del" runat="server" ButtonType="ToggleButton" Text="删除" OnClientClicking="confirmWindow" CommandName="delete"></telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <CommandItemTemplate>
                            <telerik:RadButton ID="RB_Add" runat="server" Text="新增配送地址" CommandName="InitInsert" CssClass="floatleft"></telerik:RadButton>
                            配送地址列表                    
                        </CommandItemTemplate>
                        <CommandItemStyle HorizontalAlign="Center" Font-Size="15px" />
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <%-- 删除弹出窗口--开始--%>
            <telerik:RadWindow ID="RadWindowDelete" runat="server" VisibleTitlebar="false"
                VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                <ContentTemplate>
                    <div style="margin-top: 30px; float: left;">
                        <div style="width: 60px; padding-left: 15px; float: left;">
                            <img src="../Images/images/warnning1.jpg" alt="" />
                        </div>
                        <div style="width: 200px; float: left;">
                            <asp:Label ID="Label2" Font-Size="14px" Text="确定要删除吗？" runat="server" Font-Bold="true"
                                ForeColor="#25a0da" />
                            <br />
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="RadButton3" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClickedDelete">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                            &nbsp;
                        <telerik:RadButton ID="RadButton4" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClickedDelete">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </telerik:RadButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
            <%-- 删除弹出窗口--结束--%>
            <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
            AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
        </telerik:RadNotification>
        </div>
    </form>
</body>
</html>
