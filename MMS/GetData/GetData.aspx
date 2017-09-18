<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetData.aspx.cs" Inherits="mms.GetData.GetData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Btn_ALL" runat="server" Text="获取全部" OnClick="Btn_ALL_Click" /><br />
        <asp:Button runat="server" ID="btn_GetExeInf" Text="GetExeInf需求执行信息" OnClick="btn_GetExeInf_OnClick"/>
        <asp:Button runat="server" ID="Btn_GetRqStatus" Text="GetRqStatus需求申请提交状态变更信息" OnClick="Btn_GetRqStatus_OnClick"/>
        <asp:Button runat="server" ID="Btn_GetRcoStatus" Text="GetRqStatus需求申请提交状态变更信息" OnClick="Btn_GetRcoStatus_OnClick"/>
    </div>
    </form>
</body>
</html>
