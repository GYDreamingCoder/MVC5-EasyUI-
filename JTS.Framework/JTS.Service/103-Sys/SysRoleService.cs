using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTS.Core;
using JTS.Entity;

namespace JTS.Service
{
    public class SysRoleService : BaseService<SysRole>
    {
        /// <summary>
        /// SysRoleService实例
        /// </summary>
        /// <returns>SysRoleService</returns>
        public static SysRoleService Instance()
        {
            return new SysRoleService();
        }
        /// <summary>
        /// 获取当前角色所有的菜单权限、按钮权限数据。
        /// </summary>
        /// <param name="roleid">角色id。</param>
        /// <returns>返回List</returns>
        public List<SysRoleMenuFunctions> GetRoleMenuData(int roleid)
        {
            return this.GetList_Fish<SysRoleMenuFunctions>("jo_sys_SysRoleMenuFunctions_Select", new { roleid = roleid });
        }

        public List<SysFunction> GetRoleMenuFunctions(int roleid, int menuid)
        {
            return this.GetList_Fish<SysFunction>("jo_sys_SysFunction_GetRoleMenuFunctions", new { roleid = roleid, menuid = menuid });
        }

        public List<SysUser> GetRoleUsers(int userid, int roleid)
        {
            return this.GetList_Fish<SysUser>("jo_sys_SysRoleUsers_Select", new { roleid = roleid, userid = userid });
        }

        public CommandResult SaveRoleUsers(int userid, int roleid, string userids, int isadd)
        {
            var inpuntParams = new
              {
                  userid = userid,
                  roleid = roleid,
                  userids = userids,
                  isadd = isadd
              };
            return this.GetDataItem<CommandResult>("jo_sys_SysRoleUsers_Save", inpuntParams);
        }

        public List<SysFunction> GetMenuFunctions(int menuid)
        {
            return this.GetList_Fish<SysFunction>("jo_sys_SysMenuFunctions_Select", new { menuid = menuid, });
        }

        /// <summary>
        /// 保存当前角色的菜单-按钮权限数据。
        /// </summary>
        /// <param name="ismutil">是否批量。（勾选全部，默认选择所有的按钮。）</param>
        /// <param name="roleid">角色id。</param>
        /// <param name="menuid">菜单id。</param>
        /// <param name="functionid">按钮id。</param>
        /// <param name="isadd">是否新增。</param>
        /// <returns>返回IResult</returns>
        public CommandResult SaveRoleMenuFunctions(bool ismutil, int roleid, int menuid, int functionid, int isadd)
        {
            var spName = ismutil == true ? "jo_sys_SysRoleMenuFunctions_MutilSave" : "jo_sys_SysRoleMenuFunctions_Save";
            var inpuntParams = new
              {
                  roleid = roleid,
                  menuid = menuid,
                  functionid = functionid,
                  isadd = isadd
              };
            return this.GetDataItem<CommandResult>(spName, inpuntParams);
        }
    }
}
