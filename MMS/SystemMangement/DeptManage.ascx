<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptManage.ascx.cs" Inherits="mms.SystemMangement.DeptManage1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<table style="text-align:left; margin:0px auto;">
    <tr>
        <td style="text-align:right;">部门名称：</td>
        <td><telerik:RadTextBox ID="RTB_DeptName" runat="server" Width="160px" Text='<%# Bind("Dept") %>'></telerik:RadTextBox></td>
    </tr>
    <tr>
        <td style="text-align:right;">部门编号：</td>
        <td><telerik:RadTextBox ID="RTB_DeptCode" runat="server" Width="160px" Text='<%# Bind("DeptCode") %>'></telerik:RadTextBox></td>
    </tr>
    <tr>
        <td style="text-align:right;">物流中心对应部门：</td>
        <td><telerik:RadDropDownList ID="RDDL_Cust_Account_ID" runat="server" Width="160px"
            DataSourceID="SqlDataSourceCust_Account" DataTextField="Account_Name" DataValueField="Cust_Account_ID"></telerik:RadDropDownList></td>
    </tr>
    <tr>
        <td style="vertical-align:top; text-align:right;">配送地址：</td>
        <td>
            <div style="border:solid 1px black;">
                <asp:CheckBoxList ID="CBL_Shipping_Address" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
            </div>
        </td>
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