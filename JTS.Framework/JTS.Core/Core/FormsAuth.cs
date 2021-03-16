/*************************************************************************
 * 文件名称 ：FormsAuth.cs                          
 * 描述说明 ：窗体登陆验证
 * 
 **************************************************************************/

using System;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JTS.Core
{
    /// <summary>
    /// 框架唯一身份认证工具类
    /// </summary>
    public static class FormsAuth
    {
        /// <summary>
        /// 登入系统。
        /// <remarks>登录成功后，</remarks>
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="userData">自定义数据， 存放系统登录者（BaseLoginer）对象数据</param>
        /// <param name="expireMin">过期时间，单位：分钟</param>
        public static void SignIn(string loginName, object userData, int expireMin)
        {
            //var data = JsonConvert.SerializeObject(userData, Formatting.Indented, //缩进  
            //    new JsonSerializerSettings { }
            //    );
            var data = JsonConvert.SerializeObject(userData);
            //创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。
            var ticket = new FormsAuthenticationTicket(2,loginName, DateTime.Now, DateTime.Now.AddDays(1), true, data);

            //加密Ticket，变成一个加密的字符串。
            var cookieValue = FormsAuthentication.Encrypt(ticket);

            //根据加密结果创建登录Cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
                {
                    HttpOnly = true,
                    Secure = FormsAuthentication.RequireSSL,
                    Domain = FormsAuthentication.CookieDomain,
                    Path = FormsAuthentication.FormsCookiePath
                };
            if (expireMin > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expireMin);

            var context = HttpContext.Current;
            if (context == null)
                throw new InvalidOperationException();

            //写登录Cookie
            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);
            bool islogin = HttpContext.Current.Request.IsAuthenticated;
        }

        /// <summary>
        /// 登出系统
        /// </summary>
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 系统登录者对象，静态实例
        /// </summary>
        //public static BaseLoginer LoginerInstance
        //{
        //    get
        //    {
        //        return GetBaseLoginerData();
        //    }
        //}

        /// <summary>
        /// 获取系统登录者对象
        /// </summary>
        /// <returns>返回BaseLoginer</returns>
        public static BaseLoginer GetBaseLoginerData()
        {
            BaseLoginer result = GetUserData<BaseLoginer>();
            return result;
        }

        /// <summary>
        /// 解析登录用户数据
        /// </summary>
        /// <typeparam name="T">T就是BaseLoginer</typeparam>
        /// <returns></returns>
        private static T GetUserData<T>() where T : class, new()
        {
            var userData = new T();
            try
            {
                var context = HttpContext.Current;
                var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                userData = JsonConvert.DeserializeObject<T>(ticket.UserData);
            }
            catch
            { }

            return userData;
        }

        /// <summary>
        /// 获取当前用户是否已经登录。
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.Request.IsAuthenticated;
            }
        }
    }
}
