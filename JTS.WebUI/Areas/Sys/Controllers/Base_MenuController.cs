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

namespace JTS.Areas.Sys.Controllers
{
    /// <summary>
    /// 系统角色-控制器
    /// </summary>
    public class Base_MenuController : BaseController
    {
        //页面：主页面
        // GET: /Sys/Base_Menu/
        public ActionResult Index(string menucode)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["pagelist_button"] = string.Format("/{0}/{1}/GetPageList_Buttons", "Sys", "Base_Menu");
            expando["getmenubuttons"] = string.Format("/{0}/{1}/GetMenuButtons", "Sys", "Base_Menu");
            expando["addmenubuttons"] = string.Format("/{0}/{1}/AddMenuButtons", "Sys", "Base_Menu");
            expando["removemenubuttons"] = string.Format("/{0}/{1}/RemoveMenuButtons", "Sys", "Base_Menu");
            expando["savemenubuttons"] = string.Format("/{0}/{1}/SaveMenuButtons", "Sys", "Base_Menu");
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_Menu", expando),
                buttons = this.GetUserMenuButtons(menucode)
            };
            ViewBag.Ver = Guid.NewGuid().ToString("N");
            return View(model);
        }

        //动作：查询列表数据 
        // GET: /Sys/Base_Menu/GetList
        public JsonResult GetList(Dictionary<string, string> data)
        {
            string where = " and 1=1 order by Sort ASC";
            var list = Base_MenuService.Instance.GetList_Fish(where, true);
            var jsonData = this.CreateJsonData_DataGrid(list.Count, list, null);
            return JsonNet(jsonData);
        }
        //动作：分页查询列表数据
        // GET: /Sys/Base_Menu/GetPageList
        public JsonResult GetPageList(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_MenuService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            return JsonNet(jsonData);
        }

        //动作：分页查询列表数据
        // GET: /Sys/Base_Menu/GetPageList_Buttons
        public JsonResult GetPageList_Buttons(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            string keyword = data.Value<string>("keyword");
            if (keyword.IsNullOrEmpty() == false)
            {
                where += string.Format(" and ButtonCode  like '%{0}%' or ButtonName like '%{0}%' or JsEvent  like '%{0}%' ", keyword);
            }
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_ButtonService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            return JsonNet(jsonData);
        }

        //动作：添加记录
        // POST: /Sys/Base_Menu/Add
        public JsonResult Add(JObject data)
        {
            var entity = data.ToObject<Base_Menu>();
            int result = Base_MenuService.Instance.Insert(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "保存成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：编辑记录
        // POST: /Sys/Base_Menu/Edit
        public JsonResult Edit(JObject data)
        {
            var entity = data.ToObject<Base_Menu>();
            int result = Base_MenuService.Instance.Update(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            if (result > 0 && entity.Enabled==0)
            {
                Base_MenuService.Instance.DisableChildrenMenus(entity.MenuCode);
            }
            cmdResult.ResultMsg = "修改成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：删除记录
        // POST: /Sys/Base_Menu/Delete
        public JsonResult Delete(JObject data)
        {
            var entity = data.ToObject<Base_Menu>();
            var cmdResult = Base_MenuService.Instance.DeleteMenu(entity.MenuCode);
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            #region 弃用
            /* 
            bool exists_MenuButton = Base_MenuButtonService.Instance.Exists_Fish("MenuCode='" + entity.MenuCode + "'");
            bool exists_RoleMenu = Base_RoleMenuService.Instance.Exists_Fish("MenuCode='" + entity.MenuCode + "'");
            bool exists_RoleMenuButton = Base_RoleMenuButtonService.Instance.Exists_Fish("MenuCode='" + entity.MenuCode + "'");
            CommandResult cmdResult = CommandResult.Instance_Error;
            if (exists_MenuButton)
            {
                cmdResult.ResultMsg = "此菜单已经被菜单按钮关联表使用！";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            if (exists_RoleMenu)
            {
                cmdResult.ResultMsg = "此菜单已经被角色菜单关联表使用！";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            if (exists_RoleMenu)
            {
                cmdResult.ResultMsg = "此菜单已经被角色菜单按钮关联表使用！";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }

            int result = Base_MenuService.Instance.DeleteByWhere(" and MenuCode='" + entity.MenuCode + "'");
            cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "删除成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
             */

            #endregion
        }

        //动作：根据菜单代码查询菜单按钮数据
        // GET: /Sys/Base_Menu/GetMenuButtons
        public JsonResult GetMenuButtons(Dictionary<string, string> data)
        {
            string menucode = data.Value<string>("menucode");
            if (menucode.IsNullOrEmpty())
            {
                dynamic result = new ExpandoObject();
                result.rows = new List<dynamic>();
                result.total = 0;
                return JsonNet(result);
            }
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            ParamQuery pq = ParamQuery.Instance().From("V_Base_MenuButton").OrderBy("ButtonSort ASC").Paging(pageIndex, pageSize).ClearWhere().AndWhere("MenuCode", menucode);
            var list = Base_MenuService.Instance.GetDynamicPageList(pq);
            return JsonNet(list);
        }

        //动作：批量添加菜单按钮
        // POST: /Sys/Base_Menu/AddMenuButtons
        public JsonResult AddMenuButtons(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            string menucode = data.Value<string>("menucode");
            var listButtons = (data.Value<object>("crows") as JArray).ToList();
            foreach (JObject item in listButtons)
            {
                string buttonCode = item.Value<string>("ButtonCode");
                string buttonName = item.Value<string>("ButtonName");
                int sort = item.Value<int>("Sort");
                var result = Base_MenuService.Instance.AddMenuButton(menucode, buttonCode, sort, "");
                if (result.Succeed == false)
                {
                    cmdResult.ResultMsg += "菜单按钮[" + buttonName + "]添加失败";
                }
            }
            return JsonNet(cmdResult);
        }

        //动作：批量移除菜单按钮
        // POST: /Sys/Base_Menu/RemoveMenuButtons
        public JsonResult RemoveMenuButtons(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            string menucode = data.Value<string>("menucode");
            var listButtons = (data.Value<object>("crows") as JArray).ToList();
            foreach (JObject item in listButtons)
            {
                string buttonCode = item.Value<string>("ButtonCode");
                string buttonName = item.Value<string>("ButtonName");
                var result = Base_MenuService.Instance.RemoveMenuButton(menucode, buttonCode);
                if (result.Succeed == false)
                {
                    cmdResult.ResultMsg += "菜单按钮[" + buttonName + "]移除失败";
                }
            }
            return JsonNet(cmdResult);
        }
        //动作：批量保存菜单按钮顺序和显示文本
        // POST: /Sys/Base_Menu/SaveMenuButtons
        public JsonResult SaveMenuButtons(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            string menucode = data.Value<string>("menucode");
            var listButtons = (data.Value<object>("crows") as JArray).ToList();
            foreach (JObject item in listButtons)
            {
                string buttonCode = item.Value<string>("ButtonCode");
                string buttonName = item.Value<string>("ButtonName");
                int buttonSort = item.Value<int>("ButtonSort");
                string buttonText = item.Value<string>("ButtonText");
                buttonText = buttonText.IsNullOrEmpty() ? "" : buttonText;
                var result = Base_MenuService.Instance.SaveMenuButton(menucode, buttonCode, buttonSort, buttonText);
                if (result.Succeed == false)
                {
                    cmdResult.ResultMsg += "菜单按钮[" + buttonName + "]保存失败";
                }
            }
            return JsonNet(cmdResult);
        }

    }


}