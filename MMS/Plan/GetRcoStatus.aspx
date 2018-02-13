<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="GetRcoStatus.aspx.cs" Inherits="mms.Plan.GetRcoStatus" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
          function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 ||args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 ||args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0 )
            {

                args.set_enableAjax(false);

            }
        }

 

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资查询-->需求变更申请状态变更" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
	</telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
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
        <div style="width:100%;">
 <table style="text-align:left;">
                <tr>
                      <td>变更申请编号：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_RCO_No" runat="server" Width="120px">
                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                   
                    <td style="text-align:right;">物资名称：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Name" runat="server" Width="120px">
                        <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
              
                  
              
             
                     <td><telerik:RadButton ID="RB_Query" runat="server" Text="查询" OnClick="RB_Query_Click"></telerik:RadButton></td>
                </tr>
            </table>
        </div>
        <div style="width:100%;">
            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" 
                AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True"
                OnItemDataBound="RadGrid1_ItemDataBound" OnNeedDataSource="RadGrid1_NeedDataSource" >
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                </ClientSettings>
                  <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
                         <telerik:RadButton ID="RB_Add" runat="server" Text="新增计划包" CssClass="floatleft" AutoPostBack="false" OnClientClicked="ImportPlan" Visible="false"></telerik:RadButton>
						    需求变更申请状态变更信息列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
               
                    <Columns>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Column_Changed" HeaderText="变更字段" HeaderStyle-Width="100px" UniqueName="Column_Changed"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Original_Value" HeaderText="原值" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Changed_Value" HeaderText="变更值" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RCO_No" HeaderText="变更申请编号" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="User_RCO_No" HeaderText="用户变更<br />申请号" HeaderStyle-Width="180px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="User_RCO_ID" HeaderText="用户变更<br />申请ID" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RCO_Status" HeaderText="变更申<br />请状态" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Confirmed_Quantity" HeaderText="确认需<br />求数量" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Confirmed_RQ_Date" HeaderText="确认需<br />求时间" HeaderStyle-Width="80px" DataFormatString="{0:yyyy/MM/dd}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Test_material_Quantity" HeaderText="化验料<br />数量" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Process_Approach" HeaderText="变更处<br />理方式" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LAST_STATUS_CHANGE_DATE" HeaderText="状态最后<br />更新时间" HeaderStyle-Width="80px" DataFormatString="{0:yyyy/MM/dd}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute1" HeaderText="需求单位变更<br />申请单行ID" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                   </Columns>                    
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
