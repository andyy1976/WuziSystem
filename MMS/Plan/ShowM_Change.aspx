<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="ShowM_Change.aspx.cs" Inherits="mms.Plan.ShowM_Change" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
     <div style="width:100%; font-weight:bold; padding-bottom:10px; font-size:15px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        型号：<asp:Label ID="lblModel" runat="server" Font-Bold="false" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        计划包名称：<asp:Label ID="lblPlanName" runat="server" Font-Bold="false"></asp:Label>
    </div>
    <telerik:RadGrid ID="RadGridMCL" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True"
        OnNeedDataSource="RadGridMCL_NeedDataSource" OnItemDataBound="RadGridMCL_ItemDataBound"
        AllowMultiRowSelection="false">
        <ClientSettings Selecting-AllowRowSelect="true" EnableRowHoverStyle="true">
            <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="3" ScrollHeight="600px"></Scrolling>
            <Selecting AllowRowSelect="true" />
        </ClientSettings>
        <HeaderStyle HorizontalAlign="Center" Font-Size="12px" /> 
        <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />            
        <MasterTableView DataKeyNames="RowsId" CommandItemDisplay="Top">
            <ColumnGroups>
                <telerik:GridColumnGroup Name="ChangeDetailed" HeaderText="更改单详细信息"
                     />
            </ColumnGroups>
            <Columns>
                <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ItemStyle-Width="40px" HeaderStyle-Width="40px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Change_Date" HeaderText="更改时间" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Reason" HeaderText="更改原因" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Remark" HeaderText="更改说明" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ChangeList_Code" HeaderText="更改单号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Relevance_Dept" HeaderText="工艺路线" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TDM_Description" HeaderText="名称" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Change_Reason" HeaderText="更改内容" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Change_UserName" HeaderText="更改人" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CN_Drawing_No" HeaderText="图号" ColumnGroupName="ChangeDetailed" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Name" HeaderText="名称" ColumnGroupName="ChangeDetailed" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CN_Com_Drawing_No" HeaderText="所属图号" ColumnGroupName="ChangeDetailed" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ChangeContent" HeaderText="更改内容" UniqueName="ChangeContent" ColumnGroupName="ChangeDetailed" ItemStyle-Width="300px" HeaderStyle-Width="300px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CN_Edit_Type1" HeaderText="类型" ColumnGroupName="ChangeDetailed" ItemStyle-Width="40px" HeaderStyle-Width="40px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CN_Technics_Line" HeaderText="工艺路线" ColumnGroupName="ChangeDetailed" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>    
                <telerik:GridBoundColumn DataField="ChangePerson" HeaderText="更改人" ColumnGroupName="ChangeDetailed" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>     
                <telerik:GridBoundColumn DataField="Influence" HeaderText="影响" ColumnGroupName="ChangeDetailed" ItemStyle-Width="300px" HeaderStyle-Width="300px"></telerik:GridBoundColumn>                     
            </Columns>
            <CommandItemTemplate>
                更改单信息列表
            </CommandItemTemplate>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
