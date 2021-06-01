
var information = {};
//相关事件注册
$(document).ready(function () {
    //菜单选中
    common.menuSelected("message","Information1");
    //初始化表格
    var oTable = new information.tableInit();
    oTable.Init();
    //注册事件
    information.initEvents();
});
//注册事件
information.initEvents = function () {
    //查询按钮
    $("#btnQuery").click(function () {
        information.query();
    });
}
//初始化table
information.tableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_information').bootstrapTable({
            url: '/Information/Page',         //请求后台的URL（*）
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
            uniqueId: "GrabID",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [
                {
                    field: 'GrabID',
                    title: '编号'
                }, {
                    field: 'ProName',
                    title: '项目及标段名称',
                    formatter: function (value, row, index) {
                        return '<a href="/Information/Detail?id=' + row.GrabID + '">' + value + '</a>';
                    }
                }, {
                    field: 'WinCompany',
                    title: '中标候选人名称'
                }, {
                    field: 'ProPrice',
                    title: '投标报价',
                    formatter: function (value, row, index) {
                        var s = "工程建设费";
                        var reg = new RegExp("(" + s + ")", "g");
                        var newstr = value.replace(reg, "<font color=red>$1</font>");
                        return newstr;
                    }
                }, {
                    field: 'URL',
                    title: '信息地址',
                    formatter: function (value, row, index) {
                        return value == null ? "" : ('<a href="' + value + '"  target="_blank">查看</a>');
                    }
                }, {
                    field: 'CreateTime',
                    title: '创建时间'
                }]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var searchName = $('#txtName').val();
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            PageSize: params.limit,   //页面大小
            PageIndex: (params.offset + params.limit) / params.limit,  //页码
            ProName: searchName,
            r: Math.random()
        };
        return temp;
    };
    return oTableInit;
};

//提交查询请求
information.query = function () {
    var searchName = $('#txtName').val();
    var param = {
        query: {
            r: Math.random(),
            ProName: searchName
        }
    }
    $('#tb_information').bootstrapTable('refresh', param);
}





