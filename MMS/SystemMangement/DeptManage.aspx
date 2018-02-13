<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="DeptManage.aspx.cs" Inherits="mms.SystemMangement.DeptManage" %>

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
        function ShowAddr() {
            window.radopen("/SystemMangement/ShippingAddressManage.aspx", "RadWindow1");
        }
        //刷新页面
        function refreshGrid(arg) {
            if (arg) {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->部门信息管理" ClientIDMode="Static" />
    <div style="width: 100%; margin: 0px auto;">
        <div style="width: 100%; float: left;">
            <div class="divContant">
                <div class="divViewPanel">
                    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                        <Scripts>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                        </Scripts>
                    </telerik:RadScriptManager>
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                <ClientEvents OnRequestStart="onRequestStart" />
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="RadGrid_DeptManage">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_DeptManage" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_DeptManage" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="SqlDataSourceAddress" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>

                    <telerik:RadGrid ID="RadGrid_DeptManage" runat="server" AllowPaging="True" PageSize="20" PagerStyle-AlwaysVisible="True"
                        DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                        OnNeedDataSource="RadGrid_DeptManage_NeedDataSource" OnItemCommand="RadGrid_DeptManage_ItemCommand" OnItemDataBound="RadGrid_DeptManage_ItemDataBound">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <ItemStyle HorizontalAlign="Center" />
                        <AlternatingItemStyle HorizontalAlign="Center" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true">
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                        </ClientSettings>
                        <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
                        </ExportSettings>
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
						 <telerik:RadButton ID="RadButton_AddNew" runat="server" Text="新增部门" CommandName="InitInsert" CssClass="floatleft"></telerik:RadButton>
                         <telerik:RadButton ID="RB_Addr" runat="server" Text="配送地管理" AutoPostBack="false" OnClientClicking="ShowAddr" CssClass="floatleft" Visible="false"></telerik:RadButton>

                                部门信息列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
      
               
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsID" HeaderText="序号" ReadOnly="True" ItemStyle-Width="40px" HeaderStyle-Width="40px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Dept" HeaderText="部门名称" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DeptCode" HeaderText="部门编号" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="Cust_Account_ID" DataSourceID="SqlDataSourceCust_Account" ListTextField="Account_Name" ListValueField="Cust_Account_ID" HeaderText="物流中心对应部门" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridDropDownColumn>
                               <%-- <telerik:GridDropDownColumn DataField="ADDRESS" HeaderText="物流中心对应部门配送地址" ReadOnly="true" UniqueName="ADDRESS"></telerik:GridDropDownColumn>--%>
                                <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" UniqueName="Shipping_Address" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn DataField="Is_Del" HeaderText="删除标记" ReadOnly="True" ItemStyle-Width="100px" HeaderStyle-Width="100px"/>
                                <telerik:GridEditCommandColumn ButtonType="LinkButton" HeaderText="编辑"  ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridEditCommandColumn>
                                <telerik:GridTemplateColumn HeaderText="删除"  ItemStyle-Width="80px" HeaderStyle-Width="80px" UniqueName="DeleteColumn">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadButton ID="RadButtonDelete" runat="server" Text="删除" OnClientClicking="CustomRadWindowConfirm" CommandName="Delete"></telerik:RadButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <EditFormSettings UserControlName="DeptManage.ascx" EditFormType="WebUserControl"></EditFormSettings>
                    
                        </MasterTableView>
                    </telerik:RadGrid>
                    <asp:SqlDataSource ID="SqlDataSourceCust_Account" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>"
                        SelectCommand="select '0' as Cust_Account_ID, '' as Account_Name union  select Cust_Account_ID, Account_Name FROM GetCustInfo_T_CUST_ACCT WHERE Customer_ID = '276095' order by Account_Name"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SDS_Shipping_Address" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>"
                        SelectCommand="select '0' as KeyWordCode, '' as KeyWord union  select KeyWordCode, KeyWord FROM Dict WHERE TypeID = '2' order by KeyWord"></asp:SqlDataSource>
                    
                    <telerik:RadWindow ID="confirmDeleteWindow" runat="server" VisibleTitlebar="false"
                        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                        <ContentTemplate>
                            <div style="margin-top: 30px; float: left;">
                                <div style="width: 60px; padding-left: 15px; float: left;">
                                    <img src="/Images/images/warnning1.jpg" alt="" />
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
            </div>
        </div>
    </div>
    <%--配送地址管理弹出窗口--开始--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Title="配送地址管理" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="None" Modal="true" Width="600px" Height="520px" />
        </Windows>
    </telerik:RadWindowManager>
    <%-- 配送地址管理弹出窗口--结束--%>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
</asp:Content>
