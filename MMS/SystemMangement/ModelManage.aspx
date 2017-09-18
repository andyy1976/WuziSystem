<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="ModelManage.aspx.cs" Inherits="mms.SystemMangement.ModelManage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />--%>
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0) {

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="系统管理-->型号、地区管理" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridModel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridModel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridArea">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridArea" LoadingPanelID="RadAjaxLoadingPanel1" />
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
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine ="true" Skin="Default">
                <Tabs>
                    <telerik:RadTab Value="0" Text="型号管理" Selected="true"></telerik:RadTab>
                    <telerik:RadTab Value="1" Text="地区管理"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
                <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
                    <telerik:RadGrid ID="RadGridModel" runat="server" AllowPaging="true" OnNeedDataSource="RadGridModel_NeedDataSource"
                        OnItemCommand="RadGridModel_ItemCommand" AutoGenerateColumns="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
					
                            <CommandItemTemplate>
                                <telerik:RadButton ID="RadButtonAdd" runat="server"
                                    Text="新增型号" Font-Bold="true" CommandName="InitInsert" CssClass="floatleft">
                                </telerik:RadButton>
                                型号列表
                         <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                         <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsID" HeaderText="序号" ReadOnly="true" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Model" HeaderText="型号" ItemStyle-Width="300px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataField="AreaId" HeaderText="地区代码 - 地区名称" 
                                    DataSourceID="SqlDataSource1" ListTextField="AreaCode" ListValueField="Id" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridDropDownColumn>
                                <telerik:GridCheckBoxColumn DataField="IsGetBOM" HeaderText="获取材料定额时使用地区代码" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridCheckBoxColumn>
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
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>" 
                        SelectCommand="select '' as Id , '' as AreaCode union SELECT Id, AreaCode + '-' + AreaName as AreaCode FROM [Sys_Area] where IsDel = 'false' order by AreaCode"></asp:SqlDataSource>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <telerik:RadGrid ID="RadGridArea" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGridArea_NeedDataSource" OnItemCommand="RadGridArea_ItemCommand">
                        <AlternatingItemStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="true" />
                        </ClientSettings>
                        <MasterTableView DataKeyNames="Id" CommandItemDisplay="Top" EditMode="InPlace">
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ReadOnly="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AreaCode" HeaderText="地区代码"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AreaName" HeaderText="地区名称"></telerik:GridBoundColumn>     
                                <telerik:GridEditCommandColumn ButtonType="LinkButton" HeaderText="编辑"
                                    ItemStyle-Width="100px" HeaderStyle-Width="100px" InsertText="新增" EditText="编辑" UpdateText="修改" CancelText="取消">
                                </telerik:GridEditCommandColumn>
                                <telerik:GridTemplateColumn HeaderText="删除">
                                    <ItemTemplate>
                                        <telerik:RadButton ID="RBDeleteArea" runat="server" Text="删除" ButtonType="ToggleButton" CommandName="delete"  OnClientClicking="confirmWindow"></telerik:RadButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                         
                            </Columns>
                            <CommandItemStyle HorizontalAlign="Center" Height="36px" Font-Size="16px" Font-Bold="true" />
                            <CommandItemTemplate>
                                <telerik:RadButton ID="RBAddArea" runat="server" Text="新增地区" Font-Bold="true" CommandName="InitInsert" CssClass="floatleft"></telerik:RadButton>
                                地区列表
                            </CommandItemTemplate>
                        </MasterTableView>
                    </telerik:RadGrid>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </div>
    </div>
    <asp:SqlDataSource ID="SqlDataSourceArea" runat="server" ConnectionString="<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>"
        SelectCommand=" select '' as Area union select '1' as Area union select '2' as Area"></asp:SqlDataSource>
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
