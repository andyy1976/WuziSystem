<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MDemandMergeListChange.aspx.cs" Inherits="mms.Plan.MDemandMergeListChange" %>

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
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->需求变更" ClientIDMode="Static" />
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Silk" ShowBaseLine="True" >
                <Tabs>
                    <telerik:RadTab Text="需要提交" TabIndex="0"></telerik:RadTab>
                    <telerik:RadTab Text="需求变更" TabIndex="1" Selected="true"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Query">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />                 
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" MinDisplayTime="0"></telerik:RadAjaxLoadingPanel>  
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">


            function EnterKeyProcessing(sender, eventArgs) {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }

            function ShowWin(ID) {
                window.radopen("/Plan/MDemandMergeListUpdate.aspx?MDMLID=" + ID, "RadWindow1");
            }
            //刷新页面
            function refreshGrid(arg) {
                if (!arg) {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div style="width: 100%; margin: 0px auto;">
       <div class="divSiteMap" style="width: 100%; float: left; height: 30px; border-bottom-style: solid; border-bottom-width:0px;">
                    <label style="float: left; margin-top: 0px;">型号：</label>
                    <span id="span_model" style="font-weight:bold; color:red;width:200px;" runat="server"></span>
                    <label style="float: left; margin-top: 0px;">计划包名称：</label>
                    <span id="span_PlanName" style="font-weight:bold; width:200px;" runat="server"></span>
                    <label style="float: left; margin-top: 0px;">对应型号投产计划编号：</label>
                    <span id="span_plancode" style="font-weight:bold;width:200px;" runat="server"></span>
                    <label style="float: left">材料清单编号：</label>
                    <span id="span_DraftCode" style="font-weight:bold;width:200px;" runat="server"></span>
        </div>
        
       <div style="width: 100%; height: 0px;  border-bottom-style: solid; border-bottom-width: 1px; margin: 5px 0; clear: both;"></div>

            <table style="text-align:left;">
                <tr>
                    <td style="text-align:right;">物资编码：</td>
                    <td><telerik:RadTextBox ID="RTB_ItemCode" runat="server" Width="120px">
                         <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td style="text-align:right;">任务号：</td>
                    <td><telerik:RadTextBox ID="RTB_Task" runat="server" Width="120px">
                         <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td style="text-align:right;">图号：</td>
                    <td><telerik:RadTextBox ID="RTB_Drawing_No" runat="server" Width="120px">
                         <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td style="text-align:right;">提交日期：</td>
                    <td>
                        <telerik:RadDatePicker ID="RDP_SubmitDateStart" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                        </telerik:RadDatePicker>
                    </td>
                    <td>～</td>
                    <td>
                        <telerik:RadDatePicker ID="RDP_SubmitDateEnd" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
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
                    <td>
                        <telerik:RadDatePicker ID="RDP_DemandDateStart" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                        </telerik:RadDatePicker>
                    </td>
                    <td>～</td>
                    <td>
                        <telerik:RadDatePicker ID="RDP_DemandDateEnd" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                        </telerik:RadDatePicker>
                    </td>
                    <td><telerik:RadButton ID="RB_Query" runat="server" Text="查询" OnClick="RB_Query_Click"></telerik:RadButton></td>
                </tr>
            </table>
            <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false"
                OnItemDataBound="RadGrid1_ItemDataBound" OnNeedDataSource="RadGrid1_NeedDataSource"
                AllowPaging="true" PageSize="15" PagerStyle-AlwaysVisible="True">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle Font-Size="12px" HorizontalAlign="Center" />
                <HeaderStyle Font-Size="13px" HorizontalAlign="Center"/>
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings Selecting-AllowRowSelect="true"  EnablePostBackOnRowClick="true" EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                </ClientSettings>
                <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                </ExportSettings>
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <Columns> 
                        <telerik:GridTemplateColumn HeaderText="修改" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            <ItemTemplate>
                                <telerik:RadButton ID="RB_ShowWin" runat="server" Text="修改需求" ButtonType="ToggleButton" ForeColor="Blue"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ID" HeaderText="需求行号" HeaderStyle-Width="70px" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TDM_Description" HeaderText="产品名称" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum1" HeaderText="需求件数" HeaderStyle-Width="70px" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量" HeaderStyle-Width="70px" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Dept" HeaderText="领料部门" HeaderStyle-Width="70px" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" HeaderStyle-Width="70px" ItemStyle-Width="170px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求时间" HeaderStyle-Width="120px" ItemStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UrgencyDegre" HeaderText="紧急程度" HeaderStyle-Width="70px" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" HeaderStyle-Width="70px" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UseDes" HeaderText="用途" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Manufacturer" HeaderText="生产厂家" HeaderStyle-Width="100px" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="State" HeaderText="状态" HeaderStyle-Width="70px" ItemStyle-Width="70px" Visible="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SUBMISSION_DATE" HeaderText="提交时间" HeaderStyle-Width="120px" ItemStyle-Width="120px"></telerik:GridBoundColumn>  


                     
                        </Columns>

                        <CommandItemTemplate>
                        <asp:Label ID="lbltop" runat="server">型号投产任务</asp:Label>
					     <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                    </MasterTableView>
                </telerik:RadGrid>
        </div>
    <%--弹出窗口--开始--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Title="修改需求" Left="50px" top="50px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="1100px" Height="600px" />
        </Windows>
    </telerik:RadWindowManager>
                <%--结束--%>
</asp:Content>
