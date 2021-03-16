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
    public class Base_ButtonController : BaseController
    {
        //页面：按钮页面
        // GET: /Sys/Base_Button/
        public ActionResult Index(string menucode)
        {
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_Button"),
                buttons = this.GetUserMenuButtons(menucode)
            };
            return View(model);
        }

        //动作：分页查询列表数据
        // GET: /Sys/Base_Button/GetPageList
        public JsonResult GetPageList(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_ButtonService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            return JsonNet(jsonData);
        }

        //动作：添加记录
        // POST: /Sys/Base_Button/Add
        public JsonResult Add(JObject data)
        {
            var entity = data.ToObject<Base_Button>();
            int result = Base_ButtonService.Instance.Insert(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "保存成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：编辑记录
        // POST: /Sys/Base_Button/Edit
        public JsonResult Edit(JObject data)
        {
            var entity = data.ToObject<Base_Button>();
            int result = Base_ButtonService.Instance.Update(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "修改成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：添删除记录
        // POST: /Sys/Base_Button/Delete
        public JsonResult Delete(JObject data)
        {
            var entity = data.ToObject<Base_Button>();
            bool exists = Base_MenuService.Instance.ExistsMenuButton(entity.ButtonCode);
            CommandResult cmdResult = CommandResult.Instance_Error;
            if (exists)
            {
                cmdResult.ResultMsg = "此按钮已经被菜单使用！";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            int result = Base_ButtonService.Instance.DeleteByWhere(" and ButtonCode='" + entity.ButtonCode + "'");
            cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "删除成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

    }


}