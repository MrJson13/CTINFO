
var admins = {};
window.adminRowBtnEvents = {
    'click .BtnOfResetPwd': function (e, value, row, index) {
        admins.loadResetPwdPage(row.AdminGUID);
    },
    'click .BtnOfEdit': function (e, value, row, index) {
        admins.loadEidtPage(row.AdminGUID);
    },
    'click .BtnOfDelete': function (e, value, row, index) {
        admins.delete(row.AdminGUID);
    }
};
//相关事件注册
$(document).ready(function () {
    //菜单选中
    common.menuSelected("admin");
    //初始化表格
    var oTable = new admins.tableInit();
    oTable.Init();
    //注册事件
    admins.initEvents();
    console.log(`
       ┏┓　　　┏┓
 　　┏┛┻━━━┛┻┓
 　　┃　　　　　　　 ┃
 　　┃　　　━　　　 ┃
 　　┃　┳┛　┗┳　 ┃
 　　┃　　　　　　　 ┃
 　　┃　　　┻　　　 ┃
 　　┃　　　　　　　 ┃
 　　┗━┓　　　┏━┛Codes are far away from bugs with the animal protecting
 　　　　┃　　　┃    神兽保佑,代码无bug
 　　　　┃　　　┃
 　　　　┃　　　┗━━━┓
 　　　　┃　　　　　    ┣┓
 　　　　┃　　　　    ┏┛
 　　　　┗┓┓┏━┳┓┏┛
 　　　　　┃┫┫　┃┫┫
 　　　　　┗┻┛　┗┻┛
    `);
});
//注册事件
admins.initEvents = function () {
    //查询按钮
    $("#btnQuery").click(function () {
        admins.query();
    });
    //加载新增页面按钮
    $("#btnLoadInsertPage").click(function () {
        admins.loadInsertPage();
    });
}
//初始化table
admins.tableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_admin').bootstrapTable({
            url: '/Admin/Page',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            queryParamsType: 'limit',
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            //height: 450,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "AdminGUID",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [
                {
                    field: 'operate',
                    title: '操作',
                    width: '250px',
                    events: adminRowBtnEvents,
                    align: 'center',
                    formatter: admins.operateFormatter
                },
                {
                    field: 'AdminName',
                    title: '姓名'
                }, {
                    field: 'AdminID',
                    title: '登录账号'
                }, {
                    field: 'AddTime',
                    title: '创建时间'
                }]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            PageSize: params.limit,   //页面大小
            PageIndex: (params.offset + params.limit) / params.limit,  //页码 
            r: Math.random()
        };
        return temp;
    };
    return oTableInit;
};
//table行中的按钮操作
admins.operateFormatter = function (value, row, index) {
    var btns = [
        '<button id="btn_delete" type="button" style="margin-left:10px;" class="BtnOfResetPwd btn btn-primary btn-xs">重置密码</button>',
        //'<button id="btn_edit" type="button" style="margin-left:10px;"  class="BtnOfEdit btn btn-primary btn-xs">编辑</button>',
    ];
    if (!row.IsSupper) {
        btns.push('<button id="btn_delete" type="button" style="margin-left:10px;" class="BtnOfDelete btn btn-danger btn-xs">删除</button>');
    }
    else {
        btns.push('<button id="btn_delete" type="button" style="margin-left:10px;" class="btn btn-default btn-xs" disabled="disabled">删除</button>');
    }
    return btns.join('');
}

/*------------------------------------------加载子页面Start----------------------------------------*/
/*------------------------------------------加载子页面Start----------------------------------------*/
//加载重置密码界面
admins.loadResetPwdPage = function (primaryKey) {
    var formId = "#FormAdminResetPwd";
    common.loadModalPage(
        "#adminModalContainer",
        "#adminModal",
        "/Admin/ResetPwdPartial",
        function () { admins.formValidateAdmin(formId); },
        function () { return 'primaryKey=' + primaryKey; }
        );
}
//加载编辑页面
admins.loadEidtPage = function (primaryKey) {
    var formId = "#FormAdminEdit";
    common.loadModalPage(
       "#adminModalContainer",
       "#adminModal",
       "/Admin/EditPartial",
       function () { admins.formValidateAdmin(formId, "CheckEditAdminID"); $(formId).data("bootstrapValidator").validate();  },
       function () { return 'primaryKey=' + primaryKey; }
       );
}
//加载新增页面
admins.loadInsertPage = function () {
    var formId = "#FormAdminInsert";
    common.loadModalPage(
      "#adminModalContainer",
      "#adminModal",
      "/Admin/InsertPartial",
      function () { admins.formValidateAdmin(formId, "CheckInsertAdminID"); },
      function () { }
      );
}
/*------------------------------------------加载子页面End----------------------------------------*/
/*------------------------------------------加载子页面End----------------------------------------*/


/*------------------------------------------提交表单Start----------------------------------------*/
/*------------------------------------------提交表单Start----------------------------------------*/
//提交查询请求
admins.query = function () {
    var searchAdminName = $('#txtSearchAdminName').val();
    var param = {
        query: {
            r: Math.random(),
            AdminName: searchAdminName
        }
    }
    $('#tb_admin').bootstrapTable('refresh', param);
}
//提交新增请求
admins.insert = function () {
    var formId = "#FormAdminInsert";
    //$(formId).data("bootstrapValidator").validate();
    admins.insertOrEdit(formId, "Admin", "Insert");
}
//提交更新请求
admins.edit = function () {
    var formId = "#FormAdminEdit";
    var oldvalDom = $("input[old-val]");
    var isEdit = false;
    oldvalDom.each(function () {
        var oldval = $(this).attr("old-val");
        var newval = $(this).val();
        var currname = $(this).attr("name");
        if (oldval !== newval) {
            isEdit = true;
        }
        else
        {
            $(formId).bootstrapValidator('enableFieldValidators', currname, false);
        }
    });
    if (!isEdit) {
        $("#adminModal").modal('hide');
        return;
    }
    admins.insertOrEdit(formId, "Admin", "Edit");
}
//提交新增/修改请求
admins.insertOrEdit = function (formId, controller, action) {

    $(formId).data("bootstrapValidator").validate();
    var flag = $(formId).data("bootstrapValidator").isValid();
    if (!flag) return;
    $(formId).ajaxSubmit({
        url: "/" + controller + "/" + action + '?r=' + Math.random(),
        type: 'POST',
        dataType: 'JSON',
        success: function (result) {
            if (result.success) {
                $("#tb_admin").bootstrapTable('refresh');
                $("#adminModal").modal('hide');
                toastr.success(result.message);
            }
            else {
                toastr.error(result.message);
            }
        }, error: function (err) {
            alert(err);
        }
    });
};
//提交删除请求
admins.delete = function (primaryKey) {
    Ewin.confirm({ message: "确认要删除吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        $.ajax({
            url: '/Admin/Delete?r=' + Math.random() + "&primaryKey=" + primaryKey,
            type: 'GET',
            dataType: 'JSON',
            success: function (result) {
                if (result.success) {
                    $("#tb_admin").bootstrapTable('refresh');
                    toastr.success(result.message);
                }
                else {
                    toastr.error(result.message);
                }
            },
            error: function (err) {
                alert(err);
            }
        });
    });
}
//提交重置密码请求
admins.resetPwd = function () {
    var formId = "#FormAdminResetPwd";
    var controller = "Admin";
    var action = "ResetPwd";
    $(formId).data("bootstrapValidator").validate();
    var flag = $(formId).data("bootstrapValidator").isValid();
    if (!flag) return;
    $(formId).ajaxSubmit({
        url: "/" + controller + "/" + action + '?r=' + Math.random(),
        type: 'POST',
        dataType: 'JSON',
        success: function (result) {
            if (result.success) {
                $("#tb_admin").bootstrapTable('refresh');
                $("#adminModal").modal('hide');
                toastr.success(result.message);
            }
            else {
                toastr.error(result.message);
            }
        }, error: function (err) {
            alert(err);
        }
    });
}
/*------------------------------------------提交表单End----------------------------------------*/
/*------------------------------------------提交表单End----------------------------------------*/

//初始化表单验证插件
admins.formValidateAdmin = function (formId, checkAdminAction) {
    $(formId).bootstrapValidator({
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            AdminName: {
                message: '姓名验证失败',
                validators: {
                    notEmpty: {
                        message: '姓名不能为空'
                    },
                    stringLength: {
                        max: 20,
                        message: '姓名不能超过20个字符'
                    }
                }
            },
            AdminID: {
                //threshold: 2, //有3字符以上才发送ajax请求，（input中输入一个字符，插件会向服务器发送一次，设置限制，6字符以上才开始）
                validators: {
                    notEmpty: {
                        message: '登录名不能为空'
                    },
                    stringLength: {
                        min: 2,
                        max: 50,
                        message: '账号长度2-50个字符'
                    },
                    remote: {
                        url: checkAdminAction,//验证地址
                        message: '用户已存在',//提示消息
                        type: 'GET',//请求方式
                        data: function (validator) {
                            return {
                                adminGUID: $("#AdminGUID").val(),
                                adminID: $("#AdminID").val()
                            };
                        },
                        delay: 250//每输入一个字符，就发ajax请求，服务器压力还是太大，设置2秒发送一次ajax（默认输入一个字符，提交一次，服务器压力太大）
                    }
                }
            },
            AdminPSW: {
                validators: {
                    notEmpty: {
                        message: '密码不能为空'
                    },
                    stringLength: {
                        min: 1,
                        max: 50,
                        message: '账号长度1-50个字符'
                    }
                }
            }
        }
    });
}

