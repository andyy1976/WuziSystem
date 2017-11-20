<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WUCBusiNav1.ascx.cs" Inherits="mms.UserControls.WUCBusiNav1" %>
<%--<style>
    .zt_3 {
        color:green; font-weight:bold; 
    }
    .zt_1 {
        color:red; font-weight:bold;
    }
    .zt_2 {
        color:blue; font-weight:bold;
    }
    .span_1 {
        margin-left:15px;color:black;
    }
    .span_0 {
        color:black;
    }
</style>--%>

<div style="width:100%;">
    <div id="div1">
        <div id="iwucDiv1" class="iwucDiv">
                    <div id="divMenu1"><a href="/Plan/MDemandDetails.aspx?inFlag=1" id="a1">需要提交</a></div>
            <%--<div style="font-size:12px;margin-top:2px;" id="divNum">
                <span class="span_0">未提交：</span><asp:Label ID="lbl_state_0" runat="server" Text="0" CssClass="zt_1"></asp:Label>
                <span class="span_1">已提交：</span><asp:Label ID="lbl_state_1" runat="server" Text="0" CssClass="zt_2"></asp:Label>
                <span class="span_1">有更改可再提交：</span><asp:Label ID="lbl_state_2" runat="server" Text="0" CssClass="zt_3"></asp:Label>
                <span class="span_1">缺失材料定额：</span><asp:Label ID="lbl_state_4" runat="server" Text="0" CssClass="zt_1"></asp:Label>
                <span class="span_1">错误数据：</span><asp:Label ID="lbl_state_5" runat="server" Text="0" CssClass="zt_2"></asp:Label>
                <span class="span_1">有更改不可再提交：</span><asp:Label ID="lbl_state_6" runat="server" Text="0" CssClass="zt_3"></asp:Label>
            </div>--%>
        </div>
    </div>
    <div style=" float:left;margin-top:16px;"><img src="/Images/images/jt.png" style="border:0px; width: 22px;"/></div>
    <div id="div2" class="class_add2">
        <div id="iwucDiv2" class="iwucDiv">
            <div style="margin-top:9px;" id="divMenu2"><a href="/Plan/MDemandPlan.aspx?inFlag=2" id="a2">已提交清单</a></div>
            <%--<div style="font-size:12px;margin-top:2px;" id="div_list">
                <a class="span_0" id="a_ListNav" href="../Plan/MDemandPlan.aspx?inFlag=1">已提交清单列表</a>
                <a class="span_1" id="a_Details" href="../Plan/MDemandPlanList.aspx?inFlag=2">已提交清单详细信息</a>
            </div>--%>
        </div>
    </div>
    <script type="text/javascript">

        function showFontStyle() {
            var flag = '<%=Request.QueryString["inFlag"]%>';
            flag = flag == "" ? 1 : flag;
            document.getElementById("a1").className = "borderLC3";
            document.getElementById("a2").className = "borderLC3";
            document.getElementById("div1").className = "borderLC9";
            document.getElementById("div2").className = "borderLC7";
            if (flag == "1") {
                document.getElementById("a1").className = "borderLC4";
                document.getElementById("div1").className = "borderLC10";
                $("#divMenu2").css("margin-top", "9px");
                $("#divNum").show();
            }
            else if (flag == "2") {
                document.getElementById("a2").className = "borderLC4";
                document.getElementById("div2").className = "borderLC8";
                $("#divNum").hide();
            }
        }
        function getMaterialStateSum(jzwzcode) {
            $.post("../AjaxCode/GetMaterialStateSum.ashx", { "jzwzcode": jzwzcode }, function (data, backstatus) {
                if (backstatus == "success") {
                    $(data).find("Proc_Sel_Material_State_Sum").each(function () {
                        var zt0 = $(this).find("zt0").text();
                        var zt1 = $(this).find("zt1").text();
                        var zt2 = $(this).find("zt2").text();
                        var zt4 = $(this).find("zt4").text();
                        var zt5 = $(this).find("zt5").text();
                        var zt6 = $(this).find("zt6").text();
                        $("#ContentPlaceHolder1_WUCBusiNav1_lbl_state_0").html(zt0);
                        $("#ContentPlaceHolder1_WUCBusiNav1_lbl_state_1").html(zt1);
                        $("#ContentPlaceHolder1_WUCBusiNav1_lbl_state_2").html(zt2);
                        $("#ContentPlaceHolder1_WUCBusiNav1_lbl_state_4").html(zt4);
                        $("#ContentPlaceHolder1_WUCBusiNav1_lbl_state_5").html(zt5);
                        $("#ContentPlaceHolder1_WUCBusiNav1_lbl_state_6").html(zt6);
                    })
                }
            })
        }
        $(function () {
            showFontStyle();
            var jzwzcode = '<%=Request.QueryString["DraftCode"]%>';
            //if (jzwzcode != "" && typeof (jzwzcode) != "undefined") {
            //    getMaterialStateSum(jzwzcode);
            //}
        })
    </script> 
</div>
