/*************************************************************************
 * 文件名称 ：ParamInsert.cs                          
 * 描述说明 ：插入参数构建类
**************************************************************************/

using System.Collections.Generic;
using System.Dynamic;

namespace JTS.Core
{
    /// <summary>
    /// 插入参数构建类
    /// </summary>
    public class ParamInsert
    {
        protected ParamInsertData data;
 
        public dynamic this[string index]
        {
            get { return data.Columns[index]; }
            set { data.Columns[index] = value; }
        }

        /// <summary>
        /// 插入的表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>ParamInsert</returns>
        public ParamInsert Insert(string tableName)
        {
            data.TableName = tableName;
            return this;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="columnName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>ParamInsert</returns>
        public ParamInsert Column(string columnName, object value)
        {
            data.Columns[columnName] = value;
            return this;
        }

        public ParamInsert()
        {
            data = new ParamInsertData();
        }

        /// <summary>
        /// 实例对象
        /// </summary>
        /// <returns>ParamInsert</returns>
        public static ParamInsert Instance()
        {
            return new ParamInsert();
        }

        /// <summary>
        /// 获取Columns键值集合数据（IDictionary<string, object>）
        /// </summary>
        /// <returns></returns>
        public dynamic GetDynamicValue()
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            foreach (var c in this.data.Columns)
                expando.Add(c.Key, c.Value);

            return (ExpandoObject)expando;
        }

        /// <summary>
        /// 获取插入参数数据
        /// </summary>
        /// <returns></returns>
        public ParamInsertData GetData()
        {
            return data;
        }
    }

    /// <summary>
    /// 插入参数数据
    /// </summary>
    public class ParamInsertData
    {
        public string TableName { get; set; }
        public Dictionary<string, object> Columns { get; set; }

        public ParamInsertData()
        {
            TableName = "";
            Columns = new Dictionary<string, object>();
        }
    }
}
