<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MDemandDetails.aspx.cs" ValidateRequest="false" Inherits="mms.Plan.MDemandDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        
    </script>
    <style>
        .btn_margin1 {
            margin-left: 10px;
        }

        .btn_margin2 {
            margin-left: 50px;
        }

        .zt_3 {
            color: green;
            font-weight: bold;
        }

        .zt_1 {
            color: red;
            font-weight: bold;
        }

        .zt_2 {
            color: blue;
            font-weight: bold;
        }

        .span_1 {
            margin-left: 15px;
            font-size: 13px;
        }

        .span_0 {
            color: black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfFlag" runat="server" />
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->物资需求清单" ClientIDMode="Static" />
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" ShowBaseLine="True" SelectedIndex="1">
        <Tabs>
            <telerik:RadTab Text="需要提交" TabIndex="0"></telerik:RadTab>
            <telerik:RadTab Text="需求变更" TabIndex="1" Selected="True"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <hr style="margin-top: 0px;" />

          <div style="width: 100%; margin: 0px auto;">
            <table style="text-align:left;">
                <tr>
                    <td style="text-align:right;">物资编码：</td>
                    <td><telerik:RadTextBox ID="RTB_ItemCode" runat="server" Width="120px"></telerik:RadTextBox></td>
                    <td><telerik:RadButton ID="RB_Query" runat="server" Text="查询" OnClick="WZBH_Query_Click"></telerik:RadButton></td>

                </tr>
              
            </table>
        </div>
    <div style="width: 100%; float: left;">
        <div class="divContant">
            <div class="divSiteMap add_divSiteMap" style="clear: both; width: 100%;">
                <div class="divSiteMap" style="width: 100%; float: none; height: 30px; border-bottom-style: solid; border-bottom-width: 1px;">
                    <label style="float: left; margin-top: 0px;">型号：</label>
                    <span id="span_model" style="font-weight:bold; width:200px;" runat="server"></span>
                    <label style="float: left; margin-top: 0px;">计划包名称：</label>
                    <span id="span_PlanName" style="font-weight:bold; width:200px;" runat="server"></span>
                    <label style="float: left; margin-top: 0px;">对应型号投产计划编号：</label>
                    <span id="span_plancode" style="font-weight:bold;width:200px;" runat="server"></span>
                    <label style="float: left">材料清单编号：</label>
                    <span id="span_DraftCode" style="font-weight:bold;width:200px;" runat="server"></span>
                </div>
                <div style="float: left; height: 40px; line-height: 40px;">
                    <span style="font-weight: bold;font-size:20px;">物资需求清单</span>
                </div>

                <div style="float: left; height: 40px; line-height: 40px;">
                    <asp:CheckBox ID="chb_all" runat="server" OnCheckedChanged="chb_all_CheckedChanged" AutoPostBack="True" Text="全选"/>
                </div>
                <div class="div_addclass" style="float: left; height: 40px; line-height: 40px;">
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
                <div style="width: 100%; height: 0px; border: solid #000 1px; margin: 5px 0; clear: both"></div>
                <div style="clear: both; overflow: hidden"></div>
            </div>
            <div class="divSiteMap" style="width: 100%; display:none; float: none; height: 30px; border-bottom-style: solid; border-bottom-width: 1px;">
                <div style="float: left; height: 30px;">
                    <label>材料清单编号：</label>
                    <telerik:RadTextBox ID="RadTxt_DraftCode" runat="server"></telerik:RadTextBox>
                    <label>提交状态：</label>
                    <asp:DropDownList ID="ddlMState" runat="server">
                        <asp:ListItem Value="-1">全部</asp:ListItem>
                        <asp:ListItem Value="0">未提交</asp:ListItem>
                        <asp:ListItem Value="1">已提交</asp:ListItem>
                        <asp:ListItem Value="2">已提交有更改可再次提交</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="float: left; height: 30px;">
                    <telerik:RadButton ID="RadBtn_Search" runat="server" Text="查询" OnClick="RadBtn_Search_Click"></telerik:RadButton>
                    <telerik:RadButton ID="RadBtnReturn" runat="server" Text="返回投产计划包列表" OnClick="RadBtnReturn_Click"></telerik:RadButton>
                </div>
            </div>
            <div style="width: 100%; float: left;">
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

                    <script type="text/javascript">
           
          
                var submintId;
              
                function YesOrNoClicked(sender, args) {
                    var oWnd = $find("<%=RadWindow.ClientID %>");
                    oWnd.close();
                    if (sender.get_text() == "是") {
                        $find(submintId).click();
                    }
                }
                        function ShowMDemandMergeList() {
                          
                            var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                            win.set_title("物资需求计划清单");
                            window.radopen("/Plan/MDemandMergeList.aspx", "RadWindowRecordWindow");
                            return false;
                        }
                        function confirmWindowSubmitMerge(sender, args) {
                            var grid = $find("<%=RadGrid_MDemandDetails.ClientID%>"); 
                        //    var flag = document.getElementById("ContentPlaceHolder1_hfFlag").value;
                        //    var checkAll = document.getElementById("ContentPlaceHolder1_chb_all");
                       //     if (flag == "0" && !checkAll.checked)
                            var masterTableView = grid.get_masterTableView();

                            var selectedItems = masterTableView.get_selectedItems();
                            if (selectedItems.length <= 0)
                            {


                                var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                rnal.set_text("请选择要提交的数据");
                                rnal.show();
                                args.set_cancel(true);
                            } else {
                                ShowMDemandMergeList();
                            }
                        }

                        function ShowMDemandCombineList(itemCode) {

                            var win = $find("<%=RadWindowRecordWindow.ClientID %>");
                            win.set_title("物资需求计划合并清单");
                           
                            window.radopen("/Plan/MDemandDetailsCombine.aspx?PackId=" + "<%= Request.QueryString["PackId"].ToString() %>"+"&itemCode1="+itemCode, "RadWindowRecordWindow");
                             return false;
                         }

                        function confirmWindowSubmitCancel(sender, args) {
                            var grid = $find('<%= RadGrid_MDemandDetails.ClientID %>');
                            var masterTableView = grid.get_masterTableView();

                            var selectedItems = masterTableView.get_selectedItems();
                            if (selectedItems.length <= 0)
                            {
                                var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                       rnal.set_text("请选择一条以红颜色标记的合并过的数据记录");
                                       rnal.show();
                                       args.set_cancel(true);
                                     
                            }
                            else  if (selectedItems.length>1)
                            {
                                var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                  rnal.set_text("请勿选择两条（含）以上的数据记录");
                                  rnal.show();
                                  args.set_cancel(true);
                                  return;
                            }
                            else
                            {
                                var combine_State = $(selectedItems[0].get_cell("Combine_State")).text();
                              //  var parentId_For_Combine = selectedItems[0].getDataKeyValue("ParentId_For_Combine");

                                if (combine_State != 1) {
                                    var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                    rnal.set_text("请选择一条以红颜色标记的合并过的数据记录");
                                    rnal.show();
                                    args.set_cancel(true)

                                }
                                else {
                                    //ShowMDemandCombineList(temp);
                                    $find("<%= RadWindow.ClientID %>").show();
                                    submintId = sender.get_id();
                                    args.set_cancel(true);
                                }
                            }

                                 


                        }

                        function confirmWindowSubmitCombineClicking(sender, args)
                        {
                                  var grid = $find('<%= RadGrid_MDemandDetails.ClientID %>');
                                   var masterTableView = grid.get_masterTableView();

                                   var selectedItems = masterTableView.get_selectedItems();
                                   if (selectedItems.length <= 0) {
                                       var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                       rnal.set_text("请选择要合并的数据");
                                       rnal.show();
                                       args.set_cancel(true);
                                   }
                                   else
                                   {
                                       var temp = $(selectedItems[0].get_cell("ItemCode1")).text();
                                       for (var i = 1; i < selectedItems.length; i++) {
                                           var item = selectedItems[i];

                                           //  var dataKeyValue = item.getDataKeyValue("OrderID");
                                           var itemCode1 = $(item.get_cell("ItemCode1")).text();

                                           if (itemCode1 != temp) {
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

              
                        function refreshGrid1() {

                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind1");

                             }

                        function refreshGrid() {
                       
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                           
                        }
                    </script>
                </telerik:RadCodeBlock>
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandDetails">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandDetails"/>
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                <telerik:AjaxUpdatedControl ControlID="hfFlag" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadBtn_Search">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandDetails" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                <telerik:AjaxUpdatedControl ControlID="span_DraftCode" />
                                <telerik:AjaxUpdatedControl ControlID="span_model" />
                                <telerik:AjaxUpdatedControl ControlID="span_plancode" />
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
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server">
				</telerik:RadAjaxLoadingPanel>
                <telerik:RadGrid ID="RadGrid_MDemandDetails" runat="server" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                    AllowPaging="True" AllowSorting="True" PageSize="20" PagerStyle-AlwaysVisible="True" AllowMultiRowSelection="True" AutoGenerateColumns="False"
                    OnNeedDataSource="RadGrid_MDemandDetails_NeedDataSource"  OnItemDataBound="RadGrid_MDemandDetails_ItemDataBound"
                     OnDetailTableDataBind="RadGrid1_DetailTableDataBind"     OnPreRender="RadGrid1_PreRender" >
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <ClientSettings EnableRowHoverStyle="true" >
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="6" ScrollHeight="600px"></Scrolling>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID,ItemCode1,ParentId_For_Combine,Combine_State" AllowMultiColumnSorting="True" CommandItemDisplay="Bottom">
                       <DetailTables>
                        <telerik:GridTableView DataKeyNames="ID,ItemCode1,ParentId_For_Combine,Combine_State" Name="Child1" Width="100%">
                          <DetailTables>
                            <telerik:GridTableView DataKeyNames="ID,ItemCode1,ParentId_For_Combine,Combine_State" Name="Child2" Width="100%">
                            <Columns>
                            <telerik:GridBoundColumn DataField="ParentId_For_Combine"   HeaderText="父记录序号" SortExpression="ParentId_For_Combine" UniqueName="parentId_For_Combine" Visible="true"> </telerik:GridBoundColumn>
                            <%--<telerik:GridClientSelectColumn HeaderText="" UniqueName="is_checked"></telerik:GridClientSelectColumn>--%>
                            <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="序号" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="mstate" FilterControlAltText="Filter mstate column" HeaderText="物资状态" SortExpression="mstate" UniqueName="mstate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Material_State" FilterControlAltText="Filter Material_State column" Visible="false" SortExpression="Material_State" UniqueName="Material_State">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MDPId" FilterControlAltText="Filter MDPId column" Visible="false" SortExpression="MDPId" UniqueName="MDPId">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="MCODE" FilterControlAltText="Filter MCODE column" HeaderText="编号" SortExpression="MCODE" UniqueName="MCODE">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="产品名称" SortExpression="Material_Name" UniqueName="Material_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Drawing_No" FilterControlAltText="Filter Drawing_No column" HeaderText="产品图号" SortExpression="Drawing_No" UniqueName="Drawing_No">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Technics_Line" FilterControlAltText="Filter Technics_Line column" HeaderText="工艺路线" SortExpression="Technics_Line" UniqueName="Technics_Line">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Unit" FilterControlAltText="Filter Mat_Unit column" HeaderText="计量单位" SortExpression="Mat_Unit" UniqueName="Mat_Unit">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Rough_Weight" itemstyle-HorizontalAlign="Right" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Pro_Weight" itemstyle-HorizontalAlign="Right" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Rough_Size" FilterControlAltText="Filter Rough_Size column" HeaderText="物资尺寸" SortExpression="Rough_Size" UniqueName="Rough_Size">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DemandNumSum" itemstyle-HorizontalAlign="Right" HeaderText="共计需求数量" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NumCasesSum" itemstyle-HorizontalAlign="Right" HeaderText="共计需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                            </telerik:GridBoundColumn>
                           
                            <%--<telerik:GridBoundColumn DataField="quantity1" itemstyle-HorizontalAlign="Right" HeaderText="已提交" SortExpression="quantity1" UniqueName="quantity1">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridTemplateColumn HeaderText="需求时间" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" UniqueName="DemandDate" Visible="False">
                                <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                <ItemTemplate>
                                    <telerik:RadDatePicker ID="NewDemandDate" runat="server" Width="100px" CssClass='<%#Eval("ID") %>'></telerik:RadDatePicker>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Combine_State" FilterControlAltText="Filter Combine_State column" HeaderText="合并后的记录?" SortExpression="Combine_State" UniqueName="Combine_State"> </telerik:GridBoundColumn>

                        </Columns>
                        </telerik:GridTableView>
                       </DetailTables>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ParentId_For_Combine" itemstyle-HorizontalAlign="Right"  HeaderText="父记录序号" SortExpression="ParentId_For_Combine" UniqueName="parentId_For_Combine" Visible="true"> </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="序号" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="mstate" FilterControlAltText="Filter mstate column" HeaderText="物资状态" SortExpression="mstate" UniqueName="mstate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Material_State" FilterControlAltText="Filter Material_State column" Visible="false" SortExpression="Material_State" UniqueName="Material_State">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MDPId" FilterControlAltText="Filter MDPId column" Visible="false" SortExpression="MDPId" UniqueName="MDPId">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="MCODE" FilterControlAltText="Filter MCODE column" HeaderText="编号" SortExpression="MCODE" UniqueName="MCODE">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="产品名称" SortExpression="Material_Name" UniqueName="Material_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Drawing_No" FilterControlAltText="Filter Drawing_No column" HeaderText="产品图号" SortExpression="Drawing_No" UniqueName="Drawing_No">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Technics_Line" FilterControlAltText="Filter Technics_Line column" HeaderText="工艺路线" SortExpression="Technics_Line" UniqueName="Technics_Line">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Unit" FilterControlAltText="Filter Mat_Unit column" HeaderText="计量单位" SortExpression="Mat_Unit" UniqueName="Mat_Unit">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Rough_Weight" itemstyle-HorizontalAlign="Right" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Pro_Weight" itemstyle-HorizontalAlign="Right" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Rough_Size" FilterControlAltText="Filter Rough_Size column" HeaderText="物资尺寸" SortExpression="Rough_Size" UniqueName="Rough_Size">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DemandNumSum" itemstyle-HorizontalAlign="Right" HeaderText="共计需求数量" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NumCasesSum" itemstyle-HorizontalAlign="Right" HeaderText="共计需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                            </telerik:GridBoundColumn>
                           
                            <%--<telerik:GridBoundColumn DataField="quantity1" itemstyle-HorizontalAlign="Right" HeaderText="已提交" SortExpression="quantity1" UniqueName="quantity1">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridTemplateColumn HeaderText="需求时间" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" UniqueName="DemandDate" Visible="False">
                                <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                <ItemTemplate>
                                    <telerik:RadDatePicker ID="NewDemandDate" runat="server" Width="100px" CssClass='<%#Eval("ID") %>'></telerik:RadDatePicker>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                           <telerik:GridBoundColumn DataField="Combine_State" FilterControlAltText="Filter Combine_State column" HeaderText="合并后的记录?" SortExpression="Combine_State" UniqueName="Combine_State"> </telerik:GridBoundColumn>

                        </Columns>
                        </telerik:GridTableView>
                       </DetailTables>
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                              <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="ToggleRowSelection" AutoPostBack="True" CssClass='<%#Eval("ID") %>' />
                              </ItemTemplate>
                              <HeaderTemplate>
                                <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True" />
                              </HeaderTemplate>
                            </telerik:GridTemplateColumn>

                            <%--<telerik:GridClientSelectColumn HeaderText="" UniqueName="is_checked"></telerik:GridClientSelectColumn>--%>
                            <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="序号" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="mstate" FilterControlAltText="Filter mstate column" HeaderText="物资状态" SortExpression="mstate" UniqueName="mstate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Material_State" FilterControlAltText="Filter Material_State column" Visible="false" SortExpression="Material_State" UniqueName="Material_State">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MDPId" FilterControlAltText="Filter MDPId column" Visible="false" SortExpression="MDPId" UniqueName="MDPId">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="MCODE" FilterControlAltText="Filter MCODE column" HeaderText="编号" SortExpression="MCODE" UniqueName="MCODE">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="产品名称" SortExpression="Material_Name" UniqueName="Material_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Drawing_No" FilterControlAltText="Filter Drawing_No column" HeaderText="产品图号" SortExpression="Drawing_No" UniqueName="Drawing_No">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Technics_Line" FilterControlAltText="Filter Technics_Line column" HeaderText="工艺路线" SortExpression="Technics_Line" UniqueName="Technics_Line">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Unit" FilterControlAltText="Filter Mat_Unit column" HeaderText="计量单位" SortExpression="Mat_Unit" UniqueName="Mat_Unit">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Rough_Weight" itemstyle-HorizontalAlign="Right" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Pro_Weight" itemstyle-HorizontalAlign="Right" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Rough_Size" FilterControlAltText="Filter Rough_Size column" HeaderText="物资尺寸" SortExpression="Rough_Size" UniqueName="Rough_Size">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DemandNumSum" itemstyle-HorizontalAlign="Right" HeaderText="共计需求数量" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NumCasesSum" itemstyle-HorizontalAlign="Right" HeaderText="共计需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="ParentId_For_Combine" itemstyle-HorizontalAlign="Right"  SortExpression="ParentId_For_Combine" UniqueName="ParentId_For_Combine" Visible="true">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="quantity1" itemstyle-HorizontalAlign="Right" HeaderText="已提交" SortExpression="quantity1" UniqueName="quantity1">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridTemplateColumn HeaderText="需求时间" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" UniqueName="DemandDate" Visible="False">
                                <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                <ItemTemplate>
                                    <telerik:RadDatePicker ID="NewDemandDate" runat="server" Width="100px" CssClass='<%#Eval("ID") %>'></telerik:RadDatePicker>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Combine_State" FilterControlAltText="Filter Combine_State column" HeaderText="" SortExpression="Combine_State" UniqueName="Combine_State"> </telerik:GridBoundColumn>

                        </Columns>

                        <CommandItemTemplate>
                              <div style="margin:5px 0 5px 4px;">
                                <telerik:RadButton ID="RadBtnCombineMergeList" runat="server" Text="合并相同编码清单" CssClass="btn_margin1" Font-Bold="true" 
                                    CommandName="CombineMergeList" CausesValidation="true" OnClick="confirmWindowSubmitCombineServer" OnClientClicking="confirmWindowSubmitCombineClicking" ></telerik:RadButton>



                                  <telerik:RadButton ID="RB_Combine_Cancel" runat="server" Text="取消合并编码清单" CssClass="btn_margin1" Font-Bold="true" 
                                    CommandName="CancelCombine" OnClick="RB_Combine_Cancel_Click" OnClientClicking="confirmWindowSubmitCancel"></telerik:RadButton>

                                <telerik:RadButton ID="RadBtnBuildMergeList" runat="server" Text="生成物资需求清单" CssClass="btn_margin1" Font-Bold="true" 
                                    CommandName="BuildMergeList" OnClientClicking="confirmWindowSubmitMerge"></telerik:RadButton>

                            </div>
                        </CommandItemTemplate>
                    </MasterTableView>
                </telerik:RadGrid>
               <telerik:RadWindow ID="RadWindow" runat="server" VisibleTitlebar="false"
                   VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
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
                <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                    AutoCloseDelay="4000" Width="240" Height="90" Title="提示" EnableRoundedCorners="true">
                </telerik:RadNotification>
                <%--弹出窗口--开始--%>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowRecordWindow" runat="server" Title="物资需求清单-待提交" Left="150px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="None" Modal="true" Width="1400px" Height="660px" />
                    </Windows>
                </telerik:RadWindowManager>
                <%--结束--%>

            </div>
        </div>
    </div>
</asp:Content>
