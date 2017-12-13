<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="InventoryQuery.aspx.cs" Inherits="mms.OutOfStorageManagement.InventoryQuery" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
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
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资查询-->出库单查询" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
	</telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
         
            <telerik:AjaxSetting AjaxControlID="RB_Query1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID ="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
			   <telerik:AjaxSetting AjaxControlID="RB_Query2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1" />
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
    <asp:HiddenField ID="HF_DeptID" runat="server" />
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true" Skin="Default">
        <Tabs>
            <telerik:RadTab Value="0" Text="库存查询" Selected="true"></telerik:RadTab>
            <telerik:RadTab Value="1" Text="出入库流水查询"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
            <div style="width:100%; margin-top:20px;">
                日期：<telerik:RadDatePicker ID="RDP_Start1" runat="server" Width="100px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                      </telerik:RadDatePicker>
                    ～<telerik:RadDatePicker ID="RDP_End1" runat="server" Width="100px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                      </telerik:RadDatePicker>
                物资名称：<telerik:RadTextBox ID="RTB_Material_Name1" runat="server" Width="100px">
                                                    <ClientEvents OnKeyPress="EnterKeyProcessing" />
                     </telerik:RadTextBox>
                物资编码：<telerik:RadTextBox ID="RTB_ItemCode11" runat="server" Width="100px">
                                                    <ClientEvents OnKeyPress="EnterKeyProcessing" />
                     </telerik:RadTextBox>
                <telerik:RadButton ID="RB_Query1" runat="server" Text="库存查询" OnClick="RB_Query1_Click"></telerik:RadButton>
            </div>
            <div style="width:100%;">
                <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource"
                    AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True" AutoGenerateColumns="false" AllowMultiRowSelection="true">
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                    <ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="True" ScrollHeight="600px" UseStaticHeaders="true"></Scrolling>
                    </ClientSettings>
                        <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
                        </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
				
                              库存查询
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
      
                        <Columns>
                            <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="InitialNumber" HeaderText="期初库存数量" ItemStyle-HorizontalAlign="Right"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StorageQuantity" HeaderText="入库数量" ItemStyle-HorizontalAlign="Right"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="OutgoingQuantity" HeaderText="出库数量" ItemStyle-HorizontalAlign="Right"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FinalNumber" HeaderText="期末库存数量" ItemStyle-HorizontalAlign="Right"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">
            <div style="width:100%; margin-top:20px;">
                出入库：<telerik:RadDropDownList ID="RDDL_BD" runat="server" Width="100px">
                    <Items>
                        <telerik:DropDownListItem Value="" Text="全部" />
                        <telerik:DropDownListItem Value="1" Text="入库" />
                        <telerik:DropDownListItem Value="2" Text="出库" />
                    </Items>
                    </telerik:RadDropDownList>
                日期：<telerik:RadDatePicker ID="RDP_Start2" runat="server" Width="100px"></telerik:RadDatePicker>
                ～<telerik:RadDatePicker ID="RDP_End2" runat="server" Width="100px"></telerik:RadDatePicker>
                物资名称：<telerik:RadTextBox ID="RTB_Material_Name2" runat="server" Width="100px"></telerik:RadTextBox>
                物资编码：<telerik:RadTextBox ID="RTB_ItemCode12" runat="server" Width="100px"></telerik:RadTextBox>
                <telerik:RadButton ID="RB_Query2" runat="server" Text="出入库流水" OnClick="RB_Query2_Click"></telerik:RadButton>
            </div>
            <div style="width:100%;">
                <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGrid2_NeedDataSource"
                    AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True">
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                    <ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="True" ScrollHeight="600px" UseStaticHeaders="true"></Scrolling>
                    </ClientSettings>
                    <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                          <MasterTableView  CommandItemDisplay="Top">
						  <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />

					    <CommandItemTemplate>
						 出入库流水查询
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click2" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click2"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click2"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Type1" HeaderText="出入库" UniqueName="Type1"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" UniqueName="Material_Name"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" UniqueName="ItemCode1"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Type" HeaderText="材料类型" UniqueName="Material_Type"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Mark" HeaderText="物资牌号" UniqueName="Material_Mark"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_State" HeaderText="物资状态" UniqueName="Material_State"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Tech_Condition" HeaderText="技术条件" UniqueName="Material_Tech_Condition"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="坯料规格" UniqueName="Rough_Spec"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="物资尺寸" UniqueName="Rough_Size"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Mat_Unit" HeaderText="计量单位" UniqueName="Mat_Unit"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件质量" UniqueName="Mat_Rough_Weight"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Quantity" HeaderText="出库数量" ItemStyle-HorizontalAlign="Right"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserName" HeaderText="操作人"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OpeTime" HeaderText="操作时间"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
</asp:Content>
