/*
路径：~/Areas/Sys/ViewJS/Base_SysLog.js
说明：权限按钮页面Base_SysLog的js文件
*/
//当前页面对象
var km = {};
km.model = null;
km.parent_model = null;

km.init = function () {
    km.init_parent_model();
    km.maingrid.init();
}

$(km.init);

/*初始化iframe父页面的model对象，即：访问app.index.js文件中的客户端对象*/
km.init_parent_model = function () {
    //只有当前页面有父页面时，可以获取到父页面的model对象 parent.wrapper.model
    if (window != parent) {
        if (parent.wrapper) {
            km.parent_model = parent.wrapper.model;
            //com.message('s', '获取到父页面的model对象：<br>' + JSON.stringify(km.parent_model));
        } else {
            com.showcenter('提示', "存在父页面，但未能获取到parent.wrapper对象");
        }
    } else {
        com.showcenter('提示', "当前页面已经脱离iframe，无法获得parent.wrapper对象！");
    }
}

//页面对象参数设置
km.settings = {};

//格式化数据
km.gridformat = {};

//百度模板引擎使用 详情：http://tangram.baidu.com/BaiduTemplate/
km.template = {
    tpl_add_html: '',//tpl_add模板的html
    jq_add: null,
    initTemplate: function () {
        var data = { title: 'baiduTemplate', list: ['test data 1', 'test data 2', 'test data3'] };
        this.tpl_add_html = baidu.template('tpl_add', data);//使用baidu.template命名空间
        this.jq_add = $(this.tpl_add_html);
    },
    init: function () { this.initTemplate(); }
};

km.maingrid = {
    jq: null,
    init: function () {
        this.jq = $("#maingrid").datagrid({
            fit: true, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'ButtonCode',
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["pagelist"],
            pagination: true, pageList: [5, 10, 15, 20, 30, 50, 100], pageSize: 15,
            columns: [[
	            { field: 'Id', title: '编号', width: 70, align: 'left', sortable: true },
                { field: 'LogTime', title: '时间', width: 140, align: 'left', sortable: true},
                {
                    field: 'LogType', title: '类型', width: 80, align: 'center', sortable: true, formatter: function (value, row, index) {
                        //日志类型.0=未定义 1=登录日志 2=操作日志 3=其他
                        var h = '未定义';
                        if (value == 1) { h = '登录日志'; } else if (value == 2) { h = '操作日志'; } else if (value == 3) { h = '其他'; }
                        return h;
                    }
                },
                { field: 'LogObject', title: '日志对象', width: 80, align: 'left', sortable: true },
                { field: 'LogAction', title: '操作类型', width: 80, align: 'center', sortable: true },
                { field: 'LogIP', title: 'IP地址', width: 120, align: 'center', sortable: true },
                { field: 'LogIPCity', title: 'IP城市', width: 100, align: 'left', sortable: true },
                { field: 'LogDesc', title: '描述', width: 480, align: 'center' }
            ]], toolbar: '#toolbar1',
            onLoadSuccess: function () {
                //alert('load data successfully!');
            }
        });//end grid init
    },
    reload: function () { this.jq.datagrid('reload', { _t: com.settings.timestamp() }); },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); }
};

/*工具栏权限按钮事件*/
km.toolbar = {
    do_refresh: function () {
        km.maingrid.reload(); //window.location.reload();
    },
    do_search: function () {

    }
};

