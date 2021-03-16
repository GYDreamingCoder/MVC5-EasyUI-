using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// NameValueCollection_Extentions扩展方法 
    /// </summary>
    public static class NameValueCollection_Extentions 
    {
        /// <summary>
        /// NameValueCollection扩展：将NameValueCollection对象转换成IDictionary字典集合
        /// </summary>
        /// <param name="source">NameValueCollection对象</param>
        /// <returns>IDictionary<string, string> </returns>
        public static IDictionary<string, string> ToDictionary(this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary(k => k, k => source[k]);
        }

        /// <summary>
        /// NameValueCollection扩展：将NameValueCollection对象转换成IDictionary字典集合
        /// </summary>
        /// <param name="source">NameValueCollection对象</param>
        /// <returns>IDictionary<string, string[]></returns>
        public static IDictionary<string, string[]> ToDictionaryArray(this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary(k => k, k => source.GetValues(k));
        }
    }
}
