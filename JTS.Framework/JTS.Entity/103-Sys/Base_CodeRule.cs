//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:03
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
    /// 实体类：单据编码规则(Base_CodeRule)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_CodeRule", PrimaryField = "RuleCode", IdentityField = "", Order = "Sort", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_CodeRule : BaseEntity
    {

        /// <summary>
        ///  规则代码
        /// </summary>
        public string RuleCode { get; set; }

        /// <summary>
        ///  规则名称
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        ///  开始字符
        /// </summary>
        public string StartChars { get; set; }

        /// <summary>
        ///  格式 none=无格式 date=日期格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        ///  填充字符
        /// </summary>
        public string FillChar { get; set; }

        /// <summary>
        ///  排序
        /// </summary>
        public int Seed { get; set; }

        /// <summary>
        ///  排序
        /// </summary>
        public int Length { get; set; }

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

    public partial class Base_CodeRule
    {

    }
}
