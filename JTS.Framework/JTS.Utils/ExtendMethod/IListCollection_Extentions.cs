using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// IListCollection_Extentions扩展方法 
    /// </summary>
    public static class IListCollection_Extentions 
    {
        /// <summary>
        /// IList扩展：将NameValueCollection对象转换成 EasyUI DataGrid数据格式
        /// </summary>
        /// <param name="source">NameValueCollection对象</param>
        /// <returns>IDictionary<string, string> </returns>
        //public static dynamic ToEasyUIData_DataGrid(this IList list)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.rows = list;
        //    result.total = list.Count;
        //    return result;
        //} 
    }
}
