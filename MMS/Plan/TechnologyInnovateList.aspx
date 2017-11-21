<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="TechnologyInnovateList.aspx.cs" Inherits="mms.Plan.TechnologyInnovateList" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="天津公司技术创新课题任务-->技术创新课题申请列表" ClientIDMode="Static" />
    <hr style="margin-top:0px;" />
    <div style="width: 100%; float: left;">
        <div class="divContant">
            <div class="divViewPanel">
                <asp:Button ID="btnNewAdd" runat="server" Text="新增技术创新课题申请" AutoPostBack="false"/>
            </div>
            <div class="divViewPanel">
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
                        function ShowTechnologyTestListDetails(MDP_Code, MDPID, SubmitType, Type) {
                            var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                            win.set_title("技术创新课题");
                            window.radopen("/Plan/TechnologyTest.aspx?MDP_Code=" + MDP_Code + "&MDPID=" + MDPID +
                                "&SubmitType=" + SubmitType + "&Type=" + Type, "RadWindowRecordWindow");
                            return false;
                        }
                        function ShowTechnologyInnovateAdd(MDPId) {
                            var win = $find("<%=RadWindowRecordWindow1.ClientID %>");
                            win.set_title("新增工艺试验件");
                            window.radopen("/Plan/TechnologyInnovateAdd.aspx?MDPId=" + MDPId, "RadWindowRecordWindow1");
                            return false;
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
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                              <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_TechnologyInnovateList">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyInnovateList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server"></telerik:RadAjaxLoadingPanel>

                <telerik:RadGrid ID="RadGrid_TechnologyInnovateList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top" 
                    OnNeedDataSource="RadGrid_TechnologyInnovateList_NeedDataSource" OnItemCommand="RadGrid_TechnologyInnovateList_ItemCommand" PageSize="50"
                    OnItemDataBound="RadGrid_TechnologyInnovateList_ItemDataBound">
                          <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                         <CommandItemSettings ShowExportToWordButton="true" ShowExportToCsvButton="true" ShowExportToPdfButton="true"  ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                         <CommandItemTemplate>

                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号" ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MDP_Code" FilterControlAltText="Filter MDP_Code column" HeaderText="编号" SortExpression="MDP_Code" UniqueName="MDP_Code">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="subtype" FilterControlAltText="Filter subtype column" HeaderText="类型" SortExpression="subtype" UniqueName="subtype">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="substate" FilterControlAltText="Filter substate column" HeaderText="提交状态" SortExpression="substate" UniqueName="substate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="reState" FilterControlAltText="Filter reState column" HeaderText="退回状态" SortExpression="reState" UniqueName="reState">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" Visible="false" SortExpression="ID" UniqueName="ID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UserAccount" FilterControlAltText="Filter UserAccount column" HeaderText="操作员" SortExpression="UserAccount" UniqueName="UserAccount">
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
                    </MasterTableView>
                </telerik:RadGrid>
                <%--弹出窗口--开始--%>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowRecordWindow" runat="server" Title="技术创新课题" Left="150px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close,Maximize,Minimize" Modal="true" Width="1300px" Height="620px" />
                        <telerik:RadWindow ID="RadWindowRecordWindow1" runat="server" Title="新增技术创新课题" Left="100px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close,Maximize,Minimize" Modal="true" Width="1300px" Height="620px" />
                    </Windows>
                </telerik:RadWindowManager>
                <%--结束--%>
            </div>
        </div>
    </div>
</asp:Content>
