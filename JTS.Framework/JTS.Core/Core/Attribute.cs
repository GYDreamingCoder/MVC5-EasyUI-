/*************************************************************************
 * 文件名称 ：Attribute.cs                          
 * 描述说明 ：定义属性
 **************************************************************************/

using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JTS.Core
{
    #region AttributeHelper
    public class AttributeHelper
    {
        private static readonly ConcurrentDictionary<Type, string> _cachedModule = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, Attribute> _cachedPrimaryKeyAttribute = new ConcurrentDictionary<Type, Attribute>();

        /// <summary>
        /// 获取模块名称（自定义属性）
        /// </summary>
        /// <param name="type">Service Type</param>
        /// <returns>string</returns>
        public static string GetModuleName(Type type)
        {
            var description = _cachedModule.GetOrAdd(type, delegate(Type t)
            {
                var attrs = t.GetCustomAttributes(typeof(ModuleAttribute), false);
                if (attrs.Length == 0)
                    return APP.Db_Default_ConnName;

                var module = (ModuleAttribute)attrs[0];
                return module.ModuleName;
            });
            return description;
        }

        /// <summary>
        /// 获取实体属性的自定义特性Attribute
        /// </summary>
        /// <typeparam name="TAttribute">自定义特性Attribute对象</typeparam>
        /// <param name="type">实体对象Type</param>
        /// <returns>TAttribute</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            var result = _cachedPrimaryKeyAttribute.GetOrAdd(type, delegate(Type t)
            {
                var attrs = t.GetCustomAttributes(typeof(TAttribute), false);
                if (attrs.Length == 0)
                    return null;

                var att = (TAttribute)attrs[0];
                return att;
            });
            return (TAttribute)result;
        }

    }

    #endregion

    #region Module 业务模块
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleAttribute : Attribute
    {
        public string ModuleName { get; set; }
        public ModuleAttribute(string name)
        {
            ModuleName = name;
        }
    }
    #endregion

    #region   实体对象TableAttribute
    /// <summary>
    /// 自定义特性 
    /// <remarks>实体(表)对象 自定义特性 </remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 获取或设置表名特性
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 获取或设置主键字段名称特性
        /// </summary>
        public string PrimaryField { get; set; }

        /// <summary>
        /// 获取或设置自增字段名称特性
        /// </summary>
        public string IdentityField { get; set; }

        /// <summary>
        /// 获取或设置排序字段名称特性(可以带排序方式 如：asc/desc 默认asc)
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 获取或设置是否忽略插入字段,用逗号隔开
        /// </summary>
        public string IgnoreInsertFields { get; set; }

        /// <summary>
        /// 获取忽略插入字段列表
        /// </summary>
        public List<string> IgnoreInsertFieldsList
        {
            get
            {
                List<string> list = null;
                if (this.IgnoreInsertFields != "")
                {
                    list = this.IgnoreInsertFields.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                return list;
            }
        }

        /// <summary>
        /// 获取或设置是否忽略更新,用逗号隔开
        /// </summary>
        public string IgnoreUpdateFields { get; set; }

        /// <summary>
        /// 获取忽略更新字段列表
        /// </summary>
        public List<string> IgnoreUpdateFieldsList
        {
            get
            {
                List<string> list = null;
                if (this.IgnoreUpdateFields != "")
                {
                    list = this.IgnoreUpdateFields.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                return list;
            }
        }


        /// <summary>
        /// 【自定义特性】实体(表)对象 自定义特性
        /// </summary>
        public TableAttribute()
        {
            this.TableName = "";
            this.PrimaryField = "";
            this.IdentityField = "";
            this.Order = this.PrimaryField;
            this.IgnoreInsertFields = "";
            this.IgnoreUpdateFields = "";
        }

    }
    #endregion

}




/* 
#region   实体对象ColumnAttribute
/// <summary>
/// 自定义特性 
/// <remarks>列特性 </remarks>
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false)] 
public class ColumnAttribute_Old : Attribute
{
    /// <summary>
    /// 获取或设置中文列名称特性
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// 获取或设置是否为主键特性
    /// </summary>
    public bool IsPrimaryKey { get; set; }

    /// <summary>
    /// 获取或设置是否为自动增长列特性
    /// </summary>
    public bool IsIdentity { get; set; }

    /// <summary>
    /// 获取或设置是否忽略插入
    /// </summary>
    public bool IgnoreInsert { get; set; }

    /// <summary>
    /// 获取或设置是否忽略更新
    /// </summary>
    public bool IgnoreUpdate { get; set; }

    /// <summary>
    /// 【自定义特性】列特性
    /// </summary>
    public ColumnAttribute_Old()
    {
        this.IsPrimaryKey = false;
        this.IsIdentity = false;
        this.IgnoreInsert = false;
        this.IgnoreUpdate = false;
    }

}
#endregion
  */
