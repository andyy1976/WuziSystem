<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="TechnologyTestAdd.aspx.cs" Inherits="mms.Plan.TechnologyTestAdd" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script src="../Scripts/PubFun.js"></script>
    <link type="text/css" rel="stylesheet" href="../Styles/TechnologyTestAdd.css" />
    <script>       
        function CloseWindow1(args) {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            var oArg = new Object();
            oWindow.BrowserWindow.refreshGrid(args);
            oWindow.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        $(function () {
            $("#btnNew").click(function () {
                $("#div_newTecTest").show();
            })
            $("#btnNewTCode").click(function () {
                $("#ContentPlaceHolder1_txt_TaskCode").val("").removeAttr("disabled");
                //$("#ContentPlaceHolder1_txt_Drawing_No").val("").removeAttr("disabled");

            })
            $("#txt_NumCasesSum").blur(function () {

                var uprice = $("#RTB_Unit_Price").val();
                var NumCasesSum = $("#txt_NumCasesSum").val();
                var numprice = 0;
                if (uprice != "" && NumCasesSum != "") {
                    uprice = parseFloat(uprice);
                    NumCasesSum = parseFloat(NumCasesSum);
                    numprice = uprice * NumCasesSum;
                }
                $("#span_Sum_Price").text(numprice);
            });
            $("#RTB_Unit_Price").blur(function () {

                var uprice = $("#RTB_Unit_Price").val();
                var NumCasesSum = $("#txt_NumCasesSum").val();
                var numprice = 0;
                if (uprice != "" && NumCasesSum != "") {
                    uprice = parseFloat(uprice);
                    NumCasesSum = parseFloat(NumCasesSum);
                    numprice = uprice * NumCasesSum;
                }
                $("#span_Sum_Price").text(numprice);
            });

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; float: left;">
            <div class="divContant">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                        <Scripts>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                        </Scripts>
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
                            <telerik:AjaxSetting AjaxControlID="RadGrid_TechnologyTestList">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                           <telerik:AjaxSetting AjaxControlID="RB_Import">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                     <telerik:AjaxUpdatedControl ControlID="RadBtnSubmit" />
                                      <telerik:AjaxUpdatedControl ControlID="hfBh" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="RadBtnSubmit">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                      <telerik:AjaxUpdatedControl ControlID="hfBh" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                          <telerik:AjaxSetting AjaxControlID="RadBtnSave">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                     <telerik:AjaxUpdatedControl ControlID="RadBtnSubmit" />
                                       <telerik:AjaxUpdatedControl ControlID="hfBh" />    
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
                            
                       <telerik:AjaxSetting AjaxControlID="RadComboBox_Dept">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadComboBox_User" />
                                </UpdatedControls>
                       </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="RadComboBoxMaterialDept">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxShipping_Address" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="RadBtnChoose">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Material_Name" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Material_Mark" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_CN_Material_State" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Material_Tech_Condition" />                       
                                    <telerik:AjaxUpdatedControl ControlID="txt_Mat_Unit" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Rough_Size" />
                                    <telerik:AjaxUpdatedControl ControlID="RTV_Dinge_Size" />
                                    <telerik:AjaxUpdatedControl ControlID="txt_Rough_Spec" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_MaterialsDes" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Unit_Price" />
                                    <telerik:AjaxUpdatedControl ControlID="txt_DemandNumSum" />
                                    <telerik:AjaxUpdatedControl ControlID="span_Sum_Price" />
                                    <telerik:AjaxUpdatedControl ControlID="lblMSG" />
                                    <telerik:AjaxUpdatedControl ControlID="rfv_ItemCode1" />
                                    <telerik:AjaxUpdatedControl ControlID="txt_ItemCode1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="txt_ItemCode1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Material_Name" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Material_Mark" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_CN_Material_State" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Material_Tech_Condition" />   
                                    <telerik:AjaxUpdatedControl ControlID="txt_Mat_Unit" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Rough_Size" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Dinge_Size" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_MaterialsDes" />
                                    <telerik:AjaxUpdatedControl ControlID="txt_Rough_Spec" />
                                    <telerik:AjaxUpdatedControl ControlID="RTB_Unit_Price" />
                                    <telerik:AjaxUpdatedControl ControlID="txt_DemandNumSum" />
                                    <telerik:AjaxUpdatedControl ControlID="span_Sum_Price" />
                                    <telerik:AjaxUpdatedControl ControlID="lblMSG" />
                                    <telerik:AjaxUpdatedControl ControlID="rfv_ItemCode1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="Panel1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="Panel1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="RB_Search">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
            </telerik:RadAjaxLoadingPanel>
            <asp:HiddenField ID="HiddenField" runat="server" Value="" ClientIDMode="Static" />
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

             
                            function ShowItemCode() {
                                $find("<%=RadWindow1.ClientID %>").Show();                          
                            }
                       
                            function ShowApprove(sender,args) {
                                $find("<%=RadWindowApprove.ClientID%>").show();
                                args.set_cancel(true);
                            }
                            function SubmitOrCancel(sender, args) {
                          
                                 $find("<%=RadWindowApprove.ClientID %>").close();
                          
                                if (sender.get_text() == "提交") {
                                    $find("<%=RadBtnSubmit.ClientID%>").click();
                                }
                            }
                        

                            function confirmWindowChoose(sender, args) {

                          
                                var grid = $find('<%= RadGrid1.ClientID %>');
                               // var masterTableView = grid.get_masterTableView();
                               
                            //    var selectedItems = masterTableView.get_selectedItems();
                             //   if (selectedItems.length <= 0) {
                                    if (grid._selectedIndexes.length == 0) {
                                    var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                           rnal.set_text("请选择一条物资编码记录");
                                           rnal.show();
                                           args.set_cancel(true);
                                       }
                                else {
                                    $find("<%=RadWindow1.ClientID %>").close();
                                     //      var SEG3 = $(selectedItems[0].get_cell("SEG3")).text();
                                        //   document.getElementById("txt_ItemCode1").value = SEG3;
                                       }

                           
                            }

                            function confirmWindowCancel(sender, args) {

                                $find("<%=RadWindow1.ClientID %>").close();

                            }
                        </script>
                    </telerik:RadCodeBlock>
              
            </div>
   
            <div class="divContant">
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
                            </td>
                            <td style="width:80px;">
                                <telerik:RadButton ID="btnDown" runat="server" Text="下载模版" OnClick="btnDown_Click">
                                </telerik:RadButton>
                            </td>
                            <td style="width:200px;">
                                <span title="1、导入数据必须在Sheet1工作簿内；2、导入的列名称必须含有：型号工程、产品名称、产品图号、
                                    任务号、物资牌号、供应状态、技术标准、规格、胚料尺寸、需求尺寸、物资编码、共计需求件数、共计需求数量、
                                    单价、需求时间、紧急程度、密级、用途、研制阶段、配送地址、国产\进口；其中：型号工程、紧急程度、密级、
                                    用途、研制阶段、配送地址的选项必须与后台的设置完全相同">导入Excel文件说明</span>
                            </td>
                        </tr>
                    </table>
                </div>
      
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
                         <telerik:RadButton ID="RB_Import" runat="server" Text="导入" OnClick="RBImport_Click" OnClientClicking="ShowRadWindowImport"></telerik:RadButton>
                         <telerik:RadButton ID="RB_Delete" runat="server" Text="删除"  OnClick="RBDelete_Click" OnClientClicking="ShowRadWindowDelete"></telerik:RadButton>
                         <telerik:RadButton ID="RB_Clear" runat="server" Text="清空"  OnClick="RBClear_Click" OnClientClicking="ShowRadWindowClear"></telerik:RadButton> 
                        </CommandItemTemplate>
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ItemStyle-Width="30px" HeaderStyle-Width="30px"  >
                                </telerik:GridClientSelectColumn>
                                <telerik:GridBoundColumn DataField="ID" ItemStyle-Width="40px" HeaderStyle-Width="40px" HeaderText="序号" SortExpression="ID" UniqueName="ID"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="型号工程" ItemStyle-Width="80px" HeaderStyle-Width="80px" UniqueName="Project">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemTemplate>
                                         <telerik:RadDropDownList ID="RDDL_Project1" runat="server" AutoPostBack="true"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceProject" DataTextField="DICT_Name" DataValueField="DICT_Name">
                                            <Items>
                                            </Items>
                                        </telerik:RadDropDownList>
                                       </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn DataField="TaskCode" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="任务编号" SortExpression="TaskCode" UniqueName="TaskCode">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DRAWING_NO" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="产品图号" SortExpression="DRAWING_NO" UniqueName="DRAWING_NO">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TDM_Description" ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderText="产品名称" SortExpression="TDM_Description" UniqueName="TDM_Description">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Material_Name" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="物资名称" SortExpression="Material_Name" UniqueName="Material_Name">
                                    </telerik:GridBoundColumn>  
                                    <telerik:GridBoundColumn DataField="Material_Mark" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="物资牌号" SortExpression="Material_Mark" UniqueName="Material_Mark">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CN_Material_State" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="供应状态" SortExpression="CN_Material_State" UniqueName="CN_Material_State" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Material_Tech_Condition" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="技术标准" SortExpression="Material_Tech_Condition" UniqueName="Material_Tech_Condition" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Rough_Spec" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="规格" SortExpression="Rough_Spec" UniqueName="Rough_Spec">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Dinge_Size" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="胚料尺寸" SortExpression="Dinge_Size" UniqueName="Dinge_Size" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ItemCode1" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                                    </telerik:GridBoundColumn>
                     
                                    <telerik:GridBoundColumn DataField="MaterialsDes" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="物资描述" SortExpression="MaterialsDes" UniqueName="MaterialsDes" Visible="false">
                                    </telerik:GridBoundColumn> 

                                    <telerik:GridBoundColumn DataField="ROUGH_SIZE" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="需求尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Special_Needs" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="特殊需求" SortExpression="Special_Needs" UniqueName="Special_Needs">
                                    </telerik:GridBoundColumn>

                                   <telerik:GridBoundColumn DataField="NumCasesSum" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MAT_UNIT" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DemandNumSum" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="需求数量" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                                    </telerik:GridBoundColumn>


                                
                                    <telerik:GridBoundColumn DataField="Mat_Rough_Weight" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Mat_Pro_Weight" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight" Visible="False">
                                    </telerik:GridBoundColumn>

                                 
                                    <telerik:GridBoundColumn DataField="Unit_Price" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="单价" SortExpression="Unit_Price" UniqueName="Unit_Price">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="DemandDate" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="需求时间" DataFormatString="{0:yyyy-MM-dd}" SortExpression="DemandDate" UniqueName="DemandDate" >
                                    </telerik:GridBoundColumn>

                                   <telerik:GridTemplateColumn HeaderText="紧急程度" ItemStyle-Width="70px" HeaderStyle-Width="70px" UniqueName="Urgency_Degre">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxUrgencyDegree" runat="server" Width="60px" AutoPostBack="true"
                                           Culture="zh-CN" DataSourceID="SqlDataSourceUrgencyDegree" DataTextField="DICT_Name" DataValueField="DICT_Code">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn HeaderText="密级" ItemStyle-Width="80px" HeaderStyle-Width="80px" UniqueName="Secret_Level" >
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxSecretLevel" runat="server" AutoPostBack="true"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceSecretLevel" DataTextField="SecretLevel_Name" DataValueField="SecretLevel_Name">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn HeaderText="用途" HeaderStyle-HorizontalAlign="Center" UniqueName="Use_Des"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxUseDes" runat="server" Width="70px" AutoPostBack="true"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceUseDes" DataTextField="DICT_Name" DataValueField="DICT_Code">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="研制阶段" ItemStyle-Width="80px" HeaderStyle-Width="80px" UniqueName="stage">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                          <telerik:RadComboBox ID="RadComboBoxStage" runat="server" AutoPostBack="true"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceStage" DataTextField="Phase" DataValueField="Code">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                  </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="合格证" ItemStyle-Width="60px"  HeaderStyle-width="60px"  UniqueName="Certification">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxCertification" runat="server" AutoPostBack="true" >
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y" />
                                                <telerik:RadComboBoxItem Text="N" Value="N" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                             
                                <telerik:GridTemplateColumn HeaderText="配送地址" ItemStyle-Width="80px" HeaderStyle-Width="80px" UniqueName="Shipping_Address">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxShippingAddress" runat="server" Culture="zh-CN"
                                            AutoPostBack="true">
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               

                                 <telerik:GridTemplateColumn HeaderText="国产/进口" ItemStyle-Width="80px" HeaderStyle-Width="80px" UniqueName="Attribute4" >
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxAttribute4" runat="server" AutoPostBack="true" >
                                            <Items>
                                                <telerik:RadComboBoxItem Text="国产" Value="国产" />
                                                <telerik:RadComboBoxItem Text="进口" Value="进口" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="领料部门" ItemStyle-Width="80px" HeaderStyle-Width="80px" UniqueName="MaterialDept" Visible="false">
                                       <ItemTemplate>
                                          <telerik:RadComboBox ID="RadComboBoxMaterialDept" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                            OnSelectedIndexChanged="RadComboBoxMaterialDept_SelectedIndexChanged" Enabled="False">
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
                    <asp:SqlDataSource ID="SqlDataSourceProject" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select DICT_Code, DICT_Name from GetBasicdata_T_Item  where DICT_CLASS = 'CUX_DM_PROJECT' and ENABLED_FLAG = 'Y' order by DICT_Name"></asp:SqlDataSource>
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
                        <telerik:RadGrid ID="RadGrid_TechnologyTestList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                            OnNeedDataSource="RadGrid_TechnologyTestList_NeedDataSource" OnItemCommand="RadGrid_TechnologyTestList_ItemCommand" PageSize="50"
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
                                     <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ItemStyle-Width="40px" HeaderStyle-Width="40px"  >
                                     </telerik:GridClientSelectColumn>
                                     <telerik:GridTemplateColumn HeaderText="操作" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <telerik:RadButton ID="RadButtonDelete" runat="server" Text="删除" OnClientClicking="CustomRadWindowConfirm" CommandName="delete"></telerik:RadButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" ItemStyle-Width="50px" HeaderStyle-Width="50px" HeaderText="序号" SortExpression="rownum" UniqueName="rownum" visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Project" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="型号工程" SortExpression="Project" UniqueName="Project">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TaskCode" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="任务编号" SortExpression="TaskCode" UniqueName="TaskCode">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DRAWING_NO" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="产品图号" SortExpression="DRAWING_NO" UniqueName="DRAWING_NO">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TDM_Description" ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderText="产品名称" SortExpression="TDM_Description" UniqueName="TDM_Description">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Material_Name" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="物资名称" SortExpression="Material_Name" UniqueName="Material_Name">
                                    </telerik:GridBoundColumn> 
                                    <telerik:GridBoundColumn DataField="Material_Mark" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="物资牌号" SortExpression="Material_Mark" UniqueName="Material_Mark">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CN_Material_State" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="供应状态" SortExpression="CN_Material_State" UniqueName="CN_Material_State" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Material_Tech_Condition" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="技术标准" SortExpression="Material_Tech_Condition" UniqueName="Material_Tech_Condition" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Rough_Spec" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="规格" SortExpression="Rough_Spec" UniqueName="Rough_Spec">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Dinge_Size" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="胚料尺寸" SortExpression="Dinge_Size" UniqueName="Dinge_Size" Visible="true">
                                    </telerik:GridBoundColumn> 
                                    <telerik:GridBoundColumn DataField="ItemCode1" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                                    </telerik:GridBoundColumn>
                      
                                   
                                    <telerik:GridBoundColumn DataField="ROUGH_SIZE" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="需求尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Special_Needs" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="特殊需求" SortExpression="Special_Needs" UniqueName="Special_Needs">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="NumCasesSum" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MAT_UNIT" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DemandNumSum" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="需求数量" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Mat_Rough_Weight" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight" Visible="false">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Unit_Price" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="单价" SortExpression="Unit_Price" UniqueName="Unit_Price">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="DemandDate" ItemStyle-Width="120px" HeaderStyle-Width="120px" HeaderText="需求时间" DataFormatString="{0:yyyy-MM-dd}" SortExpression="DemandDate" UniqueName="DemandDate" >
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="UrgencyDegre" ItemStyle-Width="60px" HeaderStyle-Width="60px" HeaderText="紧急程度" SortExpression="UrgencyDegre" UniqueName="UrgencyDegre">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Secret_Level" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="密级" SortExpression="Secret_Level" UniqueName="Secret_Level">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="UseDes" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="用途" SortExpression="UseDes" UniqueName="UseDes">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="stage1" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="研制阶段" SortExpression="stage1" UniqueName="stage1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" ItemStyle-Width="60px" HeaderStyle-Width="60px" SortExpression="Certification" UniqueName="Certification" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Shipping_Address"  HeaderText="配送地址" ItemStyle-Width="80px" HeaderStyle-Width="80px" SortExpression="Shipping_Address" UniqueName="Shipping_Address">
                                    </telerik:GridBoundColumn>
                                
                                    <telerik:GridBoundColumn DataField="Attribute4" ItemStyle-Width="80px" HeaderStyle-Width="80px" HeaderText="国产/进口" SortExpression="Attribute4" UniqueName="Attribute4">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MaterialDept" HeaderText="领料部门" ItemStyle-Width="80px" HeaderStyle-Width="80px"  SortExpression="MaterialDept" UniqueName="MaterialDept" Visible="false">
                                    </telerik:GridBoundColumn>

                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid><br />
                        <telerik:RadButton ID="RadBtnSubmit" runat="server" ValidationGroup="3" Text="提交流程平台" Width="120px" OnClick="RadBtnSubmit_Click" OnClientClicking="ShowApprove"></telerik:RadButton>
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
                        <%--查询物资编码窗口--开始--%>                       
                        <telerik:RadWindow ID="RadWindow1" runat="server" Title="查询物资编码"
                            ReloadOnShow="false" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Modal="true" Behaviors="Close,Maximize,Minimize" Width="1100px" Height="500px">
                            <ContentTemplate>
                                <div style="width: 100%; padding: 0px; margin: 0px">
                                    <div style="width: 100%;">
                                        <asp:Panel runat="server" ID="Panel1">
                                            <table id="tableMT" style="margin: 0px auto; width: 100%;">
                                                <tr>
                                                    <th colspan="10" style="font-size: 16px; letter-spacing: 16px;">物资基础库查询</th>
                                                </tr>
                                                <tr>
                                                    <td style="width: 70px;">物资名称：</td>
                                                    <td><telerik:RadTextBox ID="RTB_MaterialName" runat="server" Width="100px">
                                                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                                        </telerik:RadTextBox></td>
                                                    <td style="width:70px">物资牌号：</td>
                                                    <td><telerik:RadTextBox ID="RTB_MaterialPaihao" runat="server" Width="100px">
                                                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                                        </telerik:RadTextBox></td>

                                                    <td style="width:70px">物资规格:</td>
                                                    <td><telerik:RadTextBox ID="RTB_MaterialGuige" runat="server" Width="100px">
                                                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                                        </telerik:RadTextBox></td>

                                                    <td style="width:70px">物资标准：</td>
                                                    <td><telerik:RadTextBox ID="RTB_MaterialBiaozhun" runat="server" Width="100px">
                                                                                   <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                                        </telerik:RadTextBox></td>

                                                    <td style="width:200px" >
                                                        <telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton>
                                                         <telerik:RadButton ID="RadBtnChoose" runat="server" Text="选择该物资编码" CssClass="btn_margin1" Font-Bold="true" 
                                                    CommandName="RadBtnChoose" CausesValidation="true" OnClick="confirmWindowClick"  OnClientClicking="confirmWindowChoose" >
                                                </telerik:RadButton>

                                            <%--    <telerik:RadButton ID="RadBtnCancel" runat="server" Text="取消并退出搜素框" CssClass="btn_margin1" Font-Bold="true" 
                                                    CommandName="CancelCombine"  OnClientClicking="confirmWindowCancel">
                                                </telerik:RadButton>--%>
                                                    </td>
                                                  </tr>
                                                <tr>
                                                    <td style="width: 100px;">物资类别：</td>
                                                    <td>
                                                        <telerik:RadDropDownList ID="RDDLMT" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="RDDLMT_SelectedIndexChanged" AutoPostBack="true" Width="100px">
                                                            <Items>
                                                                <telerik:DropDownListItem Value="" Text="" />
                                                            </Items>
                                                        </telerik:RadDropDownList>
                                                    </td>
                                                    <%-- 
                                                    <td colspan="8">
                                                        <div id="div1" runat="server" visible="false">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 80px;">二级</td>
                                                                    <td>
                                                                        <telerik:RadDropDownList ID="RDDLMT1" runat="server" OnSelectedIndexChanged="RDDLMT1_SelectedIndexChanged" AutoPostBack="true" Width="100px">
                                                                        </telerik:RadDropDownList>
                                                                    </td>
                                                                    <td style="width: 80px;">三级</td>
                                                                    <td>
                                                                        <telerik:RadDropDownList ID="RDDLMT2" runat="server" OnSelectedIndexChanged="RDDLMT2_SelectedIndexChanged" AutoPostBack="true" Width="100px">
                                                                        </telerik:RadDropDownList>
                                                                    </td>
                                                                    <td style="width: 80px;">四级</td>
                                                                    <td>
                                                                        <telerik:RadDropDownList ID="RDDLMT3" runat="server" OnSelectedIndexChanged="RDDLMT3_SelectedIndexChanged" AutoPostBack="true" Width="100px">
                                                                        </telerik:RadDropDownList>
                                                                    </td>
                                                                    <td style="width: 80px;">五级</td>
                                                                    <td>
                                                                        <telerik:RadDropDownList ID="RDDLMT4" runat="server" Width="100px">
                                                                        </telerik:RadDropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="div2" runat="server" visible="false">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 80px;">物资编码：</td>
                                                                    <td>
                                                                        <telerik:RadTextBox ID="RTB_ItemCode" runat="server" Width="100px"></telerik:RadTextBox></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                        --%>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                    <div style="width: 100%;">
                                        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource" AutoGenerateColumns="false"
                                            AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True" AllowMultiRowSelection="False">
                                            <PagerStyle AlwaysVisible="true" />
                                            <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="true" />
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3" ScrollHeight="300px"></Scrolling>
                                            </ClientSettings>
                                            <MasterTableView DataKeyNames="SEG3">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="SEG3" HeaderText="物资编码" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SEG4" HeaderText="描述"></telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                       <div style="margin:20px 0 5px 4px;">
                                               
                                      </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </telerik:RadWindow>
                        <%--查询物资编码窗口--结束--%>
                        <%--选择审批流程--开始----%>
                        <telerik:RadWindow ID="RadWindowApprove" runat="server" Title="业务审批流程"
                            ReloadOnShow="false" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Modal="true" Behaviors="Close,Maximize,Minimize" Width="700px" Height="300px">
                            <ContentTemplate>
                                <table style="margin:50px auto; width:700px; height:120px;">
                                    <tr>
                                        <td>1、<asp:Label ID="lbl_ApproveAccount1" runat="server"></asp:Label></td>
                                        <td><telerik:RadDropDownList ID="RDDL_ApproveAccount1" runat="server" Width="160px"></telerik:RadDropDownList></td>
                                        <td>2、<asp:Label ID="lbl_ApproveAccount2" runat="server"></asp:Label></td>
                                        <td><telerik:RadDropDownList ID="RDDL_ApproveAccount2" runat="server" Width="160px"></telerik:RadDropDownList></td>
                                        <td><asp:Label ID="lbl_ApproveAccount3" runat="server" visible="false"></asp:Label></td>
                                        <td><telerik:RadDropDownList ID="RDDL_ApproveAccount3" runat="server" Width="160px" visible="false"></telerik:RadDropDownList></td>
                                    </tr>
                                    <tr>                                        
                                        <td colspan="3" style="text-align:center;">
                                             <telerik:RadButton ID="RadButton3" runat="server" Text="提交" AutoPostBack="false" OnClientClicked="SubmitOrCancel">
                                                    <Icon PrimaryIconCssClass="rbOk" />
                                                </telerik:RadButton>
                                            &nbsp;
                                                
                                        </td>
                                        <td colspan="3" style="text-align:center;">
                                            <telerik:RadButton ID="RadButton4" runat="server" Text="取消" AutoPostBack="false" OnClientClicked="SubmitOrCancel">
                                                <Icon PrimaryIconCssClass="rbCancel" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </telerik:RadWindow>
                        <%--选择审批流程--结束----%>
                    </div>
                </div>
            </div>
            <div class="divContant">
                <div class="technology_Div_content">
                    <div class="technology_Div_title"><b id="b_title" runat="server"></b></div>

                        <div class="technology_Div_smalltitle"><b>申请人信息</b></div>
                        <div class="technology_Div_detailcontent">
                            <table border="0">
                                <tr>
                                    <td class="technology_Div_detailcontent_title">部门：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBox_Dept" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                            OnSelectedIndexChanged="RadComboBox_Dept_SelectedIndexChanged" Width="150" Enabled="False">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">申请人：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBox_User" runat="server" Width="150" Enabled="False"></telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">申请时间：</td>
                                     <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="span_apply_time" Width="150" MaxLength="30" runat="server" Enabled="false"></telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">型号工程:</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadDropDownList runat="server" ID="RDDL_Project" Width="150px" AppendDataBoundItems="True">
                                            <Items>
                                                <telerik:DropDownListItem Value="" Text="" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>

         
                        <div class="technology_Div_smalltitle"><b>请领信息</b></div>
                        <div class="technology_Div_detailcontent">
                            <table border="0">
                                <tr>
                                    <td class="technology_Div_detailcontent_title">产品名称：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_TDM_Description" Width="150" MaxLength="50" runat="server">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">任务号：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_TaskCode" Width="150" MaxLength="50" runat="server">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_TaskCode" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_TaskCode" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">产品图号：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Drawing_No" Width="150" MaxLength="50" runat="server">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Drawing_No" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Drawing_No" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">需求时间：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadDatePicker ID="DemandDate" runat="server" Width="150px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </div>
         
                        <div class="technology_Div_smalltitle">
						<b>物资信息：（需求数量=单件质量*需求件数）</b>
                          <telerik:RadButton ID="btn_ItemCodeOK" width="120" AutoPostBack="False" ValidationGroup="2" runat="server" Text="点击查询物资编码"  ForeColor="Blue" OnClientClicking="ShowItemCode"></telerik:RadButton>
                        </div>
                        <div class="technology_Div_detailcontent">
                            <table border="0">
                                        <tr>
                                    <td class="technology_Div_detailcontent_title">物资编码：</td>
                                    <td class="technology_Div_detailcontent_content">
                                         <telerik:RadTextBox ID="txt_ItemCode1" Width="150" runat="server" ValidationGroup="2" AutoPostBack="True" OnTextChanged="txt_ItemCode1_OnTextChanged">
                                               <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                         </telerik:RadTextBox>
                                            <asp:Label ID="lblMSG" runat="server" ForeColor="Red"></asp:Label>
                                           <asp:RequiredFieldValidator ID="rfv_ItemCode1" ValidationGroup="2" runat="server" ErrorMessage="*" ControlToValidate="txt_ItemCode1" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">物资描述：</td>
                                    <td colspan="5" class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_MaterialsDes" runat="server" Width="700" MaxLength="200">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">物资名称：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Material_Name" runat="server" Width="150" MaxLength="50">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_MaterialName" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="RTB_Material_Name" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">物资牌号：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Material_Mark" runat="server" Width="150" MaxLength="30">
                                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox></td>
                                    <td class="technology_Div_detailcontent_title">供应状态：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_CN_Material_State" runat="server" Width="150" MaxLength="20">
                                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox></td>
                                    <td class="technology_Div_detailcontent_title">采用标准：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Material_Tech_Condition" runat="server" Width="150" MaxLength="50">
                                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">坯料规格：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Rough_Spec" runat="server" MaxLength="30" Width="150">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">胚料尺寸：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Dinge_Size" runat="server" MaxLength="30" Width="150">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                    </td>

                                    <td class="technology_Div_detailcontent_title">计量单位：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Mat_Unit" runat="server" MaxLength="10" Width="150">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Mat_Unit" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Mat_Unit" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">单件质量：</td>
                                    <td><telerik:RadTextBox ID="RTB_Mat_Rough_Weight" runat="server" MaxLength="10" Width="150" onpaste="return false" onkeyup='clearNoNum(this)'>
                                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox></td>
                                 </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">单价(元）：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Unit_Price" runat="server" MaxLength="10" Width="150" EmptyMessage="0" onpaste="return false" onkeyup='clearNoNum(this)'>
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">需求尺寸：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Rough_Size" runat="server" Width="150" MaxLength="30">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                    </td>

                          
                                    <td class="technology_Div_detailcontent_title">需求数量：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_DemandNumSum" Width="150" runat="server" MaxLength="5" onpaste="return false" onkeyup='clearNoDecimal(this)'>
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_DemandNumSum" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_DemandNumSum" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title" style="width: 10%">需求件数：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_NumCasesSum" ClientIDMode="Static" Width="150" runat="server" MaxLength="5" onpaste="return false" onkeyup='clearNoNum(this)'>
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_NumCasesSum" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_NumCasesSum" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">密级：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxSecretLevel1" runat="server" DataSourceID="SqlDataSourceSecretLevel1"
                                            DataTextField="SecretLevel_Name" DataValueField="SecretLevel_Name" Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceSecretLevel1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)"></asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">总价（元）：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="span_Sum_Price" runat="server" Width="150" EmptyMessage="0" MaxLength="10" onpaste="return false" onkeyup='clearNoNum(this)'>
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                           
                                    </td>
                                    <td class="technology_Div_detailcontent_title">研制阶段：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxStage" runat="server" Width="150">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">用途：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxUseDes1" runat="server" DataSourceID="SqlDataSourceUseDes1"
                                            DataTextField="DICT_Name" DataValueField="DICT_CODE" Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceUseDes1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE' and ENABLED_FLAG = 'Y'"></asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">合格证：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxCertification1" runat="server" Width="150">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y" />
                                                <telerik:RadComboBoxItem Text="N" Value="N" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>

                                         
                                    <td class="technology_Div_detailcontent_title">配送地址：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxShipping_Address" runat="server" Width="150">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">紧急程度：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxUrgencyDegree1" runat="server" DataSourceID="SqlDataSourceUrgencyDegree1"
                                            DataTextField="DICT_Name" DataValueField="DICT_CODE" Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceUrgencyDegree1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL' and ENABLED_FLAG = 'Y'"></asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">领料部门：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxMaterialDept1" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                            OnSelectedIndexChanged="RadComboBoxMaterialDept_SelectedIndexChanged1" Width="150" Enabled="False">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">国产/进口：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadButton ID="RB_Attribute41" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Attribute4" Text="国产" Checked="true" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton ID="RB_Attribute42" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Attribute4" Text="进口" AutoPostBack="false"></telerik:RadButton>
                                    </td>

                                    <td class="technology_Div_detailcontent_title">特殊需求：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="rtb_SpecialNeeds" runat="server" Width="150" MaxLength="20" EmptyMessage="无">
                                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                        </telerik:RadTextBox>
                                        <%-- <asp:RequiredFieldValidator ID="rfv_SpecialNeeds" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="rtb_SpecialNeeds" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="technology_Div_button">
                        <div style="margin-top: 13px;">
                            <telerik:RadButton ID="RadBtnSave" runat="server" ValidationGroup="1" Text="保 存" Width="120px" OnClick="RadBtnSave_Click"></telerik:RadButton>
                            <asp:HiddenField ID="hfBh" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

