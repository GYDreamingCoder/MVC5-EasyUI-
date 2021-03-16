/*************************************************************************
 * 文件名称 ：ParamDelete.cs                          
 * 描述说明 ：删除参数构建类
**************************************************************************/

using System;
using System.Collections.Generic;

namespace JTS.Core
{
    /// <summary>
    /// 删除参数构建类
    /// </summary>
    public class ParamDelete
    {
        protected ParamDeleteData data;

        public ParamDelete From(string sql)
        {
            data.From = sql;
            return this;
        }

        public ParamDelete AndWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {

            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "and", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }

        public ParamDelete OrWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {
            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "or", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }
  
        public ParamDelete()
        {
            data = new ParamDeleteData();
        }

        public static ParamDelete Instance()
        {
            return new ParamDelete();
        }

        public ParamDeleteData GetData()
        {
            return data;
        }
     }

    /// <summary>
    /// 删除参数数据
    /// </summary>
    public class ParamDeleteData
    {
        public string From { get; set; }
        public List<ParamWhere> Where { get; set; }
        public string WhereSql { get { return ParamUtils.GetWhereSql(Where); } }

        public object GetValue(string column)
        {
            var first = Where.Find(x => x.Data.Column == column);
            return first == null ? null : first.Data.Value;
        }

        public ParamDeleteData()
        {
            From = "";
            Where = new List<ParamWhere>();
        }
    }
}
