<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="Exe_Inf.aspx.cs" Inherits="mms.Plan.Exe_Inf" %>

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
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资查询-->物资需求执行信息" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
             <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RB_Query">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"></telerik:RadCodeBlock>
    <div style="width: 100%; margin: 0px auto;">
        <div style="width: 100%; margin: 0px auto;">
            <table style="text-align:left;">
                <tr>
                    <td style="text-align:right;">任务类型：</td>
                    <td>
                        <telerik:RadDropDownList ID="RDDL_Task" runat="server" Width="120" AppendDataBoundItems="true">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="全部" />
                                <telerik:DropDownListItem Value="0" Text="型号投产计划任务" />
                                <telerik:DropDownListItem Value="1" Text="工艺试验件任务" />
                                <telerik:DropDownListItem Value="2" Text="技术创新课题任务" />
                                <telerik:DropDownListItem Value="3" Text="车间备料任务"/>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td style="text-align:right;">物资编码：</td>
                    <td><telerik:RadTextBox ID="RTB_ItemCode" runat="server" Width="120px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">任务号：</td>
                    <td><telerik:RadTextBox ID="RTB_Task" runat="server" Width="120px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">图号：</td>
                    <td><telerik:RadTextBox ID="RTB_Drawing_No" runat="server" Width="120px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">提交日期：</td>
                    <td><telerik:RadDatePicker ID="RDP_SubmitDateStart" runat="server" Width="120px"></telerik:RadDatePicker></td>
                    <td>～</td>
                    <td><telerik:RadDatePicker ID="RDP_SubmitDateEnd" runat="server" Width="120px"></telerik:RadDatePicker></td>
                </tr>
                <tr>
                    <td style="text-align:right;">需求行号：</td>
                    <td><telerik:RadTextBox ID="RTB_ID" runat="server" Width="120px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">紧急程度：</td>
                    <td><telerik:RadDropDownList ID="RDDL_Urgency_Degre" runat="server" Width="120px" AppendDataBoundItems="true">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="全部" />
                            </Items>
                        </telerik:RadDropDownList></td>
                    <td style="text-align:right;">领料部门：</td>
                    <td><telerik:RadDropDownList ID="RDDL_Dept" runat="server" Width="120px" AppendDataBoundItems="true">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="全部" />
                        </Items>
                        </telerik:RadDropDownList></td>
                    <td style="text-align:right;">密级：</td>
                    <td><telerik:RadDropDownList ID="RDDL_Secret_Level" runat="server" Width="120px" AppendDataBoundItems="true">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="全部" />
                        </Items>
                        </telerik:RadDropDownList></td>
                    <td>需求时间：</td>
                    <td><telerik:RadDatePicker ID="RDP_DemandDateStart" runat="server" Width="120px"></telerik:RadDatePicker></td>
                    <td>～</td>
                    <td><telerik:RadDatePicker ID="RDP_DemandDateEnd" runat="server" Width="120px"></telerik:RadDatePicker></td>
                    <td><telerik:RadButton ID="RB_Query" runat="server" Text="查询" OnClick="RB_Query_Click"></telerik:RadButton></td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; margin: 0px auto;">
            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource"
                AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True">
                <HeaderStyle Font-Size="13px" HorizontalAlign="Center"></HeaderStyle>
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings EnableRowHoverStyle="true" >
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3" ScrollHeight="600px"></Scrolling>
                </ClientSettings>
                    <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                          <MasterTableView DataKeyNames="ID" CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
						    物资需求执行信息列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                         <Columns>
                        <telerik:GridBoundColumn DataField="USER_RQ_LINE_ID" HeaderText="需求行号" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="User_Rq_Number" HeaderText="用户需求编号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Total_Pr_Quantity" HeaderText="累计采购计划数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Total_Po_Quantity" HeaderText="累计采购订单数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Total_Received_Quantity" HeaderText="累计到货数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Total_Inspected_Quantity" HeaderText="累计检验数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Total_Delivered_Quantity" HeaderText="累计入库数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Total_Shipped_Quantity" HeaderText="累计发放数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Total_Borrowed_Quantity" HeaderText="被借用数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TOTAL_RECEIVED_QUANTITY" HeaderText="累计锁定库存数量" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rq_Execution_Status" HeaderText="需求执行状态" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rq_Execution_Phase" HeaderText="执行进度" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rq_Status" HeaderText="需求状态" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Close_Reason" HeaderText="关闭原因" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Closed_By" HeaderText="关闭人" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Close_Date" HeaderText="需求关闭时间" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Last_Update_Date" HeaderText="最后更新时间" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>   
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
