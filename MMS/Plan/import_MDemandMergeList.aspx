﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="import_MDemandMergeList.aspx.cs" Inherits="mms.Plan.import_MDemandMergeList" %>

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
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" Skin="Silk"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
          	  	  	    <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
           			        <UpdatedControls>
                	    <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandMergelist" LoadingPanelID="RadAjaxLoadingPanel1" />
           				    </UpdatedControls>
          			    </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid_MDemandMergelist">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandMergelist" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadBtn_Search">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandMergelist" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RB_Submit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_MDemandMergelist" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadNotificationAlert" />
                        <telerik:AjaxUpdatedControl ControlID="RB_Submit" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"  MinDisplayTime="0"></telerik:RadAjaxLoadingPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">

                function EnterKeyProcessing(sender, eventArgs) {
                    var c = eventArgs.get_keyCode();
                    if ((c == 13)) {
                        eventArgs.set_cancel(true);
                    }
                }

                function CloseWindow(sender,args){
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
        <div style="width: 100%; float: left;">
            <div class="divContant" style="margin-top: 0px;">
                <div class="divSiteMap" style="width: 100%; float: none; height: 30px; border-bottom-style: solid; border-bottom-width: 0px;">
                    <label style="margin-left: 10px; float: left;color:red;">型号：</label><span id="span_model" style="float: left;color:red;" runat="server"></span>
                    <label style="margin-left: 50px; float: left">基准物资材料清单号：</label><span id="span_listNo" style="float: left;" runat="server"></span>
                  
    
                </div>
                <div class="divViewPanel">
                <telerik:RadGrid ID="RadGrid_MDemandMergelist" runat="server" Culture="zh-CN" GroupPanelPosition="Top"
                    OnNeedDataSource="RadGrid_MDemandMergelist_NeedDataSource" OnItemDataBound="RadGrid_MDemandMergelist_ItemDataBound" 
                AllowPaging="true" PageSize="15" PagerStyle-AlwaysVisible="True" AllowSorting="true" AllowMultiRowSelection="True" AutoGenerateColumns="False">
                <AlternatingItemStyle HorizontalAlign="Center" />
                <ItemStyle Font-Size="12px" HorizontalAlign="Center" />
                <HeaderStyle Font-Size="13px" HorizontalAlign="Center"/>
                <CommandItemStyle Font-Bold="true" Font-Size="16px" HorizontalAlign="Center" Height="40px" />
                <ClientSettings Selecting-AllowRowSelect="true"  EnablePostBackOnRowClick="true" EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                </ClientSettings>
                <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" OpenInNewWindow="true">
                       <Pdf  DefaultFontFamily="Arial Unicode MS" />
                </ExportSettings>
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID,MaterialDept" PagerStyle-AlwaysVisible="true">
                            <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" DataType="System.Int32" FilterControlAltText="Filter ID column" HeaderText="编号" ReadOnly="True" SortExpression="ID" UniqueName="ID"
                                    HeaderStyle-width="80px" ItemStyle-Width="80px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="任务号" SortExpression="TaskCode" UniqueName="TaskCode"
                                     HeaderStyle-width="100px" ItemStyle-Width="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Drawing_No" FilterControlAltText="Filter Drawing_No column" HeaderText="图号" SortExpression="Drawing_No" UniqueName="Drawing_No"
                                    HeaderStyle-width="100px" ItemStyle-Width="100px">
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="TDM_Description" FilterControlAltText="Filter TDM_Description column" HeaderText="产品名称" SortExpression="TDM_Description" UniqueName="TDM_Description"
                                     HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                 </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemCode1" FilterControlAltText="Filter ItemCode1 column" HeaderText="物资编码" SortExpression="ItemCode1" UniqueName="ItemCode1"
                                     HeaderStyle-width="100px" ItemStyle-Width="100px">
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="Material_Name" FilterControlAltText="Filter Material_Name column" HeaderText="物资名称" SortExpression="Material_Name" UniqueName="Material_Name"  
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                 </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Mat_Unit" FilterControlAltText="Filter Mat_Unit column" HeaderText="计量单位" SortExpression="Mat_Unit" UniqueName="Mat_Unit"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Dinge_Size" FilterControlAltText="Filter Rough_Size column" HeaderText="胚料尺寸" SortExpression="Dinge_Size" UniqueName="Dinge_Size"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Spec" FilterControlAltText="Filter Rough_Spec column" HeaderText="物资规格" SortExpression="Rough_Spec" UniqueName="Rough_Spec"
                                     HeaderStyle-width="100px" ItemStyle-Width="100px">
                                </telerik:GridBoundColumn>

                                 <telerik:GridBoundColumn DataField="Material_Mark" FilterControlAltText="Filter Material_Mark column" HeaderText="物资牌号" SortExpression="Material_Mark" UniqueName="Material_Mark"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                </telerik:GridBoundColumn>
                                  <telerik:GridBoundColumn DataField="Material_Tech_Condition" FilterControlAltText="Filter Material_Tech_Condition column" HeaderText="技术条件" SortExpression="Material_Tech_Condition" UniqueName="Material_Tech_Condition"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="特殊需求"  UniqueName="Special_Needs"
                                     HeaderStyle-width="80px" ItemStyle-Width="80px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="rtb_SpecialNeeds" runat="server" Width="50" AutoPostBack="true" MaxLength="50" Value="无" EmptyMessage="无" OnTextChanged="rtb_SpecialNeeds_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="紧急程度"  UniqueName="Urgency_Degre" HeaderStyle-width="70px" ItemStyle-Width="70px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxUrgencyDegre" runat="server" Width="60px" AutoPostBack="true"
                                            OnSelectedIndexChanged="RadComboBoxUrgencyDegre_SelectedIndexChanged"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceUrgencyDegre" DataTextField="DICT_Name" DataValueField="DICT_Code">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="密级"  UniqueName="Secret_Level" HeaderStyle-width="70px" ItemStyle-Width="70px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxSecretLevel" runat="server" Width="60px" AutoPostBack="true"
                                            OnSelectedIndexChanged="RadComboBoxSecretLevel_SelectedIndexChanged"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceSecretLevel" DataTextField="SecretLevel_Name" DataValueField="SecretLevel_Name">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="用途" HeaderStyle-HorizontalAlign="Center" UniqueName="Use_Des"  HeaderStyle-width="90px" ItemStyle-Width="90px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxUseDes" runat="server" Width="80px" AutoPostBack="true"
                                            OnSelectedIndexChanged="RadComboBoxUseDes_SelectedIndexChanged"
                                            Culture="zh-CN" DataSourceID="SqlDataSourceUseDes" DataTextField="DICT_Name" DataValueField="DICT_Code">
                                            <Items>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                             
                                <telerik:GridBoundColumn DataField="MaterialDept" FilterControlAltText="Filter MaterialDept column" HeaderText="领用单位" SortExpression="MaterialDept" UniqueName="MaterialDept"
                                     HeaderStyle-width="70px" ItemStyle-Width="70px">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="配送地址"  UniqueName="Shipping_Address"
                                     HeaderStyle-width="110px" ItemStyle-Width="110px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxShippingAddress" runat="server" Width="100px" Culture="zh-CN"
                                            AutoPostBack="true" OnSelectedIndexChanged="RadComboBoxShippingAddress_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="合格证"  UniqueName="Certification"  HeaderStyle-width="60px" ItemStyle-Width="60px">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="RadComboBoxCertification" runat="server" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="RadComboBoxCertification_SelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y" />
                                                <telerik:RadComboBoxItem Text="N" Value="N" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="生产厂家"  UniqueName="MANUFACTURER" HeaderStyle-width="90px" ItemStyle-Width="90px">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="RTB_MANUFACTURER" runat="server" Width="80px" AutoPostBack="true" OnTextChanged="RTB_MANUFACTURER_TextChanged"></telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="需求数量" SortExpression="DemandNumSum" UniqueName="DemandNumSum" 
                                     HeaderStyle-width="80px" ItemStyle-Width="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="需求件数" SortExpression="NumCasesSum" UniqueName="NumCasesSum" 
                                     HeaderStyle-width="80px" ItemStyle-Width="60px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Rough_Size"  HeaderText="需求尺寸" SortExpression="Rough_Size" UniqueName="Rough_Size"
                                     HeaderStyle-width="100px" ItemStyle-Width="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="stage1" HeaderText="研制阶段" SortExpression="stage1" UniqueName="stage1"  HeaderStyle-width="80px"
                                     ItemStyle-Width="60px">
                                </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="DemandDate" HeaderText="需求日期"  SortExpression="DemandDate" UniqueName="DemandDate" HeaderStyle-Width="100px" 
                                   ItemStyle-Width="100px">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="PackId" Visible="false" UniqueName="PackId"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TaskId" Visible="false" UniqueName="TaskId"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DraftId" Visible="false" UniqueName="DraftId"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Stage" Visible="false" UniqueName="Stage"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Unit_Price" FilterControlAltText="Filter Unit_Price column" HeaderText="单价" SortExpression="Unit_Price" UniqueName="Unit_Price" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Sum_Price" FilterControlAltText="Filter Sum_Price column" HeaderText="总价" SortExpression="Sum_Price" UniqueName="Sum_Price" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Is_Save" FilterControlAltText="Filter Is_Save column" Visible="false" SortExpression="Is_Save" UniqueName="Is_Save">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Is_Submit" FilterControlAltText="Filter Is_Submit column" Visible="false" SortExpression="Is_Submit" UniqueName="Is_Submit">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MDMId" FilterControlAltText="Filter MDMId column" Visible="false" SortExpression="MDMId" UniqueName="MDMId">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <%--<CommandItemTemplate>
                        <telerik:RadButton ID="RadButton_BatchSubmit" runat="server" Text="提交物流清单" Font-Bold="true" CommandName="BatchSubmit"></telerik:RadButton>
                        <telerik:RadButton ID="RadButton_ExportExcel" runat="server" Text="导出Excel" Font-Bold="true" CommandName="ExportExcel" OnClick="RadButton_ExportExcel_Click"></telerik:RadButton>
                    </CommandItemTemplate>--%>
                        </MasterTableView>
                    </telerik:RadGrid>
                  
                    <%--<telerik:RadWindow ID="confirmWindow" runat="server" VisibleTitlebar="false"
                VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                <ContentTemplate>
                    <div style="margin-top: 30px; float: left;">
                        <div style="width: 60px; padding-left: 15px; float: left;">
                            <img src="../Images/images/warnning1.jpg" alt="" />
                        </div>
                        <div style="width: 200px; float: left;">
                            <asp:Label ID="Label1" Font-Size="14px" Text="提交成功" runat="server" Font-Bold="true"
                                ForeColor="#25a0da" />
                            <br />
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <telerik:RadButton ID="RadButton1" runat="server" Text="关闭" AutoPostBack="false" OnClientClicked="YesOrNoClicked">
                                    <Icon PrimaryIconCssClass="rbOk" />
                                </telerik:RadButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>--%>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from V_Get_Sys_Dept_ShipAddrByDeptID"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceUrgencyDegre" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_URGENCY_LEVEL'"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceSecretLevel" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="SELECT * FROM [Sys_SecretLevel] WHERE ([Is_Del] = 0)"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataSourceUseDes" runat="server" ConnectionString='<%$ ConnectionStrings:MaterialManagerSystemConnectionString %>'
                        SelectCommand="select * from GetBasicdata_T_Item where DICT_CLASS='CUX_DM_USAGE'and ENABLED_FLAG = 'Y'"></asp:SqlDataSource>
                    <telerik:RadButton ID="RB_Submit" runat="server" Text="提交物流中心" OnClick="RB_Submit_Click" OnClientClicking="confirmRadWindow"></telerik:RadButton>
                </div>

                <div class="divSiteMap add_divSiteMap" style="clear: both; width: 100%;" runat="server" id="divListTitle">
                    <div style="float: left; height: 40px; line-height: 40px;">
                        <h3 style="font-weight: bold">需求变更申请单</h3>
                    </div>
                </div>
                <div style="width: 100%; float: left; margin-top: 10px;" runat="server" id="divListContent">
                    <telerik:RadGrid ID="RadGrid_ChangeRecord" runat="server" AllowPaging="True" PageSize="15" PagerStyle-AlwaysVisible="true" 
                        Culture="zh-CN" GroupPanelPosition="Top" 
                        OnNeedDataSource="RadGrid_ChangeRecord_NeedDataSource">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="13px" />
                        <ClientSettings EnableRowHoverStyle="true" >
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="100px"></Scrolling>
                        </ClientSettings>
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="MDMLID">
                            <Columns>
                                <telerik:GridBoundColumn DataField="RowsId" DataType="System.Int32" FilterControlAltText="Filter RowsId column" HeaderText="序号" ReadOnly="True" SortExpression="RowsId" UniqueName="RowsId">
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="MDDLDID" HeaderText="对应编号">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MDMLID" HeaderText="原需求行号">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ItemCode" HeaderText="物资编码">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Material_Name" HeaderText="物资名称">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridBoundColumn DataField="Change_Code" FilterControlAltText="Filter Change_Code column" HeaderText="变更单据" SortExpression="Change_Code" UniqueName="Change_Code">
                        </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="ChangeList_Code" HeaderText="变更凭据">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Change_Date" HeaderText="变更时间" >
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="zt" HeaderText="变更属性">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumCasesSum" HeaderText="原需求件数" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NNumCasesSum" HeaderText="新需求件数" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DNumCasesSum" HeaderText="减少" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DemandNumSum" HeaderText="原需求量" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NDemandNumSum" HeaderText="新需求量" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DDemandNumSum" HeaderText="减少" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                <%--提交物流中心后的状态--弹窗--开始--%>
                <%--<telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowState" runat="server" Title="提交物流中心状态" Left="150px"
                            ReloadOnShow="true" ShowContentDuringLoad="false" VisibleTitlebar="true" VisibleStatusbar="false"
                            Behaviors="Close" Modal="true" Width="1400px" Height="660px" />
                    </Windows>
                </telerik:RadWindowManager>--%>
                <%--提交物流中心后的状态--弹窗--结束--%>
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
                        <asp:Label ID="Label2" Font-Size="14px" Text="确定要提交吗？" runat="server" Font-Bold="true"
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
            AutoCloseDelay="4000" Width="240" Title="提示" OnClientHidden="CloseWindow" EnableRoundedCorners="true">
        </telerik:RadNotification>
    </form>
</body>
</html>
