using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Reflection;

namespace JTS.Utils
{
    /// <summary>
    /// 系统会话(Session)帮工具类。
    /// </summary>
    public class SessionUtil
    {
        /// <summary>
        /// 设置会话值。
        /// </summary>
        /// <typeparam name="T">指定类型。</typeparam>
        /// <param name="sessionName">会话名称。</param>
        /// <param name="value">值。</param>
        /// <param name="timeout">过期时间(单位：分钟)。默认是20分钟。</param>
        public static void Set<T>(string sessionName, T value, int timeout = 20)
        {
            HttpContext.Current.Session[sessionName] = value;
            HttpContext.Current.Session.Timeout = timeout;
        }
        /// <summary>
        /// 设置会话值。
        /// </summary>
        /// <typeparam name="T">指定类型。</typeparam>
        /// <param name="sessionName">会话名称。</param>
        /// <param name="value">值。(string[])</param>
        /// <param name="timeout">过期时间(单位：分钟)。默认是20分钟。</param>
        public static void Set(string sessionName, string[] value, int timeout = 20)
        {
            HttpContext.Current.Session[sessionName] = value;
            HttpContext.Current.Session.Timeout = timeout;
        }
        /// <summary>
        /// 获取会话值。
        /// </summary>
        /// <typeparam name="T">指定类型。</typeparam>
        /// <param name="sessionName">会话名。</param>
        /// <returns>T</returns>
        public static T Get<T>(string sessionName)
        {
            T retVal = default(T);
            if (HttpContext.Current.Session[sessionName] != null)
            {
                return (T)HttpContext.Current.Session[sessionName];
            }
            return retVal;
        }

        /// <summary>
        /// 获取会话值。
        /// </summary>
        /// <typeparam name="T">指定类型。</typeparam>
        /// <param name="sessionName">会话名。</param>
        /// <returns>string[] </returns>
        public static string[] Get(string sessionName)
        {
            return (string[])HttpContext.Current.Session[sessionName];
        }

        /// <summary>
        /// 移除会话。Session。
        /// </summary>
        /// <param name="sessionName">会话名。</param>
        public static void Remove(string sessionName)
        {
            HttpContext.Current.Session.Remove(sessionName);
        }
        /// <summary>
        /// 移除所有会话。Session。
        /// </summary>
        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }
        /// <summary>
        /// 检测是否存在此session
        /// </summary>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public static bool ExistsSession(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
            {
                return false;
            }
            return true;
        }

    }
}
