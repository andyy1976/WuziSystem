/*********获得年*******/
function GetYear() {
    var Today = new Date();
    return Today.getFullYear();
}
/*********获得月*******/
function GetMonth() {
    var Today = new Date();
    return Today.getMonth() + 1;
}
/********获得日期*********/
function GetDay() {
    var Today = new Date();
    return Today.getDate();
}
/********获得小时*********/
function GetHours() {
    var Today = new Date();
    return Today.getHours();
}
/********获得分钟*********/
function GetMinutes() {
    var Today = new Date();
    return Today.getMinutes();
}
/********获得秒钟*********/
function GetSecond() {
    var Today = new Date();
    ss=Today.getTime() % 60000;
    return (ss - (ss % 1000)) / 1000;
}
/*********当前星期几*******/
function GetWeek() {
    var day = new Date();
    var today = new Array('星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六');
    var week = today[day.getDay()];
    return week;
}
function get_php_time() {
    var nowtime = (new Date).getTime();/*当前时间戳*/
    return nowtime;
}

//php时间戳转换为标准时间,timestamp为php时间，n为模式.1:年-月-日-时间。2：年-月-日
function Trans_php_time_to_str(timestamp, n) {
    update = new Date(timestamp * 1000);//时间戳要乘1000
    year = update.getFullYear();
    month = (update.getMonth() + 1 < 10) ? ('0' + (update.getMonth() + 1)) : (update.getMonth() + 1);
    day = (update.getDate() < 10) ? ('0' + update.getDate()) : (update.getDate());
    hour = (update.getHours() < 10) ? ('0' + update.getHours()) : (update.getHours());
    minute = (update.getMinutes() < 10) ? ('0' + update.getMinutes()) : (update.getMinutes());
    second = (update.getSeconds() < 10) ? ('0' + update.getSeconds()) : (update.getSeconds());
    if (n == 1) {
        return (year + '-' + month + '-' + day + ' ' + hour + ':' + minute + ':' + second);
    } else if (n == 2) {
        return (year + '-' + month + '-' + day);
    } else {
        return 0;
    }
}
/******获得当前节日*******/
function GetFestival() {
    var today = new Date();
    var month = today.getMonth();
    var day = today.getDate();
    var Festival = "";
    if ((month == 0) && (date == 1)) {
        Festival = "元旦";
    }
    if ((month == 2) && (date == 14)) {
        Festival = "情人节";
    }
    if ((month == 3) && (date == 8)) {
        Festival = "三八妇女节";
    }
    if ((month == 3) && (date == 12)) {
        Festival = "植树节";
    }
    if ((month == 4) && (date == 5)) {
        Festival = "清明节";
    }
    if ((month == 5) && (date == 1)) {
        Festival = "劳动节";
    }
    if ((month == 6) && (date == 1)) {
        Festival = "儿童节";
    }
    if ((month == 7) && (date == 1)) {
        Festival = "中国共产党建党日";
    }
    if ((month == 7) && (date == 7)) {
        Festival = "中国人民抗日战争纪念日 ";
    }
    if ((month == 8) && (date == 1)) {
        Festival = "建军节 ";
    }
    if ((month == 9) && (date == 10)) {
        Festival = "教师节 ";
    }
    if ((month == 10) && (date == 1)) {
        Festival = "国庆节  ";
    }
}
/**************显示当前时钟的函数**************************************/
function ChangeClock() {
    var Clock = new Date();
    var vYear = Clock.getFullYear();
    var vMon = Clock.getMonth() + 1;
    var vDay = Clock.getDate();
    var h = Clock.getHours();
    var m = Clock.getMinutes();
    var se = Clock.getSeconds();
    var s = vYear + '-' + vMon + '-' + vDay + ' ' + h + ':' + m + ':' + se;
    return s;
}

