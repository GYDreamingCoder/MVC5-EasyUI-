/*************************************************************************
 * 文件名称 ：ServiceBaseQuery.cs                          
 * 描述说明 ：定义数据服务基类中的查询处理
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace JTS.Core
{
    public partial class BaseService<T> where T : BaseEntity, new()
    {
        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回List实体列表</returns>
        public List<T> GetList(string sql)
        {
            var result = new List<T>();
            result = db.Sql(sql).QueryMany<T>();
            return result;
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <param name="flagSql">是否sql语句，false=条件字符串</param>
        /// <param name="sqlOrconfition">如果是条件字符串，如果传入null，则查询所有记录。注意以'and'开发，如："and id=9"</param>
        /// <returns>返回List实体列表</returns>
        public List<T> GetList(bool flagSql, string sqlOrconfition)
        {
            var result = new List<T>();
            if (flagSql)
            {
                return db.Sql(sqlOrconfition).QueryMany<T>();
            }
            else
            {
                string condition = string.IsNullOrEmpty(sqlOrconfition) == true ? " 1=1 " : " 1=1 " + sqlOrconfition;
                string sql = string.Format("select * from {0} where {1}", typeof(T).Name, condition);
                return db.Sql(sql).QueryMany<T>();
            }
        }

        /// <summary>
        /// 获取实体列表(支持分页)
        /// </summary>
        /// <param name="param">查询参数对象(支持分页)</param>
        /// <returns>返回List实体列表</returns>
        public List<T> GetList(ParamQuery param = null)
        {
            var result = new List<T>();
            result = BuilderParse(param).QueryMany();
            return result;
        }

        /// <summary>
        /// 获取动态列表
        /// </summary>
        /// <param name="param">查询参数对象</param>
        /// <returns>返回List<dynamic>动态列表</returns>
        public List<dynamic> GetDynamicList(ParamQuery param = null)
        {
            var result = new List<dynamic>();// db.Sql("").QueryMany<dynamic>();
            string sql = BuilderParse(param).Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForSelectBuilder(BuilderParse(param).Data);
            result = BuilderParse(param).Data.Command.Sql(sql).QueryMany<dynamic>();
            return result;
        }

        /// <summary>
        /// 获取动态列表
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>返回List<dynamic>动态列表</returns>
        public List<dynamic> GetDynamicList(string sql)
        {
            var result = new List<dynamic>();
            result = db.Sql(sql).QueryMany<dynamic>();
            return result;
        }

        ///// <summary>
        ///// 分页获取动态实体列表（dynamic属性：rows、total）
        ///// </summary>
        ///// <param name="param">查询参数对象</param>
        ///// <returns></returns>
        //public dynamic GetPageList(ParamQuery param = null)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.rows = this.GetList(param);
        //    result.total = this.QueryRowCount(param, result.rows);
        //    return result;
        //}

        /// <summary>
        /// 分页获取动态列表（dynamic属性：rows、total）
        /// </summary>
        /// <param name="param">ParamQuery查询参数对象</param>
        /// <returns>dynamic</returns>
        public dynamic GetDynamicPageList(ParamQuery param = null)
        {
            dynamic result = new ExpandoObject();
            result.rows = this.GetDynamicList(param);
            result.total = this.QueryRowCount(param, result.rows);
            return result;
        }

        /// <summary>
        /// 分页获取动态数据列表。
        /// </summary>
        /// <param name="param">ParamSP存储过程参数对象</param>
        /// <returns>dynamic</returns>
        public dynamic GetDynamicPageList(ParamSP param = null)
        {
            var result = BuilderParse(param).QueryMany<dynamic>();
            return result;
        }

        /// <summary>
        /// 分页获取数据列表。
        /// <para>使用公共分页的形式。参数使用分页对象。</para>
        /// <para>此方法默认使用存储过程（p_com_pagination）的分页方式。(默认查询所有数据。)</para>
        /// </summary>
        /// <param name="tableName">表名或者视图名。</param>
        /// <param name="columns">需要查询的列名，逗号分开。</param>
        /// <param name="where">查询条件。如果没有条件可以为null。</param>
        /// <param name="order">排序，如：id asc。</param>
        /// <param name="pageIndex">分页序号，从0开始计数。从哪页开始。</param>
        /// <param name="pageSize">分页大小。每页显示的记录数。</param>
        /// <param name="pageCount">输出参数：分页数。</param>
        /// <param name="total">输出参数：总记录数。</param>
        /// <returns>返回List数据列表。</returns>
        public dynamic GetDynamicPageList(string tableName, string columns, string where, string order, int pageIndex, int pageSize, out int pageCount, out int total)
        {
            pageCount = 0;
            total = 0;
            var store = db.StoredProcedure("com_pagination_fluent")
                    .Parameter("tableName", tableName)
                    .Parameter("columns", columns)
                    .Parameter("where", where)
                    .Parameter("order", order)
                    .Parameter("pageIndex", pageIndex)
                    .Parameter("pageSize", pageSize)
                    .ParameterOut("total", FluentData.DataTypes.Int32);

            //执行数据库
            var list = store.QueryMany<dynamic>();
            total = store.ParameterValue<int>("total");
            pageCount = (int)Math.Ceiling((decimal)total / pageSize);

            dynamic result = new ExpandoObject();
            result.rows = list;
            result.total = total;
            return result;
        }

        /// <summary>
        /// 分页获取数据列表。
        /// <para>使用公共分页的形式。参数使用分页对象。</para>
        /// <para>此方法默认使用存储过程（p_com_pagination）的分页方式。(默认查询所有数据。)</para>
        /// </summary>
        /// <param name="tableName">表名或者视图名。</param>
        /// <param name="columns">需要查询的列名，逗号分开。</param>
        /// <param name="where">查询条件。如果没有条件可以为null。</param>
        /// <param name="order">排序，如：id asc。</param>
        /// <param name="pageIndex">分页序号，从0开始计数。从哪页开始。</param>
        /// <param name="pageSize">分页大小。每页显示的记录数。</param>
        /// <param name="pageCount">输出参数：分页数。</param>
        /// <param name="total">输出参数：总记录数。</param>
        /// <returns>返回List数据列表。</returns>
        public List<T> GetPageList(string tableName, string columns, string where, string order, int pageIndex, int pageSize, out int pageCount, out int total)
        {
            pageCount = 0;
            total = 0;
            var store = db.StoredProcedure("com_pagination_fluent")
                    .Parameter("tableName", tableName)
                    .Parameter("columns", columns)
                    .Parameter("where", where)
                    .Parameter("order", order)
                    .Parameter("pageIndex", pageIndex)
                    .Parameter("pageSize", pageSize)
                    .ParameterOut("total", FluentData.DataTypes.Int32);

            //执行数据库
            var list = store.QueryMany<T>();
            total = store.ParameterValue<int>("total");
            pageCount = (int)Math.Ceiling((decimal)total / pageSize);
            return list;
        }

        /// <summary>
        /// 分页获取数据列表2。
        /// <para>使用公共分页的形式。参数使用分页对象。</para>
        /// <para>此方法默认使用存储过程（p_com_pagination）的分页方式。(默认查询所有数据。)</para>
        /// </summary>
        /// <param name="where">查询条件。如果没有条件可以为null。</param>
        /// <param name="order">排序，如：id asc。</param>
        /// <param name="pageIndex">分页序号，从0开始计数。从哪页开始。</param>
        /// <param name="pageSize">分页大小。每页显示的记录数。</param>
        /// <param name="pageCount">输出参数：分页数。</param>
        /// <param name="total">输出参数：总记录数。</param>
        /// <returns>返回List数据列表。</returns>
        public List<T> GetPageList(string where, string order, int pageIndex, int pageSize, out int pageCount, out int total)
        {
            pageCount = 0;
            total = 0;
            return this.GetPageList(typeof(T).Name, "*", where, order, pageIndex, pageSize, out pageCount, out total);
        }

        /// <summary>
        /// 分页获取数据列表2。
        /// <para>使用公共分页的形式。参数使用分页对象。</para>
        /// <para>此方法默认使用存储过程（p_com_pagination）的分页方式。(默认查询所有数据。)</para>
        /// </summary>
        /// <param name="where">查询条件。如果没有条件可以为null。</param>
        /// <param name="pageIndex">分页序号，从0开始计数。从哪页开始。</param>
        /// <param name="pageSize">分页大小。每页显示的记录数。</param>
        /// <param name="pageCount">输出参数：分页数。</param>
        /// <param name="total">输出参数：总记录数。</param>
        /// <returns>返回List数据列表。</returns>
        public List<T> GetPageList(string where, int pageIndex, int pageSize, out int pageCount, out int total)
        {
            pageCount = 0;
            total = 0;
            var columnAttribute = AttributeHelper.GetCustomAttribute<TableAttribute>(typeof(T));
            if (columnAttribute == null)
                throw new Exception("实体对象（" + typeof(T).Name + "）不支持TableAttribute特性");
            return this.GetPageList(typeof(T).Name, "*", where, columnAttribute.Order, pageIndex, pageSize, out pageCount, out total);
        }


        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="value">主键值</param>
        /// <returns>返回实体对象T</returns>
        public T GetEntity(object value)
        {
            var columnAttribute = AttributeHelper.GetCustomAttribute<TableAttribute>(typeof(T));
            var p = ParamQuery.Instance().From(columnAttribute.TableName).ClearWhere().AndWhere(columnAttribute.PrimaryField, value);
            return this.GetEntity(p);
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>返回实体对象T</returns>
        public T GetEntity(string sql)
        {
            var result = db.Sql(sql).QuerySingle<T>();
            return result;
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="param">查询参数对象</param>
        /// <returns>返回实体对象T</returns>
        public T GetEntity(ParamQuery param)
        {
            var result = new T();
            result = BuilderParse(param).QuerySingle();
            return result;
        }

        /// <summary>
        /// 获取单个动态对象
        /// </summary>
        /// <param name="param">查询参数对象</param>
        /// <returns>返回单个动态对象dynamic</returns>
        public dynamic GetDynamic(ParamQuery param)
        {
            var result = new ExpandoObject();
            string sql = BuilderParse(param).Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForSelectBuilder(BuilderParse(param).Data);
            result = BuilderParse(param).Data.Command.Sql(sql).QuerySingle<dynamic>();
            return result;
        }

        /// <summary>
        /// 获取单个动态对象
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>返回单个动态对象dynamic</returns>
        public dynamic GetDynamic(string sql)
        {
            var result = db.Sql(sql).QuerySingle<dynamic>();
            return result;
        }

        #region 执行存储过程StoredProcedure

        /// <summary>
        /// 执行存储过程，返回int
        /// </summary>
        /// <param name="param">ParamSP</param>
        /// <returns>int</returns>
        public int SP(ParamSP param)
        {
            var result = 0;
            result = BuilderParse(param).Execute();
            return result;
        }

        /// <summary>
        /// 执行存储过程，返回CommandResult对象
        /// </summary>
        /// <param name="param">ParamSP</param>
        /// <returns>CommandResult</returns>
        public CommandResult PCmdResult(ParamSP param)
        {
            CommandResult result = null;
            result = BuilderParse(param).QuerySingle<CommandResult>();
            return result;
        }

        #endregion



        ///// <summary>
        ///// 获取字段
        ///// </summary>
        ///// <typeparam name="TField"></typeparam>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public TField GetField<TField>(ParamQuery param)
        //{
        //    var result = default(TField);
        //    string sql = BuilderParse(param).Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForSelectBuilder(BuilderParse(param).Data);
        //    result = BuilderParse(param).Data.Command.Sql(sql).QuerySingle<TField>();
        //    return result;
        //}


    }
}
