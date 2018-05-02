<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MaterialApplication.aspx.cs" Inherits="mms.MaterialApplicationCollar.MaterialApplication" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/Plan.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script src="../Scripts/PubFun.js"></script>
    <style type="text/css">
        .CommonSymbols {
            color: blue;
            cursor: pointer;
            padding: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资申请-->无需求物资申请" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RB_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PanelAppDetail" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridMDDLD">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    <telerik:AjaxUpdatedControl ControlID="RTB_TaskCode" />
                    <telerik:AjaxUpdatedControl ControlID="RTB_Drawing_No" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lbl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTB_Remark" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RDDLMT">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="div1" />
                    <telerik:AjaxUpdatedControl ControlID="div2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="RB_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadButton1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RTB_ItemCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    <telerik:AjaxUpdatedControl ControlID="Panel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Submit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
              <telerik:AjaxSetting AjaxControlID="RB_Save">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Clear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            $(document).ready(function ()
            {
                $(".CommonSymbols").click(function ()
                {
                    var a = $(this).text();
                    var remark = $("#<%=RTB_Remark.ClientID%>").text();
                    remark += a;
                    $("#<%=RTB_Remark.ClientID%>").text(remark);
                    $("#<%=RTB_Remark.ClientID%>").focus();
                });
            });

            function EnterKeyProcessing(sender, eventArgs) {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }

            function CloseWindow(args) {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                var oArg = new Object();
                oWindow.BrowserWindow.refreshGrid(args);
                oWindow.close(oArg);
            }
            var AppId;
            function confirmWindow(sender, args) {
                var win = $find("<%=confirmWindow.ClientID%>");
                AppId = sender.get_id();
                win.show();
                args.set_cancel(true);
            }
            function YesOrNoClicked(sender, args)
            {
                var win = $find("<%=confirmWindow.ClientID%>");
                win.close();
                if (sender.get_text() == "是") {
                    $find(AppId).click();
                }
            }
            function CommonSymbols(sender, args)
            {
                var rbremark = $find("<%=RTB_Remark.ClientID%>");
                var text = $find(sender.get_id())._text;
                rbremark._text = rbremark._text + text;
            }
            function ShowItemCode()
            {
                $find("<%=RadWindow1.ClientID %>").Show();
            }

            function confirmWindowChoose(sender, args)
            {
                var grid = $find('<%= RadGrid1.ClientID %>');
                    var masterTableView = grid.get_masterTableView();

                    var selectedItems = masterTableView.get_selectedItems();
                    if (selectedItems.length <= 0) 
					{
                        var rnal = $find("<%=RadNotificationAlert.ClientID %>");
                                rnal.set_text("请选择一条物资编码记录");
                                rnal.show();
                                args.set_cancel(true);
                    }
         }

         function confirmWindowCancel(sender, args)
         {

                        $find("<%=RadWindow1.ClientID %>").close();

         }
        </script>
    </telerik:RadCodeBlock>
    <div style="width: 100%;">
          <asp:Panel ID="Panel1" runat="server">
            <table id="table2" style="margin: 0px auto; text-align: left; font-size: 13px; border: solid 1px #ccc; padding: 10px;">
                <tr>
                    <th colspan="6" style="text-align: center;">
                        <asp:Label ID="lbltitle" runat="server" Font-Size="22px">无需求物料申请</asp:Label>
                    </th>
                </tr>
                <tr>
                    <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc; padding-top: 30px;">申请人信息
                        <span style="float: right;">申请部门：
                        <asp:HiddenField ID="HF_DeptCode" runat="server" />
                            <asp:Label ID="lbl_Dept" runat="server"></asp:Label></span>
                    </th>
                </tr>
                <tr>
                    <td style="width: 140px; text-align: right;">申请人：</td>
                    <td style="width: 160px;">
                        <telerik:RadTextBox ID="RTB_Applicant" runat="server" Enabled="false">
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                    <td style="text-align: right; width: 100px;">申请时间：</td>
                    <td style="width: 160px;">
                        <telerik:RadDatePicker ID="RDP_ApplicationTime" runat="server" DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                        </telerik:RadDatePicker>
                    </td>
                    <td style="text-align: right; width: 100px;">联系方式：</td>
                    <td style="width: 160px;">
                        <telerik:RadTextBox ID="RTB_ContactInformation" runat="server" >
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox></td>
                </tr>
                <tr>
                    <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc;">请领信息</th>
                </tr>
                <tr>
                    <td style="text-align: right;">任务号：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_TaskCode" runat="server">
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                    <td style="text-align: right;">图号：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_DrawingNo" runat="server">
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                    <td style="text-align: right;">申请件数：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Quantity" runat="server" onpaste="return false" onkeyup='clearNoDecimal(this)'>
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">领料方式：</td>
                    <td>
                        <telerik:RadDropDownList ID="RDDL_TheMaterialWay" runat="server" >
                            <Items>
                                <telerik:DropDownListItem Value="0" Text="单件领料" />
                                <telerik:DropDownListItem Value="1" Text="成组领料" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td style="text-align: right;">要求供料时间：</td>
                    <td>
                        <telerik:RadDatePicker ID="RDP_FeedingTime" runat="server"  DateInput-ClientEvents-OnKeyPress='EnterKeyProcessing'>
                        </telerik:RadDatePicker>
                    </td>
                    <td style="text-align: right;">申请数量：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_PleaseTakeQuality" runat="server" onpaste="return false" onkeyup='clearNoDecimal(this)' Enabled="true">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">需求尺寸：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Rough_Size" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                    <td style="text-align: right;">备注：</td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="RTB_Remark" runat="server" TextMode="MultiLine" Rows="3" Columns="70">
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadButton ID="RB_IsDispatch" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" Text="急件" AutoPostBack="false"></telerik:RadButton><br />
                        <telerik:RadButton ID="RB_IsConfirm" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" Text="确认调拨" Checked="true" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">常用符号：</td>
                    <td colspan="5">
                        <label id="lbl" class="CommonSymbols">Ⅰ</label><label class="CommonSymbols">Ⅱ</label><label class="CommonSymbols">Ⅲ</label><label class="CommonSymbols">Ⅳ</label><label class="CommonSymbols">Ⅴ</label>
                        <label class="CommonSymbols">Ⅵ</label><label class="CommonSymbols">Ⅶ</label><label class="CommonSymbols">Ⅷ</label><label class="CommonSymbols">Ⅸ</label><label class="CommonSymbols">Ⅹ</label>
                        <label class="CommonSymbols">Ⅺ</label><label class="CommonSymbols">Ⅻ</label><label class="CommonSymbols">Δ</label><label class="CommonSymbols">Π</label><label class="CommonSymbols">Σ</label>
                        <label class="CommonSymbols">Y</label><label class="CommonSymbols">Φ</label><label class="CommonSymbols">Ψ</label><label class="CommonSymbols">Ω</label><label class="CommonSymbols">α</label>
                        <label class="CommonSymbols">β</label><label class="CommonSymbols">γ</label><label class="CommonSymbols">δ</label><label class="CommonSymbols">ε</label><label class="CommonSymbols">ζ</label>
                        <label class="CommonSymbols">η</label><label class="CommonSymbols">θ</label><label class="CommonSymbols">ι</label><label class="CommonSymbols">κ</label><label class="CommonSymbols">λ</label><br />
                        <label class="CommonSymbols">μ</label><label class="CommonSymbols">ν</label><label class="CommonSymbols">ξ</label><label class="CommonSymbols">ο</label><label class="CommonSymbols">π</label>
                        <label class="CommonSymbols">ρ</label><label class="CommonSymbols">σ</label><label class="CommonSymbols">τ</label><label class="CommonSymbols">υ</label><label class="CommonSymbols">φ</label>
                        <label class="CommonSymbols">χ</label><label class="CommonSymbols">ψ</label><label class="CommonSymbols">ω</label><label class="CommonSymbols">￡</label><label class="CommonSymbols">￠</label>
                        <label class="CommonSymbols">￥</label><label class="CommonSymbols">∝</label><label class="CommonSymbols">∫</label><label class="CommonSymbols">∞</label><label class="CommonSymbols">∩</label>
                        <label class="CommonSymbols">∪</label><label class="CommonSymbols">≠</label><label class="CommonSymbols">÷</label><label class="CommonSymbols">±</label><label class="CommonSymbols">‰</label>
                        <label class="CommonSymbols">℃</label><label class="CommonSymbols">⊙</label>
                    </td>
                </tr>
                <tr>
                    <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc;">物资信息</th>
                </tr>
                <tr>
                    <td style="text-align: right;">物资编码：</td>
                    <td>
                        <table style="width:100%; margin:0px; padding:0px;">
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="RTB_ItemCode" runat="server" Width="110px" OnTextChanged="RTB_ItemCode_TextChanged" AutoPostBack="true">
                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                    </telerik:RadTextBox>
                                </td>
                                <td><asp:Label ID="lblMSG" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label></td>
                                <td><telerik:RadButton ID="RB_Search" runat="server" Text ="搜索" AutoPostBack="false" OnClientClicking="ShowItemCode" ButtonType="ToggleButton" ForeColor="Blue"></telerik:RadButton></td>
                            </tr>
                        </table>
                    
                    
                    
                    </td>

                    <td style="text-align: right;">物资名称：</td>
                    <td> 
                        <telerik:RadTextBox ID="RTB_Material_Name" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                         </telerik:RadTextBox></td>
                    <td style="text-align: right;">物资牌号：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Material_Mark" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">供应状态：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_CN_Material_State" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                    <td style="text-align: right;">采用标准：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Material_Tech_Condition" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>

                    <td style="text-align: right;">胚料规格：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Rough_Spec" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">胚料尺寸：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Dinge_Size" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>

                    <td style="text-align: right;">计量单位：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Mat_Unit" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                  
                    <td style="text-align: right;">单件定额质量：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Mat_Rough_Weight" runat="server">
                          <ClientEvents OnKeyPress="EnterKeyProcessing" />
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">物资描述：</td>
                     <td colspan="5" >
                           <telerik:RadTextBox ID="RTB_MaterialsDes" runat="server" Width="750" MaxLength="200">
                                 <ClientEvents OnKeyPress="EnterKeyProcessing" />
                           </telerik:RadTextBox>
                     </td>
  
                </tr>
                        <tr>
                   <td  style="text-align: right;">密级：</td>
                           <td>
                                        <telerik:RadComboBox ID="RadComboBoxSecretLevel" runat="server" DataSourceID="SqlDataSourceSecretLevel"
                                            DataTextField="SecretLevel_Name" DataValueField="SecretLevel_Name" Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceSecretLevel" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)"></asp:SqlDataSource>
                           </td>
                           <td  style="text-align: right;">用途：</td>
                           <td >
                                        <telerik:RadComboBox ID="RadComboBoxUseDes" runat="server" DataSourceID="SqlDataSourceUseDes"
                                            DataTextField="DICT_Name" DataValueField="DICT_CODE" Width="150">
                                        </telerik:RadComboBox>
                                        <asp:SqlDataSource ID="SqlDataSourceUseDes" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                                            SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE' and ENABLED_FLAG = 'Y'"></asp:SqlDataSource>
                          </td>
                          <td  style="text-align: right;">是否开证：</td>
                          <td >
                                        <telerik:RadComboBox ID="RadComboBoxIsApply" runat="server" Width="150">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y" />
                                                <telerik:RadComboBoxItem Text="N" Value="N" />
                                            </Items>
                                        </telerik:RadComboBox>
                          </td>                                   
                </tr>
                <tr>
                    <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc;">业务审批流程</th>
                </tr>
                <tr>
                    <td style="text-align: right;">1.车间调度员：</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="RDDL_DiaoDu" ></telerik:RadDropDownList>
                    </td>
                    <td style="text-align: right;">2.型号计划员：</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="RDDL_XingHao" ></telerik:RadDropDownList>
                    </td>
                    <td style="text-align: right;">3.物资计划员：</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="RDDL_WuZi" ></telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center;">
                        <table style="text-align: center; margin: 0px auto; width: 300px;">
                            <tr>
                                <td style="width: 100px;">
                                    <telerik:RadButton ID="RB_Save" runat="server" Text="保存" OnClick="RB_Save_Click" ></telerik:RadButton>
                                </td>
                                <td style="width: 100px;">
                                    <telerik:RadButton ID="RB_Submit" runat="server" Text="提交" OnClick="RB_Submit_Click" OnClientClicking="confirmWindow"></telerik:RadButton>
                                </td>
                                <td style="width: 100px;">
                                    <telerik:RadButton ID="RB_Clear" runat="server" Text="清空" OnClick="RB_Clear_Click"></telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <telerik:RadWindow ID="RadWindow1" runat="server" Title="物资编码"  
        ReloadOnShow="false" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
        Modal="true" Behaviors="Close,Maximize,Minimize" Height="500px" Width="1200px">
        <ContentTemplate>
            <div style="width: 100%;">
                <table id="tableMT" style="margin:0px auto; width: 100%;">
                    <tr>
                        <th colspan="10" style="font-size:16px; letter-spacing:16px;">物资基础库查询</th>
                    </tr>
                    <tr>
                        <td style="width:50px;">物资名称：</td>
                        <td>
                            <telerik:RadTextBox ID="RTB_MaterialName" runat="server" Width="100px">
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox></td>
                        <td style="width:70px;">物资牌号：</td>
                        <td>
                            <telerik:RadTextBox ID="RTB_Material_Paihao" runat="server" Width="100px">
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox></td>

                        <td style="width:70px;">物资规格:</td>
                        <td>
                            <telerik:RadTextBox ID="RTB_Material_Guige" runat="server" Width="100px">
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox>
                        </td>

                        <td style="width:70px;">物资标准：</td>
                        <td>
                            <telerik:RadTextBox ID="RTB_Material_Biaozhun" runat="server" Width="100px">
                              <ClientEvents OnKeyPress="EnterKeyProcessing" />
                            </telerik:RadTextBox>
                        </td>

                              <td style="width:180px;" >
                                  <telerik:RadButton ID="RadButton1" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton>
                                 <telerik:RadButton ID="RadBtnChoose" runat="server" Text="选择该物资编码" CssClass="btn_margin1" Font-Bold="true" 
                                                    CommandName="RadBtnChoose" CausesValidation="true" OnClick="confirmWindowClick" OnClientClicking="confirmWindowChoose" >
                                                </telerik:RadButton>

                                       <%--         <telerik:RadButton ID="RadBtnCancel" runat="server" Text="取消并退出搜素框" CssClass="btn_margin1" Font-Bold="true" 
                                                    CommandName="CancelCombine"  OnClientClicking="confirmWindowCancel">
                                                </telerik:RadButton>--%> 
                                  </td>
                         </tr>
                         <tr>
                        <td style="width:100px;">物资类别：</td>
                        <td style="width:100px;">
                            <telerik:RadDropDownList ID="RDDLMT" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="RDDLMT_SelectedIndexChanged" AutoPostBack="true" Width="140px">
                                <Items>
                                    <telerik:DropDownListItem Value="" Text="" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%;">
                <telerik:RadGrid ID="RadGrid1" runat="server" Width="1160px" OnNeedDataSource="RadGrid1_NeedDataSource" AutoGenerateColumns="false" AllowPaging="true" 
                    PageSize="15" AllowMultiRowSelection="False">
                    <PagerStyle AlwaysVisible="true" />
                    <ClientSettings Selecting-AllowRowSelect="true">
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                    </ClientSettings>
                    <MasterTableView DataKeyNames="SEG3">
                        <Columns>
                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                                </telerik:GridClientSelectColumn>
                            <telerik:GridBoundColumn DataField="SEG3" HeaderText="物资编码" ItemStyle-Width="120px" HeaderStyle-Width="120px"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SEG4" HeaderText="描述" ItemStyle-Width="800px" HeaderStyle-width="800px"></telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                 <div style="margin:20px 0 5px 4px;">
                                       
                                              </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <%-- 申请弹出窗口--开始--%>
    <telerik:RadWindow ID="confirmWindow" runat="server" VisibleTitlebar="false"
        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
        <ContentTemplate>
            <div style="margin-top: 30px; float: left;">
                <div style="width: 60px; padding-left: 15px; float: left;">
                    <img src="../Images/images/warnning1.jpg" alt="" />
                </div>
                <div style="width: 200px; float: left;">
                    <asp:Label ID="Label2" Font-Size="14px" Text="确定要申请吗？" runat="server" Font-Bold="true"
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
    <%-- 删除弹出窗口--结束--%>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
       AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true" LoadContentOn="EveryShow">
    </telerik:RadNotification>
</asp:Content>
