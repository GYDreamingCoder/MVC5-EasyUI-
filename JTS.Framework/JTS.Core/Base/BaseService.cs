/*************************************************************************
 * 文件名称 ：ServiceBase.cs                          
 * 描述说明 ：定义数据服务基类
 **************************************************************************/

using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using FluentData;
using Newtonsoft.Json.Linq;

namespace JTS.Core
{

    /// <summary>
    /// 框架服务层基类，适用于带有实体对象的业务访问
    /// <remarks>所有业务服务层都基层此类</remarks>
    /// </summary>
    /// <typeparam name="T">实体对象</typeparam>
    public partial class BaseService<T> where T : BaseEntity, new()
    {
        #region 变量
        private IDbContext _db;
        /// <summary>
        /// 当前数据操作中上下文对象。
        /// <remarks>（创建并且初始化的一个IDbContext，执行各种数据库操作）</remarks>
        /// </summary>
        protected IDbContext db
        {
            get
            {
                if (_db == null)
                    _db = Db.Context(this.ModuleName);

                return _db;
            }
        }

        /// <summary>
        /// 【属性】获取或设置模块名称
        /// <remarks>不同的模块有不同的数据库链接字符串，可以支持多数据库操作</remarks>
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 获取或设置消息
        /// </summary>
        public CommandResult CommandResult { get; set; }

        /// <summary>
        /// 获取当前实体TableAttribute自定义特性
        /// </summary>
        public TableAttribute CurrentTableAttribute
        {
            get
            {
                var tableAttribute = AttributeHelper.GetCustomAttribute<TableAttribute>(typeof(T));
                return tableAttribute;
            }
        }

        /// <summary>
        /// 获取或设置当前登录对象
        /// </summary>
        public BaseLoginer CurrentBaseLoginer
        {
            get
            {
                return FormsAuth.GetBaseLoginerData();
            }
        }

        #endregion

        /// <summary>
        /// 构造函数1
        /// </summary>
        public BaseService()
        {
            ModuleName = AttributeHelper.GetModuleName(this.GetType());
            CommandResult = new CommandResult();
        }

        /// <summary>
        /// 构造函数2
        /// </summary>
        /// <param name="moduleName">模块名</param>
        public BaseService(string moduleName)
        {
            ModuleName = moduleName;
            CommandResult = new CommandResult();
        }

        ~BaseService()
        {
            try
            {
                db.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// BaseService static实例
        /// </summary>
        /// <returns></returns>
        //public static BaseService<T> Instance()
        //{
        //    return new BaseService<T>();
        //}

    }

    /// <summary>
    /// 框架服务层基类，适用于没有实体对象的业务访问
    /// </summary>
    public class BaseService : BaseService<BaseEntity>
    {
        public BaseService(string moduleName)
            : base(moduleName)
        { }
    }

}
