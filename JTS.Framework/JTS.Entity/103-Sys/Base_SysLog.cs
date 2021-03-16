//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:35:19
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
    /// 实体类：系统日志表(Base_SysLog)
    /// </summary>
    [Serializable]
    [Table(TableName = "Base_SysLog", PrimaryField = "Id", IdentityField = "Id", Order = "LogTime DESC", IgnoreInsertFields = "LogTime", IgnoreUpdateFields = "LogTime")]
    public partial class Base_SysLog : BaseEntity
    {

        /// <summary>
        ///  自增编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  日志时间
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        ///  日志类型.0=未定义 1=登录日志 2=操作日志 3=其他
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        ///  日志对象
        /// </summary>
        public string LogObject { get; set; }

        /// <summary>
        ///  操作类型
        /// </summary>
        public string LogAction { get; set; }

        /// <summary>
        ///  IP地址
        /// </summary>
        public string LogIP { get; set; }

        /// <summary>
        ///  IP城市
        /// </summary>
        public string LogIPCity { get; set; }

        /// <summary>
        ///  描述
        /// </summary>
        public string LogDesc { get; set; }

    }

    public partial class Base_SysLog
    {

    }
}
