///*************************************************************************
// * 文件名称 ：Attribute.cs                          
// * 描述说明 ：定义属性
// **************************************************************************/

//using System;
//using System.Collections.Concurrent;

//namespace JTS.Core
//{
//    #region AttributeHelper
//    public class AttributeHelper
//    {
//        private static readonly ConcurrentDictionary<Type, string> _cachedModule = new ConcurrentDictionary<Type, string>();
//        private static readonly ConcurrentDictionary<Type, Attribute> _cachedPrimaryKeyAttribute = new ConcurrentDictionary<Type, Attribute>();

//        /// <summary>
//        /// 获取模块名称（自定义属性）
//        /// </summary>
//        /// <param name="type">Service Type</param>
//        /// <returns>string</returns>
//        public static string GetModuleName(Type type)
//        {
//            var description = _cachedModule.GetOrAdd(type, _GetModuleAttribute);
//            return description;
//        }

//        private static string _GetModuleAttribute(Type type)
//        {
//            var attrs = type.GetCustomAttributes(typeof(ModuleAttribute), false);
//            if (attrs.Length == 0)
//                return APP.Db_Default_ConnName;

//            var module = (ModuleAttribute)attrs[0];
//            return module.ModuleName;
//        }

//        /// <summary>
//        /// 获取实体属性的自定义特性Attribute
//        /// </summary>
//        /// <typeparam name="TAttribute"></typeparam>
//        /// <param name="type">实体对象Type</param>
//        /// <returns>TAttribute</returns>
//        public static TAttribute GetFieldAttribute<TAttribute>(Type type) where TAttribute : Attribute
//        {
//            var result = _cachedPrimaryKeyAttribute.GetOrAdd(type, delegate(Type t)
//            {
//                var attrs = type.GetCustomAttributes(typeof(TAttribute), false);
//                if (attrs.Length == 0)
//                    return null;

//                var att = (TAttribute)attrs[0];
//                return att;
//            });
//            return (TAttribute)result;
//        }

//        /// <summary>
//        /// 获取自定义主键特性（PrimaryKeyAttribute）
//        /// <remarks>没有的话返回null</remarks>
//        /// </summary>
//        /// <param name="type">实体Type</param>
//        /// <returns>PrimaryKeyAttribute</returns>
//        //public static PrimaryKeyAttribute GetPrimaryKeyAttribute(Type type)
//        //{
//        //    var result = _cachedPrimaryKeyAttribute.GetOrAdd(type, delegate(Type t)
//        //    {
//        //        var attrs = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
//        //        if (attrs.Length == 0)
//        //            return null;

//        //        var att = (PrimaryKeyAttribute)attrs[0];
//        //        return att;
//        //    });
//        //    return (PrimaryKeyAttribute)result;
//        //}

//    }

//    #endregion

//    #region Module 业务模块
//    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
//    public class ModuleAttribute : Attribute
//    {
//        public string ModuleName { get; set; }
//        public ModuleAttribute(string name)
//        {
//            ModuleName = name;
//        }
//    }
//    #endregion
//    #region DisplayFormatAttribute
//    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
//    public class DisplayFormatAttribute : Attribute
//    {
//        public bool ApplyFormatInEditMode { get; set; }
//        public bool ConvertEmptyStringToNull { get; set; }
//        public string DataFormatString { get; set; }
//        public string NullDisplayText { get; set; }
//        public DisplayFormatAttribute() { }
//        public DisplayFormatAttribute(string formartString)
//        {
//            DataFormatString = formartString;
//        }
//    }
//    #endregion
//    /* 
    
//    #region PrimaryKeyAttribute
//    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
//    public class PrimaryKeyAttribute : Attribute
//    {
//        public bool IsPrimaryKey { get; set; }
//        public PrimaryKeyAttribute()
//        {
//            IsPrimaryKey = true;
//        }

//        //public static PrimaryKeyAttribute Get(Type type)
//        //{
//        //    var attrs = type.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
//        //    if (attrs.Length == 0)
//        //        return null;
//        //    return (PrimaryKeyAttribute)attrs[0];
//        //}
//    }
//    #endregion
//    */
//    /*
//    #region IdentityAttribute
//    /// <summary>
//    /// 自定义特性 。
//    /// 作用：说明列是否为自动增长列 
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
//    public class IdentityAttribute : Attribute
//    {
//        public bool IsIdentity { get; set; }
//        public IdentityAttribute()
//        {
//            IsIdentity = true;
//        }
//    }
//    #endregion
//    */
//    /*
//    #region IgnoreAttribute
//    /// <summary>
//    /// 自定义特性 。
//    /// 作用：插入或更新操作时需要忽略更新的字段
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
//    public class IgnoreAttribute : Attribute
//    {
//        /// <summary>
//        /// 忽略插入
//        /// </summary>
//        public bool IgnoreInsert { get; set; }

//        /// <summary>
//        /// 忽略更新
//        /// </summary>
//        public bool IgnoreUpdate { get; set; }

//        /// <summary>
//        /// 自定义特性 。插入或更新操作时需要忽略更新的字段
//        /// </summary>
//        public IgnoreAttribute()
//        {
//            this.IgnoreInsert = true;
//            this.IgnoreUpdate = true;
//        }

//        /// <summary>
//        /// 自定义特性 。插入或更新操作时需要忽略更新的字段
//        /// </summary>
//        /// <param name="ignoreInsert">忽略插入</param>
//        /// <param name="ignoreUpdate">忽略更新</param>
//        public IgnoreAttribute(bool ignoreInsert, bool ignoreUpdate)
//        {
//            this.IgnoreInsert = ignoreInsert;
//            this.IgnoreUpdate = ignoreUpdate;
//        }
//    }
//    #endregion
//    */
    

//    #region   实体对象EntityMappingAttribute
//    /// <summary>
//    /// 自定义特性 
//    /// <remarks>属性/类/方法 可用  支持继承</remarks>
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method, Inherited = true)]
//    public class EntityMappingAttribute : Attribute
//    {
//        /// <summary>
//        /// 获取或设置表名特性
//        /// </summary>
//        public string TableName { get; set; }

//        /// <summary>
//        /// 获取或设置中午列名称特性
//        /// </summary>
//        public string ColumnName { get; set; }

//        /// <summary>
//        /// 获取或设置主键字段名称特性
//        /// </summary>
//        public string PrimaryName { get; set; }

//        /// <summary>
//        /// 获取或设置自增字段名称特性
//        /// </summary>
//        public string IdentityName { get; set; }

//        /// <summary>
//        /// 获取或设置排序字段名称特性
//        /// </summary>
//        public string OrderName { get; set; }


//        public EntityMappingAttribute()
//        {
//            this.TableName = "";
//            this.ColumnName = "";
//            this.PrimaryName = "";
//            this.IdentityName = "";
//            this.OrderName = this.PrimaryName;
//        }

//    }
//    #endregion
//}
