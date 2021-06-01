
var projectinfo = {};
//相关事件注册
$(document).ready(function () {
    //菜单选中
    common.menuSelected("message", "Information2");
    //初始化表格
    var oTable = new projectinfo.tableInit();
    oTable.Init();
    //注册事件
    projectinfo.initEvents();
    $("#formSearch .form_datetime").click(function () {
        common.WdatePickerNoValidate('yyyy-MM-dd')
    });
});
//注册事件
projectinfo.initEvents = function () {
    //查询按钮
    $("#btnQuery").click(function () {
        projectinfo.query();
    });
}
//初始化table
projectinfo.tableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_projectinfo').bootstrapTable({
            url: '/ProjectInfoes/Page',         //请求后台的URL（*）
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
            uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [
                {
                    field: 'Title',
                    title: '标题',
                    formatter: function (value, row, index) {
                        return '<a href="/ProjectInfoes/Detail?id=' + row.Id + '">' + value + '</a>';
                    }
                },
                {
                    field: 'Categorynum',
                    title: '数据类型',
                    formatter: function (value, row, index) {
                        if (value == '002001006') {
                            return "工程建设";
                        }
                        else if (value == '002002003') {
                            return "政府采购";
                        } else {
                            return "其他";
                        }
                    }
                },
                {
                    field: 'Project',
                    title: '标段名'
                },
                {
                    field: 'publicityPeriod',
                    title: '公示期'
                },
                {
                    field: 'successfulName',
                    title: '中标候选第一名'
                },
                {
                    field: 'successfulPrice',
                    title: '投标报价'
                },
                {
                    field: 'Details',
                    title: '数据详情'
                },
                {
                    field: 'successfulReviewPrice',
                    title: '经评审的投标价'
                },
                {
                    field: 'Infodate',
                    title: '时间'
                },
                {
                    field: 'Linkurl',
                    title: '地址',
                    formatter: function (value, row, index) {
                        return '<a href="http://ggzyjy.sc.gov.cn' + value + '" target="_blank">查看</a>';
                    }
                }
            ]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var beginTime = $('#txtBeginTime').val();
        var endTime = $('#txtEndTime').val();
        var category = $('#txtCategory').val();
        var title = $('#txtTitle').val();
        var successfulName = $('#txtSuccessfulName').val();
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            PageSize: params.limit,   //页面大小
            PageIndex: (params.offset + params.limit) / params.limit,  //页码 
            BeginTime: beginTime,
            EndTime: endTime,
            Category: category,
            Title: title,
            SuccessfulName: successfulName,
            r: Math.random()
        };
        return temp;
    };
    return oTableInit;
};

//提交查询请求
projectinfo.query = function () {
    var beginTime = $('#txtBeginTime').val();
    var endTime = $('#txtEndTime').val();
    var category = $('#txtCategory').val();
    var title = $('#txtTitle').val();
    var successfulName = $('#txtSuccessfulName').val();
    var param = {
        query: {
            r: Math.random(),
            BeginTime: beginTime,
            EndTime: endTime,
            Category: category,
            Title: title,
            SuccessfulName: successfulName
        }
    }
    $('#tb_projectinfo').bootstrapTable('refresh', param);
}





