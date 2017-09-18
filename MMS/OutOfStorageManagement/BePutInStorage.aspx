<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="BePutInStorage.aspx.cs" Inherits="mms.OutOfStorageManagement.BePutInStorage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="库存管理-->物资入库" ClientIDMode="Static" />

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RTB_BarCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RTB_BarCode" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Add1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="table1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RTB_ItemCode1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="table1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function RTB_BarCodeValueChanged(sender, args) {
             
                if (args.get_newValue() == "") {
                    args.set_cancel(true);
                }
            }
            function RTB_BarCodeClick() {

                $find("<%=RTB_BarCode.ClientID %>").clear();
                
            }
            var buttonid ;
            function confirmWindow(sender, args) {
                var grid = $find("<%=RadGrid1.ClientID %>");
                var masterTableView = grid.get_masterTableView();
                var dataItems = masterTableView.get_dataItems()
                var length = dataItems.length;
                if (length == 0) {
                    $find("<%=RadNotificationAlert.ClientID%>").set_text("没有可入库的物资信息！");
                    $find("<%=RadNotificationAlert.ClientID%>").show();
                    args.set_cancel(true);
                } else {
                    $find("<%=confirmWindow.ClientID%>").show();
                    buttonid = sender.get_id();
                    args.set_cancel(true);
                }

                
            }
            function YesOrNoClicked(sender,args) {
                $find("<%=confirmWindow.ClientID%>").close();
                $find("<%=RadWindow1.ClientID%>").close();
                if (sender.get_text() == "是") {
                    $find(buttonid).click();
                }
            }
            function RadWindow1(sender, args) {
                $find("<%=RadWindow1.ClientID%>").show();
                buttonid = sender.get_id();
                args.set_cancel(true);
            }
            function reset(sender, args) {
                
                $find("<%=RTB_Material_Name.ClientID%>").clear();
                $find("<%=RTB_ItemCode1.ClientID%>").clear();
                $find("<%=RTB_Material_Type.ClientID%>").clear();
                $find("<%=RTB_Material_Mark.ClientID%>").clear();
                $find("<%=RTB_Material_State.ClientID%>").clear();
                $find("<%=RTB_Material_Tech_Condition.ClientID%>").clear();
                $find("<%=RTB_Rough_Spec.ClientID%>").clear();
                $find("<%=RTB_Rough_Size.ClientID%>").clear();
                $find("<%=RTB_Mat_Unit.ClientID%>").clear();
                $find("<%=RTB_Mat_Rough_Weight.ClientID%>").clear();
                $find("<%=RTB_Quantity1.ClientID%>").clear();
                $find("<%=RTB_BarCode1.ClientID%>").clear();
            }
        </script>
    </telerik:RadCodeBlock>
    <asp:HiddenField ID="HF_DeptID" runat="server" />
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true" Skin="Default">
        <Tabs>
            <telerik:RadTab Value="0" Text="扫码入库"></telerik:RadTab>
            <telerik:RadTab Value="1" Text="输入入库" Selected="true"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="RadPageView1" runat="server">
            <div style="width: 100%;">
                <div style="width: 100%; text-align:center;">
                    <span style="font-size:30px; font-weight:bold;">请扫描出库单条码：</span>
                    <telerik:RadTextBox ID="RTB_BarCode" runat="server" Width="600px" Height="60px"
                        Font-Size="40px" AutoPostBack="true" OnTextChanged="RTB_BarCode_TextChanged"
                        FocusedStyle-HorizontalAlign="Center" FocusedStyle-ForeColor="Black" EnabledStyle-HorizontalAlign="Center">
                        <ClientEvents OnFocus="RTB_BarCodeClick" OnValueChanged="RTB_BarCodeValueChanged" />
                    </telerik:RadTextBox>
                </div>
                <div style="width:100%;">
                    <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource"
                        OnItemCommand="RadGrid1_ItemCommand" >
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ClientSettings EnableRowHoverStyle="true" Selecting-AllowRowSelect="true" >
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" ScrollHeight="600px" UseStaticHeaders="true"></Scrolling>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="RowsId">
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" UniqueName="Material_Name"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" UniqueName="ItemCode1"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Type" HeaderText="材料类型" UniqueName="Material_Type"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Mark" HeaderText="物资牌号" UniqueName="Material_Mark"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_State" HeaderText="物资状态" UniqueName="Material_State"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Tech_Condition" HeaderText="技术条件" UniqueName="Material_Tech_Condition"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="坯料规格" UniqueName="Rough_Spec"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="物资尺寸" UniqueName="Rough_Size"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Mat_Unit" HeaderText="计量单位" UniqueName="Mat_Unit"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件质量" UniqueName="Mat_Rough_Weight"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件质量" UniqueName="Mat_Rough_Weight"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="入库数量">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="RTB_Quantity" runat="server" Width="100px" Text='<%#Eval("Quantity") %>'></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="BarCode" HeaderText="条码" UniqueName="BarCode"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <telerik:RadButton ID="RB_Del" runat="server" Text="删除" ButtonType="ToggleButton" CommandName="del" OnClientClicking="RadWindow1"></telerik:RadButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <CommandItemTemplate>
                                入库物资信息列表
                                <telerik:RadButton ID="RB_Add" runat="server" Text="入库" CssClass="floatright" OnClientClicking="confirmWindow" CommandName="add"></telerik:RadButton>
                            </CommandItemTemplate>
                        </MasterTableView>                        
                    </telerik:RadGrid>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server" Selected="true">
            <table id="table1" style="width:600px; margin:0px auto; font-size:14px; text-align:left; height:300px;">
                <tr>
                    <th colspan="4" style="letter-spacing:16px; font-size:16px; font-weight:bold;">入库登记表</th>
                </tr>
                <tr>
                    <td colspan="4" style="font-weight:bold; text-align:left;">物资信息：</td>
                </tr>
                <tr>
                    <td style="width:140px; text-align:right;">物资编码：</td>
                    <td style="width:200px;"><telerik:RadTextBox ID="RTB_ItemCode1" runat="server" Width="180px" AutoPostBack="true" OnTextChanged="RTB_ItemCode1_TextChanged"></telerik:RadTextBox></td>
                    <td style="width:100px; text-align:right;">物资名称：</td>
                    <td style="width:200px;"><telerik:RadTextBox ID="RTB_Material_Name" runat="server" Width="180px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td style="text-align:right;">材料类型：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Type" runat="server" Width="180px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">物资牌号：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Mark" runat="server" Width="180px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td style="text-align:right;">物资状态：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_State" runat="server" Width="180px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">技术条件：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Tech_Condition" runat="server" Width="180px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td style="text-align:right;">坯料规格：</td>
                    <td><telerik:RadTextBox ID="RTB_Rough_Spec" runat="server" Width="180px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">物资尺寸：</td>
                    <td><telerik:RadTextBox ID="RTB_Rough_Size" runat="server" Width="180px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td style="text-align:right;">计量单位：</td>
                    <td><telerik:RadTextBox ID="RTB_Mat_Unit" runat="server" Width="180px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">单件质量：</td>
                    <td><telerik:RadTextBox ID="RTB_Mat_Rough_Weight" runat="server" Width="180px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td colspan="4" style="font-weight:bold; text-align:left;">入库信息：</td>
                </tr>
                <tr>
                    <td style="text-align:right;">入库数量：</td>
                    <td><telerik:RadTextBox ID="RTB_Quantity1" runat="server" Width="180px"></telerik:RadTextBox></td>
                    <td style="text-align:right;">条码：</td>
                    <td><telerik:RadTextBox ID="RTB_BarCode1" runat="server" Width="180px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;"><telerik:RadButton ID="RB_Add1" runat="server" Text="入库" OnClientClicking="confirmWindow" OnClick="RB_Add1_Click"></telerik:RadButton></td>
                    <td colspan="2" style="text-align:center;"><telerik:RadButton ID="RB_Reset" runat="server" Text="重置" AutoPostBack="false" OnClientClicking="reset"></telerik:RadButton></td>
                </tr> 
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    
    <%-- 入库弹出窗口--开始--%>
    <telerik:RadWindow ID="confirmWindow" runat="server" VisibleTitlebar="false"
        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
        <ContentTemplate>
            <div style="margin-top: 30px; float: left;">
                <div style="width: 60px; padding-left: 15px; float: left;">
                    <img src="../Images/images/warnning1.jpg" alt="" />
                </div>
                <div style="width: 200px; float: left;">
                    <asp:Label ID="Label1" Font-Size="14px" Text="确定要入库吗？" runat="server" Font-Bold="true"
                        ForeColor="#25a0da" />
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="RadButton1" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                    &nbsp;
                        <telerik:RadButton ID="RadButton2" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </telerik:RadButton>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <%-- 入库弹出窗口--结束--%>
    <%-- 删除弹出窗口--开始--%>
    <telerik:RadWindow ID="RadWindow1" runat="server" VisibleTitlebar="false"
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
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
</asp:Content>
