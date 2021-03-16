using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JTS.Core;

namespace JTS.Entity
{
    /// <summary>
    /// 实体类：系统参数表(Base_DataBase)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_DataBase", PrimaryField = "XXXCode", IdentityField = "", Order = "Sort", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_DataBase : BaseEntity
    {
    }
    public partial class Base_SysParam
    {

    }


    public class sys_table
    {
        public string TableName { get; set; }
        public string TableDescription { get; set; }
    }

    public partial class sys_column
    {
        public string TableName { get; set; }
        public string TableDescription { get; set; }

        public Int16 ColumnOrder { get; set; }
        public string ColumnName { get; set; }
        public string FlagIdentity { get; set; }
        public string FlagPrimary { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public int Length { get; set; }
        public string DecimalNumber { get; set; }
        public string FlagNullable { get; set; }
        public string DefaultValue { get; set; }
        public string ColumnDescription { get; set; }
        public string ColumnCaption { get; set; }

    }
    public partial class sys_column
    {
        public bool IsIdentity
        {
            get
            {
                return this.FlagIdentity == "√";
            }
        }
        public bool IsPrimaryKey
        {
            get
            {
                return this.FlagPrimary == "√";
            }
        }
        public bool IsNullable
        {
            get
            {
                return this.FlagNullable == "√";
            }
        }
        public string TypeName
        {
            get
            {
                return GetTypeName(this.Type, this.IsNullable);
            }
        }

        /// <summary>
        /// 类型转换 从数据库类型转换为C#类型
        /// </summary>
        /// <param name="SqlTypeName">SqlTypeName</param>
        /// <param name="IsNullable">IsNullable</param>
        /// <returns>string</returns>
        public static string GetTypeName(string SqlTypeName, bool IsNullable)
        {
            string str = "";
            switch (SqlTypeName.ToLower())
            {
                case "char":
                case "nchar":
                case "ntext":
                case "text":
                case "nvarchar":
                case "varchar":
                case "xml": str = "string"; break;//String
                case "smallint": str = "short"; break;//Int16
                case "int": str = "int"; break;//Int32
                case "bigint": str = "long"; break;//Int64
                case "binary":
                case "image":
                case "varbinary":
                case "timestamp": str = "byte[]"; break;//Byte[]
                case "tinyint": str = "SByte"; break;//SByte
                case "bit": str = "bool"; break;//Boolean
                case "float": str = "double"; break;//Double
                case "real": str = "Guid"; break;//Single
                case "uniqueidentifier": str = "Guid"; break;//Guid
                case "sql_variant": str = "object"; break;//Object
                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney": str = "decimal"; break;//Decimal
                case "datetime":
                case "smalldatetime": str = "DateTime"; break;//DateTime
                default: str = SqlTypeName; break;
            }
            if (IsNullable)
            {
                switch (str)
                {
                    case "short":
                    case "int":
                    case "long":
                    case "SByte":
                    case "bool":
                    case "double":
                    case "Guid":
                    case "decimal":
                    case "DateTime":
                        str += "?";
                        break;
                    default:
                        break;
                }
            }
            return str;
        }
    }
}
