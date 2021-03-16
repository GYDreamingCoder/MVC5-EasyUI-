//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:06
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
    /// 实体类：部门表(Base_Department)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_Department", PrimaryField = "DepartmentCode", IdentityField = "", Order = "Sort", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_Department : BaseEntity
    {
        /// <summary>
        ///  部门编号
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        ///  父部门编号
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        ///  部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        ///  公司编号
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        ///  部门简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        ///  手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///  邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///  传真
        /// </summary>
        public string Fax { get; set; }

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

    public partial class Base_Department
    {

    }
}
