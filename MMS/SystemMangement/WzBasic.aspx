<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="WzBasic.aspx.cs" Inherits="mms.SystemMangement.WzBasic" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
    <script>
           function onRequestStart(sender, args) {
               if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                   args.set_enableAjax(false);

               }
           }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:HiddenField ID="HiddenField" runat="server" Value="系统查询-->物资基础库查询" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
	</telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID ="RB_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID ="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
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
    <div style="width: 100%; float:left">
        <asp:Panel runat="server" id="Panel1">
            <table id="tableMT" style="margin:0px auto;float:left">
                <tr>
                    <td style="width:80px">物资名称：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Name" runat="server" Width="150px" >
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>

                    <td style="width:80px">物资编码：</td>
                    <td><telerik:RadTextBox ID="RTB_ItemCode" runat="server" Width="150px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td style="width:80px">物资牌号：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Paihao" runat="server" Width="150px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>

                    <td style="width:80px">物资规格:</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Guige" runat="server" Width="150px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>

                    <td style="width:80px">物资标准：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Biaozhun" runat="server" Width="150px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                     <td  style="width:200px" colspan="2"><telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton></td>

           </tr>
                 <%-- 
                <tr>
                    <td style="width:120px">物资类别：</td>
                    <td >
                        <telerik:RadDropDownList ID="RDDLMT" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="RDDLMT_SelectedIndexChanged" AutoPostBack="true" Width="150px">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                  
                    <td colspan="8">
                        <div id="div1" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td style="width:80px">二级</td>
                                    <td>
                                        <telerik:RadDropDownList ID="RDDLMT1" runat="server" OnSelectedIndexChanged="RDDLMT1_SelectedIndexChanged" AutoPostBack="true" Width="150px">
                                        </telerik:RadDropDownList>
                                    </td>
                                    <td style="width:80px">三级</td>
                                    <td>
                                        <telerik:RadDropDownList ID="RDDLMT2" runat="server" OnSelectedIndexChanged="RDDLMT2_SelectedIndexChanged" AutoPostBack="true" Width="150px">
                                        </telerik:RadDropDownList>
                                    </td>
                                    <td style="width:80px">四级</td>
                                    <td>
                                        <telerik:RadDropDownList ID="RDDLMT3" runat="server" OnSelectedIndexChanged="RDDLMT3_SelectedIndexChanged" AutoPostBack="true" Width="150px">
                                        </telerik:RadDropDownList>
                                    </td>
                                    <td style="width:80px">五级</td>
                                    <td>
                                        <telerik:RadDropDownList ID="RDDLMT4" runat="server" Width="150px">
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                   
                    </td>
                     
                </tr>
                     --%>
            </table>
        </asp:Panel>
    </div>
    <div style="width: 100%;">
        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource" AutoGenerateColumns="false" 
            AllowPaging="true" PageSize="15" PagerStyle-AlwaysVisible="True">
            <PagerStyle AlwaysVisible="true" />
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
            </ClientSettings>
              <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
              <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                          <MasterTableView  CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <CommandItemTemplate>
						  物资基础库查询
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
   
                <Columns>
                    <telerik:GridBoundColumn DataField="SEG3" HeaderText="物资编码" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SEG4" HeaderText="描述" headerstyle-HorizontalAlign="left" itemstyle-HorizontalAlign="left"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
</asp:Content>
