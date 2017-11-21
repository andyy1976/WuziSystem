<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialImport.aspx.cs" Inherits="mms.Plan.MaterialImport" %>

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
        <telerik:RadSkinManager runat="server" Skin="Default"></telerik:RadSkinManager>
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
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="RB_Import">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
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
                                <asp:HiddenField ID="hfBh" runat="server" />
                            </td>
                            <td style="width:80px;">
                                <telerik:RadButton ID="btnDown" runat="server" Text="下载模版" OnClick="btnDown_Click">
                                </telerik:RadButton>
                            </td>
                            <td style="width:200px;">
                                <span title="1、导入数据必须在Sheet1工作簿内；2、导入的列名称必须含有：产品图号、任务号、物资编码、共计需求件数、共计需求数量、物资尺寸、单价、需求时间；型号工程、紧急程度、密级、用途、研制阶段、配送地址的选项必须与后台的设置完全相同">导入Excel文件说明</span>
                            </td>
                        </tr>
                    </table>
             </div>

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
                                    <telerik:GridTemplateColumn HeaderText="型号工程" HeaderStyle-HorizontalAlign="Center" UniqueName="Project" HeaderStyle-width="70px" ItemStyle-Width="70px">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemTemplate>
                                         <telerik:RadDropDownList ID="RDDL_Project1" runat="server" Width="100px" AutoPostBack="true"
                                           
                                            Culture="zh-CN" DataSourceID="SqlDataSourceProject" DataTextField="DICT_Name" DataValueField="DICT_Name">
                                            <Items>
                                            </Items>
                                        </telerik:RadDropDownList>
                                       </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="DRAWING_NO" FilterControlAltText="Filter DRAWING_NO column" HeaderStyle-HorizontalAlign="Center" HeaderText="产品图号" SortExpression="DRAWING_NO" UniqueName="DRAWING_NO">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="物资名称" SortExpression="Material_Name" UniqueName="Material_Name">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MAT_UNIT" FilterControlAltText="Filter MAT_UNIT column" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ROUGH_SIZE" FilterControlAltText="Filter ROUGH_SIZE column" HeaderText="物资尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Rough_Spec" FilterControlAltText="Filter Rough_Spec column" HeaderText="物资规格" SortExpression="Rough_Spec" UniqueName="Rough_Spec">
                                    </telerik:GridBoundColumn>
                                     <telerik:GridBoundColumn DataField="Special_Needs" FilterControlAltText="Filter Special_Needs column" HeaderText="特殊需求" SortExpression="Special_Needs" UniqueName="Special_Needs">
                                    </telerik:GridBoundColumn>
                                  <telerik:GridTemplateColumn HeaderText="紧急程度" HeaderStyle-HorizontalAlign="Center" UniqueName="Urgency_Degre">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxUrgencyDegre1" runat="server" Width="60px" AutoPostBack="true"
                                           Culture="zh-CN" DataSourceID="SqlDataSourceUrgencyDegre1" DataTextField="DICT_Name" DataValueField="DICT_Name">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn HeaderText="密级" HeaderStyle-HorizontalAlign="Center" UniqueName="Secret_Level" HeaderStyle-width="70px" ItemStyle-Width="70px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxSecretLevel1" runat="server" Width="60px" AutoPostBack="true"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceSecretLevel1" DataTextField="SecretLevel_Name" DataValueField="SecretLevel_Name">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn HeaderText="用途" HeaderStyle-HorizontalAlign="Center" UniqueName="Use_Des"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxUseDes1" runat="server" Width="70px" AutoPostBack="true"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceUseDes1" DataTextField="DICT_Name" DataValueField="DICT_Name">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 <telerik:GridTemplateColumn HeaderText="领料<br />部门" HeaderStyle-HorizontalAlign="Center" UniqueName="MaterialDept"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                       <ItemTemplate>
                                          <telerik:RadComboBox ID="RadComboBoxMaterialDept1" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                            OnSelectedIndexChanged="RadComboBoxMaterialDept_SelectedIndexChanged1" Width="70" Enabled="False">
                                        </telerik:RadComboBox>
                                       </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                  <telerik:GridTemplateColumn HeaderText="配送地址" HeaderStyle-HorizontalAlign="Center" UniqueName="Shipping_Address"
                                     HeaderStyle-width="110px" ItemStyle-Width="110px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxShippingAddress1" runat="server" Width="110px" Culture="zh-CN"
                                            AutoPostBack="true">
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="合格证" HeaderStyle-HorizontalAlign="Center" UniqueName="Certification"  HeaderStyle-width="60px" ItemStyle-Width="60px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxCertification1" runat="server" Width="60px" AutoPostBack="true" >
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y" />
                                                <telerik:RadComboBoxItem Text="N" Value="N" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn HeaderText="国产/进口" HeaderStyle-HorizontalAlign="Center" UniqueName="Attribute4"  HeaderStyle-width="60px" ItemStyle-Width="60px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxAttribute4" runat="server" Width="60px" AutoPostBack="true" >
                                            <Items>
                                                <telerik:RadComboBoxItem Text="国产" Value="国产" />
                                                <telerik:RadComboBoxItem Text="进口" Value="进口" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn HeaderText="研制<br />阶段" HeaderStyle-HorizontalAlign="Center" UniqueName="stage"  HeaderStyle-width="60px" ItemStyle-Width="60px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                          <telerik:RadComboBox ID="RadComboBoxStage1" runat="server" Width="60px" AutoPostBack="true"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceStage" DataTextField="Phase" DataValueField="Phase">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                  </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求数量(kg)" SortExpression="DemandNumSum" UniqueName="DemandNumSum" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Unit_Price" FilterControlAltText="Filter Unit_Price column" HeaderText="单价" SortExpression="Unit_Price" UniqueName="Unit_Price">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求时间" DataFormatString="{0:yyyy-MM-dd}" >
                                </telerik:GridBoundColumn>
                                </Columns>		
						</MasterTableView>
                    </telerik:RadGrid>
                     <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from V_Get_Sys_Dept_ShipAddrByDeptID"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceUrgencyDegre1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceSecretLevel1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceUseDes1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
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
                <div id="div_no_submit" style="font-size: 14px; text-align: center; margin-bottom: 20px;">
                    <div class="divViewPanel">
                        <telerik:RadGrid ID="RadGrid_TechnologyTestList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                            OnNeedDataSource="RadGrid_TechnologyTestList_NeedDataSource" OnItemCommand="RadGrid_TechnologyTestList_ItemCommand" PageSize="50">
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                                <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                   <CommandItemTemplate>
						   未提交申请
                                             </CommandItemTemplate>
                                 <Columns>
                                    <telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号" ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MDPId" FilterControlAltText="Filter MDPId column" Visible="false" SortExpression="MDPId" UniqueName="MDPId">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DRAWING_NO" FilterControlAltText="Filter DRAWING_NO column" HeaderStyle-HorizontalAlign="Center" HeaderText="产品图号" SortExpression="DRAWING_NO" UniqueName="DRAWING_NO">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="物资名称" SortExpression="Material_Name" UniqueName="Material_Name">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NumCasesSum" FilterControlAltText="Filter NumCasesSum column" HeaderText="需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量" UniqueName="DemandNumSum">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MAT_UNIT" FilterControlAltText="Filter MAT_UNIT column" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column" HeaderText="物资件数" SortExpression="Quantity" UniqueName="Quantity">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ROUGH_SIZE" FilterControlAltText="Filter ROUGH_SIZE column" HeaderText="物资尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Special_Needs" FilterControlAltText="Filter SpecialNeeds column" HeaderText="特殊需求" SortExpression="SpecialNeeds" UniqueName="SpecialNeeds">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="UrgencyDegre" FilterControlAltText="Filter UrgencyDegre column" HeaderText="紧急程度" SortExpression="UrgencyDegre" UniqueName="UrgencyDegre">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Secret_Level" FilterControlAltText="Filter SecretLevel column" HeaderText="密级" SortExpression="SecretLevel" UniqueName="SecretLevel">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="stage1" FilterControlAltText="Filter stage1 column" HeaderText="研制阶段" SortExpression="stage1" UniqueName="stage1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="UseDes" FilterControlAltText="Filter UseDes column" HeaderText="用途" SortExpression="UseDes" UniqueName="UseDes">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Shipping_Address" FilterControlAltText="Filter Shipping_Address column" HeaderText="配送地址" SortExpression="Shipping_Address" UniqueName="Shipping_Address">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Certification" FilterControlAltText="Filter Certification1 column" HeaderText="合格证" SortExpression="Certification1" UniqueName="Certification1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Unit_Price" FilterControlAltText="Filter Unit_Price column" HeaderText="单价" SortExpression="Unit_Price" UniqueName="Unit_Price">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Sum_Price" FilterControlAltText="Filter Sum_Price column" HeaderText="总价" SortExpression="Sum_Price" UniqueName="Sum_Price">
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
				            AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
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

