<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="ChangeMaterialQuota.aspx.cs" Inherits="mms.Plan.ChangeMaterialQuota" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
    <script type="text/javascript" src="../Scripts/jquery-1.7.2.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->SmartTeam列表" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RTB_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tablechangelist"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="HFChange"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lblSecrchCNResult"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGridChange" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RTB_Remark"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_SynchronChange">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTL_BOM"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RTL_Defect"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RTL_SubmitState"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGridChange" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RTB_MCLCode"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="lbl_DrawingNo"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label10"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label11"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label12"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label14"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label15"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label16"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label24"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label25"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label26"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label30"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label31"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label32"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label1All"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">

            function EnterKeyProcessing(sender, eventArgs) {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }
            //更改材料定额 开始
            function ConfirmChange(sender, args) {
                var grid = $find("<%=RadGridChange.ClientID%>");
                var masterTableView = grid.get_masterTableView();
                if (masterTableView == null) {
                    var radalert = $find("<%=RadNotificationAlert.ClientID%>");
                    radalert.set_text("还没有更改单无法更改！");
                    radalert.show();
                } else {
                    var dataItems = masterTableView.get_dataItems()
                    var length = dataItems.length;
                    if (length == "0") {
                        var radalert = $find("<%=RadNotificationAlert.ClientID%>");
                        radalert.set_text("还没有更改单无法更改！");
                        radalert.show();
                    } else {
                        $find("<%=confirmWindow.ClientID%>").show();
                    }
                }
                args.set_cancel(true);
            }

            function YesOrNoClicked(sender, args) {
                if (sender.get_text() == "否") {
                    var oWnd = $find("<%=confirmWindow.ClientID %>");
                    oWnd.close();
                }
            }
            //更改材料定额 结束
            function ShowMDemandChangeSubmit(PackId) {
                var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                win.set_title("型号任务变更提交");
                window.radopen("/Plan/MDemandChangeSubmit.aspx?PackId=" + PackId + "&fromPage=" + Request.QueryString["PackId"], "RadWindowRecordWindow");
                return false;
            }
            $(function () {
                $("#<%=RadButton1.ClientID%>").click(function () {

                    var radalert = $find("<%=RadNotificationAlert.ClientID%>");
                            var Remark = $("#<%=RTB_Remark.ClientID%>").val();
                            if (Remark == "") {
                                radalert.set_text("请输入更改说明！");
                                radalert.show();
                                return;
                            }
                            var rbl = $('input:radio[name="ctl00$ContentPlaceHolder1$confirmWindow$C$RBL1"]:checked').val();
                            if (rbl == undefined) {
                                radalert.set_text("请选择更改原因！");
                                radalert.show();
                                return;
                            }
                            var oWnd = $find("<%=confirmWindow.ClientID %>");
                            oWnd.close();
                            $find("<%=RB_SynchronChange.ClientID%>").click();
                        });
            });
        </script>
    </telerik:RadCodeBlock>
    <div style="width: 100%; margin: 0px auto;">
        <div style="width: 100%; float: left; padding-bottom: 10px;">
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true" Skin="Default">
                <Tabs>
                    <telerik:RadTab Value="0" Text="BOM总表"></telerik:RadTab>
                    <telerik:RadTab Value="1" Text="缺定额和不规范"></telerik:RadTab>
                    <telerik:RadTab Value="2" Text="提交状态"></telerik:RadTab>
                    <telerik:RadTab Value="3" Text="物资需求变更" Selected="true"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
        </div>
        <div style="width:100%; font-weight:bold; padding-bottom:10px;">
            型号：<asp:Label ID="lblModel" runat="server" Font-Bold="false" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            计划包名称：<asp:Label ID="lblPlanName" runat="server" Font-Bold="false"></asp:Label>
        </div>
        <div style="width: 100%; float: left;">
            <div>
                <div>
                    <table>
                        <tr>
                            <td>更改单号：
                                <telerik:RadTextBox ID="RTB_CN_CHANGEREPORT_NO" runat="server" Width="120px">
                                                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                </telerik:RadTextBox>
                                <asp:HiddenField ID="HFChange" runat="server" />
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadButton ID="RTB_Search" runat="server" Text="搜索" OnClick="RTB_Search_Click"></telerik:RadButton>
                            </td>
                            <td>
                                <asp:Label ID="lblSecrchCNResult" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table id="tablechangelist" runat="server" style="text-align: left;">
                        <tr>
                            <td style="width: 80px; text-align: right;">工艺路线：</td>
                            <td style="width: 180px;">
                                <asp:Label ID="lbl_TECHNICS_LINE" runat="server"></asp:Label></td>
                            <td style="width: 80px; text-align: right;">编号：</td>
                            <td style="width: 180px;">
                                <asp:Label ID="lbl_CN_CHANGEREPORT_NO" runat="server"></asp:Label></td>
                            <td style="width: 80px; text-align: right;">图号：</td>
                            <td style="width: 180px;">
                                <asp:Label ID="lbl_C_CN_DRAWING_NO" runat="server"></asp:Label></td>
                            <td rowspan="2" style="vertical-align: bottom;">
                                <telerik:RadButton ID="RB_SynchronChange" runat="server" Text="更改材料定额" OnClick="RB_SynchronChange_Click" OnClientClicking="ConfirmChange" Visible="false"></telerik:RadButton>
                                <telerik:RadButton ID="RadButtonBuild" runat="server" Text="生成变更列表" Visible="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">名称：</td>
                            <td>
                                <asp:Label ID="lbl_C_TDM_DESCRIPTION" runat="server"></asp:Label></td>
                            <td style="text-align: right;">更改内容：</td>
                            <td>
                                <asp:Label ID="lbl_CN_CHANGE_REASON" runat="server"></asp:Label></td>
                            <td style="text-align: right;">编制：</td>
                            <td>
                                <asp:Label ID="lbl_Change_UserName" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>
                <div>
                    <telerik:RadGrid ID="RadGridChange" runat="server" OnNeedDataSource="RadGridChange_NeedDataSource" AutoGenerateColumns="false">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="600px"></Scrolling>
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CN_DRAWING_NO" HeaderText="图号" UniqueName="CN_DRAWING_NO"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Name" HeaderText="名称"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CN_COM_DRAWING_NO" HeaderText="所属图号"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ChangeContent" HeaderText="更改内容"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CN_TECHNICS_LINE" HeaderText="工艺路线"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ChangePerson" HeaderText="更改人"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CN_EDIT_TYPE1" HeaderText="类型"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PackState" HeaderText="计划包中的状态"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    </div>
    <%-- 更改材料定额--开始--%>
    <telerik:RadWindow ID="confirmWindow" runat="server" VisibleTitlebar="false"
        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="260px" Width="400px">
        <ContentTemplate>
            <table style="text-align: center; margin: 20px auto;">
                <tr>
                    <td style="text-align: right; width: 80px;">更改原因：</td>
                    <td style="width: 240px;">
                        <asp:RadioButtonList ID="RBL1" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" ClientIDMode="Static"></asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">更改说明：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Remark" runat="server" TextMode="MultiLine" Width="220px" Height="40px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;" rowspan="2">
                        <img src="../Images/images/warnning1.jpg" alt="" /></td>
                    <td style="text-align: left; padding-left: 20px;">
                        <asp:Label ID="Label1" Font-Size="14px" runat="server" Font-Bold="true"
                            ForeColor="#25a0da" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; padding-left: 20px;">
                        <telerik:RadButton ID="RadButton1" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                        &nbsp;
                            <telerik:RadButton ID="RadButton2" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                                <Icon PrimaryIconCssClass="rbCancel" />
                            </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </telerik:RadWindow>
    <%-- 更改材料定额--结束--%>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
    <%--弹出窗口--开始--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowRecordWindow" runat="server" Title="型号任务变更提交" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="1200px" Height="620px" />
        </Windows>
    </telerik:RadWindowManager>

    <%--结束--%>
</asp:Content>
