//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:18
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
    /// 实体类：角色菜单按钮权限表(Base_RoleMenuButton)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_RoleMenuButton", PrimaryField = "ButtonCode", IdentityField = "", Order = "ButtonCode", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_RoleMenuButton : BaseEntity
    {

        /// <summary>
        ///  角色代码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        ///  菜单代码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        ///  按钮代码
        /// </summary>
        public string ButtonCode { get; set; }

    }

    public partial class Base_RoleMenuButton
    {

    }
}
