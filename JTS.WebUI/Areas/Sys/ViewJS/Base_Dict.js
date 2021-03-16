/*
路径：~/Areas/Sys/ViewJS/Base_Button.js
说明：权限按钮页面Base_Button的js文件
*/
//当前页面对象
var km = {};
km.model = null;
km.parent_model = null;

km.init = function () {
    com.initbuttons($('#km_toolbar'), km.model.buttons);
    km.init_parent_model();
    km.template.init();
    km.maintree.init();
    km.dictitemgrid.init();
    km.set_mode('clear');
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
km.settings = {
    op_mode: ''
};

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
    init: function () {
        this.initTemplate();
    }
};

km.maintree = {
    jq: null,
    treedata: null,
    selectedNode: null,
    selectedData: null,
    setRowData: function (node) {
        var rowData = {
            DictCode: node.attributes.dict_code,
            ParentDictCode: node.attributes.dict_pcode,
            DictName: node.text,
            Sort: node.attributes.dict_sort,
            Enabled: node.attributes.dict_enabled,
            Remark: node.attributes.dict_remark,
        };
        km.maintree.selectedData = rowData;
    },
    init: function () {
        this.treedata = null;
        this.selectedNode = null;
        this.selectedData = null;
        this.jq = $("#maintree").tree({
            method: 'GET', animate: true, url: km.model.urls["getlist_dict"],
            formatter: function (node) {
                var c1 = node.attributes.dict_enabled == 1 ? 'black' : 'gray';
                var c2 = node.attributes.dict_enabled == 1 ? '#133E51' : 'gray';
                var enable_html = node.attributes.dict_enabled == 1 ? '' : '<span style="color:red">[禁用]</span>';
                var code_html = '<span style="color:' + c2 + '">[' + node.attributes.dict_code + ']</span>';
                var title_html = '';
                var h = '<div  style="color:' + c1 + '">' + node.text + code_html + enable_html + '-[' + node.attributes.dict_sort + ']</div>';
                return h;
            },
            onClick: function (node) {// 在用户点击的时候提示
                //alert(JSON.stringify(node)); 
                km.maintree.selectedNode = node;
                km.maintree.setRowData(node);
                km.set_mode('show');
                km.dictitemgrid.reload({ DictCode: node.id });
            },
            onLoadSuccess: function (node, data) { // $("#div_content").html(JSON.stringify(data));
                km.maintree.treedata = data;
                //km.maintree.jq.tree('collapseAll');
            }
        });//end tree init
    }
};

km.dictitemgrid = {
    jq: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#dictitemgrid").datagrid({
            fit: true, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'DictItemCode',
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["getpagelist_dictitem"],
            pagination: true, pageList: [5, 10, 15, 20, 30, 50, 100], pageSize: 15,
            rowStyler: function (index, row) {
                if (row.Enabled == 0) { return 'color:red;'; }
            },
            columns: [[
	            { field: 'DictItemCode', title: '字典代码', width: 80, align: 'left', sortable: true },
                { field: 'DictItemName', title: '字典名称', width: 120, align: 'left', sortable: true },
                { field: 'Sort', title: '排序', width: 50, align: 'center', sortable: true },
                { field: 'Enabled', title: '启用', width: 50, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'Remark', title: '备注', width: 100, align: 'left', sortable: true }
            ]], toolbar: '#toolbar1',
            onSelect: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
                km.dictitemgrid.selectedIndex = index;
                km.dictitemgrid.selectedRow = row;
            },
            onClickRow: function (index, row) { },
            onLoadSuccess: function (data) { }
        });//end grid init
    },
    reload: function (queryParams) {
        var default_QueryParams = { _t: com.settings.timestamp() };
        if (queryParams) {
            default_QueryParams = $.extend(default_QueryParams, queryParams);
        }
        this.jq.datagrid('reload', default_QueryParams);
    },
    selectRow: function () { this.jq.datagrid('selectRow', this.selectedIndex);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*刷新当前选中的行*/ },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); /*获取当前选中的行*/ },
    add_dictitem: function () {
        if (km.maintree.selectedData == null) { layer.msg('请选择一条记录'); return; }
        km.template.jq_add.dialog_ext({
            title: '添加字典', iconCls: 'icon-standard-add',top:100,
            onOpenEx: function (win) {
                win.find('#TPL_DictCode').val(km.maintree.selectedData.DictCode);
                win.find('#TPL_DictItemCode').textbox('setValue', km.maintree.selectedData.DictCode+'_');
                win.find('#TPL_Enabled').combobox('setValue', 1);
                win.find('#TPL_Sort').numberbox('setValue', 100);
            },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var jsonStr = JSON.stringify(jsonObject);
                if (jsonObject.DictItemCode == "" || jsonObject.DictItemName == "") { flagValid = false; com.message('e', '字典代码或字典名称不能为空！'); return; }
                if (jsonObject.Sort == "" || jsonObject.Enable == "") { flagValid = false; com.message('e', '排序和启用状态有误！'); return; }
                if (flagValid) {
                    com.ajax({
                        type: 'POST', url: km.model.urls["add_dictitem"], data: jsonStr, success: function (result) {
                            if (result.s) {
                                com.message('s', result.emsg);
                                win.dialog('destroy');
                                km.dictitemgrid.reload({ DictCode: km.maintree.selectedData.DictCode });
                            } else {
                                com.message('e', result.emsg);
                            }
                        }
                    });
                }
            }
        });
    },
    edit_dictitem: function () {
        var sRow = km.dictitemgrid.getSelectedRow();
        if (sRow == null) { layer.msg('请选择一条记录！'); return; }
        km.template.jq_add.dialog_ext({
            title: '编辑字典', iconCls: 'icon-standard-pencil', top: 100,
            onOpenEx: function (win) {
                win.find('#formadd').form('load', sRow);
                //win.find('#TPL_DictItemCode').textbox().textbox('readonly', true);
            },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var jsonStr = JSON.stringify(jsonObject);
                if (jsonObject.DictItemCode == "" || jsonObject.DictItemName == "") { flagValid = false; com.message('e', '字典代码或字典名称不能为空！'); return; }
                if (flagValid) {
                    com.ajax({
                        type: 'POST', url: km.model.urls["edit_dictitem"], data: jsonStr, success: function (result) {
                            if (result.s) {
                                com.message('s', result.emsg);
                                win.dialog('destroy');
                                km.dictitemgrid.reload({ DictCode: km.maintree.selectedData.DictCode });
                            } else {
                                com.message('e', result.emsg);
                            }
                        }
                    });
                }
            }
        });
    },
    delete_dictitem: function () {
        var sRow = km.dictitemgrid.getSelectedRow();
        if (sRow == null) { layer.msg('请选择一条记录！'); return; } 
        var jsonParam = JSON.stringify(sRow);
        com.message('c', ' <b style="color:red">确定要删除字典【' + sRow.DictItemName + '】吗？ </b>', function (b) {
            if (b) {
                com.ajax({
                    url: km.model.urls["delete_dictitem"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', '删除成功');
                            km.dictitemgrid.reload({ DictCode: km.maintree.selectedData.DictCode });
                        } else {
                            com.message('e', result.emsg);
                        }
                    }
                });
            }
        });
    }
};

/*工具栏权限按钮事件*/
km.toolbar = {
    do_refresh: function () { window.location.reload(); },
    do_add: function () {
        if (km.maintree.selectedData == null) { layer.msg('请选择一个父字典分类'); return; }
        km.set_mode('add');
    },
    do_edit: function () {
        if (km.maintree.selectedData == null) { layer.msg('请选择一条记录'); return; }
        if (km.maintree.selectedData.DictCode == "0") { layer.msg('不可操作'); return; }
        km.set_mode('edit');
    },
    do_delete: function () {
        if (km.maintree.selectedData == null) { layer.msg('请选择一条记录'); return; }
        if (km.maintree.selectedData.DictCode == "0") { layer.msg('不可操作'); return; }
        var jsonParam = JSON.stringify(km.maintree.selectedData);
        com.message('c', ' 确定要删除字典分类【' + km.maintree.selectedData.DictName + '】吗？', function (b) {
            if (b) {
                com.ajax({
                    url: km.model.urls["delete"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', result.emsg);
                            km.toolbar.do_refresh();
                        } else {
                            com.message('e', result.emsg);
                        }
                    }
                });
            }
        });
    },
    do_undo: function () {
        km.set_mode('show');
    },
    do_save: function () {
        var flagValid = true;
        var jsonObject = $('#form_content').serializeJson();
        if (km.settings.op_mode == 'add') {
            jsonObject.ParentDictCode = km.maintree.selectedData.DictCode;
        }
        if (jsonObject.DictCode == "" || jsonObject.DictName == "") { flagValid = false; com.message('e', '字典分类代码或名称不能为空！'); return; }
        if (jsonObject.ParentDictCode == "") { flagValid = false; com.message('e', '父字典分类代码不能为空！'); return; }
        //alert(JSON.stringify(km.maintree.selectedData));
        if (flagValid) {
            com.ajax({
                type: 'POST', url: km.model.urls[km.settings.op_mode], data: JSON.stringify(jsonObject), success: function (result) {
                    if (result.s) {
                        com.message('s', result.emsg);
                        if (km.settings.op_mode == 'add') {
                            km.toolbar.do_refresh();
                        }
                        if (km.settings.op_mode == 'edit') {
                            km.maintree.selectedNode.text = jsonObject.DictName;
                            km.maintree.selectedNode.attributes.dict_sort = jsonObject.Sort;
                            km.maintree.selectedNode.attributes.dict_enabled = jsonObject.Enabled;
                            km.maintree.selectedNode.attributes.dict_remark = jsonObject.Remark;
                            km.maintree.setRowData(km.maintree.selectedNode);
                            km.maintree.jq.tree('update', km.maintree.selectedNode);
                            km.set_mode('show');
                        }
                    } else {
                        com.message('e', result.emsg);
                    }
                }
            });//end com.ajax
        }
    }
};



//设置右侧控件显示模式  flag：'show'=显示模式 'edit'=编辑模式 'add'=新增模式 'clear'=清空模式
km.set_mode = function (flag) {
    km.settings.op_mode = flag;
    $('#km_toolbar').show();
    $('#km_toolbar_2').hide();
    com.mask($('#west_panel'), false);
    com.mask($('#east_panel'), true);
    $('.form_content .easyui-combobox').combobox('readonly', true);
    $('.form_content .easyui-combotree').combotree('readonly', true);
    $('.form_content .easyui-textbox').textbox('readonly', true);
    $('.form_content .easyui-numberbox').numberbox('readonly', true);

    if (flag == 'show') {
        $('#form_content').form('load', km.maintree.selectedData);        //alert(JSON.stringify(km.maintree.selectedData));
        com.mask($('#east_panel'), false);
        if (km.maintree.selectedData.DictCode == '0') {
            com.mask($('#east_panel'), true);
        }
    } else if (flag == 'edit') {
        $('#km_toolbar').hide();
        $('#km_toolbar_2').show();
        com.mask($('#west_panel'), true);
        com.mask($('#east_panel'), true);
        if (km.maintree.selectedData.DictCode != '0') {
            $('.form_content .easyui-combobox').combobox('readonly', false);
            $('.form_content .easyui-combotree').combotree('readonly', false);
            $('.form_content .easyui-textbox').textbox('readonly', false);
            $('.form_content .easyui-numberbox').numberbox('readonly', false);
        }
        $('#TPL_DictCode').textbox('readonly', true);
    }
    else if (flag == 'add') {
        $('#km_toolbar').hide();
        $('#km_toolbar_2').show();
        com.mask($('#west_panel'), true);
        com.mask($('#east_panel'), true);
        $('#form_content').form('clear');
        $('#TPL_Sort').numberbox('setValue', 100);
        $('#TPL_Enabled').combobox('setValue', 1);

        $('.form_content .easyui-combobox').combobox('readonly', false);
        $('.form_content .easyui-combotree').combotree('readonly', false);
        $('.form_content .easyui-textbox').textbox('readonly', false);
        $('.form_content .easyui-numberbox').numberbox('readonly', false);
    } else if (flag == 'clear') {
        $('#form_content').form('clear');
        km.dictitemgrid.reload({ mode: 'clear' });
    }
}



$(km.init);
