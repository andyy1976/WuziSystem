<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="WriteReqOrderRecList.aspx.cs" Inherits="mms.Plan.WriteReqOrderRecList" %>

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
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资查询-->需求与变更错误查询" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID ="RB_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID ="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID ="RB_SearchRco">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID ="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <script type="text/javascript">
             function EnterKeyProcessing(sender, eventArgs) {
                 var c = eventArgs.get_keyCode();
                 if ((c == 13)) {
                     eventArgs.set_cancel(true);
                 }
             }
        </script>
    </telerik:RadCodeBlock>
    <div style="width:100%;">
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Default" MultiPageID="RadMultiPage1" ShowBaseLine="true">
            <Tabs>
                <telerik:RadTab Text="需求错误列表" TabIndex="0" Selected="true"></telerik:RadTab>
                <telerik:RadTab Text="变更错误列表" TabIndex="1"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
    </div>
    <div style="width:100%;">
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
            <telerik:RadPageView runat="server" Selected="true">
                <div style="width:100%;">
                    <table>
                        <tr>
                            <td>任务类型：</td>
                            <td><telerik:RadDropDownList ID="RDDL_TaskType" runat="server" Width="120px">
                                <Items>
                                    <telerik:DropDownListItem Value="" Text="全部" />
                                    <telerik:DropDownListItem Value="0" Text="型号投产计划任务" />
                              <%--  <telerik:DropDownListItem Value="1" Text="工艺试验任务" /> --%>
                                    <telerik:DropDownListItem Value="2" Text="创新课题任务" />
                                    <telerik:DropDownListItem Value="3" Text="车间备料任务" />
                                </Items>
                            </telerik:RadDropDownList></td>
                            <td>提交时间：</td>
                            <td>
                                <telerik:RadDatePicker ID="RDP_Start" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                </telerik:RadDatePicker>
                            </td>
                            <td>～</td>
                            <td>
                                <telerik:RadDatePicker ID="RDP_End" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                </telerik:RadDatePicker>
                            </td>
                            <td>需求计划ID：</td>
                            <td><telerik:RadTextBox ID="RTB_HeaderID" runat="server" Width="120px">
                                                           <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                </telerik:RadTextBox></td>
                            <td>需求行号：</td>
                            <td><telerik:RadTextBox ID="RTB_LineID" runat="server" Width="120px">
                                                           <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                </telerik:RadTextBox></td>
                            <td><telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton></td>
                        </tr>
                    </table>
                </div>
                <div style="width:100%;">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" PagerStyle-AlwaysVisible="true"
                        OnNeedDataSource="RadGrid1_NeedDataSource">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders ="true" ScrollHeight="500px" />
                        </ClientSettings>
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />

                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
                        </ExportSettings>
                          <MasterTableView DataKeyNames="RowsId" CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
						    物流中心返回错误列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>     
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ItemStyle-Width="60px" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="USER_RQ_LINE_ID" HeaderText="需求行号" Visible="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ERR_MSG" HeaderText="提交失败原因" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Dinge_Size" HeaderText="胚料尺寸" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="规格" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="需求尺寸" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Urgency_Degre" HeaderText="紧急程度" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Use_Des" HeaderText="用途" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MANUFACTURER" HeaderText="生产厂家" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView runat="server">
                <div style="width:100%;">
                    <table>
                        <tr>
                            <td>任务类型：</td>
                            <td><telerik:RadDropDownList ID="RDDL_TaskTypeRco" runat="server" Width="120px">
                                <Items>
                                    <telerik:DropDownListItem Value="" Text="全部" />
                                    <telerik:DropDownListItem Value="0" Text="型号投产计划任务" />
                                  <%--   <telerik:DropDownListItem Value="1" Text="工艺试验任务" /> --%>
                                    <telerik:DropDownListItem Value="2" Text="创新课题任务" />
                                    <telerik:DropDownListItem Value="3" Text="车间备料任务" />
                                </Items>
                            </telerik:RadDropDownList></td>
                            <td>提交时间：</td>
                            <td> 
                                <telerik:RadDatePicker ID="RDP_StartRco" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                 </telerik:RadDatePicker>
                            </td>
                            <td>～</td>
                            <td>
                                <telerik:RadDatePicker ID="RDP_EndRco" runat="server" Width="120px"  DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                </telerik:RadDatePicker>
                            </td>
                            <td>需求行ID：</td>
                            <td><telerik:RadTextBox ID="RTB_RQ_LineId" runat="server" Width="120px">
                                                           <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                </telerik:RadTextBox></td>
                            <td>变更行ID：</td>
                            <td><telerik:RadTextBox ID="RTB_RCO_LintId" runat="server" Width="120px">
                                                           <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                </telerik:RadTextBox></td>
                            <td><telerik:RadButton ID="RB_SearchRco" runat="server" Text="搜索" OnClick="RB_SearchRco_Click"></telerik:RadButton></td>
                        </tr>
                    </table>
                </div>
                <div style="width:100%;">
                    <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" PagerStyle-AlwaysVisible="true"
                        OnNeedDataSource="RadGrid2_NeedDataSource">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders ="true" ScrollHeight="500px" />
                        </ClientSettings>
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <MasterTableView DataKeyNames="RowsId" CommandItemDisplay="Top">
                            <CommandItemTemplate>
                                物流中心返回错误列表--变更
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ItemStyle-Width="60px" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="USER_RQ_LINE_ID" HeaderText="需求行号" Visible="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ERR_MSG" HeaderText="提交失败原因" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Column_Changed" HeaderText="变更信息" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Original_Value" HeaderText="原值" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Changed_Value" HeaderText="变更值" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Dinge_Size" HeaderText="胚料尺寸" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="规格" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="需求尺寸" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Urgency_Degre" HeaderText="紧急程度" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Use_Des" HeaderText="用途" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MANUFACTURER" HeaderText="生产厂家" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
        
    </div>
</asp:Content>
