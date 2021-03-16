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
    public class Base_RoleController : BaseController
    {
        //页面：主页面
        // GET: /Sys/Base_Role/
        public ActionResult Index(string menucode)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["pagelist_user"] = string.Format("/{0}/{1}/GetPageList_User", "Sys", "Base_Role");
            expando["getroleusers"] = string.Format("/{0}/{1}/GetRoleUsers", "Sys", "Base_Role");
            expando["addroleusers"] = string.Format("/{0}/{1}/AddRoleUsers", "Sys", "Base_Role");
            expando["removeroleusers"] = string.Format("/{0}/{1}/RemoveRoleUsers", "Sys", "Base_Role");
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_Role", expando),
                buttons=this.GetUserMenuButtons(menucode)
                //loginer = this.CurrentUser//将当前登录用户对象返回给页面
            };
            return View(model);
        }

        //页面：角色权限设置页面 SaveRoleMenuButton
        // GET: /Sys/Base_Role/RoleRights?rolecode=000
        public ActionResult RoleRights(string rolecode)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["menulist"] = string.Format("/{0}/{1}/GetList_Meuns", "Sys", "Base_Role");
            expando["saverolemenus"] = string.Format("/{0}/{1}/SaveRoleMenus", "Sys", "Base_Role");
            expando["saverolemenubutton"] = string.Format("/{0}/{1}/SaveRoleMenuButton", "Sys", "Base_Role");
            expando["saverolemenubutton_all"] = string.Format("/{0}/{1}/SaveRoleMenuButton_All", "Sys", "Base_Role");
            ParamQuery pq = ParamQuery.Instance().From("V_Base_MenuButton").OrderBy("order by MenuCode asc,ButtonSort asc");
            var listMenuButtons = Base_MenuService.Instance.GetMenuButtons();
            var listRoleMenus = Base_RoleService.Instance.GetRoleMenus(rolecode);
            var listRoleMenuButtons = Base_RoleService.Instance.GetRoleMenuButtons(rolecode);
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_Role", expando),
                data = new { rolecode = rolecode },
                list_menu_buttons = listMenuButtons,
                list_role_menus = listRoleMenus,
                list_role_menu_buttons = listRoleMenuButtons
            };
            return View("RoleMenuButton", model);
        }



        //动作：分页查询列表数据
        // GET: /Sys/Base_Role/GetPageList
        public JsonResult GetPageList(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            if (this.CurrentUser.IsSuperAdmin != 1)
            {
                where += " and RoleCode !='000' ";
            }
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_RoleService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            return JsonNet(jsonData);
        }

        //动作：分页查询列表数据
        // GET: /Sys/Base_Role/GetPageList_User
        public JsonResult GetPageList_User(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            string keyword = data.Value<string>("keyword");
            if (keyword.IsNullOrEmpty() == false)
            {
                where += string.Format(" and UserCode  like '%{0}%' or RealName like '%{0}%' or Phone  like '%{0}%' ", keyword);
            }
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_UserService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            return JsonNet(jsonData);
        }

        //动作：查询用于权限设置的菜单列表数据 
        // GET: /Sys/Base_Role/GetList_Meuns
        public JsonResult GetList_Meuns(Dictionary<string, string> data)
        {
            string where = " and Enabled=1 order by Sort ASC";
            var list = Base_MenuService.Instance.GetList_Fish(where, true);
            var jsonData = this.CreateJsonData_DataGrid(list.Count, list, null);
            return JsonNet(jsonData);
        }

        //动作：添加记录
        // POST: /Sys/Base_Role/Add
        public JsonResult Add(JObject data)
        {
            var entity = data.ToObject<Base_Role>();
            int result = Base_RoleService.Instance.Insert(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "保存成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：编辑记录
        // POST: /Sys/Base_Role/Edit
        public JsonResult Edit(JObject data)
        {
            var entity = data.ToObject<Base_Role>();
            int result = Base_RoleService.Instance.Update(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "修改成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：添删除记录
        // POST: /Sys/Base_Role/Delete
        public JsonResult Delete(JObject data)
        {
            var entity = data.ToObject<Base_Role>();
            bool exists_UserRole = Base_UserRoleService.Instance.Exists_Fish("and RoleCode='" + entity.RoleCode + "'");
            bool exists_RoleMenu = Base_RoleMenuService.Instance.Exists_Fish("and RoleCode='" + entity.RoleCode + "'");
            bool exists_RoleMenuButton = Base_RoleMenuButtonService.Instance.Exists_Fish("and RoleCode='" + entity.RoleCode + "'");
            CommandResult cmdResult = CommandResult.Instance_Error;
            if (exists_UserRole)
            {
                cmdResult.ResultMsg = "此角色已经被用户角色关联表使用！";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            if (exists_RoleMenu)
            {
                cmdResult.ResultMsg = "此角色已经被角色菜单关联表使用！";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }
            if (exists_RoleMenu)
            {
                cmdResult.ResultMsg = "此角色已经被角色菜单按钮关联表使用！";
                return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            }

            int result = Base_RoleService.Instance.DeleteByWhere(" and RoleCode='" + entity.RoleCode + "'");
            cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "删除成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：根据角色代码查询角色的用户数据
        // GET: /Sys/Base_Role/GetRoleUsers
        public JsonResult GetRoleUsers(Dictionary<string, string> data)
        {
            string rolecode = data.Value<string>("rolecode");
            if (rolecode.IsNullOrEmpty())
            {
                dynamic result = new ExpandoObject();
                result.rows = new List<dynamic>();
                result.total = 0;
                return JsonNet(result);
            }
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            ParamQuery pq = ParamQuery.Instance().From("V_Base_UserRole").OrderBy("Sort ASC").Paging(pageIndex, pageSize).ClearWhere().AndWhere("RoleCode", rolecode);
            var list = Base_RoleService.Instance.GetDynamicPageList(pq);
            //var list = Base_UserService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            //var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            //return JsonNet(jsonData); 
            //var list = Base_RoleService.Instance.GetRoleUser(rolecode);
            return JsonNet(list);
        }

        //动作：添加角色用户
        // POST: /Sys/Base_Role/AddRoleUsers
        public JsonResult AddRoleUsers(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            string rolecode = data.Value<string>("rolecode");
            var listUser = (data.Value<object>("crows") as JArray).ToList();
            foreach (JObject item in listUser)
            {
                int userid = item.Value<int>("UserId");
                string realname = item.Value<string>("RealName");
                var result = Base_RoleService.Instance.AddRoleUser(rolecode, userid);
                if (result.Succeed == false)
                {
                    cmdResult.ResultMsg += "角色用户[" + realname + "]添加失败";
                }
            }
            // string userids = data.Value<string>("userids");
            //string[] arr = userids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (var item in arr)
            //{
            //    var result = Base_RoleService.Instance.AddRoleUser(rolecode, item.ToInt());
            //    if (result.Succeed == false) {
            //        cmdResult.ResultMsg += "角色用户[" + item + "]添加失败";
            //    }
            //}
            return JsonNet(cmdResult);
        }

        //动作：移除角色用户
        // POST: /Sys/Base_Role/RemoveRoleUsers
        public JsonResult RemoveRoleUsers(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            string rolecode = data.Value<string>("rolecode");
            var listUser = (data.Value<object>("crows") as JArray).ToList();
            foreach (JObject item in listUser)
            {
                int userid = item.Value<int>("UserId");
                string realname = item.Value<string>("RealName");
                var result = Base_RoleService.Instance.RemoveRoleUser(rolecode, userid);
                if (result.Succeed == false)
                {
                    cmdResult.ResultMsg += "角色用户[" + realname + "]移除失败";
                }
            }
            //string userids = data.Value<string>("userids");
            //string[] arr = userids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (var item in arr)
            //{
            //    var result = Base_RoleService.Instance.RemoveRoleUser(rolecode, item.ToInt());
            //    if (result.Succeed == false)
            //    {
            //        cmdResult.ResultMsg += "角色用户[" + item + "]移除失败";
            //    }
            //}
            return JsonNet(cmdResult);
        }


        //动作：保存角色的菜单权限数据
        // POST: /Sys/Base_Role/SaveRoleMenus
        public JsonResult SaveRoleMenus(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            string rolecode = data.Value<string>("rolecode");
            bool flagadd = data.Value<bool>("flagadd");
            string menucodeStr = data.Value<string>("menucode_str");
            string[] arr = menucodeStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arr)
            {
                var result = Base_RoleService.Instance.SaveRoleMenus(rolecode, item, flagadd);
                if (result.Succeed == false)
                {
                    cmdResult.ResultMsg += "(其中角色菜单[" + item + "]保存失败)";
                }
            }
            return JsonNet(cmdResult);
        }

        //动作：保存单个按钮权限
        // POST: /Sys/Base_Role/SaveRoleMenuButton
        public JsonResult SaveRoleMenuButton(JObject data)
        {
            string rolecode = data.Value<string>("rolecode");
            bool flagadd = data.Value<bool>("flagadd");
            string menucode = data.Value<string>("menucode");
            string buttoncode = data.Value<string>("buttoncode");
            CommandResult cmdResult = Base_RoleService.Instance.SaveRoleMenuButton(flagadd, rolecode, menucode, buttoncode);
            return JsonNet(cmdResult);
        }
        //动作：保存单个按钮权限-全选和反选操作
        // POST: /Sys/Base_Role/SaveRoleMenuButton_All
        public JsonResult SaveRoleMenuButton_All(JObject data)
        {
            string rolecode = data.Value<string>("rolecode");
            bool flagadd = data.Value<bool>("flagadd");
            string menucode = data.Value<string>("menucode");
            CommandResult cmdResult = Base_RoleService.Instance.SaveRoleMenuButton_All(flagadd, rolecode, menucode);
            return JsonNet(cmdResult);
        }
    }


}