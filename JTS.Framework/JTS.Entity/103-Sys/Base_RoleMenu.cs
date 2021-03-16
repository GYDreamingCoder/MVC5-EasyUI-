//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:16
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
    /// 实体类：角色菜单权限关联表(Base_RoleMenu)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_RoleMenu", PrimaryField = "MenuCode", IdentityField = "", Order = "MenuCode", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_RoleMenu : BaseEntity
    {

        /// <summary>
        ///  角色代码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        ///  菜单代码
        /// </summary>
        public string MenuCode { get; set; }

    }

    public partial class Base_RoleMenu
    {

    }
}
