<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlanImport.aspx.cs" Inherits="mms.Plan.PlanImport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server">
		</telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" Skin="Silk"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGridPack" />
                        <telerik:AjaxUpdatedControl ControlID="HFGridItemsCount" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        <telerik:AjaxUpdatedControl ControlID="HFFileName" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                function validationFailed(sender, eventArgs) {
                    $(".ErrorHolder").append("<p>Validation failed for '" + eventArgs.get_fileName() + "'.</p>").fadeIn("slow");
                }
                function fileUploaded(sender, args) {
                    $find("<%=RadAjaxManager1.ClientID %>").ajaxRequest();
                    setTimeout(function () {
                        sender.deleteFileInputAt(0);
                    }, 10);
                }
                //暂存
                function confirmWindowTemporary(sender, args) {
                    var ddl = $find("<%= RDDL_Model.ClientID %>");
                    if (ddl._selectedValue == "0" || ddl._selectedValue == "") {
                        $find("<%= RadNotificationAlert.ClientID %>").set_text("请选择：型号");
                        $find("<%= RadNotificationAlert.ClientID %>").show();
                        args.set_cancel(true);
                    } else {
                        $find("<%=confirmWindowTemporary.ClientID %>").show();
                        args.set_cancel(true);
                    }
                }

                function YesOrNoClickedTemporary(sender, args) {
                    var oWnd = $find("<%=confirmWindowTemporary.ClientID %>");
                    oWnd.close();
                    if (sender.get_text() == "是") {
                        $find("<%=RB_Temporary.ClientID %>").click();
                    }
                }
                //归档
                function confirmWindow(sender, args) {
                    var ddl = $find("<%= RDDL_Model.ClientID %>");
                    if (ddl._selectedValue == "0" || ddl._selectedValue == "") {
                        $find("<%= RadNotificationAlert.ClientID %>").set_text("请选择：型号");
                        $find("<%= RadNotificationAlert.ClientID %>").show();
                        args.set_cancel(true);
                    } else {
                        $find("<%=confirmWindow.ClientID %>").show();
                        args.set_cancel(true);
                    }
                }

                function YesOrNoClicked(sender, args) {
                    var oWnd = $find("<%=confirmWindow.ClientID %>");
                    oWnd.close();
                    if (sender.get_text() == "是") {
                        $find("<%=RBAdd.ClientID %>").click();
                    }
                }
                //导入
                function ShowRadWindowImport(sender, args) {
                    if (document.getElementById("<%= HFGridItemsCount.ClientID %>").value == "0") {
                        $find("<%= RadNotificationAlert.ClientID %>").set_text("没有可导入计划");
                        $find("<%= RadNotificationAlert.ClientID %>").show();
                    } else {
                        var planName = $find("<%=RTB_Name.ClientID%>")._text;
                        if (planName == "") {
                            $find("<%= RadNotificationAlert.ClientID %>").set_text("请输入计划包名称");
                            $find("<%= RadNotificationAlert.ClientID %>").show();
                        } else {
                            var oWnd = $find("<%= RadWindowImport.ClientID %>");
                            oWnd.show();
                        }
                    }
                    
                }
                //取消
                function CloseRadWinowImport(sender, args) {
                    var oWnd = $find("<%=RadWindowImport.ClientID %>");
                    oWnd.close();
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

        <div style="width: 100%; margin: 0px auto;">
            <%--1029-lrw--width:1200px修改为width:100%--%>
            <div style="width: 100%; float: left;">
                <table style="text-align: left; width:100%; vertical-align: middle;">
                    <tr>
                        <td style="width:80px;">计划包名称：</td>
                        <td style="width:130px;"><telerik:RadTextBox ID="RTB_Name" runat="server" Width="120px"></telerik:RadTextBox></td>
                        <td style="width:40px;">备注：</td>
                        <td style="width:210px;"><telerik:RadTextBox ID="RTB_Remark" runat="server" Width="200px"></telerik:RadTextBox> </td>
                        <td style="width: 300px;">
                            <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" AllowedFileExtensions="xlsx,xls" Width="200px"
                                OnFileUploaded="RadAsyncUpload1_FileUploaded" MaxFileInputsCount="1" OnClientValidationFailed="validationFailed"
                                UploadedFilesRendering="BelowFileInput" OnClientFileUploaded="fileUploaded">
                                <Localization Select="选择文件" />
                            </telerik:RadAsyncUpload>
                        </td>
                        <td>
                            <asp:HiddenField ID="HFFileName" runat="server" />
                            <asp:HiddenField ID="HFGridItemsCount" runat="server" Value="0" />
                        </td>
                        <td style="width: 60px;">
                            <telerik:RadButton ID="RB_Import" runat="server" Text="导入" AutoPostBack="false" OnClientClicked="ShowRadWindowImport"></telerik:RadButton>
                        </td>
                        <td style="width:80px;">
                            <telerik:RadButton ID="btnDown" runat="server" Text="下载模版" OnClick="btnDown_Click">
                            </telerik:RadButton>
                        </td>
                        <td style="width:80px;">
                            <span title="1、导入数据必须在Sheet1工作簿内；2、导入的列名称必须含有：产品名称、产品图号、任务号、计量单位、单机配套数量、交付总数量、本次投产数量、计划交付时间；">导入Excel文件说明</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%; float: left;">
                <telerik:RadGrid ID="RadGridPack" runat="server" OnNeedDataSource="RadGridPack_NeedDataSource" AllowPaging="false"
                    AutoGenerateColumns="false" ItemStyle-HorizontalAlign="Center"  Height="420px">
                    <AlternatingItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                    <ClientSettings EnableRowHoverStyle="true" >
                        <Selecting AllowRowSelect="true" />
                         <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="400px"></Scrolling>
                    </ClientSettings>
                    <MasterTableView AllowMultiColumnSorting="true" CommandItemDisplay="Top">
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
                            <telerik:GridTemplateColumn HeaderText="是否展开" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:RadioButtonList ID="RBL_IsSpread" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="false" Text="否"></asp:ListItem>
                                        <asp:ListItem Value="true" Text="是"></asp:ListItem>
                                    </asp:RadioButtonList>
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
                            <span class="floatright">是否展开：</span>
                        </CommandItemTemplate>
                    </MasterTableView>
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
        <telerik:RadWindow ID="confirmWindowTemporary" runat="server" VisibleTitlebar="false" VisibleStatusbar="false"
            Modal="true" Behaviors="None" Height="120px" Width="320px">
            <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <div style="width: 60px; padding-left: 15px; float: left;">
                        <img src="/Images/images/warnning1.jpg" alt="" />
                    </div>
                    <div style="width: 200px; float: left;">
                        <asp:Label ID="Label1" Font-Size="14px" Text="确定暂存？" runat="server" Font-Bold="true"
                            ForeColor="#25a0da" />
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadButton ID="RadButton1" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClickedTemporary">
                        <Icon PrimaryIconCssClass="rbOk" />
                    </telerik:RadButton>
                        &nbsp;
                    <telerik:RadButton ID="RadButton2" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClickedTemporary">
                        <Icon PrimaryIconCssClass="rbCancel" />
                    </telerik:RadButton>
                    </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
        <%-- 导入--开始--%>
        <telerik:RadWindow ID="RadWindowImport" runat="server" VisibleTitlebar="false"
            VisibleStatusbar="false" Modal="true" Behaviors="None" Height="300px" Width="500px">
            <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <table cellspacing="20" style="text-align: left; width: 460px; margin: 0px auto;">
                        <tr>
                            <td colspan="2" style="text-align: center; font-size: 15px; font-weight: bold;">请填写计划包型号阶段信息</td>
                        </tr>
                        <tr>
                            <td style="width: 180px; text-align: right;">型号：</td>
                            <td style="width: 260px;">
                                <telerik:RadDropDownList ID="RDDL_Model" runat="server" AppendDataBoundItems="true" Width="120px">
                                    <Items>
                                        <telerik:DropDownListItem Value="0" Text="请选择型号" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40px; text-align: right;">阶段：</td>
                            <td style="width: 160px;">
                                <telerik:RadDropDownList ID="RDDL_Stage" runat="server" Width="120px"></telerik:RadDropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="width: 320px; margin: 0px auto; text-align: center;">
                                    <div style="width: 100px; float: left;">
                                        <telerik:RadButton ID="RB_Temporary" runat="server" Text="暂存" OnClick="RB_Temporary_Click" OnClientClicking="confirmWindowTemporary" ToolTip="暂存后可以修改计划包数据，不可以同步材料定额">
                                        </telerik:RadButton>
                                    </div>
                                    <div style="width: 100px; float: left;">
                                        <telerik:RadButton ID="RBAdd" runat="server" Text="归档" OnClick="RBAdd_Click" OnClientClicking="confirmWindow" ToolTip="归档后可以同步材料定额信息，不可以在修改计划包数据">
                                        </telerik:RadButton>
                                    </div>
                                    <div style="width: 100px; float: left;">
                                        <telerik:RadButton ID="RB_Cancel" runat="server" Text="取消" AutoPostBack="false" OnClientClicking="CloseRadWinowImport" CssClass="floatright">
                                        </telerik:RadButton>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
        <%-- 导入--结束--%>
        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
            AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
        </telerik:RadNotification>
    </form>
</body>
</html>
