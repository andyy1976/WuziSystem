<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MDemandDetails.aspx.cs" ValidateRequest="false" Inherits="mms.Plan.MDemandDetails" %>
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
    <asp:HiddenField ID="hfFlag" runat="server" />
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->型号投产任务-->物资需求清单" ClientIDMode="Static" />
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Silk" ShowBaseLine="True" >
        <Tabs>
            <telerik:RadTab Text="需要提交" TabIndex="0" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="需求变更" TabIndex="1"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800">
         <Scripts>
         <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
         <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
         <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
	</telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"  OnAjaxRequest="RadAjaxManager1_AjaxRequest">
      	 <ClientEvents OnRequestStart="onRequestStart" />
         <AjaxSettings>
          	  	  	    <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
           			        <UpdatedControls>
                	    <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandDetails" LoadingPanelID="RadAjaxLoadingPanel1" />
           				    </UpdatedControls>
          			    </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandDetails">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandDetails"/>
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                <telerik:AjaxUpdatedControl ControlID="hfFlag" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadBtn_Search">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandDetails" LoadingPanelID="RadAjaxLoadingPanel1" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />                 
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="chb_all">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandDetails"/>
                                <telerik:AjaxUpdatedControl ControlID="chb_all"/>
                                <telerik:AjaxUpdatedControl ControlID="hfFlag" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                       <telerik:AjaxSetting AjaxControlID="RadBtnBuildMergeList">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandDetails" LoadingPanelID="RadAjaxLoadingPanel1" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
            function refreshGrid(arg) {
                if (!arg)
                {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                }
            }

            function RefreshParent(sender, eventArgs)
            {
                document.location.reload();
            }

            function EnterKeyProcessing(sender, eventArgs)
            {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }
            function ShowMDemandMergeList()
            {
                var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                win.set_title("物资需求计划清单-待提交");
                window.radopen("/Plan/MDemandMergeList.aspx", "RadWindowRecordWindow");
                return false;
            }
            function confirmWindowSubmitMerge(sender, args)
            {
                var grid = $find("<%=RadGrid_MDemandDetails.ClientID%>");
                var flag = document.getElementById("ContentPlaceHolder1_hfFlag").value;
                var checkAll = document.getElementById("ContentPlaceHolder1_chb_all");
                if (flag == "0" && !checkAll.checked)
                {
                    var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                    rnal.set_text("请选择要提交的数据");
                    rnal.show();
                    args.set_cancel(true);
                 }
                else
                {
                     ShowMDemandMergeList();
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
                    <td>零件类型：</td>
                    <td>
                        <telerik:RadDropDownList ID="RDDL_LingJian_Type" runat="server" Width="100px" AppendDataBoundItems="true">
                           <Items><telerik:DropDownListItem Text="全部" Value="" /></Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>图号：</td>
                    <td><telerik:RadTextBox ID="RTB_Drawing_No" runat="server" Width="100px">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                      <td>工艺路线：</td>
                    <td><telerik:RadTextBox ID="Rad_TechLine" runat="server"  Width="100px">
                         <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td><telerik:RadButton ID="RB_Query" runat="server" Text="查询" OnClick="WZBH_Query_Click"></telerik:RadButton></td>
                </tr>
            </table>
       <div class="divSiteMap add_divSiteMap" style="clear: both; width: 100%;">
           <div class="div_addclass" style="float: right; height: 40px; line-height: 40px;">
                    <span style="font-size: 13px;color:red;"> 
					<asp:CheckBox ID="chb_all" runat="server" OnCheckedChanged="chb_all_CheckedChanged" AutoPostBack="True" Text="全选"/></span>
                    <span style="font-size: 13px;">未提交：</span>
                    <label id="lbl_state_0" runat="server" style="color: green; font-weight: bold;">0</label>
                    <span style="margin-left: 20px; font-size: 13px;">已提交：</span>
                    <label id="lbl_state_1" runat="server" style="color: blue; font-weight: bold;">0</label>
                    <span style="margin-left: 20px; font-size: 13px;">有更改可再提交：</span>
                    <label id="lbl_state_2" runat="server" style="color: red; font-weight: bold;">0</label>
                    <span style="margin-left: 20px; font-size: 13px;">缺失材料定额：</span>
                    <label id="lbl_state_4" runat="server" style="color: green; font-weight: bold;">0</label>
                    <span style="margin-left: 20px; font-size: 13px;">错误数据：</span>
                    <label id="lbl_state_5" runat="server" style="color: blue; font-weight: bold;">0</label>
                    <span style="margin-left: 20px; font-size: 13px;">有更改不可再提交：</span>
                    <label id="lbl_state_6" runat="server" style="color: red; font-weight: bold;">0</label>
                </div>
           <div style="clear: both; overflow: hidden"></div>
        </div>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  >
           <telerik:RadGrid ID="RadGrid_MDemandDetails" runat="server" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                    OnNeedDataSource="RadGrid_MDemandDetails_NeedDataSource"  OnItemDataBound="RadGrid_MDemandDetails_ItemDataBound"
                AllowPaging="true" PageSize="10" PagerStyle-AlwaysVisible="True" AllowSorting="true" AllowMultiRowSelection="True" AutoGenerateColumns="False">
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
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" AllowMultiColumnSorting="True" CommandItemDisplay="Top">
			    <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                        <Columns>
                             <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" ItemStyle-Width="50px" HeaderStyle-Width="50px" >
                              <ItemTemplate >
                                <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True" CssClass='<%#Eval("ID") %>' />
                              </ItemTemplate>
                              <HeaderTemplate>
                                <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True" />
                              </HeaderTemplate>
                            </telerik:GridTemplateColumn>
                           
                             
                            <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" HeaderText="需求行号" ItemStyle-Width="70px" HeaderStyle-Width="70px" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TaskCode"  HeaderText="任务编号" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="TaskCode" UniqueName="TaskCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Drawing_No"  HeaderText="图号" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Drawing_No" UniqueName="Drawing_No">
                            </telerik:GridBoundColumn>
                           
                            <telerik:GridBoundColumn DataField="TDM_Description" HeaderText="产品名称" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="TDM_Description" UniqueName="TDM_Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="LingJian_Type1" HeaderText="零件类型" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="LingJian_Type1" UniqueName="LingJian_Type1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Technics_Line"  HeaderText="工艺路线" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Technics_Line" UniqueName="Technics_Line">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" ItemStyle-Width="150px" HeaderStyle-Width="150px" SortExpression="Material_Name" UniqueName="Material_Name">
                            </telerik:GridBoundColumn>        
                            <telerik:GridBoundColumn DataField="Material_Mark" HeaderText="物资牌号" ItemStyle-Width="70px" HeaderStyle-Width="80px" SortExpression="Material_Name" UniqueName="Material_Name"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CN_Material_State" HeaderText="供应状态" ItemStyle-Width="70px" HeaderStyle-Width="80px" SortExpression="CN_Material_State" UniqueName="CN_Material_State"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Material_Tech_Condition" HeaderText="技术标准" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Material_Tech_Condition" UniqueName="Material_Tech_Condition"> </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="胚料规格" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Rough_Spec" UniqueName="Rough_Spec"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Dinge_Size" HeaderText="胚料尺寸" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Dinge_Size" UniqueName="Dinge_Size"></telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件质量" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Pro_Weight"  HeaderText="每产品质量" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="ItemCode1" UniqueName="ItemCode1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="需求尺寸" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Rough_Size" UniqueName="Rough_Size">
                            </telerik:GridBoundColumn>
                                    
                
                            <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="Special_Needs" UniqueName="Special_Needs" Visible="true">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="NumCasesSum"  HeaderText="需求件数" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Unit" HeaderText="计量单位" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Mat_Unit" UniqueName="Mat_Unit">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DemandNumSum"  HeaderText="需求数量" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                            </telerik:GridBoundColumn>


                            <telerik:GridBoundColumn DataField="Tech_Quantity"  HeaderText="工艺数量" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Tech_Quantity" UniqueName="Tech_Quantity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Technics_Comment" HeaderText="路线备注" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Technics_Comment" UniqueName="Technics_Comment">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Memo_Quantity"  HeaderText="备件数量" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Memo_Quantity" UniqueName="Memo_Quantity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Comment" HeaderText="定额备注" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Mat_Comment" UniqueName="Mat_Comment">
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求日期" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="DemandDate" UniqueName="DemandDate" Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UrgencyDegre" HeaderText="紧急程度" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="UrgencyDegre" UniqueName="UrgencyDegre" Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Secret_Level" UniqueName="Secret_Level" Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UseDes" HeaderText="用途" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="UseDes" UniqueName="UseDes" Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Shipping_Address" UniqueName="Shipping_Address" Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Certification" UniqueName="Certification" Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Attribute4" HeaderText="国产/进口" ItemStyle-Width="80px" HeaderStyle-Width="80px"  SortExpression="Attribute4" UniqueName="Attribute4">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Manufacturer" HeaderText="生产厂家" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="Manufacturer" UniqueName="Manufacturer" Visible="true">
                            </telerik:GridBoundColumn>

                       
                        </Columns>

                        <CommandItemTemplate>
                        <telerik:RadButton ID="RadBtnBuildMergeList" runat="server" Text="生成物资需求清单" Font-Bold="true" CommandName="BuildMergeList" OnClientClicking="confirmWindowSubmitMerge" CssClass="floatleft"></telerik:RadButton>
                        <asp:Label ID="lbltop" runat="server">型号物资需求清单</asp:Label>
					     <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                        </CommandItemTemplate>
                    </MasterTableView>
                </telerik:RadGrid>
                   </telerik:RadAjaxPanel>
    </div>

    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                    AutoCloseDelay="4000" Width="240" Height="90" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
    <%--弹出窗口--开始--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowRecordWindow" runat="server" Title="物资需求计划清单-待提交" Left="50px" Top="50px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close,Maximize,Minimize" OnClientClose="RefreshParent" Modal="true" Width="1200px" Height="500px" />
                    </Windows>
                </telerik:RadWindowManager>
    <%--结束--%>
</asp:Content>
