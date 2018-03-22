<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MaterialApplicationQuery.aspx.cs" Inherits="mms.MaterialApplicationCollar.MaterialApplicationQuery" %>

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
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资申请-->申请单状态查询" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800">
	</telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMA" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridMA">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMA" />
                    <telerik:AjaxUpdatedControl ControlID="HFMDMLID" />
                    <telerik:AjaxUpdatedControl ControlID="HFType" />
                    <telerik:AjaxUpdatedControl ControlID="HFMAID" />
                    <telerik:AjaxUpdatedControl ControlID="HFAppSate1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMA" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Edit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" LoadingPanelID="RadAjaxLoadingPanel1" />
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

            function refreshGrid(arg) {
                if (!arg) {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                }
            }

            //删除
            var deleteID;
            function confirmRadWindowDelete(sender, args) {
                var grid = $find("<%=RadGridMA.ClientID%>");
                if (grid._selectedIndexes.length == 0) {
                    $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择申请行");
                    $find("<%=RadNotificationAlert.ClientID%>").show();
                    args.set_cancel(true);
                    return;
                } else {
                    var appstate = $get("<%=HFAppSate1.ClientID%>").value;
                    if (appstate == "已退回" || appstate == "已申请,未审批" || appstate == "取消审批" || appstate=="已审批未通过") {
                        $find("<%= RadWindowDelete.ClientID %>").show();
                        deleteID = sender.get_id();
                        args.set_cancel(true);
                    } else {
                        $find("<%=RadNotificationAlert.ClientID%>").set_text("该申请单不可以删除！");
                        $find("<%=RadNotificationAlert.ClientID%>").show();
                        args.set_cancel(true);
                    }
                }
            }
            function YesOrNoClickedDelete(sender, args) {
                var oWnd = $find("<%=RadWindowDelete.ClientID %>");
                oWnd.close();
                if (sender.get_text() == "是") {
                    $find(deleteID).click();
                }
            }
            function ShowWindow(sender, args) {
                var grid = $find("<%=RadGridMA.ClientID%>");
                if (grid._selectedIndexes.length == 0) {
                    $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择申请行");
                    $find("<%=RadNotificationAlert.ClientID%>").show();
                    return;
                } else {
                    var win = $find("<%=RadWindowManager1.ClientID %>");
                    var appstate = $get("<%=HFAppSate1.ClientID%>").value;
                    if (appstate == "已退回" || appstate == "已申请,未审批" || appstate == "取消审批" || appstate == "已审批未通过") {
                        var MDMLID = $get("<%=HFMDMLID.ClientID%>").value;
                        var Type = $get("<%=HFType.ClientID%>").value;
                        var MAID = $get("<%=HFMAID.ClientID%>").value;
                        if (MAID == "") {
                            $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择申请行");
                            $find("<%=RadNotificationAlert.ClientID%>").show();
                            return;
                        }
                        var win = $find("<%=RadWindowManager1.ClientID %>");
                        win.set_title("型号物资申请");
                        window.radopen("/MaterialApplicationCollar/MaterialAppWindow.aspx?Type=" + Type + "&MDMLID=" + MDMLID + "&MAID=" + MAID, "RadWindowApp");
                    } else {
                        $find("<%=RadNotificationAlert.ClientID%>").set_text("该申请单不可以修改！");
                        $find("<%=RadNotificationAlert.ClientID%>").show();
                    }
                }
            }
            function ShowK2(MAID) {
                var win = $find("<%=RadWindowManager1.ClientID %>");
                win.set_title("型号物资申请");
                window.radopen("/MaterialApplicationCollar/MaterialApplicationApprove.aspx?MAID=" + MAID, "RadWindowK2");
            }
        </script>
    </telerik:RadCodeBlock>
    <asp:HiddenField ID="HF_DeptCode" runat="server" />
    <asp:HiddenField ID="HFMDMLID" runat="server" />
    <asp:HiddenField ID="HFType" runat="server" />
    <asp:HiddenField ID="HFMAID" runat="server" />
    <asp:HiddenField ID="HFAppSate1" runat="server" />
    <div style="width: 100%; float: left;">
        需求行号：<telerik:RadTextBox ID="RTB_ID" runat="server" Width="100px">
                                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
             </telerik:RadTextBox>
        任务号：<telerik:RadTextBox ID="RTB_TaskCode" runat="server" Width="100px">
                                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
            </telerik:RadTextBox>
        图号：<telerik:RadTextBox ID="RTB_DrawingNo" runat="server" Width="100px">
                                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
           </telerik:RadTextBox>
        类型：<telerik:RadDropDownList ID="RDDL_Type" runat="server" Width="100px">
            <Items>
                <telerik:DropDownListItem Value="" Text="全部" />
                <telerik:DropDownListItem Value="0" Text="型号投产" />
                <telerik:DropDownListItem Value="2" Text="技术创新" />
                <telerik:DropDownListItem Value="3" Text="车间备料" />
                <telerik:DropDownListItem Value="4" Text="无需求" />
            </Items>
        </telerik:RadDropDownList>
       申请单状态：<telerik:RadDropDownList ID="RDDL_AppState" runat="server" Width="100px">
            <Items>
                <telerik:DropDownListItem Value="" Text="全部" />
                <telerik:DropDownListItem Value="1" Text="未进入流程平台" />
                <telerik:DropDownListItem Value="2" Text="进入流程平台" />
               
                <telerik:DropDownListItem Value="4" Text="已审批已通过" />
                <telerik:DropDownListItem Value="5" Text="已审批未通过" />
                <telerik:DropDownListItem Value="6" Text="物流中心退单" />
            </Items>
        </telerik:RadDropDownList>
        申请时间：<telerik:RadDatePicker ID="RDPStart" runat="server" Width="100px"  DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
             </telerik:RadDatePicker>
            ～<telerik:RadDatePicker ID="RDPEnd" runat="server" Width="100px"  DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
             </telerik:RadDatePicker>
        物资编码：<telerik:RadTextBox ID="RTB_ItemCode1" runat="server" Width="100px">
                                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
             </telerik:RadTextBox>
      
        <telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton>
    </div>
    <div style="width: 100%; float: left;">
        <telerik:RadGrid ID="RadGridMA" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
            OnNeedDataSource="RadGridMA_NeedDataSource" OnItemCommand="RadGridMA_ItemCommand" OnItemDataBound="RadGridMA_OnItemDataBound" OnSelectedIndexChanged="RadGridMA_SelectedIndexChanged"
                AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True" AllowSorting="true">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle Font-Size="12px" HorizontalAlign="Center" />
                <HeaderStyle Font-Size="13px" HorizontalAlign="Center"/>
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings Selecting-AllowRowSelect="true"  EnablePostBackOnRowClick="true" EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                </ClientSettings>
                <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                </ExportSettings>
                <MasterTableView DataKeyNames="ID" CommandItemDisplay="Top">
			    <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />

                <ColumnGroups>
                        <telerik:GridColumnGroup Name="Applicant" HeaderText="申请信息" ></telerik:GridColumnGroup>
                    <telerik:GridColumnGroup Name="Material" HeaderText="物资信息" ></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="ProductInfo" HeaderText="产品信息" ></telerik:GridColumnGroup>
                </ColumnGroups>
           
                <Columns>
                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ColumnGroupName="Applicant"  ItemStyle-Width="30px" HeaderStyle-Width="30px"></telerik:GridClientSelectColumn>
                    <telerik:GridBoundColumn DataField="Material_Id" UniqueName="Material_Id" HeaderText="需求行号" ColumnGroupName="Applicant" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ColumnGroupName="Applicant" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Quantity" HeaderText="请领件数" ColumnGroupName="Applicant" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Mat_Unit" HeaderText="计量单位" UniqueName="Mat_Unit" ColumnGroupName="Applicant" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PleaseTakeQuality" HeaderText="申请数量" ColumnGroupName="Applicant" DataFormatString="{0:N0}" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Type1"  UniqueName="Type1" HeaderText="申请类型" ColumnGroupName="Applicant" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AppState1" UniqueName="AppState1" HeaderText="申请单状态" ColumnGroupName="Applicant"  ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="需求尺寸" UniqueName="Rough_Size" ColumnGroupName="Applicant" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="Applicant" HeaderText="申请人" ColumnGroupName="Applicant" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicationTime" HeaderText="申请时间" ColumnGroupName="Applicant" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="FeedingTime" HeaderText="需求时间" ColumnGroupName="Applicant" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Model" HeaderText="型号" ColumnGroupName="ProductInfo" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="true"></telerik:GridBoundColumn>
             
                    <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" ColumnGroupName="ProductInfo" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Stage1" HeaderText="阶段" ColumnGroupName="ProductInfo" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TDM_Description" HeaderText="产品名称" ColumnGroupName="ProductInfo" UniqueName="TDM_Description" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号" ColumnGroupName="ProductInfo" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ItemCode2" HeaderText="物资编码" ColumnGroupName="Material" UniqueName="ItemCode2" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" UniqueName="Material_Name" ColumnGroupName="Material" ItemStyle-Width="120px" HeaderStyle-Width="120px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Material_Mark" HeaderText="牌号" UniqueName="Material_Mark" ColumnGroupName="Material" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CN_Material_State" HeaderText="供应状态" UniqueName="CN_Material_State" ColumnGroupName="Material" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Material_Tech_Condition" HeaderText="采用标准" UniqueName="Material_Tech_Condition" ColumnGroupName="Material" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="规格" UniqueName="Rough_Spec" ColumnGroupName="Material" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Dinge_Size" HeaderText="胚料尺寸" UniqueName="Dinge_Size" ColumnGroupName="Material" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件定额质量" UniqueName="Mat_Rough_Weight" ColumnGroupName="Material" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" UniqueName="Special_Needs" ColumnGroupName="Material" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn HeaderText ="查询流程<br />平台信息"  ItemStyle-Width="70px" HeaderStyle-Width="70px">
                        <ItemTemplate>
                        <telerik:RadButton ID="RB_K2" runat="server" Text="查看" CommandName="K2" ButtonType="ToggleButton" ToggleType="None" AutoPostBack="false" ForeColor="blue"></telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
   			    <CommandItemTemplate>
                    <telerik:RadButton ID="RB_Delete" runat="server" Text="删除" CommandName="del" CssClass="floatleft" OnClientClicking="confirmRadWindowDelete"></telerik:RadButton>
                    <telerik:RadButton ID="RB_Edit" runat="server" Text="修改"  Font-Bold="true" OnClientClicking="ShowWindow" CssClass="floatleft" AutoPostBack="true"  ></telerik:RadButton>
                    申请单列表
				    <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                    <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                    <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                </CommandItemTemplate>
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
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowApp" runat="server" Title="型号物资申请" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="1000px" Height="660px" />
            <telerik:RadWindow ID="RadWindowK2" runat="server" Title="型号物资申请" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="800px" Height="400px" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
</asp:Content>
