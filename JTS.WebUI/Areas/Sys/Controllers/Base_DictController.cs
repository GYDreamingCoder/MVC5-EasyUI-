
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTS.Controllers;
using JTS.Core;
using JTS.Utils;
using JTS.Entity;
using JTS.Service;
using JTS.WebUI;
using Newtonsoft.Json.Linq;
using System.Text;

namespace JTS.Areas.Sys.Controllers
{
    /// <summary>
    /// 数据字典-控制器 - Base_Dict	 
    /// </summary>
    public class Base_DictController : BaseController
    {
        //页面：主页面
        // GET: /Sys/Base_Dict/
        public ActionResult Index(string menucode)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["getlist_dict"] = string.Format("/{0}/{1}/GetList_Dict", "Sys", "Base_Dict");
            expando["getpagelist_dictitem"] = string.Format("/{0}/{1}/GetPageList_DictItem", "Sys", "Base_Dict");
            expando["add_dictitem"] = string.Format("/{0}/{1}/Add_DictItem", "Sys", "Base_Dict");
            expando["edit_dictitem"] = string.Format("/{0}/{1}/Edit_DictItem", "Sys", "Base_Dict");
            expando["delete_dictitem"] = string.Format("/{0}/{1}/Delete_DictItem", "Sys", "Base_Dict");
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_Dict", expando),
                buttons = this.GetUserMenuButtons(menucode)
            };
            return View(model);
        }

        //动作：查询字典分类树形数据  
        // GET: /Sys/Base_Dict/GetList_Dict
        public JsonResult GetList_Dict(Dictionary<string, string> data)
        {
            var list = Base_DictService.Instance.GetTreeData_Dict();
            return JsonNet(list);
        }

        //动作：分页查询字典明细列表数据
        // GET: /Sys/Base_Role/GetPageList_DictItem
        public JsonResult GetPageList_DictItem(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            string DictCode = data.Value<string>("DictCode");
            if (DictCode.IsNullOrEmpty()) { return JsonNet(this.CreateJsonData_DataGrid(0, new List<Base_DictItem>(), null)); }
            where += " and DictCode='" + DictCode + "'";
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_DictItemService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            return JsonNet(jsonData);
        }

        //动作：添加记录
        // POST: /Sys/Base_Dict/Add
        public JsonResult Add(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            var entity = data.ToObject<Base_Dict>();
            bool exists = Base_DictService.Instance.Exists_Fish(" and DictCode='" + entity.DictCode + "'");
            if (exists)
            {
                cmdResult = CommandResult.Instance_Error;
                cmdResult.ResultMsg = "字典分类代码已被使用";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            else
            {
                int result = Base_DictService.Instance.Insert(entity);
                if (result == 0)
                {
                    cmdResult = CommandResult.Instance_Error; cmdResult.ResultMsg = "新建字典分类失败";
                }
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
        }

        //动作：编辑记录
        // POST: /Sys/Base_Dict/Edit
        public JsonResult Edit(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            var entity = data.ToObject<Base_Dict>();
            bool exists = Base_DictService.Instance.Exists_Fish(" and DictCode='" + entity.DictCode + "'");
            if (exists == false)
            {
                cmdResult = CommandResult.Instance_Error;
                cmdResult.ResultMsg = "字典分类代码【" + entity.DictCode + "】不存在";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            else
            {
                int result = Base_DictService.Instance.Update(entity);
                if (result == 0)
                {
                    cmdResult = CommandResult.Instance_Error; cmdResult.ResultMsg = "更新字典分类失败";
                }
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
        }

        //动作：删除记录
        // POST: /Sys/Base_Dict/Delete
        public JsonResult Delete(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            string DictCode = data.Value<string>("DictCode");
            Base_DictItemService.Instance.DeleteByWhere(" and DictCode='" + DictCode + "'");
            Base_DictService.Instance.DeleteByWhere(" and DictCode in(select DictCode from [dbo].[fn_GetChildrenDictCode]('" + DictCode + "'))");
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }


        //动作：添加字典明细记录
        // POST: /Sys/Base_Dict/Add_DictItem
        public JsonResult Add_DictItem(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            var entity = data.ToObject<Base_DictItem>();
            bool exists = Base_DictItemService.Instance.Exists_Fish(" and DictItemCode='" + entity.DictItemCode + "' ");
            if (exists)
            {
                cmdResult = CommandResult.Instance_Error;
                cmdResult.ResultMsg = "字典代码已被使用";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            else
            {
                int result = Base_DictItemService.Instance.Insert(entity);
                if (result == 0)
                {
                    cmdResult = CommandResult.Instance_Error; cmdResult.ResultMsg = "新建字典失败";
                }
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
        }

        //动作：编辑字典明细记录
        // POST: /Sys/Base_Dict/Edit_DictItem
        public JsonResult Edit_DictItem(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            var entity = data.ToObject<Base_DictItem>();
            bool exists = Base_DictItemService.Instance.Exists_Fish(" and DictItemCode='" + entity.DictItemCode + "' ");
            if (exists==false)
            {
                cmdResult = CommandResult.Instance_Error;
                cmdResult.ResultMsg = "字典代码【" + entity.DictItemCode + "】不存在";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            else
            {
                int result = Base_DictItemService.Instance.Update(entity);
                if (result == 0)
                {
                    cmdResult = CommandResult.Instance_Error; cmdResult.ResultMsg = "更新字典失败";
                }
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
        }

        //动作：删除记录
        // POST: /Sys/Base_Dict/Delete_DictItem
        public JsonResult Delete_DictItem(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            string DictItemCode = data.Value<string>("DictItemCode");
            Base_DictItemService.Instance.DeleteByWhere(" and DictItemCode='" + DictItemCode + "'"); 
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }
    }
}

