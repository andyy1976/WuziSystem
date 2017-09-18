$(function () {
    $(".rgRow").css("background-color", "#fff");
    $(".rgAltRow").css("background-color", "#ccc");
    $(".rgRow").removeClass("rgHoveredRow");
    $(".rgAltRow").removeClass("rgHoveredRow");
    $(".rgRow").hover(function () {
        $(".rgRow").removeClass("rgHoveredRow");
        $(this).css("background-color", "#ddedfb");
    }, function () {
        $(this).css("background-color", "#fff");
    })
    $(".rgAltRow").hover(function () {
        $(".rgAltRow").removeClass("rgHoveredRow");
        $(this).css("background-color", "#ddedfb");
    }, function () {
        $(this).css("background-color", "#ccc");
    })
    $(".rgRow").live("click", function () {
        $(".rgRow").bind({
            mouseenter: function () {
                $(this).css("background-color", "#ddedfb");
            }, mouseleave: function () {
                $(this).css("background-color", "#fff");
            }
        })
        $(".rgAltRow").bind({
            mouseenter: function () {
                $(this).css("background-color", "#ddedfb");
            }, mouseleave: function () {
                $(this).css("background-color", "#ccc");
            }
        })
        $(".rgRow").css("background-color", "#fff");
        $(".rgAltRow").css("background-color", "#ccc");
        $(this).css("background-color", "#ddedfb");
        $(this).unbind("hover");
    })
    $(".rgAltRow").live("click", function () {
        $(".rgRow").bind({
            mouseenter: function () {
                $(this).css("background-color", "#ddedfb");
            }, mouseleave: function () {
                $(this).css("background-color", "#fff");
            }
        })
        $(".rgAltRow").bind({
            mouseenter: function () {
                $(this).css("background-color", "#ddedfb");
            }, mouseleave: function () {
                $(this).css("background-color", "#ccc");
            }
        })
        $(".rgRow").css("background-color", "#fff");
        $(".rgAltRow").css("background-color", "#ccc");
        $(this).css("background-color", "#ddedfb");
        $(this).unbind("hover");
    })
})