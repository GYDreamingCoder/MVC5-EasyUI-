using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTS.Controllers;
using JTS.Core;
using JTS.Service;
using Newtonsoft.Json.Linq;

namespace JTS.Areas.Sys.Controllers
{
    /// <summary>
    /// 个性化设置 控制器
    /// </summary>
    public class ConfigController : BaseController
    {
        //页面：个性化设置
        // GET: /Sys/Config/
        public ActionResult Index()
        {
            var themes = new List<dynamic>();
            themes.Add(new { text = "默认皮肤(default)", value = "default" });
            themes.Add(new { text = "流行灰(gray)", value = "gray" });
            themes.Add(new { text = "银色(bootstrap)", value = "bootstrap" });

            var navigations = new List<dynamic>();
            navigations.Add(new { text = "横向菜单", value = "menubutton" });
            navigations.Add(new { text = "手风琴", value = "accordion" });
            navigations.Add(new { text = "树形结构", value = "tree" });

            var model = new
            {
                dataSource = new
                {
                    themes = themes,
                    navigations = navigations
                },
                form = Base_UserService.Instance.GetCurrentUserSettings()
            };
            return View(model);
        }

        //动作：保存个性化设置信息
        // POST: /Sys/Config/SaveUserSetting
        public JsonResult SaveUserSetting(JObject data)
        {
            var theme = data.Value<string>("theme");
            var navigation = data.Value<string>("navigation");
            var gridrows = data.Value<int>("gridrows");
            Base_UserService.Instance.SaveCurrentUserSettings(data);
            return new JsonNetResult(CommandResult.Instance_Succeed);
        }
    }
}