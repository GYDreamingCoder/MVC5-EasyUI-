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
using System.Dynamic;

namespace JTS.Areas.Dev.Controllers
{
    /// <summary>
    /// 数据库管理（Base_DataBase） 控制器类 
    /// </summary>
    public class Base_DataBaseController : BaseController
    {
        //页面：系统参数
        // GET: /Sys/Base_DataBase/
        public ActionResult Index(string menucode)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["getlist_table"] = string.Format("/{0}/{1}/GetList_Table", "Dev", "Base_DataBase");
            expando["getlist_tablecolumn"] = string.Format("/{0}/{1}/GetList_TableColumn", "Dev", "Base_DataBase");
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Dev", "Base_DataBase", expando),
                buttons = this.GetUserMenuButtons(menucode),
                loginer = this.CurrentUser
            };
            return View(model);
        }

        //动作：查询数据表
        // GET: /Sys/Base_DataBase/GetList_Table
        public JsonResult GetList_Table(Dictionary<string, string> data)
        {
            var list = Base_DataBaseService.Instance.GetTableList();
            return JsonNet(new { rows = list, total = list.Count });
        }

        //动作：查询数据表
        // GET: /Sys/Base_DataBase/GetList_TableColumn
        public JsonResult GetList_TableColumn(Dictionary<string, string> data)
        {
            string tableName = data.Value<string>("TableName");
            if (tableName.IsNullOrEmpty()) { return JsonNet(new { rows = new List<sys_column>(), total = 0 }); }
            var list = Base_DataBaseService.Instance.GetTableColumnList(tableName);
            return JsonNet(new { rows = list, total = list.Count });
        }

        /*
        //动作：添加记录
        // POST: /Sys/Base_DataBase/Add
        public JsonResult Add(JObject data)
        {
            var entity = data.ToObject<Base_DataBase>();
            int result = Base_DataBaseService.Instance.Insert(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：编辑记录
        // POST: /Sys/Base_DataBase/Edit
        public JsonResult Edit(JObject data)
        {
            var entity = data.ToObject<Base_DataBase>();
            int result = Base_DataBaseService.Instance.Update(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：添删除记录
        // POST: /Sys/Base_DataBase/Delete
        public JsonResult Delete(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            var entity = data.ToObject<Base_DataBase>();
            int result = Base_DataBaseService.Instance.DeleteByPrimaryField(entity.ParamCode);
            cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }
         */
    }
}