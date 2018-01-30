<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="AuxiliaryMaterialAdd.aspx.cs" Inherits="mms.Plan.AuxiliaryMaterialAdd" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script src="../Scripts/PubFun.js"></script>
    <link href="../Styles/TechnologyTestAdd.css" rel="stylesheet" />
    <script>
        function closeWindow(sender, args) {
            var oWindow = GetRadWindow();
            oWindow.close(null);
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
              var uprice = $("#span_Unit_Price").text();
              var NumCasesSum = $("#txt_NumCasesSum").val();
              var numprice = 0;
              if (uprice != "" && NumCasesSum != "") {
                uprice = parseFloat(uprice);
                NumCasesSum = parseFloat(NumCasesSum);
                numprice = uprice * NumCasesSum;
              }
              $("#span_Sum_Price").html(numprice);
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; float: left;">
            <div class="divContant">
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
                        var deleteButtonID;
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
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_TechnologyTestList">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyTestList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server"></telerik:RadAjaxLoadingPanel>
                <asp:HiddenField ID="HiddenField1" runat="server" Value="天津公司车间物资申请" ClientIDMode="Static" />
            </div>
            <div class="divContant">
                <div class="technology_Div_content">
                    <div class="technology_Div_title"><b>物资领用申请</b></div>
                    <div>
                        <div class="technology_Div_smalltitle"><b>申请人信息</b></div>
                        <div class="technology_Div_detailcontent">
                            <table border="0">
                                <tr>
                                    <td class="technology_Div_detailcontent_title">部门：</td>
                                    <td class="technology_Div_detailcontent_content">
                                      <telerik:RadComboBox ID="RadComboBox_Dept" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                          OnSelectedIndexChanged="RadComboBox_Dept_SelectedIndexChanged">
                                            <Items>
                                              <telerik:RadComboBoxItem Text="请选择" Value="0" Selected="True" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">申请人：</td>
                                    <td class="technology_Div_detailcontent_content">
                                      <telerik:RadComboBox ID="RadComboBox_User" runat="server"></telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">申请时间：</td>
                                    <td class="technology_Div_detailcontent_content"><span id="span_apply_time" runat="server"></span></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div>
                        <div class="technology_Div_smalltitle"><b>请领信息</b></div>
                        <div class="technology_Div_detailcontent">
                            <table border="0">
                                <tr>
                                    <td class="technology_Div_detailcontent_title">任务号：</td>
                                    <td class="technology_Div_detailcontent_content1">
                                        <asp:TextBox ID="txt_TaskCode" Width="120" MaxLength="50" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_TaskCode" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_TaskCode" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <input type="button" id="btnNewTCode" value="新任务号" />
                                    </td>
                                    <td class="technology_Div_detailcontent_title">产品图号：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <asp:TextBox ID="txt_Drawing_No" Width="120" MaxLength="50" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Drawing_No" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Drawing_No" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">物资编码：</td>
                                    <td class="technology_Div_detailcontent_content1">
                                        <asp:TextBox ID="txt_ItemCode1" Width="120" runat="server"></asp:TextBox>
                                        <asp:Button ID="btn_ItemCodeOK" runat="server" Text="搜索" OnClick="btn_ItemCodeOK_Click" />
                                        <asp:RequiredFieldValidator ID="rfv_ItemCode1" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_ItemCode1" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">需求时间：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadDatePicker ID="DemandDate" runat="server" Width="100px"></telerik:RadDatePicker></td>
                                    <%--<td style="width: 40%"></td>--%>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div>
                        <div class="technology_Div_smalltitle"><b>物资信息</b></div>
                        <div class="technology_Div_detailcontent">
                            <table border="0">
                                <tr>
                                    <td class="technology_Div_detailcontent_title">共计需求件数：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_NumCasesSum" Width="150" ClientIDMode="Static" runat="server" MaxLength="5" onpaste="return false" onkeyup='clearNoNum(this)'></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_NumCasesSum" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_NumCasesSum" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">共计需求量：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_DemandNumSum" Width="150" runat="server" MaxLength="5" onpaste="return false" onkeyup='clearNoNum(this)'></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_DemandNumSum" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_DemandNumSum" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">计量单位：</td>
                                    <td class="technology_Div_detailcontent_content">
                                      <asp:TextBox ID="txt_Mat_Unit" runat="server" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Mat_Unit" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Mat_Unit" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title"></td>
                                    <td class="technology_Div_detailcontent_content">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">物资尺寸：</td>
                                    <td class="technology_Div_detailcontent_content">
                                      <asp:TextBox ID="txt_Rough_Size" runat="server" MaxLength="30"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Rough_Size" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Rough_Size" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">坯料规格：</td>
                                    <td class="technology_Div_detailcontent_content">
                                      <asp:TextBox ID="txt_Rough_Spec" runat="server" MaxLength="30"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Rough_Spec" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Rough_Spec" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">单价：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <span id="span_Unit_Price" runat="server" ClientIDMode="Static"></span>元
                                    </td>
                                    <td class="technology_Div_detailcontent_title">总价：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <span id="span_Sum_Price" runat="server" ClientIDMode="Static"></span>元
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">密级：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxSecretLevel" runat="server" DataSourceID="SqlDataSourceSecretLevel" 
                                          DataTextField="SecretLevel_Name" DataValueField="Id">
                                            <Items>
                                                <%--<telerik:RadComboBoxItem Text="非密" Value="0" />
                                                <telerik:RadComboBoxItem Text="内部" Value="1" Checked="true" />
                                                <telerik:RadComboBoxItem Text="秘密" Value="2" />
                                                <telerik:RadComboBoxItem Text="机密" Value="3" />--%>
                                            </Items>
                                        </telerik:RadComboBox>
                                      <asp:SqlDataSource ID="SqlDataSourceSecretLevel" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>' SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)">
                                      </asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">研制阶段：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxStage" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="M" Value="1" Checked="true" />
                                                <telerik:RadComboBoxItem Text="C" Value="2" />
                                                <telerik:RadComboBoxItem Text="S" Value="3" />
                                                <telerik:RadComboBoxItem Text="D" Value="4" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">用途：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxUseDes" runat="server" DataSourceID="SqlDataSourceUseDes" 
                                          DataTextField="UseDes_Name" DataValueField="Id">
                                            <Items>
                                                <%--<telerik:RadComboBoxItem Text="箭上" Value="0" />
                                                <telerik:RadComboBoxItem Text="其他" Value="1" />--%>
                                            </Items>
                                        </telerik:RadComboBox>
                                      <asp:SqlDataSource ID="SqlDataSourceUseDes" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>' SelectCommand="SELECT * FROM [Sys_UseDes] WHERE ([Is_Del] = 0)">
                                      </asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">合格证：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxCertification" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="是" Value="1" />
                                                <telerik:RadComboBoxItem Text="否" Value="0" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">特殊需求：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxSpecialNeeds" runat="server" DataSourceID="SqlDataSpecialNeeds"
                                           DataTextField="SpecialNeeds_Name" DataValueField="Id">
                                            <Items>
                                                <%--<telerik:RadComboBoxItem Text="有" Value="1" />
                                                <telerik:RadComboBoxItem Text="无" Value="0" />--%>
                                            </Items>
                                        </telerik:RadComboBox>
                                      <asp:SqlDataSource ID="SqlDataSpecialNeeds" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>' SelectCommand="SELECT * FROM [Sys_SpecialNeeds] WHERE ([Is_Del] = 0)">
                                      </asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">紧急程度：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxUrgencyDegre" runat="server" DataSourceID="SqlDataSourceUrgencyDegre"
                                           DataTextField="UrgencyDegre_Name" DataValueField="Id">
                                            <Items>
                                                <%--<telerik:RadComboBoxItem Text="正常" Value="0" />
                                                <telerik:RadComboBoxItem Text="加急" Value="1" />--%>
                                            </Items>
                                        </telerik:RadComboBox>
                                      <asp:SqlDataSource ID="SqlDataSourceUrgencyDegre" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>' SelectCommand="SELECT * FROM [Sys_UrgencyDegre] WHERE ([Is_Del] = 0)">
                                      </asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">领料部门：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <span id="span_MaterialDept" runat="server"></span>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">配送地址：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <span id="span_Shipping_Address" runat="server"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="technology_Div_button">
                        <div style="margin-top:13px;">
                            <telerik:RadButton ID="RadBtnSave" runat="server" ValidationGroup="1" Text="保 存" Width="120px" OnClick="RadBtnSave_Click"></telerik:RadButton>
                            <asp:HiddenField ID="hfBh" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="divContant">
                <div style="width: 100%; text-align: center;margin-top:20px;margin-bottom:20px;">
                    <b style="font-size: 22px;">未提交申请</b>
                </div>
                <div id="div_no_submit" style="font-size: 14px; text-align: center; margin-bottom:20px;">
                    <div class="divViewPanel">
                    

                        <telerik:RadGrid ID="RadGrid_AuxiliaryMaterialList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                            OnNeedDataSource="RadGrid_AuxiliaryMaterialList_NeedDataSource" OnItemCommand="RadGrid_AuxiliaryMaterialList_ItemCommand" PageSize="50">
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                                <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="rownum" DataType="System.Int32" FilterControlAltText="Filter rownum column" HeaderText="序号" ReadOnly="True" SortExpression="rownum" UniqueName="rownum">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MDP_Code" FilterControlAltText="Filter MDP_Code column" HeaderStyle-HorizontalAlign="Center" HeaderText="编号" SortExpression="MDP_Code" UniqueName="MDP_Code">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MDPId" FilterControlAltText="Filter MDPId column" Visible="false" SortExpression="MDPId" UniqueName="MDPId">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DRAWING_NO" FilterControlAltText="Filter DRAWING_NO column" HeaderStyle-HorizontalAlign="Center" HeaderText="产品图号" SortExpression="DRAWING_NO" UniqueName="DRAWING_NO">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NumCasesSum" FilterControlAltText="Filter NumCasesSum column" HeaderText="共计需求量" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MAT_UNIT" FilterControlAltText="Filter MAT_UNIT column" HeaderText="计量单位" SortExpression="MAT_UNIT" UniqueName="MAT_UNIT">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column" HeaderText="物资件数" SortExpression="Quantity" UniqueName="Quantity">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ROUGH_SIZE" FilterControlAltText="Filter ROUGH_SIZE column" HeaderText="物资尺寸" SortExpression="ROUGH_SIZE" UniqueName="ROUGH_SIZE">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SpecialNeeds" FilterControlAltText="Filter SpecialNeeds column" HeaderText="特殊需求" SortExpression="SpecialNeeds" UniqueName="SpecialNeeds">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="UrgencyDegre" FilterControlAltText="Filter UrgencyDegre column" HeaderText="紧急程度" SortExpression="UrgencyDegre" UniqueName="UrgencyDegre">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SecretLevel" FilterControlAltText="Filter SecretLevel column" HeaderText="密级" SortExpression="SecretLevel" UniqueName="SecretLevel">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="stage1" FilterControlAltText="Filter stage1 column" HeaderText="研制阶段" SortExpression="stage1" UniqueName="stage1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="UseDes" FilterControlAltText="Filter UseDes column" HeaderText="用途" SortExpression="UseDes" UniqueName="UseDes">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="KeyWord" FilterControlAltText="Filter KeyWord column" HeaderText="配送地址" SortExpression="KeyWord" UniqueName="KeyWord">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Certification1" FilterControlAltText="Filter Certification1 column" HeaderText="合格证" SortExpression="Certification1" UniqueName="Certification1">
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
                        <telerik:RadButton ID="RadBtnSubmit" runat="server" ValidationGroup="2" Text="提 交" Width="120px" OnClick="RadBtnSubmit_Click"></telerik:RadButton>
                        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                            AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true"  >
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
        </div>
    </form>
</body>
</html>
