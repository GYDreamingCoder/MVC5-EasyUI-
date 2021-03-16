//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:53
// </自动生成>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTS.Core;
using JTS.Entity;
using JTS.Utils;

namespace JTS.Service
{
    /// <summary>
    /// 【数据库管理】服务
    /// </summary>
    public class Base_DataBaseService : BaseService<Base_DataBase>
    {

        /// <summary>
        /// 【数据库管理】服务实例【单例模式】
        /// </summary>
        private static readonly Base_DataBaseService _Instance = new Base_DataBaseService();

        /// <summary>
        /// 获取【数据库管理】服务实例【单例模式】
        /// </summary>
        public static Base_DataBaseService Instance
        {
            get
            {
                return _Instance;
            }
        }
        /// <summary>
        /// 查询当前数据库的所有用数据表列表
        /// </summary>
        /// <returns>List<sys_table></returns>
        public List<sys_table> GetTableList()
        {
            List<sys_table> list = this.db.Sql("select * from sys_table order by TableName asc").QueryMany<sys_table>();
            return list;
        }
        /// <summary>
        /// 根据表名获取表的列信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns> List<sys_column></returns>
        public List<sys_column> GetTableColumnList(string tableName)
        {
            List<sys_column> list = this.db.Sql("select * from sys_column where TableName='" + tableName + "'  order by ColumnOrder asc").QueryMany<sys_column>();
            return list;
        }

        public  CommandResult SaveTableDescription(string tableName, string tableDescription)
        {
            var commandResult = this.db.StoredProcedure("sys_addtabledescription")
                    .Parameter("TableName", tableName)
                    .Parameter("TableDescription", tableDescription).QuerySingle<CommandResult>();
            return commandResult;
        }

        public  CommandResult SaveTableColumnDescription(string tableName, string columnName, string columnDescription)
        {
            var commandResult = this.db.StoredProcedure("sys_addtableculumndescription")
                    .Parameter("TableName", tableName)
                    .Parameter("ColumnName", columnName)
                    .Parameter("ColumnDescription", columnDescription).QuerySingle<CommandResult>();
            return commandResult;
        }

        public  CommandResult SaveTableColumnCaption(string tableName, string columnName, string columnCaption)
        {
            var commandResult = this.db.StoredProcedure("sys_addtableculumncaption")
                    .Parameter("TableName", tableName)
                    .Parameter("ColumnName", columnName)
                    .Parameter("ColumnCaption", columnCaption).QuerySingle<CommandResult>();
            return commandResult;
        }
    }
}
