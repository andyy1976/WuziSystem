<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import_MaterialRequirement.aspx.cs" Inherits="mms.Plan.Import_MaterialRequirement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server">
		</telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" Skin="Silk"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGridImport" />
                        <telerik:AjaxUpdatedControl ControlID="HFGridItemsCount" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        <telerik:AjaxUpdatedControl ControlID="HFFileName" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="RadGrid_DemandDetailedList">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_DemandDetailedList" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="RB_Import">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_DemandDetailedList" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                       <telerik:AjaxSetting AjaxControlID="RB_Clear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGridImport" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                    <telerik:AjaxUpdatedControl ControlID="HFGridItemsCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                       <telerik:AjaxSetting AjaxControlID="RB_Delete">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGridImport" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                    <telerik:AjaxUpdatedControl ControlID="HFGridItemsCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="RadComboBox_Dept1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadComboBox_User1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="RadComboBoxMaterialDept">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxShipping_Address" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" MinDisplayTime="0"></telerik:RadAjaxLoadingPanel>  
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                        <script type="text/javascript">
                            var deleteButtonID;
                            var importButtonID;
                            function EnterKeyProcessing(sender, eventArgs) {
                                var c = eventArgs.get_keyCode();
                                if ((c == 13)) {
                                    eventArgs.set_cancel(true);
                                }
                            }

                            function CloseWindow1(args) {
                                var oWindow = null;
                                if (window.radWindow) oWindow = window.radWindow;
                                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                                var oArg = new Object();
                                oWindow.BrowserWindow.refreshGrid();
                                oWindow.close(oArg);
                            }
                            function validationFailed(sender, eventArgs) {
                                $(".ErrorHolder").append("<p>Validation failed for '" + eventArgs.get_fileName() + "'.</p>").fadeIn("slow");
                            }
                            function fileUploaded(sender, args) {
                                $find("<%=RadAjaxManager1.ClientID %>").ajaxRequest();
                                   setTimeout(function () {
                                       sender.deleteFileInputAt(0);
                                   }, 10);
                            }

             
				
			    
                            //导入
                            function ShowRadWindowImport(sender, args)
                            {
                                if (document.getElementById("<%= HFGridItemsCount.ClientID %>").value == "0")
                                {
                                    $find("<%= RadNotificationAlert.ClientID %>").set_text("没有可导入需求清单");
                                    $find("<%= RadNotificationAlert.ClientID %>").show();
                                }
                                else
                                {                
                                 
                                    $find("<%= confirmWindowImport.ClientID %>").show();
                                    importButtonID = sender.get_id();
                                    args.set_cancel(true);
                                   }

                            }
                            function YesOrNoClicked1(sender, args) {
                                var oWnd = $find("<%=confirmWindowImport.ClientID %>");
                                    oWnd.close();
                                    if (sender.get_text() == "是") {
                                        $find(importButtonID).click();
                                    }
                                }
                            //清空
                            function ShowRadWindowClear(sender, args) {
                                if (document.getElementById("<%= HFGridItemsCount.ClientID %>").value == "0") {
                                    $find("<%= RadNotificationAlert.ClientID %>").set_text("没有数据");
                                    $find("<%= RadNotificationAlert.ClientID %>").show();
                                }
                                else {

                                    $find("<%= confirmWindowClear.ClientID %>").show();
                                    clearButtonID = sender.get_id();
                                    args.set_cancel(true);
                                }

                            }
                            function YesOrNoClicked2(sender, args) {
                                var oWnd = $find("<%=confirmWindowClear.ClientID %>");
                                   oWnd.close();
                                   if (sender.get_text() == "是") {
                                       $find(clearButtonID).click();
                                   }
                               }
                            //删除
                            function ShowRadWindowDelete(sender, args) {

                                if (document.getElementById("<%= HFGridItemsCount.ClientID %>").value == "0") {
                                    $find("<%= RadNotificationAlert.ClientID %>").set_text("没有可导入需求清单");
                                    $find("<%= RadNotificationAlert.ClientID %>").show();
                                }
                                else {
                                    var grid = $find("<%=RadGridImport.ClientID%>");
                                    var masterTableView = grid.get_masterTableView();

                                    var selectedItems = masterTableView.get_selectedItems();
                                    if (selectedItems.length <= 0) {
                                        $find("<%= RadNotificationAlert.ClientID %>").set_text("请选择待删除的记录");
                                        $find("<%= RadNotificationAlert.ClientID %>").show();
                                    }
                                    else {

                                        $find("<%= confirmWindowDelete.ClientID %>").show();
                                        deleteButtonID = sender.get_id();
                                        args.set_cancel(true);
                                    }
                                }

                            }
                            function YesOrNoClicked3(sender, args) {
                                var oWnd = $find("<%=confirmWindowDelete.ClientID %>");
                                    oWnd.close();
                                    if (sender.get_text() == "是") {
                                       
                                        $find(deleteButtonID).click();
                                  
                                    }
                            }

                            //
                            function CustomRadWindowConfirm(sender, args) {
                                $find("<%=confirmDeleteWindow.ClientID %>").show();
                                deleteButtonID = sender.get_id();
                                args.set_cancel(true);
                            }
                            function YesOrNoClicked(sender, args) {
                                var oWnd = $find("<%=confirmDeleteWindow.ClientID %>");
                                oWnd.close();
                                if (sender.get_text() == "是") {
                                    $find(deleteButtonID).click();
                                }
                            }

           
            </script>
        </telerik:RadCodeBlock>

            <div style="width: 100%; margin: 0px auto;">
                <div style="width: 100%; float: left; padding-top:10px; border-bottom: 1px solid Black; font-size: 16px; font-weight: normal;">
                    编号：<asp:Label ID="lblPlanCode" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    型号：<asp:Label ID="lblModel" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    计划包名称：<telerik:RadTextBox ID="RTB_PlanName" runat="server" ></telerik:RadTextBox>
                       <asp:Label ID="lblPlanName" runat="server"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    备注：<telerik:RadTextBox ID="RTB_Remark" runat="server"></telerik:RadTextBox>
                    <asp:Label ID="lblRemark" runat="server"></asp:Label>
                    <asp:HiddenField ID="HFState" runat="server" />                   
                </div>
                <div style="width: 100%; float: left; margin-top: 10px;">
                    <table style="text-align: left;" id="table1" runat="server" visible="false">
                        <tr>
                            <td style="text-align: right;">产品名称：</td>
                            <td>
                                <telerik:RadTextBox ID="RTB_ProductName" runat="server" Width="120px">
                                     <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                </telerik:RadTextBox></td>
                            <td style="text-align: right;">产品图号：</td>
                            <td>
                                <telerik:RadTextBox ID="RTB_DrawingNum" runat="server" Width="120px">
                                     <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                </telerik:RadTextBox></td>
                            <td style="text-align: right;">任务号：</td>
                            <td>
                                <telerik:RadTextBox ID="RTB_TaskNum" runat="server" Width="120px">
                                     <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                </telerik:RadTextBox></td>
                            <td>交付时间：</td>
                            <td>
                                <telerik:RadDatePicker ID="RDP_StartDate" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                </telerik:RadDatePicker>
                                ～<telerik:RadDatePicker ID="RDP_EndDate" runat="server" Width="120px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                 </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadButton ID="RB_Query" runat="server" Text="筛选" OnClick="RB_Query_Click"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </div>

             <div style="width: 100%; float: left;">
                    <table style="text-align: left; width:100%; vertical-align: middle;">
                        <tr>
                               <td style="width:110px;">申请部门：</td>
                            <td style="width:80px;">
                                <telerik:RadComboBox ID="RadComboBox_Dept" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                            OnSelectedIndexChanged="RadComboBox_Dept_SelectedIndexChanged" Width="100" Enabled="False">
                                </telerik:RadComboBox>
                                 </td>
                            <td style="width:90px;">申请人：</td>
                            <td style="width:80px;">
                                <telerik:RadComboBox ID="RadComboBox_User" runat="server" Width="100" Enabled="False"></telerik:RadComboBox>
                            </td>
                            <td style="width:110px;">申请时间：</td>
                            <td style="width:100px;"><span id="span_apply_time1" runat="server"></span></td>
                            <td style="width: 150px;">
                                <telerik:RadAsyncUpload runat="server" ID="RadAsyncUpload1" AllowedFileExtensions="xlsx,xls" Width="150px"
                                    OnFileUploaded="RadAsyncUpload1_FileUploaded" MaxFileInputsCount="1" OnClientValidationFailed="validationFailed"
                                    UploadedFilesRendering="BelowFileInput" OnClientFileUploaded="fileUploaded">
                                    <Localization Select="选择文件" />
                                </telerik:RadAsyncUpload>
                            </td>
                            <td>
                                <asp:HiddenField ID="HFFileName" runat="server" />
                                <asp:HiddenField ID="HFGridItemsCount" runat="server" Value="0" />
                                <asp:HiddenField ID="hfBh" runat="server" />
                                <asp:HiddenField ID="hfTaskId" runat="server" />
                            </td>
                            <td style="width:80px;">
                                <telerik:RadButton ID="btnDown" runat="server" Text="下载模版" OnClick="btnDown_Click">
                                </telerik:RadButton>
                            </td>
                            <td style="width:200px;">
                                <span title="1、导入数据必须在Sheet1工作簿内；2、导入的列名称必须含有：产品名称、技术条件、工艺路线、零件类型、物资编码、单件质量、每产品质量、物资尺寸、需求时间">导入Excel文件说明</span>
                            </td>
                        </tr>
                    </table>
             </div>

                <div style="width: 100%; float: left; margin-top: 10px;">
                    <telerik:RadGrid ID="RadGridP_Pack_Task" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="false"
                        OnNeedDataSource="RadGridP_Pack_Task_NeedDataSource" OnItemCreated="RadGridP_Pack_Task_ItemCreated">
                        <AlternatingItemStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="160px"></Scrolling>
                        </ClientSettings>                        
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="TaskID" ClientDataKeyNames="TaskID" AllowAutomaticInserts="false">
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ItemStyle-Width="30px" HeaderStyle-Width="30px"  >
                                </telerik:GridClientSelectColumn>
                                <telerik:GridBoundColumn DataField="RowsID" HeaderText="序号" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                </telerik:GridBoundColumn>
               
                                <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>                  
                                <telerik:GridBoundColumn DataField="TaskDrawingCode" HeaderText="图号" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Stage1" HeaderText="阶段" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProductName" HeaderText="产品名称" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Unit" HeaderText="计量单位" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MatingNum" HeaderText="单机配套数量" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DefrayNum" HeaderText="交付总数量" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProductionNum" HeaderText="本次投产数量" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PlanFinishTime" HeaderText="计划交付时间" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="是否可展开" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:RadioButtonList ID="RBL_IsSpread" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBL_IsSpread_SelectedIndexChanged">
                                            <asp:ListItem Value="false" Text="否"></asp:ListItem>
                                            <asp:ListItem Value="true" Text="是"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:Label ID="lbl_IsSpread" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <CommandItemTemplate>
                                企业备料计划任务列表
                                <asp:RadioButtonList ID="RBL_IsSpreadAll" runat="server" RepeatDirection="Horizontal" CssClass="floatright" AutoPostBack="true" OnSelectedIndexChanged="RBL_IsSpreadAll_SelectedIndexChanged">
                                    <asp:ListItem Value="false" Text="全否"></asp:ListItem>
                                    <asp:ListItem Value="true" Text="全是"></asp:ListItem>
                                    <asp:ListItem Value="" Text="全取消"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:Label ID="lbl" runat="server" CssClass="floatright" Text="是否展开："></asp:Label>
                            </CommandItemTemplate>
                        </MasterTableView>
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="30px" />
                    </telerik:RadGrid>
                </div>
            </div>

            <div class="divContant"> 
             <div id="div_no_submit1" style="font-size: 13px; text-align: center; margin-bottom: 20px;">
                    <div class="divViewPanel">
                  <telerik:RadGrid ID="RadGridImport" runat="server" AllowPaging="True"  DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                       OnNeedDataSource="RadGridImport_NeedDataSource" OnItemDataBound="RadGrid_Importlist_ItemDataBound" PageSize="50"
                      AllowMultiRowSelection="true" >
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="260px"></Scrolling>
                        </ClientSettings>
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <CommandItemTemplate>
                          从Excel导入的物资需求列表
                        <telerik:RadButton ID="RB_Import" runat="server" Text="导入"  Font-Bold="true" CommandName="Import" OnClick="RBImport_Click" OnClientClicking="ShowRadWindowImport" CssClass="floatleft"></telerik:RadButton>
                        <telerik:RadButton ID="RB_Delete" runat="server" Text="删除"  Font-Bold="true" CommandName="Delete" OnClick="RBDelete_Click" OnClientClicking="ShowRadWindowDelete" CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RB_Clear"  runat="server" Text="清空"  Font-Bold="true" CommandName="Clear" OnClick="RBClear_Click"  OnClientClicking="ShowRadWindowClear"  CssClass="floatright"></telerik:RadButton>

                         </CommandItemTemplate>
                            <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ItemStyle-Width="30px" HeaderStyle-Width="30px"  >
                                    </telerik:GridClientSelectColumn>
                               
                                    <telerik:GridBoundColumn DataField="ID" ItemStyle-Width="40px" HeaderStyle-Width="40px" HeaderText="序号" SortExpression="ID" UniqueName="ID">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Project" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="型号" SortExpression="Project" UniqueName="Project" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Drawing_No" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="图号" SortExpression="Drawing_No" UniqueName="Drawing_No">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" ItemStyle-Width="80px" HeaderStyle-Width="80px" Visible="false">
                                    </telerik:GridBoundColumn>  
                                    <telerik:GridBoundColumn DataField="TDM_Description" ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderText="产品名称" SortExpression="TDM_Description" UniqueName="TDM_Description" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LingJian_Type" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="零件类型" SortExpression="LingJian_Type" UniqueName="LingJian_Type" Visible="true">
                                    </telerik:GridBoundColumn>
                                   
                                    <telerik:GridBoundColumn DataField="Technics_Line"  ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderText="工艺路线" SortExpression="Technics_Line" UniqueName="Technics_Line">
                                    </telerik:GridBoundColumn>
                            
                                        
                                    <telerik:GridBoundColumn DataField="Material_Name" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="物资名称" SortExpression="Material_Name" UniqueName="Material_Name">
                                    </telerik:GridBoundColumn>   
                         
             

                                    <telerik:GridBoundColumn DataField="Material_Mark" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="物资牌号" SortExpression="Material_Mark" UniqueName="Material_Mark">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CN_Material_State" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="供应状态" SortExpression="CN_Material_State" UniqueName="CN_Material_State" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Material_Tech_Condition" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="技术条件" SortExpression="Material_Tech_Condition" UniqueName="Material_Tech_Condition" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Rough_Spec" ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderText="坯料规格" SortExpression="Rough_Spec" UniqueName="Rough_Spec">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Dinge_Size" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="坯料尺寸" SortExpression="Dinge_Size" UniqueName="Dinge_Size" Visible="true">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Mat_Rough_Weight" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Mat_Pro_Weight" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight" Visible="true">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="ItemCode1" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="ROUGH_SIZE" ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderText="需求尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                                    </telerik:GridBoundColumn>
                                   <telerik:GridBoundColumn DataField="Special_Needs" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="特殊需求" SortExpression="Special_Needs" UniqueName="Special_Needs">
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="NumCasesSum" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MAT_UNIT" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT" Visible="true">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="DemandNumSum" ItemStyle-Width="70px" HeaderStyle-Width="70px" HeaderText="需求数量" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                                    </telerik:GridBoundColumn> 
                        
                                    <telerik:GridBoundColumn DataField="Tech_Quantity"  HeaderText="工艺数量" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Tech_Quantity" UniqueName="Tech_Quantity">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Technics_Comment" HeaderText="路线备注" ItemStyle-Width="120px" HeaderStyle-Width="120px" SortExpression="Technics_Comment" UniqueName="Technics_Comment">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Memo_Quantity"  HeaderText="备件数量" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Memo_Quantity" UniqueName="Memo_Quantity">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Mat_Comment" HeaderText="定额备注" ItemStyle-Width="120px" HeaderStyle-Width="120px" SortExpression="Mat_Comment" UniqueName="Mat_Comment">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="DemandDate" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="需求日期" DataFormatString="{0:yyyy-MM-dd}" SortExpression="DemandDate" UniqueName="DemandDate" >
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Unit_Price" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="单价" SortExpression="Unit_Price" UniqueName="Unit_Price" Visible="false" >
                                    </telerik:GridBoundColumn>
                                   <telerik:GridBoundColumn DataField="Urgency_Degre" HeaderText="紧急程度" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Urgency_Degre" UniqueName="Urgency_Degre" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Secret_Level" UniqueName="Secret_Level" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Use_Des" HeaderText="用途" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="Use_Des" UniqueName="Use_Des" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" ItemStyle-Width="70px" HeaderStyle-Width="70px" SortExpression="Shipping_Address" UniqueName="Shipping_Address" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Certification" UniqueName="Certification" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Attribute4" HeaderText="国产/进口" ItemStyle-Width="80px" HeaderStyle-Width="80px"  SortExpression="Attribute4" UniqueName="Attribute4">
                                    </telerik:GridBoundColumn>
                                 
                              
                                    <telerik:GridBoundColumn DataField="MANUFACTURER" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="生产厂家" SortExpression="MANUFACTURER" UniqueName="MANUFACTURER" Visible="true" >
                                    </telerik:GridBoundColumn>
                         
                                    <telerik:GridTemplateColumn HeaderText="领料部门" ItemStyle-Width="80px" HeaderStyle-Width="80px" UniqueName="MaterialDept" Visible="false">
                                           <ItemTemplate>
                                              <telerik:RadComboBox ID="RadComboBoxMaterialDept" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                                Enabled="False">
                                            </telerik:RadComboBox>
                                           </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>		
						</MasterTableView>
                    </telerik:RadGrid>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from V_Get_Sys_Dept_ShipAddrByDeptID"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceUrgencyDegree" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceSecretLevel" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceUseDes" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE'"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceLingJian_Type" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select LingJian_Type_Code, LingJian_Type_Name from Sys_LingJian_Info where Is_Del = 'false'"></asp:SqlDataSource>
                     <asp:SqlDataSource ID="SqlDataSourceStage" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from Sys_Phase order by Code"></asp:SqlDataSource>
                </div>
              </div>
		
		     <telerik:RadWindow ID="confirmWindowImport" runat="server" VisibleTitlebar="false" VisibleStatusbar="false"
            Modal="true" Behaviors="None" Height="120px" Width="320px">
            <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <div style="width: 60px; padding-left: 15px; float: left;">
                        <img src="/Images/images/warnning1.jpg" alt="" />
                    </div>
                    <div style="width: 200px; float: left;">
                        <asp:Label ID="lblConfirm1" Font-Size="14px" Text="确定要导入吗？" runat="server" Font-Bold="true"
                            ForeColor="#25a0da" />
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadButton ID="btnYes1" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked1">
                        <Icon PrimaryIconCssClass="rbOk" />
                    </telerik:RadButton>
                        &nbsp;
                    <telerik:RadButton ID="btnNo1" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked1">
                        <Icon PrimaryIconCssClass="rbCancel" />
                    </telerik:RadButton>
                    </div>
                </div>
            </ContentTemplate>
           </telerik:RadWindow>

          <telerik:RadWindow ID="confirmWindowClear" runat="server" VisibleTitlebar="false" VisibleStatusbar="false"
            Modal="true" Behaviors="None" Height="120px" Width="320px">
            <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <div style="width: 60px; padding-left: 15px; float: left;">
                        <img src="/Images/images/warnning1.jpg" alt="" />
                    </div>
                    <div style="width: 200px; float: left;">
                        <asp:Label ID="Label1" Font-Size="14px" Text="确定要清空吗？" runat="server" Font-Bold="true"
                            ForeColor="#25a0da" />
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadButton ID="RadButton1" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked2">
                        <Icon PrimaryIconCssClass="rbOk" />
                    </telerik:RadButton>
                        &nbsp;
                    <telerik:RadButton ID="RadButton2" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked2">
                        <Icon PrimaryIconCssClass="rbCancel" />
                    </telerik:RadButton>
                    </div>
                </div>
            </ContentTemplate>
           </telerik:RadWindow>
            
           <telerik:RadWindow ID="confirmWindowDelete" runat="server" VisibleTitlebar="false" VisibleStatusbar="false"
            Modal="true" Behaviors="None" Height="120px" Width="320px">
            <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <div style="width: 60px; padding-left: 15px; float: left;">
                        <img src="/Images/images/warnning1.jpg" alt="" />
                    </div>
                    <div style="width: 200px; float: left;">
                        <asp:Label ID="Label2" Font-Size="14px" Text="确定要删除吗？" runat="server" Font-Bold="true"
                            ForeColor="#25a0da" />
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadButton ID="RadButton5" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked3">
                        <Icon PrimaryIconCssClass="rbOk" />
                    </telerik:RadButton>
                        &nbsp;
                    <telerik:RadButton ID="RadButton6" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked3">
                        <Icon PrimaryIconCssClass="rbCancel" />
                    </telerik:RadButton>
                    </div>
                </div>
            </ContentTemplate>
           </telerik:RadWindow>
            </div>
            <div class="divContant">
                <div id="div_no_submit" style="font-size: 12px; text-align: center; margin-bottom: 20px;">
                    <div class="divViewPanel">
                        <telerik:RadGrid ID="RadGrid_DemandDetailedList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                            OnNeedDataSource="RadGrid_DemandDetailedList_NeedDataSource" OnItemCommand="RadGrid_DemandDetailedList_ItemCommand" PageSize="50"
                      AllowMultiRowSelection="true" >
                        <HeaderStyle HorizontalAlign="Center" Font-Size="10px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="260px"></Scrolling>
                        </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                                <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                   <CommandItemTemplate>未提交申请</CommandItemTemplate>
                                 <Columns>
                                     <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ItemStyle-Width="30px" HeaderStyle-Width="30px"  >
                                     </telerik:GridClientSelectColumn>

                                    <telerik:GridTemplateColumn HeaderText="操作" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <telerik:RadButton ID="RadButtonDelete" runat="server" Text="删除" OnClientClicking="CustomRadWindowConfirm" CommandName="delete"></telerik:RadButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="ID" HeaderText="编号" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Model" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="型号工程" SortExpression="Model" UniqueName="Model" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Drawing_No"  HeaderText="图号" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Drawing_No" UniqueName="Drawing_No">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TaskCode"  HeaderText="任务号" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="TaskCode" UniqueName="TaskCode">
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
                                    <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="胚料规格" ItemStyle-Width="100px" HeaderStyle-Width="100px" SortExpression="Rough_Spec" UniqueName="Rough_Spec"></telerik:GridBoundColumn>
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

                                    <telerik:GridBoundColumn DataField="MaterialDept" HeaderText="领料部门" ItemStyle-Width="80px" HeaderStyle-Width="80px"  SortExpression="MaterialDept" UniqueName="MaterialDept" Visible="true">
                                    </telerik:GridBoundColumn>
                            
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid><br />
				        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
				            AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
				        </telerik:RadNotification>
                        <telerik:RadWindow ID="confirmDeleteWindow" runat="server" VisibleTitlebar="false"
                            VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                            <ContentTemplate>
                                <div style="margin-top: 30px; float: left;">
                                    <div style="width: 60px; padding-left: 15px; float: left;">
                                        <img src="../Images/images/warnning1.jpg" alt="" />
                                    </div>
                                    <div style="width: 200px; float: left;">
                                        <asp:Label ID="lblConfirm" Font-Size="14px" Text="确定要删除选定的记录吗？" runat="server" Font-Bold="true"
                                            ForeColor="#25a0da" />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <telerik:RadButton ID="btnYes" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                                            <Icon PrimaryIconCssClass="rbOk" />
                                        </telerik:RadButton>
                                        &nbsp;
                                        <telerik:RadButton ID="btnNo" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                                            <Icon PrimaryIconCssClass="rbCancel" />
                                        </telerik:RadButton>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </telerik:RadWindow>
     </div>
                </div>
            </div>
    </form>
</body>
</html>

