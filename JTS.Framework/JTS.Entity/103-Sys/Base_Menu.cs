//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:11
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
    /// 实体类：系统菜单(Base_Menu)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_Menu", PrimaryField = "MenuCode", IdentityField = "", Order = "Sort", IgnoreInsertFields = "icon,name", IgnoreUpdateFields = "icon,name")]
    public partial class Base_Menu : BaseEntity
    {

        /// <summary>
        ///  菜单代码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        ///  菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        ///  父级菜单代码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        ///  菜单类型：0=未定义 1=目录 2=页面
        /// </summary>
        public int MenuType { get; set; }

        /// <summary>
        ///  按钮模式：0=未定义 1=动态按钮 2=静态按钮 3=无按钮
        /// </summary>
        public int ButtonMode { get; set; }

        /// <summary>
        ///  菜单导航URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///  图标class
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        ///  图标Url
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        ///  排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        ///  是否启用
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

    public partial class Base_Menu
    {
        public string icon
        {
            get
            {
                string f = this.IconClass.Replace("icon-standard-", "").Replace("icon-hamburg-", "").Replace("icon-metro-", "");
                return "/Scripts/03jeasyui/icons/icon-standard/16x16/" +f+ ".png";
            }
        }
        public string name
        {
            get
            {
                return this.MenuName;
            }
        }
    }
}
