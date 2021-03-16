//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:52
// </自动生成>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTS.Core;
using JTS.Entity;
using JTS.Utils;

namespace JTS.Service
{
    /// <summary>
    /// 系统日志】服务实例
    /// </summary>
    public class Base_SysLogService : BaseService<Base_SysLog>
    {

        /// <summary>
        /// 【系统日志】服务实例【单例模式】
        /// </summary>
        private static readonly Base_SysLogService _Instance = new Base_SysLogService();

        /// <summary>
        /// 获取【系统日志】服务实例【单例模式】
        /// </summary>
        public static Base_SysLogService Instance
        {
            get
            {
                return _Instance;
            }
        }

        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <param name="ip">用户IP地址</param>
        /// <param name="city">用户IP所在城市</param>
        public void AddLoginLog(string userinfo,string ip, string city)
        {
            Base_SysLog log = new Base_SysLog()
            {
                LogType = 1,
                LogObject = "Base_User",
                LogAction = "用户登录",
                LogIP = ip,
                LogIPCity = city,
                LogDesc = userinfo
            };
            this.Insert(log);
        }

        /// <summary>
        /// 添加登出日志
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <param name="ip">用户IP地址</param>
        /// <param name="city">用户IP所在城市</param>
        public void AddLogoutLog(string userinfo, string ip, string city)
        {
            Base_SysLog log = new Base_SysLog()
            {
                LogType = 1,
                LogObject = "Base_User",
                LogAction = "用户登出",
                LogIP = ip,
                LogIPCity = city,
                LogDesc = userinfo
            };
            this.Insert(log);
        }
    }
}
