//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:01
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
    /// 实体类：系统按钮(Base_Button)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_Button", PrimaryField = "ButtonCode", IdentityField = "", Order = "Sort", IgnoreInsertFields = "", IgnoreUpdateFields = "")]
    public partial class Base_Button : BaseEntity
    {

        /// <summary>
        ///  按钮代码
        /// </summary>
        public string ButtonCode { get; set; }

        /// <summary>
        ///  按钮名称
        /// </summary>
        public string ButtonName { get; set; }

        /// <summary>
        ///  按钮类型 0=未定义 1=工具栏按钮 2=右键按钮 3=其他(待定)
        /// </summary>
        public int ButtonType { get; set; }

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
        ///  js事件方法
        /// </summary>
        public string JsEvent { get; set; }

        /// <summary>
        ///  是否分隔符
        /// </summary>
        public int Split { get; set; }

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

    public partial class Base_Button
    {

    }
}
