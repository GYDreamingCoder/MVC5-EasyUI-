//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:14
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
    /// 实体类：系统角色(Base_Role)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_Role", PrimaryField = "RoleCode", IdentityField = "", Order = "Sort", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_Role : BaseEntity
    {

        /// <summary>
        ///  角色代码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        ///  角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        ///  角色类型：0=未定义 1=系统角色 2=业务角色 3=其他 （系统角色不允许删除和编辑）
        /// </summary>
        public int RoleType { get; set; }

        /// <summary>
        ///  排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        ///  启用状态 1=是 0=否
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

    public partial class Base_Role
    {

    }
}
