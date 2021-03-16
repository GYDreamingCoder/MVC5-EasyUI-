/*************************************************************************
 * 文件名称 ：ParamQuery.cs                          
 * 描述说明 ：查询参数构建
 * 
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace JTS.Core
{
    public class ParamQuery
    {
        protected ParamQueryData data;

        public ParamQuery Select(string sql)
        {
            data.Select = sql;
            return this;
        }

        public ParamQuery From(string sql)
        {
            data.From = sql;
            return this;
        }
 
        //public ParamQuery AndWhere(string column, object value, Cp cp = Cp.equal, params object[] extend)
        //{
        //    data.Where.Add(new ParamWhere() { AndOr = "and", Column = column, Value = value, Compare = cp, Extend = extend });
        //    return this;
        //}

        public ParamQuery AndWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {
            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "and", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }

        //public ParamQuery OrWhere(string column, object value, Cp cp = Cp.equal, params object[] extend)
        //{
        //    data.Where.Add(new ParamWhere() { AndOr = "or", Column = column, Value = value, Compare = cp, Extend = extend });
        //    return this;
        //}

        public ParamQuery OrWhere(string column, object value, Func<WhereData, string> cp = null, params object[] extend)
        {
            data.Where.Add(new ParamWhere() { Data = new WhereData() { AndOr = "or", Column = column, Value = value, Extend = extend }, Compare = cp ?? Cp.Equal });
            return this;
        }

        public ParamQuery ClearWhere()
        {
            data.Where.Clear();
            return this;
        }
 
        public ParamQuery GroupBy(string sql)
        {
            data.GroupBy = sql;
            return this;
        }

        public ParamQuery OrderBy(string sql)
        {
            var sortOrder = sql.Trim().Split(' ');
            if (!string.IsNullOrWhiteSpace(sql))
            {
                string mainTable = null;
                data.Select.Trim().Split(',').ToList().ForEach(x => {
                    if (x.Trim().EndsWith("." + sortOrder[0])) sortOrder[0] = x;
                    if (x.Trim().EndsWith(".*")) mainTable = x.Split('.')[0];
                });

                if (mainTable !=null && mainTable.ToLower().StartsWith("distinct"))
                    mainTable = mainTable.Substring(8);

                if (sortOrder[0].IndexOf(".") == -1 && !string.IsNullOrEmpty(mainTable))
                    sortOrder[0] = mainTable + "." + sortOrder[0];
            }

            data.OrderBy = string.Join(" ", sortOrder);
            return this;
        }

        public ParamQuery Having(string sql)
        {
            data.Having = sql;
            return this;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public ParamQuery Paging(int pageIndex, int pageSize)
        {
            data.PageIndex = pageIndex;
            data.PageSize = pageSize;
            return this;
        }

        public ParamQuery()
        {
            data = new ParamQueryData();
        }

        public static ParamQuery Instance()
        {
            return new ParamQuery();
        }

        public ParamQueryData GetData()
        {
            return data;
        }

     }


    /// <summary>
    /// 查询参数数据
    /// </summary>
    public class ParamQueryData
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string Having { get; set; }
        public string GroupBy { get; set; }
        public string OrderBy { get; set; }
        public string From { get; set; }
        public string Select { get; set; }
        public List<ParamWhere> Where { get; set; }
        public string WhereSql { get { return ParamUtils.GetWhereSql(Where); } }

        public ParamQueryData()
        {
            Having = "";
            GroupBy = "";
            OrderBy = "";
            From = "";
            Select = "";
            Where = new List<ParamWhere>();
            PageIndex = 1;
            PageSize = 0;
        }
    }
}
