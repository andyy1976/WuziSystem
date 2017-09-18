<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncUserFromAD.aspx.cs" Inherits="mms.SystemMangement.SyncUserFromAD" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2015.3.930.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <tr height="50">
            <td style="padding-left: 22px;">
                <table class="InputArea" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="title">
                            <span class="InputLabel">域服务器：</span>
                        </td>
                        <td class="textbox">
                            <asp:TextBox ID="txtServerName" runat="server"></asp:TextBox>
                        </td>
                        <td class="title">
                            <span class="InputLabel">端口号：</span>
                        </td>
                        <td class="textbox">
                            <asp:TextBox ID="txtPort" runat="server"></asp:TextBox>
                            
                        </td>
                        <td class="title">
                            <span class="InputLabel">用户名：</span>
                        </td>
                        <td class="textbox">
                            <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                        </td>
                        <td class="title">
                            <span class="InputLabel">密码：</span>
                        </td>
                        <td class="textbox">
                            <asp:TextBox ID="txtPwd" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnSynch" runat="server" Text="Button" OnClick="btnSynch_Click"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </div>
    </form>
</body>
</html>
