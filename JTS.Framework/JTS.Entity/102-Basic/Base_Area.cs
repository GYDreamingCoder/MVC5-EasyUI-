//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:34:59
// </自动生成>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JTS.Core;

namespace JTS.Entity
{
    /// <summary>
    /// 实体类：地区表(Base_Area)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_Area", PrimaryField = "Id", IdentityField = "", Order = "Id", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_Area : BaseEntity
    {

        /// <summary>
        ///  地区编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///  地区名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///  父地区编号
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        ///  简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        ///  拼音
        /// </summary>
        public string Pinyin { get; set; }

        /// <summary>
        ///  级别
        /// </summary>
        public int? LevelType { get; set; }

        /// <summary>
        ///  城市代码
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        ///  邮编
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        ///  地区全称
        /// </summary>
        public string MergerName { get; set; }

        /// <summary>
        ///  经度
        /// </summary>
        public decimal? Lng { get; set; }

        /// <summary>
        ///  纬度
        /// </summary>
        public decimal? Lat { get; set; }

    }

    public partial class Base_Area
    {
        public string state { get;set;}
    }
}
