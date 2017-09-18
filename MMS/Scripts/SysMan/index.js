function x(){
    var clock = GetYear();
    $("#NowTime").html(clock + "-" + GetMonth() + "-" + GetDay());
}
$(function () {
    setInterval("x()", "0");
});