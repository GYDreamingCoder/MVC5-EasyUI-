//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:04
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
    /// 实体类：公司表(Base_Company)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_Company", PrimaryField = "CompanyCode", IdentityField = "", Order = "Sort", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_Company : BaseEntity
    {
        /// <summary>
        ///  公司代码
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        ///  父公司代码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        ///  公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        ///  公司简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        ///  公司分类.0=未定义 1=集团 2=公司 3=分公司 4=子公司
        /// </summary>
        public int CompanyType { get; set; }

        /// <summary>
        ///  法人
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        ///  电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        ///  手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///  传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        ///  邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///  邮编
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        ///  网址
        /// </summary>
        public string WebSite { get; set; }

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

    public partial class Base_Company
    {

    }
}
