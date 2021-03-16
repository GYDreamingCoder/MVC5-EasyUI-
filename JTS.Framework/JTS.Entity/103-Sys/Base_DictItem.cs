//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:08
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
    /// 实体类：数据字典明细表(Base_DictItem)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_DictItem", PrimaryField = "DictItemCode", IdentityField = "", Order = "Sort", IgnoreInsertFields = "", IgnoreUpdateFields = "DictCode")]
    public partial class Base_DictItem : BaseEntity
    {

        /// <summary>
        ///  TS.ColumnName
        /// </summary>
        public string DictCode { get; set; }

        /// <summary>
        ///  TS.ColumnName
        /// </summary>
        public string DictItemCode { get; set; }

        /// <summary>
        ///  TS.ColumnName
        /// </summary>
        public string DictItemName { get; set; }

        /// <summary>
        ///  排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        ///  启用
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        ///  备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///  创建人
        /// </summary>
        public string AddBy { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? AddOn { get; set; }

        /// <summary>
        ///  编辑人
        /// </summary>
        public string EditBy { get; set; }

        /// <summary>
        ///  编辑时间
        /// </summary>
        public DateTime? EditOn { get; set; }

    }

    public partial class Base_DictItem
    {

    }
}
