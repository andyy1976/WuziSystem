<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="M_Demand_Merge_List.aspx.cs" Inherits="mms.Plan.M_Demand_Merge_List" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"></telerik:RadCodeBlock>

    <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false"
        OnNeedDataSource="RadGrid1_NeedDataSource">
        <MasterTableView DataKeyNames="ID">
            <Columns>
                <telerik:GridBoundColumn DataField="Task_Code" HeaderText="任务号"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="MaterialDept" HeaderText="领料部门"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Special_Needs" HeaderText="特殊需求"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Urgency_Degre" HeaderText="紧急程度"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Secret_Level" HeaderText="密级"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Use_Des" HeaderText="用途"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Shipping_Address" HeaderText="配送地址"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Certification" HeaderText="合格证"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求时间"></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
