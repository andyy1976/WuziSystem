<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="mms.Admin.Welcome" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=divm.ClientID%>").slideDown("slow");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <asp:HiddenField ID="HiddenField" runat="server" Value="欢迎您使用本系统" ClientIDMode="Static" />
    <%--<div style="background-image:url('/Images/index/mianbj.jpg'); background-repeat:no-repeat;
        background-position:top center; width:100%;height:400px;">
    </div>--%>
    <div style="background-image:url('/Images/master/mainbg.jpg'); background-size:cover; 
        background-position:top center; width:100%; height: 500px;">
        <div id="divm" runat="server" style="position:absolute; bottom:0px; left:0px; display:none;">
            <telerik:RadGrid ID="RadGridPlan" runat="server" AutoGenerateColumns="false" Width="300px">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="PlanName" HeaderText="计划包名称" HeaderStyle-Width="100px" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MaterialState4" HeaderText="缺定额" HeaderStyle-Width="50px" ItemStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MaterialState5" HeaderText="不规范" HeaderStyle-Width="50px" ItemStyle-Width="50px"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
</asp:Content>
