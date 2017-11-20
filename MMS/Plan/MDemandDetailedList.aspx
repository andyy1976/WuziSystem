<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MDemandDetailedList.aspx.cs" Inherits="mms.Plan.MDemandDetailedList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--<%@ Register Src="~/UserControls/WUCBusiNav1.ascx" TagPrefix="uc1" TagName="WUCBusiNav1" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/Plan/PlanJS.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->物资需求清单列表" ClientIDMode="Static" />
    <%--<uc1:WUCBusiNav1 runat="server" ID="WUCBusiNav1" />--%>
    <div style="width: 100%; float: left;">
        <div class="divContant">
        <div class="divSiteMap">
            <span>物资需求清单列表</span>
        </div>
        
        <div class="divViewPanel">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandDetailedList">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandDetailedList" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
                        //同步窗口开始
                        var SynchronID;
                        function confirmWindowSynchron(sender, args) {
                            $find("<%=confirmWindowSynchron.ClientID %>").show();
                        SynchronID = sender.get_id();
                        args.set_cancel(true);
                    }

                    function YesOrNoClickedSynchron(sender, args) {
                        var oWnd = $find("<%=confirmWindowSynchron.ClientID %>");
                        oWnd.close();
                        if (sender.get_text() == "是") {
                            $find(SynchronID).click();
                        }
                    }
                    //同步窗口结束
                    </script>
                </telerik:RadCodeBlock>
            <telerik:RadGrid ID="RadGrid_MDemandDetailedList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top" 
                OnNeedDataSource="RadGrid_MDemandDetailedList_NeedDataSource" OnItemCommand="RadGrid_MDemandDetailedList_ItemCommand" OnItemDataBound="RadGrid_MDemandDetailedList_ItemDataBound" PageSize="3">
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="序号" HeaderStyle-Width="40" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="draftid" FilterControlAltText="Filter draftid column" Visible="false" SortExpression="draftid" UniqueName="draftid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Model" FilterControlAltText="Filter Model column" Visible="false" SortExpression="Model" UniqueName="Model">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PlanCode" FilterControlAltText="Filter PlanCode column" Visible="false" SortExpression="PlanCode" UniqueName="PlanCode">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DraftCode" FilterControlAltText="Filter DraftCode column" HeaderText="材料清单草稿编号" HeaderStyle-Width="150" SortExpression="DraftCode" UniqueName="DraftCode">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="mstate" FilterControlAltText="Filter mstate column" HeaderText="材料定额状态" HeaderStyle-Width="150" SortExpression="mstate" UniqueName="mstate">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lstime" FilterControlAltText="Filter lstime column" HeaderText="上次同步定额时间" HeaderStyle-Width="150" SortExpression="lstime" UniqueName="lstime">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PlanCode" FilterControlAltText="Filter PlanCode column" HeaderText="计划编号" HeaderStyle-Width="150" SortExpression="PlanCode" UniqueName="PlanCode">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="tasktyle" FilterControlAltText="Filter tasktyle column" HeaderText="任务类型" HeaderStyle-Width="100" SortExpression="tasktyle" UniqueName="tasktyle">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserAccount" FilterControlAltText="Filter UserAccount column" HeaderText="清单编制人" HeaderStyle-Width="100" SortExpression="UserAccount" UniqueName="UserAccount">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="详细信息" UniqueName="DetailOper" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadButton ID="RadButtonDetail" runat="server" Text="查看详细" CommandName="Detail"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="同步操作" UniqueName="SynchronOper" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                            <ItemTemplate>
                                    <telerik:RadButton ID="RadButtonSynchron" runat="server" Text="查看BOM" CommandName="Synchron" OnClientClicking="confirmWindowSynchron"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <CommandItemTemplate>
                    </CommandItemTemplate>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        </div>
    </div>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
    <%-- 同步弹出窗口--开始--%>
    <telerik:RadWindow ID="confirmWindowSynchron" runat="server" VisibleTitlebar="false"
        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
        <ContentTemplate>
            <div style="margin-top: 30px; float: left;">
                <div style="width: 60px; padding-left: 15px; float: left;">
                    <img src="../Images/images/warnning1.jpg" alt="" />
                </div>
                <div style="width: 200px; float: left;">
                    <asp:Label ID="Label1" Font-Size="14px" Text="确定要同步吗？" runat="server" Font-Bold="true"
                        ForeColor="#25a0da" />
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="RadButton1" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClickedSynchron">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                    &nbsp;
                        <telerik:RadButton ID="RadButton2" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClickedSynchron">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </telerik:RadButton>
    </div>
</div>
        </ContentTemplate>
    </telerik:RadWindow>
    <%-- 同步弹出窗口--结束--%>
</asp:Content>
