using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using JTS.Core;
using JTS.Service;
using JTS.Utils;


namespace JTS.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            if (FormsAuth.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return KM();
        }

        public ActionResult KM()
        {
            ViewBag.CnName = "KM企业快速开发框架";
            ViewBag.EnName = "KM EnterpriseRapid Development Framework.";
            CookiesUtil.RemoveCookie("KMAUTH");
            CookiesUtil.RemoveCookie("EasyuiTheme");
            CookiesUtil.RemoveCookie("EasyuiVersion");
            return View("Index");
        }

        public ActionResult JTS()
        {
            ViewBag.CnName = "LincChic店铺服务系统";
            ViewBag.EnName = "LincChic Shop Service System";
            return View("Index");
        }

        // POST: /Login/
        public JsonResult Login(JObject data)
        {
            string UserCode = data.Value<string>("usercode");
            string Password = data.Value<string>("pwd");
            string IP = data.Value<string>("ip");
            string City = data.Value<string>("city");
            var loginResult = Base_UserService.Instance.Login(UserCode, Md5Util.MD5(Password), IP, City);
            if (loginResult.Succeed)
            {
                //登录成功后，查询当前用户数据 
                var user = Base_UserService.Instance.GetEntity(ParamQuery.Instance()
                                .AndWhere("UserCode", UserCode).AndWhere("Password", Md5Util.MD5(Password))
                                .AndWhere("Enabled", 1).AndWhere("IsAudit", 1));

                //调用框架中的登录机制
                var loginer = new BaseLoginer
                {
                    UserId = user.UserId,
                    UserCode = user.UserCode,
                    Password = user.Password,
                    UserName = user.RealName,
                    Data = user,
                    IsAdmin = user.UserType == 1  //根据用户UserType判断。用户类型：0=未定义 1=超级管理员 2=普通用户 3=其他
                };

                //读取配置登录默认失效时长：小时
                var effectiveHours = Convert.ToInt32(60 * ConfigUtil.GetConfigDecimal("LoginEffectiveHours"));
                // 无限光年网络科技
 
                //执行web登录
                FormsAuth.SignIn(loginer.UserId.ToString(), loginer, effectiveHours);
                LogHelper.Write("登录成功！用户：" + loginer.UserName + "，账号：" + UserCode + "，密码：" + Password);
                //设置服务基类中，当前登录用户信息
                //this.CurrentBaseLoginer = loginer;
                //登陆后处理
                //更新用户登陆次数及时间（存储过程登录，数据库已经处理）
                //添加登录日志
                string userinfo = string.Format("用户姓名：{0}，用户编号：{1}，登录账号：{2}，登录密码：{3}",
                    loginer.UserName, loginer.UserId, loginer.UserCode, loginer.Password);
                Base_SysLogService.Instance.AddLoginLog(userinfo, IP, City);
                //更新其它业务
            }
            else
            {
                LogHelper.Write("登录失败！账号：" + UserCode + "，密码：" + Password + "。原因：" + loginResult.ResultMsg);
            }
            return Json(loginResult, JsonRequestBehavior.DenyGet);
        }

        // POST: /Login/LogOff
        public ActionResult LogOff()
        {
            var loginer = FormsAuth.GetBaseLoginerData();
            LogHelper.Write("退出系统！账号：" + loginer.UserCode + "，姓名：" + loginer.UserName);
            string userinfo = string.Format("用户姓名：{0}，用户编号：{1}，登录账号：{2}，登录密码：{3}",
                 loginer.UserName, loginer.UserId, loginer.UserCode, loginer.Password);
            Base_SysLogService.Instance.AddLogoutLog(userinfo, "", "");
            FormsAuth.SignOut(); 
            return Redirect("~/Login");
        }

        // GET: /Login/CheckLogin
        public JsonResult CheckLogin(string _t)
        {
            var result = new { s = FormsAuth.IsAuthenticated };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}