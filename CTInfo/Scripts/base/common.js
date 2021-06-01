
var common = {};
//相关事件注册
$(document).ready(function () {
    //初始化toastr（消息提示框）的参数
    toastr.options = {
        "timeOut": "3000", //展现时间
        "hideDuration": "500",//消失的动画时间
        "positionClass": "toast-bottom-right"//弹出窗的位置
    }; 
});
common.menuSelected = function (menu, action) {
    $("#menu li").removeClass("active");
    if (menu) $("#menu li[data-menu=" + menu + "]").addClass("active");
    if (action) $("#menu li[data-action=" + action + "]").addClass("active");
}
common.RightsSelected = function (menu1, menu2) {
    //$("#menu li").removeClass("active");
    if (menu) $("#menu li[data-menu=" + menu1 + "]").addClass("active");//子
    if (action) $("#more_panel li[data-menu=" + menu2 + "]").addClass("active");//父
}
common.loadModalPage = function (containerId,modalId, url, completeFunc, getPramsFunc) {
    url = url + "?r=" + Math.random();
    if (getPramsFunc instanceof Function) {
        url += "&" + getPramsFunc();
    }
    $(containerId).load(url, function () {
        $(modalId).modal('show');
        if (completeFunc instanceof Function) {
            completeFunc();
        }
    });
}
//解决bootstrap在文本框输入回车回发问题，禁止在文本框内使用回车事件
common.boostrapSolveInput = function (formID) {
    $(formID + " input[type=text]").attr("onkeypress", "if(event.keyCode==13)return false;");
}
function confirmRevert(fun, params) {
    if ($("#myConfirm").length > 0) {
        $("#myConfirm").remove();
    }
    var html = "<div class='modal fade' id='myConfirm' >"
        + "<div class='modal-backdrop in' style='opacity:0; '></div>"
        + "<div class='modal-dialog' style='z-index:2901; margin-top:60px; width:400px; '>"
        + "<div class='modal-content'>"
        + "<div class='modal-header'  style='font-size:16px; '>"
        + "<span class='glyphicon glyphicon-envelope'>&nbsp;</span>信息！<button type='button' class='close' data-dismiss='modal'>"
        + "<span style='font-size:20px;  ' class='glyphicon glyphicon-remove'></span><tton></div>"
        + "<div class='modal-body text-center' id='myConfirmContent' style='font-size:18px; '>"
        + "是否确定要恢复？"
        + "</div>"
        + "<div class='modal-footer ' style=''>"
        + "<button class='btn btn-danger ' id='confirmOk'>确定<tton>"
        + "<button class='btn btn-info ' data-dismiss='modal'>取消<tton>"
        + "</div>" + "</div></div></div>";
    $("body").append(html);
    $("#myConfirm").modal("show");
    $("#confirmOk").on("click", function () {
        $("#myConfirm").modal("hide");
        fun(params); // 执行函数
    });
}
common.WdatePickerNoValidate = function (format) {
    WdatePicker({
        dateFmt: format,
        readOnly: true
    })
}
function DateTimeToDate(value) {
    if (value != '' && value != null) {
        value = new Date(value.replace(/-/g, "/"));
        var y = value.getFullYear();
        var m = value.getMonth() + 1;
        if (m < 10) {
            m = '0' + m;
        }
        var d = value.getDate();
        if (d < 10) {
            d = '0' + d;
        }
        return y + '-' + m + '-' + d;
    }
    else {
        return '';
    }
}
//注册事件委托
var ZJ = window.ZJ || {};
ZJ.Delegate = function (a, b) {
    return function () {
        return b.apply(a, arguments);
    }
};
/*office操作 start*/
//打开word
common.OpenWordByPageOffice = function (path) {
    POBrowser.openWindowModeless('/Office/Word?path=' + path, 'width=1200px;height=800px;');
}
//打开Excel
common.OpenExcelByPageOffice = function (path) {
    POBrowser.openWindowModeless('/Office/Excel?path=' + path, 'width=1200px;height=800px;');
}
//下载报告文件模板
common.DownReportNumber = function (path) {
    document.location.href = "/Report/DownOfficeFile?path=" + path;
}
/*office操作 end*/