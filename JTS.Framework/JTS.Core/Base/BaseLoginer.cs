/*************************************************************************
 * 文件名称 ：BaseLoginer.cs                          
 * 描述说明 ：定义系统登录者
 * 
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JTS.Core
{
    /// <summary>
    /// 系统登录者
    /// </summary>
    public class BaseLoginer
    {
        /// <summary>
        /// 获取或设置用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置用户登录账号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 获取或设置用户登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置用户姓名
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// 获取或设置是否超级管理员
        /// <remarks>设置时根据用户UserType判断。用户类型：0=未定义 1=超级管理员 2=普通用户 3=其他</remarks>
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 自定义数据（可以存储对象）
        /// </summary>
        public object Data { get; set; }

    }
}
