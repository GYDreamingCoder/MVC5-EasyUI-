/*
路径：~/Areas/Sys/ViewJS/Base_Menu.js
说明：系统菜单页面Base_Menu的js文件
*/
//当前页面对象
var km = {};
km.model = null;//当前model对象
km.parent_model = null;//存储父页面的model对象 parent.wrapper.model

/*页面初始方法*/
km.init = function () {
    com.initbuttons($('#km_toolbar'), km.model.buttons);
    km.init_parent_model();
    km.template.init();
    km.maingrid.init();
    km.menubuttongrid.init();
    //km.enableToolbar(false);
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
km.settings = {
    currentMenuButtons: null,//存储当前获取的菜单按钮数据
    isInitButtonGrid: false //标记是否已经初始化按钮表格 buttongrid
};

//格式化数据
km.gridformat = {};

/*
百度模板引擎使用。详情参考官方文档：http://tangram.baidu.com/BaiduTemplate/        
*/
km.template = {
    tpl_add_html: '',//tpl_add模板的html
    tpl_icon_html: '',
    jq_add: null,
    jq_icon: null,
    initTemplate: function () {
        var data = { title: 'baiduTemplate', list: ['test data 1', 'test data 2', 'test data3'] };
        this.tpl_add_html = baidu.template('tpl_add', data);//使用baidu.template命名空间
        this.jq_add = $(this.tpl_add_html);

        var data16 = $.kmui.icons.all;
        var listData = { "title": 'icon16x16', "list": data16 }
        this.tpl_icon_html = baidu.template('tpl_icon', listData);
        this.jq_icon = $(this.tpl_icon_html);
    },
    init: function () {
        this.initTemplate();
    }
};

/*菜单datagrid*/
km.maingrid = {
    jq: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#maingrid").treegrid({
            fit: true, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'MenuCode', treeField: 'MenuName',
            queryParams: { _t: com.settings.timestamp() }, url: km.model.urls["list"],
            rowStyler: function (row) {
                if (row.Enabled == 0) {
                    return 'color:red;';
                }
            },
            columns: [[
	            { field: 'MenuCode', title: '菜单代码', width: 110, align: 'left' },
                { field: 'MenuName', title: '菜单名称', width: 130, align: 'left' },
                { field: 'Url', title: '菜单URL', width: 180, align: 'left' },
                {
                    field: 'MenuType', title: '菜单类型', width: 55, align: 'center', formatter: function (value, row, index) {
                        //菜单类型：0=未定义 1=目录 2=页面 7=备用1 8=备用2 9=备用3   
                        var h = '未定义';
                        if (value == 1) { h = '目录'; } else if (value == 2) { h = '页面'; }
                        else if (value == 7) { h = '备用1'; } else if (value == 8) { h = '备用2'; } else if (value == 9) { h = '备用3'; }
                        return h;
                    }
                },
                {
                    field: 'ButtonMode', title: '按钮模式', width: 55, align: 'center', formatter: function (value, row, index) {
                        //按钮模式：0=未定义 1=动态按钮 2=静态按钮 3=无按钮
                        var h = '未定义';
                        if (value == 1) { h = '动态按钮'; } else if (value == 2) { h = '静态按钮'; } else if (value == 3) { h = '无按钮'; }
                        return h;
                    }
                },
                { field: 'Sort', title: '排序', width: 50, align: 'center' },
                { field: 'Enabled', title: '启用', width: 50, align: 'center', formatter: com.html_formatter.yesno },
                { field: 'Remark', title: '备注', width: 50, align: 'left' }
                //{
                //    field: 'OP', title: '操作', width: 80, formatter: function (value, row, index) {
                //        var title = '菜单[' + row.MenuName + ']按钮';
                //        var html = '<a href="javascript:void(0);" title=' + title + ' onclick="km.loadMenuButtons(\'' + row.MenuCode + '\');"><span class="icon icon-standard-page">&nbsp;&nbsp;</span>菜单按钮</a>';
                //        return html;
                //    }
                //}
            ]],
            loadFilter: function (data) {
                var d = utils.copyProperty(data.rows || data, ["MenuCode", "IconClass"], ["_id", "iconCls"], false);
                var resultData = utils.toTreeData(d, '_id', 'ParentCode', "children");
                return resultData;
            },
            onClickRow: function (row) {
                //点击行行事件，将当前选中行的的索引和行数据存储起来
                //km.maingrid.selectedIndex = index;
                km.maingrid.selectedRow = row;
                km.loadMenuButtons(row);
                //com.message('s', JSON.stringify(km.maingrid.selectedRow));
            },
            onLoadSuccess: function (data) {
                //alert('load data successfully!');
                if (km.maingrid.selectedIndex > 0) {
                    km.maingrid.selectRow();
                }
            }
        });//end grid init
    },
    refreshRow: function () { this.jq.treegrid('refreshRow', this.selectedIndex);/*刷新当前选中的行*/ },
    reload: function () { this.jq.treegrid('reload', { _t: com.settings.timestamp() }); },
    selectRow: function () { this.jq.treegrid('selectRow', this.selectedIndex);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.treegrid('selectRecord', idValue);/*选中当前选中的行*/ },
    deleteRow: function () { this.jq.treegrid('deleteRow', this.selectedIndex);/*删除当前选中的行*/ },
    getSelectedRow: function () { return this.jq.treegrid('getSelected'); /*获取当前选中的行*/ }
};

/*菜单按钮datagrid*/
km.menubuttongrid = {
    jq: null,
    pageData: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#menubuttongrid").datagrid({
            fit: true, striped: false, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'ButtonCode',
            selectOnCheck: false, checkOnSelect: false,
            queryParams: { _t: com.settings.timestamp(), menucode: "" }, url: km.model.urls["getmenubuttons"],
            pagination: true, pageList: [5, 10, 15, 20, 30, 50, 100], pageSize: 10,
            columns: [[
                { field: 'ck', title: '', align: 'center', checkbox: true, width: 50, sortable: true },
                //{ field: 'MenuCode', title: '菜单代码', width: 70, align: 'left', sortable: true },
                { field: 'ButtonCode', title: '按钮代码', width: 70, align: 'left', sortable: true },
                {
                    field: 'ButtonName', title: '按钮名称', width: 80, align: 'left', sortable: true, formatter: function (value, row, index) {
                        return '&nbsp;&nbsp;<a href="#"><span class="icon ' + row.IconClass + '">&nbsp;</span>&nbsp;' + value + '</a>';
                    }
                },
                { field: 'ButtonSort', title: '按钮顺序', width: 70, align: 'left', sortable: true, editor: { type: 'numberbox' } },
                { field: 'ButtonText', title: '按钮文本', width: 70, align: 'left', sortable: true, editor: { type: 'textbox' } },
                {
                    field: 'ButtonType', title: '按钮类型', width: 80, align: 'center', sortable: true, formatter: function (value, row, index) {
                        //按钮类型 0=未定义 1=工具栏按钮 2=右键按钮 3=其他(待定)
                        var h = '未定义';
                        if (value == 1) { h = '工具栏按钮'; } else if (value == 2) { h = '右键按钮'; } else if (value == 3) { h = '其他'; }
                        return h;
                    }
                }
                //{ field: 'JsEvent', title: 'js事件', width: 80, align: 'left', sortable: true }
            ]], toolbar: '#toolbar1',
            onSelect: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
                km.menubuttongrid.selectedIndex = index;
                km.menubuttongrid.selectedRow = row;
                km.menubuttongrid.jq.datagrid('beginEdit', index);
            },
            onClickRow: function (index, row) {
                var i = index;
            },
            onLoadSuccess: function (data) {
                km.menubuttongrid.pageData = data.rows;
                if (km.menubuttongrid.pageData != null && km.menubuttongrid.pageData.length > 0) {
                    for (var i = 0; i < km.menubuttongrid.pageData.length; i++) {
                        km.menubuttongrid.jq.datagrid('beginEdit', i);
                    }
                }
            }
        });//end grid init
        return this.jq;
    },
    refreshRow: function () { this.jq.datagrid('refreshRow', this.selectedIndex);/*刷新当前选中的行*/ },
    reload: function () { this.jq.datagrid('reload', { _t: com.settings.timestamp(), menucode: km.maingrid.selectedRow.MenuCode }); },
    selectRow: function () { this.jq.datagrid('selectRow', this.selectedIndex);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*刷新当前选中的行*/ },
    deleteRow: function () { this.jq.datagrid('deleteRow', this.selectedIndex);/*删除当前选中的行*/ },
    checkRow: function (index) { this.jq.datagrid('checkRow', index);/*勾选一行，行索引从0开始*/ },
    uncheckAll: function () { this.jq.datagrid('uncheckAll'); },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); /*获取当前选中的行*/ },
    add_button: function () {
        if (km.maingrid.selectedRow == null) { com.message('w', "未点击菜单按钮！"); return; }
        var t = '给菜单 【' + km.maingrid.selectedRow.MenuName + '[' + km.maingrid.selectedRow.MenuCode + '] 】添加按钮';
        var menucode = km.maingrid.selectedRow.MenuCode;
        $("#dd_button").dialog_page({
            title: t, iconCls: 'icon-standard-add', resizable: true, maximizable: true, width: 900, height: 430,
            onClickButton: function (self) { //保存操作
                var checkedRows = km.buttongrid.getChecked();
                var jsonStr = JSON.stringify(checkedRows);  //alert("选择数据：" + jsonStr) 
                if (checkedRows.length == 0) {
                    com.message('w', '未勾选记录！'); return;
                }
                var jsonParam = JSON.stringify({ menucode: menucode, crows: checkedRows });
                com.ajax({
                    type: 'POST', url: km.model.urls["addmenubuttons"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', '按钮添加成功！');
                            self.dialog('close');
                            km.menubuttongrid.reload();
                        } else {
                            com.message('w', '按钮添加失败！');
                        }
                    }
                });// end ajax  
            }
        });
    },
    remove_button: function () {
        //com.message('w', '待实现'); 
        var checkedRows = km.menubuttongrid.jq.datagrid('getChecked');
        var jsonStr = JSON.stringify(checkedRows);
        if (checkedRows.length == 0) {
            com.message('w', '未勾选记录！'); return;
        }
        var jsonParam = JSON.stringify({ menucode: checkedRows[0].MenuCode, crows: checkedRows });
        com.message('c', '确定要移除勾选的【' + checkedRows.length + '】个按钮吗？', function (ok) {
            if (ok) {
                com.ajax({
                    type: 'POST', url: km.model.urls["removemenubuttons"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', '按钮移除成功！');
                            km.menubuttongrid.reload();
                        } else {
                            com.message('w', '按钮移除失败！');
                        }
                    }
                });// end ajax  
            }
        }); // end message  
    },
    save_button: function () {
        if (km.menubuttongrid.pageData == null || km.menubuttongrid.pageData.length == 0) {
            com.message('w', '没有可以保存的数据！'); return;
        }
        com.message('c', '确定要保存按钮的顺序和显示文本信息吗？', function (ok) {
            if (ok) {
                km.menubuttongrid.jq.datagrid('acceptChanges');
                var rows = km.menubuttongrid.jq.datagrid('getRows');
                var jsonParam = JSON.stringify({ menucode: rows[0].MenuCode, crows: rows });
                //alert(JSON.stringify(jsonParam));
                com.ajax({
                    type: 'POST', url: km.model.urls["savemenubuttons"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', '保存成功！');
                            km.menubuttongrid.reload();
                        } else {
                            com.message('w', '保存失败！');
                        }
                    }
                });// end ajax  
            }
        }); // end message  
    }
};

/*按钮datagrid*/
km.buttongrid = {
    jq: null,
    pageData: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function (target) {
        this.jq = target.datagrid({
            fit: true, striped: false, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'ButtonCode',
            selectOnCheck: false, checkOnSelect: false,
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["pagelist_button"],
            pagination: true, pageList: [5, 10, 15, 20], pageSize: 10,
            columns: [[
                { field: 'ck', title: '', align: 'center', checkbox: true, width: 50, sortable: true },
	          { field: 'ButtonCode', title: '按钮代码', width: 100, align: 'left', sortable: true },
                {
                    field: 'ButtonName', title: '按钮名称', width: 100, align: 'left', sortable: true, formatter: function (value, row, index) {
                        return '&nbsp;&nbsp;<a href="#"><span class="icon ' + row.IconClass + '">&nbsp;</span>&nbsp;' + value + '</a>';
                    }
                },
                { field: 'JsEvent', title: 'js事件', width: 90, align: 'left', sortable: true },
                {
                    field: 'ButtonType', title: '按钮类型', width: 80, align: 'center', sortable: true, formatter: function (value, row, index) {
                        //按钮类型 0=未定义 1=工具栏按钮 2=右键按钮 3=其他(待定)
                        var h = '未定义';
                        if (value == 1) { h = '工具栏按钮'; } else if (value == 2) { h = '右键按钮'; } else if (value == 3) { h = '其他'; }
                        return h;
                    }
                },
                { field: 'IconClass', title: '图标样式', width: 150, align: 'left', sortable: true },
                //{ field: 'IconUrl', title: '图标Url', width: 80, align: 'center' },
                { field: 'Sort', title: '排序', width: 60, align: 'center', sortable: true },
                //{ field: 'Split', title: '是否分隔符', width: 80, align: 'center' },
                { field: 'Enabled', title: '启用', width: 60, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'Remark', title: '备注', width: 80, align: 'left', sortable: true }
            ]], toolbar: '#toolbar2',
            onSelect: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
                km.buttongrid.selectedIndex = index;
                km.buttongrid.selectedRow = row;
            },
            onClickRow: function (index, row) {
                var i = index;
            },
            onLoadSuccess: function (data) {
                km.buttongrid.pageData = data.rows;
            }
        });//end grid init
        return this.jq;
    },
    refreshRow: function () { this.jq.datagrid('refreshRow', this.selectedIndex);/*刷新当前选中的行*/ },
    reload: function (queryParams) {
        var default_QueryParams = { _t: com.settings.timestamp() };
        if (queryParams) {
            default_QueryParams = $.extend(default_QueryParams, queryParams);
        }
        this.jq.datagrid('reload', default_QueryParams);
    },
    selectRow: function () { this.jq.datagrid('selectRow', this.selectedIndex);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*刷新当前选中的行*/ },
    deleteRow: function () { this.jq.datagrid('deleteRow', this.selectedIndex);/*删除当前选中的行*/ },
    checkRow: function (index) { this.jq.datagrid('checkRow', index);/*勾选一行，行索引从0开始*/ },
    uncheckAll: function () { this.jq.datagrid('uncheckAll'); },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); /*获取当前选中的行*/ },
    getChecked: function () { return this.jq.datagrid('getChecked'); /*获取当前勾选的行*/ },
    search_button: function () {
        var keyword = $("#search_key").textbox('getValue');
        km.buttongrid.reload({ keyword: keyword });
    }
};

/*工具栏权限按钮事件*/
km.toolbar = {
    do_browse: function () { },
    do_refresh: function () {
        km.maingrid.reload(); //window.location.reload();
    },
    do_add: function () {
        km.template.jq_add.dialog_ext({
            title: '新增菜单', iconCls: 'icon-standard-table-add',
            onOpenEx: function (win) {
                var sRow = km.maingrid.getSelectedRow();
                var icon = 'icon-standard-bricks';
                win.find("#span_icon").removeClass();
                win.find("#span_icon").addClass("icon").addClass(icon);
                win.find('#TPL_IconClass').textbox({
                    buttonText: '选择图标', buttonIcon: '', editable: false, value: icon, onClickButton: function () {
                        km.initIcon(win);
                    }
                });

                win.find('#TPL_MenuType').combobox('setValue', 2);
                win.find('#TPL_ButtonMode').combobox('setValue', 1);
                win.find('#TPL_Sort').numberbox('setValue', 200);
                win.find('#TPL_Enabled').combobox('setValue', 1);
                win.find('#TPL_ParentMenuCode').combotree({
                    url: km.model.urls["list"], editable: false, loadFilter: function (data) {
                        var d = utils.copyProperty(data.rows || data, ["MenuCode", "IconClass", "MenuName"], ["id", "iconCls", "text"], false);
                        var resultData = utils.toTreeData(d, 'id', 'ParentCode', "children");
                        return resultData;
                    }
                });
                if (sRow != null) {
                    win.find('#TPL_ParentMenuCode').combotree('setValue', sRow.MenuCode);
                }
            },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var parentCode = win.find('#TPL_ParentMenuCode').combotree('getValue');
                if (parentCode == null || parentCode == "") {
                    flagValid = false; com.message('e', '父级菜单不能为空！'); return;
                }
                if (jsonObject.MenuCode == "" || jsonObject.MenuName == "") {
                    flagValid = false; com.message('e', '菜单代码或菜单名称不能为空！'); return;
                }
                if (jsonObject.IconClass == "" || jsonObject.Url == "") {
                    flagValid = false; com.message('e', '菜单图标Class或菜单Url不能为空！'); return;
                }
                jsonObject.ParentCode = parentCode;
                var jsonStr = JSON.stringify(jsonObject);
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
            title: '编辑菜单【' + sRow.MenuName + '】', iconCls: 'icon-standard-table-edit',
            onOpenEx: function (win) {
                win.find("#span_icon").removeClass();
                var icon = 'icon-standard-bricks';
                if (sRow.IconClass != "" && sRow.IconClass != null) {
                    icon = sRow.IconClass;
                }
                win.find("#span_icon").addClass("icon").addClass(icon);
                win.find('#TPL_IconClass').textbox({
                    buttonText: '选择图标', buttonIcon: '', editable: false, value: icon, onClickButton: function () {
                        km.initIcon(win);
                    }
                });
                win.find('#TPL_ParentMenuCode').combotree({
                    url: km.model.urls["list"], editable: false, loadFilter: function (data) {
                        var d = utils.copyProperty(data.rows || data, ["MenuCode", "IconClass", "MenuName"], ["id", "iconCls", "text"], false);
                        var resultData = utils.toTreeData(d, 'id', 'ParentCode', "children");
                        return resultData;
                    }
                }).combotree('setValue', sRow.ParentCode);
                win.find('#formadd').form('load', sRow);
                win.find('#TPL_MenuCode').textbox().textbox('readonly', true);
                //console.info(JSON.stringify(sRow));
            },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var parentCode = win.find('#TPL_ParentMenuCode').combotree('getValue');
                if (parentCode == null || parentCode == "") {
                    flagValid = false;
                    com.message('e', '父级菜单不能为空！'); return;
                }
                var jsonObject = win.find("#formadd").serializeJson();

                if (jsonObject.MenuCode == "" || jsonObject.MenuName == "") {
                    flagValid = false;
                    com.message('e', '菜单代码或菜单名称不能为空！'); return;
                }
                jsonObject.ParentCode = parentCode;
                var jsonStr = JSON.stringify(jsonObject);
                //alert(jsonStr);
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
    },/*编辑*/
    do_delete: function () {
        var sRow = km.maingrid.getSelectedRow();
        if (sRow == null) { layer.msg('请选择一条记录！'); return; }
        if (sRow.MenuType == 1) { layer.msg('系统菜单不能删除！'); return; }

        var jsonParam = JSON.stringify(sRow);
        com.message('c', ' <b style="color:red">确定要删除菜单【' + sRow.MenuName + '】吗（请慎重）？ </b>', function (b) {
            if (b) {
                com.ajax({
                    url: km.model.urls["delete"], data: jsonParam, success: function (result) {
                        //layer.msg(result.emsg); 
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


/*根据菜单代码获取菜单按钮   按钮模式：0=未定义 1=动态按钮 2=静态按钮 3=无按钮*/
km.loadMenuButtons = function (row) {
    if (row.MenuType == 2 && row.ButtonMode == 1 && row.Enabled == 1) {
        $("#east_panel").show();
        //km.enableToolbar(true);
        var east_panel = $(".easyui-layout").layout("panel", "east");
        east_panel.panel('setTitle', '设置菜单【' + row.MenuName + '】的按钮');
        km.menubuttongrid.jq.datagrid('reload', { _t: com.settings.timestamp(), menucode: row.MenuCode });
    } else {
        $("#east_panel").hide();
        //km.enableToolbar(false);
        var east_panel = $(".easyui-layout").layout("panel", "east");
        east_panel.panel('setTitle', '【' + row.MenuName + '】');
        //km.menubuttongrid.jq.datagrid('reload', { _t: com.settings.timestamp(), menucode: "" });
    }

    //加载菜单的时候再初始化待选择的按钮
    if (km.settings.isInitButtonGrid == false) {
        km.buttongrid.init($("#buttongrid"));
        km.settings.isInitButtonGrid == true;
    }
    // km.maingrid.selectRecord(menucode);//选中菜单当前点击的行
}


km.enableToolbar = function (enable) {
    var t = enable == true ? 'enable' : 'disable';
    $('#btn_add_menubutton').linkbutton(t);
    $('#btn_remove_menubutton').linkbutton(t);
    $('#btn_save_menubutton').linkbutton(t);
}




//初始化图标选择
km.initIcon = function (addwin) {
    //addwin.find('#TPL_IconClass').textbox({
    //    buttonText: '选择',
    //    buttonIcon: '', editable: false,
    //    onClickButton: function () {
    //        km.initIcon(win);
    //    }
    //});
    var $this = addwin.find('#TPL_IconClass').textbox();
    var $spanicon = addwin.find('#span_icon');
    km.showIconSelector(km.template.tpl_icon_html, function (win) {
        var hwin = win.html();
        win.find("span").click(function () {
            //alert($(this)[0].id);
            var old_icontext = win.find("#icontext").text();
            if (old_icontext != "") {
                win.find("#" + old_icontext).removeClass("icon_selected");
            }
            var new_icontext = $(this)[0].id;
            $(this).addClass("icon_selected");
            win.find("#icontext").text(new_icontext);
            win.find("#span_selected_icon").removeClass();
            win.find("#span_selected_icon").addClass("icon ").addClass(new_icontext).addClass("iconbox");

        });
    }, function (win) {
        var selected_icontext = win.find("#icontext").text();
        if (selected_icontext == "") {
            com.message('w', '请选择一个图标');
            return;
        }
        //console.info($this);
        $this.textbox('setValue', selected_icontext);
        $spanicon.removeClass().addClass("icon").addClass(selected_icontext);
        $spanicon.attr('title', selected_icontext);
        parent.$(win).dialog('destroy');
        //alert(selected_icontext);
    });
}
//显示图标选择对话框
km.showIconSelector = function (html, before, dosave) {
    com.dialog({
        title: '图标选择',
        html: html, resizable: true, maximizable: true, width: 600,
        height: 400, btntext: '确定选择'
    }, before, dosave);
}