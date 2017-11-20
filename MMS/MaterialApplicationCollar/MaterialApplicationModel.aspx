<%@ Page Title="" Language="C#" MasterPageFile="~/index.Master" AutoEventWireup="true" CodeBehind="MaterialApplicationModel.aspx.cs" Inherits="mms.MaterialApplicationCollar.MaterialApplicationModel" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0)
			{

                args.set_enableAjax(false);

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField" runat="server" Value="物资申请-->型号投产物资申请" ClientIDMode="Static" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnRequestStart="onRequestStart" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMDML" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGridMDML">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMDML" />
                    <telerik:AjaxUpdatedControl ControlID="HFMDMLID"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Search_Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridMDML" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_FillInApp">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock runat="server">
        <script type="text/javascript">

            function AlphabetOnly(sender, eventArgs) {
                var c = eventArgs.get_keyCode();
                if ((c == 13)) {
                    eventArgs.set_cancel(true);
                }
            }

            function refreshGrid(arg) {
                if (!arg) {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                }
            }
           function ShowWindow(sender, args) {
                var grid = $find("<%=RadGridMDML.ClientID%>");
                if (grid._selectedIndexes.length == 0) {
                    $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择申请行");
                    $find("<%=RadNotificationAlert.ClientID%>").show();
                    return;
                } else {
                    var win = $find("<%=RadWindowManager1.ClientID %>");
                    var ID = $get("<%=HFMDMLID.ClientID%>").value;
                    if (ID == "") {
                        $find("<%=RadNotificationAlert.ClientID%>").set_text("请选择申请行");
                        $find("<%=RadNotificationAlert.ClientID%>").show();
                        return;
                    }
                    window.radopen("/MaterialApplicationCollar/MaterialAppWindow.aspx?Type=0&MDMLID=" + ID, "RadWindowApp");
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <div style="width: 100%; float: left;">
        <div style="width: 100%; float: left;">
            <table>
                <tr>
                    <td>需求行号：</td>
                    <td><telerik:RadTextBox ID="RTB_ID" runat="server" OnKeyPress="AlphabetOnly" Width="100px"></telerik:RadTextBox></td>
                    <td>型号：</td>
                    <td><telerik:RadTextBox ID="RTB_Project" runat="server" OnKeyPress="AlphabetOnly" Width="100px"></telerik:RadTextBox></td>
                    <td>图号：</td>
                    <td><telerik:RadTextBox ID="RTB_Drawing_No" runat="server" OnKeyPress="AlphabetOnly" Width="100px"></telerik:RadTextBox></td>
                    <td>任务号：</td>
                    <td><telerik:RadTextBox ID="RTB_TaskCode" runat="server" OnKeyPress="AlphabetOnly" Width="100px"></telerik:RadTextBox></td>

                    <td>物资名称：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Name" runat="server" OnKeyPress="AlphabetOnly" Width="100px"></telerik:RadTextBox></td>
                    <td>物资编码：</td>
                    <td><telerik:RadTextBox ID="RTB_ItemCode1" runat="server" OnKeyPress="AlphabetOnly" Width="100px"></telerik:RadTextBox></td>
                    <td>申请时间：</td>
                    <td><telerik:RadDatePicker ID="RDPStart" runat="server" Width="100px"></telerik:RadDatePicker>
                      ～<telerik:RadDatePicker ID="RDPEnd" runat="server" Width="100px"></telerik:RadDatePicker></td>
                    <td><telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton></td>
                </tr>

                 <tr>
                    <td>提交人：</td>
                    <td><telerik:RadTextBox ID="RTB_Applicant" runat="server" OnKeyPress="AlphabetOnly" Width="100px"></telerik:RadTextBox></td>
                    <td>领用单位：</td>
                    <td><telerik:RadTextBox ID="RTB_App_Depart" runat="server" OnKeyPress="AlphabetOnly" Width="100px"></telerik:RadTextBox></td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; float: left;">
            <asp:HiddenField ID="HF_DeptCode" runat="server" />
            <asp:HiddenField ID="HFMDMLID" runat="server" />
            <telerik:RadGrid ID="RadGridMDML" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                OnNeedDataSource="RadGridMDML_NeedDataSource" OnItemDataBound="RadGridMDML_ItemDataBound" OnSelectedIndexChanged="RadGridMDML_SelectedIndexChanged"
                AllowPaging="true" PageSize="20" PagerStyle-AlwaysVisible="True" AllowSorting="true">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle Font-Size="12px" HorizontalAlign="Center" />
                <HeaderStyle Font-Size="13px" HorizontalAlign="Center"/>
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings Selecting-AllowRowSelect="true"  EnablePostBackOnRowClick="true" EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3" ScrollHeight="600px"></Scrolling>
                </ClientSettings>
                <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                </ExportSettings>
                <MasterTableView DataKeyNames="ID" CommandItemDisplay="Top" ClientDataKeyNames="ID" ClientIDMode="AutoID" >
			    <CommandItemSettings ShowExportToExcelButton="true" ShowExportToWordButton="true" ShowExportToPdfButton="true" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="Requirement" HeaderText="需求信息" ></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="Material" HeaderText="物资信息" ></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="ProductInfo" HeaderText="产品信息" ></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ColumnGroupName="Requirement"  ItemStyle-Width="30px" HeaderStyle-Width="30px"></telerik:GridClientSelectColumn>
                        <telerik:GridBoundColumn DataField="ID" UniqueName="ID" HeaderText="需求行号" ColumnGroupName="Requirement" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数" ColumnGroupName="Requirement" DataFormatString="{0:N0}" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求量" ColumnGroupName="Requirement" DataFormatString="{0:N0}" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RowsId" HeaderText="序号" ColumnGroupName="ProductInfo" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Model" HeaderText="型号" ColumnGroupName="ProductInfo" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Com_ProductName" HeaderText="所属产品名称" ColumnGroupName="ProductInfo" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TaskCode" HeaderText="任务号" ColumnGroupName="ProductInfo" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>                       
                        <telerik:GridBoundColumn DataField="Com_Stage" HeaderText="阶段" ColumnGroupName="ProductInfo" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TDM_Description" HeaderText="产品名称" ColumnGroupName="ProductInfo" UniqueName="TDM_Description" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Drawing_No" HeaderText="图号" ColumnGroupName="ProductInfo" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemCode1" HeaderText="物资编码"  ColumnGroupName="Material" UniqueName="ItemCount1" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称" UniqueName="Material_Name" ColumnGroupName="Material" ItemStyle-Width="120px" HeaderStyle-Width="120px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Mark" HeaderText="牌号" UniqueName="Material_Mark" ColumnGroupName="Material" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CN_Material_State" HeaderText="供应状态" UniqueName="CN_Material_State" ColumnGroupName="Material" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Material_Tech_Condition" HeaderText="采用标准" UniqueName="Material_Tech_Condition" ColumnGroupName="Material" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rough_Spec" HeaderText="规格" UniqueName="Rough_Spec" ColumnGroupName="Material" ItemStyle-Width="80px" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Mat_Rough_Weight" HeaderText="单件定额质量" ItemStyle-Width="100px" HeaderStyle-Width="100px" UniqueName="Mat_Rough_Weight" ColumnGroupName="Material"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Mat_Unit" HeaderText="计量单位" UniqueName="Mat_Unit" ColumnGroupName="Material" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rough_Size" HeaderText="尺寸" UniqueName="Rough_Size" ColumnGroupName="Material" ItemStyle-Width="70px" HeaderStyle-Width="70px"></telerik:GridBoundColumn>
                      </Columns>
                    <CommandItemTemplate>
                        请领物资信息列表--型号投产
                        <telerik:RadButton ID="RB_FillInApp" runat="server" Text="填写申请单" OnClientClicking="ShowWindow" CssClass="floatright" AutoPostBack="true"></telerik:RadButton>
				        <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
                    </CommandItemTemplate>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <div style="width: 100%; float: left;"></div>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowApp" runat="server" Title="型号投产物资申请" Left="150px"
                ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                Behaviors="Close,Maximize,Minimize" Modal="true" Width="1000px" Height="680px" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="请选择申请行" Position="Center"
        AutoCloseDelay="4000" Width="300" Title="提示" EnableRoundedCorners="true"  >
    </telerik:RadNotification>
</asp:Content>
