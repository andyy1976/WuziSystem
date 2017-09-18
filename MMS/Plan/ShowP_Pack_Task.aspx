<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowP_Pack_Task.aspx.cs" Inherits="mms.Plan.ShowP_Pack_Task" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager runat="server" Skin="Silk"></telerik:RadSkinManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RB_Query">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGridP_Pack_Task" LoadingPanelID="RadAjaxLoadingPanel1" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="RBL_IsSpread">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGridP_Pack_Task" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    var RBAddID;
                    function confirmWindow(sender, args) {
                        $find("<%=confirmWindow.ClientID %>").show();
                        RBAddID = sender.get_id();;
                        args.set_cancel(true);
                    }
                    function YesOrNoClicked(sender, args) {
                        var oWnd = $find("<%=confirmWindow.ClientID %>");
                        oWnd.close();
                        if (sender.get_text() == "是") {
                            $find(RBAddID).click();
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
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
            <div style="width: 100%; margin: 0px auto;">
                <div style="width: 100%; float: left; padding-top:10px; border-bottom: 1px solid Black; font-size: 16px; font-weight: bold;">
                    编号：<asp:Label ID="lblPlanCode" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    型号：<asp:Label ID="lblModel" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    计划包名称：<telerik:RadTextBox ID="RTB_PlanName" runat="server" ></telerik:RadTextBox>
                       <asp:Label ID="lblPlanName" runat="server"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    备注：<telerik:RadTextBox ID="RTB_Remark" runat="server"></telerik:RadTextBox>
                    <asp:Label ID="lblRemark" runat="server"></asp:Label>
                    <asp:HiddenField ID="HFState" runat="server" />                   
                </div>
                <div style="width: 100%; float: left; margin-top: 10px;">
                    <table style="text-align: left;" id="table1" runat="server" visible="false">
                        <tr>
                            <td style="text-align: right;">产品名称：</td>
                            <td>
                                <telerik:RadTextBox ID="RTB_ProductName" runat="server" Width="120px"></telerik:RadTextBox></td>
                            <td style="text-align: right;">产品图号：</td>
                            <td>
                                <telerik:RadTextBox ID="RTB_DrawingNum" runat="server" Width="120px"></telerik:RadTextBox></td>
                            <td style="text-align: right;">任务号：</td>
                            <td>
                                <telerik:RadTextBox ID="RTB_TaskNum" runat="server" Width="120px"></telerik:RadTextBox></td>
                            <td>交付时间：</td>
                            <td>
                                <telerik:RadDatePicker ID="RDP_StartDate" runat="server" Width="120px"></telerik:RadDatePicker>
                                ～<telerik:RadDatePicker ID="RDP_EndDate" runat="server" Width="120px"></telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadButton ID="RB_Query" runat="server" Text="筛选" OnClick="RB_Query_Click"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                     <telerik:RadButton ID="RB_Add" runat="server" Text="归档" CssClass="floatright" Visible="false" CommandName="File" OnClientClicking="confirmWindow" OnClick="RB_Add_Click"></telerik:RadButton>
                </div>
                <div style="width: 100%; float: left; margin-top: 10px;">
                    <telerik:RadGrid ID="RadGridP_Pack_Task" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="true"
                        OnNeedDataSource="RadGridP_Pack_Task_NeedDataSource" OnItemCreated="RadGridP_Pack_Task_ItemCreated">
                        <AlternatingItemStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="280px"></Scrolling>
                        </ClientSettings>                        
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="TaskID" ClientDataKeyNames="TaskID" AllowAutomaticInserts="false">
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsID" HeaderText="序号" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProductName" HeaderText="产品名称" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaskDrawingCode" HeaderText="产品图号" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Stage1" HeaderText="阶段" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Unit" HeaderText="计量单位" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MatingNum" HeaderText="单机配套数量" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DefrayNum" HeaderText="交付总数量" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProductionNum" HeaderText="本次投产数量" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PlanFinishTime" HeaderText="计划交付时间" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="是否可展开" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:RadioButtonList ID="RBL_IsSpread" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBL_IsSpread_SelectedIndexChanged">
                                            <asp:ListItem Value="false" Text="否"></asp:ListItem>
                                            <asp:ListItem Value="true" Text="是"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:Label ID="lbl_IsSpread" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <CommandItemTemplate>
                                型号投产计划任务列表
                                <asp:RadioButtonList ID="RBL_IsSpreadAll" runat="server" RepeatDirection="Horizontal" CssClass="floatright" AutoPostBack="true" OnSelectedIndexChanged="RBL_IsSpreadAll_SelectedIndexChanged">
                                    <asp:ListItem Value="false" Text="全否"></asp:ListItem>
                                    <asp:ListItem Value="true" Text="全是"></asp:ListItem>
                                    <asp:ListItem Value="" Text="全取消"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:Label ID="lbl" runat="server" CssClass="floatright" Text="是否展开："></asp:Label>
                            </CommandItemTemplate>
                        </MasterTableView>
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="30px" />
                    </telerik:RadGrid>
                </div>
            </div>
            <telerik:RadWindow ID="confirmWindow" runat="server" VisibleTitlebar="false" VisibleStatusbar="false"
                Modal="true" Behaviors="None" Height="120px" Width="320px">
                <ContentTemplate>
                    <div style="margin-top: 30px; float: left;">
                        <div style="width: 60px; padding-left: 15px; float: left;">
                            <img src="/Images/images/warnning1.jpg" alt="" />
                        </div>
                        <div style="width: 200px; float: left;">
                            <asp:Label ID="lblConfirm" Font-Size="14px" Text="确定归档？" runat="server" Font-Bold="true"
                                ForeColor="#25a0da" />
                            <br />
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
            <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
            </telerik:RadNotification>
        </div>
    </form>
</body>
</html>
