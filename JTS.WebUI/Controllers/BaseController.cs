using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTS.Core;
using JTS.Entity;
using JTS.Utils;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Dynamic;
using JTS.Service;
// 无限光年网络科技
namespace JTS.Controllers
{
    /// <summary>
    /// 控制器基类。
    /// <remarks>所有授权控制器全部继承此类</remarks>
    /// </summary>
    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewBag.ts = DateTime.Now.ToString("yyyyMMddhhmmss");
        }

        /// <summary>
        /// 获取当前系统登录用户
        /// </summary>
        protected internal Base_User CurrentUser
        {
            get
            {
                return (CurrentBaseLoginer.Data as JObject).ToObject<Base_User>();
            }
        }

        /// <summary>
        /// 获取当前的登录用户者
        /// </summary>
        protected internal BaseLoginer CurrentBaseLoginer
        {
            get
            {
                return FormsAuth.GetBaseLoginerData();
            }
        }

        /// <summary>
        /// 获取当前用户的菜单权限按钮
        /// </summary>
        /// <param name="menucode">菜单代码</param>
        /// <returns>dynamic</returns>
        protected dynamic GetUserMenuButtons(string menucode)
        {
            return Base_MenuService.Instance.GetUserMenuButtons(this.CurrentBaseLoginer.UserId, menucode);
        }




        /// <summary>
        /// 创建Json数据，Easyui的DataGrid专用
        /// </summary>
        /// <param name="total"></param>
        /// <param name="rows"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        protected string CreateJsonString_DataGrid(int total, object rows, object footer)
        {
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            dicData.Add("total", total);
            dicData.Add("rows", rows);
            if (footer != null)
            {
                dicData.Add("footer", footer);
            }
            string jsonResultString = Newtonsoft.Json.JsonConvert.SerializeObject(dicData, Formatting.Indented, this.DefaultTimeConverter);
            return jsonResultString;
            /*
             var footer = new[]
            {
                new {  SL = list.Sum(p => p.SL), JE = list.Sum(p => p.JE), GGMC = "合计" }
            };
             */
        }

        /// <summary>
        /// 创建Easyui的DataGrid数据格式
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected dynamic CreateData_DataGrid(IList list, int total)
        {
            dynamic result = new ExpandoObject();
            result.rows = list;
            result.total = total;
            return result;
        }

        /// <summary>
        /// 创建Json数据，Easyui的DataGrid专用
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <param name="rows">当前页记录数</param>
        /// <param name="footer">表格footer</param>
        /// <returns>字典</returns>
        protected Dictionary<string, object> CreateJsonData_DataGrid(int total, object rows, object footer)
        {
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            dicData.Add("total", total);
            dicData.Add("rows", rows);
            if (footer != null)
            {
                dicData.Add("footer", footer);
            }
            return dicData;
            /*
             var footer = new[]
            {
                new {  SL = list.Sum(p => p.SL), JE = list.Sum(p => p.JE), GGMC = "合计" }
            };
             */
        }

        /// <summary>
        /// 获取json序列化是日期格式方式。
        /// </summary>
        protected IsoDateTimeConverter DefaultTimeConverter
        {
            get
            {
                return new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss",
                    DateTimeStyles = System.Globalization.DateTimeStyles.AllowInnerWhite
                };
            }
        }

        /// <summary>
        /// 摘要: 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 System.Web.Mvc.JsonResult 对象。
        /// </summary>
        /// <param name="data">参数: data: 要序列化的 JavaScript 对象图。</param>
        /// <returns> 返回结果:将指定对象序列化为 JSON 格式的 JSON 结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        protected internal JsonResult JsonNet(object data)
        {
            return new JsonNetResult(data);
        }

        /// <summary>
        /// 摘要: 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 System.Web.Mvc.JsonResult 对象。
        /// </summary>
        /// <param name="data">参数: data: 要序列化的 JavaScript 对象图。</param>
        /// <param name="behavior">指定是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns> 返回结果:将指定对象序列化为 JSON 格式的 JSON 结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        protected internal JsonResult JsonNet(object data, JsonRequestBehavior behavior)
        {
            return new JsonNetResult(data, behavior);
        }

    }

    /// <summary>
    /// 定义自JsonResult对象JsonNetResult
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult() { }
        public JsonNetResult(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet, string contentType = null, Encoding contentEncoding = null)
        {
            this.Data = data;
            this.JsonRequestBehavior = behavior;
            this.ContentEncoding = contentEncoding;
            this.ContentType = contentType;
        }

        /// <summary>
        /// 获取json序列化是日期格式方式。
        /// </summary>
        protected IsoDateTimeConverter IsoDateTimeConverter
        {
            get
            {
                return new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss",
                    DateTimeStyles = System.Globalization.DateTimeStyles.AllowInnerWhite
                };
            }
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            var json = JsonConvert.SerializeObject(this.Data, Formatting.Indented, this.IsoDateTimeConverter);
            response.Write(json);
        }
    }

}

