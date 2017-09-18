<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="NoRights.aspx.cs" Inherits="mms.Admin.NoRights" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #NoRightBox {
            float:left;
            margin:30px 0 0 35%;
        }
        #NoRightsTitle {
            width:300px;
            text-align:center;
            line-height:30px;
            color:#ff0000;
            font-size:20px;
            font-weight:bold;
        }
        #HomeBox {
            width:300px;
            line-height:30px;
            text-align:center;
        }
            #HomeBox a {
            color:#0a07f6;
            text-decoration:solid;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="NoRightBox">
        <div id="NoRightsTitle">无权限访问此页面</div>
        <img src="../Images/images/NoRights.jpg" width="300" height="300" alt="" /><br />
        <div id="HomeBox">
            <a id="Home" href="Welcome.aspx">点此跳转至主页</a>
        </div>
    </div>
</asp:Content>
