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

namespace JTS.Areas.Basic.Controllers
{
    /// <summary>
    /// 地区-控制器
    /// </summary>
    public class Base_AreaController : BaseController
    {
        //页面：主页面
        // GET: /Sys/Base_Area/
        public ActionResult Index(string menucode)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            //expando["pagelist_button"] = string.Format("/{0}/{1}/GetPageList_Buttons", "Sys", "Base_Area");
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Basic", "Base_Area", expando)
            };
            return View(model);
        }

        //动作：分页查询列表数据
        // GET: /Sys/Base_Area/GetList
        public JsonResult GetList(Dictionary<string, string> data)
        {
            string where = "and 1=1 ";
            var id = data.Value<int>("id");
            where = " and ParentId=" + id;
            var list = Base_AreaService.Instance.GetList_Fish(where,true);
            list.ForEach(delegate(Base_Area item)
            {
                bool existsChilds = Base_AreaService.Instance.Exists_Fish(" and ParentId=" + item.Id);
                item.state = existsChilds == true ? "closed" : "open";
            }); 
            return JsonNet(list);
        }
        //动作：分页查询列表数据
        // GET: /Sys/Base_Area/GetPageList
        public JsonResult GetPageList(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            var id = data.Value<int>("id");
            where += " and ParentId="+id;
            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_AreaService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            return JsonNet(jsonData);
        }

       

    }
}