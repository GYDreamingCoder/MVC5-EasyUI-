using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTS.Core;
using JTS.Entity;

namespace JTS.Service
{
    public class SysFunctionService : BaseService<SysFunction>
    {
        /// <summary>
        /// SysFunctionService实例
        /// </summary>
        /// <returns>SysFunctionService</returns>
        public static SysFunctionService Instance()
        {
            return new SysFunctionService();
        }

        /// <summary>
        /// 获取当前用户的页面按钮权限。
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="menuid">菜单id</param>
        /// <returns>返回List</returns>
        public List<SysFunction> GetAvailableMenuFunctionsByUserID(int userid, int menuid)
        {
            return this.GetList_Fish<SysFunction>("jo_sys_SysFunction_SelectByUserID", new { userid = userid, menuid = menuid });
        }

        /// <summary>
        /// 获取菜单的可用按钮。
        /// </summary>
        /// <param name="menuid">menuid</param>
        /// <returns>List</returns>
        public List<SysFunction> GetMenuFunctionsByMenuID(int menuid)
        {
            return this.GetList_Fish<SysFunction>("jo_sys_SysMenuFunctions_Select", new { menuid = menuid });
        }
    }
}
