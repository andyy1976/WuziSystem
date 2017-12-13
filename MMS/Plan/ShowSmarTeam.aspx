<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="ShowSmarTeam.aspx.cs" Inherits="mms.Plan.ShouSmarTeam" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
    <script type="text/javascript" src="../Scripts/jquery-1.7.2.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->SmartTeam列表" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RB_SynchronAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTL_BOM"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RTL_Defect" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RTL_SubmitState"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label10"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label11"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label12"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label14"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label15"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label16"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label24"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label25"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label26"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label30"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label31"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label32"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="Label1All"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Expand">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTL_BOM" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RTL_Defect" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RTL_SubmitState" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_ExpandBOM">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTL_BOM" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_ExpandDefect">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTL_Defect" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_ExpandSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTL_SubmitState" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Search_BOM">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTL_BOM" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Search_Defect">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RTL_Defect" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">     
            function EnterKeyProcessing(sender, eventArgs) {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div style="width: 100%; margin: 0px auto;">
        <div style="width: 100%; float: left; padding-bottom: 10px;">
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true" Skin="Default">
                <Tabs>
                    <telerik:RadTab Value="0" Text="BOM总表" Selected="true"></telerik:RadTab>
                    <telerik:RadTab Value="1" Text="缺定额和不规范"></telerik:RadTab>
                    <telerik:RadTab Value="2" Text="提交状态"></telerik:RadTab>
                    <telerik:RadTab Value="3" Text="物资需求变更"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
        </div>
        <div style="width:100%; font-weight:bold; padding-bottom:10px;">
            型号：<asp:Label ID="lblModel" runat="server" Font-Bold="true" Font-color="red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            计划包名称：<asp:Label ID="lblPlanName" runat="server" Font-Bold="false"></asp:Label>
        </div>
        <div style="width: 100%; float: left;">
            <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
                <telerik:RadPageView runat="server" Selected="true">
                    <div style="width: 100%;">
                        <table id="table1">
                            <tr>
                                <td>需提交物资请求：</td>
                                <td>
                                    <asp:Label ID="Label1All" runat="server"></asp:Label></td>
                                <td>，</td>
                                <td>（已提交：</td>
                                <td>
                                    <asp:Label ID="Label11" runat="server"></asp:Label></td>
                                <td>，</td>
                                <td>未提交：</td>
                                <td>
                                    <asp:Label ID="Label10" runat="server"></asp:Label></td>
                                <td>，</td>
                                <td>缺定额：</td>
                                <td>
                                    <asp:Label ID="Label14" runat="server"></asp:Label></td>
                                <td>，</td>
                                <td>不规范：</td>
                                <td>
                                    <asp:Label ID="Label15" runat="server"></asp:Label></td>
                                <td>，</td>
                                <td>有更改可再提交：</td>
                                <td>
                                    <asp:Label ID="Label12" runat="server"></asp:Label></td>
                                <td>，</td>
                                <td>有更改不可再提交：</td>
                                <td>
                                    <asp:Label ID="Label16" runat="server"></asp:Label></td>
                                <td>）</td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%;">
                        <table>
                            <tr>
                                <td>产品名称：</td>
                                <td>
                                    <telerik:RadTextBox ID="RTB_TDM_Description_BOM" runat="server" Width="100px">
                                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                    </telerik:RadTextBox></td>
                                <td>零件类型：</td>
                                <td>
                                    <telerik:RadDropDownList ID="RDDL_LingJian_Type_BOM" runat="server" Width="100px" AppendDataBoundItems="true">
                                        <Items>
                                            <telerik:DropDownListItem Text="全部" Value="" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>
                                <td>图号：</td>
                                <td>
                                    <telerik:RadTextBox ID="RTB_Drawing_No_BOM" runat="server" Width="100px">
                                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                    </telerik:RadTextBox></td>
                                <td>领料部门：</td>
                                <td>
                                    <telerik:RadTextBox ID="RTB_MaterialDept_BOM" runat="server" Width="100px">
                                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                    </telerik:RadTextBox></td>
                                <td>
                                    <telerik:RadButton ID="RB_Search_BOM" runat="server" Text="搜索" OnClick="RB_Search_BOM_Click"></telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%;">
                        <telerik:RadTreeList ID="RTL_BOM" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                            ParentDataKeyNames="ParentId" OnNeedDataSource="RTL_BOM_NeedDataSource" AllowSorting="true" AllowMultiItemSelection="true">
                            <ClientSettings Selecting-AllowItemSelection="true">
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="600px" />
                            </ClientSettings>
                            <AlternatingItemStyle HorizontalAlign="Left" Font-Size="12px" />
                            <ItemStyle HorizontalAlign="Left" Font-Size="12px" />
                            <HeaderStyle HorizontalAlign="Left" Font-Size="13px" />
                            <Columns>
                                <telerik:TreeListTemplateColumn ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Code" runat="server" Text='<%#Eval("Material_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <telerik:RadButton ID="RB_ExpandBOM" runat="server" Text="+" OnClick="RB_Expand_Click"></telerik:RadButton>
                                        编号
                                    </HeaderTemplate>
                                </telerik:TreeListTemplateColumn>
                               <%-- <telerik:TreeListBoundColumn DataField="Material_Code" HeaderText="编号" SortExpression="Material_Code"></telerik:TreeListBoundColumn>  --%>
                                <telerik:TreeListBoundColumn DataField="TDM_Description" HeaderText="产品名称"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="LingJian_Type1" HeaderText="零件类型"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Material_Name" HeaderText="物资名称"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Drawing_No" HeaderText="图号" SortExpression="Drawing_No"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Technics_Line" HeaderText="工艺路线"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="MaterialDept" HeaderText="领料部门"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="ItemCode1" HeaderText="物资编码"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Material_Mark" HeaderText="物资牌号"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Quantity" HeaderText="物资件数"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Unit" HeaderText="计量单位"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件质量"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Pro_Weight" HeaderText="每产品<br />质量"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Dinge_Size" HeaderText="胚料尺寸"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Rough_Spec" HeaderText="物资规格"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Rough_Size" HeaderText="需求尺寸"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="DemandNumSum" HeaderText="需求数量(kg)" ItemStyle-HorizontalAlign="Center"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="NumCasesSum" HeaderText="需求件数" ItemStyle-HorizontalAlign="Center"></telerik:TreeListBoundColumn>
                            </Columns>
                        </telerik:RadTreeList>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server">
                    <div style="width: 100%; float: left; text-align: left;">
                        <div style="width: 80%; float: left;">
                            <table>
                                <tr>
                                    <td>缺定额：</td>
                                    <td>
                                        <asp:Label ID="Label24" runat="server"></asp:Label></td>
                                    <td>，</td>
                                    <td>不规范：</td>
                                    <td>
                                        <asp:Label ID="Label25" runat="server"></asp:Label></td>
                                    <td>，</td>
                                    <td>有更改不可再提交：</td>
                                    <td>
                                        <asp:Label ID="Label26" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                        <%--<div style="width:20%; float:left;">
                            <div style="width:100px; float:right;">
                                <telerik:RadButton ID="RB_SynchronAll" runat="server" Text="全部重新同步" OnClientClicking="confirmWindowSynchron" OnClick="RB_SynchronAll_Click" CssClass="floatright" ></telerik:RadButton>
                            </div>
                        </div>--%>
                    </div>
                    <div style="width: 100%;">
                        <table>
                            <tr>
                                <td>产品名称：</td>
                                <td>
                                    <telerik:RadTextBox ID="RTB_TDM_Description_Defect" runat="server" Width="100px">
                                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                    </telerik:RadTextBox></td>
                                <td>零件类型：</td>
                                <td>
                                    <telerik:RadDropDownList ID="RDDL_LingJian_Type_Defect" runat="server" Width="100px" AppendDataBoundItems="true">
                                        <Items>
                                            <telerik:DropDownListItem Text="全部" Value="" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>
                                <td>图号：</td>
                                <td>
                                    <telerik:RadTextBox ID="RTB_Drawing_No_Defect" runat="server" Width="100px">
                                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                    </telerik:RadTextBox></td>
                                <td>
                                    <telerik:RadButton ID="RB_Search_Defect" runat="server" Text="搜索" OnClick="RB_Search_Defect_Click"></telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%; float: left;">
                        <telerik:RadTreeList ID="RTL_Defect" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                            ParentDataKeyNames="ParentId" OnNeedDataSource="RTL_Defect_NeedDataSource" AllowMultiItemSelection="true">
                            <ItemStyle Font-Size="12px" />
                            <AlternatingItemStyle  Font-Size="12px" />
                            <HeaderStyle Font-Size="13px" />
                            <ClientSettings Selecting-AllowItemSelection="true">
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="600px" />
                            </ClientSettings>
                            <Columns>
                                <telerik:TreeListTemplateColumn>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_CodeDefect" runat="server" Text='<%#Eval("Material_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <telerik:RadButton ID="RB_ExpandDefect" runat="server" Text="+" OnClick="RB_Expand_Click"></telerik:RadButton>
                                        编号
                                    </HeaderTemplate>
                                </telerik:TreeListTemplateColumn>
                                <%--<telerik:TreeListBoundColumn DataField="Material_Code" HeaderText="编号"></telerik:TreeListBoundColumn>--%>
                                <telerik:TreeListBoundColumn DataField="TDM_Description" HeaderText="产品名称"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="LingJian_Type1" HeaderText="零件类型" UniqueName="LingJian_Type1"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Material_Name" HeaderText="物资名称"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Drawing_No" HeaderText="图号"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Technics_Line" HeaderText="工艺路线"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="MaterialDept" HeaderText="领料部门"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Material_Mark" HeaderText="物资牌号"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="ItemCode1" HeaderText="物资编码"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Quantity" HeaderText="物资件数"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Unit" HeaderText="计量单位"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件质量"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Pro_Weight" HeaderText="每产品<br />质量"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Dinge_Size" HeaderText="胚料尺寸"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Rough_Spec" HeaderText="物资规格"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Rough_Size" HeaderText="需求尺寸"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="DemandNumSum" HeaderText="需求数量(kg)" ItemStyle-HorizontalAlign="Center"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="NumCasesSum" HeaderText="需求件数" ItemStyle-HorizontalAlign="Center"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="MissingDescription" HeaderText="缺定额不<br />规范说明"></telerik:TreeListBoundColumn>
                                <%--<telerik:TreeListTemplateColumn HeaderText="同步SmarTeam">
                                    <ItemTemplate>
                                        <telerik:RadButton ID="RB_Synchron" runat="server" Text="同步" CommandName="Synchron" OnClientClicking="confirmWindowSynchron" Visible="false"></telerik:RadButton>
                                    </ItemTemplate>
                                </telerik:TreeListTemplateColumn>--%>
                            </Columns>
                        </telerik:RadTreeList>
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView runat="server">
                    <div style="width: 100%;">
                        <table>
                            <tr>
                                <td>已提交：</td>
                                <td>
                                    <asp:Label ID="Label31" runat="server"></asp:Label></td>
                                <td>，</td>
                                <td>未提交：</td>
                                <td>
                                    <asp:Label ID="Label30" runat="server"></asp:Label></td>
                                <td>，</td>
                                <td>有更改可再提交：</td>
                                <td>
                                    <asp:Label ID="Label32" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%;">
                        <table>
                            <tr>
                                <td>产品名称：</td>
                                <td>
                                    <telerik:RadTextBox ID="RTB_TDM_Description_SubmitState" runat="server" Width="100px">
                                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                    </telerik:RadTextBox></td>
                                <td>零件类型：</td>
                                <td>
                                    <telerik:RadDropDownList ID="RDDL_LingJian_Type_SubmitState" runat="server" Width="100px" AppendDataBoundItems="true">
                                        <Items>
                                            <telerik:DropDownListItem Text="全部" Value="" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>
                                <td>图号：</td>
                                <td>
                                    <telerik:RadTextBox ID="RTB_Drawing_No_SubmitState" runat="server" Width="100px">
                                                                  <ClientEvents OnKeyPress="EnterKeyProcessing" />
                                    </telerik:RadTextBox></td>
                                <td>提交状态：</td>
                                <td>
                                    <telerik:RadDropDownList ID="RDDL_SubmitState" runat="server" Width="100px">
                                        <Items>
                                            <telerik:DropDownListItem Value="0" Text="全部" />
                                            <telerik:DropDownListItem Value="1" Text="未提交" />
                                            <telerik:DropDownListItem Value="2" Text="已提交" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>
                                <td>
                                    <telerik:RadButton ID="RB_SubmitState" runat="server" Text="搜索" OnClick="RB_SubmitState_Click"></telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 100%;">
                        <telerik:RadTreeList ID="RTL_SubmitState" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                            ParentDataKeyNames="ParentId" OnNeedDataSource="RTL_SubmitState_NeedDataSource" OnItemDataBound="RTL_SubmitState_ItemDataBound"
                            AllowMultiItemSelection="true">
                            <ItemStyle  Font-Size="12px" />
                            <AlternatingItemStyle  Font-Size="12px" />
                            <HeaderStyle  Font-Size="13px" />
                            <ClientSettings Selecting-AllowItemSelection="true">
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="600px" />
                            </ClientSettings>
                            <Columns>
                                <telerik:TreeListTemplateColumn>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_CodeSubmit" runat="server" Text='<%#Eval("Material_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <telerik:RadButton ID="RB_ExpandSubmit" runat="server" Text="+" OnClick="RB_Expand_Click"></telerik:RadButton>
                                        编号
                                    </HeaderTemplate>
                                </telerik:TreeListTemplateColumn>
                                <%--<telerik:TreeListBoundColumn DataField="Material_Code" HeaderText="编号"></telerik:TreeListBoundColumn>--%>
                                <telerik:TreeListBoundColumn DataField="TDM_Description" HeaderText="产品名称"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="LingJian_Type1" HeaderText="零件类型"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Material_Name" HeaderText="物资名称"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Material_State1" HeaderText="提交状态" UniqueName="Material_State1"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Drawing_No" HeaderText="图号"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Technics_Line" HeaderText="工艺路线"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="MaterialDept" HeaderText="领料部门"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="ItemCode1" HeaderText="物资编码"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Material_Mark" HeaderText="物资牌号"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Quantity" HeaderText="物资件数"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Unit" HeaderText="计量单位"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件质量"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Mat_Pro_Weight" HeaderText="每产品<br />质量"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Dinge_Size" HeaderText="胚料尺寸"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Rough_Spec" HeaderText="物资规格"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="Rough_Size" HeaderText="需求尺寸"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="DemandNumSum" HeaderText="需求数量(kg)" ItemStyle-HorizontalAlign="Center"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn DataField="NumCasesSum" HeaderText="需求件数" ItemStyle-HorizontalAlign="Center"></telerik:TreeListBoundColumn>
                            </Columns>
                        </telerik:RadTreeList>
                    </div>
                </telerik:RadPageView>                
            </telerik:RadMultiPage>
        </div>
    </div>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
</asp:Content>
