/*************************************************************************
 * 文件名称 ：BaseEntity.cs                          
 * 描述说明 ：定义实体基类
 * 
 **************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using JTS.Utils;

namespace JTS.Core
{
    /// <summary>
    /// 框架实体基类
    /// </summary>
    public class BaseEntity : ICloneable, IComparable, IDisposable
    {
        /// <summary>
        /// 定义属性缓存对象
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Dictionary<string, List<string>>> _cachedAtrributes = new ConcurrentDictionary<Type, Dictionary<string, List<string>>>();

        /// <summary>
        /// 获取实体某个自定义特性的所有的字段名称
        /// </summary>
        /// <typeparam name="TEntity">TEntity实体类型</typeparam>
        /// <typeparam name="TAttribute">TAttribute自定义特性类型</typeparam>
        /// <returns>list</returns>
        public static List<string> GetAttributeFields<TEntity, TAttribute>()
        {
            var key = typeof(TAttribute).Name;
            var thisAttributes = _cachedAtrributes.GetOrAdd(typeof(TEntity), BuildAtrributeDictionary);
            return thisAttributes.ContainsKey(key) ? thisAttributes[typeof(TAttribute).Name] : new List<string>();
        }

        private static Dictionary<string, List<string>> BuildAtrributeDictionary(Type TEntity)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var property in TEntity.GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
                if (attributes != null)
                    foreach (var key in attributes.Select(attr => attr.GetType().Name))
                    {
                        if (!result.ContainsKey(key))
                            result.Add(key, new List<string>());

                        result[key].Add(property.Name);
                    }
            }

            return result;
        }

        /// <summary>
        /// 扩展实体对象的属性值
        /// </summary>
        /// <param name="obj">实体对象，包含实体属性名和属性值</param>
        /// <returns>返回dynamic</returns>
        public dynamic Extend(object obj)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();

            EachHelper.EachObjectProperty(this, (i, name, value) =>
            {
                expando.Add(name, value);
            });

            EachHelper.EachObjectProperty(obj, (i, name, value) =>
            {
                expando[name] = value;
            });

            return expando;
        }

        /// <summary>
        /// 获取实体属性值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>object</returns>
        public object GetValue(string propertyName)
        {
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }
 
        /// <summary>
        /// 克隆类的新实例 [可重写.NET2.0重写成(Activator.CreateInstance(this.GetType());)]
        /// </summary>
        /// <returns>object</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 排序方法。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable.CompareTo(object obj)
        {
            return OnSort(obj);
        }

        /// <summary>
        /// 排序方法【具体实体类需要重写】
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>int</returns>
        public virtual int OnSort(object obj)
        {
            return 0;
        }

        //IDisposable
        public void Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// 获取或设置实体公共属性tempid
        /// </summary>
        public int tempid { get; set; }

    }
}
