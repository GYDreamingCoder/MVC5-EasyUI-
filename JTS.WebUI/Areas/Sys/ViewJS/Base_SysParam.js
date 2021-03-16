/*
//------------------------------------------------------------------------------
// <自动生成>此代码由代码生成器生成 autocode {GenDate}</自动生成>
//------------------------------------------------------------------------------
路径：~/Areas/Sys/ViewJS/Base_SysParam.js
说明：权限按钮(Base_SysParam)的js文件
*/
//当前页面对象
var km = {};
km.model = null;
km.parent_model = null;

km.init = function () {
    com.initbuttons($('#km_toolbar'), km.model.buttons);
    km.init_parent_model();
    km.template.init();
    km.maingrid.init();
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

$(km.init);

//页面对象参数设置
km.settings = {};

//格式化数据
km.gridformat = {};

//百度模板引擎使用 详情：http://tangram.baidu.com/BaiduTemplate/
km.template = {
    tpl_add_html: '',
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
            fit: true, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'ParamCode',
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["pagelist"],
            pagination: true, pageList: [5, 10, 15, 20, 30, 50, 100], pageSize: 15,
            rowStyler: function (row) { if (row.Enabled == 0) { return 'color:red;'; } },
            columns: [[
	            { field: 'ParamCode', title: '参数代码', width: 120, align: 'left', sortable: true },
                { field: 'ParamValue', title: '参数值', width: 350, align: 'left', sortable: true },
                { field: 'Sort', title: '排序', width: 80, align: 'center', sortable: true },
                { field: 'AllowEdit', title: '允许编辑', width: 80, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'Enabled', title: '启用', width: 80, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'Remark', title: '备注', width: 200, align: 'left', sortable: true }
            ]],
            onLoadSuccess: function () { }
        });//end grid init
    },
    reload: function (queryParams) {
        var defaults = { _t: com.settings.timestamp() };
        if (param) { defaults = $.extend(defaults, queryParams); }
        this.jq.datagrid('reload', defaults);
    },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); }
};

/*工具栏权限按钮事件*/
km.toolbar = {
    do_refresh: function () { km.maingrid.reload(); },
    do_add: function () {
        km.template.jq_add.dialog_ext({
            title: '新增系统参数', iconCls: 'icon-standard-add', top: 100,
            onOpenEx: function (win) {
                win.find('#TPL_Enabled').combobox('setValue', 1);
                win.find('#TPL_AllowEdit').combobox('setValue', 1);
                win.find('#TPL_Sort').numberbox('setValue', 100);
                if (km.model.loginer.IsSuperAdmin == 0) { win.find('#TPL_AllowEdit').combobox('readonly', true); }
            },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var jsonStr = JSON.stringify(jsonObject);
                if (jsonObject.ParamCode == "" || jsonObject.ParamValue == "") { flagValid = false; com.message('e', '参数代码或参数名称不能为空！'); return; }
                if (flagValid) {
                    com.ajax({
                        type: 'POST', url: km.model.urls["add"], data: jsonStr, success: function (result) {
                            if (result.s) {
                                com.message('s', result.emsg);
                                win.dialog('destroy');
                                km.maingrid.reload();
                            } else {
                                com.message('e', result.emsg);
                            }
                        }
                    });
                }
            }
        });
    },
    do_edit: function () {
        var sRow = km.maingrid.getSelectedRow();
        if (sRow == null) { layer.msg('请选择一条记录！'); return; }
        if (km.model.loginer.IsSuperAdmin == 0 && sRow.AllowEdit == 0) { layer.msg('此参数不可编辑！'); return; }
        var t = '【' + sRow.ParamCode + '：' + sRow.ParamValue + '】';
        km.template.jq_add.dialog_ext({
            title: '编辑系统参数' + t, iconCls: 'icon-standard-edit', top: 100,
            onOpenEx: function (win) { win.find('#formadd').form('load', sRow); },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var jsonStr = JSON.stringify(jsonObject);
                if (jsonObject.ParamCode == "" || jsonObject.ParamValue == "") { flagValid = false; com.message('e', '参数代码或参数名称不能为空！'); return; }
                if (flagValid) {
                    com.ajax({
                        type: 'POST', url: km.model.urls["edit"], data: jsonStr, success: function (result) {
                            if (result.s) {
                                com.message('s', result.emsg);
                                win.dialog('destroy');
                                km.maingrid.reload();
                            } else {
                                com.message('e', result.emsg);
                            }
                        }
                    });
                }
            }
        });

    },
    do_delete: function () {
        var sRow = km.maingrid.getSelectedRow();
        if (sRow == null) { layer.msg('请选择一条记录！'); return; }
        if (km.model.loginer.IsSuperAdmin == 0 && sRow.AllowEdit == 0) { layer.msg('此参数不可编辑！'); return; }
        var jsonParam = JSON.stringify(sRow);
        com.message('c', ' <b style="color:red">确定要删除系统参数【' + sRow.ParamValue + '】吗？ </b>', function (b) {
            if (b) {
                com.ajax({
                    url: km.model.urls["delete"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', result.emsg);
                            km.maingrid.reload();
                        } else {
                            com.message('e', result.emsg);
                        }
                    }
                });
            }
        });
    },
    do_search: function () { }
};




