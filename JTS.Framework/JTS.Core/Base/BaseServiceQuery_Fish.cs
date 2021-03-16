/*************************************************************************
 * 文件名称 ：BaseServiceQueryClownFish.cs                          
 * 描述说明 ：定义数据服务基类中的查询处理（使用ClownFish数据访问组件，参数更灵活）
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using ClownFish;

namespace JTS.Core
{
    public partial class BaseService<T> where T : BaseEntity, new()
    {
        /// <summary>
        /// 是否存在记录，完整的where条件，必须以and开头-ClownFish
        /// </summary>
        /// <returns>返回bool</returns>
        public bool Exists_Fish(string condition)
        {
            var sql = string.Format("select * from dbo.{0} where 1=1 {1} ", typeof(T).Name, condition);
            object o = DbHelper.ExecuteScalar(sql, null, CommandKind.SqlTextNoParams);
            return o != null;
        }

        /// <summary>
        /// 执行一个ExecuteNonQuery调用-ClownFish
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>int</returns>
        public int ExecuteNonQuery_Fish(string sql)
        {
            return DbHelper.ExecuteNonQuery(sql, null, CommandKind.SqlTextNoParams);
        }

        /// <summary>
        /// 查询DataTable数据-ClownFish
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetDataTable_Fish(string sql)
        {
            return DbHelper.FillDataTable(sql, null, CommandKind.SqlTextNoParams);
        }

        /// <summary>
        ///  查询DataTable数据-ClownFish
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="inputParams">参数对象</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetDataTable_Fish(string spName, object inputParams)
        {
            return DbHelper.FillDataTable(spName, inputParams, CommandKind.StoreProcedure);
        }

        /// <summary>
        /// 获取实体列表-ClownFish
        /// </summary> 
        /// <param name="sqlOrCondition">如果是条件字符串，如果传入null，则查询所有记录。注意以'and'开头，如："and id=9"</param>
        /// <param name="flagCondition">是否条件查询</param>
        /// <returns>返回List实体列表</returns>
        public List<T> GetList_Fish(string sqlOrCondition, bool flagCondition)
        {
            if (flagCondition==false)
            {
                return DbHelper.FillList<T>(sqlOrCondition, null, CommandKind.SqlTextNoParams);
            }
            else
            {
                string condition = string.IsNullOrEmpty(sqlOrCondition) == true ? " 1=1 " : " 1=1 " + sqlOrCondition;
                string sql = string.Format("select * from {0} where {1}", typeof(T).Name, condition);
                return DbHelper.FillList<T>(sql, null, CommandKind.SqlTextNoParams);
            }
        }

        /// <summary>
        /// 获取实体列表-ClownFish
        /// </summary>
        /// <param name="sql"> sql语句</param>
        /// <returns>返回List实体列表</returns>
        public List<T> GetList_Fish(string sql)
        {
            return DbHelper.FillList<T>(sql, null, CommandKind.SqlTextNoParams);
        }

        /// <summary>
        /// 获取单个实体对象-ClownFish
        /// </summary>
        /// <param name="condition">条件字符串，注意以'and'开发，如："and id=9"</param>
        /// <returns>T</returns>
        public T GetEntity_Fish(string condition)
        {
            condition = string.IsNullOrEmpty(condition) == true ? " 1=1 " : " 1=1 " + condition;
            string sql = string.Format("select * from {0} where {1}", typeof(T).Name, condition);
            return DbHelper.GetDataItem<T>(sql, null, CommandKind.SqlTextNoParams);
        }



        /// <summary>
        /// 执行存储过程-ClownFish
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="inputParams">输入参数</param> 
        /// <returns>返回CommandResult对象</returns>
        public CommandResult SP_Fish(string spName, object inputParams)
        {
            return DbHelper.GetDataItem<CommandResult>(spName, inputParams, CommandKind.StoreProcedure);
        }

        /// <summary>
        /// 执行存储过程-ClownFish
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="inputParams">输入参数</param> 
        /// <returns>返回CommandResult对象</returns>
        public CommandResult SPCmdResult_Fish(string spName, object inputParams)
        {
            return DbHelper.GetDataItem<CommandResult>(spName, inputParams, CommandKind.StoreProcedure);
        }

        /// <summary>
        /// 执行存储过程-ClownFish
        /// <remarks>返回自定义对象（ClownFish）</remarks>
        /// </summary>
        /// <typeparam name="K">自定义对象</typeparam>
        /// <param name="spName">存储过程名称。</param>
        /// <param name="inputParams">包含所有命令参数对应的输入对象。如果没有参数，可为null</param>
        /// <returns>返回对象K</returns>
        public K GetDataItem<K>(string spName, object inputParams) where K : class, new()
        {
            K result = DbHelper.GetDataItem<K>(spName, inputParams, CommandKind.StoreProcedure);
            return result;
        }

        /// <summary>
        /// 执行存储过程-ClownFish
        /// <remarks>返回自定义对象列表（ClownFish）</remarks>
        /// </summary>
        /// <typeparam name="K">自定义对象</typeparam>
        /// <param name="spName">存储过程名称。</param>
        /// <param name="inputParams">包含所有命令参数对应的输入对象。如果没有参数，可为null</param>
        /// <returns>返回对象K</returns>
        public List<K> GetList_Fish<K>(string spName, object inputParams) where K : class, new()
        {
            List<K> result = DbHelper.FillList<K>(spName, inputParams, CommandKind.StoreProcedure);
            return result;
        }

        /// <summary>
        /// 分页获取数据列表。-ClownFish
        /// <para>使用公共分页的形式。参数使用分页对象。</para>
        /// <para>此方法默认使用存储过程（p_com_pagination）的分页方式。(默认查询所有数据。)</para>
        /// <para>说明：最后的三个参数一定要是用于分页的参数，且参数名为(前缀部分请自行添加)：in PageIndex int, in PageSize int, out TotalRecords int 注意：pageIndex从零开始计数。</para>
        /// </summary>
        /// <param name="tableName">表名或者视图名。</param>
        /// <param name="columns">需要查询的列名，逗号分开。</param>
        /// <param name="where">查询条件。如果没有条件可以为null。</param>
        /// <param name="order">排序，如：id asc。</param>
        /// <param name="pageIndex">分页序号，从0开始计数。从哪页开始。</param>
        /// <param name="pageSize">分页大小。每页显示的记录数。</param>
        /// <param name="pageCount">输出参数：分页数。</param>
        /// <param name="totalRecords">输出参数：总记录数。</param>
        /// <returns>返回List数据列表。</returns>
        public List<T> GetPageList_Fish(string tableName, string columns, string where, string order, int pageIndex, int pageSize, out int pageCount, out int total)
        {
            pageCount = 0;
            total = 0;
            PagingInfoExtension parameters = new PagingInfoExtension()
            {
                TableName = tableName,
                Columns = columns,
                Order = order,
                Where = where,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total
            };
            //pageCount = (int)Math.Ceiling((decimal)total / pageSize);
            List<T> list = DbHelper.FillListPaged<T>("p_com_pagination_clownfish", parameters, CommandKind.StoreProcedure);//这个需要提供存储过程名称。
            pageCount = parameters.CalcPageCount();
            total = parameters.TotalRecords;
            return list;
        }

        /// <summary>
        /// 分页获取数据列表2。-ClownFish
        /// <para>使用公共分页的形式。参数使用分页对象。</para>
        /// <para>此方法默认使用存储过程（p_com_pagination_clownfish）的分页方式。(默认查询所有数据。)</para>
        /// <para>说明：最后的三个参数一定要是用于分页的参数，且参数名为(前缀部分请自行添加)：in PageIndex int, in PageSize int, out TotalRecords int 注意：pageIndex从零开始计数。</para>
        /// </summary>
        /// <param name="tableName">表名或者视图名。</param>
        /// <param name="columns">需要查询的列名，逗号分开。</param>
        /// <param name="where">查询条件。如果没有条件可以为null。</param>
        /// <param name="order">排序，如：id asc。</param>
        /// <param name="pageIndex">分页序号，从0开始计数。从哪页开始。</param>
        /// <param name="pageSize">分页大小。每页显示的记录数。</param>
        /// <param name="pageCount">输出参数：分页数。</param>
        /// <param name="totalRecords">输出参数：总记录数。</param>
        /// <returns>返回List数据列表。</returns>
        public List<T> GetPageList_Fish(string where, string order, int pageIndex, int pageSize, out int pageCount, out int total)
        {
            pageCount = 0;
            total = 0;
            PagingInfoExtension parameters = new PagingInfoExtension()
            {
                TableName = typeof(T).Name,
                Columns = "*",
                Order = order,
                Where = where,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total
            };
            List<T> list = DbHelper.FillListPaged<T>("p_com_pagination_clownfish", parameters, CommandKind.StoreProcedure);//这个需要提供存储过程名称。
            pageCount = parameters.CalcPageCount();
            total = parameters.TotalRecords;
            return list;
        }

        /// <summary>
        /// 分页获取数据列表3。-ClownFish
        /// <para>使用公共分页的形式。参数使用分页对象。</para>
        /// <para>此方法默认使用存储过程（p_com_pagination_clownfish）的分页方式。(默认查询所有数据。)</para>
        /// <para>说明：最后的三个参数一定要是用于分页的参数，且参数名为(前缀部分请自行添加)：in PageIndex int, in PageSize int, out TotalRecords int 注意：pageIndex从零开始计数。</para>
        /// </summary>
        /// <param name="tableName">表名或者视图名。</param>
        /// <param name="columns">需要查询的列名，逗号分开。</param>
        /// <param name="where">查询条件。如果没有条件可以为null。</param>
        /// <param name="pageIndex">分页序号，从0开始计数。从哪页开始。</param>
        /// <param name="pageSize">分页大小。每页显示的记录数。</param>
        /// <param name="pageCount">输出参数：分页数。</param>
        /// <param name="totalRecords">输出参数：总记录数。</param>
        /// <returns>返回List数据列表。</returns>
        public List<T> GetPageList_Fish(string where, int pageIndex, int pageSize, out int pageCount, out int total)
        {
            pageCount = 0;
            total = 0;
            var attr = AttributeHelper.GetCustomAttribute<TableAttribute>(typeof(T));
            PagingInfoExtension parameters = new PagingInfoExtension()
            {
                TableName = attr.TableName,
                Columns = "*",
                Order = attr.Order,
                Where = where,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total
            };
            List<T> list = DbHelper.FillListPaged<T>("p_com_pagination_clownfish", parameters, CommandKind.StoreProcedure);//这个需要提供存储过程名称。
            pageCount = parameters.CalcPageCount();
            total = parameters.TotalRecords;
            return list;
        }
    }
}
