<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MDemandMergeListState.aspx.cs" Inherits="mms.Plan.MDemandMergeListState" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .floatright {
            float: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" Skin="Silk"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function CloseWindow(args) {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    var oArg = new Object();
                    oWindow.BrowserWindow.refreshGrid(args);
                    oWindow.close(oArg);
                }
            </script>
        </telerik:RadCodeBlock>
        <div>
            <telerik:RadGrid ID="RadGridFailed" runat="server" AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="true"
                OnItemCommand="RadGridFailed_ItemCommand">
                <HeaderStyle HorizontalAlign="Center" Font-Size="12px" />
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="3" ScrollHeight="260px" />
                </ClientSettings>
                <MasterTableView>
                    <CommandItemTemplate>
                        提交物流中心失败列表
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" HeaderText="编号" ItemStyle-Width="50" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Err_MSG" HeaderText="提交失败原因" ItemStyle-Width="200px" HeaderStyle-Width="200px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="产品图号" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="共计需求<br />数量(kg)" ItemStyle-Width="70" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="共计需求<br />件数" ItemStyle-Width="70" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Urgency_Degre1" HeaderText="紧急程度" ItemStyle-Width="70" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" ItemStyle-Width="50" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Phase1" HeaderText="研制<br />阶段" ItemStyle-Width="50" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Use_Des1" HeaderText="用途" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" ItemStyle-Width="60" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Manufacturer" HeaderText="生产厂家" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MaterialDept" HeaderText="领料<br />部门" ItemStyle-Width="50" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>

            <telerik:RadButton ID="BtnClose" runat="server" Text="关闭" AutoPostBack="false" OnClientClicking="CloseWindow" CssClass="floatright"></telerik:RadButton>
            <telerik:RadGrid runat="server" ID="RadGrid1" AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="true" OnNeedDataSource="RadGrid1_OnNeedDataSource">
                <HeaderStyle HorizontalAlign="Center" Font-Size="12px" />
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="3" ScrollHeight="260px" />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" CommandItemDisplay="Top">
                    <CommandItemTemplate>提交物流中心成功列表</CommandItemTemplate>
                    <CommandItemStyle HorizontalAlign="Center" Height="36px" Font-Size="16px" Font-Bold="true" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" HeaderText="编号" ItemStyle-Width="50" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="产品图号" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Dinge_Size" HeaderText="胚料尺寸" ItemStyle-Width="100" HeaderStyle-Width="100px"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="胚料规格" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="共计数量" ItemStyle-Width="70" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数" ItemStyle-Width="70" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="需求尺寸" ItemStyle-Width="100" HeaderStyle-Width="100px"> </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Urgency_Degre1" HeaderText="紧急程度" ItemStyle-Width="70" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级" ItemStyle-Width="70" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Phase1" HeaderText="研制阶段" ItemStyle-Width="70" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Use_Des1" HeaderText="用途" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证" ItemStyle-Width="60" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Manufacturer" HeaderText="生产厂家" ItemStyle-Width="100" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MaterialDept" HeaderText="领料部门" ItemStyle-Width="80" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
