/*
路径：~/Areas/Sys/ViewJS/Base_Role.js
说明：系统角色页面Base_Role的js文件
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
    km.roleusergrid.init();
    $('#btn_add_roleuser').linkbutton('disable');
    $('#btn_remove_roleuser').linkbutton('disable');
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
    currentRoleUsers: null,//存储当前获取的角色用户数据
    isInitUserGrid: false //标记是否已经初始化用户表格 usergrid
};

//格式化数据
km.gridformat = {};

/*
百度模板引擎使用。详情参考官方文档：http://tangram.baidu.com/BaiduTemplate/        
*/
km.template = {
    tpl_add_html: '',//tpl_add模板的html
    jq_add: null,
    tpl_icon_html: '',
    initTemplate: function () {
        var data = { title: 'baiduTemplate', list: ['test data 1', 'test data 2', 'test data3'] };
        this.tpl_add_html = baidu.template('tpl_add', data);//使用baidu.template命名空间
        this.jq_add = $(this.tpl_add_html);
    },
    init: function () {
        this.initTemplate();
    }
};

/*角色datagrid*/
km.maingrid = {
    jq: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#maingrid").datagrid({
            fit: true, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'RoleCode',
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["pagelist"],
            pagination: true, pageList: [5, 10, 15, 20, 30, 50, 100], pageSize: 15,
            rowStyler: function (index, row) {
                if (row.Enabled == 0) {
                    return 'color:red;';
                }
            },
            columns: [[
	            { field: 'RoleCode', title: '角色代码', width: 80, align: 'left', sortable: true },
                { field: 'RoleName', title: '角色名称', width: 120, align: 'left', sortable: true },
                {
                    field: 'RoleType', title: '角色类型', width: 80, align: 'center', sortable: true, formatter: function (value, row, index) {
                        //角色类型：0=未定义 1=系统角色 2=业务角色 3=其他 （系统角色不允许编辑和删除）
                        var h = '未定义';
                        if (value == 1) { h = '系统角色'; } else if (value == 2) { h = '业务角色'; } else if (value == 3) { h = '其他'; }
                        return h;
                    }
                },
                { field: 'Sort', title: '排序', width: 50, align: 'center', sortable: true },
                { field: 'Enabled', title: '启用', width: 50, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'Remark', title: '备注', width: 100, align: 'left', sortable: true },
                {
                    field: 'OP', title: '操作', width: 80, formatter: function (value, row, index) {
                        if (row.Enabled == 0) { return ''; }
                        var title = '角色[' + row.RoleName + ']的用户';
                        var html = '<a href="javascript:void(0);" title=' + title + ' onclick="km.getRoleUsers(\'' + row.RoleCode + '\');"><span class="icon icon-standard-user">&nbsp;&nbsp;</span>角色用户</a>';
                        return html;
                    }
                }
                //{ field: 'AddBy', title: '创建人', width: 80, align: 'center' },
                //{ field: 'AddOn', title: '创建时间', width: 80, align: 'center' },
                //{ field: 'EditBy', title: '编辑人', width: 80, align: 'center' },
                //{ field: 'EditOn', title: '编辑时间', width: 80, align: 'center' },
            ]],
            onSelect: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
                km.maingrid.selectedIndex = 0;
                km.maingrid.selectedRow = null;
                km.maingrid.selectedIndex = index;
                km.maingrid.selectedRow = row;
            },
            onClickRow: function (index, row) {
                var i = index;
            },
            onLoadSuccess: function (data) {
                //alert('load data successfully!');
                if (km.maingrid.selectedIndex > 0) {
                    km.maingrid.selectRow();
                }
            }
        });//end grid init
    },
    refreshRow: function () { this.jq.datagrid('refreshRow', this.selectedIndex);/*刷新当前选中的行*/ },
    reload: function (param) {
        var defaults = { _t: com.settings.timestamp() };
        if (param) {
            defaults = $.extend(defaults, param);
        }
        this.jq.datagrid('reload', defaults);
    },
    selectRow: function () { this.jq.datagrid('selectRow', this.selectedIndex);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*刷新当前选中的行*/ },
    deleteRow: function () { this.jq.datagrid('deleteRow', this.selectedIndex);/*删除当前选中的行*/ },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); /*获取当前选中的行*/ }
};

/*角色用户datagrid*/
km.roleusergrid = {
    jq: null,
    pageData: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#roleusergrid").datagrid({
            fit: true, striped: false, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'UserId',
            selectOnCheck: false, checkOnSelect: false,
            queryParams: { _t: com.settings.timestamp(), rolecode: "" }, url: km.model.urls["getroleusers"],
            pagination: true, pageList: [5, 10, 15, 20, 30, 50, 100], pageSize: 10,
            columns: [[
                { field: 'ck', title: '', align: 'center', checkbox: true, width: 50, sortable: true },
	            { field: 'UserId', title: '编号', width: 60, align: 'left', sortable: true },
                { field: 'RealName', title: '姓名', width: 80, align: 'left', sortable: true },
                //{ field: 'UserCode', title: '登录账号', width: 150, align: 'left', sortable: true },
                {
                    field: 'UserType', title: '用户类型', width: 80, align: 'center', sortable: true, formatter: function (value, row, index) {
                        //用户类型：0=未定义 1=管理员 3=普通用户 5=微信用户 7=ERP用户 9=OA用户  99=其他
                        var h = '未定义';
                        if (value == 1) { h = '管理员'; } else if (value == 3) { h = '普通用户'; }
                        else if (value == 5) { h = '微信用户'; } else if (value == 7) { h = 'ERP用户'; }
                        else if (value == 9) { h = 'OA用户'; } else if (value == 99) { h = '其他'; }
                        return h;
                    }
                },
                //{ field: 'Sort', title: '排序', width: 50, align: 'center', sortable: true },
                { field: 'Enabled', title: '启用', width: 50, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                //{ field: 'Remark', title: '备注', width: 100, align: 'left', sortable: true }
            ]], toolbar: '#toolbar1',
            onSelect: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
                km.roleusergrid.selectedIndex = index;
                km.roleusergrid.selectedRow = row;
            },
            onClickRow: function (index, row) {
                var i = index;
            },
            onLoadSuccess: function (data) {
                km.roleusergrid.pageData = data.rows;
                km.roleusergrid.uncheckAll();
            }
        });//end grid init
        return this.jq;
    },
    refreshRow: function () { this.jq.datagrid('refreshRow', this.selectedIndex);/*刷新当前选中的行*/ },
    reload: function () { this.jq.datagrid('reload', { _t: com.settings.timestamp(), rolecode: km.maingrid.selectedRow.RoleCode }); },
    selectRow: function () { this.jq.datagrid('selectRow', this.selectedIndex);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*刷新当前选中的行*/ },
    deleteRow: function () { this.jq.datagrid('deleteRow', this.selectedIndex);/*删除当前选中的行*/ },
    checkRow: function (index) { this.jq.datagrid('checkRow', index);/*勾选一行，行索引从0开始*/ },
    uncheckAll: function () { this.jq.datagrid('uncheckAll'); },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); /*获取当前选中的行*/ },
    add_user: function () {
        if (km.maingrid.selectedRow == null) { com.message('w', "未点击角色用户！"); return; }
        var t = '给角色' + km.maingrid.selectedRow.RoleName + '[' + km.maingrid.selectedRow.RoleCode + ']添加用户';
        var rolecode = km.maingrid.selectedRow.RoleCode;
        $("#dd_user").dialog_page({
            title: t, iconCls: 'icon-standard-group-add', resizable: true, maximizable: true, inline: true, width: 600, height: 350,
            onClickButton: function (self) { //保存操作
                var checkedRows = km.usergrid.getChecked();
                var jsonStr = JSON.stringify(checkedRows);  //alert("选择数据：" + jsonStr) 
                if (checkedRows.length == 0) {
                    com.message('w', '未勾选记录！'); return;
                }
                var jsonParam = JSON.stringify({ rolecode: rolecode, crows: checkedRows });
                com.ajax({
                    type: 'POST', url: km.model.urls["addroleusers"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', '用户添加成功！');
                            self.dialog('close');
                            km.roleusergrid.reload();
                        } else {
                            com.message('w', '用户添加失败！');
                        }
                    }
                });// end ajax  
            }
        });
    },
    remove_user: function () {
        //com.message('w', '待实现'); 
        var checkedRows = km.roleusergrid.jq.datagrid('getChecked');
        var jsonStr = JSON.stringify(checkedRows);
        if (checkedRows.length == 0) {
            com.message('w', '未勾选记录！'); return;
        }
        var jsonParam = JSON.stringify({ rolecode: checkedRows[0].RoleCode, crows: checkedRows });
        com.message('c', '确定要移除勾选的【' + checkedRows.length + '】个用户吗？', function (ok) {
            if (ok) {
                com.ajax({
                    type: 'POST', url: km.model.urls["removeroleusers"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', '用户移除成功！');
                            km.roleusergrid.reload();
                        } else {
                            com.message('w', '用户移除失败！');
                        }
                    }
                });// end ajax  
            }
        }); // end message  
    }
};

/*用户datagrid*/
km.usergrid = {
    jq: null,
    pageData: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function (target) {
        this.jq = target.datagrid({
            fit: true, striped: false, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'UserId',
            selectOnCheck: false, checkOnSelect: false,
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["pagelist_user"],
            pagination: true, pageList: [5, 10, 15, 20], pageSize: 10,
            columns: [[
                { field: 'ck', title: '', align: 'center', checkbox: true, width: 50, sortable: true },
	            { field: 'UserId', title: '编号', width: 60, align: 'left', sortable: true },
                { field: 'RealName', title: '姓名', width: 80, align: 'left', sortable: true },
                { field: 'UserCode', title: '登录账号', width: 150, align: 'left', sortable: true },
                {
                    field: 'UserType', title: '用户类型', width: 80, align: 'center', sortable: true, formatter: function (value, row, index) {
                        //用户类型：0=未定义 1=管理员 3=普通用户 5=微信用户 7=ERP用户 9=OA用户  99=其他
                        var h = '未定义';
                        if (value == 1) { h = '管理员'; } else if (value == 3) { h = '普通用户'; }
                        else if (value == 5) { h = '微信用户'; } else if (value == 7) { h = 'ERP用户'; }
                        else if (value == 9) { h = 'OA用户'; } else if (value == 99) { h = '其他'; }
                        return h;
                    }
                },
                //{ field: 'Sort', title: '排序', width: 50, align: 'center', sortable: true },
                { field: 'Enabled', title: '启用', width: 50, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'Remark', title: '备注', width: 100, align: 'left', sortable: true },
                //{
                //    field: 'OP', title: '操作', width: 90, formatter: function (value, row, index) {
                //        var title = '角色[' + row.RoleName + ']的用户';
                //        var html = '<a href="javascript:void(0);" title=' + title + ' onclick="km.roleuser_datalist.reload(\'' + row.RoleCode + '\');"><span class="icon icon-hamburg-config">&nbsp;&nbsp;</span>角色用户</a>';
                //        return html;
                //    }
                //}
            ]], toolbar: '#toolbar2',
            onSelect: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
                km.usergrid.selectedIndex = index;
                km.usergrid.selectedRow = row;
            },
            onClickRow: function (index, row) {
                var i = index;
            },
            onLoadSuccess: function (data) {
                km.usergrid.pageData = data.rows;
                //km.setRoleUsers();
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
    search_user: function () {
        var keyword = $("#search_key").textbox('getValue');
        km.usergrid.reload({ keyword: keyword });
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
            title: '新增角色', iconCls: 'icon-standard-group-add',
            onOpenEx: function (win) {
                win.find('#TPL_Enable').combobox('setValue', 1);
                win.find('#TPL_RoleType').combobox('setValue', 2);
                win.find('#TPL_Sort').numberbox('setValue', 200);
            },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var jsonStr = JSON.stringify(jsonObject);
                if (jsonObject.RoleCode == "" || jsonObject.RoleName == "") {
                    flagValid = false;
                    com.message('e', '角色代码或角色名称不能为空！'); return;
                }
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
        if (sRow.RoleType == 1) { layer.msg('系统角色不能编辑！'); return; }
        km.template.jq_add.dialog_ext({
            title: '编辑角色', iconCls: 'icon-standard-group-edit',
            onOpenEx: function (win) {
                win.find('#formadd').form('load', sRow);
                win.find('#TPL_RoleCode').textbox().textbox('readonly', true);
            },
            onClickButton: function (win) { //保存操作
                var flagValid = true;
                var jsonObject = win.find("#formadd").serializeJson();
                var jsonStr = JSON.stringify(jsonObject);
                if (jsonObject.RoleCode == "" || jsonObject.RoleName == "") {
                    flagValid = false;
                    com.message('e', '角色代码或角色名称不能为空！'); return;
                }
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
        if (sRow.RoleType == 1) { layer.msg('系统角色不能删除！'); return; }

        var jsonParam = JSON.stringify(sRow);
        com.message('c', ' <b style="color:red">确定要删除角色【' + sRow.RoleName + '】吗？ </b>', function (b) {
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
    do_rolerights: function () {
        var sRow = km.maingrid.getSelectedRow();
        if (sRow == null) { layer.msg('请选择一条记录！'); return; }
        //alert('角色[' + sRow.RoleName + '(' + sRow.RoleCode + ')]权限');
        parent.wrapper.addTab('角色[' + sRow.RoleName + '(' + sRow.RoleCode + ')]权限', '', '/Sys/Base_Role/RoleRights?rolecode=' + sRow.RoleCode, 'icon-standard-key');
    }
};


/*根据角色代码获取角色用户*/
km.getRoleUsers = function (rolecode) {
    km.maingrid.selectRecord(rolecode);
    if (km.maingrid.selectedRow == null) { layer.msg('请选中行记录'); return; }
    if (km.maingrid.selectedRow.Enabled == 0) { layer.msg('当前角色未启用，请先启用！'); return; }

    $('#btn_add_roleuser').linkbutton('enable');
    $('#btn_remove_roleuser').linkbutton('enable');
    var east_panel = $(".easyui-layout").layout("panel", "east");
    east_panel.panel('setTitle', '角色【【' + km.maingrid.selectedRow.RoleName + '】】的用户');

    km.roleusergrid.jq.datagrid('reload', { _t: com.settings.timestamp(), rolecode: rolecode });
    //加载角色的时候再初始化待选择的用户
    if (km.settings.isInitUserGrid == false) {
        km.usergrid.init($("#usergrid"));
        km.settings.isInitUserGrid == true;
    }
}



/*根据角色代码获取角色用户*/
//km.getRoleUsers = function (rolecode) {
//    km.usergrid.uncheckAll();
//    km.settings.currentRoleUsers = null;
//    com.ajax({
//        url: km.model.urls["getroleuser"] + com.settings.ajax_timestamp(), type: 'GET', showLoading: false, data: { rolecode: rolecode }, async: true, success: function (result) {
//            if (result || result.length > 0) {
//                km.settings.currentRoleUsers = result;
//                km.usergrid.reload();
//            } else {
//                com.message('w', '获取角色用户数据失败！');
//            }
//        }
//    });// end ajax  
//}
//km.setRoleUsers = function (rolecode, checkedRows) {
//    var jsonParam = JSON.stringify({ rolecode: rolecode, crows: checkedRows });
//    com.ajax({
//        url: km.model.urls["addroleuser"] + com.settings.ajax_timestamp(), data: jsonParam, async: true, success: function (result) {
//            if (result.s) {
//                com.message('s', '用户添加成功！');
//                win.dialog('destroy');
//                km.roleusergrid.reload();
//            } else {
//                com.message('w', '用户添加失败！');
//            }
//        }
//    });// end ajax  
//}

/*弃用
km.setRoleUsers = function () {
    var list1 = km.settings.currentRoleUsers;
    var list2 = km.usergrid.pageData;
    if (list1 != null) {
        for (var i = 0; i < list1.length; i++) {
            for (var j = 0; j < list2.length; j++) {
                if (list1[i].UserId == list2[j].UserId) {
                    km.usergrid.checkRow(j);
                }
            }
        }
    }
}
*/
/*弃用roleuser_datalist
km.roleuser_datalist = {
    jq: null,
    init: function () {
        this.jq = $('#roleuser_datalist').datalist({
            queryParams: { _t: km.settings.rand_time(), rolecode: "" }, url: km.model.urls["getroleuser"], loadMsg: '你看，马上就好...',
            checkbox: true, lines: true, fit: true, border: false, selectOnCheck: false, checkOnSelect: false,
            valueField: 'UserId', textField: 'RealName', toolbar: '#toolbar1',
            textFormatter: function (value, row, index) {
                return '[' + row.UserId + ']' + km.gridformat.get_color_html(value, 'blue');
            }
        });
    },
    reload: function (rolecode) { this.jq.datalist('reload', { _t: km.settings.rand_time(), rolecode: rolecode }); },
    add_user: function () {
        $("#usergrid").dialog_ext2({
            title: '测试对话框', width: 600, heigth: 450,
            onOpenEx: function (win) {
                //var target = win.find("#usergrid");
                //km.usergrid.init($(target));
            },
            //onBeforeDestroy: function () { return false },
            onClickButton: function (win) {
                //var jsonObject = win.find("#formadd").serializeJson();
                //var jsonStr = JSON.stringify(jsonObject);
                //var s1 = win.find("#TPL_RoleType").combobox('getValue');
                //var s2 = win.find("#TPL_Sort").numberbox('getValue');
                //com.message('s', s1 + '--' + s2 + '--' + jsonStr);
                //win.dialog('destroy');
            }, onClose: function () {
                com.message('w', '对话框关闭事件');
            }
        });
    },
    remove_user: function () { com.message('w', '待实现'); }
}
*/




















