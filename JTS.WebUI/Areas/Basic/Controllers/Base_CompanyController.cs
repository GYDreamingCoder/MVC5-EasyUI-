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

namespace JTS.Areas.Basic.Controllers
{
    /// <summary>
    /// 公司-控制器 - Base_Company	 
    /// </summary>
    public class Base_CompanyController : BaseController
    {
        //页面：主页面
        // GET: /Sys/Base_Company/
        public ActionResult Index(string menucode)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["listtreedata"] = string.Format("/{0}/{1}/GetList", "Basic", "Base_Company");
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Basic", "Base_Company", expando)
            };
            return View(model);
        }

        //动作：查询列表数据 
        // GET: /Sys/Base_Company/GetList
        public JsonResult GetList(Dictionary<string, string> data)
        {
            var list =Base_CompanyService.Instance.GetTreeData_Comany();
            return JsonNet(list);
        }


        //动作：添加记录
        // POST: /Sys/Base_Company/Add
        public JsonResult Add(JObject data)
        {
            CommandResult cmdResult = Base_CompanyService.Instance.AddOrg(data, this.CurrentBaseLoginer.UserName);
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：编辑记录
        // POST: /Sys/Base_Company/Edit
        public JsonResult Edit(JObject data)
        {
            CommandResult cmdResult = Base_CompanyService.Instance.EditOrg(data, this.CurrentBaseLoginer.UserName);
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：删除记录
        // POST: /Sys/Base_Company/Delete
        public JsonResult Delete(JObject data)
        {
            var cmdResult = Base_CompanyService.Instance.DeleteOrg(data);
            return JsonNet(cmdResult, JsonRequestBehavior.DenyGet);
        }

        //动作：根据菜单代码查询菜单按钮数据
        // GET: /Sys/Base_Company/GetCompanyDepartments
        public JsonResult GetCompanyDepartments(Dictionary<string, string> data)
        {
            string CompanyCode = data.Value<string>("CompanyCode");
            if (CompanyCode.IsNullOrEmpty())
            {
                dynamic result = new ExpandoObject();
                result.rows = new List<dynamic>();
                result.total = 0;
                return JsonNet(result);
            }
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            ParamQuery pq = ParamQuery.Instance().From("V_Base_CompanyDepartment").OrderBy("DepartmentSort ASC").Paging(pageIndex, pageSize).ClearWhere().AndWhere("CompanyCode", CompanyCode);
            var list = Base_CompanyService.Instance.GetDynamicPageList(pq);
            return JsonNet(list);
        }

        //动作：批量添加菜单按钮
        // POST: /Sys/Base_Company/AddCompanyDepartments
        public JsonResult AddCompanyDepartments(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            //string menucode = data.Value<string>("menucode");
            //var listDepartments = (data.Value<object>("crows") as JArray).ToList();
            //foreach (JObject item in listDepartments)
            //{
            //    string buttonCode = item.Value<string>("DepartmentCode");
            //    string buttonName = item.Value<string>("DepartmentName");
            //    int sort = item.Value<int>("Sort");
            //    var result = Base_CompanyService.Instance.AddCompanyDepartment(menucode, buttonCode, sort, "");
            //    if (result.Succeed == false)
            //    {
            //        cmdResult.ResultMsg += "菜单按钮[" + buttonName + "]添加失败";
            //    }
            //}
            return JsonNet(cmdResult);
        }

        //动作：批量移除菜单按钮
        // POST: /Sys/Base_Company/RemoveCompanyDepartments
        public JsonResult RemoveCompanyDepartments(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            //string menucode = data.Value<string>("menucode");
            //var listDepartments = (data.Value<object>("crows") as JArray).ToList();
            //foreach (JObject item in listDepartments)
            //{
            //    string buttonCode = item.Value<string>("DepartmentCode");
            //    string buttonName = item.Value<string>("DepartmentName");
            //    var result = Base_CompanyService.Instance.RemoveCompanyDepartment(menucode, buttonCode);
            //    if (result.Succeed == false)
            //    {
            //        cmdResult.ResultMsg += "菜单按钮[" + buttonName + "]移除失败";
            //    }
            //}
            return JsonNet(cmdResult);
        }
        //动作：批量保存菜单按钮顺序和显示文本
        // POST: /Sys/Base_Company/SaveCompanyDepartments
        public JsonResult SaveCompanyDepartments(JObject data)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            //string menucode = data.Value<string>("menucode");
            //var listDepartments = (data.Value<object>("crows") as JArray).ToList();
            //foreach (JObject item in listDepartments)
            //{
            //    string buttonCode = item.Value<string>("DepartmentCode");
            //    string buttonName = item.Value<string>("DepartmentName");
            //    int buttonSort = item.Value<int>("DepartmentSort");
            //    string buttonText = item.Value<string>("DepartmentText");
            //    buttonText = buttonText.IsNullOrEmpty() ? "" : buttonText;
            //    var result = Base_CompanyService.Instance.SaveCompanyDepartment(menucode, buttonCode, buttonSort, buttonText);
            //    if (result.Succeed == false)
            //    {
            //        cmdResult.ResultMsg += "菜单按钮[" + buttonName + "]保存失败";
            //    }
            //}
            return JsonNet(cmdResult);
        }

    }
}

