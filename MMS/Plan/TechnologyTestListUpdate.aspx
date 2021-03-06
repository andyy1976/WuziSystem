﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TechnologyTestListUpdate.aspx.cs" Inherits="mms.Plan.TechnologyTestListUpdate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Styles/TechnologyTestAdd.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script src="../Scripts/PubFun.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="divContant">
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
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
                            oWindow.close();
                        }

                        function OnClientHidden(sender, args) {
                            CloseWindow();
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
                    </script>
                </telerik:RadCodeBlock>
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
                <telerik:RadSkinManager runat="server" Skin="Silk"></telerik:RadSkinManager>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_TechnologyTestList">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanel1" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
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
                        <telerik:AjaxSetting AjaxControlID="txt_ItemCode1">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RTB_MaterialName" />
                                <telerik:AjaxUpdatedControl ControlID="txt_Mat_Unit" />
                                <telerik:AjaxUpdatedControl ControlID="RTB_Dinge_Size" />
                                <telerik:AjaxUpdatedControl ControlID="RTB_Rough_Size" />
                                <telerik:AjaxUpdatedControl ControlID="txt_Rough_Spec" />
                                <telerik:AjaxUpdatedControl ControlID="RTB_Unit_Price" />
                                <telerik:AjaxUpdatedControl ControlID="txt_DemandNumSum" />
                                <telerik:AjaxUpdatedControl ControlID="span_Sum_Price" />
                                <telerik:AjaxUpdatedControl ControlID="lblMSG" />
                                <telerik:AjaxUpdatedControl ControlID="rfv_ItemCode1" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <%--<telerik:AjaxSetting AjaxControlID="RadBtnSave">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
                <asp:HiddenField ID="HiddenField" runat="server" Value="" ClientIDMode="Static" />
            </div>
            <div class="divContant">
                <div class="technology_Div_content">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="technology_Div_title"><b id="b_title" runat="server"></b></div>
                        <div>
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
                                            <telerik:RadDatePicker ID="span_apply_time" runat="server" Width="150px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                            </telerik:RadDatePicker>
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
                        </div>
                        <div>
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
                                        <telerik:RadDatePicker ID="RDP_DemandDate" runat="server" Width="150px" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </div>
                        <div>
                        <div class="technology_Div_smalltitle">
						<b>物资信息：（需求量=单件质量*需求件数）</b>
                        </div>
                        <div class="technology_Div_detailcontent">
                            <table border="0">
                                        <tr>
                                    <td class="technology_Div_detailcontent_title">物资编码：</td>
                                    <td class="technology_Div_detailcontent_content">
                                            <telerik:RadTextBox ID="txt_ItemCode1" Width="150" runat="server" ValidationGroup="2" Enabled="false"></telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">物资描述：</td>
                                    <td colspan="5" class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_MaterialsDes" runat="server" Width="800" MaxLength="200"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">物资名称：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_MaterialName" runat="server" Width="150" MaxLength="50"></telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="rfv_MaterialName" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="RTB_MaterialName" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">物资牌号：</td>
                                    <td class="technology_Div_detailcontent_content"><telerik:RadTextBox ID="RTB_Material_Mark" runat="server" Width="150" MaxLength="30"></telerik:RadTextBox></td>
                                    <td class="technology_Div_detailcontent_title">供应状态：</td>
                                    <td class="technology_Div_detailcontent_content"><telerik:RadTextBox ID="RTB_CN_Material_State" runat="server" Width="150" MaxLength="20"></telerik:RadTextBox></td>
                                    <td class="technology_Div_detailcontent_title">采用标准：</td>
                                    <td class="technology_Div_detailcontent_content"><telerik:RadTextBox ID="RTB_Material_Tech_Condition" runat="server" Width="150" MaxLength="50"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">坯料规格：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Rough_Spec" runat="server" MaxLength="30" Width="150"></telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">胚料尺寸：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Dinge_Size" runat="server" MaxLength="30" Width="150"></telerik:RadTextBox>
                                    </td>

                                    <td class="technology_Div_detailcontent_title">计量单位：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Mat_Unit" runat="server" MaxLength="10" Width="150"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Mat_Unit" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Mat_Unit" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">单件质量：</td>
                                    <td><telerik:RadTextBox ID="RTB_Mat_Rough_Weight" runat="server" MaxLength="10" Width="150" onpaste="return false" onkeyup='clearNoDecimal(this)'></telerik:RadTextBox></td>
                                 </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">单价(元）：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Unit_Price" runat="server" MaxLength="10" Width="150" EmptyMessage="0" onpaste="return false" onkeyup='clearNoDecimal(this)'></telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">需求尺寸：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Rough_Size" runat="server" Width="150" MaxLength="30"></telerik:RadTextBox>
                                    </td>

                          
                                    <td class="technology_Div_detailcontent_title">需求数量：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_DemandNumSum" Width="150" runat="server" MaxLength="10" onpaste="return false" onkeyup='clearNoDecimal(this)'></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_DemandNumSum" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_DemandNumSum" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title" style="width: 10%">需求件数：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_NumCasesSum" ClientIDMode="Static" Width="150" runat="server" MaxLength="10" onpaste="return false" onkeyup='clearNoDecimal(this)'></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_NumCasesSum" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_NumCasesSum" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">密级：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxSecretLevel" runat="server" AppendDataBoundItems="true"  Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceSecretLevel" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)"></asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">总价(元)：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="span_Sum_Price" runat="server" Width="150" EmptyMessage="0" MaxLength="10" onpaste="return false" onkeyup='clearNoDecimal(this)'></telerik:RadTextBox>
                           
                                    </td>
                                    <td class="technology_Div_detailcontent_title">研制阶段：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxStage" runat="server" Width="150">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">用途：</td>
                                    <td class="technology_Div_detailcontent_content">
                                            <telerik:RadDropDownList ID="RadComboBoxUseDes" runat="server" Width="150" AppendDataBoundItems="true">
                                            </telerik:RadDropDownList>
                                          <asp:SqlDataSource ID="SqlDataSourceUseDes" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                         SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE'"></asp:SqlDataSource>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">合格证：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxCertification" runat="server" Width="150">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y" />
                                                <telerik:RadComboBoxItem Text="N" Value="N" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>

                                         
                                    <td class="technology_Div_detailcontent_title">配送地址：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxShipping_Address" runat="server" Width="150"></telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">紧急程度：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxUrgencyDegre" runat="server" AppendDataBoundItems="true" Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceUrgencyDegre" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL' and ENABLED_FLAG = 'Y'"></asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">领料部门：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxMaterialDept" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                           Width="150" Enabled="False">
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
                                        <telerik:RadTextBox ID="rtb_SpecialNeeds" runat="server" Width="150" MaxLength="50" EmptyMessage="无"></telerik:RadTextBox>
                                        <%-- <asp:RequiredFieldValidator ID="rfv_SpecialNeeds" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="rtb_SpecialNeeds" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">变更原因：</td>
                                    <td class="technology_Div_detailcontent_content"><telerik:RadTextBox runat="server" ID="RTB_Reason" Width="150"></telerik:RadTextBox></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="technology_Div_button">

                            <div style="margin-top: 13px;">
                                <telerik:RadButton ID="RadBtnSave" runat="server" ValidationGroup="1" Text="保存更改内容" Width="120px" OnClick="RadBtnSave_Click"></telerik:RadButton>
                                <telerik:RadButton ID="RadBtnSubmit" runat="server" ValidationGroup="3" Text="提交流程平台" Width="120px" OnClick="RadBtnSubmit_Click" OnClientClicking="ShowApprove"></telerik:RadButton>
                                <asp:HiddenField ID="HF_DeptCode" runat="server" />
                                <asp:HiddenField ID="MDMLID" runat="server" />
                                <asp:HiddenField ID="MDPLID" runat="server" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
   

        <%-- 删除弹出窗口--开始--%>
     <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
            AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
        </telerik:RadNotification>
           <telerik:RadNotification ID="RadNotificationAlert1" runat="server" Text="" Position="Center" 
                AutoCloseDelay="2000" Width="300" Title="提示" OnClientHidden="OnClientHidden"  EnableRoundedCorners="true">
            </telerik:RadNotification>
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
                                        <td colspan="2" style="text-align:center;">
                                             <telerik:RadButton ID="RadButton5" runat="server" Text="提交" AutoPostBack="false" OnClientClicked="SubmitOrCancel">
                                                    <Icon PrimaryIconCssClass="rbOk" />
                                                </telerik:RadButton>
                                            &nbsp;
                                                
                                        </td>
                                        <td colspan="2" style="text-align:center;">
                                            <telerik:RadButton ID="RadButton6" runat="server" Text="取消" AutoPostBack="false" OnClientClicked="SubmitOrCancel">
                                                <Icon PrimaryIconCssClass="rbCancel" />
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </telerik:RadWindow>
                        <%--选择审批流程--结束----%>
   
    </form>
</body>
</html>
