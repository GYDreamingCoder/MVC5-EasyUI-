//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:24
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
    /// 实体类：用户角色关联表(Base_UserRole)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_UserRole", PrimaryField = "RoleCode", IdentityField = "", Order = "RoleCode", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_UserRole : BaseEntity
    {

        /// <summary>
        ///  用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  角色代码
        /// </summary>
        public string RoleCode { get; set; }

    }

    public partial class Base_UserRole
    {

    }
}
