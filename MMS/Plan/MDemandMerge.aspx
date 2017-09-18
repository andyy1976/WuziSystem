<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MDemandMerge.aspx.cs" Inherits="mms.Plan.MDemandMerge" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
    <script src="../Scripts/Plan/PlanJS.js"></script>
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->物物资需求清单列表(合并)" ClientIDMode="Static" />
<div style="width: 100%; float: left;">
    <div class="divContant">
        <div class="divSiteMap">
            <span id="span_title" runat="server">物资需求清单列表(合并)</span>
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
                    <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandMerge">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandMerge" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server"></telerik:RadAjaxLoadingPanel>

            <telerik:RadGrid ID="RadGrid_MDemandMerge" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                OnNeedDataSource="RadGrid_MDemandMerge_NeedDataSource" OnItemCommand="RadGrid_MDemandMerge_ItemCommand" OnItemDataBound="RadGrid_MDemandMerge_ItemDataBound" PageSize="3">
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <Columns>
                        <telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号" HeaderStyle-Width="40" ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Is_Submit" FilterControlAltText="Filter Is_Submit column" Visible="false" SortExpression="Is_Submit" UniqueName="Is_Submit">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MergeList_Code" FilterControlAltText="Filter MergeList_Code column" HeaderText="编号" HeaderStyle-Width="150" SortExpression="MergeList_Code" UniqueName="MergeList_Code">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="MDP_Code" FilterControlAltText="Filter MDP_Code column" HeaderText="编号" HeaderStyle-Width="150" SortExpression="MDP_Code" UniqueName="MDP_Code">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="Model" FilterControlAltText="Filter Model column" HeaderText="型号" HeaderStyle-Width="50" SortExpression="Model" UniqueName="Model">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Draft_Code" FilterControlAltText="Filter Draft_Code column" HeaderText="基准物资编码" HeaderStyle-Width="150" SortExpression="Draft_Code" UniqueName="Draft_Code">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" Visible="false" SortExpression="ID" UniqueName="ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserAccount" FilterControlAltText="Filter UserAccount column" HeaderText="操作员" HeaderStyle-Width="60" SortExpression="UserAccount" UniqueName="UserAccount">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Submit_Date" FilterControlAltText="Filter Submit_Date column" HeaderText="提交时间" HeaderStyle-Width="100" SortExpression="Submit_Date" UniqueName="Submit_Date">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsSubmit" FilterControlAltText="Filter IsSubmit column" HeaderText="提交状态" HeaderStyle-Width="60" SortExpression="IsSubmit" UniqueName="IsSubmit">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="详细信息" UniqueName="DetailOper" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadButton ID="RadButtonDetail" runat="server" Text="查看详细" CommandName="Detail"></telerik:RadButton>
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
</asp:Content>
