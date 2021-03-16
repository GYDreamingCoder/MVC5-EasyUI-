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
    /// Dictionary_Extentions扩展方法 
    /// </summary>
    public static class Dictionary_Extentions
    {
        /// <summary>
        /// Dictionary扩展方法： 通过key查找value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Value<T>(this Dictionary<string, string> dict, string key)
        {
            if (dict == null)
                throw new Exception("Dictionary is NULL.");
            string value = string.Empty;
            if (!dict.TryGetValue(key, out value)) return default(T);
            //throw new Exception("The given key:" + key + " was not present in the dictionary.");
            var v = ExtendMethod.FromType<T, string>(value);
            return v;
        }

        //public static string GetValue_String(this Dictionary<string, string> dict, string key)
        //{
        //    return dict[key].ToString();
        //}
        //public static int GetValue_Int(this Dictionary<string, string> dict, string key)
        //{
        //    return Convert.ToInt32(dict[key]);
        //}
        //public static decimal GetValue_Decimal(this Dictionary<string, string> dict, string key)
        //{
        //    return Convert.ToDecimal(dict[key]);
        //}
        //public static DateTime GetValue_DateTime(this Dictionary<string, string> dict, string key)
        //{
        //    return Convert.ToDateTime(dict[key]);
        //}
        //public static bool GetValue_Boolean(this Dictionary<string, string> dict, string key)
        //{
        //    return Convert.ToBoolean(dict[key]);
        //}

        /// <summary>
        /// Dictionary扩展方法： 列表同步方法封装-查找项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ab"></param>
        /// <param name="item"></param>
        public static V Value<K, V>(this Dictionary<K, V> dict, K key)
        {
            lock (dict)
            {
                if (dict.ContainsKey(key))
                {
                    return dict[key];
                }
                else
                {
                    return default(V);
                }
            }
        }

        /// <summary>
        /// Dictionary扩展方法： 列表同步方法封装-添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ab"></param>
        /// <param name="item"></param>
        public static void DictionaryAdd<K, V>(this Dictionary<K, V> dict, K id, V val)
        {
            if (val != null)
            {
                lock (dict)
                {
                    if (!dict.ContainsKey(id))
                    {
                        dict.Add(id, val);
                    }
                }
            }
        }

        /// <summary>
        ///  Dictionary扩展方法：列表同步方法封装 - 替换或添加项
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="ab"></param>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public static void DictionaryExchange<K, V>(this Dictionary<K, V> dict, K id, V val)
        {
            if (val != null)
            {
                lock (dict)
                {
                    if (dict.ContainsKey(id))
                    {
                        dict.Remove(id);
                    }
                    dict.Add(id, val);
                }
            }
        }

    }
}
