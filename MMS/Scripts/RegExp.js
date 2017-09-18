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
/*********非空验证,并计算字符串长度，如果长度为0，则为空************/
function RequiredFieldValidator(str) {
    var totalCount = countCharacters(str);
    return totalCount;
}
/**************身份证验证******************************/
function IDCardAuth(str) {
    var reg = /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z,a-z])$/;
    return reg.test(str);
}
/**************邮箱*********************************/
function AuthEmail(str) {
    var reg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((.[a-zA-Z0-9_-]{2,3}){1,2})$/;
    return reg.test(str);
}
/**************邮编*****************************/
function PostCode(str) {
    var reg = /^[0-9]{6}$/;
    return reg.test(str);
}

/*工商注册号：14位数字本体码和一位数字效验码组成.*/
/*前六位代表的是工商行政管理机关的代码，国家工商行政管理总局用“100000”表示，省级、地市级、区县级登记机关代码分别使用6位行政区划代码表示*/
/*顺序码是7-14位.第7位：
                   一.个体工商户
                      6：个体工商户， 7、8、9三个数字根据发展变化由省局决定启用时间
                   二.内资企业
                     第7位：
                        0：表示全民所有制企业、集体所有制企业、联营企业、股份合作制企业
                        1：表示国有独资、法人独资、国有控股、法人投资或控股的有限责任公司和股份有限公司
                        2：表示自然人独资、自然人投资或控股的有限责任公司及自然人发起设立的股份有限公司
                        3：表示其他非公司企业（包括个人独资企业、合伙企业、农民专业合作社）                       
                     第8位：
                        1：表示具有法人资格（含个人独资企业、合伙企业）
                        2：表示营业单位或分支机构（分公司）
                     第9,10位
                        01：表示内资企业法人
                        02：表示内资企业法人分支机构以及事业法人所办的营业单位
                        11：表示内资有限公司
                        12：表示内资有限公司分支机构
                        21：表示私营有限公司
                        22：表示私营有限公司分支机构
                        31：表示其他非公司企业法人、个人独资企业、合伙企业
                        32：表示其他非公司企业法人分支机构
                    三.外资企业
                      第7位:第七位数全省先统一使用“5”，“4”
                      第八位数
                        1:表示外国投资企业
                        2:表示台/港/澳投资企业
                        3:表示外商投资企业分支机构
                        4:表示常驻代表机构
                        5:表示外国企业在中国境内从事经营活动
                      第九位数
                        0:表示有限公司
                        1:表示股份有限公司上市公司
                        2:表示股份有限公司未上市公司

*/
function AuthBLNo(str) {
    //var reg = /^[0-9]{6}([6,7,8,9]{1}[0-9]{8}|[0,1,2,3]{1}[1,2]{1}([0]{1}[1,2]{1}|[1]{1}[1,2]{1}|[2]{1}[1,2]{1}|[3]{1}[1,2]{1})[0-9]{5}|[4,5]{1}[1,5]{1}[0,1,2]{1}[0-9]{6})$/;
    var reg = /^[0-9]{15}$/
    return reg.test(str);
}
/************税号******************/
function AuthTax(str) {
    var reg = /^[0-9]{14}[0-9,a-z,A-Z]{1}$/;
    return reg.test(str);
}
/************机构信用代码***************/
function AuthCreditNo(str) {
    var reg = /^[0-9a-zA-Z]{18}$/;
    //var reg = /^[0-9a-zA-Z]\w{18}$/;
    return reg.test(str);
}
/**************企业名称拼音缩写验证,大写，最少5个字符，最大50个字符**********************/
function AuthChinaWord(str) {
    var reg = /^[a-zA-Z]{2,50}$/;
    return reg.test(str);
}
/**************姓名验证，2到9个汉字**********/
function AuthName(str) {
    var reg = /^[\u4E00-\u9FA5]+$/;
    if (reg.test(str) && countCharacters(str) >= 4 && countCharacters(str) < 18) {
        return true;
    }
    else {
        return false;
    }
}
/**************姓名拼音缩写验证,大写，最少2个字符，最大18个字符**********************/
function AuthNamePy(str) {
    var reg = /^[a-zA-Z]{2,18}$/;
    return reg.test(str);
}
/**************企业名称验证,最少5个汉字，最大100个汉字**********************/
function EnterpriseNameAuth(str) {
    var reg = /^[\u4E00-\u9FA5A-Za-z0-9]+$/;
    if (reg.test(str) && countCharacters(str) >= 10 && countCharacters(str) < 200) {
        return true;
    }
    else {
        return false;
    }
}
/**************地址验证,最少10个汉字，最大50个汉字**********************/
function AuthAdress(str) {
    var reg = /^[\u4e00-\u9fa5\\w]+/;
    //var reg = /^[\u4E00-\u9FA5A-Za-z0-9]+$/;
    if (reg.test(str) && countCharacters(str) >= 6 && countCharacters(str) < 100) {
        return true;
    }
    else {
        return false;
    }
}
/**************全数字验证(1到20位）*****************************/
function AllDigitCode(str) {
    var reg = /^[0-9]{1,20}$/;
    return reg.test(str);
}
/**************全数字验证(1到2位）*****************************/
function AllDigitCode2(str) {
    var reg = /^[0-9]{1,2}$/;
    return reg.test(str);
}
/*********4位数字的验证码验证**********/
function AuthCodeAuth(str) {
    var reg = /^[0-9]{4}$/;
    return reg.test(str);
}
/*********6位数字的验证码验证**********/
function SixCodeAuth(str) {
    var reg = /^[0-9]{6}$/;
    return reg.test(str);
}
/**************汉字验证**********************/
function ChineseAuth(str) {
    var reg = /^[\u4E00-\u9FA5]+$/;
    return reg.test(str);
}
/**************由26个英文字母组成的字符串 *******************/
function LetterAuth(str) {
    var reg = /^[A-Za-z]+$/;
    return reg.test(str);
}
/**************由26个英文字母大写组成的字符串 *******************/
function UpperLetterAuth(str) {
    var reg = /^[A-Z]+$/;
    return reg.test(str);
}
/**************由26个英文字母小写组成的字符串 *******************/
function LowerLetterAuth(str) {
    var reg = /^[a-z]+$/;
    return reg.test(str);
}
/**************手机号码验证******************/
function AuthMobile(str) {
    var reg = /^(13[0-9]{9})|(14[7][0-9]{8})|(15[0-9]{9})|(18[0,2,3,5,6,7,8,9][0-9]{8})|(17[6,7,8][0-9]{8})$/;
    return reg.test(str);
}
/**************座机加分机号码验证;必须符合以下格式 xxxx-xxxxxxxx-xxxx******************/
function TexNumberAuth(str) {
    var reg = /(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})$/;
    return reg.test(str);
}
/*****多个号码，中间以逗号分割*******/
function MoreTexAuth(str) {
    var reg = /^((\d{3,4}-)?\d{7,8}|13[0-9]{9})(,(\d{3,4}-)?\d{7,8}|13[0-9]{9})*$/;
    return reg.test(str);
}
/*******验证传真号码********/
function AuthFaxNo(str) {
    //var reg = /(^[0-9]…{3,4}-[0-9]…{3,8}$)|(^[0-9]…{3,8}$)|(^([0-9]…{3,4})[0-9]…{3,8}$)|(^0…{0,1}13[0-9]…{9}$)/;
    var reg = /(^[0-9]{3,4}-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^([0-9]{3,4})[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)/;
    return reg.test(str);
}
/**********企业组织机构代码证验证,必须为9位数字和字母组成。*****************/
function OrganizationCode(str) {
    var reg = /^[0-9A-Z]{8}[0-9,a-z,A-Z]{1}$/;
    return reg.test(str);
}
/*********日期验证，规则：yyyy-mm-dd*************/
function DateAuth(str) {
    var reg = /(?!0000)[0-9]{4}-((0[1-9]|1[0-2])-(0[1-9]|1[0-9]|2[0-8])|(0[13-9]|1[0-2])-(29|30)|(0[13578]|1[02])-31)/;
    return reg.test(str);
}
/*********月份验证*********/
function MonthAuth(str) {
    var reg = /^0[1-9]|1[0-2]$/;
    return reg.test(str);
}
/*********年份验证2000年以后*********/
function YearAuth(str) {
    var reg = /[2]{1}[0]{1}[0-9]{2}$/;
    return reg.test(str);
}
/*********校验密码：只能输入6-18个字母、数字、下划线*********/
function AuthPassWord(str) {
    var reg = /(([-\da-zA-Z`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]*((\d+[a-zA-Z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+)|(\d+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[a-zA-Z]+)|([a-zA-Z]+\d+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+)|([a-zA-Z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+\d+)|([-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+\d+[a-zA-Z]+)|([-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[a-zA-Z]+\d+))[-\da-zA-Z`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]*)|([-\da-zA-Z]*((\d+[a-z]+[A-Z]+)|(\d+[a-z]+[a-zA-Z]+)|([A-Z]+\d+[a-z]+)|([A-Z]+[a-z]+\d+)|([a-z]+\d+[A-Z]+)|([a-z]+[A-Z]+\d+))[-\da-zA-Z]*)|([-a-zA-Z`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]*(([a-z]+[A-Z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+)|([a-z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[A-Z]+)|([A-Z]+[a-z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+)|([A-Z]+[-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[a-z]+)|([-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[a-z]+[A-Z]+)|([-`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]+[A-Z]+[a-z]+))[-a-zA-Z`=\\\[\];',./~!@#$%^&*()_+|{}:"<>?]*))/;
    return reg.test(str);
}
/******以字母开头，长度在5~18之间，只能包含字符、数字和下划线。密码必须同时包含大写字母、小写字母、数字、特殊符号等四项中的至少三项正则表达式*****/
function AuthUserName(str) {
    var reg = /^[a-zA-Z]\w{4,17}$/;
    return reg.test(str);
}
/*****货币验证，保留俩位小数*******/
function AuthCurrency(str) {
    str = str.toString().replace(/,/g, "");
    str = str.replace(/^0+/, ""); // 金额数值转字符、移除逗号、移除前导零
    var reg = /^[0-9]\d*(,\d{3})*(\.\d{1,2})?$/;
    return reg.test(str);
}
/*************期限验证,不超过3位数字*********************/
function LifeAuth(str) {
    var reg = /[1-9]{1}[0-9]{0,2}$/;
    return reg.test(str);
}
/*************5位只包含数字和字符的验证码*********************/
function Captcha4Auth(str) {
    var reg = /[2345678abcdefhijkmnpqrstuvwxyzABCDEFGHJKLMNPQRTUVWXY]{4}$/;
    return reg.test(str);
}
/*************5位只包含数字和字符的验证码*********************/
function Captcha5Auth(str) {
    var reg = /[2345678abcdefhijkmnpqrstuvwxyzABCDEFGHJKLMNPQRTUVWXY]{5}$/;
    return reg.test(str);
}
/*************5位只包含数字和字符的验证码*********************/
function Captcha6Auth(str) {
    var reg = /[2345678abcdefhijkmnpqrstuvwxyzABCDEFGHJKLMNPQRTUVWXY]{6}$/;
    return reg.test(str);
}
/*************整数验证，不超过10位*********************/
function IntegerAuth(str){
    var reg = /[1-9]{1}[0-9]{0,9}$/;
    return reg.test(str);
}



 