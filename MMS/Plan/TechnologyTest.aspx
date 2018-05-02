<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TechnologyTest.aspx.cs" Inherits="mms.Plan.TechnologyTest" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" href="../Styles/Plan.css" rel="stylesheet" />
    <script>
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 ||args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 ||args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0 )
            {

                args.set_enableAjax(false);

            }
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;
            return oWindow;
        }

    </script>
    <style type="text/css">
        .floatleft {
            float: left;
            margin-left: 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" Skin="Silk"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <ClientEvents OnRequestStart="onRequestStart" />
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid_TechnologyTest">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTest" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadBtn_Search">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTest" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="span_hbxqCode" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                function CloseWindow(args) {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    var oArg = new Object();
                    oWindow.BrowserWindow.refreshGrid(args);
                    oWindow.close(oArg);
                }
                var submitId;
                function confirmRadWindow(sender, args) {
                    debugger;
                    $find("<%= RadWindow.ClientID %>").show();
                    submitId = sender.get_id();
                    args.set_cancel(true);
                }
                function YesOrNoClicked(sender, args) {
                    var oWnd = $find("<%=RadWindow.ClientID %>");
                    oWnd.close();
                    if (sender.get_text() == "是") {
                        $find(submitId).click();
                    }
                }
            </script>
        </telerik:RadCodeBlock>
        <div style="width:100%;">
            <div style="width:100%; font-size:16px;">
                <span style="float: left">编号：</span><span id="span_gysyjCode" runat="server" style="float: left"></span>
            </div>
            <div style="width:100%;">
                <telerik:RadGrid ID="RadGridApp" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGridApp_NeedDataSource">
                    <HeaderStyle HorizontalAlign="Center" Font-Size="12px" />
                    <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                    <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="100px" UseStaticHeaders="True" />
                    </ClientSettings>
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <MasterTableView CommandItemDisplay="Top">
                        <CommandItemTemplate>
                            审批流程列表
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ActivityName" HeaderText="核准流程名称" HeaderStyle-Width="200px" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ApproveLeaderDept" HeaderText="核准人部门" HeaderStyle-Width="200px" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ApproveLeaderName" HeaderText="核准人姓名" HeaderStyle-Width="200px" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ApproveTime" HeaderText="核准时间" HeaderStyle-Width="200px" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Reason" HeaderText="原因" HeaderStyle-Width="200px" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Result" HeaderText="结果" HeaderStyle-Width="200px" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <div style="width:100%; margin-top:10px;">
                <telerik:RadButton ID="BtnSubmit" runat="server" Text="提交物流中心" OnClick="BtnSubmit_Click" OnClientClicking="confirmRadWindow" Visible="false"></telerik:RadButton>
            </div>
            <div style="width:100%; margin-top:10px;">
                 <telerik:RadGrid ID="RadGridErr" runat="server" AllowPaging="True" Culture="zh-CN" GroupPanelPosition="Top" PageSize="10"
                    OnNeedDataSource="RadGridErr_NeedDataSource">
                    <HeaderStyle HorizontalAlign="Center" Font-Size="12px" />
                    <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                    <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="100px" UseStaticHeaders="True" />
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                        <CommandItemTemplate>
                            提交物流中心失败列表
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="rownum" HeaderText="序号" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ID" HeaderText="需求行号" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DRAWING_NO" HeaderText="产品图号" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Model" HeaderText="型号" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                               <telerik:GridBoundColumn DataField="RQ_DATE" HeaderText="需求时间" HeaderStyle-Width="100px" ItemStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}">
                            </telerik:GridBoundColumn>
                               <telerik:GridBoundColumn DataField="Mat_Unit" HeaderText="计量单位" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UrgencyDegre" HeaderText="紧急程度" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="stage1" HeaderText="研制阶段" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UseDes" HeaderText="用途" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MaterialDept" HeaderText="领料部门" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ADDRESS" HeaderText="配送地址" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Err_Msg" HeaderText="失败原因" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <div style="width:100%; margin-top:10px;">
                <telerik:RadGrid ID="RadGrid_TechnologyTest" runat="server" AllowPaging="True" Culture="zh-CN" GroupPanelPosition="Top" PageSize="10"
                    OnNeedDataSource="RadGrid_TechnologyTest_NeedDataSource">
                    <HeaderStyle HorizontalAlign="Center" Font-Size="12px" />
                    <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                    <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="200px" UseStaticHeaders="True" />
                    </ClientSettings>
                     <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
						    物资需求清单列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="rownum" HeaderText="序号" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ID" HeaderText="需求行号" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DRAWING_NO" HeaderText="产品图号" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="Model" HeaderText="型号" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="需求尺寸" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="RQ_DATE" HeaderText="需求时间" HeaderStyle-Width="100px" ItemStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Unit" HeaderText="计量单位" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UrgencyDegre" HeaderText="紧急<br />程度" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="stage1" HeaderText="研制<br />阶段" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UseDes" HeaderText="用途" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MaterialDept" HeaderText="领料部门" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ADDRESS" HeaderText="配送地址" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="State" HeaderText="执行状态" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SUBMISSION_DATE" HeaderText="提交时间" DataFormatString="{0:yyyy-MM-dd}"  HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>       
        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
            AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
        </telerik:RadNotification>
        <%-- 提交确认弹出窗口--开始--%>
        <telerik:RadWindow ID="RadWindow" runat="server" VisibleTitlebar="false"
            VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
            <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <div style="width: 60px; padding-left: 15px; float: left;">
                        <img src="../Images/images/warnning1.jpg" alt="" />
                    </div>
                    <div style="width: 200px; float: left;">
                        <asp:Label ID="Label2" Font-Size="14px" Text="确定要提交吗？" runat="server" Font-Bold="true"
                            ForeColor="#25a0da" />
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="RadButton3" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                        &nbsp;
                        <telerik:RadButton ID="RadButton4" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </telerik:RadButton>
                    </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
        <%-- 提交确认弹出窗口--结束--%>
    </form>
</body>
</html>
