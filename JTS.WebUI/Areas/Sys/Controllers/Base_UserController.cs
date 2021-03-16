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
    /// 系统用户-控制器
    /// </summary>
    public class Base_UserController : BaseController
    {
        //页面：主页面
        // GET: /Sys/Base_User/
        public ActionResult Index(string menucode)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["listorgdata"] = string.Format("/{0}/{1}/GetList_Org", "Sys", "Base_User");
            expando["resetpwd"] = string.Format("/{0}/{1}/ResetPwd", "Sys", "Base_User");
            expando["audituser"] = string.Format("/{0}/{1}/AuditUser", "Sys", "Base_User");
            expando["listroledata"] = string.Format("/{0}/{1}/GetList_Role", "Sys", "Base_User");
            expando["getlist_userrole"] = string.Format("/{0}/{1}/GetList_UserRole", "Sys", "Base_User");
            expando["save_userroles"] = string.Format("/{0}/{1}/Save_UserRoles", "Sys", "Base_User");
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_User", expando),
                buttons = this.GetUserMenuButtons(menucode),
                loginer = this.CurrentUser//将当前登录用户对象返回给页面
            };
            return View(model);
        }

        //动作：分页查询列表数据
        // GET: /Sys/Base_User/GetPageList
        public JsonResult GetPageList(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            if (this.CurrentUser.IsSuperAdmin != 1)
            {
                where += " and ( IsSuperAdmin=0 )";
            }
            string keyword = data.Value<string>("keyword");
            if (keyword.IsNullOrEmpty() == false)
            {
                where += string.Format(" and ( RealName like '%{0}%' or UserCode like '%{0}%' or Phone like '%{0}%' or  Email like '%{0}%' or  QQ like '%{0}%' or  CompanyName like '%{0}%' or DepartmentName like '%{0}%' ) ", keyword);
            }

            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_UserService.Instance.GetDynamicPageList("V_Base_User", "*", where, "Sort ASC", pageIndex, pageSize, out pageCount, out totalRows);
            return JsonNet(list);
        }

        //动作：查询列表数据 
        // GET: /Sys/Base_User/GetList_Org
        public JsonResult GetList_Org(Dictionary<string, string> data)
        {
            var list = Base_CompanyService.Instance.GetTreeData_Comany();
            return JsonNet(list);
        }

        //动作：查询角色列表
        // GET: /Sys/Base_User/GetList_Role
        public JsonResult GetList_Role(Dictionary<string, string> data)
        {
            string where = " and Enabled=1 ";
            if (this.CurrentUser.IsSuperAdmin != 1)
            {
                where += " and RoleType>1 ";
            }
            var list = Base_RoleService.Instance.GetList_Fish(where, true);
            var jsonData = this.CreateJsonData_DataGrid(list.Count, list, null);
            return JsonNet(jsonData);
        }

        //动作：查询用户角色列表
        // GET: /Sys/Base_User/GetList_UserRole
        public JsonResult GetList_UserRole(Dictionary<string, string> data)
        {
            int userid = data.Value<int>("UserId");
            var list = Base_UserRoleService.Instance.GetList_Fish(" and UserId=" + userid + "", true);
            return JsonNet(list);
        }

        //动作：保存用户角色
        // POST: /Sys/Base_User/Save_UserRoles
        public JsonResult Save_UserRoles(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            int UserId = data.Value<int>("userid");
            var listUser = (data.Value<object>("crows") as JArray).ToList();
            Base_UserRoleService.Instance.DeleteByWhere("and UserId=" + UserId);
            foreach (JObject item in listUser)
            {
                string RoleCode = item.Value<string>("RoleCode");
                string RoleName = item.Value<string>("RoleName");
                Base_UserRole obj = new Base_UserRole() { UserId = UserId, RoleCode = RoleCode };
                bool exists = Base_UserRoleService.Instance.Exists_Fish("and UserId=" + UserId + " and RoleCode='" + RoleCode + "'");
                var result = Base_UserRoleService.Instance.Insert(obj);
                if (result == 0)
                {
                    cmdResult.ResultMsg += "用户角色[" + RoleName + "]插入失败";
                }
            }
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：添加记录
        // POST: /Sys/Base_User/Add
        public JsonResult Add(JObject data)
        {
            var entity = data.ToObject<Base_User>();
            entity.Password = Md5Util.MD5(entity.UserCode);//新增用户，密码默认和登录账号一样
            int result = Base_UserService.Instance.Insert(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "保存成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：编辑记录
        // POST: /Sys/Base_User/Edit
        public JsonResult Edit(JObject data)
        {
            var entity = data.ToObject<Base_User>();
            int result = Base_UserService.Instance.Update(entity);
            CommandResult cmdResult = result > 0 ? CommandResult.Instance_Succeed : CommandResult.Instance_Error;
            cmdResult.ResultMsg = "修改成功！";
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：删除记录
        // POST: /Sys/Base_User/Delete
        public JsonResult Delete(JObject data)
        {
            var entity = data.ToObject<Base_User>();
            CommandResult cmdResult = CommandResult.Instance_Error;
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：用户密码重置
        // POST: /Sys/Base_User/ResetPwd
        public JsonResult ResetPwd(JObject data)
        {
            int UserId = data.Value<int>("UserId");
            string UserCode = data.Value<string>("UserCode");
            string RealName = data.Value<string>("RealName");
            CommandResult cmdResult = Base_UserService.Instance.ResetUserPassword(UserId, RealName, UserCode);
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
            //cmdResult.ResultMsg = "密码重置成功";
            //int UserId = data.Value<int>("UserId");
            //string UserCode = data.Value<string>("UserCode");
            //string RealName = data.Value<string>("RealName");
            //string newPwd = Md5Util.MD5("123456");
            //var par = ParamUpdate.Instance().Update("Base_User")
            //    .Column("Password", newPwd)
            //    .Column("LastChangePassword", DateTime.Now)
            //    .AndWhere("UserId", UserId);
            //var result = Base_UserService.Instance.Update(par);
            //if (result == 0) { cmdResult.Set(false, "密码重置失败"); }
            //string logmsg = string.Format("用户密码重置。结果：{3}。用户：{0}-({1})，登录账号:{2}", RealName, UserId, UserCode, cmdResult.ResultMsg);
            //LogHelper.Write(logmsg);

        }

        //动作：审核用户 
        // POST: /Sys/Base_User/AduitUser
        public JsonResult AuditUser(JObject data)
        {
            int UserId = data.Value<int>("UserId");
            CommandResult cmdResult = Base_UserService.Instance.AuditUser(UserId, this.CurrentBaseLoginer.UserName);
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }


    }
}