<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MDemandImport.aspx.cs" Inherits="mms.Plan.MDemandImport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
    <style type="text/css">
        .RadUpload {
            width:200px;
        }
    </style>
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
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_DemandDetailedList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="RB_Import">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_DemandDetailedList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                       <telerik:AjaxSetting AjaxControlID="RB_Clear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGridImport" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                    <telerik:AjaxUpdatedControl ControlID="HFGridItemsCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                       <telerik:AjaxSetting AjaxControlID="RB_Delete">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGridImport" LoadingPanelID="RadAjaxLoadingPanelLoading" />
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
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                var deleteButtonID;
                var importButtonID;

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
                                    //   var grid = $find("<%=RadGridImport.ClientID%>");
                                    //   var masterTableView = grid.get_masterTableView();
                                     //  masterTableView.selectAllItems();
                                     //  masterTableView.deleteSelectedItems();
                                       //  masterTableView.rebind();
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
                                        $find("<%= RadNotificationAlert.ClientID %>").set_text("请选择代删除的记录");
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

                                    //    var grid = $find("<%=RadGridImport.ClientID%>");
                                    //    var masterTableView = grid.get_masterTableView();
                                        //   masterTableView.deleteSelectedItems();
                                        //maserTableView.rebind();
                                        
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
                <div style="width: 100%; float: left; padding-top:10px; border-bottom: 1px solid Black; font-size: 16px; font-weight: bold;">
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
                                <telerik:RadTextBox ID="RTB_ProductName" runat="server" Width="120px"></telerik:RadTextBox></td>
                            <td style="text-align: right;">产品图号：</td>
                            <td>
                                <telerik:RadTextBox ID="RTB_DrawingNum" runat="server" Width="120px"></telerik:RadTextBox></td>
                            <td style="text-align: right;">任务号：</td>
                            <td>
                                <telerik:RadTextBox ID="RTB_TaskNum" runat="server" Width="120px"></telerik:RadTextBox></td>
                            <td>交付时间：</td>
                            <td>
                                <telerik:RadDatePicker ID="RDP_StartDate" runat="server" Width="120px"></telerik:RadDatePicker>
                                ～<telerik:RadDatePicker ID="RDP_EndDate" runat="server" Width="120px"></telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadButton ID="RB_Query" runat="server" Text="筛选" OnClick="RB_Query_Click"></telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                     <telerik:RadButton ID="RB_Add" runat="server" Text="归档" CssClass="floatright" Visible="false" CommandName="File" OnClientClicking="confirmWindow" OnClick="RB_Add_Click"></telerik:RadButton>
                </div>

              <div style="position:fixed; right:0px; top:0px;">
                <telerik:RadButton ID="BtnClose" runat="server" Text="关闭" AutoPostBack="false" OnClientClicking="CloseWindow1" CssClass="floatright"></telerik:RadButton>
             </div>
             <div style="width: 100%; float: left;">
                    <table style="text-align: left; width:100%; vertical-align: middle;">
                        <tr>
                               <td style="width:110px;">申请部门：</td>
                            <td style="width:80px;">
                                <telerik:RadComboBox ID="RadComboBox_Dept1" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                            OnSelectedIndexChanged="RadComboBox_Dept_SelectedIndexChanged1" Width="100" Enabled="False">
                                </telerik:RadComboBox>
                                 </td>
                            <td style="width:90px;">申请人：</td>
                            <td style="width:80px;">
                                <telerik:RadComboBox ID="RadComboBox_User1" runat="server" Width="100" Enabled="False"></telerik:RadComboBox>
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
                                <telerik:GridBoundColumn DataField="RowsID" HeaderText="序号" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProductName" HeaderText="产品名称" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaskDrawingCode" HeaderText="产品图号" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Stage1" HeaderText="阶段" ItemStyle-Width="40px" HeaderStyle-Width="40px">
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
                                型号投产计划任务列表
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
        



            <div id="div_no_submit1" style="font-size: 12px; text-align: center; margin-bottom: 20px;">
                 <div class="divViewPanel">
                  <telerik:RadGrid ID="RadGridImport" runat="server" AllowPaging="True"  DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                       OnNeedDataSource="RadGridImport_NeedDataSource" OnItemDataBound="RadGrid_Importlist_ItemDataBound" PageSize="50"
                      AllowMultiRowSelection="true" >
                        <HeaderStyle HorizontalAlign="Center" Font-Size="10px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="260px"></Scrolling>
                        </ClientSettings>
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                              <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />

                            
                            			  <CommandItemTemplate>
                          从Excel导入的物资需求列表
                         <telerik:RadButton ID="RB_Import" runat="server" Text="导入" OnClick="RBImport_Click" OnClientClicking="ShowRadWindowImport"></telerik:RadButton>
                         <telerik:RadButton ID="RB_Delete" runat="server" Text="删除"  OnClick="RBDelete_Click" OnClientClicking="ShowRadWindowDelete"></telerik:RadButton>
                         <telerik:RadButton ID="RB_Clear" runat="server" Text="清空"  OnClick="RBClear_Click" OnClientClicking="ShowRadWindowClear"></telerik:RadButton>
                                  

                        </CommandItemTemplate>
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn">
                                </telerik:GridClientSelectColumn>
                                <telerik:GridBoundColumn DataField="ID" HeaderText="序号" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn HeaderText="领料<br />部门" HeaderStyle-HorizontalAlign="Center" UniqueName="MaterialDept"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                       <ItemTemplate>
                                          <telerik:RadComboBox ID="RadComboBoxMaterialDept1" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                            OnSelectedIndexChanged="RadComboBoxMaterialDept_SelectedIndexChanged1" Width="70" Enabled="False">
                                        </telerik:RadComboBox>
                                       </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="零件类型" HeaderStyle-HorizontalAlign="Center" UniqueName="LingJian_Type" HeaderStyle-width="100px" ItemStyle-Width="100px">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemTemplate>
                                    
                                          <telerik:RadComboBox ID="RDDL_LingJian_Type" runat="server" Width="100px" AutoPostBack="true"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceLingJian_Type" DataTextField="LingJian_Type_Name" DataValueField="LingJian_Type_Code">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>

                                       </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                            

                                    
                           


                                    <telerik:GridBoundColumn DataField="Material_Tech_Condition" FilterControlAltText="Filter Material_Tech_Condition column" HeaderStyle-HorizontalAlign="Center" HeaderText="技术条件" SortExpression="Material_Tech_Condition" UniqueName="Material_Tech_Condition">
                                    </telerik:GridBoundColumn>
                                     <telerik:GridBoundColumn DataField="Technics_Line" FilterControlAltText="Filter Technics_Line column" HeaderStyle-HorizontalAlign="Center" HeaderText="工艺路线" SortExpression="Technics_Line" UniqueName="Technics_Line">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="物资名称" SortExpression="Material_Name" UniqueName="Material_Name">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MAT_UNIT" FilterControlAltText="Filter MAT_UNIT column" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ROUGH_SIZE" FilterControlAltText="Filter ROUGH_SIZE column" HeaderText="物资尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                                    </telerik:GridBoundColumn>

                                   <telerik:GridBoundColumn DataField="Mat_Rough_Weight" FilterControlAltText="Filter Mat_Rough_Weight column" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                                    </telerik:GridBoundColumn>

                                   <telerik:GridBoundColumn DataField="Mat_Pro_Weight" FilterControlAltText="Filter Mat_Pro_Weight column" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Rough_Spec" FilterControlAltText="Filter Rough_Spec column" HeaderText="物资规格" SortExpression="Rough_Spec" UniqueName="Rough_Spec">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Material_Mark" FilterControlAltText="FilterMaterial_Mark column" HeaderText="物资牌号" SortExpression="Material_Mark" UniqueName="Material_Mark">
                                    </telerik:GridBoundColumn>

                                     <telerik:GridBoundColumn DataField="Comment" FilterControlAltText="Filter Comment column" HeaderText="备注" SortExpression="Comment" UniqueName="Comment">
                                    </telerik:GridBoundColumn>
                         
                                <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求时间" DataFormatString="{0:yyyy-MM-dd}" >
                                </telerik:GridBoundColumn>
                                </Columns>		
						</MasterTableView>
                    </telerik:RadGrid>
          
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
                <div id="div_no_submit" style="font-size: 14px; text-align: center; margin-bottom: 20px;">
                    <div class="divViewPanel">
                        <telerik:RadGrid ID="RadGrid_DemandDetailedList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                            OnNeedDataSource="RadGrid_DemandDetailedList_NeedDataSource" OnItemCommand="RadGrid_DemandDetailedList_ItemCommand" PageSize="50">
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                                <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                   <CommandItemTemplate>
						   未提交申请
                                             </CommandItemTemplate>
                                 <Columns>
                                    <telerik:GridBoundColumn DataField="ID" HeaderText="序号" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DRAWING_NO" FilterControlAltText="Filter DRAWING_NO column" HeaderStyle-HorizontalAlign="Center" HeaderText="产品图号" SortExpression="DRAWING_NO" UniqueName="DRAWING_NO">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                                    </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn DataField="Material_Code" FilterControlAltText="Filter Material_Code column" HeaderText="产品编号" SortExpression="Material_Code" UniqueName="Material_Code">
                                    </telerik:GridBoundColumn>
                                   <telerik:GridBoundColumn DataField="TDM_Description" FilterControlAltText="Filter TDM_Description column" HeaderText="产品名称" SortExpression="TDM_Description" UniqueName="TDM_Description">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Material_Tech_Condition" FilterControlAltText="Filter Material_Tech_Condition column" HeaderStyle-HorizontalAlign="Center" HeaderText="技术条件" SortExpression="Material_Tech_Condition" UniqueName="Material_Tech_Condition">
                                    </telerik:GridBoundColumn>
                                     <telerik:GridBoundColumn DataField="Technics_Line" FilterControlAltText="Filter Technics_Line column" HeaderStyle-HorizontalAlign="Center" HeaderText="工艺路线" SortExpression="Technics_Line" UniqueName="Technics_Line">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Stage" FilterControlAltText="Filter Stage column" HeaderText="研制阶段" SortExpression="Stage" UniqueName="Stage">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="物资名称" SortExpression="Material_Name" UniqueName="Material_Name">
                                    </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="ROUGH_SIZE" FilterControlAltText="Filter ROUGH_SIZE column" HeaderText="物资尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Rough_Spec" FilterControlAltText="Filter Rough_Spec column" HeaderText="物资规格" SortExpression="Rough_Spec" UniqueName="Rough_Spec">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Material_Mark" FilterControlAltText="FilterMaterial_Mark column" HeaderText="物资牌号" SortExpression="Material_Mark" UniqueName="Material_Mark">
                                    </telerik:GridBoundColumn>

                                     <telerik:GridBoundColumn DataField="Comment" FilterControlAltText="Filter Comment column" HeaderText="备注" SortExpression="Comment" UniqueName="Comment">
                                    </telerik:GridBoundColumn>

                                    
                                    <telerik:GridBoundColumn DataField="MAT_UNIT" FilterControlAltText="Filter MAT_UNIT column" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column" HeaderText="物资件数" SortExpression="Quantity" UniqueName="Quantity">
                                    </telerik:GridBoundColumn>
                                 
                                     
                                
                                    <telerik:GridBoundColumn DataField="Mat_Rough_Weight" FilterControlAltText="Filter Mat_Rough_Weight column" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                                    </telerik:GridBoundColumn>
                                   <telerik:GridBoundColumn DataField="Mat_Pro_Weight" FilterControlAltText="Filter Mat_Pro_Weight column" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                                    </telerik:GridBoundColumn>

                                     <telerik:GridBoundColumn DataField="NumCasesSum" FilterControlAltText="Filter NumCasesSum column" HeaderText="共计需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                                    </telerik:GridBoundColumn>
                                     <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求总量kg" UniqueName="DemandNumSum">
                                    </telerik:GridBoundColumn>

                                     <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求时间" DataFormatString="{0:yyyy-MM-dd}" >
                                </telerik:GridBoundColumn>
                               
                                  
                                    <telerik:GridTemplateColumn HeaderText="操作" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <telerik:RadButton ID="RadButtonDelete" runat="server" Text="删除" OnClientClicking="CustomRadWindowConfirm" CommandName="delete"></telerik:RadButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
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

