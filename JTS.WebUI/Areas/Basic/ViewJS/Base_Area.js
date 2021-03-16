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
    km.init_parent_model();
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

/*执行页面初始化*/
$(km.init);

//页面对象参数设置
km.settings = {};

//格式化数据
km.gridformat = {};

/*菜单datagrid*/
km.maingrid = {
    jq: null,
    init: function () {
        this.jq = $("#maingrid").treegrid({
            fit: true, border: false, singleSelect: true, rownumbers: true, remoteSort: false, cache: false, animate: true, method: 'get', idField: 'Id', treeField: 'FullName',
            queryParams: { _t: com.settings.timestamp() }, url: km.model.urls["list"],
            columns: [[
	            { field: 'Id', title: '地区编号', width: 60, align: 'left' },
                { field: 'FullName', title: '地区名称', width: 200, align: 'left' },
                { field: 'ShortName', title: '简称', width: 80, align: 'left' },
                { field: 'Pinyin', title: '拼音', width: 80, align: 'left' },
                {
                    field: 'LevelType', title: '级别', width: 80, align: 'left', formatter: function (value, row, index) {
                        //级别 0=中国 1=省份(直辖) 2=城市 3=区县
                        var h = '未定义';
                        if (value == 0) { h = '中国'; } else if (value == 1) { h = '省份'; } else if (value == 2) { h = '城市'; } else if (value == 3) { h = '区县'; }
                        return h;
                    }
                },
                //{ field: 'CityCode', title: '城市代码', width: 60, align: 'center' },
                //{ field: 'ZipCode', title: '邮编', width: 60, align: 'center' },
                { field: 'MergerName', title: '地区全称', width: 300, align: 'left' },
                { field: 'Lng', title: '经度', width: 100, align: 'left' },
                { field: 'Lat', title: '纬度', width: 100, align: 'left' }
            ]],
            onBeforeLoad: function (row, param) {
                if (!row) { param.id = 0; }
            },
            onClickRow: function (row) { },
            onLoadSuccess: function (row, data) { }
        });//end grid init
    },
    reload: function () { this.jq.treegrid('reload', { _t: com.settings.timestamp() }); },
    selectRow: function () { this.jq.treegrid('selectRow', this.selectedIndex);/*选择一行，行索引从0开始*/ },
    selectRecord: function (idValue) { this.jq.treegrid('selectRecord', idValue);/*选中当前选中的行*/ },
    getSelectedRow: function () { return this.jq.treegrid('getSelected'); /*获取当前选中的行*/ }
};
