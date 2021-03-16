using System.Web;
using System.Web.Mvc;

namespace JTS.WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());//默认的，删除
            
            //添加全局过滤器 
            filters.Add(new CustomHandleErrorAttribute());//自定义MVC异常处理 过滤器 
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());//使用默认的授权
            filters.Add(new CustomDisposeFilter());//自定义 Action过滤器：垃圾回收
    
        }
    }
}
