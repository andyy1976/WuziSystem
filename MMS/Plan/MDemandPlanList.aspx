<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MDemandPlanList.aspx.cs" Inherits="mms.Plan.MDemandPlanList" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script>
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                args.set_enableAjax(false);

            }
        }
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
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; float: left;">
    <div class="divContant">
        <div class="divSiteMap">
            <h2>物资需求计划清单详细-提交版</h2>
        </div>
        <div class="divSiteMap">
            <span style="float:left">编号：</span><span id="span_xqjhCode" runat="server" style="float:left"></span>
            <span style="margin-left:50px;float:left">型号：</span><span id="span_model" runat="server" style="float:left"></span>
            <span style="margin-left:50px;float:left">基准物资材料清单号：</span><span id="span_listNo" runat="server" style="float:left"></span>
        </div>
        <div class="divViewPanel" style="width:100%;">
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
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <ClientEvents OnRequestStart="onRequestStart" />
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandPlanList">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandPlanList" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="RadBtn_Search">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandPlanList" LoadingPanelID="RadAjaxLoadingPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="span_hbxqCode" />
                            <telerik:AjaxUpdatedControl ControlID="span_model" />
                            <telerik:AjaxUpdatedControl ControlID="span_listNo" />
                            <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
            <telerik:RadGrid ID="RadGrid_MDemandPlanList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top" PageSize="10" 
                OnNeedDataSource="RadGrid_MDemandPlanList_NeedDataSource">
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
                        <%--<telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号" ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="行号" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Is_Change_1" FilterControlAltText="Filter Is_Change_1 column" HeaderText="变更状态" SortExpression="Is_Change_1" UniqueName="Is_Change_1">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="需求行号" ReadOnly="True" Visible="false" SortExpression="ID" UniqueName="ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DRAWING_NO" FilterControlAltText="Filter DRAWING_NO column"  HeaderText="产品图号" SortExpression="DRAWING_NO" UniqueName="DRAWING_NO">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ddate" FilterControlAltText="Filter ddate column" HeaderText="需求时间" SortExpression="ddate" UniqueName="ddate">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum" FilterControlAltText="Filter NumCasesSum column" HeaderText="共计需求量" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" FilterControlAltText="Filter DemandNumSum column" HeaderText="共计需求件数" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MAT_UNIT" FilterControlAltText="Filter MAT_UNIT column" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column" HeaderText="物资件数" SortExpression="Quantity" UniqueName="Quantity">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="schedule" FilterControlAltText="Filter schedule column" HeaderText="备料进度" SortExpression="schedule" UniqueName="schedule">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="ROUGH_SIZE" FilterControlAltText="Filter ROUGH_SIZE column" HeaderText="物资尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MaterialsDes" FilterControlAltText="Filter MaterialsDes column" HeaderText="物资描述" SortExpression="MaterialsDes" UniqueName="MaterialsDes">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SpecialNeeds" FilterControlAltText="Filter SpecialNeeds column" HeaderText="特殊需求" SortExpression="SpecialNeeds" UniqueName="SpecialNeeds">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UrgencyDegre" FilterControlAltText="Filter UrgencyDegre column" HeaderText="紧急程度" SortExpression="UrgencyDegre" UniqueName="UrgencyDegre">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SecretLevel" FilterControlAltText="Filter SecretLevel column" HeaderText="密级" SortExpression="SecretLevel" UniqueName="SecretLevel">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="stage1" FilterControlAltText="Filter stage1 column" HeaderText="研制阶段" SortExpression="stage1" UniqueName="stage1">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UseDes" FilterControlAltText="Filter UseDes column" HeaderText="用途" SortExpression="UseDes" UniqueName="UseDes">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TechnicsLine" FilterControlAltText="Filter TechnicsLine column" HeaderText="领料部门" SortExpression="TechnicsLine" UniqueName="TechnicsLine">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="KeyWord" FilterControlAltText="Filter KeyWord column" HeaderText="配送地址" SortExpression="KeyWord" UniqueName="KeyWord">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Certification1" FilterControlAltText="Filter Certification1 column" HeaderText="合格证" SortExpression="Certification1" UniqueName="Certification1">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="Unit_Price" FilterControlAltText="Filter Unit_Price column" HeaderText="单价" SortExpression="Unit_Price" UniqueName="Unit_Price">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Sum_Price" FilterControlAltText="Filter Sum_Price column" HeaderText="总价" SortExpression="Sum_Price" UniqueName="Sum_Price">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MDMId" FilterControlAltText="Filter MDMId column" Visible="false" SortExpression="MDMId" UniqueName="MDMId">
                        </telerik:GridBoundColumn>
                    </Columns>
           
                 
                </MasterTableView>
            </telerik:RadGrid>
            <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                AutoCloseDelay="4000" Width="240" Height="90" Title="提示" EnableRoundedCorners="true">
            </telerik:RadNotification>
        </div>
        <div class="divSiteMap add_divSiteMap" style="clear: both; width: 100%; " runat="server" id="divListTitle">
            <div style="float: left; height: 40px; line-height: 40px;">
                <h3 style="font-weight: bold">需求变更申请单</h3>
            </div>
        </div>
        <div style="width: 100%; float: left; margin-top:10px;" runat="server" id="divListContent">
            <telerik:RadGrid ID="RadGrid_ChangeRecord" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top" PageSize="20"
                OnNeedDataSource="RadGrid_ChangeRecord_NeedDataSource">
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Bottom">
                    <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="RowsId" DataType="System.Int32" FilterControlAltText="Filter RowsId column" HeaderText="序号" ReadOnly="True" SortExpression="RowsId" UniqueName="RowsId">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MDMId" FilterControlAltText="Filter MDMId column" HeaderText="原需求行号" SortExpression="MDMId" UniqueName="MDMId">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MDPId" Visible="false" UniqueName="MDPId"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Correspond_Draft_Code" Visible="false" UniqueName="Correspond_Draft_Code"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Change_Code" FilterControlAltText="Filter Change_Code column" HeaderText="变更单据" SortExpression="Change_Code" UniqueName="Change_Code">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Change_Evidence_Id" FilterControlAltText="Filter Change_Evidence_Id column" HeaderText="变更凭据" SortExpression="Change_Evidence_Id" UniqueName="Change_Evidence_Id">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Change_Date" FilterControlAltText="Filter Change_Date column" HeaderText="变更时间" SortExpression="Change_Date" UniqueName="Change_Date">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="zt" FilterControlAltText="Filter zt column" HeaderText="变更属性" SortExpression="zt" UniqueName="zt">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ysz" FilterControlAltText="Filter ysz column" HeaderText="原始值" SortExpression="ysz" UniqueName="ysz">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="bgz" FilterControlAltText="Filter bgz column" HeaderText="变更值" SortExpression="bgz" UniqueName="bgz">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MReduce_Num_1" FilterControlAltText="Filter MReduce_Num_1 column" HeaderText="减少数量" SortExpression="MReduce_Num_1" UniqueName="MReduce_Num_1">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</div>
</form>
</body>
</html>
