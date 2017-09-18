<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="PhaseManage.aspx.cs" Inherits="mms.SystemMangement.PhaseManage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                args.set_enableAjax(false);

            }
        }
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->研制阶段管理" ClientIDMode="Static" />

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
	</telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridPhase">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridPhase" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var deleteId;
            function confirmWindow(sender, args) {
                var oWnd = $find("<%=confirmWindow.ClientID %>");
                oWnd.show();
                deleteId = sender.get_id();
                args.set_cancel(true);
            }
            function YesOrNoClicked(sender, args) {
                var oWnd = $find("<%=confirmWindow.ClientID %>");
                oWnd.close();
                if (sender.get_text() == "是") {
                    $find(deleteId).click();
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div style="width: 100%; margin: 0px auto;">
       
        <div style="width: 100%; float: left; padding-top: 20px;">           
            <telerik:RadGrid ID="RadGridPhase" runat="server" AllowPaging="true" OnNeedDataSource="RadGridPhase_NeedDataSource"
                OnItemCommand="RadGridPhase_ItemCommand" AutoGenerateColumns="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings EnableRowHoverStyle="true" >
                    <Selecting AllowRowSelect="true" />
                </ClientSettings>
                <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                        <Pdf  DefaultFontFamily="Arial Unicode MS" />
                </ExportSettings>
                <MasterTableView DataKeyNames="Id" CommandItemDisplay="Top" EditMode="InPlace">
                    <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                    <CommandItemTemplate>
                        <telerik:RadButton ID="RadButtonAdd" runat="server" Text="新增研制阶段" Font-Bold="true" CommandName="InitInsert" CssClass="floatleft"></telerik:RadButton>
                        研制阶段列表
                        <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Code" HeaderText="序号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Phase" HeaderText="代码" ItemStyle-Width="300px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridDropDownColumn DataField="Basicdata_DICT_Code" HeaderText="物资基础库中的名称" 
                            DataSourceID="SqlDataSource1" ListTextField="DICT_Name" ListValueField="DICT_Code" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridDropDownColumn>
                        <telerik:GridEditCommandColumn ButtonType="LinkButton" HeaderText="编辑"
                            ItemStyle-Width="100px" HeaderStyle-Width="100px" InsertText="新增" EditText="编辑" UpdateText="修改" CancelText="取消">
                        </telerik:GridEditCommandColumn>
                        <telerik:GridTemplateColumn  ItemStyle-Width="100px" HeaderStyle-Width="100px" HeaderText="删除">
                            <ItemTemplate>
                                <telerik:RadButton ID="RBDeleteModel" runat="server" Text="删除" ButtonType="ToggleButton" CommandName="delete" OnClientClicking="confirmWindow"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
           
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>"
        SelectCommand=" select DICT_CODE, DICT_NAME from GetBasicdata_T_Item where DICT_CLASS = 'CUX_DM_PHASE'"></asp:SqlDataSource>
    <%-- 删除提示--开始--%>
    <telerik:RadWindow ID="confirmWindow" runat="server" VisibleTitlebar="false"
        VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
        <ContentTemplate>
            <div style="margin-top: 30px; float: left;">
                <div style="width: 60px; padding-left: 15px; float: left;">
                    <img src="../Images/images/warnning1.jpg" alt="" />
                </div>
                <div style="width: 200px; float: left;">
                    <asp:Label ID="Label1" Font-Size="14px" Text="确定要删除吗？" runat="server" Font-Bold="true"
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
     <%-- 删除提示--开始--%>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
    </telerik:RadNotification>
</asp:Content>
