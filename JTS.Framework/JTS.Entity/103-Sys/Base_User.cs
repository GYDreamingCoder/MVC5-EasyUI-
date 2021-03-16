//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-08 17:32:33
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
    /// 实体类：系统用户表(Base_User)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_User", PrimaryField = "UserId", IdentityField = "UserId", Order = "Sort desc", IgnoreInsertFields = "IsAudit,AuditBy,AuditTime,IsOnline,LoginCount,LoginTime,LoginIP,LoginCity,LastChangePassword", IgnoreUpdateFields = "Password,IsAudit,AuditBy,AuditTime,IsOnline,LoginCount,LoginTime,LoginIP,LoginCity,LastChangePassword")]
    public partial class Base_User : BaseEntity
    {

        /// <summary>
        ///  用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  用户类型：0=未定义 1=管理员 3=普通用户 5=微信用户 7=ERP用户 9=OA用户  99=其他
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        ///  公司编号
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        ///  部门编号
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        ///  账号代码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        ///  登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///  密码秘钥
        /// </summary>
        public string Secretkey { get; set; }

        /// <summary>
        ///  真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        ///  姓名拼音
        /// </summary>
        public string Spell { get; set; }

        /// <summary>
        ///  性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        ///  手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///  邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///  QQ号码
        /// </summary>
        public string QQ { get; set; }

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
        ///  是否审核
        /// </summary>
        public int IsAudit { get; set; }

        /// <summary>
        ///  审核人
        /// </summary>
        public string AuditBy { get; set; }

        /// <summary>
        ///  审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        ///  是否超级管理员.1=是 0=否
        /// </summary>
        public int IsSuperAdmin { get; set; }

        /// <summary>
        ///  是否单点登录.1=是 0=否
        /// </summary>
        public int IsSingleLogin { get; set; }

        /// <summary>
        ///  是否在线.1=是 0=否
        /// </summary>
        public int IsOnline { get; set; }

        /// <summary>
        ///  登录次数
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        ///  最后登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        ///  最后登录IP地址
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        ///  最后登录城市
        /// </summary>
        public string LoginCity { get; set; }

        /// <summary>
        ///  最后修改密码时间
        /// </summary>
        public DateTime? LastChangePassword { get; set; }
        
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

    public partial class Base_User
    {

    }
}
