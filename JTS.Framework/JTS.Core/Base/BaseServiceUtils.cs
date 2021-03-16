/*************************************************************************
 * 文件名称 ：ServiceBaseUtils.cs                          
 * 描述说明 ：定义数据服务基类中的工具类
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using FluentData;
using JTS.Utils;


namespace JTS.Core
{
    /// <summary>
    /// 定义数据服务基类中的工具类
    /// </summary>
    /// <typeparam name="T">where T : BaseEntity, new()</typeparam>
    public partial class BaseService<T> where T : BaseEntity, new()
    {
        private static Dictionary<string, object> GetCommonFieldValueForAdd()
        {
            var user = FormsAuth.GetBaseLoginerData().UserName;
            var dict = new Dictionary<string, object>
                {
                    {APP.Field_AddBy, user},
                    {APP.Field_AddOn, DateTime.Now}
                };
            return dict;
        }

        private static Dictionary<string, object> GetCommonFieldValueForEdit()
        {
            var user = FormsAuth.GetBaseLoginerData().UserName;
            var dict = new Dictionary<string, object>
                {
                    {APP.Field_EditBy, user},
                    {APP.Field_EditOn, DateTime.Now}
                };
            return dict;
        }

        public static List<string> GetCommonFieldNameList()
        {
            var list = new List<string>();
            list.Add(APP.Field_AddBy);
            list.Add(APP.Field_AddOn);
            list.Add(APP.Field_EditBy);
            list.Add(APP.Field_EditOn);
            list.Add("tempid"); 
            return list;
        }

        /// <summary>
        /// 获取公共字段集合
        /// <remarks>公共字段：AddBy、AddOn、EditBy、EditOn、tempid</remarks>
        /// </summary>
        /// <returns>返回字典集合（字段名-默认值）</returns>
        public static Dictionary<string, object> GetCommonFieldName()
        {
            //var user = FormsAuth.GetLoginerData().UserName;
            var dict = new Dictionary<string, object>
                {
                    {APP.Field_AddBy, ""},
                    {APP.Field_AddOn, DateTime.Now},
                    {APP.Field_EditBy, ""},
                    {APP.Field_EditOn, DateTime.Now},
                    {"tempid", 0}
                };
            return dict;
        }

        protected ISelectBuilder<T> BuilderParse(ParamQuery param)
        {
            if (param == null)
            {
                param = new ParamQuery();
            }
            var data = param.GetData();
            var sFrom = data.From.Length == 0 ? typeof(T).Name : data.From;
            var sSelect = string.IsNullOrEmpty(data.Select) ? (sFrom + ".*") : data.Select;
            var selectBuilder = db.Select<T>(sSelect).From(sFrom)
                .Where(data.WhereSql)
                .GroupBy(data.GroupBy)
                .Having(data.Having)
                .OrderBy(data.OrderBy)
                .Paging(data.PageIndex, data.PageSize);
            string sql = selectBuilder.Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForSelectBuilder(selectBuilder.Data);
            return selectBuilder;
        }

        protected IInsertBuilder BuilderParse(ParamInsert param)
        {
            var data = param.GetData();
            var insertBuilder = db.Insert(data.TableName.Length == 0 ? typeof(T).Name : data.TableName);

            var dict = GetCommonFieldValueForAdd();
            //除开添加人和添加日期字段外，其他data中的所有字段都加入参数内
            foreach (var column in data.Columns.Where(column => !dict.ContainsKey(column.Key)))
                insertBuilder.Column(column.Key, column.Value);

            //如果实体对象中有添加人和添加日期字段，则将其加入参数内
            var properties = ReflectionUtil.GetProperties(typeof(T));
            foreach (var item in dict.Where(item => properties.ContainsKey(item.Key.ToLower())))
                insertBuilder.Column(item.Key, item.Value);

            return insertBuilder;
        }

        protected IUpdateBuilder BuilderParse(ParamUpdate param)
        {
            var data = param.GetData();
            var updateBuilder = db.Update(data.Update.Length == 0 ? typeof(T).Name : data.Update);
            //除开编辑人和编辑日期字段外，其他data中的所有字段都加入参数内
            var dict = GetCommonFieldValueForEdit();
            foreach (var column in data.Columns.Where(column => !dict.ContainsKey(column.Key)))
                updateBuilder.Column(column.Key, column.Value);

            //如果实体中有编辑人和编辑日期字段，则加入参数内
            var properties = JTS.Utils.ReflectionUtil.GetProperties(typeof(T));
            foreach (var item in dict.Where(item => properties.ContainsKey(item.Key.ToLower())))
                updateBuilder.Column(item.Key, item.Value);

            data.Where.ForEach(item => updateBuilder.Where(item.Data.Column, item.Data.Value));
            //foreach (var item in data.Where)
            //{
            //    updateBuilder.Where(item.Data.Column, item.Data.Value);
            //}  
            return updateBuilder;
        }

        protected IDeleteBuilder BuilderParse(ParamDelete param)
        {
            var data = param.GetData();
            var deleteBuilder = db.Delete(data.From.Length == 0 ? typeof(T).Name : data.From);
            data.Where.ForEach(item => deleteBuilder.Where(item.Data.Column, item.Data.Value));
            return deleteBuilder;
        }

        protected IStoredProcedureBuilder BuilderParse(ParamSP param)
        {
            var data = param.GetData();
            var spBuilder = db.StoredProcedure(data.Name);
            foreach (var item in data.Parameter)
                spBuilder.Parameter(item.Key, item.Value);

            foreach (var item in data.ParameterOut)
                spBuilder.ParameterOut(item.Key, item.Value);

            return spBuilder;
        }

        /// <summary>
        /// 查询记录数（分页查询使用）
        /// </summary>
        /// <param name="param">ParamQuery</param>
        /// <param name="rows">dynamic rows</param>
        /// <returns>int</returns>
        protected int QueryRowCount(ParamQuery param, dynamic rows)
        {
            if (rows != null)
                if (null == param || param.GetData().PageSize == 0)
                    return rows.Count;

            var RowCountParam = param.Paging(1, 0).OrderBy(string.Empty);
            var sql = BuilderParse(RowCountParam).Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForSelectBuilder(BuilderParse(RowCountParam).Data);
            return db.Sql(@"select count(*) from ( " + sql + " ) tb_temp").QuerySingle<int>();
        }
    }
}
