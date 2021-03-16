using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTS.Controllers;
using JTS.Core;
using JTS.Entity;
using JTS.Service;
using JTS.WebUI;
using JTS.Utils;
using Newtonsoft.Json.Linq;

namespace JTS.Areas.Sys.Controllers
{
    public class Base_CodeRuleController : BaseController
    {
        //页面：主页面
        // GET: /Sys/Base_CodeRule/
        public ActionResult Index(string menucode)
        {
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_CodeRule"),
                buttons=this.GetUserMenuButtons(menucode),
                loginer = this.CurrentUser//将当前登录用户对象返回给页面
            };
            return View(model);
        }

        //动作：分页查询列表数据
        // GET: /Sys/Base_CodeRule/GetPageList
        public JsonResult GetPageList(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_CodeRuleService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            return JsonNet(new { rows = list, total = totalRows });
        }

        //动作：添加记录
        // POST: /Sys/Base_CodeRule/Add
        public JsonResult Add(JObject data)
        {
            var entity = data.ToObject<Base_CodeRule>();
            int result = Base_CodeRuleService.Instance.Insert(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：编辑记录
        // POST: /Sys/Base_CodeRule/Edit
        public JsonResult Edit(JObject data)
        {
            var entity = data.ToObject<Base_CodeRule>();
            int result = Base_CodeRuleService.Instance.Update(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：添删除记录
        // POST: /Sys/Base_CodeRule/Delete
        public JsonResult Delete(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            var entity = data.ToObject<Base_CodeRule>();
            int result = Base_CodeRuleService.Instance.DeleteByPrimaryField(entity.RuleCode);
            cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

    }


}