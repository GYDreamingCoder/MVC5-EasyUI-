using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTS.Core;
using JTS.Service;
using JTS.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JTS.Controllers
{
    //[AllowAnonymous]
    public class HomeController : BaseController
    {
        //首页
        public ActionResult Index(string timestamp)
        {
            if (!FormsAuth.IsAuthenticated)
                return RedirectToAction("Index", "Login");
            // 无限光年网络科技
            var loginer = FormsAuth.GetBaseLoginerData();
            ViewBag.Title = "KM企业快速开发框架";
            ViewBag.UserId = loginer.UserId;
            ViewBag.UserCode = loginer.UserCode;
            ViewBag.UserName = loginer.UserName;
            ViewBag.LoginUser = "[" + loginer.UserCode + "]" + loginer.UserName;
            var userSettings = Base_UserService.Instance.GetCurrentUserSettings();
            ViewBag.Settings = userSettings;
            ViewBag.EasyuiTheme = userSettings["theme"].ToString();
            ViewBag.EasyuiVersion = JTS.Utils.ConfigUtil.GetConfigString("EasyuiVersion");
            ViewBag.SystemVersion = JTS.Utils.ConfigUtil.GetConfigString("SystemVersion");
            CookiesUtil.WriteCookies("EasyuiTheme", 0, userSettings["theme"].ToString());
            CookiesUtil.WriteCookies("EasyuiVersion", 0, JTS.Utils.ConfigUtil.GetConfigString("EasyuiVersion"));
            var list = Base_MenuService.Instance.GetUserMenus(this.CurrentUser.UserId, this.CurrentUser.IsSuperAdmin == 1);
            //测试发送一条验证短信
            //Random rnd = new Random();
            //int num = rnd.Next(304001, 504001);
            //var result = KM.TaobaoApi.ApiManager.Instance.SendSMS(loginer.UserId.ToString(), num.ToString(), "13520075291,15301024100");
            //var smsStr = JsonConvert.SerializeObject(result);
            //LogHelper.Write("测试短信发送结果：" + smsStr);
            var model = new
            {
                userSettings = userSettings,
                UserId = loginer.UserId,
                UserCode = loginer.UserCode,
                UserName = loginer.UserName,
                UserMenus = list
            };


            return View(model);
        }

        public ActionResult Portal()
        {
            ViewBag.Message = "KM企业快速开发框架";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        //修改自己密码
        //POST /Home/ModifySelfPassword
        public JsonResult ModifySelfPassword(JObject data)
        {
            string newpassword = data.Value<string>("newpassword");
            int userid = this.CurrentBaseLoginer.UserId;
            var result = Base_UserService.Instance.ModifySelfPassword(newpassword, userid);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        //public JsonResult UserMenu()
        //{
        //    string sql = "select * from Base_Menu order by Sort";
        //    var list = Base_MenuService.Instance.GetList_Fish(sql);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ////查询一级模块菜单
        ////POST /Home/UserMenu_First
        //public JsonResult UserMenu_First()
        //{
        //    string sql = "select * from Base_Menu where ParentCode='0' order by Sort";
        //    var list = Base_MenuService.Instance.GetList_Fish(sql);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        ////查询子菜单
        ////POST /Home/UserMenu_Children
        //public JsonResult UserMenu_Children(JObject data)
        //{
        //    string pcode = data.Value<string>("ParentCode");
        //    //string sql = "select * from Base_Menu where ParentCode='" + pcode + "' or ParentCode in(select MenuCode from Base_Menu where ParentCode='" + pcode + "') order by Sort";
        //    var list = Base_MenuService.Instance.GetChildrenMenus(pcode);
        //    return Json(list, JsonRequestBehavior.DenyGet);
        //}
       
        ////查询子菜单
        ////POST /Home/UserMenu_Tree
        //public JsonResult UserMenu_Tree()
        //{
        //    string sql = "select MenuCode as id,MenuName as name,ParentCode as pid,Url as url,'/Scripts/03jeasyui/icons/icon-standard/16x16/'+replace(IconClass,'icon-standard-','')+'.png' as icon from Base_Menu order by Sort";
        //    var list = Base_MenuService.Instance.GetDynamicList(sql);
        //    string JsonString = JsonConvert.SerializeObject(list);
        //    var jsonObject = JsonConvert.DeserializeObject<object>(JsonString);
        //    return JsonNet(jsonObject, JsonRequestBehavior.AllowGet);
        //}

        //修改个人密码 
    }
}