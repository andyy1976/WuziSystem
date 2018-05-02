<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="TechnologyTestListChange.aspx.cs" Inherits="mms.Plan.TechnologyTestListChange" %>

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
    <asp:HiddenField ID="HiddenField" runat="server" Value="" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
	 <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
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
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function EnterKeyProcessing(sender, eventArgs) {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }
            function ShowWin(ID) {
                window.radopen("/Plan/TechnologyTestListUpdate.aspx?MDMLID=" + ID, "RadWindow1");
            }
            //刷新页面
            function refreshGrid(arg) {
                if (!arg) {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                }
            }
            function RefreshParent(sender, eventArgs) {
                document.location.reload();
            }

            function ShowTechnologyTestListDetails(MDP_Code, MDPID, SubmitType, Type) {
                var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                 var title = "";
                 switch (SubmitType) {
                     case "1":
                         title = "工艺试验件"; break;
                     case "2":
                         title = "技术创新课题"; break;
                     case "3":
                         title = "车间备料"; break;
                 }
                 win.set_title(title);
                 window.radopen("/Plan/TechnologyTest.aspx?changeSubmit=1&MDP_Code=" + MDP_Code + "&MDPID=" + MDPID +
                     "&SubmitType=" + SubmitType + "&Type=" + Type, "RadWindowRecordWindow");
                 return false;
             }
        </script>
    </telerik:RadCodeBlock>
    <div style="width:100%;">
        <div style="width:100%; float:left;">
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Default" ShowBaseLine="true">
                <Tabs>
                    <telerik:RadTab TabIndex="0" Value="0" Text="需求清单"></telerik:RadTab>
                    <telerik:RadTab TabIndex="1" Value="1" Text="更改需求" Selected="true"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
        </div>
                <div style="width: 100%; float: left;">
            <table>
                <tr>
                    <td>需求行号：</td>
                    <td><telerik:RadTextBox ID="RTB_ID" runat="server" Width="100px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td>型号：</td>
                    <td><telerik:RadTextBox ID="RTB_Project" runat="server" Width="100px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td>图号：</td>
                    <td><telerik:RadTextBox ID="RTB_Drawing_No" runat="server" Width="100px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td>任务号：</td>
                    <td><telerik:RadTextBox ID="RTB_TaskCode" runat="server" Width="100px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>

                    <td>物资名称：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Name" runat="server" Width="100px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td>物资编码：</td>
                    <td><telerik:RadTextBox ID="RTB_ItemCode1" runat="server" Width="100px">
                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td>申请时间：</td>
                    <td>
                        <telerik:RadDatePicker ID="RDPStart" runat="server" Width="100px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                        </telerik:RadDatePicker>
                      ～<telerik:RadDatePicker ID="RDPEnd" runat="server" Width="100px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                       </telerik:RadDatePicker>
                    </td>
                    <td><telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton></td>
                </tr>
            </table>
        </div>
        <div style="width:100%;">
            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" 
                OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                AllowPaging="true" PageSize="15" PagerStyle-AlwaysVisible="True" >
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings EnableRowHoverStyle="true" >
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="600px"></Scrolling>
                </ClientSettings>
                     <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                <MasterTableView  AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <CommandItemSettings ShowExportToExcelButton="True" ShowExportToWordButton="true" ShowExportToPdfButton="true"  ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>  
                        <telerik:GridTemplateColumn HeaderText="修改" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadButton ID="RB_ShowWin" runat="server" Text="修改需求"  ButtonType="ToggleButton" ForeColor="Blue"></telerik:RadButton>
                                <telerik:RadButton ID="RadButtonDetails" runat="server"  Text="查看详细" Visible="false" AutoPostBack="false"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID" HeaderText="需求行号" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="substate" HeaderText="变更提交状态" ItemStyle-Width="120px" HeaderStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Model" HeaderText="型号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" ItemStyle-Width="120px" HeaderStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号" ItemStyle-Width="120px" HeaderStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TDM_Description" HeaderText="产品名称" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum1" HeaderText="件数" ItemStyle-Width="60px" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量" ItemStyle-Width="60px" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Dept" HeaderText="领料部门" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SUBMISSION_DATE" HeaderText="申请时间" ItemStyle-Width="150px" HeaderStyle-Width="150px"></telerik:GridBoundColumn> 
                        <telerik:GridBoundColumn DataField="UrgencyDegre" HeaderText="紧急程度" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UseDes" HeaderText="用途" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求时间" ItemStyle-Width="100px" HeaderStyle-Width="100px" DataFormatString="{0:yyyy/MM/dd}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="State" HeaderText="状态" ItemStyle-Width="70px" HeaderStyle-Width="70px" Visible="false"></telerik:GridBoundColumn>

                    </Columns>
                    <CommandItemTemplate>
                        <asp:Label ID="lbltop" runat="server"></asp:Label>
				 	    <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                    </CommandItemTemplate>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
     <%--修改需求弹窗--开始--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
              <telerik:RadWindow ID="RadWindowRecordWindow" runat="server" Title="工艺试验件" Left="100px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close,Maximize,Minimize" Modal="true" Width="1300px" Height="620px" />
            <telerik:RadWindow ID="RadWindow1" runat="server" Title="修改需求" Left="150px" Top="20"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" OnClientClose="RefreshParent" Modal="true" Width="1300px" Height="600px" />
        </Windows>
    </telerik:RadWindowManager>
    <%--修改需求弹窗--结束--%>
      <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="350" Title="提示" EnableRoundedCorners="true"  >
        </telerik:RadNotification>
</asp:Content>
