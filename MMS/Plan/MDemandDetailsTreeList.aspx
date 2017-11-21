<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MDemandDetailsTreeList.aspx.cs" Inherits="mms.Plan.MDemandDetailsTreeList" %>
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
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->型号投产任务-->物资需求清单合并" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800">
         <Scripts>
         <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
         <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
         <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
	</telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"  OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadTreeList1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1"  />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtn_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1"  />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />   
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chb_all">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTreeList1"/>
                    <telerik:AjaxUpdatedControl ControlID="chb_all"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
            function AlphabetOnly(sender, eventArgs) {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }

            function refreshGrid(arg) {
                if (!arg)
                {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                }
            }
            function refreshGrid1()
            {

                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind1");

            }
            function RefreshParent(sender, eventArgs)
            {
                document.location.reload();
            }

            function AlphabetOnly(sender, eventArgs)
            {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }
            var submintId;

            function YesOrNoClicked(sender, args)
            {
                var oWnd = $find("<%=RadWindow.ClientID %>");
                            oWnd.close();
                            if (sender.get_text() == "是") {
                                $find(submintId).click();
                            }
             }
            function ShowMDemandCombineList(itemCode)
            {

                var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                win.set_title("物资需求计划合并清单");

                window.radopen("/Plan/MDemandDetailsCombine.aspx?PackId=" + "<%= Request.QueryString["PackId"].ToString() %>" + "&itemCode1=" + itemCode, "RadWindowRecordWindow");
             }
            function confirmWindowSubmitCancel(sender, args)
            {
                var treeList = $find('<%= RadTreeList1.ClientID %>');

                var selectedItems = treeList.get_selectedItems();
                if (selectedItems.length <= 0)
                {
                    var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                rnal.set_text("请选择一条以红颜色标记的合并过的数据记录");
                                rnal.show();
                                args.set_cancel(true);

                }
                else if (selectedItems.length > 1)
                {
                                var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                rnal.set_text("请勿选择两条（含）以上的数据记录");
                                rnal.show();
                                args.set_cancel(true);
                                return;
                 }
                else
                {
                    //  var combine_State = $(selectedItems[0].get_cell("Combine_State")).text();
                    var combine_State = treeList.getCellByColumnUniqueName(selectedItems[0], "Combine_State").innerText;
                    var parentId_For_Combine = treeList.getCellByColumnUniqueName(selectedItems[0], "ParentId_For_Combine").innerText;

                    if (combine_State == 1 && parentId_For_Combine == 0)
                    {
                        //ShowMDemandCombineList(temp);
                        $find("<%= RadWindow.ClientID %>").show();
                                    submintId = sender.get_id();
                                    args.set_cancel(true);


                    }
                    else
                    {
                                    var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                    rnal.set_text("请选择一条以红颜色标记的合并过的数据记录");
                                    rnal.show();
                                    args.set_cancel(true)
                     }

                }
             }

            function confirmWindowSubmitCombineClicking(sender, args)
            {
                var treeList = $find('<%= RadTreeList1.ClientID %>');

                var selectedItems = treeList.get_selectedItems();
                if (selectedItems.length <= 0)
                {
                    var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                rnal.set_text("请选择要合并的数据");
                                rnal.show();
                                args.set_cancel(true);
                }
                else
                {
                    var combineparentid = treeList.getCellByColumnUniqueName(selectedItems[0], "ParentId_For_Combine").innerText;
                    if (combineparentid != 0)
                    {
                        var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                    rnal.set_text("请务选择已经合并过的数据记录");
                                    rnal.show();
                                    args.set_cancel(true);
                     }
                    else
                    {
                        var temp = treeList.getCellByColumnUniqueName(selectedItems[0], "ItemCode1").innerText;
                        //  var temp = selectedItems[0]["ItemCode1"].text;
                        for (var i = 1; i < selectedItems.length; i++)
                        {

                            var combineparentid = treeList.getCellByColumnUniqueName(selectedItems[i], "ParentId_For_Combine").innerText;
                            if (combineparentid != 0)
                            {
                                var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                            rnal.set_text("请务选择已经合并的数据");
                                            rnal.show();
                                            args.set_cancel(true);
                                            return;
                             }
                            //  var dataKeyValue = item.getDataKeyValue("OrderID");
                            //    var itemCode1 = item["ItemCode1"].text;
                            var itemCode1 = treeList.getCellByColumnUniqueName(selectedItems[i], "ItemCode1").innerText;
                            if (itemCode1 != temp)
                            {
                                var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                            rnal.set_text("请选择物资编码相同的数据");
                                            rnal.show();
                                            args.set_cancel(true)
                                            return;
                                        }
                                    }
                                    ShowMDemandCombineList(temp);
                            }
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
                          <ClientEvents OnKeyPress="AlphabetOnly" />
                        </telerik:RadTextBox></td>
                    <td>零件类型：</td>
                    <td>
                        <telerik:RadDropDownList ID="RDDL_LingJian_Type" runat="server" Width="100px" AppendDataBoundItems="true">
                           <Items><telerik:DropDownListItem Text="全部" Value="" /></Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>图号：</td>
                    <td><telerik:RadTextBox ID="RTB_Drawing_No" runat="server" Width="100px">
                          <ClientEvents OnKeyPress="AlphabetOnly" />
                        </telerik:RadTextBox></td>
                      <td>工艺路线：</td>
                    <td><telerik:RadTextBox ID="Rad_TechLine" runat="server"  Width="100px">
                         <ClientEvents OnKeyPress="AlphabetOnly" />
                        </telerik:RadTextBox></td>
                    <td><telerik:RadButton ID="RB_Query" runat="server" Text="查询" OnClick="WZBH_Query_Click"></telerik:RadButton></td>
                </tr>
            </table>
       <div class="divSiteMap add_divSiteMap" style="clear: both; width: 100%;">
           <div class="div_addclass" style="float: right; height: 40px; line-height: 40px;">
                    <span style="font-size: 13px;color:red;"> <asp:CheckBox ID="chb_all" runat="server" OnCheckedChanged="chb_all_CheckedChanged" AutoPostBack="True" Text="全选"/></span>
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
        
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  ClientEvents-OnRequestStart="onRequestStart">
         
                      <telerik:RadTreeList RenderMode="Lightweight" Width="100%" ID="RadTreeList1" AutoGenerateColumns="false" AllowSorting="true" 
                         AllowLoadOnDemand="true"  OnItemDataBound="RadTreeList_MDemandDetails_ItemDataBound" 
                          OnItemSelected="RadTreeList1_SelectedIndexChanged"  OnChildItemsDataBind="RadTreeList1_ChildItemsDataBind" OnNeedDataSource="RadTreeList1_NeedDataSource"
                          DataKeyNames="ID" ParentDataKeyNames="ParentId_For_Combine" AllowMultiItemSelection="true" runat="server">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle Font-Size="12px" HorizontalAlign="Center" />
                <HeaderStyle Font-Size="13px" HorizontalAlign="Center"/>
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                          <ClientSettings>
                              <Selecting AllowItemSelection="true" AllowToggleSelection="true" />
                              <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" ScrollHeight="480px"></Scrolling>
                          </ClientSettings> 
                         <ExportSettings IgnorePaging="false"  OpenInNewWindow="true">
                           <Pdf  DefaultFontFamily="Arial Unicode MS" />

                         </ExportSettings>

                             <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="false" />
                          <Columns>
                             <telerik:TreeListTemplateColumn UniqueName="CheckBoxTemplateColumn" ItemStyle-Width="50px" HeaderStyle-Width="50px" >
                              <ItemTemplate >
                                <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True" CssClass='<%#Eval("ID") %>' />
                              </ItemTemplate>
                              <HeaderTemplate>
                                <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True" />
                              </HeaderTemplate>
                            </telerik:TreeListTemplateColumn>
                            <telerik:TreeListBoundColumn DataField="ID" DataType="System.Int32" HeaderText="序号" ItemStyle-Width="50px" HeaderStyle-Width="50px" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                            </telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="mstate"  HeaderText="提交<br />状态" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="mstate" UniqueName="mstate">
                            </telerik:TreeListBoundColumn>
                     
                            <telerik:TreeListBoundColumn DataField="LingJian_Type1" HeaderText="零件<br />类型" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="LingJian_Type1" UniqueName="LingJian_Type1"></telerik:TreeListBoundColumn>
            
                            <telerik:TreeListBoundColumn DataField="TDM_Description" HeaderText="产品<br />名称" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="TDM_Description" UniqueName="TDM_Description">
                            </telerik:TreeListBoundColumn>
                           
                            <telerik:TreeListBoundColumn DataField="Drawing_No"  HeaderText="图号" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Drawing_No" UniqueName="Drawing_No">
                            </telerik:TreeListBoundColumn>
                            
                            <telerik:TreeListBoundColumn DataField="TaskCode"  HeaderText="任务编号" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="TaskCode" UniqueName="TaskCode">
                            </telerik:TreeListBoundColumn>
                      
                            <telerik:TreeListBoundColumn DataField="Technics_Line"  HeaderText="工艺路线" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Technics_Line" UniqueName="Technics_Line">
                            </telerik:TreeListBoundColumn>

                            <telerik:TreeListBoundColumn DataField="Material_Name" HeaderText="材料名称" ItemStyle-Width="150px" HeaderStyle-Width="150px" SortExpression="Material_Name" UniqueName="Material_Name">
                            </telerik:TreeListBoundColumn>

                            <telerik:TreeListBoundColumn DataField="Material_Mark" HeaderText="材料<br />牌号" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="Material_Name" UniqueName="Material_Name"></telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="CN_Material_State" HeaderText="材料<br />状态" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="CN_Material_State" UniqueName="CN_Material_State"></telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="Material_Tech_Condition" HeaderText="技术条件" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Material_Tech_Condition" UniqueName="Material_Tech_Condition"> </telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="Rough_Spec" HeaderText="胚料<br />规格" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Rough_Spec" UniqueName="Rough_Spec"></telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="Rough_Size" HeaderText="胚料<br />尺寸" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="Rough_Size" UniqueName="Rough_Size"></telerik:TreeListBoundColumn>
                                                  
                            <telerik:TreeListBoundColumn DataField="Mat_Unit" HeaderText="计量<br />单位" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Mat_Unit" UniqueName="Mat_Unit">
                            </telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件<br />质量" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                            </telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="Mat_Pro_Weight"  HeaderText="产品<br />质量" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                            </telerik:TreeListBoundColumn>

                            <telerik:TreeListBoundColumn DataField="ItemCode1" HeaderText="物资<br />编码" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="ItemCode1" UniqueName="ItemCode1">
                            </telerik:TreeListBoundColumn>

                            <telerik:TreeListBoundColumn DataField="NumCasesSum"  HeaderText="需求<br />件数" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                            </telerik:TreeListBoundColumn>

                            <telerik:TreeListBoundColumn DataField="DemandNumSum"  HeaderText="需求<br />质量" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                            </telerik:TreeListBoundColumn>

                            <telerik:TreeListBoundColumn DataField="Tech_Quantity"  HeaderText="工艺<br />数量" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Tech_Quantity" UniqueName="Tech_Quantity">
                            </telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="Technics_Comment" HeaderText="路线<br />备注" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Technics_Comment" UniqueName="Technics_Comment">
                            </telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="Memo_Quantity"  HeaderText="备件<br />数量" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Memo_Quantity" UniqueName="Memo_Quantity">
                            </telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="Mat_Comment" HeaderText="定额<br />备注" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Mat_Comment" UniqueName="Mat_Comment">
                            </telerik:TreeListBoundColumn>
                            <telerik:TreeListBoundColumn DataField="DemandDate" HeaderText="需求日期" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="DemandDate" UniqueName="DemandDate" Visible="true">
                            </telerik:TreeListBoundColumn>

                         
                          
                            <telerik:TreeListBoundColumn DataField="Combine_State"  HeaderText="合并<br />状态" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Combine_State"  UniqueName="Combine_State" Visible="true"> 
                            </telerik:TreeListBoundColumn>

                            <telerik:TreeListBoundColumn DataField="ParentId_For_Combine" HeaderText="" ItemStyle-Width="20px" HeaderStyle-Width="20px" SortExpression="ParentId_For_Combine" UniqueName="ParentId_For_Combine" Visible="true">
                            </telerik:TreeListBoundColumn>
                        </Columns>
                    </telerik:RadTreeList>
         <div style="width: 100%; float: left;">

                                <telerik:RadButton ID="RadBtnCombineMergeList" runat="server" Text="合并相同编码清单" CssClass="floatleft" Font-Bold="true" 
                                    CommandName="CombineMergeList" CausesValidation="true"  OnClientClicking="confirmWindowSubmitCombineClicking" >
                                </telerik:RadButton>

                                <telerik:RadButton ID="RB_Combine_Cancel" runat="server" Text="取消合并编码清单" CssClass="floatleft" Font-Bold="true" 
                                    CommandName="CancelCombine" OnClick="RB_Combine_Cancel_Click" OnClientClicking="confirmWindowSubmitCancel">
                                </telerik:RadButton>

					     <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>

                    </div>    
                             
    
                   </telerik:RadAjaxPanel>
                </div>
        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                    AutoCloseDelay="4000" Width="240" Height="90" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
    <%--弹出窗口--开始--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
      <Windows>
            <telerik:RadWindow ID="RadWindowRecordWindow" runat="server" Title="物资需求清单-待提交" Left="50px" Top="50px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close,Maximize,Minimize" OnClientClose="RefreshParent" Modal="true" Width="1400px" Height="660px" />
            <telerik:RadWindow ID="RadWindow" runat="server" VisibleTitlebar="false" VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                 <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <div style="width: 60px; padding-left: 15px; float: left;">
                        <img src="../Images/images/warnning1.jpg" alt="" />
                    </div>
                    <div style="width: 200px; float: left;">
                        <asp:Label ID="Label2" Font-Size="14px" Text="确定要取消合并吗？" runat="server" Font-Bold="true"
                            ForeColor="#25a0da" />
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="RadButton3" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                        &nbsp;
                        <telerik:RadButton ID="RadButton4" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </telerik:RadButton>
                    </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
      </Windows>
    </telerik:RadWindowManager>
    <%--结束--%>
</asp:Content>
