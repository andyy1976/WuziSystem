<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialAppWindow.aspx.cs" Inherits="mms.MaterialApplicationCollar.MaterialAppWindow" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Styles/Plan.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <style type="text/css">
        .CommonSymbols {
            color: blue;
            cursor: pointer;
            padding: 4px;
        }

        #table1 tr {
            height: 26px;
        }

        #table2 tr {
            height: 30px;
            padding: 4px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            </telerik:RadAjaxManager>
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    function CloseWindow(args) {
                        var oWindow = null;
                        if (window.radWindow) oWindow = window.radWindow;
                        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                        var oArg = new Object();
                        oWindow.BrowserWindow.refreshGrid(args);
                        oWindow.close(oArg);
                    }

                    function OnClientHidden(sender, args) {
                        CloseWindow();
                    }

                    $(document).ready(function ()
                    {
                        $(".CommonSymbols").click(function ()
                        {
                            var a = $(this).text();
                            var remark = $("#<%=RTB_Remark.ClientID%>").text();
                            remark += a;
                            $("#<%=RTB_Remark.ClientID%>").text(remark);
                        $("#<%=RTB_Remark.ClientID%>").focus();
                        });
                    });

                    function ShowRadWindowSubmit(sender, args)
                    {
                        if ($find("<%=RTB_Applicant.ClientID%>")._text == "")
                        {
                            $find("<%=RadNotificationAlert.ClientID%>").set_text("请输入申请人！");
                            $find("<%=RadNotificationAlert.ClientID%>").show();
                            args.set_cancel(true);
                            return;
                         }
                        if ($find("<%=RTB_Quantity.ClientID%>")._text == "")
                        {
                            $find("<%=RadNotificationAlert.ClientID%>").set_text("请输入申请数量！");
                            $find("<%=RadNotificationAlert.ClientID%>").show();
                            args.set_cancel(true);
                            return;
                        }

                        $find("<%= RadWindowSubmit.ClientID %>").show();
                        args.set_cancel(true);
                   }

                   function YesOrNoClickedSubmit(sender, args)
                   {
                        var oWnd = $find("<%=RadWindowSubmit.ClientID %>");
                        oWnd.close();
                        if (sender.get_text() == "是") {
                            $find("<%=RB_Submit.ClientID%>").click();
                        }
                   }
                </script>
            </telerik:RadCodeBlock>
            <asp:HiddenField ID="HFType" runat="server" />
            <asp:HiddenField ID="HFMDMLID" runat="server" />
            <asp:HiddenField runat="server" id="HFMDPLID"/>
            <asp:HiddenField ID="HFMAID" runat="server" />

            <table id="table2" style="margin: 0px auto; text-align: left; font-size: 13px;">
                <tr>
                    <th colspan="6" style="text-align: center;">
                        <asp:Label ID="lbltitle" runat="server" Font-Size="22px"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc; padding-top: 30px;">申请人信息
                        <span style="float: right;">申请部门：
                        <asp:HiddenField ID="HF_DeptCode" runat="server" />
                            <asp:Label ID="lbl_Dept" runat="server"></asp:Label></span>
                    </th>
                </tr>
                <tr>
                    <td style="width: 140px; text-align: right;">申请人：</td>
                    <td style="width: 160px;">
                        <telerik:RadTextBox ID="RTB_Applicant" runat="server" Width="120px"  Enabled="false"></telerik:RadTextBox></td>
                    <td style="text-align: right; width: 100px;">申请时间：</td>
                    <td style="width: 160px;">
                        <telerik:RadDatePicker ID="RDP_ApplicationTime" runat="server" Width="120px"></telerik:RadDatePicker>
                    </td>
                    <td style="text-align: right; width: 100px;">联系方式：</td>
                    <td style="width: 160px;">
                        <telerik:RadTextBox ID="RTB_ContactInformation" runat="server" Width="120px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc;">请领信息</th>
                </tr>
                <tr>
                    <td style="text-align: right;">任务号：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_TaskCode" runat="server"></telerik:RadTextBox>
                    </td>
                    <td style="text-align: right;">图号：</td>
                    <td>
                        <asp:Label ID="lbl_DrawingNo" runat="server"></asp:Label></td>
                    <td style="text-align: right;">申请件数：</td>
                    <td>
                        <telerik:RadTextBox ID="RTB_Quantity" runat="server" Width="120px" Enabled="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">领料方式：</td>
                    <td>
                        <telerik:RadDropDownList ID="RDDL_TheMaterialWay" runat="server" Width="120px">
                            <Items>
                                <telerik:DropDownListItem Value="0" Text="单件领料" />
                                <telerik:DropDownListItem Value="1" Text="成组领料" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td style="text-align: right;">要求供料时间：</td>
                    <td>
                        <telerik:RadDatePicker ID="RDP_FeedingTime" runat="server" Width="120px"></telerik:RadDatePicker>
                    </td>
                    <td style="text-align: right;">申请数量(单件定额质量*申请件数）：</td>
                    <td><telerik:RadTextBox ID="RTB_PleaseTakeQuality" runat="server" Width="120px" Enabled="true"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right;">备注：</td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="RTB_Remark" runat="server" TextMode="MultiLine" Rows="3" Columns="70"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadButton ID="RB_IsDispatch" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" Text="急件" AutoPostBack="false"></telerik:RadButton><br />
                        <telerik:RadButton ID="RB_IsConfirm" runat="server" ButtonType="ToggleButton" ToggleType="CheckBox" Text="确认调拨" Checked="true" AutoPostBack="false"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">常用符号：</td>
                    <td colspan="5">
                        <label id="lbl" class="CommonSymbols">Ⅰ</label><label class="CommonSymbols">Ⅱ</label><label class="CommonSymbols">Ⅲ</label><label class="CommonSymbols">Ⅳ</label><label class="CommonSymbols">Ⅴ</label>
                        <label class="CommonSymbols">Ⅵ</label><label class="CommonSymbols">Ⅶ</label><label class="CommonSymbols">Ⅷ</label><label class="CommonSymbols">Ⅸ</label><label class="CommonSymbols">Ⅹ</label>
                        <label class="CommonSymbols">Ⅺ</label><label class="CommonSymbols">Ⅻ</label><label class="CommonSymbols">Δ</label><label class="CommonSymbols">Π</label><label class="CommonSymbols">Σ</label>
                        <label class="CommonSymbols">Y</label><label class="CommonSymbols">Φ</label><label class="CommonSymbols">Ψ</label><label class="CommonSymbols">Ω</label><label class="CommonSymbols">α</label>
                        <label class="CommonSymbols">β</label><label class="CommonSymbols">γ</label><label class="CommonSymbols">δ</label><label class="CommonSymbols">ε</label><label class="CommonSymbols">ζ</label>
                        <label class="CommonSymbols">η</label><label class="CommonSymbols">θ</label><label class="CommonSymbols">ι</label><label class="CommonSymbols">κ</label><label class="CommonSymbols">λ</label><br />
                        <label class="CommonSymbols">μ</label><label class="CommonSymbols">ν</label><label class="CommonSymbols">ξ</label><label class="CommonSymbols">ο</label><label class="CommonSymbols">π</label>
                        <label class="CommonSymbols">ρ</label><label class="CommonSymbols">σ</label><label class="CommonSymbols">τ</label><label class="CommonSymbols">υ</label><label class="CommonSymbols">φ</label>
                        <label class="CommonSymbols">χ</label><label class="CommonSymbols">ψ</label><label class="CommonSymbols">ω</label><label class="CommonSymbols">￡</label><label class="CommonSymbols">￠</label>
                        <label class="CommonSymbols">￥</label><label class="CommonSymbols">∝</label><label class="CommonSymbols">∫</label><label class="CommonSymbols">∞</label><label class="CommonSymbols">∩</label>
                        <label class="CommonSymbols">∪</label><label class="CommonSymbols">≠</label><label class="CommonSymbols">÷</label><label class="CommonSymbols">±</label><label class="CommonSymbols">‰</label>
                        <label class="CommonSymbols">℃</label><label class="CommonSymbols">⊙</label>
                    </td>
                </tr>
                <tr>
                    <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc;">物资信息</th>
                </tr>
                <tr>
                    <td style="text-align: right;">物资编码：</td>
                    <td>
                        <asp:Label ID="lbl_ItemCode" runat="server"></asp:Label></td>

                    <td style="text-align: right;">物资名称：</td>
                    <td>
                        <asp:Label ID="lbl_Material_Name" runat="server"></asp:Label></td>
                    <td style="text-align: right;">物资牌号：</td>
                    <td>
                        <asp:Label ID="lbl_Material_Mark" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;">供应状态：</td>
                    <td>
                        <asp:Label ID="lbl_CN_Material_State" runat="server"></asp:Label></td>
                    <td style="text-align: right;">采用标准：</td>
                    <td>
                        <asp:Label ID="lbl_Material_Tech_Condition" runat="server"></asp:Label></td>

                    <td style="text-align: right;">胚料规格：</td>
                    <td>
                        <asp:Label ID="lbl_Rough_Spec" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;">单个零件尺寸：</td>
                    <td><asp:Label ID="lbl_Rough_Size" runat="server"></asp:Label></td>

                    <td style="text-align: right;">计量单位：</td>
                    <td><asp:Label ID="lbl_Mat_Unit" runat="server"></asp:Label></td>

                    <td style="text-align: right;">单件定额质量：</td>
                    <td><asp:Label ID="lbl_Mat_Rough_Weight" runat="server">1</asp:Label></td>
                   
                </tr>
                <tr>
                    <th colspan="6" style="text-align: left; font-size: 14px; border-bottom: solid 1px #ccc;">业务审批流程</th>
                </tr>
                <tr>
                    <td style="text-align: right;">1.车间调度员：</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="RDDL_DiaoDu" Width="120px"></telerik:RadDropDownList>
                    </td>
                    <td style="text-align: right;">2.型号计划员：</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="RDDL_XingHao" Width="120px"></telerik:RadDropDownList>
                    </td>
                    <td style="text-align: right;">3.物资计划员：</td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="RDDL_WuZi" Width="120px"></telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center;">
                        <table style="text-align: center; margin: 0px auto; width: 300px;">
                            <tr>
                                <td style="width: 100px;">
                                    <telerik:RadButton ID="RB_Submit" runat="server" Text="提交" OnClick="RB_Submit_Click" OnClientClicking="ShowRadWindowSubmit"></telerik:RadButton>
                                </td>
                                <td style="width: 100px;">
                                    <telerik:RadButton ID="RB_Close" runat="server" Text="取消" AutoPostBack="false" OnClientClicking="CloseWindow"></telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
  
            <%-- 提交弹出窗口--开始--%>
            <telerik:RadWindow ID="RadWindowSubmit" runat="server" VisibleTitlebar="false"
                VisibleStatusbar="false" Modal="true" Behaviors="None" Height="120px" Width="320px">
                <ContentTemplate>
                    <div style="margin-top: 30px; float: left;">
                        <div style="width: 60px; padding-left: 15px; float: left;">
                            <img src="../Images/images/warnning1.jpg" alt="" />
                        </div>
                        <div style="width: 200px; float: left;">
                            <asp:Label ID="Label1" Font-Size="14px" Text="确定要提交吗？" runat="server" Font-Bold="true"
                                ForeColor="#25a0da" />
                            <br />
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="RadButton1" runat="server" Text="是" AutoPostBack="false" OnClientClicked="YesOrNoClickedSubmit">
                            <Icon PrimaryIconCssClass="rbOk" />
                        </telerik:RadButton>
                            &nbsp;
                        <telerik:RadButton ID="RadButton2" runat="server" Text="否" AutoPostBack="false" OnClientClicked="YesOrNoClickedSubmit">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </telerik:RadButton>

                            
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

                    
            <%-- 提交弹出窗口--结束--%>
            <telerik:RadNotification ID="RadNotificationAlert" runat="server" Text="" Position="Center" 
                AutoCloseDelay="4000" Width="300" Title="提示"   EnableRoundedCorners="true" LoadContentOn="EveryShow">
            </telerik:RadNotification>

           <telerik:RadNotification ID="RadNotificationAlert1" runat="server" Text="" Position="Center" 
                AutoCloseDelay="2000" Width="300" Title="提示" OnClientHidden="OnClientHidden"  EnableRoundedCorners="true">
            </telerik:RadNotification>
        </div>
    </form>
</body>
</html>
