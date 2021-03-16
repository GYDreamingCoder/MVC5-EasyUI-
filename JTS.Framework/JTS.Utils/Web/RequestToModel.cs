using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JTS.Utils
{
    /// <summary>
    /// 描述：表单帮助类，将Request请求转换成实体模型对象
    /// </summary>
    public class RequestToModel
    {
        /// <summary>
        /// 提交表单通过反射获取单个像
        /// 注意：表单控件name必包含对应类中的第一个字段，否则将报错或使用加载重载
        /// </summary>
        public static T GetSingleForm<T>() where T : new()
        {
            T t = SetList<T>(null, 0).Single();
            return t;
        }

        /// <summary>
        /// 提交表单通过反射获取单个像
        /// 注意：表单控件name必包含对应类中的第一个字段，否则将报错或使用加载重载
        /// </summary>
        public static T GetSingleForm<T>(string appstr) where T : new()
        {
            T t = SetList<T>(appstr, 0).Single();
            return t;
        }

        /// <summary>
        /// 提交表单通过反射获取多个对像
        /// 注意：表单控件name必包含对应类中的第一个字段，否则将报错或使用加载重载
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> GetListByForm<T>() where T : new()
        {
            List<T> t = SetList<T>(null, 0);
            return t;
        }

        /// <summary>
        /// 提交表单通过反射获取多个对像
        /// 注意：表单控件name必包含对应类中的第一个字段，否则将报错或使用加载重载
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="appstr">控件前缀,比如 name="form1.sex" appstr可以设为form1</param>
        /// <returns></returns>
        public static List<T> GetListByForm<T>(string appstr) where T : new()
        {
            List<T> t = SetList<T>(appstr, 0);
            return t;
        }
        /// <summary>
        /// 提交表单通过反射获取多个对像
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="appstr">控件前缀,比如 name="form1.sex" appstr可以设为form1</param>
        /// <typeparam name="index">表单控件中第一个控件，对应类中字段在该类中的索引号,特殊情况可以是第二第三控件</typeparam>
        /// <returns></returns>
        private static List<T> GetListByForm<T>(string appstr, int index) where T : new()
        {
            List<T> t = SetList<T>(appstr, index);
            return t;
        }
        private static List<T> SetList<T>(string appendstr, int index) where T : new()
        {
            List<T> t = new List<T>();
            try
            {
                var properties = new T().GetType().GetProperties();
                var subNum = System.Web.HttpContext.Current.Request[appendstr + properties[index].Name].Split(',').Length;
                for (int i = 0; i < subNum; i++)
                {
                    var r = properties;
                    var model = new T();
                    foreach (var p in properties)
                    {
                        string pval = System.Web.HttpContext.Current.Request[appendstr + p.Name + ""];
                        if (!string.IsNullOrEmpty(pval))
                        {
                            pval = pval.Split(',')[i];
                            string pptypeName = p.PropertyType.Name;
                            p.SetValue(model, Convert.ChangeType(pval, p.PropertyType), null);
                        }
                    }
                    t.Add(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }
    }


}
