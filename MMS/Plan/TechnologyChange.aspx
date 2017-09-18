<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TechnologyChange.aspx.cs" Inherits="mms.Plan.TechnologyChange" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid_TechnologyList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_TechnologyList" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <div style="width: 100%; float: left;">
            <div class="divContant">
                <div style="width: 100%; text-align: center;margin-top:20px;margin-bottom:20px;">
                    <b style="font-size: 22px;"><span id="span_title" runat="server"></span></b>
                </div>
                <div class="divSiteMap">
                    <span style="float:left">编号：</span><span id="span_gysyjCode" runat="server" style="float:left"></span>
                </div>
                <div id="div_no_submit" style="font-size: 14px; text-align: center; margin-bottom:20px;">
                    <div class="divViewPanel">
                        <telerik:RadGrid ID="RadGrid_TechnologyList" runat="server" AllowPaging="True" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                            OnNeedDataSource="RadGrid_TechnologyList_NeedDataSource" OnItemCommand="RadGrid_TechnologyList_ItemCommand" PageSize="50">
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
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderText="编辑" UniqueName="EditCommandColumn"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                    </telerik:GridEditCommandColumn>
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn ButtonType="ImageButton" UpdateText="修改" CancelText="取消">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
                            AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
                        </telerik:RadNotification>
                        <telerik:RadWindow ID="confirmDeleteWindow" runat="server" VisibleTitlebar="false"
                            VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                            <ContentTemplate>
                                <div style="margin-top: 30px; float: left;">
                                    <div style="width: 60px; padding-left: 15px; float: left;">
                                        <img src="../Images/images/warnning1.jpg" alt="" />
                                    </div>
                                    <div style="width: 200px; float: left;">
                                        <asp:Label ID="lblConfirm" Font-Size="14px" Text="确定要更新选定的记录吗？" runat="server" Font-Bold="true"
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
