<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MDemandPlan.aspx.cs" Inherits="mms.Plan.MDemandPlan" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script>
        $(function () {
            $(".rgRow").css("background-color", "#fff");
            $(".rgAltRow").css("background-color", "#ccc");
            $(".rgRow").hover(function () {
                $(this).css("background-color", "#ddedfb");
            }, function () {
                $(this).css("background-color", "#fff");
            })
            $(".rgAltRow").hover(function () {
                $(this).css("background-color", "#ddedfb");
            }, function () {
                $(this).css("background-color", "#ccc");
            })
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->物资需求计划清单（提交版）" ClientIDMode="Static" />
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server">
        <Tabs>
            <telerik:RadTab Text="需要提交"  TabIndex="0"></telerik:RadTab>
            <telerik:RadTab Text="已提交清单" Selected="true"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <hr style="margin-top:-1px;" />
<div style="width: 100%; float: left;">
    <div class="divContant">
        <div class="divSiteMap">
            <h2>物资需求计划清单（提交版）</h2>
        </div>
        
        <div class="divViewPanel">
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    function ShowMDemandPlanDetails(MDP_Code,MDPID) {
                        var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                        win.set_title("物资需求计划清单-详细信息");
                        window.radopen("/Plan/MDemandPlanList.aspx?MDP_Code=" + MDP_Code + "&MDPID=" + MDPID, "RadWindowRecordWindow");
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
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandPlan">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandPlan" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server"></telerik:RadAjaxLoadingPanel>

            <telerik:RadGrid ID="RadGrid_MDemandPlan" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top" 
                OnNeedDataSource="RadGrid_MDemandPlan_NeedDataSource" OnItemCommand="RadGrid_MDemandPlan_ItemCommand" PageSize="50"
                OnItemDataBound="RadGrid_MDemandPlan_ItemDataBound">
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号" ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MDP_Code" FilterControlAltText="Filter MDP_Code column" HeaderText="编号" SortExpression="MDP_Code" UniqueName="MDP_Code">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Model" FilterControlAltText="Filter Model column" HeaderText="型号" SortExpression="Model" UniqueName="Model">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PlanCode" FilterControlAltText="Filter PlanCode column" HeaderText="投产计划编号" SortExpression="PlanCode" UniqueName="PlanCode">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Draft_Code" FilterControlAltText="Filter Draft_Code column" HeaderText="基准物资编码" SortExpression="Draft_Code" UniqueName="Draft_Code">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" Visible="false" SortExpression="ID" UniqueName="ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserAccount" FilterControlAltText="Filter UserAccount column" HeaderText="操作员" SortExpression="UserAccount" UniqueName="UserAccount">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Submit_Date" FilterControlAltText="Filter Submit_Date column" HeaderText="提交时间" SortExpression="Submit_Date" UniqueName="Submit_Date">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="schedule" FilterControlAltText="Filter schedule column" HeaderText="完成进度" SortExpression="schedule" UniqueName="schedule">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="操作"  ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadButton ID="RadButtonDetails" runat="server"  Text="查看详细" AutoPostBack="false"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <%--弹出窗口--开始--%>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowRecordWindow" runat="server" Title="物资需求计划清单-详细信息" Left="150px"
                        ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                        Behaviors="Close,Maximize,Minimize" Modal="true" Width="1200px" Height="620px" />
                </Windows>
            </telerik:RadWindowManager>
            <%--结束--%>
        </div>
    </div>
</div>
</asp:Content>
