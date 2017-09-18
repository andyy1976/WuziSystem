<%@ Page  Language="C#"  AutoEventWireup="true" CodeBehind="MDemandDetailsCombine.aspx.cs"  Inherits="mms.Plan.MDemandDetailsCombine" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .floatright {
            float:right;
        }
    </style>
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("RadButton_ExportExcel") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportWord") >= 0 || args.get_eventTarget().indexOf("RadButton_ExportPDF") >= 0)
			{

                args.set_enableAjax(false);

            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800"></telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" Skin="Silk"></telerik:RadSkinManager>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                      <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandCombinelist">
                          <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandCombinelist" LoadingPanelID="RadAjaxLoadingPanelLoading" /><telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                          </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadBtn_Search">
                            <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandCombinelist" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                                <telerik:AjaxUpdatedControl ControlID="span_DraftCode" />
                                <telerik:AjaxUpdatedControl ControlID="span_model" />
                                <telerik:AjaxUpdatedControl ControlID="span_plancode" />
                                <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RB_Submit">
                            <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandCombinelist" LoadingPanelID="RadAjaxLoadingPanelLoading" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanelLoading" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                function CloseRadWindow(sender,args) {
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    var oArg = new Object();
                    oWindow.close();
                    oWindow.BrowserWindow.refreshGrid1(args);
                    
                }
                function CloseWindow1(sender,args){
                    var oWindow = null;
                    if (window.radWindow) oWindow = window.radWindow;
                    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                    oWindow.close();
                }
                var submintId;
                function confirmRadWindow(sender, args) {
                    $find("<%= RadWindow.ClientID %>").show();
                    submintId = sender.get_id();
                    args.set_cancel(true);
                }
                function YesOrNoClicked(sender, args) {
                    var oWnd = $find("<%=RadWindow.ClientID %>");
                    oWnd.close();
                    if (sender.get_text() == "是") {
                        $find(submintId).click();
                    }
                }
            </script>
        </telerik:RadCodeBlock>
        <div style="position:fixed; right:0px; top:0px;">
            <telerik:RadButton ID="BtnClose" runat="server" Text="关闭" AutoPostBack="false" OnClientClicking="CloseWindow1" CssClass="floatright"></telerik:RadButton>
        </div>
        <asp:HiddenField ID="HiddenField" runat="server" Value="物资需求-->物资需求清单数据合并" ClientIDMode="Static" />
        <div style="width: 100%; float: left;">
            <div class="divContant" style="margin-top: 0px;">
                <div class="divSiteMap add_divSiteMap" style="clear: both; width: 100%;">
                    <div style="float: left; height: 40px; line-height: 0px;">
                        <h3 style="font-weight: bold">您选择的需要合并的物资需求清单数据记录</h3>
                        
                    </div>
                    <div style="width: 100%; height: 0px; border: solid #000 1px; margin: 5px 0; clear: both;"></div>
                </div>

                <div class="divViewPanel">
                <telerik:RadGrid ID="RadGrid_MDemandDetails" runat="server" DataKeyNames="ID" Culture="zh-CN" GroupPanelPosition="Top"
                    AllowPaging="True"  PageSize="20" PagerStyle-AlwaysVisible="True"
                    OnNeedDataSource="RadGrid_MDemandDetails_NeedDataSource" AllowMultiRowSelection="true"
                    OnItemDataBound="RadGrid_MDemandDetails_ItemDataBound">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                    <ClientSettings EnableRowHoverStyle="true" >
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="6" ScrollHeight="160px"></Scrolling>
                    </ClientSettings>
                     <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="false" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                     </ExportSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID,ItemCode1" CommandItemDisplay="Top">
                          <CommandItemSettings ShowExportToExcelButton="True" ShowExportToWordButton="true" ShowExportToPdfButton="true"  ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>
                        

                            <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="编号" ReadOnly="True" SortExpression="ID" UniqueName="ID">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="mstate" FilterControlAltText="Filter mstate column" HeaderText="物资状态" SortExpression="mstate" UniqueName="mstate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Material_State" FilterControlAltText="Filter Material_State column" Visible="false" SortExpression="Material_State" UniqueName="Material_State">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="MDPId" FilterControlAltText="Filter MDPId column" Visible="false" SortExpression="MDPId" UniqueName="MDPId">
                            </telerik:GridBoundColumn>
                      
                            <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="材料名称" SortExpression="Material_Name" UniqueName="Material_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Drawing_No" FilterControlAltText="Filter Drawing_No column" HeaderText="产品图号" SortExpression="Drawing_No" UniqueName="Drawing_No">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Technics_Line" FilterControlAltText="Filter Technics_Line column" HeaderText="工艺路线" SortExpression="Technics_Line" UniqueName="Technics_Line">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Unit" FilterControlAltText="Filter Mat_Unit column" HeaderText="计量单位" SortExpression="Mat_Unit" UniqueName="Mat_Unit">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Rough_Weight" itemstyle-HorizontalAlign="Right" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Mat_Pro_Weight" itemstyle-HorizontalAlign="Right" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Rough_Size" FilterControlAltText="Filter Rough_Size column" HeaderText="物资尺寸" SortExpression="Rough_Size" UniqueName="Rough_Size">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DemandNumSum" itemstyle-HorizontalAlign="Right" HeaderText="共计需求数量" SortExpression="DemandNumSum" UniqueName="DemandNumSum">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NumCasesSum" itemstyle-HorizontalAlign="Right" HeaderText="共计需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum">
                            </telerik:GridBoundColumn>
                      
                       
                        </Columns>
                        	<CommandItemTemplate>
                            
					 	    <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click" CssClass="floatright"></telerik:RadButton>
	                        <telerik:RadButton ID="RadButton_ExportWord"  runat="server" Text="导出Word"  Font-Bold="true" CommandName="ExportWord" Visible="false"  OnClick="RadButton_ExportWord_Click"  CssClass="floatright"></telerik:RadButton>
	                        <telerik:RadButton ID="RadButton_ExportPDF"   runat="server" Text="导出PDF"   Font-Bold="true" CommandName="ExportPDF" Visible="false"   OnClick="RadButton_ExportPdf_Click"   CssClass="floatright"></telerik:RadButton>
	                        </CommandItemTemplate>

                        </MasterTableView>
                    </telerik:RadGrid>
                   </div>

                <div class="divSiteMap add_divSiteMap" style="clear: both; width: 100%;">
                    <div style="float: left; height: 40px; line-height: 0px;">
                        <h3 style="font-weight: bold">合并后的物资需求清单数据记录</h3>
                        
                    </div>
                    <div style="width: 100%; height: 0px; border: solid #000 1px; margin: 5px 0; clear: both;"></div>
                </div>
                    <div class="divViewPanel">
                    <telerik:RadGrid ID="RadGrid_MDemandCombinelist" runat="server" DataKeyNames="ID" Culture="zh-CN"
                        GroupPanelPosition="Top" AllowMultiRowSelection="False" 
                         AllowPaging="False" PageSize="1" PagerStyle-AlwaysVisible="False"
                        OnItemDataBound="RadGrid_MDemandCombinelist_ItemDataBound" OnNeedDataSource="RadGrid_MDemandCombinelist_NeedDataSource">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="12px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="False" />
                            <Scrolling AllowScroll="False" UseStaticHeaders="True" ScrollHeight="60px"></Scrolling>
                        </ClientSettings>
                        <ExportSettings HideStructureColumns="true" ExportOnlyData="true" />
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID,MaterialDept" PagerStyle-AlwaysVisible="False">
                            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="编号" ReadOnly="True" SortExpression="ID" UniqueName="ID" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="mstate" FilterControlAltText="Filter mstate column" HeaderText="物资状态" SortExpression="mstate" UniqueName="mstate" >
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1"  HeaderStyle-width="100px" ItemStyle-Width="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="材料名称" SortExpression="Material_Name" UniqueName="Material_Name"  Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Mark" FilterControlAltText="Filter Material_Mark column" HeaderText="物资牌号" SortExpression="Material_Marke" UniqueName="Material_Mark"  Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="Drawing_No" FilterControlAltText="Filter Drawing_No column" HeaderText="产品图号" SortExpression="Drawing_No" UniqueName="Drawing_No">
                                   <ItemTemplate>
                                        <telerik:RadTextBox ID="Drawing_No" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="Drawing_No_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode">
                                   <ItemTemplate>
                                        <telerik:RadTextBox ID="TaskCode" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="TaskCode_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                 
                                <telerik:GridBoundColumn DataField="Mat_Unit" FilterControlAltText="Filter Mat_Unit column" HeaderText="计量单位" SortExpression="Mat_Unit" UniqueName="Mat_Unit" Visible="false"
                                     HeaderStyle-width="50px" ItemStyle-Width="100px">
                                </telerik:GridBoundColumn>               
                               <telerik:GridTemplateColumn DataField="Mat_Pro_Weight" FilterControlAltText="Filter Mat_Pro_Weight column" HeaderText="每产品质量" SortExpression="Mat_Pro_Weight" UniqueName="Mat_Pro_Weight">
                                   <ItemTemplate>
                                        <telerik:RadTextBox ID="Mat_Pro_Weight" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="Mat_Mat_Pro_Weight_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>


                                <telerik:GridTemplateColumn DataField="Mat_Rough_Weight" FilterControlAltText="Filter Mat_Rough_Weight column" HeaderText="单件质量" SortExpression="Mat_Rough_Weight" UniqueName="Mat_Rough_Weight">
                                   <ItemTemplate>
                                        <telerik:RadTextBox ID="Mat_Rough_Weight" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="Mat_Rough_Weight_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn DataField="Rough_Size" FilterControlAltText="Filter Rough_Size column" HeaderText="物资尺寸" SortExpression="Rough_Size" UniqueName="Rough_Size">
                                   <ItemTemplate>
                                        <telerik:RadTextBox ID="Rough_Size" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="Rough_Size_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            
                                <telerik:GridBoundColumn DataField="Rough_Spec" FilterControlAltText="Filter Rough_Spec column" HeaderText="物资规格" SortExpression="Rough_Spec" UniqueName="Rough_Spec" Visible="false" >
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="特殊需求" HeaderStyle-HorizontalAlign="Center" UniqueName="Special_Needs" >
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="rtb_SpecialNeeds" runat="server" Width="50" AutoPostBack="true" MaxLength="20" EmptyMessage="无" OnTextChanged="rtb_SpecialNeeds_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                
                                
                                <telerik:GridBoundColumn DataField="MaterialDept" FilterControlAltText="Filter MaterialDept column" HeaderText="领料部门" SortExpression="MaterialDept" UniqueName="MaterialDept" Visible="false"
                                     HeaderStyle-width="40px" ItemStyle-Width="40px">
                                </telerik:GridBoundColumn>
                                

                                <telerik:GridTemplateColumn HeaderText="生产厂家" HeaderStyle-HorizontalAlign="Center" UniqueName="MANUFACTURER" Visible="false" HeaderStyle-width="90px" ItemStyle-Width="90px">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="RTB_MANUFACTURER" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="RTB_MANUFACTURER_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                         
                                <telerik:GridTemplateColumn HeaderText="共计需求数量(kg)" SortExpression="DemandNumSum" UniqueName="DemandNumSum" ItemStyle-HorizontalAlign="Right"  HeaderStyle-width="150px" ItemStyle-Width="150px">
                                     <ItemTemplate>
                                        <telerik:RadTextBox ID="DemandNumSum" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="DemandNumSum_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="NumCasesSum" HeaderText="共计需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum" ItemStyle-HorizontalAlign="Right">
                                       <ItemTemplate>
                                        <telerik:RadTextBox ID="NumCasesSum" runat="server" Width="100px" AutoPostBack="true" OnTextChanged="NumCasesSum_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ParentId_For_Combine" itemstyle-HorizontalAlign="Right"  SortExpression="ParentId_For_Combine" UniqueName="ParentId_For_Combine" Visible="true"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="stage1" FilterControlAltText="Filter stage1 column" HeaderText="研制阶段" SortExpression="stage1" UniqueName="stage1" Visible="false"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PackId" Visible="false" UniqueName="PackId"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaskId" Visible="false" UniqueName="TaskId"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DraftId" Visible="false" UniqueName="DraftId"></telerik:GridBoundColumn>
                            </Columns>
  						
                          </MasterTableView>
                    </telerik:RadGrid>
                      <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from V_Get_Sys_Dept_ShipAddrByDeptID"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceUrgencyDegre" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceSecretLevel" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceUseDes" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE'"></asp:SqlDataSource>
                    <telerik:RadButton ID="RB_Submit" runat="server" Text="确认合并物资记录" OnClick="RB_Submit_Click" OnClientClicking="confirmRadWindow"></telerik:RadButton>

                </div>


            </div>
        </div>
        <telerik:RadWindow ID="RadWindow" runat="server" VisibleTitlebar="false"
            VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
            <ContentTemplate>
                <div style="margin-top: 30px; float: left;">
                    <div style="width: 60px; padding-left: 15px; float: left;">
                        <img src="../Images/images/warnning1.jpg" alt="" />
                    </div>
                    <div style="width: 200px; float: left;">
                        <asp:Label ID="Label2" Font-Size="14px" Text="确定要合并吗？" runat="server" Font-Bold="true"
                            ForeColor="#25a0da" />
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="RadButton3" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                        &nbsp;
                        <telerik:RadButton ID="RadButton4" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </telerik:RadButton>
                    </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center"
            AutoCloseDelay="4000" Width="240" Title="提示" EnableRoundedCorners="true">
        </telerik:RadNotification>
    </form>
</body>
</html>
