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
    public class Base_SysLogController : BaseController
    {
        //页面：按钮页面
        // GET: /Sys/Base_SysLog/
        public ActionResult Index(string menucode)
        {
            var model = new
            {
                urls = ControllerHelper.GetIndexUrls("Sys", "Base_SysLog"),
                buttons = this.GetUserMenuButtons(menucode)
            };
            return View(model);
        }

        //动作：分页查询列表数据
        // GET: /Sys/Base_SysLog/GetPageList
        public JsonResult GetPageList(Dictionary<string, string> data)
        {
            string where = " 1=1 ";
            var keyword = data.Value<string>("keyword");
            var logtype = data.Value<int>("logtype");
            var start_rq = data.Value<DateTime>("start_rq");
            var end_rq = data.Value<DateTime>("end_rq"); 

            int pageIndex = data.Value<int>("page");
            int pageSize = data.Value<int>("rows");
            int pageCount = 0;
            int totalRows = 0;
            var list = Base_SysLogService.Instance.GetPageList(where, pageIndex, pageSize, out pageCount, out totalRows);
            var jsonData = this.CreateJsonData_DataGrid(totalRows, list, null);
            return JsonNet(jsonData);
        }
    }


}