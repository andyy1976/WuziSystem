<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserManagePWD.ascx.cs" Inherits="mms.SystemMangement.UserManagePWD1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<table  style="width: 540px; height: 200px; margin: 0px auto; text-align: left;">
    <tr>
        <td>
            <table style="width: 320px; height: 200px; margin: 0px auto; text-align: left;">
                <tr>
                    <td style="width: 100px; text-align: right;">账户名称：</td>
                    <td style="width: 200px;">
                        <telerik:RadTextBox ID="RTB_UserAccount" runat="server" Text='<%# Bind("UserAccount") %>'></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="float: right;">域帐号：</td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="RTB_DomainAccount" Text='<%# Bind("DomainAccount") %>'></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">用户名称：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_UserName" runat="server" Text='<%# Bind("UserName") %>'></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;">用户密码：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_PassWord" runat="server" TextMode="Password"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadButton ID="RB_PWD1" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="使用原密码" GroupName="RB_PWD" Checked="true" AutoPostBack="false"></telerik:RadButton>
                        <telerik:RadButton ID="RB_PWD2" runat="server" ButtonType="ToggleButton" ToggleType="Radio" Text="修改密码" GroupName="RB_PWD" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">部门名称：</td>
                    <td>
                        <telerik:RadComboBox ID="RCB_Dept" runat="server" DataSourceID="SqlDataSourceDept" DataTextField="Dept" DataValueField="ID" DataCheckedField='<%# Bind("Dept") %>'></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">联系电话：</td>
                    <td><telerik:RadTextBox ID="RTB_Phone" runat="server" Text='<%# Bind("Phone") %>'></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadButton ID="RB_Update" Text='<%# (Container is GridEditFormInsertItem) ? "新增":"更新" %>'
                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem)?"PerformInsert":"Update" %>'>
                        </telerik:RadButton>
                        <telerik:RadButton ID="RB_Cancel" runat="server" Text="取消" CausesValidation="false" CommandName="Cancel"></telerik:RadButton>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            用户所属角色
            <div style="width:200px; height: 200px; overflow: scroll; overflow-x: hidden; border: 1px solid #ccc;">
                <asp:CheckBoxList ID="CBL_Role" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
            </div>
        </td>
    </tr>
</table>

