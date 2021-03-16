/*
路径：~/Areas/Sys/ViewJS/Base_CodeRule.js
说明：单据编号规则页面Base_CodeRule的js文件
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
            fit: true, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'RuleCode',
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["pagelist"],
            pagination: true, pageList: [5, 10, 15, 20, 30, 50, 100], pageSize: 15,
            rowStyler: function (row) { if (row.Enabled == 0) { return 'color:red;'; } },
            columns: [[
	            { field: 'RuleCode', title: '规则代码', width: 100, align: 'left', sortable: true },
                { field: 'RuleName', title: '规则名称', width: 200, align: 'left', sortable: true },
                { field: 'StartChars', title: '开始字符', width: 80, align: 'left', sortable: true },
                {
                    field: 'Format', title: '填充格式', width: 80, align: 'left', sortable: true, formatter: function (value, row, index) {
                        var h = '无填充格式'; if (value == 'date') { h = '日期格式填充' } return h;
                    }
                },
                { field: 'FillChar', title: '填充字符', width: 80, align: 'left', sortable: true },
                { field: 'Seed', title: '种子', width: 80, align: 'center', sortable: true },
                { field: 'Length', title: '长度', width: 80, align: 'center', sortable: true },
                { field: 'Sort', title: '排序', width: 80, align: 'center', sortable: true },
                { field: 'Enabled', title: '启用', width: 80, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'Remark', title: '备注', width: 200, align: 'left', sortable: true }
            ]],
            onLoadSuccess: function () {
                //alert('load data successfully!');
            }
        });//end grid init
    },
    reload: function (param) {
        var defaults = { _t: com.settings.timestamp() };
        if (param) {
            defaults = $.extend(defaults, param);
        }
        this.jq.datagrid('reload', defaults);
    },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); }
};

/*工具栏权限按钮事件*/
km.toolbar = {
    do_browse: function () { },
    do_refresh: function () {
        km.maingrid.reload(); //window.location.reload();
    },
    do_add: function () {
        km.template.jq_add.dialog_ext({
            title: '新增单据编码规则', iconCls: 'icon-standard-add', top: 100,
            onOpenEx: function (win) {
                win.find('#TPL_Format').combobox('setValue', 'none');
                win.find('#TPL_FillChar').textbox('setValue', '0');
                win.find('#TPL_Sort').numberbox('setValue', 100);
                win.find('#TPL_Enabled').combobox('setValue', 1);
            },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var jsonStr = JSON.stringify(jsonObject);
                if (jsonObject.RuleCode == "" || jsonObject.RuleName == "") { flagValid = false; com.message('e', '规则代码或规则名称不能为空！'); return; }
                if (jsonObject.StartChars == "" || jsonObject.Format == "" || jsonObject.FillChar == "") { flagValid = false; com.message('e', '开始字符或填充格式或填充字符不能为空！'); return; }
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
        km.template.jq_add.dialog_ext({
            title: '编辑单据编码规则', iconCls: 'icon-standard-pencil', top: 100,
            onOpenEx: function (win) { win.find('#formadd').form('load', sRow); },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var jsonStr = JSON.stringify(jsonObject);
                if (jsonObject.RuleCode == "" || jsonObject.RuleName == "") { flagValid = false; com.message('e', '规则代码或规则名称不能为空！'); return; }
                if (jsonObject.StartChars == "" || jsonObject.Format == "" || jsonObject.FillChar == "") { flagValid = false; com.message('e', '开始字符或填充格式或填充字符不能为空！'); return; }

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
        com.message('c', ' <b style="color:red">确定要删除系统参数【' + sRow.RuleName + '】吗？ </b>', function (b) {
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
    do_search: function () {

    }
};



$(km.init);
