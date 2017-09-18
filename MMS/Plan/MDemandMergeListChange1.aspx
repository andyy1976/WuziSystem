<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MDemandMergeListChange1.aspx.cs" Inherits="mms.Plan.MDemandMergeListChange1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfFlag" runat="server" />
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->物资需求清单" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RB_Submit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"></telerik:RadCodeBlock>
    <div style="width:100%;">
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Default" ShowBaseLine ="true">
            <Tabs>
                <telerik:RadTab Text="需要提交" TabIndex="0"></telerik:RadTab>
                <telerik:RadTab Text="需求变更" TabIndex="1" Selected="true"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
    </div>
    <div style="width:100%;">
        <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="false" OnNeedDataSource="RadGrid1_NeedDataSource"
            AllowPaging="true" OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">
            <AlternatingItemStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
            <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
            <ClientSettings EnableRowHoverStyle="true" >
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <MasterTableView DataKeyNames="ID">
                <Columns>
                    <telerik:GridBoundColumn DataField="ID" HeaderText="需求行号"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Material_Name" HeaderText="产品名称"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Dept" HeaderText="领料部门"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="配送地址">
                        <ItemTemplate>
                            <telerik:RadDropDownList ID="RDDL_Shipping_Address" runat="server" Width="120px"></telerik:RadDropDownList>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="需求时间">
                        <ItemTemplate>
                            <telerik:RadDatePicker ID="RDP_DemandDate" runat="server" Width="120px"></telerik:RadDatePicker>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="特殊需求">
                        <ItemTemplate>
                            <telerik:RadTextBox ID="RTB_Special_Needs" runat="server" Width="100px"></telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="紧急程度">
                        <ItemTemplate>
                            <telerik:RadDropDownList ID="RDDL_Urgency_Degre" runat="server" Width="80px" 
                                DataSourceID="SqlDataSourceUrgencyDegre" DataTextField="DICT_Name" DataValueField="DICT_Code"></telerik:RadDropDownList>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="密级">
                        <ItemTemplate>
                            <telerik:RadDropDownList ID="RDDL_Secret_Level" runat="server" Width="80px"
                                DataSourceID="SqlDataSourceSecretLevel" DataTextField="SecretLevel_Name" DataValueField="SecretLevel_Name"></telerik:RadDropDownList>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="用途">
                        <ItemTemplate>
                            <telerik:RadDropDownList ID="RDDL_Use_Des" runat="server" Width="100px"
                                DataSourceID="SqlDataSourceUseDes" DataTextField="DICT_Name" DataValueField="DICT_Code"></telerik:RadDropDownList>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="合格证">
                        <ItemTemplate>
                            <telerik:RadDropDownList ID="RDDL_Certification" runat="server" Width="40px">
                                <Items>
                                    <telerik:DropDownListItem Value="Y" Text="Y" />
                                    <telerik:DropDownListItem Value="N" Text="N" />
                                </Items>
                            </telerik:RadDropDownList>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="生产厂家">
                        <ItemTemplate>
                            <telerik:RadTextBox ID="RTB_Manufacturer" runat="server" Width="120px"></telerik:RadTextBox>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="State" HeaderText="状态"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Submit_Date1" HeaderText="提交时间"></telerik:GridBoundColumn>   
                    <telerik:GridTemplateColumn>
                        <ItemTemplate>
                            <telerik:RadButton ID="RB_Submit" runat="server" Text="更改需求" ButtonType="ToggleButton" CommandName="Change" ForeColor="Blue"></telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <asp:SqlDataSource ID="SqlDataSourceUrgencyDegre" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
            SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceSecretLevel" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
            SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceUseDes" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
            SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE'"></asp:SqlDataSource>
    </div>
</asp:Content>
