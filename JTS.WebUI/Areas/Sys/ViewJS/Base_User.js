/*
路径：~/Areas/Sys/ViewJS/Base_User.js
说明：系统角色页面Base_User的js文件
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
    km.rolegrid.init();
    km.orgcombotree.init();
    km.set_mode('clear');
    $('#btn_save_userrole').linkbutton('disable');


    //$('#btn_add_roleuser').linkbutton('disable');
    //$('#btn_remove_roleuser').linkbutton('disable');
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
    op_mode: '' //标记当前操作模式  'show'  'add'  'edit'
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

/*用户列表datagrid*/
km.maingrid = {
    jq: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#maingrid").datagrid({
            fit: true, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'UserId',
            queryParams: { _t: com.settings.timestamp(), keyword: "" }, url: km.model.urls["pagelist"],
            pagination: true, pageList: [5, 10, 15, 20, 30, 50, 100], pageSize: 15,
            rowStyler: function (index, row) { if (row.Enabled == 0) { return 'color:red;'; } },
            columns: [[
                { field: 'Enabled', title: '启用', width: 50, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
                { field: 'IsAudit', title: '审核', width: 50, align: 'center', sortable: true, formatter: com.html_formatter.yesno },
	            { field: 'UserId', title: '编号', width: 60, align: 'left', sortable: true },
                { field: 'RealName', title: '姓名', width: 80, align: 'left', sortable: true },
                {
                    field: 'UserType', title: '类型', width: 60, align: 'center', sortable: true, formatter: function (value, row, index) {
                        //用户类型：0=未定义 1=管理员 3=普通用户 5=微信用户 7=ERP用户 9=OA用户  99=其他
                        var h = '未定义';
                        if (value == 1) { h = '管理员'; } else if (value == 3) { h = '普通用户'; } else if (value == 5) { h = '微信用户'; }
                        else if (value == 7) { h = 'ERP用户'; } else if (value == 9) { h = 'OA用户'; } else if (value == 99) { h = '其他'; }
                        return h;
                    }
                },
                { field: 'Sort', title: '排序', width: 50, align: 'center', sortable: true },
                { field: 'CompanyName', title: '公司', width: 60, align: 'center', sortable: true },
                { field: 'DepartmentName', title: '部门', width: 60, align: 'center', sortable: true }
            ]], toolbar: '#toolbar1',
            onSelect: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
            },
            onClickRow: function (index, row) {
                km.maingrid.selectedIndex = index;
                km.maingrid.selectedRow = row;
                km.rolegrid.setUserRoles(row);
                if (km.maingrid.selectedRow)
                    km.set_mode('show');
            },
            onLoadSuccess: function (data) {
                if (km.maingrid.selectedIndex > 0) {
                    km.maingrid.selectRow(km.maingrid.selectedIndex);
                }
            }
        });//end grid init
    },
    search_data: function () {
        var keyword = $('#keyword').val();
        this.reload({ keyword: keyword });
        this.unselectAll();
        km.set_mode('clear');
    },
    reload: function (param) {
        var defaults = { _t: com.settings.timestamp() };
        if (param) {
            defaults = $.extend(defaults, param);
        }
        this.jq.datagrid('reload', defaults);
    },
    selectRow: function (index) { this.jq.datagrid('selectRow', index);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*选中一条记录，根据idValue值*/ },
    unselectAll: function () {
        /*取消选择所有当前页中所有的行*/
        this.jq.datagrid('unselectAll');
        km.maingrid.selectedIndex = -1;
        km.maingrid.selectedRow = null;
    },
    getSelectedRow: function () { return this.jq.datagrid('getSelected'); /*获取当前选中的行*/ }
};

/*角色datagrid*/
km.rolegrid = {
    jq: null,
    pageData: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#rolegrid").datagrid({
            fit: true, striped: false, border: false, singleSelect: false, rownumbers: true, remoteSort: false, cache: false, method: 'get', idField: 'RoleCode',
            selectOnCheck: true, checkOnSelect: true,
            queryParams: { _t: com.settings.timestamp() }, url: km.model.urls["listroledata"],
            columns: [[
                { field: 'ck', title: '', align: 'center', checkbox: true, width: 50, sortable: true },
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
                { field: 'Remark', title: '备注', width: 100, align: 'left', sortable: true }
            ]], toolbar: '#toolbar2',
            onSelect: function (index, row) {
                //选中行事件，将当前选中行的的索引和行数据存储起来
                km.rolegrid.selectedIndex = index;
                km.rolegrid.selectedRow = row;
            },
            onClickRow: function (index, row) {
                var i = index;
            },
            onLoadSuccess: function (data) {
                km.rolegrid.pageData = data.rows;
                km.rolegrid.uncheckAll();
            }
        });//end grid init
        return this.jq;
    },
    reload: function () { this.jq.datagrid('reload', { _t: com.settings.timestamp() }); },
    selectRow: function (index) { this.jq.datagrid('selectRow', index);/*选中一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.datagrid('selectRecord', idValue);/*选中一条记录，根据idValue值*/ },
    checkRow: function (index) { this.jq.datagrid('checkRow', index);/*勾选一行，行索引从0开始*/ },
    uncheckAll: function () { this.jq.datagrid('uncheckAll'); },
    unselectAll: function () { this.jq.datagrid('unselectAll'); },
    getSelections: function () { return this.jq.datagrid('getSelections'); /*返回所有被选中的行，当没有记录被选中的时候将返回一个空数组*/ },
    setUserRoles: function (row) {
        km.rolegrid.unselectAll();
        $("#btn_save_userrole").linkbutton({ text: '保存用户[' + row.RealName + ']角色' });
        $('#btn_save_userrole').linkbutton('enable');
        var u = km.model.urls["getlist_userrole"] + com.settings.ajax_timestamp() + '&UserId=' + row.UserId;
        com.ajax({
            type: 'GET', url: u, data: {}, showLoading: false, success: function (result) {
                if (result) {
                    for (var i = 0; i < result.length; i++) {
                        km.rolegrid.selectRecord(result[i].RoleCode);
                    }
                }
            }
        });
    },
    saveUserRoles: function () {
        var rows = km.rolegrid.getSelections();
        if (rows.length == 0) { layer.msg('没有要保存的数据！'); return; } 
        var jsonParam = JSON.stringify({ userid: km.maingrid.selectedRow.UserId, crows: rows });
        com.message('c', '确定要保存用户【' + km.maingrid.selectedRow.RealName + '(' + km.maingrid.selectedRow.UserId + ')】的角色吗？', function (ok) {
            if (ok) {
                com.ajax({
                    type: 'POST', url: km.model.urls["save_userroles"], data: jsonParam, success: function (result) {
                        if (result.s) {
                            com.message('s', '保存成功！'); 
                        } else {
                            com.message('e', result.emsg);
                        }
                    }
                });// end ajax  
            }
        }); // end message  
    }
};

km.orgcombotree = {
    jq: null,
    treedata: null,
    selectedNode: null,
    init: function () {
        this.treedata = null;
        this.selectedNode = null;
        this.jq = $("#TPL_DepartmentCode").combotree({
            method: 'GET', animate: true, url: km.model.urls["listorgdata"], queryParams: { _t: com.settings.timestamp() },
            formatter: function (node) {
                var c1 = node.attributes.org_enabled == 1 ? 'black' : 'gray';
                var c2 = node.attributes.org_enabled == 1 ? '#133E51' : 'gray';
                var enable_html = node.attributes.org_enabled == 1 ? '' : '<span style="color:red">[禁用]</span>';
                var code_html = '<span style="color:' + c2 + '">[' + node.attributes.org_code + ']</span>';
                var title_html = '';
                if (node.attributes.org_type == 1) { title_html = '总部：' + node.text; }
                else if (node.attributes.org_type == 2) { title_html = '分部：' + node.text; }
                else if (node.attributes.org_type == 9) { title_html = '部门：' + node.text; }
                var h = '<div title="' + title_html + '"  style="color:' + c1 + '">' + node.text + code_html + enable_html + '</div>';
                return h;
            },
            onClick: function (node) {
                km.orgcombotree.selectedNode = node;
            },
            onLoadSuccess: function (node, data) {
                km.orgcombotree.treedata = data;
            }
        });//end tree init
    }
};
/*工具栏权限按钮事件*/
km.toolbar = {
    do_browse: function () { },
    do_refresh: function () {
        km.maingrid.reload(); //window.location.reload();
    },
    do_add: function () {
        km.set_mode('add');
    },
    do_edit: function () {
        if (km.maingrid.selectedRow == null) { layer.msg('请选择一条记录！'); return; }
        km.set_mode('edit');
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
    do_audit: function () {
        if (km.maingrid.selectedRow == null) { layer.msg('请选择一条记录！'); return; }
        com.message('c', '确定要审核[用户：' + km.maingrid.selectedRow.RealName + ']吗？', function (r) {
            if (r) {
                com.ajax({
                    type: 'POST', url: km.model.urls["audituser"], data: JSON.stringify(km.maingrid.selectedRow), success: function (result) {
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
    do_resetpwd: function () {
        if (km.maingrid.selectedRow == null) { layer.msg('请选择一条记录！'); return; }
        com.message('c', '确定要将[用户：' + km.maingrid.selectedRow.RealName + '，账号：' + km.maingrid.selectedRow.UserCode + ']的密码重置成123456吗？', function (r) {
            if (r) {
                com.ajax({
                    type: 'POST', url: km.model.urls["resetpwd"], data: JSON.stringify(km.maingrid.selectedRow), success: function (result) {
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
    do_save: function () {
        var flagValid = true;
        var jsonObject = $("#form_content").serializeJson();
        if (jsonObject.Enabled == "") { flagValid = false; com.message('e', '请选择启用状态！'); return; }
        if (jsonObject.IsSingleLogin == "") { flagValid = false; com.message('e', '请选择是否单点登录！'); return; }
        if (jsonObject.Sex == "") { flagValid = false; com.message('e', '请选择性别！'); return; }
        if (jsonObject.UserType == "") { flagValid = false; com.message('e', '请选择用户类型！'); return; }
        if (jsonObject.DepartmentCode == "") { flagValid = false; com.message('e', '请选择部门！'); return; }
        if (jsonObject.RealName == "") { flagValid = false; com.message('e', '请输入姓名！'); return; }
        if (jsonObject.UserCode == "") { flagValid = false; com.message('e', '请输入登录账号！'); return; }
        var tree_obj = km.orgcombotree.jq.combotree('tree');	// 获取树对象
        var node = tree_obj.tree('getSelected');		// 获取选择的节点
        //alert(JSON.stringify(node));
        //判断当前选中的组织机构是否是部门，不然提示阻止
        if (node.attributes.org_type != 9) { flagValid = false; com.message('e', '所属部门只能选择部门，请不要选择分部！'); return; }
        jsonObject.CompanyCode = node.attributes.org_fcode;//获取当前部门对应的公司代码
        jsonObject.IsSuperAdmin = 0;//新增和编辑默认都是0 非超级管理员 
        var jsonStr = JSON.stringify(jsonObject); //alert(jsonStr);

        if (flagValid) {
            com.ajax({
                type: 'POST', url: km.model.urls[km.settings.op_mode], data: jsonStr, success: function (result) {
                    if (result.s) {
                        com.message('s', result.emsg);
                        km.maingrid.reload();
                        if (km.settings.op_mode == 'add') {
                            km.maingrid.unselectAll();
                            km.set_mode('clear');
                        }
                        if (km.settings.op_mode == 'edit') {
                            km.maingrid.selectRow(km.maingrid.selectedIndex);
                            km.maingrid.selectedRow = $.extend(km.maingrid.selectedRow, jsonObject);
                            km.set_mode('show');
                        }
                    } else {
                        com.message('e', result.emsg);
                    }
                }
            });
        }
    },
    do_undo: function () {
        var op_mode = km.maingrid.selectedRow == null ? 'clear' : 'show';
        km.set_mode(op_mode);
    }
};

/*显示：'show'  新增：'add'  编辑 'edit'  清空 'clear'*/
km.set_mode = function (op_mode) {
    km.settings.op_mode = op_mode;
    $('#km_toolbar').show();
    $('#km_toolbar_2').hide();
    com.mask($('#west_panel'), false);
    $('#user_tabs').tabs('enableTab', 1);
    $('#user_tabs').tabs('enableTab', 2);
    $('.form_content .easyui-combobox').combobox('readonly', true);
    $('.form_content .easyui-combotree').combotree('readonly', true);
    $('.form_content .easyui-textbox').textbox('readonly', true);
    $('.form_content .easyui-numberbox').numberbox('readonly', true);

    if (op_mode == 'show') {
        //console.info(JSON.stringify(km.maingrid.selectedRow));
        $('#form_content').form('load', km.maingrid.selectedRow);
        $('#form_content_other').form('load', km.maingrid.selectedRow);
        km.orgcombotree.jq.combotree('setValue', km.maingrid.selectedRow.DepartmentCode);
    } else if (op_mode == 'add') {
        $('#km_toolbar').hide();
        $('#km_toolbar_2').show();
        com.mask($('#west_panel'), true);
        $('#user_tabs').tabs('disableTab', 1);
        $('#user_tabs').tabs('disableTab', 2);
        $('#user_tabs').tabs('select', 0);
        $('.form_content .easyui-combobox').combobox('readonly', false);
        $('.form_content .easyui-combotree').combotree('readonly', false);
        $('.form_content .easyui-textbox').textbox('readonly', false);
        $('.form_content .easyui-numberbox').numberbox('readonly', false);
        $('#form_content').form('clear');
        $('#form_content_other').form('clear');

        $('#TPL_UserId').val(0);
        $('#TPL_Enabled').combobox('setValue', 1);
        $('#TPL_UserType').combobox('setValue', 3);
        $('#TPL_IsSingleLogin').combobox('setValue', 1);
        $('#TPL_Sex').combobox('setValue', '男');
        $('#TPL_Sort').numberbox('setValue', 888);

    } else if (op_mode == 'edit') {
        $('#km_toolbar').hide();
        $('#km_toolbar_2').show();
        com.mask($('#west_panel'), true);
        $('#user_tabs').tabs('disableTab', 1);
        $('#user_tabs').tabs('disableTab', 2);
        $('#user_tabs').tabs('select', 0);
        $('.form_content .easyui-combobox').combobox('readonly', false);
        $('.form_content .easyui-combotree').combotree('readonly', false);
        $('.form_content .easyui-textbox').textbox('readonly', false);
        $('.form_content .easyui-numberbox').numberbox('readonly', false);
    } else if (op_mode == 'clear') {
        $('#form_content').form('clear');
        $('#form_content_other').form('clear');
    }
}





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






