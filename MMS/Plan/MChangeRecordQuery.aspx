<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MChangeRecordQuery.aspx.cs" Inherits="mms.Plan.MChangeRecordQuery" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->物资需求更改列表" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
	</telerik:RadScriptManager>
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
                                <telerik:DropDownListItem Value="0" Text="型号计划任务" />
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
                    <td style="text-align:right;"><%--提交状态：--%></td>
                    <td><telerik:RadDropDownList ID="RDDL_State" runat="server" Width="120px" AppendDataBoundItems="true" Visible="false">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="全部" />
                            <telerik:DropDownListItem Value="0" Text="未提交" />
                            <telerik:DropDownListItem Value="1" Text="已提交" />
                        </Items>
                        </telerik:RadDropDownList></td>
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
                AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True" OnItemDataBound="RadGrid1_ItemDataBound">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings EnableRowHoverStyle="true" >
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="600px"></Scrolling>
                </ClientSettings>
                  <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                <MasterTableView DataKeyNames="ID" Width="100%" CommandItemDisplay="Top">
                      <CommandItemSettings ShowExportToWordButton="true" ShowExportToCsvButton="true" ShowExportToPdfButton="true"  ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                         <CommandItemTemplate>
						    物资更改列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
    
   
                    <Columns>
                        <telerik:GridBoundColumn DataField="USER_RCO_HEADER_NO" HeaderText="用户变更申请号"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID" HeaderText="需求行号"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="产品名称"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Dept" HeaderText="领料部门"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求时间"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UrgencyDegre" HeaderText="紧急程度"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UseDes" HeaderText="用途"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Column_Changed" HeaderText="更改字段" UniqueName="Column_Changed"></telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Original_Value" HeaderText="原值"></telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="Changed_Value" HeaderText="变更值"></telerik:GridBoundColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
