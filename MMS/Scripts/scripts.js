(function () {
    var demo = window.demo = {};
 
    demo.onRequestStart = function (sender, args)
    {
        if (args.get_eventTarget().indexOf("Button") >= 0)
        {
            args.set_enableAjax(false);
        }
    }
})();