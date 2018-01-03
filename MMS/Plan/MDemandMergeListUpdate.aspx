<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MDemandMergeListUpdate.aspx.cs" Inherits="mms.Plan.MDemandMergeListUpdate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" rel="stylesheet" href="/Styles/Plan.css" />
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script src="../Scripts/PubFun.js"></script>
    <style type="text/css">
        body {
            font-size:13px;
        }
         .table1 tr {
             height:40px;
         }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RB_SM">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_MANUFACTURER" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid_MANUFACTURER">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_MANUFACTURER" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
               <telerik:AjaxSetting AjaxControlID="RB_Update">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert1"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">

                function EnterKeyProcessing(sender, eventArgs) {
                    var c = eventArgs.get_keyCode();
                    if ((c == 13)) {
                        eventArgs.set_cancel(true);
                    }
                }

                function ShowMANUFACTURER() {
                    $find("<%=RadWindow1.ClientID %>").Show();
                }
                function CloseWindow1(args) {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    oWindow.close();
                }
                function CloseWindow(args) {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    var oArg = new Object();
                    oWindow.BrowserWindow.refreshGrid(args);
                    oWindow.close(oArg);
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
            </script>
        </telerik:RadCodeBlock>
        <div style="width:100%;">
            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                <table class="table1" style="margin:0px auto; text-align:left;">
                    <tr>
                        <th colspan="6" style="text-align:center; font-size:16px;">型号投产计划物资需求信息 </th>
                    </tr>
                    <tr>
                        <th colspan="6" style="text-align:left; border-bottom:1px #ccc solid;">物资信息</th>
                    </tr>
                    <tr>
                        <td style="text-align:right; width:100px;">需求行号：</td>
                        <td style="width:18px;"><asp:Label ID="lbl_ID" runat="server"></asp:Label>
                            <asp:HiddenField ID="hf_MDPLID" runat="server" />
                        </td>
                        <td style="width:100px; text-align:right;">任务号：</td>
                        <td style="width:180px;"><asp:Label ID="lbl_TaskCode" runat="server"></asp:Label></td>
                        <td style="width:100px; text-align:right;">图号：</td>
                        <td style="width:180px;"><asp:Label ID="lbl_DrawingNo" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">物资名称：</td>
                        <td><asp:Label ID="lbl_Material_Name" runat="server"></asp:Label></td>
                        <td style="text-align:right;">物资编码：</td>
                        <td><asp:Label ID="lbl_ItemCode1" runat="server"></asp:Label></td>
                        <td style="text-align:right;">领料部门：</td>
                        <td><asp:Label ID="lbl_MaterialDept" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">需求件数：</td>
                        <td>
                            <telerik:RadTextBox ID="lbl_NumCasesSum" runat="server" Width="160px" onpaste="return false" onkeyup='clearNoDecimal(this)'>
                                 <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox>
                        </td>
                       <td style="text-align:right;">计量单位：</td>
                       <td>
                            <telerik:RadTextBox ID="lbl_Mat_Unit" runat="server" Width="160px" >
                                 <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox>
                        </td>
                        <td style="text-align:right;">需求数量：</td>
                        <td>
                            <telerik:RadTextBox ID="lbl_DemandNumSum" runat="server" Width="160px" onpaste="return false" onkeyup='clearNoDecimal(this)'>
                                 <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">规格：</td>
                        <td><asp:Label ID="lbl_Rough_Spec" runat="server"></asp:Label></td>
                        <td style="text-align:right;">胚料尺寸：</td>
                        <td><asp:Label ID="lbl_Dinge_Size" runat="server"></asp:Label></td>
                        <td style="text-align:right;">单件质量：</td>
                         <td>
                             <telerik:RadTextBox ID="RTB_Mat_Rough_Weight" runat="server" Width="160px" onpaste="return false" onkeyup='clearNoDecimal(this)'>
                                 <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox>
                        </td>
                       
                    </tr>
                    <tr>
                        <th colspan="6" style="text-align:left; border-bottom:1px #ccc solid;">需求信息</th>
                    </tr>
                    <tr>
                        <td style="text-align:right;">特殊需求：</td>
                        <td><telerik:RadTextBox ID="RTB_Special_Needs" runat="server" Width="160px">
                                                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox></td>
                        <td style="text-align:right;">紧急程度：</td>
                        <td><telerik:RadDropDownList ID="RDDL_Urgency_Degre" runat="server" Width="160px" >
                            </telerik:RadDropDownList></td>
                        <td style="text-align:right;">密级：</td>
                        <td><telerik:RadDropDownList ID="RDDL_Secret_Level" runat="server" Width="160px"></telerik:RadDropDownList></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">用途：</td>
                        <td><telerik:RadDropDownList ID="RDDL_Use_Des" runat="server" Width="160px"></telerik:RadDropDownList></td>
                        <td style="text-align:right;">配送地址：</td>
                        <td><telerik:RadDropDownList ID="RDDL_Shipping_Address" runat="server" Width="160px"></telerik:RadDropDownList></td>
                        <td style="text-align:right;">开具合格证：</td>
                        <td>
                            <telerik:RadDropDownList ID="RDDL_Certification" runat="server" Width="160px">
                                <Items>
                                    <telerik:DropDownListItem Text="Y" Value="Y" />
                                    <telerik:DropDownListItem Text="N" Value="N" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;">需求时间：</td>
                        <td>
                            <telerik:RadDatePicker ID="RDP_DemandDate" runat="server" Width="160px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                            </telerik:RadDatePicker>
                        </td>
                        <td style="text-align:right;">需求尺寸：</td>
                        <td>
                            <telerik:RadTextBox ID="RTB_ROUGH_SIZE" runat="server" Width="160px">
                                                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
                         </telerik:RadTextBox>
                        </td>
                        <td style="text-align:right;">生产厂家：</td>
                        <td><telerik:RadTextBox ID="RTB_MANUFACTURER" runat="server" Width="160px">
                                                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox></td>
                        <td><telerik:RadButton ID="RB_Search" runat="server" Text="搜索" AutoPostBack="false" OnClientClicking="ShowMANUFACTURER"></telerik:RadButton></td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">变更原因：</td>
                        <td><telerik:RadTextBox runat="server" id="RTB_Reason">
                                                      <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td><telerik:RadButton ID="RB_Update" runat="server" Text="修改需求" OnClick="RB_Update_Click" OnClientClicking="confirmRadWindowDelete"></telerik:RadButton></td>
                        <td><telerik:RadButton ID="RB_Cancel" runat="server" Text="取消" AutoPostBack="false" OnClientClicking="CloseWindow1"></telerik:RadButton></td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
   
    <telerik:RadWindow ID="RadWindow1" runat="server" Title="生产厂家列表"  
        ReloadOnShow="false" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
        Modal="true" Behaviors="Close,Maximize,Minimize" Height="430px" Width="700px">
        <ContentTemplate>
            生产厂家名称：<telerik:RadTextBox ID="RTB_Seg4" runat="server" Width="160px">
                                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                   </telerik:RadTextBox>
            简称：<telerik:RadTextBox ID="RTB_Seg5" runat="server" Width="160px">
                                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
               </telerik:RadTextBox>
            <telerik:RadButton ID="RB_SM" runat="server" Text="搜索" OnClick="RB_SM_Click"></telerik:RadButton>
            <telerik:RadGrid ID="RadGrid_MANUFACTURER" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGrid_MANUFACTURER_NeedDataSource" Height="350px">
                 <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" ></Scrolling>
                </ClientSettings>
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="生产厂家名称" DataField="seg4"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="生产厂家简称" DataField="seg5"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </telerik:RadWindow>
    <%-- 删除弹出窗口--开始--%>
    <telerik:RadWindow ID="RadWindowDelete" runat="server" VisibleTitlebar="false"
        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
        <ContentTemplate>
            <div style="margin-top: 30px; float: left;">
                <div style="width: 60px; padding-left: 15px; float: left;">
                    <img src="../Images/images/warnning1.jpg" alt="" />
                </div>
                <div style="width: 200px; float: left;">
                    <asp:Label ID="Label2" Font-Size="14px" Text="确定要更改需求吗？" runat="server" Font-Bold="true"
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
        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
            AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
        </telerik:RadNotification>
      
           <telerik:RadNotification ID="RadNotificationAlert1" runat="server" Text="" Position="Center"
            AutoCloseDelay="4000" Width="300" Title="提示"  OnClientHidden="CloseWindow" EnableRoundedCorners="true">
        </telerik:RadNotification>
    </form>
</body>
</html>
