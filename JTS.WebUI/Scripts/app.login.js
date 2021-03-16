
var obj = {
    ip: '0.0.0.0',
    city: '未获取到城市',
    usercode: '',
    pwd: ''
}
$(function () {
    //var returnCitySN = {"cip": "182.50.119.140", "cid": "110000", "cname": "北京市"};
    if (returnCitySN) {
        obj.ip = returnCitySN["cip"];
        obj.city = returnCitySN["cname"] + '[' + returnCitySN["cid"] + ']';
    }
    //var remote_ip_info = {"ret":1,"start":-1,"end":-1,"country":"\u4e2d\u56fd","province":"\u5317\u4eac","city":"\u5317\u4eac","district":"","isp":"","type":"","desc":""};
    //if (remote_ip_info) { var json = JSON.stringify(remote_ip_info); alert(json);} 
    init();
})

var init = function () {
    //开发阶段账号密码就默认好了
    $("#UserCode").val('sysadmin'); $("#Password").val(123);

    $("#span_ip").text(obj.city + '[' + obj.ip + ']');
    if (top != window) top.window.location = window.location;
    $("#UserCode").bind('keydown', function (e) {
        if (e.keyCode == 13) {	// when press ENTER key, accept the inputed value.
            e.preventDefault();
            $("#Password").focus();
        }
    });
    $("#Password").bind('keydown', function (e) {
        if (e.keyCode == 13) {	// when press ENTER key, accept the inputed value.
            e.preventDefault();
            doLogin();
        }
    });

    $("#UserCode").focus(); $("#UserCode").select();
    //com.message('s', obj.city);
};
var doLogin = function () {
    obj.usercode = $("#UserCode").val();
    obj.pwd = $("#Password").val();
    if (obj.usercode == "") { layer.msg('请输入账号'); return; }
    if (obj.pwd == "") { layer.msg('请输入密码'); return; }
    $(".login_msg").html('<span style="color:red; font-weight:bold"><img src="/Content/images/ajax-loader.gif" />正在进行登录验证...</span>');
    com.ajax({
        url: '/Login/Login', data: JSON.stringify(obj), showLoading: false, success: function (result) {
            if (result.s) {
                //com.message('s', result.emsg);
                $(".login_msg").html('<span style="color:green; font-weight:bold"><img src="/Content/images/ajax-loader.gif" />登录成功，正在跳转...</span>');
                window.location.href = '/' + com.settings.ajax_timestamp(); 
            } else {
                layer.msg(result.emsg);
            }
        }
    })
}
var initlocation = function () {
    var stateObject = {};
    var title = "";
    var newUrl = "/login.do";// "/login/hello-来自" + obj.city + "的朋友!";
    if (window.history.pushState) {
        window.history.pushState(stateObject, title, newUrl);
    }
}
 