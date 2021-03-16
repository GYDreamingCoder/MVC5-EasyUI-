using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTS.Core;
using JTS.Entity;

namespace JTS.Service
{
    public class SysOrgService : BaseService<SysOrg>
    {
        /// <summary>
        /// SysOrgService实例
        /// </summary>
        /// <returns>SysOrgService</returns>
        public static SysOrgService Instance()
        {
            return new SysOrgService();
        }
    }
    public class SysDJBHService : BaseService<SysDJBH>
    {
        /// <summary>
        /// SysDJBHService实例
        /// </summary>
        /// <returns>SysDJBHService</returns>
        public static SysDJBHService Instance()
        {
            return new SysDJBHService();
        }
    }
    public class SysLoginLogService : BaseService<SysLoginLog>
    {
        /// <summary>
        /// SysLoginLogService实例
        /// </summary>
        /// <returns>SysLoginLogService</returns>
        public static SysLoginLogService Instance()
        {
            return new SysLoginLogService();
        }
    }
    public class SysLogService : BaseService<SysLog>
    {
        /// <summary>
        /// SysLogService实例
        /// </summary>
        /// <returns>SysLogService</returns>
        public static SysLogService Instance()
        {
            return new SysLogService();
        }
    }
}
