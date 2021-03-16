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
    /// 常用类型扩展方法【string/int等】
    /// </summary>
    public static class ExtendMethod
    {
        /// <summary>
        /// 类型转换--位置ExtendMethod.cs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static T FromType<T, TK>(TK text)
        {
            try
            {
                return (T)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }


        /// <summary>
        /// string扩展方法：指示指定的字符串是 null 还是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="s">要测试的字符串</param>
        /// <returns>bool</returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        ///  string扩展方法：判断指定的字符串是否整型数字
        /// </summary>
        /// <param name="s">要测试的字符串</param>
        /// <returns>bool</returns>
        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }

        /// <summary>
        ///  string扩展方法：将数字的字符串表示形式转换为它的等效 32 位有符号整数。
        /// </summary>
        /// <param name="s"> 包含要转换的数字的字符串。</param>
        /// <returns>int</returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }


    }
}
