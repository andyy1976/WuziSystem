<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="mms.Default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录界面</title>
    <link href="Styles/login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" Skin="Silk"></telerik:RadSkinManager>

        <div class="login_logo">
            <img src="Images/login_logo.png" />
        </div>
        <div class="login_form">
            <div>
                <div class="login_box">
                    <div class="login_input">
                        <asp:TextBox ID="UserName" runat="server" placeholder="用户名"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="UserName">*</asp:RequiredFieldValidator>--%>
                    </div>
                    <div class="login_input login_passbg">
                        <asp:TextBox ID="PassWord" runat="server" TextMode="Password" placeholder="请输入用户密码"></asp:TextBox>
                    </div>
                    <ul class="login_button">
                        <li>
                            <asp:Button ID="LoginBut" CssClass="LoginBut" runat="server" Text="登 录" OnClick="LoginBut_Click" />
                        </li>
                        <li>
                            <asp:Button ID="AdLoginBut" CssClass="AdLoginBut" runat="server" Text="域 控 登 录" OnClick="AdLoginBut_Click" />
                        </li>

                    </ul>
                </div>
            </div>
        </div>
        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
            AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true">
        </telerik:RadNotification>
    </form>
</body>
</html>
