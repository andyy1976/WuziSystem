/************去掉左边空格函数**************/
function LTrim(str) {
    var i;
    for (i = 0; i < str.length; i++) {
        if (str.charAt(i) != "" && str.charAt(i) != " ") break;
    }
    str = str.substring(i, str.length);
    return str;
}
/************去掉右边空格函数**************/
function RTrim(str) {
    var i;
    for (i = str.length - 1; i >= 0; i--) {
        if (str.charAt(i) != "" && str.charAt(i) != " ") break;
    }
    str = str.substring(0, i + 1);
    return str;
}
/************去掉左，右俩边空格函数**************/
function Trim(str) {
    return LTrim(RTrim(str));
}


/**********************接收其他页面跳转传递的参数**********************************/
function getQueryStringByName(name) {
    var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));

    if (result == null || result.length < 1) {

        return "";

    }
    return result[1];
}

/**********************计算输入字符长度的函数,汉字为俩个字符***********************************/
function countCharacters(str) {
    var totalCount = 0;
    for (var i = 0; i < str.length; i++) {
        var c = str.charCodeAt(i);
        if ((c >= 0x0001 && c <= 0x007e) || (0xff60 <= c && c <= 0xff9f)) {
            totalCount++;
        }
        else {
            totalCount += 2;
        }
    }
    return totalCount;
}

/**************保留俩位小数，并做四舍五入处理****************************/
//保留n位小数，并四舍五入处理
function DigitRound(digit, length) {
    length = length ? parseInt(length) : 0;
    if (length <= 0) return Math.round(digit);
    digit = Math.round(digit * Math.pow(10, length)) / Math.pow(10, length);
    return digit;
}


function setReturnValueFalse() {
    if (document.all) {
        window.event.returnValue = false;
    }
    else {
        event.preventDefault();
    }
}

function clearMask(ctrlName, ctrlName1) {
    $("#" + ctrlName).hide();
    $("#" + ctrlName1).hide();
}
function createMask(ctrlName, ctrlName1) {
    var sWidth, sHeight;
    sWidth = window.screen.availWidth;
    if (window.screen.availHeight > document.body.scrollHeight) {
        sHeight = window.screen.availHeight;
    } else {
        sHeight = document.body.scrollHeight;
    }
    $("#" + ctrlName).addClass("mask").width(sWidth).height(sHeight).appendTo("body").fadeIn(300);
    $("#" + ctrlName1).show();
}

function createMask1(ctrlName, ctrlName1) {
    var sWidth, sHeight;
    sWidth = window.screen.availWidth;
    if (window.screen.availHeight > document.body.scrollHeight) {
        sHeight = window.screen.availHeight;
    } else {
        sHeight = document.body.scrollHeight;
    }
    $("#" + ctrlName).addClass("mask1").width(sWidth).height(sHeight).appendTo("body").fadeIn(300);
    $("#" + ctrlName1).show();
}

function checkMoney_2(ctrlName, str) {
    var Content = document.getElementById(ctrlName).value.replace(/\s/g, "");
    var ContentPatrn = /^(\d|-)?(\d|,)*\.?\d{0,2}$/;
    if (Content == "") {
        alert("“" + str + "”不得为空！");
        setReturnValueFalse();
    }
    else if (!ContentPatrn.exec(Content)) {
        alert("“" + str + "”格式不正确，必须是数字(含小数)！");
        setReturnValueFalse();
    }
    else {
        return true;
    }
}

function checkCompare_2(num1, num2, str) {
    if (num1 > num2) {
        alert(str);
        setReturnValueFalse();
    }
    else {
        return true;
    }
}

function checkNum_1(ctrlName, str) {
    var Content = document.getElementById(ctrlName).value.replace(/\s/g, "");
    var ContentPatrn = /^[0-9]+$/;
    if (Content == "") {
        alert("“" + str + "”不得为空！");
        setReturnValueFalse();
    }
    else if (!ContentPatrn.exec(Content)) {
        alert("“" + str + "”格式不正确，必须是数字！");
        setReturnValueFalse();
    }
    else {
        return true;
    }
}


function ValidInt(str) {
    var flag = true;
    try {
        str = parseInt(str);
        if (isNaN(str))
            flag = false;
    } catch (e) {
        flag = false;
    }
    return flag;
}

function ValidIsNotDecimal(str) {
    var flag = true;
    try {
        str = parseFloat(str);
        if (isNaN(str))
            flag = false;
    } catch (e) {
        flag = false;
    }
    return flag;
}


function GetSys_Dic_2(typeId, ctrlName) {
    $.post("../../AjaxProgram/Public/Public_Handler.ashx", { "type": "GetSys_Dictionary", "typeId": typeId }, function (data, backstatus) {
        if (backstatus == "success") {
            var str = "#" + ctrlName;
            $(str).empty();
            $(data).find("Sys_Dictionary").each(function () {
                var tid = $(this).find("TypeId").text();
                tid += "-" + $(this).find("KeyWordCode").text();
                var kword = $(this).find("KeyWord").text();
                $("<option value=" + tid + ">" + kword + " </option>").appendTo(str);
            })
        }
    })
}

function GetSys_Info(tname, code, dec, ctrlName) {
    $.post("../../AjaxProgram/Public/Public_Handler.ashx", { "type": "GetSys_Info", "tname": tname, "code": code, "dec": dec }, function (data, backstatus) {
        if (backstatus == "success") {
            var str = "#" + ctrlName;
            $(str).empty();
            $(data).find("Proc_Sys_Info").each(function () {
                var tid = $(this).find("strcode").text();
                var kword = $(this).find("strdec").text();
                $("<option value=" + tid + ">" + kword + " </option>").appendTo(str);
            })
        }
    })
}

function GetSys_Info_1(tname, code, dec, ctrlName) {
    $.post("../../AjaxProgram/Public/Public_Handler.ashx", { "type": "GetSys_Info", "tname": tname, "code": code, "dec": dec }, function (data, backstatus) {
        if (backstatus == "success") {
            var str = "#" + ctrlName;
            $(str).empty();
            $("<option value='' selected='selected'>请选择</option>").appendTo(str);
            $(data).find("Proc_Sys_Info").each(function () {
                var tid = $(this).find("strcode").text();
                var kword = $(this).find("strdec").text();
                $("<option value=" + tid + ">" + kword + " </option>").appendTo(str);
            })
        }
    })
}

function GetSys_Dic_1(typeId, ctrlName) {
    //$.ajaxSetup({ async: false });
    $.post("../../AjaxProgram/Public/Public_Handler.ashx", { "type": "GetSys_Dictionary", "typeId": typeId }, function (data, backstatus) {
        if (backstatus == "success") {
            var str = "#" + ctrlName;
            $(str).empty();
            $("<option value='' selected='selected'>全部</option>").appendTo(str);
            $(data).find("Sys_Dictionary").each(function () {
                var tid = $(this).find("TypeId").text();
                tid += "-" + $(this).find("KeyWordCode").text();
                var kword = $(this).find("KeyWord").text();
                $("<option value=" + tid + ">" + kword + " </option>").appendTo(str);
            })

        }
    })
}

function GetSys_Dic(typeId, ctrlName) {
    $.ajaxSetup({ async: false });
    $.post("../../AjaxProgram/Public/Public_Handler.ashx", { "type": "GetSys_DictionaryByCon", "typeId": typeId }, function (data, backstatus) {
        if (backstatus == "success") {
            var str = "#" + ctrlName;
            $(str).empty();
            $("<option value='' selected='selected'>全部</option>").appendTo(str);
            $(data).find("Sys_Dictionary").each(function () {
                var tid = $(this).find("TypeId").text();
                tid += "-" + $(this).find("KeyWordCode").text();
                var kword = $(this).find("KeyWord").text();
                $("<option value=" + tid + ">" + kword + " </option>").appendTo(str);
            })
        }
    })
}
function GetSys_Dic_2(typeId, ctrlName, defStr) {

    $.post("../../AjaxProgram/Public/Public_Handler.ashx", { "type": "GetSys_DictionaryByCon", "typeId": typeId }, function (data, backstatus) {
        if (backstatus == "success") {
            var str = "#" + ctrlName;
            $(str).empty();
            $("<option value='' selected='selected'>" + defStr + "</option>").appendTo(str);
            $(data).find("Sys_Dictionary").each(function () {
                var tid = $(this).find("TypeId").text();
                tid += "-" + $(this).find("KeyWordCode").text();
                var kword = $(this).find("KeyWord").text();
                $("<option value=" + tid + ">" + kword + " </option>").appendTo(str);
            })
        }
    })
}

function HTMLEncode(str) {
    var div = document.createElement("div");
    var text = document.createTextNode(str);
    div.appendChild(text);
    return div.innerHTML;
}
function HTMLDecode(str) {
    var div = document.createElement("div");
    div.innerHTML = str;
    return div.innerHTML;
}

function checkStr_1(ctrlName, str) {
    var Content = document.getElementById(ctrlName).value.replace(/\s/g, "");
    var ContentPatrn = /^[a-zA-Z\u4e00-\u9fa50-9_]+$/;
    if (Content == "") {
        alert("“" + str + "”格式不正确。允许格式：汉字、字母、数字和下划线。");
        setReturnValueFalse();
    }
    else if (!ContentPatrn.exec(Content)) {
        alert("“" + str + "”格式不正确，允许格式：汉字、字母、数字和下划线！");
        setReturnValueFalse();
    }
    else {
        return true;
    }
}
/*登陆注册页面高度计算*/
$(document).ready(function () {
    var height_window = $(window).height() - 393;
    var height_top01 = $('.denglu-main').height();
    var height_top02 = $('.zhuce-main').height();
    if (height_window < height_top01) {
        $('.denglu-main').css('height', height_top01);
    } else {
        $('.denglu-main').css('height', height_window);
    }
    if (height_window < height_top02) {
        $('.zhuce-main').css('height', height_top02);
    } else {
        $('.zhuce-main').css('height', height_window);
    }
})

function ShowImg(img1,img2,flag) {
    $(img1).hide();
    $(img2).hide();
    switch (flag) {
        case "1":
            $(img1).show();
            break;
        case "2":
            $(img2).show();
            break;
    }
}


function ValidPwd1(tbPwd, spanPwd, img1, img2) {
    var pwd = $(tbPwd).val().replace(/\s/g, "");
    if (pwd.length == 0) {
        ShowImg(img1, img2, "2");
        $(spanPwd).html("请输入密码！").css("color", "red");
    }
}
function ValidPwd(tbPwd, spanPwd, img1, img2) {
    var pwdpass = 0;
    var pwd = $(tbPwd).val().replace(/\s/g, "");
    var pwdpatrn = /(([-\da-zA-Z`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]*((\d+[a-zA-Z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+)|(\d+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[a-zA-Z]+)|([a-zA-Z]+\d+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+)|([a-zA-Z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+\d+)|([-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+\d+[a-zA-Z]+)|([-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[a-zA-Z]+\d+))[-\da-zA-Z`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]*)|([-\da-zA-Z]*((\d+[a-z]+[A-Z]+)|(\d+[a-z]+[a-zA-Z]+)|([A-Z]+\d+[a-z]+)|([A-Z]+[a-z]+\d+)|([a-z]+\d+[A-Z]+)|([a-z]+[A-Z]+\d+))[-\da-zA-Z]*)|([-a-zA-Z`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]*(([a-z]+[A-Z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+)|([a-z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[A-Z]+)|([A-Z]+[a-z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+)|([A-Z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[a-z]+)|([-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[a-z]+[A-Z]+)|([-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[A-Z]+[a-z]+))[-a-zA-Z`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]*))/;
    ///^[a-zA-Z0-9_]{6,32}$/;
    if (pwd != "" && pwdpatrn.exec(pwd)) {
        pwdpass = 0;
        ShowImg(img1, img2,"1");
        $(spanPwd).html("").css("color", "");
    }
    else {
        var pwd1 = $(tbPwd).val();

        if (pwd1.length == 0) {
            $(spanPwd).html("").css("color", "");
            pwdpass = 1;
        }
        else {
            ShowImg(img1, img2, "2");
            $(spanPwd).html("密码格式错误！").css("color", "red");
            pwdpass = 2;
        }
    }
    return pwdpass;
}

function ValidOKPwd(tbPwd, tbOKPwd, spanPwd, img1, img2) {
    var okpass = 0;
    var pwd2 = $(tbOKPwd).val();
    var pwd = $(tbPwd).val();
    if (pwd2 != "" && (pwd == pwd2)) {
        okpass = 0;
        ShowImg(img1, img2, "1");
        $(spanPwd).html("").css("color", "");
    }
    else {
        if (pwd.length == 0 && pwd2.length == 0) {
            $(spanPwd).html("").css("color", ""); okpass = 1;
        }
        else {
            ShowImg(img1, img2, "2");
            $(spanPwd).html("两次输入的密码不一致！").css("color", "red"); okpass = 2;
        }

    }
    return okpass;
}

function ValidVCode(txtVCode, authcode, spanVCode, img1, img2) {
    var vcode = $(txtVCode).val().replace(/\s/g, "");
    if (vcode != "") {
        $.ajaxSetup({ async: false });
        $.post("../AjaxProgram/Web/AutoCodeValid.ashx", { "authval": vcode }, function (data, backstatus) {
            if (backstatus == "success") {
                if (data == "1") {
                    ShowImg(img1, img2, "1");
                    $(spanVCode).html("").css("color", "");
                }
                else {
                    ShowImg(img1, img2, "2");
                    $(authcode).attr("src", imgSrc + new Date());
                    $(spanVCode).html("验证码输入错误！").css("color", "red");
                }
            }
        })

    }
    else {
        if (vcode == "") {
            $(spanVCode).html("").css("color", "");
        }
        else {
            ShowImg(img1, img2, "2");
            $(spanVCode).html("验证码不得为空！").css("color", "red");
        }

    }
}
function IdNumParsingSex(IdNum){
    var SexStr=IdNum.substr(16,1);
    return parseInt(SexStr%2);
}


//文本框输入数字，并只能输入一次小数点，并保留小数点后两位
function clearNoDecimal(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "").replace(/^\./g, "").replace(/\.{2,}/g, ".").
            replace(".", "$#$").replace(/\./g, "").replace("$#$", ".").
            replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');
}
//文本框输入数字
function clearNoNum(obj) {
    obj.value = obj.value.replace(/[^0-9]/g, "");// /^(\d{16}|\d{19})$/;
}
//文本框输入数字和字母
function clearNoNumAndChar(obj) {
    obj.value = obj.value.replace(/[^0-9A-Za-z]/g, "");// /^(\d{16}|\d{19})$/;
}


//字符串处理方法，1－str:要处理的字符串，2－len:第隔N个字符换行
function stringDispose(str, len) {
    var result = "";
    if (str != "" && str != null && typeof (str) != "undefined") {
        if (str.length > len) {
            var num = parseInt(str.length / len);
            num = str.length % len != 0 ? num + 1 : num;
            var tmp = str;
            var tmp1 = "";
            var br = "<br/>";
            for (var i = 0; i < num; i++) {
                if (str.length - (i * len) < len) {
                    tmp1 = tmp.substr(i * len);
                    br = "";
                }
                else
                    tmp1 = tmp.substr(i * len, len);
                result = result + tmp1 + br;
            }
        } else
            result = str;
    } else
        result = "";
    return result;
}

function IdNumParsingSex(IdNum) {
    var SexStr = IdNum.substr(16, 1);
    return parseInt(SexStr % 2);
}

//替换英文,为中文，
function ReplaceSpecialCharacter(str,char1,char2){
    return str.replace(char1, char2);
}

//根据字符获得该字符串中特定位置的子串
function GetSpecifySubString(str) {
    var p1 = str.indexOf("'");
    var p2 = str.lastIndexOf("'");
    return str.substring(p1+1, p2);
}


String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}

function stringFormat() {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}