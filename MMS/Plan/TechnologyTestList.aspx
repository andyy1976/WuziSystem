<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="TechnologyTestList.aspx.cs" Inherits="mms.Plan.TechnologyTestList" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <asp:HiddenField ID="HiddenField" runat="server" Value="" ClientIDMode="Static" />
    <div style="width:100%; float:left;">
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Default" ShowBaseLine="true">
            <Tabs>
                <telerik:RadTab TabIndex="0" Value="0" Text="需求清单" Selected="true"></telerik:RadTab>
                <telerik:RadTab TabIndex="1" Value="1" Text="更改需求"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
    </div>
    <div style="width: 100%; float: left; margin-top:10px;">
        <div class="divContant">
            <div class="divViewPanel">
                <asp:Button ID="btnNewAdd" runat="server" Text="" AutoPostBack="false"/>
            </div>
            <div class="divViewPanel">
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
                        function ShowTechnologyTestListDetails(MDP_Code, MDPID, SubmitType,Type) {
                          var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                          var title = "";
                          switch (SubmitType) {
                            case "1":
                              title = "工艺试验件"; break;
                            case "2":
                              title = "技术创新课题"; break;
                            case "3":
                              title = "车间备料"; break;
                          }
                          win.set_title(title);
                            window.radopen("/Plan/TechnologyTest.aspx?MDP_Code=" + MDP_Code + "&MDPID=" + MDPID +
                                "&SubmitType=" + SubmitType + "&Type=" + Type, "RadWindowRecordWindow");
                            return false;
                        }
                        function ShowTechnologyTestAdd(MDPId, SubmitType) {
                            var win = $find("<%=RadWindowRecordWindow1.ClientID %>");
                            var title = "";
                            switch (SubmitType) {
                            case "1":
                                title = "新增工艺试验件"; break;
                            case "2":
                                title = "新增技术创新课题"; break;
                            case "3":
                                title = "新增车间备料"; break;
                            }
                            win.set_title(title);
                            window.radopen("/Plan/TechnologyTestAdd.aspx?MDPId=" + MDPId +"&SubmitType=" + SubmitType, "RadWindowRecordWindow1");
                            return false;
                        }
                        function refreshGrid(arg) {
                            if (!arg) {
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                            }
                        }
                    </script>
                </telerik:RadCodeBlock>
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_OnAjaxRequest">
                      <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_TechnologyTestList">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server"></telerik:RadAjaxLoadingPanel>

                <telerik:RadGrid ID="RadGrid_TechnologyTestList" runat="server" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"  
                    AllowPaging="True" PageSize="20" PagerStyle-AlwaysVisible="True"
                    OnNeedDataSource="RadGrid_TechnologyTestList_NeedDataSource" OnItemCommand="RadGrid_TechnologyTestList_ItemCommand" OnItemDataBound="RadGrid_TechnologyTestList_ItemDataBound">
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <ClientSettings EnableRowHoverStyle="true"  Selecting-AllowRowSelect="true" >
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="True" ScrollHeight="600px" UseStaticHeaders="True"></Scrolling>
                    </ClientSettings>
                     <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <CommandItemSettings ShowExportToExcelButton="True" ShowExportToWordButton="true" ShowExportToPdfButton="true"  ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号" ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MDP_Code" FilterControlAltText="Filter MDP_Code column" HeaderText="编号" SortExpression="MDP_Code" UniqueName="MDP_Code">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="subtype" FilterControlAltText="Filter subtype column" HeaderText="类型" SortExpression="subtype" UniqueName="subtype">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="substate" FilterControlAltText="Filter substate column" HeaderText="提交状态" SortExpression="substate" UniqueName="substate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SubmitCount" HeaderText="已接受/总数"></telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="reState" FilterControlAltText="Filter reState column" HeaderText="退回状态" SortExpression="reState" UniqueName="reState">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" Visible="false" SortExpression="ID" UniqueName="ID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column" HeaderText="操作员" SortExpression="UserName" UniqueName="UserName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Submit_Date" FilterControlAltText="Filter Submit_Date column" HeaderText="提交时间" SortExpression="Submit_Date" UniqueName="Submit_Date">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Submit_State" Visible="false" UniqueName="Submit_State">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="操作" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <telerik:RadButton ID="RadButtonDetails" runat="server"  Text="查看详细" Visible="false" AutoPostBack="false"></telerik:RadButton>
                                    <%--<telerik:RadButton ID="RadBtnChange" runat="server"  Text="变更申请" Visible="false" AutoPostBack="false"></telerik:RadButton>--%>
                                    <telerik:RadButton ID="RadBtnSubmit" runat="server"  Text="继续申请" Visible="false" AutoPostBack="false"></telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <CommandItemTemplate>
                            <asp:Label ID="lbltop" runat="server"></asp:Label>
				 	    <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                    </MasterTableView>
                </telerik:RadGrid>
                <%--弹出详细窗口--开始--%>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowRecordWindow" runat="server" Title="工艺试验件" Left="100px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close,Maximize,Minimize" Modal="true" Width="1300px" Height="620px" />
                        <telerik:RadWindow ID="RadWindowRecordWindow1" runat="server" Title="新增工艺试验件" Left="100px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close,Maximize,Minimize" Modal="true" Width="1300px" Height="620px" />
                    </Windows>
                </telerik:RadWindowManager>
                <%--结束--%>
            </div>
            
        </div>
    </div>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
</asp:Content>
