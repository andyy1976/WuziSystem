<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MDemandChangeSubmit.aspx.cs" Inherits="mms.Plan.MDemandChangeSubmit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>        
        .RadAjaxPanel {
            float:left;
        }
        .screen_width {
            width:1366px;
        }
    </style>
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0) {

                args.set_enableAjax(false);

            }
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->型号任务变更提交" ClientIDMode="Static" />
<div style="width: 100%; float: left;">
    <div class="divContant" style="margin-top:0px;">
        <div class="divSiteMap add_divSiteMap" style="clear:both;width:100%;">
            <div style="float:left; height:40px;line-height:0px; "><h3 style="font-weight:bold">型号任务变更提交</h3></div>
            <div style="width:100%; height:0px; border:solid #000 1px; margin:5px 0; clear:both;"></div>
            <div style="clear:both; overflow:hidden"></div>
        </div>
        <%--<div class="divSiteMap" style="width:100%;float:none;height:30px;border-bottom-style: solid;border-bottom-width: 0px;">
            <label style="margin-left:10px;float:left">型号：</label><span id="span_model" style="float:left;" runat="server"></span>
        </div>--%>
        <div class="divViewPanel" style="width:100%;float:none;">
            
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                    </asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                    </asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                    </asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadCodeBlock runat="server">
                <script type="text/javascript">
                    function CloseRadWindow(args) {
                        var oWindow = null;
                        if (window.radWindow) oWindow = window.radWindow;
                        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                        //var oArg = new Object();
                        //oWindow.BrowserWindow.refreshGrid(args);
                        <%--var radalert = $find("<%=RadNotificationAlert.ClientID%>");
                        radalert.set_text("提交成功！");
                        radalert.show();--%>
                        oWindow.close();
                    }
                </script>
            </telerik:RadCodeBlock>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <ClientEvents OnRequestStart="onRequestStart" />
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandMergelist">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandMergelist" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="RadBtn_Search">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandMergelist" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                            <telerik:AjaxUpdatedControl ControlID="span_model" />
                            <telerik:AjaxUpdatedControl ControlID="span_listNo" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server"></telerik:RadAjaxLoadingPanel>
            <telerik:RadGrid ID="RadGrid_MDemandMergelist" runat="server" AllowPaging="false" DataKeyNames="ID" Culture="zh-CN" 
                GroupPanelPosition="Top" PageSize="10" AllowMultiRowSelection="true" skin="Silk"
                OnNeedDataSource="RadGrid_MDemandMergelist_NeedDataSource">
                <%--<ClientSettings Selecting-AllowRowSelect="true"></ClientSettings>--%>
                <ExportSettings HideStructureColumns="true" ExportOnlyData="true" />
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Bottom">
                    <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <%--<telerik:GridClientSelectColumn HeaderText=""></telerik:GridClientSelectColumn>--%>
                        <%--<telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号"  ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="需求行号" ReadOnly="True" SortExpression="ID" UniqueName="ID" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="Correspond_Draft_Code" FilterControlAltText="Filter Correspond_Draft_Code column" HeaderText="对应基准清单编号"  SortExpression="Correspond_Draft_Code" UniqueName="Correspond_Draft_Code">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="PackId" Visible="false" UniqueName="PackId"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TaskId" Visible="false" UniqueName="TaskId"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DraftId" Visible="false" UniqueName="DraftId"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Stage" Visible="false" UniqueName="Stage"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="state" Visible="false" UniqueName="state"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Drawing_No" FilterControlAltText="Filter Drawing_No column" HeaderText="产品图号" SortExpression="Drawing_No" UniqueName="Drawing_No" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号"  SortExpression="TaskCode" UniqueName="TaskCode" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="statetmp" FilterControlAltText="Filter statetmp column" HeaderText="变更状态" SortExpression="statetmp" UniqueName="statetmp" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Mat_Unit" FilterControlAltText="Filter Mat_Unit column" HeaderText="计量单位"  SortExpression="Mat_Unit" UniqueName="Mat_Unit" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column" HeaderText="物资件数"  SortExpression="Quantity" UniqueName="Quantity" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rough_Size" FilterControlAltText="Filter Rough_Size column" HeaderText="物资尺寸"  SortExpression="Rough_Size" UniqueName="Rough_Size" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rough_Spec" FilterControlAltText="Filter Rough_Spec column" HeaderText="物资规格"  SortExpression="Rough_Spec" UniqueName="Rough_Spec" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" FilterControlAltText="Filter DemandNumSum column" HeaderText="共计需求数量(kg)"  SortExpression="DemandNumSum" UniqueName="DemandNumSum" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum" FilterControlAltText="Filter NumCasesSum column" HeaderText="共计需求件数"  SortExpression="NumCasesSum" UniqueName="NumCasesSum" HeaderStyle-Width="110">
                        </telerik:GridBoundColumn>
                        
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true"  >
            </telerik:RadNotification>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>' SelectCommand="SELECT [KeyWordCode], [KeyWord] FROM [Sys_Dict] WHERE ([TypeID] = @TypeID)">
                <SelectParameters>
                    <asp:Parameter DefaultValue="2" Name="TypeID" Type="Int32"></asp:Parameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <div class="divViewPanel" style="width:1200px;float:none;">
            <asp:Button ID="BtnSubmit" runat="server" Text="提交变更清单" OnClick="BtnSubmit_Click" /></div>
    </div>
</div>
</form>
</body>
</html>
