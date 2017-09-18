function Get_FirstItem() {
    $.post("../AjaxCode/index.Master/Get_FirstItem.ashx", function (data, backstatus) {
        if (backstatus == "success") {
            $(data).find("Get_FirstItem").each(function () {
                var dataId = $(this).find("Id").text();
                var dataItemName = $(this).find("ItemName").text();
                var dataItemUrl = $(this).find("ItemUrl").text();
                if (dataItemUrl == "#") {
                    $("<li class='mainlevel'><a id='FLevel" + dataId + "' href='" + dataItemUrl + "'>" +
                      dataItemName + "</a><ul class='sub_nav_01' id='SItem_" + dataId + "'></ul></li>").appendTo("#nav");
                }
            })
            Get_SecondItem();
        }
    })
}
function Get_SecondItem() {
    $.post("../AjaxCode/index.Master/UserPermission.ashx", function (data, backstatus) {
        $(data).find("Get_UserPermission").each(function () {
            var dataItemName = $(this).find("ItemName").text();
            var dataItemUrl = $(this).find("ItemUrl").text();
            var dataParentId = $(this).find("ParentId").text();
            alert(dataParentId);
            $("<li class='Sli'><a class='Name' href='" + dataImgUrl + "'>" + dataItemName + "</a></li>").appendTo("#SItem_" + dataParentId);
            //$("#Itemhead_" + dataParentId).show();
        })
    })
}
$(function () {
    Get_FirstItem();
    $(".mainlevel").live("hover", function (event) {
        if (event.type == 'mouseenter') {
            if ($(this).children(".sub_nav_01").html() == "") {
                $(this).children(".sub_nav_01").hide();

            } else {
                $(this).children(".sub_nav_01").slideDown(200);
            }

        } else {
            $(this).children(".sub_nav_01").slideUp(200);
        }

    })
    $(".Sli").live("click", function () {
        var URL = $(this).find(".Name").attr("href");
        window.location = URL;
    })
})