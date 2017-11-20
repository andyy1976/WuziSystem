<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryItemCode.aspx.cs" Inherits="mms.Plan.QueryItemCode" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" rel="stylesheet" href="../Styles/Plan.css" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RDDLMT">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="div1" />
                    <telerik:AjaxUpdatedControl ControlID="div2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RDDLMT1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RDDLMT2" />
                    <telerik:AjaxUpdatedControl ControlID="RDDLMT3" />
                    <telerik:AjaxUpdatedControl ControlID="RDDLMT4" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RDDLMT2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RDDLMT3" />
                    <telerik:AjaxUpdatedControl ControlID="RDDLMT4" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RDDLMT3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RDDLMT4" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RB_Search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadButton1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"></telerik:RadCodeBlock>
    <div style="width: 100%;">
        <table id="tableMT" style="margin:0px auto; width: 100%;">
            <tr>
                <th colspan="10" style="font-size:16px; letter-spacing:16px;">物资基础库查询</th>
            </tr>
            <tr>
                <td style="width:80px">物资名称：</td>

                <td><telerik:RadTextBox ID="RTB_Material_Name" runat="server" OnKeyPress="AlphabetOnly" Width="140px"></telerik:RadTextBox></td>

                    <td style="width:80px">物资牌号：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Paihao" runat="server" OnKeyPress="AlphabetOnly"  Width="140px"></telerik:RadTextBox></td>

                    <td style="width:80px">物资规格:</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Guige" runat="server" OnKeyPress="AlphabetOnly" Width="140px"></telerik:RadTextBox></td>

                    <td style="width:80px">物资标准：</td>
                    <td><telerik:RadTextBox ID="RTB_Material_Biaozhun" runat="server" OnKeyPress="AlphabetOnly" Width="140px"></telerik:RadTextBox></td>
                 <td style="width:200px" colspan="2"><telerik:RadButton ID="RB_Search" runat="server" Text="搜索" OnClick="RB_Search_Click"></telerik:RadButton></td>
                </tr>
               <tr>
                <td style="width:100px">物资类别(一级)：</td>
                <td>
                    <telerik:RadDropDownList ID="RDDLMT" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="RDDLMT_SelectedIndexChanged" AutoPostBack="true" Width="140px">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="" />
                        </Items>
                    </telerik:RadDropDownList>
                </td>
               
                <td colspan="8">
                    <div id="div1" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>二级</td>
                                <td>
                                    <telerik:RadDropDownList ID="RDDLMT1" runat="server" OnSelectedIndexChanged="RDDLMT1_SelectedIndexChanged" AutoPostBack="true" Width="140px">
                                    </telerik:RadDropDownList>
                                </td>
                                <td>三级</td>
                                <td>
                                    <telerik:RadDropDownList ID="RDDLMT2" runat="server" OnSelectedIndexChanged="RDDLMT2_SelectedIndexChanged" AutoPostBack="true" Width="140px">
                                    </telerik:RadDropDownList>
                                </td>
                                <td>四级</td>
                                <td>
                                    <telerik:RadDropDownList ID="RDDLMT3" runat="server" OnSelectedIndexChanged="RDDLMT3_SelectedIndexChanged" AutoPostBack="true" Width="140px">
                                    </telerik:RadDropDownList>
                                </td>
                                <td>五级</td>
                                <td>
                                    <telerik:RadDropDownList ID="RDDLMT4" runat="server" Width="140px">
                                    </telerik:RadDropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div2" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>物资编码：</td>
                                <td><telerik:RadTextBox ID="RTB_ItemCode" runat="server"></telerik:RadTextBox></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%;">
        <telerik:RadGrid ID="RadGrid1" runat="server" Width="1000px" OnNeedDataSource="RadGrid1_NeedDataSource" AutoGenerateColumns="false" AllowPaging="true" PageSize="20">
            <PagerStyle AlwaysVisible="true" />
            <ClientSettings Selecting-AllowRowSelect="true">
                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
            </ClientSettings>
            <MasterTableView>
                <Columns>
                    <telerik:GridBoundColumn DataField="SEG3" HeaderText="物资编码" ItemStyle-Width="120px" HeaderStyle-Width="120px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SEG4" HeaderText="描述" ItemStyle-Width="800px" HeaderStyle-width="800px"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    </form>
</body>
</html>
