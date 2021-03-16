//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:26
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
    /// 实体类：用户设置表(Base_UserSetting)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_UserSetting", PrimaryField = "Id", IdentityField = "Id", Order = "Id", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_UserSetting : BaseEntity
    {

        /// <summary>
        ///  自增编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  设置代码
        /// </summary>
        public string SettingCode { get; set; }

        /// <summary>
        ///  设置名称
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        ///  设置值
        /// </summary>
        public string SettingValue { get; set; }

        /// <summary>
        ///  描述
        /// </summary>
        public string Desc { get; set; }

    }

    public partial class Base_UserSetting
    {

    }
}
