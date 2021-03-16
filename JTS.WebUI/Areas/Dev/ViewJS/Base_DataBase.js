/*
路径：~/Areas/Dev/ViewJS/Base_DataBase.js
说明：数据库管理页面Base_DataBase的js文件
*/
//当前页面对象
var km = {};
km.model = null;//当前model对象
km.parent_model = null;//存储父页面的model对象 parent.wrapper.model

/*页面初始方法*/
km.init = function () {
    com.initbuttons($('#km_toolbar'), km.model.buttons);
    km.init_parent_model();
    //km.template.init();
    km.maingrid.init();
    km.columngrid.init(); 
}

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

/*执行页面初始化*/
$(km.init);

//页面对象参数设置
km.settings = {};

//格式化数据
km.gridformat = {};

/*
百度模板引擎使用。详情参考官方文档：http://tangram.baidu.com/BaiduTemplate/        
*/
km.template = {
    tpl_add_html: '', jq_add: null,
    initTemplate: function () {
        var data = { title: 'baiduTemplate', list: ['test data 1', 'test data 2', 'test data3'] };
        this.tpl_add_html = baidu.template('tpl_add', data);//使用baidu.template命名空间
        this.jq_add = $(this.tpl_add_html);
    },
    init: function () { this.initTemplate(); }
};

/*数据表datagrid*/
km.maingrid = {
    jq: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#maingrid").datagrid({
            fit: true, border: false, singleSelect: true, rownumbers: true,nowrap:false, remoteSort: false, cache: false, method: 'get', idField: 'TableName',
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["getlist_table"],
            columns: [[
	            { field: 'TableName', title: '表名', width: 150, align: 'left', sortable: true },
                { field: 'TableDescription', title: '表描述', width: 150, align: 'left', sortable: true }
            ]],
            onSelect: function (index, row) { },
            onClickRow: function (index, row) {
                km.maingrid.selectedIndex = index;
                km.maingrid.selectedRow = row;
                km.columngrid.reload(row);
            },
            onLoadSuccess: function (data) { }
        });//end grid init
    },
    reload: function (queryParams) {
        var defaults = { _t: com.settings.timestamp() };
        if (queryParams) { defaults = $.extend(defaults, queryParams); }
        this.jq.datagrid('reload', defaults);
    },
    selectRow: function (index) { this.jq.datagrid('selectRow', index);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*刷新当前选中的行*/ },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); /*获取当前选中的行*/ }
};

/*列信息datagrid*/
km.columngrid = {
    jq: null,
    pageData: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#columngrid").datagrid({
            fit: true, striped: false, border: false, singleSelect: true, rownumbers: true, nowrap: false, remoteSort: false, cache: false, method: 'get', idField: 'ColumnName',
            selectOnCheck: false, checkOnSelect: false, queryParams: { _t: com.settings.timestamp() }, url: km.model.urls["getlist_tablecolumn"],
            columns: [[
                //{ field: 'TableName', title: 'TableName', align: 'center', checkbox: true, width: 50, align: 'left', sortable: true },
	            //{ field: 'TableDescription', title: 'TableDescription', width: 60, align: 'left', sortable: true },
                //{ field: 'ColumnDescription', title: '启用', width: 50, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'ColumnOrder', title: '序号', width: 50, align: 'center', sortable: true },
                { field: 'ColumnName', title: '列名称', width: 80, align: 'left', sortable: true },
                { field: 'ColumnDescription', title: '列描述', width: 150, align: 'left', sortable: true },
                { field: 'ColumnCaption', title: '显示标题', width: 100, align: 'left', sortable: true },
                { field: 'FlagPrimary', title: '主键', width: 50, align: 'center', sortable: true },
                { field: 'FlagIdentity', title: '标识', width: 50, align: 'center', sortable: true },
                { field: 'TypeName', title: '类型', width: 50, align: 'center', sortable: true },
                { field: 'Length', title: '长度', width: 50, align: 'center', sortable: true },
                { field: 'DecimalNumber', title: '小数位数', width: 60, align: 'center', sortable: true },
                { field: 'FlagNullable', title: '允许空', width: 60, align: 'center', sortable: true },
                { field: 'DefaultValue', title: '默认值', width: 90, align: 'center', sortable: true }
            ]], toolbar: '#toolbar1',
            onSelect: function (index, row) { },
            onClickRow: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
                km.columngrid.selectedIndex = index;
                km.columngrid.selectedRow = row;
            },
            onLoadSuccess: function (data) {
                km.columngrid.pageData = data.rows;
                km.columngrid.uncheckAll(); 
            }
        });//end grid init
        return this.jq;
    },
    reload: function (queryParams) {
        var defaults = { _t: com.settings.timestamp() };
        if (queryParams) { defaults = $.extend(defaults, queryParams); }
        //alert(JSON.stringify(defaults));
        this.jq.datagrid('reload', defaults);
    },
    selectRow: function (index) { this.jq.datagrid('selectRow', index);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*刷新当前选中的行*/ },
    checkRow: function (index) { this.jq.datagrid('checkRow', index);/*勾选一行，行索引从0开始*/ },
    uncheckAll: function () { this.jq.datagrid('uncheckAll'); },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); /*获取当前选中的行*/ }
};


/*工具栏权限按钮事件*/
km.toolbar = {
    do_refresh: function () { km.maingrid.reload(); }
};










