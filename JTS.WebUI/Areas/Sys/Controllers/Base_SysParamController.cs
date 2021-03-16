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
    /// <summary>
    /// 系统参数（Base_SysParam） 控制器类 
    /// </summary>
    public class Base_SysParamController : BaseController
    {
        //页面：系统参数
        // GET: /Sys/Base_SysParam/
        public ActionResult Index(string menucode)
        {
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_SysParam"),
                buttons=this.GetUserMenuButtons(menucode),
                loginer = this.CurrentUser
            };
            return View(model);
        }

        //动作：分页查询列表数据
        // GET: /Sys/Base_SysParam/GetPageList
        public JsonResult GetPageList(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_SysParamService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            return JsonNet(new { rows = list, total = totalRows });
        }

        //动作：添加记录
        // POST: /Sys/Base_SysParam/Add
        public JsonResult Add(JObject data)
        {
            var entity = data.ToObject<Base_SysParam>();
            int result = Base_SysParamService.Instance.Insert(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：编辑记录
        // POST: /Sys/Base_SysParam/Edit
        public JsonResult Edit(JObject data)
        {
            var entity = data.ToObject<Base_SysParam>();
            int result = Base_SysParamService.Instance.Update(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：添删除记录
        // POST: /Sys/Base_SysParam/Delete
        public JsonResult Delete(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            var entity = data.ToObject<Base_SysParam>();
            int result = Base_SysParamService.Instance.DeleteByPrimaryField(entity.ParamCode);
            cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

    }
}