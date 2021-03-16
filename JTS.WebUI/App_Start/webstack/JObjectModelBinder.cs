﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTS.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JTS.WebUI
{
    /// <summary>
    /// 注册绑定模型：JObject
    /// </summary>
    public class JObjectModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //var request = controllerContext.RequestContext.HttpContext.Request;
            //var httpMethod = controllerContext.RequestContext.HttpContext.Request.HttpMethod.ToLower();
            //if (request.Form.Count > 0)
            //{
            //    IDictionary<string, string> dicts = controllerContext.RequestContext.HttpContext.Request.Form.ToDictionary();
            //    string json0 = JsonConvert.SerializeObject(dicts);
            //    var result =JsonConvert.DeserializeObject<dynamic>(json0);
            //    return result;
            //}
            var vv = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var stream = controllerContext.RequestContext.HttpContext.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(stream).ReadToEnd();

            return JsonConvert.DeserializeObject<dynamic>(json);
        }
    }

    public class DictionaryModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IDictionary<string, string> dicts = controllerContext.RequestContext.HttpContext.Request.QueryString.ToDictionary();
            return dicts;
        }
    }

    //controllerContext.RequestContext.HttpContext.Request.Form
    public class JObjectPostModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IDictionary<string, string> dicts = controllerContext.RequestContext.HttpContext.Request.Form.ToDictionary();
            string json = JsonConvert.SerializeObject(dicts);
            var result = (JObjectPost)JsonConvert.DeserializeObject<dynamic>(json);
            return result;
        }
    }

    public class JObjectPost : JObject
    {

    }
}