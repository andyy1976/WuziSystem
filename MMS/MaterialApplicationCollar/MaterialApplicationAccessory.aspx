﻿<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MaterialApplicationAccessory.aspx.cs" Inherits="mms.MaterialApplicationCollar.MaterialApplicationAccessory" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0)
			{

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资申请-->车间备料物资需求申请" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"  OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMDML" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridMDML">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMDML" />
                    <telerik:AjaxUpdatedControl ControlID="HFMDMLID"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Search_Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMDML" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
            function refreshGrid(arg) {
                if (!arg) {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                }
            }
           function ShowWindow(sender, args) {
                var grid = $find("<%=RadGridMDML.ClientID%>");
                if (grid._selectedIndexes.length == 0) {
                    $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择申请行");
                    $find("<%=RadNotificationAlert.ClientID%>").show();
                    return;
                } else {
                    var win = $find("<%=RadWindowManager1.ClientID %>");
                    var ID = $get("<%=HFMDMLID.ClientID%>").value;
                    if (ID == "") {
                        $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择申请行");
                        $find("<%=RadNotificationAlert.ClientID%>").show();
                        return;
                    }
                    window.radopen("/MaterialApplicationCollar/MaterialAppWindow.aspx?Type=3&MDMLID=" + ID, "RadWindowApp");
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div style="width: 100%; float: left;">
        <div style="width: 100%; float: left;">
            <table>
                <tr>
                    <td>需求行号：</td>
                    <td><telerik:RadTextBox ID="RTB_ID" runat="server" Width="100px"></telerik:RadTextBox></td>
                    <td>型号：</td>
                    <td><telerik:RadTextBox ID="RTB_Project" runat="server" Width="100px"></telerik:RadTextBox></td>
                    <td>图号：</td>
                    <td><telerik:RadTextBox ID="RTB_Drawing_No" runat="server" Width="100px"></telerik:RadTextBox></td>
                    <td>任务号：</td>
                    <td><telerik:RadTextBox ID="RTB_TaskCode" runat="server" Width="100px"></telerik:RadTextBox></td>

                    <td>物资名称：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Name" runat="server" Width="100px"></telerik:RadTextBox></td>
                    <td>物资编码：</td>
                    <td><telerik:RadTextBox ID="RTB_ItemCode1" runat="server" Width="100px"></telerik:RadTextBox></td>
                    <td>申请时间：</td>
                    <td><telerik:RadDatePicker ID="RDPStart" runat="server" Width="100px"></telerik:RadDatePicker>
                      ～<telerik:RadDatePicker ID="RDPEnd" runat="server" Width="100px"></telerik:RadDatePicker></td>
                    <td><telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton></td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; float: left;">
            <asp:HiddenField ID="HF_DeptCode" runat="server" />
            <asp:HiddenField ID="HFMDMLID" runat="server" />
            <telerik:RadGrid ID="RadGridMDML" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                 AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True"
                OnNeedDataSource="RadGridMDML_NeedDataSource" OnSelectedIndexChanged="RadGridMDML_SelectedIndexChanged">
                <ClientSettings Selecting-AllowRowSelect="true"  EnablePostBackOnRowClick="true" EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" ScrollHeight="600px" UseStaticHeaders="true"></Scrolling>
                </ClientSettings>
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
            <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
            </ExportSettings>
                <MasterTableView DataKeyNames="ID" CommandItemDisplay="Top">
			    <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="Material" HeaderText="物资信息" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" UniqueName="ItemCount1" ColumnGroupName="Material" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" ColumnGroupName="Material" ItemStyle-Width="120px" HeaderStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Mark" HeaderText="牌号" UniqueName="Material_Mark" ColumnGroupName="Material" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CN_Material_State" HeaderText="供应状态" UniqueName="CN_Material_State" ColumnGroupName="Material" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Tech_Condition" HeaderText="采用标准" UniqueName="Material_Tech_Condition" ColumnGroupName="Material" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="规格" UniqueName="Rough_Spec"  ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="left" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="Mat_Unit" HeaderText="计量单位" UniqueName="Mat_Unit"  ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="共计需求量" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="left" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                     
                         <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="尺寸" UniqueName="Rough_Size"  ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                     
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" HeaderText="需求行号" ItemStyle-HorizontalAlign="left" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    </Columns>
                    <CommandItemTemplate>
                        请领物资信息列表--物资备料
                        <telerik:RadButton ID="RB_FillInApp" runat="server" Text="填写申请单" OnClientClicking="ShowWindow" CssClass="floatright" AutoPostBack="true"></telerik:RadButton>
				        <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                    </CommandItemTemplate>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <div style="width: 100%; float: left;"></div>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowApp" runat="server" Title="车间备料物资需求申请" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="1000px" Height="680px" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="申请成功！进入流程平台" Position="Center"
        AutoCloseDelay="4000" Width="350" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
</asp:Content>
