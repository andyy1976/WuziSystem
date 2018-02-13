<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="GetRqStatus.aspx.cs" Inherits="mms.Plan.GetRqStatus" %>

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
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资查询-->物资需求申请状态" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
	</telerik:RadScriptManager>
   
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
             <ClientEvents OnRequestStart="onRequestStart" />
      
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
         <div style="width: 100%; margin: 0px auto;">
            <table style="text-align:left;">
                <tr>
                      <td>需求行号：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_ID" runat="server" Width="120px">
                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                   
                    <td style="text-align:right;">物资名称：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Name" runat="server" Width="120px">
                        <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
              
                    <td style="text-align:right;"><%--确认日期：--%></td>
                    <td>
                        <telerik:RadDatePicker ID="RDP_ConfirmDateStart" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing' Visible="false">
                        </telerik:RadDatePicker>
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadDatePicker ID="RDP_ConfirmDateEnd" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing' Visible="false">
                        </telerik:RadDatePicker>
                    </td>
                    <td style="text-align:right;">提交状态：</td>
                    <td><telerik:RadDropDownList ID="RDDL_State" runat="server" Width="120px" AppendDataBoundItems="true" Visible="true">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="全部" />
                            <telerik:DropDownListItem Value="物流已认领" Text="物流已认领" />
                            <telerik:DropDownListItem Value="物流已确认" Text="物流已确认" />
                            <telerik:DropDownListItem Value="用户已提交" Text="用户已提交" />
                            <telerik:DropDownListItem Value="物流已退回" Text="物流已退回" />
                        </Items>
                        </telerik:RadDropDownList></td>
                     <td><telerik:RadButton ID="RB_Query" runat="server" Text="查询" OnClick="RB_Query_Click"></telerik:RadButton></td>
                </tr>
            </table>
        </div>
        <div style="width:100%;">
            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" HtmlEncode="false" OnNeedDataSource="RadGrid1_NeedDataSource" 
                AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="6"></Scrolling>
                    <Selecting AllowRowSelect="true" />
                    </ClientSettings>
                     <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
						   物资需求申请状态列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
             
                    <Columns>
                        <telerik:GridBoundColumn DataField="RQ_No" HeaderText="需求编号" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="User_RQ_ID" HeaderText="需求ID" HeaderStyle-Width="70px" DataFormatString="{0:F0}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="User_RQ_Line_ID" HeaderText="需求行号" HeaderStyle-Width="70px" DataFormatString="{0:F0}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求数" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="User_RQ_No" HeaderText="用户需求编号" HeaderStyle-Width="150px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Confirmed_Quantity" HeaderText="确认需求数量" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Confirmed_RQ_Date" HeaderText="确认需求时间" HeaderStyle-Width="150px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Confirmed_Item_ID" HeaderText="确认物资编码" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Confirmed_Item_Revision" HeaderText="确认物资版本" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Test_material_Quantity" HeaderText="化验料数量" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Submission_Status" HeaderText="提交状态" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Claimed_By" HeaderText="认领人" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Department_Of_Planner" HeaderText="认领人<br />所属部门" HeaderStyle-Width="150px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Claim_Date" HeaderText="认领时间" HeaderStyle-Width="150px" DataFormatString="{0:yyyy/MM/dd}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Confirmed_By" HeaderText="确认人" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Confirmation_Date" HeaderText="确认时间" HeaderStyle-Width="150px" DataFormatString="{0:yyyy/MM/dd}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SUBMITED_SYNC_STATUS" HeaderText="提交状态最后更新时间" HeaderStyle-Width="150px" DataFormatString="{0:yyyy/MM/dd}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute1" HeaderText="确认用途" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute2" HeaderText="确认生产厂家" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute3" HeaderText="确认特殊要求" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute4" HeaderText="签订合同周期" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute5" HeaderText="检测周期" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute6" HeaderText="生产周期" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute7" HeaderText="配送周期" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute8" HeaderText="固定工作周期" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute9" HeaderText="物资供应周期" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute10" HeaderText="起订量" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute11" HeaderText="标准供货日期" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute12" HeaderText="是否<br />超期" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute13" HeaderText="是否应收<br />预付款" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute14" HeaderText="预付款比例" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute15" HeaderText="预付款单价" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute16" HeaderText="预付款<br />金额" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute17" HeaderText="确认<br />件数" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        
                    </Columns>
                    
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
