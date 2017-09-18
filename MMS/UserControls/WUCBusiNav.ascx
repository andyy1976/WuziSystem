<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WUCBusiNav.ascx.cs" Inherits="mms.UserControls.WUCBusiNav" %>

<style>
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
</style>
<div style="width:100%; margin-left:15px;">
    <div id="div1">
        <div id="iwucDiv1" class="iwucDiv">
            <div style="margin-top:18px;" id="divMenu1"><a href="/Plan/PlanImport.aspx?inFlag=1" id="a1">导入计划包</a></div>
        </div>
    </div>
    <div style=" float:left;margin-top:16px;"><img src="/Images/images/jt.png" style="border:0px; width: 22px;"/></div>
    <div id="div2">
        <div id="iwucDiv2" class="iwucDiv">
            <div style="margin-top:9px;" id="divMenu2"><a href="/Plan/SynchronSmarTeam.aspx?inFlag=2" id="a2">同步SmarTeam</a></div>
            <div style="font-size:12px;margin-top:2px;" id="divNum">
                <span class="span_0">完成：</span><asp:Label ID="lblInvertoryStatus_3" runat="server" Text="" CssClass="zt_3"></asp:Label>
                <span class="span_1">未生成：</span><asp:Label ID="lblInvertoryStatus_1" runat="server" Text="" CssClass="zt_1"></asp:Label>
                <span class="span_1">部分生成：</span><asp:Label ID="lblInvertoryStatus_2" runat="server" Text="" CssClass="zt_2"></asp:Label>
            </div>
        </div>
    </div>
    <%--<div id="div3">
        <div id="iwucDiv3" class="iwucDiv">
            <div style="margin-top:9px;" id="divMenu3"><a href="/Plan/SynchronSmarTeam.aspx?inFlag=2" id="a2">同步SmarTeam</a></div>
            <div style="font-size:12px;margin-top:2px;" id="divNum">
                <span class="span_0">完成：</span><asp:Label ID="Label1" runat="server" Text="" CssClass="zt_3"></asp:Label>
                <span class="span_1">未生成：</span><asp:Label ID="Label2" runat="server" Text="" CssClass="zt_1"></asp:Label>
                <span class="span_1">部分生成：</span><asp:Label ID="Label3" runat="server" Text="" CssClass="zt_2"></asp:Label>
            </div>
        </div>
    </div>
    <div id="div4">
        <div id="iwucDiv4" class="iwucDiv">
            <div style="margin-top:9px;" id="divMenu4"><a href="/Plan/SynchronSmarTeam.aspx?inFlag=2" id="a2">同步SmarTeam</a></div>
            <div style="font-size:12px;margin-top:2px;" id="divNum">
                <span class="span_0">完成：</span><asp:Label ID="Label4" runat="server" Text="" CssClass="zt_3"></asp:Label>
                <span class="span_1">未生成：</span><asp:Label ID="Label5" runat="server" Text="" CssClass="zt_1"></asp:Label>
                <span class="span_1">部分生成：</span><asp:Label ID="Label6" runat="server" Text="" CssClass="zt_2"></asp:Label>
            </div>
        </div>
    </div>--%>
    <script type="text/javascript">

        function showFontStyle() {
            var flag = '<%=Request.QueryString["inFlag"]%>';
            flag = flag == "" ? 1 : flag;
            document.getElementById("a1").className = "borderLC3";
            document.getElementById("a2").className = "borderLC3";
            document.getElementById("div1").className = "borderLC7";
            document.getElementById("div2").className = "borderLC7";
            if (flag == "1") {
                document.getElementById("a1").className = "borderLC4";
                document.getElementById("div1").className = "borderLC8";
                $("#divNum").hide();
                $("#divMenu2").css("margin-top","18px");
            }
            else if (flag == "2") {
                document.getElementById("a2").className = "borderLC4";
                document.getElementById("div2").className = "borderLC8";
                $("#divNum").show();
            }
        }
        $(function () {
            showFontStyle();
        })
    </script> 
</div>