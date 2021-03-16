//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:13
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
    /// 实体类：菜单按钮关联表(Base_MenuButton)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_MenuButton", PrimaryField = "ButtonCode", IdentityField = "", Order = "ButtonCode", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_MenuButton : BaseEntity
    {

        /// <summary>
        ///  菜单代码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        ///  按钮代码
        /// </summary>
        public string ButtonCode { get; set; }

    }

    public partial class Base_MenuButton
    {

    }
}
