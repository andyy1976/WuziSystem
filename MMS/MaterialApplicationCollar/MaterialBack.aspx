<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MaterialBack.aspx.cs" Inherits="mms.MaterialApplicationCollar.MaterialBack" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"></telerik:RadCodeBlock>
    <div style="width:100%;">
        <table>
            <tr>
                <td>申请部门：</td>
                <td>
                    <telerik:RadDropDownList ID="RDDL_Dept" runat="server" Width="120px" AppendDataBoundItems="true">
                        <Items>
                            <telerik:DropDownListItem Value="0" Text="全部" />
                        </Items>
                    </telerik:RadDropDownList>
                </td>
                <td>申请时间：</td>
                <td><telerik:RadDatePicker ID="RDP_StartDate" runat="server" Width="120px"></telerik:RadDatePicker></td>
                <td> ～</td>
                <td><telerik:RadDatePicker ID="RDP_EndDate" runat="server" Width="120px"></telerik:RadDatePicker></td>
                <td>申请人：</td>
                <td><telerik:RadTextBox ID="RTB_Applicant" runat="server" Width="120px"></telerik:RadTextBox></td>
                <td></td>
            </tr>
        </table>
    </div>
    <div style="width:100%;">

    </div>
</asp:Content>
