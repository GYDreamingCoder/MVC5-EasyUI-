/*
路径：~/Areas/Sys/ViewJS/Base_RoleMenuButton.js
说明：系统角色权限设置页面Base_RoleMenuButton的js文件
*/
//当前页面对象
var km = {};
km.model = null;//当前model对象
km.parent_model = null;//存储父页面的model对象 parent.wrapper.model

/*页面初始方法*/
km.init = function () {
    km.init_parent_model();
    //km.template.init();
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
            layer.msg("存在父页面，但未能获取到parent.wrapper对象");
        }
    } else {
        layer.msg("当前页面已经脱离iframe，无法获得parent.wrapper对象！");
    }
}

/*执行页面初始化*/
$(km.init);

//页面对象参数设置
km.settings = {
    body_width: $('body').width() - 10,//body宽度
    rights_width: $('body').width() - 300
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

/*菜单treegrid*/
km.maingrid = {
    jq: null,
    selectedIndex: 0,
    selectedRow: null,
    init: function () {
        this.jq = $("#maingrid").treegrid({
            fit: true, border: false, singleSelect: true, rownumbers: false, nowrap: false, remoteSort: false, cache: false, striped: false, showHeader: false, scrollbarSize: 5,
            method: 'get', idField: 'MenuCode', treeField: 'MenuName',
            queryParams: { _t: com.settings.timestamp() }, url: km.model.urls["menulist"], selectOnCheck: false, checkOnSelect: false, animate: true, checkbox: true,
            columns: [[
                {
                    field: 'MenuName', title: '菜单名称', width: 180, align: 'left', formatter: function (value, row, index) {
                        var h = value;
                        if (row.MenuType == 1) {
                            h = com.html_formatter.get_color_html(value, 'blue');
                        }
                        var h_input_checkbox = '<input type="checkbox" id="ckmenu-' + row.MenuCode + '" name="ckmenu" onclick="km.maingrid.menuCheckboxClick(this,\'' + row.MenuCode + '\')"/>  ';
                        return h_input_checkbox + h;
                    }
                },
                {
                    field: 'MenuType', title: '菜单说明', width: 100, align: 'left', formatter: function (value, row, index) {
                        //菜单类型：0=未定义 1=目录 2=页面 7=备用1 8=备用2 9=备用3   
                        var h = '未定义';
                        if (value == 1) { h = com.html_formatter.get_color_html('目录', '#514B51'); } else if (value == 2) { h = com.html_formatter.get_color_html('页面', 'green'); }
                        else if (value == 7) { h = '备用1'; } else if (value == 8) { h = '备用2'; } else if (value == 9) { h = '备用3'; }

                        //按钮模式：0=未定义 1=动态按钮 2=静态按钮 3=无按钮
                        var h2 = '未定义';
                        if (row.ButtonMode == 1) { h2 = '动态按钮'; } else if (row.ButtonMode == 2) { h2 = '静态按钮'; } else if (row.ButtonMode == 3) { h2 = '无按钮'; }
                        var html = h + ' | ' + h2;
                        return html;
                    }
                },
                {
                    field: 'Rights', title: '按钮权限', width: km.settings.rights_width, align: 'left', formatter: function (value, row, index) {
                        if (row.MenuType == 2 && row.ButtonMode == 1) {
                            return km.maingrid.initMenuButtons(row);
                        } else {
                            return '';
                        }
                    }
                }
            ]],
            loadFilter: function (data) {
                var d = utils.copyProperty(data.rows || data, ["MenuCode", "IconClass"], ["_id", "iconCls"], false);
                var resultData = utils.toTreeData(d, '_id', 'ParentCode', "children");
                return resultData;
            },
            onClickRow: function (row) {
                km.maingrid.selectedRow = row;
                //com.message('s', JSON.stringify(km.maingrid.selectedRow));
            },
            onLoadSuccess: function (data) {
                //alert('load data successfully!');
                //加载完成后，设置当前角色的菜单权限，是否勾选
                //var checkboxs_menus = $("input[name=ckmenu]");
                //console.info('km.model.list_role_menus：' + JSON.stringify(km.model.list_role_menus));
                if (km.model.list_role_menus.length > 0) {
                    for (var i = 0; i < km.model.list_role_menus.length; i++) {
                        var cid = '#ckmenu-' + km.model.list_role_menus[i].MenuCode;
                        //console.info('length：' + km.model.list_role_menus.length + '，序号：' + i + '，cid：' + cid);
                        if ($(cid)[0]) { $(cid)[0].checked = true; }
                    }
                }
                //var checkboxs_actions = $("input[name=ckaction]");
                //console.info('km.model.list_role_menu_buttons：' + km.model.list_role_menu_buttons);
                if (km.model.list_role_menu_buttons.length > 0) {
                    for (var i = 0; i < km.model.list_role_menu_buttons.length; i++) {
                        var aid = '#ck-' + km.model.list_role_menu_buttons[i].MenuCode + '-' + km.model.list_role_menu_buttons[i].ButtonCode;
                        //console.info('length：' + km.model.list_role_menu_buttons.length + '，序号：xx' + i + 'aid：' + aid);
                        if ($(aid)[0]) { $(aid)[0].checked = true; }
                    }
                }
            }
        });//end grid init
    },
    reload: function () { this.jq.treegrid('reload', { _t: com.settings.timestamp() }); },
    selectRow: function (index) { this.jq.treegrid('selectRow', index);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.treegrid('selectRecord', idValue);/*选中当前选中的行*/ },
    deleteRow: function () { this.jq.treegrid('deleteRow', this.selectedIndex);/*删除当前选中的行*/ },
    getSelectedRow: function () { return this.jq.treegrid('getSelected'); /*获取当前选中的行*/ },
    initMenuButtons: function (row) {
        /*
         list_menu_buttons=listMenuButtons,
         list_role_menus = listRoleMenus,
         list_role_menu_buttons = listRoleMenuButtons 
        */
        //alert($.array.isArray(km.model.list_menu_buttons));
        var h = com.html_formatter.get_color_html('此菜单未配置权限按钮', 'red');
        var currentMenuButtons = $.array.filter(km.model.list_menu_buttons, function callbackfn(value, index, array) {
            return value.MenuCode == row.MenuCode;
        });
        // alert(JSON.stringify(currentMenuButtons));
        //layer.msg(km.model.list_menu_buttons.length);

        var h_input_checkbox = '';
        if (currentMenuButtons.length > 0) {
            //layer.msg(currentMenuButtons.length);
            for (var i = 0; i < currentMenuButtons.length; i++) {
                //var btnname = currentMenuButtons[i].ButtonText == '' ? currentMenuButtons[i].ButtonName : currentMenuButtons[i].ButtonText;
                var checkbox_id = 'ck-' + currentMenuButtons[i].MenuCode + '-' + currentMenuButtons[i].ButtonCode;
                var span_icon = '<span class="icon ' + currentMenuButtons[i].IconClass + '" style=" position:relative;top:5px"></span>' + currentMenuButtons[i].ShowButtonName;
                var h_input = '<input type="checkbox" id="' + checkbox_id + '" name="ck-' + currentMenuButtons[i].MenuCode + '"  menucode="' + currentMenuButtons[i].MenuCode + '"  buttoncode="' + currentMenuButtons[i].ButtonCode + '"  onclick="km.maingrid.actionCheckboxClick(this)"/>' + span_icon + ' ';
                h_input_checkbox += '&nbsp;&nbsp;' + h_input;
            }
            var h_input_checkbox_all = '<input type="checkbox" id="allck-' + row.MenuCode + '"  menucode="' + row.MenuCode + '" onclick="km.maingrid.allActionCheckboxClick(this,\'' + row.MenuCode + '\')"/>' + com.html_formatter.get_color_html('全选', '#6C1899') + '&nbsp;&nbsp;|';
            h = h_input_checkbox_all + h_input_checkbox;
        }

        return h;
    },
    menuCheckboxClick: function (target, menucode) {
        var menuStrs = menucode;//当前勾选的菜单代码字符串，使用逗号拼接起来
        var target_input_id = '#ckmenu_' + menucode;
        //console.info($(target)[0].checked);
        var childNodes = km.maingrid.jq.treegrid('getChildren', menucode);
        //com.message('s', JSON.stringify(childNodes));

        //点击节点处理子节点的勾选与反选
        if (childNodes.length > 0) {//存在子节点
            for (var i = 0; i < childNodes.length; i++) {
                var child_item = '#ckmenu-' + childNodes[i].MenuCode;//console.info(taget_item); 
                $(child_item)[0].checked = $(target)[0].checked;
                menuStrs += ',' + childNodes[i].MenuCode;
            }
        }

        //点击节点处理上级节点的勾选与反选
        if ($(target)[0].checked == true) {
            var parent = km.maingrid.jq.treegrid('getParent', menucode);
            if (parent != null) {
                $('#ckmenu-' + parent.MenuCode)[0].checked = true;
                menuStrs += ',' + parent.MenuCode;
            }
            while (parent) {
                parent = km.maingrid.jq.treegrid('getParent', parent.MenuCode);
                if (parent != null) {
                    $('#ckmenu-' + parent.MenuCode)[0].checked = true;
                    menuStrs += ',' + parent.MenuCode;
                }
            }
        }
        var mymsg = $(target)[0].checked == true ? '菜单权限【设置】成功' : '菜单权限【取消】成功';
        //alert('执行操作：' + $(target)[0].checked + '    关联菜单代码：' + menuStrs);
        var jsonParam = { flagadd: $(target)[0].checked, rolecode: km.model.data.rolecode, menucode_str: menuStrs };
        com.ajax({
            type: 'POST', url: km.model.urls["saverolemenus"], data: JSON.stringify(jsonParam), showLoading: false, success: function (result) {
                if (result.s) {
                    layer.msg(mymsg);
                } else {
                    layer.msg(result.emsg);
                }
            }
        });// end ajax  
    },
    actionCheckboxClick: function (target) {
        //alert($(target).attr("menucode"));
        var menu_input_name = '#ckmenu-' + $(target).attr("menucode");
        if ($(menu_input_name)[0].checked == false) {
            $(target)[0].checked = false;
            layer.msg('请先设置菜单权限');
            return;
        }
        var mymsg = $(target)[0].checked == true ? '按钮权限【设置】成功' : '按钮权限【取消】成功';
        var rolecode = km.model.data.rolecode;
        var menucode = $(target).attr("menucode");
        var buttoncode = $(target).attr("buttoncode");
        var jsonParam = { flagadd: $(target)[0].checked, rolecode: rolecode, menucode: menucode, buttoncode: buttoncode };
        com.ajax({
            type: 'POST', url: km.model.urls["saverolemenubutton"], data: JSON.stringify(jsonParam), showLoading: false, success: function (result) {
                if (result.s) {
                    layer.msg(mymsg);
                } else {
                    layer.msg(result.emsg);
                }
            }
        });// end ajax  
    },
    allActionCheckboxClick: function (target, menucode) {
        var action_input_name = 'ck-' + menucode;
        var checkboxs = $("input[name=" + action_input_name + "]");
        var menu_input_name = '#ckmenu-' + menucode;
        if ($(menu_input_name)[0].checked == false) {
            $(target)[0].checked = false;
            layer.msg('请先设置菜单权限');
            return;
        }
        for (var i = 0; i < checkboxs.length; i++) {
            checkboxs[i].checked = target.checked;
        }
        //alert(all_checkbox_id); 
        var mymsg = $(target)[0].checked == true ? '按钮权限批量【设置】成功' : '按钮权限批量【取消】成功';
        var rolecode = km.model.data.rolecode;
        var jsonParam = { flagadd: $(target)[0].checked, rolecode: rolecode, menucode: menucode };
        com.ajax({
            type: 'POST', url: km.model.urls["saverolemenubutton_all"], data: JSON.stringify(jsonParam), showLoading: false, success: function (result) {
                if (result.s) {
                    layer.msg(mymsg);
                } else {
                    layer.msg(result.emsg);
                }
            }
        });// end ajax  

    }
};


/*工具栏权限按钮事件*/
km.toolbar = {
    do_refresh: function () {
        window.location.reload();
    }
};
