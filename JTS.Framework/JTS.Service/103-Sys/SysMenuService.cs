using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTS.Core;
using JTS.Entity;

namespace JTS.Service
{
    public class SysMenuService : BaseService<SysMenu>
    {
        /// <summary>
        /// SysMenuService实例
        /// </summary>
        /// <returns>SysMenuService</returns>
        public static SysMenuService Instance()
        {
            return new SysMenuService();
        }
        /// <summary>
        /// 保存菜单的权限按钮数据。
        /// </summary>
        /// <param name="menuid">菜单id。</param>
        /// <param name="functionids">按钮di字符串集合。</param>
        /// <returns>返回IResult</returns>
        public CommandResult SaveSysMenuFunctions(int menuid, string functionids)
        {
            var result = this.GetDataItem<CommandResult>("jo_sys_SysMenuFunctions_Save", new { menuid = menuid, functionids = functionids });
            return result;
        }

        /// <summary>
        /// 获取当前用户的菜单。
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns>返回list</returns>
        public List<SysMenu> GetAvailableMenus(int userid)
        {
            var result = this.GetList_Fish<SysMenu>("jo_sys_SysMenu_SelectByUserID", new { userid = userid });
            return result;
        }
    }
}
