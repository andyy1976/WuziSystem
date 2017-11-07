<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/index.Master" CodeBehind="MaterialShowPlan.aspx.cs" Inherits="mms.MaterialApplicationCollar.MaterialShowPlan" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
   
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
    <style type="text/css">
        .cursorpointer {
            cursor: pointer;
            color: blue;
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->型号投产计划" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridP_Pack" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Query">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridP_Pack" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridP_Pack">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridP_Pack" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_SynchronStatus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridP_Pack" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
              <telerik:AjaxSetting AjaxControlID="RB_Import">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Default" GroupingText="同步中，结束前请不要有任何操作"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">
            function RefreshParent(sender, eventArgs) {
                document.location.reload();
            }
            //删除
            var deleteID;
            function confirmRadWindowDelete(sender, args) {
                $find("<%= RadWindowDelete.ClientID %>").show();
                deleteID = sender.get_id();
                args.set_cancel(true);
            }
            function YesOrNoClickedDelete(sender, args) {
                var oWnd = $find("<%=RadWindowDelete.ClientID %>");
                oWnd.close();
                if (sender.get_text() == "是") {
                    $find(deleteID).click();
                }
            }

            //新增计划包
            function ImportPlan(sender, args) {
                var win = $find("<%=RadWindowImportWindow.ClientID %>");
                win.set_title("新增计划包");
                window.radopen("/Plan/PlanImport.aspx?Type=1", "RadWindowImportWindow");
            }

            //导入型号物资需求
            function ImportMaterial(sender, args) {

                var grid = $find('<%= RadGridP_Pack.ClientID %>');
                var masterTableView = grid.get_masterTableView();

                var selectedItems = masterTableView.get_selectedItems();
                if (selectedItems.length <= 0) {
                    var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                    rnal.set_text("请选择一条型号投产计划");
                    rnal.show();


                }
                else if (selectedItems.length >1) {
                    var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                    rnal.set_text("请勿选择多条型号投产计划");
                    rnal.show();

                }
                else {
                    var PackId = selectedItems[0].getDataKeyValue("PackID");
                    var win = $find("<%=RadWindowImportMaterialWindow.ClientID %>");
                    win.set_title("导入型号物资需求");
                   
                    window.radopen("/MaterialApplicationCollar/MaterialPlanImport.aspx?PackId=" + PackId, "RadWindowImportMaterialWindow");
                }
            }


            function ImportMaterialNoSelection(PackId) 

             {
                  
                    var win = $find("<%=RadWindowImportMaterialWindow.ClientID %>");
                    win.set_title("导入型号物资需求");

                    window.radopen("/MaterialApplicationCollar/MaterialPlanImport.aspx?PackId=" + PackId, "RadWindowImportMaterialWindow");
                }
   

            //查看计划包
            function ShowP_Pack_Task(PackId) {
                var win = $find("<%=RadWindowImportWindow.ClientID %>");
                win.set_title("查看计划包");
                window.radopen("/Plan/ShowP_Pack_Task.aspx?PackId=" + PackId, "RadWindowImportWindow");
            }
            //查看BOM
            function ShowSmarTeam(PackId) {
                window.location.href = "/Plan/ShowSmarTeam.aspx?PackId=" + PackId;
            }
            function ChangeMaterialQuota(PackId) {
                window.location.href = "/Plan/ChangeMaterialQuota.aspx?PackId=" + PackId;
            }
            function ShowM_Change(PackId) {
                window.location.href = "/Plan/ShowM_Change.aspx?PackId=" + PackId;
            }
            function ShowMDemandDetails(PackId) {
               // window.location.href = "/Plan/MDemandDetails.aspx?PackId=" + PackId;
                window.location.href = "/Plan/MDemandDetailsTreeList.aspx?PackId=" + PackId;
            }
            //需求变更
            function ShowMDemandMergeListUpdate(PackId) {
                window.location.href = "/Plan/MDemandMergeListChange.aspx?PackId=" + PackId;
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
        <div style="width: 100%; float: left;">
            <table>
                <tr>
                    <td>型号：</td>
                    <td><telerik:RadTextBox ID="RTB_Model" runat="server" Width="100px"></telerik:RadTextBox></td>
                    <td>计划包名称：</td>
                    <td><telerik:RadTextBox ID="RTB_PackageName" runat="server" Width="150px"></telerik:RadTextBox></td>
                    <%--
                    <td>任务号：</td>
                    <td><telerik:RadTextBox ID="RTB_TaskCode" runat="server" Width="100px"></telerik:RadTextBox></td>
                    <td>图号：</td>
                    <td><telerik:RadTextBox ID="RTB_DrawingNo" runat="server" Width="100px"></telerik:RadTextBox></td>
                        --%>
                    <td>材料定额状态：</td>
                    <td>
                        <telerik:RadDropDownList ID="RDDL_DS" runat="server" Width="100">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="全部" />
                                <telerik:DropDownListItem Value="1" Text="未生成" />
                                <telerik:DropDownListItem Value="2" Text="部分生成" />
                                <telerik:DropDownListItem Value="3" Text="完成" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>清单：</td>
                    <td>
                        <telerik:RadDropDownList ID="RDDL_ID" runat="server" Width="100px">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="全部" />
                                <telerik:DropDownListItem Value="1" Text="未生成" />
                                <telerik:DropDownListItem Value="2" Text="部分生成" />
                                <telerik:DropDownListItem Value="3" Text="完成" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>编制人：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_UserName" runat="server" Width="100px"></telerik:RadTextBox></td>
                    <td>编制时间：</td>
                    <td>
                        <telerik:RadDatePicker ID="RDP_Start" runat="server" Width="100px"></telerik:RadDatePicker>
                        ～
                        <telerik:RadDatePicker ID="RDP_End" runat="server" Width="100px"></telerik:RadDatePicker>
                    </td>
                    <td>
                        <telerik:RadButton ID="RB_Query" runat="server" Text="筛选" OnClick="RB_Query_Click"></telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; float: left;">
            <telerik:RadGrid ID="RadGridP_Pack" runat="server" AutoGenerateColumns="false" 
                AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True"
                OnItemCommand="RadGridP_Pack_ItemCommand" OnNeedDataSource="RadGridP_Pack_NeedDataSource" OnItemDataBound="RadGridP_Pack_ItemDataBound">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings EnableRowHoverStyle="true" >
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="600px"></Scrolling>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="PackID" ClientDataKeyNames="PackID">
                    <CommandItemTemplate>
                        型号投产计划包列表
                        <telerik:RadButton ID="RB_Add" runat="server" Text="新增计划包" CssClass="floatleft" AutoPostBack="false" OnClientClicked="ImportPlan" Visible="false"></telerik:RadButton>
                        <telerik:RadButton ID="RB_Import" runat="server" Text="导入物资需求" CssClass="floatleft" AutoPostBack="false" OnClientClicked="ImportMaterial" Visible="true"></telerik:RadButton>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="RowsID" HeaderText="序号" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Model" HeaderText="型号" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="计划包名称" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                            <ItemTemplate> 
                                <telerik:RadButton ID="RB_PlanName" runat="server" AutoPostBack="false" ForeColor="Blue" ButtonType="ToggleButton" ToggleType="None" ToolTip="点击查看计划包数据"></telerik:RadButton>
                                <telerik:RadButton ID="RB_State" runat="server" AutoPostBack="false" ToolTip="点击查看计划包数据" Width="20px" Height="20px"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="BOM管理" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadButton ID="RB_ADD_1" runat="server" ButtonType="ToggleButton" Text="导入物资需求" ToolTip="点击通过Excel导入型号物资需求"
                                    OnClientClicking="ImportMaterial" CommandName="ImportFromExcel" Visible="false" ForeColor="Red">
                                </telerik:RadButton>
                                <telerik:RadButton ID="RB_Synchronization" runat="server" AutoPostBack="false" ButtonType="ToggleButton" Text="查看BOM" ToolTip="点击查看BOM与定额" Visible="false" ForeColor="Blue"></telerik:RadButton>
                                <telerik:RadButton ID="RB_Synchronization1" runat="server" AutoPostBack="false" ButtonType="ToggleButton" ToolTip="点击查看BOM与定额" Visible="false" ForeColor="Blue"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="材料定额变更" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadButton ID="RB_Change" runat="server" AutoPostBack="false" ButtonType="ToggleButton" Text="进入变更" ToolTip="点击进入变更" ForeColor="Blue"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="变更查询" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadButton ID="RB_ChangeList" runat="server" AutoPostBack="false" ButtonType="ToggleButton" ToolTip="点击查询变更单" ForeColor="Blue"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="物资需求清单管理" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadButton ID="RB_Draft" runat="server" AutoPostBack="false" ButtonType="ToggleButton" ToolTip="" ForeColor="Blue"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="需求变更" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <telerik:RadButton ID="RB_MergeListUpdate" runat="server" Text="更改需求" AutoPostBack="false" ForeColor="Blue" ButtonType="ToggleButton" ToolTip="点击进入需求变更"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="UserName" HeaderText="编制人" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ImportTime" HeaderText="编制时间" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="删除" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                            <ItemTemplate>
                                <telerik:RadButton ID="RB_Delete" runat="server" ButtonType="ToggleButton" Text="删除" OnClientClicking="confirmRadWindowDelete" CommandName="Delete" Visible="false"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <%--<PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>--%>
            </telerik:RadGrid>
        </div>
    </div>

    <%-- 删除弹出窗口--开始--%>
    <telerik:RadWindow ID="RadWindowDelete" runat="server" VisibleTitlebar="false"
        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
        <ContentTemplate>
            <div style="margin-top: 30px; float: left;">
                <div style="width: 60px; padding-left: 15px; float: left;">
                    <img src="../Images/images/warnning1.jpg" alt="" />
                </div>
                <div style="width: 200px; float: left;">
                    <asp:Label ID="Label2" Font-Size="14px" Text="确定要删除吗？" runat="server" Font-Bold="true"
                        ForeColor="#25a0da" />
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="RadButton3" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClickedDelete">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                    &nbsp;
                        <telerik:RadButton ID="RadButton4" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClickedDelete">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </telerik:RadButton>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <%-- 删除弹出窗口--结束--%>
    <%--导入计划、查看计划--开始--%>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowImportWindow" runat="server" Title="新增计划包" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="1100px" Height="520px" />
            <telerik:RadWindow ID="RadWindowImportMaterialWindow" runat="server" Title="导入型号物资需求" Left="100px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close,Maximize,Minimize" OnClientClose="RefreshParent"  Modal="true" Width="1300px" Height="620px" />
        </Windows>
    </telerik:RadWindowManager>
    <%-- 导入计划、查看计划--结束--%>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
</asp:Content>
