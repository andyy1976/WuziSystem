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
                <telerik:RadTab TabIndex="1" Value="1" Text="更改需求" Visible="false"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
    </div>

    <div style="width: 100%; float: left; margin-top:10px;">
              <div style="width: 100%; float: left;">
            <table>
                <tr>
                    <td>编号：</td>
                    <td><telerik:RadTextBox ID="RTB_MDP_Code" runat="server" Width="100px">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td>提交状态/备料状态：</td>  
                    <td><telerik:RadDropDownList ID="RDDL_AppState" runat="server" Width="100px">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="全部" />
                            <telerik:DropDownListItem Value="0" Text="未提交" />
                            <telerik:DropDownListItem Value="1" Text="进入流程平台" />
                            <telerik:DropDownListItem Value="2" Text="已审批已通过" />
                            <telerik:DropDownListItem Value="3" Text="已审批未通过" />
                            <telerik:DropDownListItem Value="4" Text="已提交物流" />
                        </Items>
                      </telerik:RadDropDownList>
                    </td>
                    <td>申请时间：</td>
                    <td>
                        <telerik:RadDatePicker ID="RDPStart" runat="server" Width="100px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                        </telerik:RadDatePicker>
                      ～<telerik:RadDatePicker ID="RDPEnd" runat="server" Width="100px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                       </telerik:RadDatePicker>
                    </td>
                    <td><telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton></td>
                </tr>
            </table>
        </div>
        <div class="divContant">
            <div class="divViewPanel">
                <asp:Button ID="btnNewAdd" runat="server" Text="" />
            </div>
            <div class="divViewPanel">
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">

                        function EnterKeyProcessing(sender, eventArgs) {
                            var c = eventArgs.get_keyCode();
                            if ((c == 13)) {
                                eventArgs.set_cancel(true);
                            }
                        }
                        function RefreshParent(sender, eventArgs) {
                            document.location.reload();
                        }
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
                                "&SubmitType=" + SubmitType + "&Type=" + Type + "&changeSubmit=0", "RadWindowRecordWindow");
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
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_OnAjaxRequest">
                      <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_TechnologyTestList">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList"  />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RB_Search_Click">
                            <UpdatedControls>
                                 <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList"  />
                             </UpdatedControls>
                          </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>

                <telerik:RadGrid ID="RadGrid_TechnologyTestList" runat="server" Culture="zh-CN" GroupPanelPosition="Top"  
                    AllowPaging="True" PageSize="15" PagerStyle-AlwaysVisible="True"
                    OnNeedDataSource="RadGrid_TechnologyTestList_NeedDataSource" OnItemCommand="RadGrid_TechnologyTestList_ItemCommand" OnItemDataBound="RadGrid_TechnologyTestList_ItemDataBound">
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <ClientSettings EnableRowHoverStyle="true"  Selecting-AllowRowSelect="true" >
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="True" ScrollHeight="600px" UseStaticHeaders="True"></Scrolling>
                    </ClientSettings>
                     <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <CommandItemSettings ShowExportToExcelButton="True" ShowExportToWordButton="true" ShowExportToPdfButton="true"  ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号" ItemStyle-HorizontalAlign="Center" ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MDP_Code" FilterControlAltText="Filter MDP_Code column" HeaderText="编号" ItemStyle-HorizontalAlign="Center" SortExpression="MDP_Code" UniqueName="MDP_Code">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="subtype" FilterControlAltText="Filter subtype column" HeaderText="类型" ItemStyle-HorizontalAlign="Center" SortExpression="subtype" UniqueName="subtype">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="substate" FilterControlAltText="Filter substate column" HeaderText="提交状态" ItemStyle-HorizontalAlign="Center" SortExpression="substate" UniqueName="substate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SubmitCount" HeaderText="已接受/总数" ItemStyle-HorizontalAlign="Center" SortExpression="SubmitCount" UniqueName="SubmitCount"> </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="reState" FilterControlAltText="Filter reState column" HeaderText="退回状态" SortExpression="reState" UniqueName="reState">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" Visible="false" SortExpression="ID" UniqueName="ID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column" HeaderText="操作员"  ItemStyle-HorizontalAlign="Center" SortExpression="UserName" UniqueName="UserName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Submit_Date" FilterControlAltText="Filter Submit_Date column" HeaderText="提交时间"  ItemStyle-HorizontalAlign="Center" SortExpression="Submit_Date" UniqueName="Submit_Date">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="操作"  ItemStyle-HorizontalAlign="Center">
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
                            Behaviors="Close,Maximize,Minimize" OnClientClose="RefreshParent" Modal="true" Width="1300px" Height="620px" />
                    </Windows>
    </telerik:RadWindowManager>
        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="350" Title="提示" EnableRoundedCorners="true"  >
        </telerik:RadNotification>
                     <%--结束--%>
            </div>
        </div>
    </div>
</asp:Content>
