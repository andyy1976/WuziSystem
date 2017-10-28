<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialApplicationApprove.aspx.cs" Inherits="mms.MaterialApplicationCollar.MaterialApplicationApprove" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" OnNeedDataSource ="RadGrid1_NeedDataSource">
            <ClientSettings Selecting-AllowRowSelect="true"  EnablePostBackOnRowClick="true" EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                </ClientSettings>
                <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
            <MasterTableView>
                <Columns>
                    <telerik:GridBoundColumn DataField="ActivityName" HeaderText="核准流程名称"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApproveLeaderDept" HeaderText="核准人部门"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApproveLeaderName" HeaderText="核准人姓名"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApproveTime" HeaderText="核准时间"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Reason" HeaderText="原因"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Result" HeaderText="结果"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
    </div>
    </form>
</body>
</html>
