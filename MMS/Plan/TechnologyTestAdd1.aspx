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
                    </div>
                </div>
            </div>

            <div class="divContant">
                <div class="technology_Div_content">
                    <div class="technology_Div_title"><b id="b_title" runat="server"></b></div>
                    <div>
                        <div class="technology_Div_smalltitle"><b>申请人信息</b></div>
                        <div class="technology_Div_detailcontent">
                            <table border="0" width="90%">
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
                                    <td class="technology_Div_detailcontent_content"><span id="span_apply_time" runat="server"></span></td>
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
                                    <td class="technology_Div_detailcontent_title">
                                        <asp:Label runat="server" ID="lblTaskSubject">任务号：</asp:Label>
                                    </td>
                                    <td class="technology_Div_detailcontent_content1">
                                        <telerik:RadTextBox ID="txt_TaskCode" Width="120" MaxLength="50" runat="server"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_TaskCode" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_TaskCode" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">产品图号：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Drawing_No" Width="120" MaxLength="50" runat="server"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Drawing_No" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Drawing_No" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">需求时间：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadDatePicker ID="DemandDate" runat="server" Width="100px"></telerik:RadDatePicker>
                                    </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div>
                        <div class="technology_Div_smalltitle"><b>物资信息：（需求量=单件质量*需求件数）</b></div>
                            <div class="technology_Div_detailcontent">
                                <table border="0">
                                    <tr>
                                        <td class="technology_Div_detailcontent_title">物资编码：</td>
                                    <td class="technology_Div_detailcontent_content">
               
                                        <table style="width:100%; margin:0px; padding:0px;">
                                            <tr>
                                        <td> <telerik:RadTextBox ID="txt_ItemCode1" Width="120" runat="server" ValidationGroup="2" AutoPostBack="True" OnTextChanged="txt_ItemCode1_OnTextChanged"></telerik:RadTextBox></td>
                                        <td> <telerik:RadButton ID="btn_ItemCodeOK" AutoPostBack="False" ValidationGroup="2" runat="server" Text="搜索"  ForeColor="Blue" OnClientClicking="ShowItemCode"></telerik:RadButton></td>
                                        <td> <asp:Label ID="lblMSG" runat="server" ForeColor="Red"></asp:Label></td>
                                         <td><asp:RequiredFieldValidator ID="rfv_ItemCode1" ValidationGroup="2" runat="server" ErrorMessage="*" ControlToValidate="txt_ItemCode1" ForeColor="Red"></asp:RequiredFieldValidator></td>
                                         </tr>
                                        </table>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">物资名称：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Material_Name" runat="server" Width="150" MaxLength="50"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_MaterialName" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="RTB_Material_Name" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">物资牌号：</td>
                                    <td><telerik:RadTextBox ID="RTB_Material_Mark" runat="server" Width="150" MaxLength="30"></telerik:RadTextBox></td>
                                    <td class="technology_Div_detailcontent_title">供应状态：</td>
                                    <td><telerik:RadTextBox ID="RTB_CN_Material_State" runat="server" Width="150" MaxLength="20"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td class="technology_Div_detailcontent_title">采用标准：</td>
                                    <td><telerik:RadTextBox ID="RTB_Material_Tech_Condition" runat="server" Width="150" MaxLength="50"></telerik:RadTextBox></td>
                                    <td class="technology_Div_detailcontent_title">坯料规格：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Rough_Spec" runat="server" MaxLength="30" Width="150"></telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">物资尺寸：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Rough_Size" runat="server" MaxLength="30" Width="150"></telerik:RadTextBox>
                                    </td>

                                    <td class="technology_Div_detailcontent_title">计量单位：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="txt_Mat_Unit" runat="server" MaxLength="10" Width="150"></telerik:RadTextBox>
                                        <asp:RequiredFieldValidator ID="rfv_Mat_Unit" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_Mat_Unit" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>


                                    
                                 </tr>
                                <tr>
                                    
                                    <td class="technology_Div_detailcontent_title">单件质量：</td>
                                    <td><telerik:RadTextBox ID="RTB_Mat_Rough_Weight" runat="server"></telerik:RadTextBox></td>
                                    <td class="technology_Div_detailcontent_title">单价(元）：</td>
                                        <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="RTB_Unit_Price" runat="server" Width="130" EmptyMessage="0" MaxLength="10" onpaste="return false" onkeyup='clearNoNum(this)'></telerik:RadTextBox>
                                        </td>
                                    <td class="technology_Div_detailcontent_title">需求量：</td>
                                        <td class="technology_Div_detailcontent_content">
                                            <telerik:RadTextBox ID="txt_DemandNumSum" Width="150" runat="server" MaxLength="5" onpaste="return false" onkeyup='clearNoNum(this)'></telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="rfv_DemandNumSum" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_DemandNumSum" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    <td class="technology_Div_detailcontent_title" style="width: 10%">需求件数：</td>
                                        <td class="technology_Div_detailcontent_content">
                                            <telerik:RadTextBox ID="txt_NumCasesSum" ClientIDMode="Static" Width="150" runat="server" MaxLength="5" onpaste="return false" onkeyup='clearNoNum(this)'></telerik:RadTextBox>
                                            <asp:RequiredFieldValidator ID="rfv_NumCasesSum" ValidationGroup="1" runat="server" ErrorMessage="*" ControlToValidate="txt_NumCasesSum" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="technology_Div_detailcontent_title">密级：</td>
                                        <td class="technology_Div_detailcontent_content">
                                            <telerik:RadComboBox ID="RadComboBoxSecretLevel" runat="server" Width="150">
                                            </telerik:RadComboBox>
                                        </td>
                                    <td class="technology_Div_detailcontent_title">总价（元）：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="span_Sum_Price" runat="server" Width="130" EmptyMessage="0" MaxLength="10" onpaste="return false" onkeyup='clearNoNum(this)'></telerik:RadTextBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">研制阶段：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxStage" runat="server" Width="150">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">用途：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxUseDes" runat="server" DataSourceID="SqlDataSourceUseDes"
                                            DataTextField="DICT_Name" DataValueField="DICT_CODE" Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceUseDes" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE' and ENABLED_FLAG = 'Y'"></asp:SqlDataSource>
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
                                        <telerik:RadComboBox ID="RadComboBoxUrgencyDegre" runat="server" DataSourceID="SqlDataSourceUrgencyDegre"
                                            DataTextField="DICT_Name" DataValueField="DICT_CODE" Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceUrgencyDegre" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL' and ENABLED_FLAG = 'Y'"></asp:SqlDataSource>
                                    </td>
                                    <td class="technology_Div_detailcontent_title">领料部门：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadComboBox ID="RadComboBoxMaterialDept" AutoPostBack="true" AppendDataBoundItems="true" runat="server"
                                            OnSelectedIndexChanged="RadComboBoxMaterialDept_SelectedIndexChanged" Width="150" Enabled="False">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr id="trAttribute4" runat="server" visible="false">
                                    <td class="technology_Div_detailcontent_title">国产/进口：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadButton ID="RB_Attribute41" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Attribute4" Text="国产" Checked="true" AutoPostBack="false"></telerik:RadButton>
                                        <telerik:RadButton ID="RB_Attribute42" runat="server" ButtonType="ToggleButton" ToggleType="Radio" GroupName="Attribute4" Text="进口" AutoPostBack="false"></telerik:RadButton>
                                    </td>

                                    <td class="technology_Div_detailcontent_title">特殊需求：</td>
                                    <td class="technology_Div_detailcontent_content">
                                        <telerik:RadTextBox ID="rtb_SpecialNeeds" runat="server" Width="150" MaxLength="20" EmptyMessage="无"></telerik:RadTextBox>
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

