/*
路径：~/Areas/Sys/ViewJS/Base_Button.js
说明：权限按钮页面Base_Button的js文件
*/
//当前页面对象
var km = {};
km.model = null;
km.parent_model = null;

km.init = function () {
    km.init_parent_model();
    //km.template.init();
    km.maintree.init();
    km.setMode('show');
    $('#Select_OP').combobox({
        onSelect: function (record) {
            if (record.id < 3) {
                $('#TPL_OrgType').combobox('setValue', 2);
            } else {
                $('#TPL_OrgType').combobox('setValue', 9);
            }
        }
    });
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
    mode_data_zb: [{ "id": 1, "text": "新建下级分部" }, { "id": 3, "text": "新建下级部门" }],
    mode_data_gs: [{ "id": 1, "text": "新建下级分部" }, { "id": 2, "text": "新建同级分部" }, { "id": 3, "text": "新建下级部门" }],
    mode_data_bm: [{ "id": 3, "text": "新建下级部门" }, { "id": 4, "text": "新建同级部门" }],
    op_mode: ''
};

//格式化数据
km.gridformat = {};

//百度模板引擎使用 详情：http://tangram.baidu.com/BaiduTemplate/
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

km.maintree = {
    jq: null,
    treedata: null,
    selectedNode: null,
    selectedData: null,
    setRowData: function (node) {
        var rowData = {
            OrgCode: node.attributes.org_code,
            ParentOrgCode: node.attributes.org_pcode,
            OrgName: node.text,
            OrgCompanyCode: node.attributes.org_fcode,//部门用
            OrgType: node.attributes.org_type,//1=总部 2=分部 9=部门
            OrgSort: node.attributes.org_sort,
            OrgEnabled: node.attributes.org_enabled,
            OrgRemark: node.attributes.org_remark,
        };
        km.maintree.selectedData = rowData;
    },
    init: function () {
        this.treedata = null;
        this.selectedNode = null;
        this.selectedData = null;
        this.jq = $("#maintree").tree({
            method: 'GET', animate: true,
            url: km.model.urls["listtreedata"],
            formatter: function (node) {
                var c1 = node.attributes.org_enabled == 1 ? 'black' : 'gray';
                var c2 = node.attributes.org_enabled == 1 ? '#133E51' : 'gray';
                var enable_html = node.attributes.org_enabled == 1 ? '' : '<span style="color:red">[禁用]</span>';
                var code_html = '<span style="color:' + c2 + '">[' + node.attributes.org_code + ']</span>';
                var title_html = '';
                if (node.attributes.org_type == 1) { title_html = '总部：' + node.text; }
                else if (node.attributes.org_type == 2) { title_html = '分部：' + node.text; }
                else if (node.attributes.org_type == 9) { title_html = '部门：' + node.text; }
                var h = '<div title="' + title_html + '"  style="color:' + c1 + '">' + node.text + code_html + enable_html + '-[' + node.attributes.org_sort + ']</div>';
                return h;
            },
            onClick: function (node) {// 在用户点击的时候提示
                //alert(JSON.stringify(node)); 
                km.maintree.selectedNode = node;
                km.maintree.setRowData(node);
                km.setMode('show');
            },
            onLoadSuccess: function (node, data) { // $("#div_content").html(JSON.stringify(data));
                km.maintree.treedata = data;
                //km.maintree.jq.tree('collapseAll');
            }
        });//end tree init
    }
};



/*工具栏权限按钮事件*/
km.toolbar = {
    do_refresh: function () { window.location.reload(); },
    do_add: function () {
        if (km.maintree.selectedData == null) { layer.msg('请选择一个组织机构'); return; }
        km.setMode('add');
    },
    do_edit: function () {
        if (km.maintree.selectedData == null) { layer.msg('请选择一个组织机构'); return; }
        km.setMode('edit');
    },
    do_delete: function () {
        if (km.maintree.selectedData == null) { layer.msg('请选择一个组织机构'); return; }
        var jsonParam = JSON.stringify(km.maintree.selectedData);
        com.message('c', ' 确定要删除组织机构【' + km.maintree.selectedData.OrgName + '】吗？', function (b) {
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
        km.setMode('show');
    },
    do_save: function () {
        var flagValid = true;
        var jsonObject = $('#form_content').serializeJson();
        if (km.settings.op_mode == 'add') {
            if (jsonObject.Select_OP == 1 || jsonObject.Select_OP == 3) {//下级分部  下级部门
                jsonObject.ParentOrgCode = km.maintree.selectedData.OrgCode;
            }
            else if (jsonObject.Select_OP == 2 || jsonObject.Select_OP == 4) {//同级分部 同级部门
                jsonObject.ParentOrgCode = km.maintree.selectedData.ParentOrgCode;
            } else {
                layer.msg('未获取到保存模式'); flagValid = false; return;
            }
            if (km.maintree.selectedData.OrgType == 1 || km.maintree.selectedData.OrgType == 2) {
                jsonObject.OrgCompanyCode = km.maintree.selectedData.OrgCode;
                if (jsonObject.Select_OP == 3 || jsonObject.Select_OP == 4) {
                    jsonObject.ParentOrgCode = "0";
                }
            }
            if (km.maintree.selectedData.OrgType == 3 || km.maintree.selectedData.OrgType == 4) {
                jsonObject.OrgCompanyCode = km.maintree.selectedData.OrgCompanyCode;
            }

            if (jsonObject.Select_OP == 1 || jsonObject.Select_OP == 2) {//分部 
                if (jsonObject.OrgCode.length != 3) { layer.msg('新增分部代码必须是3位！'); flagValid = false; return; }
            }
            else if (jsonObject.Select_OP == 3 || jsonObject.Select_OP == 4) {//部门
                if (jsonObject.OrgCode.length != 6) { layer.msg('新建部门代码必须是6位！'); flagValid = false; return; }
            }
        }


        if (jsonObject.OrgCode == "" || jsonObject.OrgName == "") {
            flagValid = false; com.message('e', '代码代码或名称不能为空！'); return;
        }

        //alert(JSON.stringify(jsonObject)); 
        if (flagValid) {
            com.ajax({
                type: 'POST', url: km.model.urls[km.settings.op_mode], data: JSON.stringify(jsonObject), success: function (result) {
                    if (result.s) {
                        com.message('s', result.emsg);
                        if (km.settings.op_mode == 'add') {
                            km.toolbar.do_refresh();
                        }
                        if (km.settings.op_mode == 'edit') {
                            km.maintree.selectedNode.text = jsonObject.OrgName;
                            km.maintree.selectedNode.attributes.org_sort = jsonObject.OrgSort;
                            km.maintree.selectedNode.attributes.org_enabled = jsonObject.OrgEnabled;
                            km.maintree.selectedNode.attributes.org_remark = jsonObject.OrgRemark;
                            km.maintree.setRowData(km.maintree.selectedNode);
                            km.maintree.jq.tree('update', km.maintree.selectedNode);
                            km.setMode('show');
                        }
                    } else {
                        com.message('e', result.emsg);
                    }
                }
            });//end com.ajax
        }
    }
};



//设置右侧控件显示模式  flag：'show'=显示模式 'edit'=编辑模式 'add'=新增模式
km.setMode = function (flag) {
    km.settings.op_mode = flag;
    $('#tr_mode').hide();
    $('#km_toolbar_1').show();
    $('#km_toolbar_2').hide();
    com.mask($('#west_panel'), false);
    if (flag == 'show') {
        if (km.maintree.selectedData == null) {
            $('#form_content').form('clear');
        } else {
            $('#form_content').form('load', km.maintree.selectedData);
            //alert(JSON.stringify(km.maintree.selectedData));
        }
        $('#TPL_OrgCode').textbox('readonly', true);
        $('#TPL_OrgName').textbox('readonly', true);
        $('#TPL_OrgType').combobox('readonly', true);
        $('#TPL_OrgSort').numberbox('readonly', true);
        $('#TPL_OrgEnabled').combobox('readonly', true);
        $('#TPL_OrgRemark').textbox('readonly', true);
    } else if (flag == 'edit') {
        $('#km_toolbar_1').hide();
        $('#km_toolbar_2').show();
        com.mask($('#west_panel'), true);
        $('#TPL_OrgCode').textbox('readonly', true);
        $('#TPL_OrgName').textbox('readonly', false);
        $('#TPL_OrgType').combobox('readonly', true);
        $('#TPL_OrgSort').numberbox('readonly', false);
        $('#TPL_OrgEnabled').combobox('readonly', false);
        $('#TPL_OrgRemark').textbox('readonly', false);
        if (km.maintree.selectedData.OrgType == 1) {
            $('#TPL_OrgSort').numberbox('readonly', true);
            $('#TPL_OrgEnabled').combobox('readonly', true);
        }
    }
    else if (flag == 'add') {
        $('#tr_mode').show();
        $('#km_toolbar_1').hide();
        $('#km_toolbar_2').show();
        com.mask($('#west_panel'), true);
        $('#form_content').form('clear');
        $('#TPL_OrgSort').numberbox('setValue', 100);
        $('#TPL_OrgType').combobox('setValue', 2);
        $('#TPL_OrgEnabled').combobox('setValue', 1);
        if (km.maintree.selectedData.OrgType == 1) {
            $('#Select_OP').combobox('loadData', km.settings.mode_data_zb).combobox('setValue', 1);
        } else if (km.maintree.selectedData.OrgType == 2) {
            $('#Select_OP').combobox('loadData', km.settings.mode_data_gs).combobox('setValue', 1);
        }
        else if (km.maintree.selectedData.OrgType == 9) {
            $('#Select_OP').combobox('loadData', km.settings.mode_data_bm).combobox('setValue', 3);
            $('#TPL_OrgType').combobox('setValue', 9);
        }

        $('#TPL_OrgCode').textbox('readonly', false);
        $('#TPL_OrgName').textbox('readonly', false);
        $('#TPL_OrgType').combobox('readonly', true);
        $('#TPL_OrgSort').numberbox('readonly', false);
        $('#TPL_OrgEnabled').combobox('readonly', false);
        $('#TPL_OrgRemark').textbox('readonly', false);
    }
}



$(km.init);
