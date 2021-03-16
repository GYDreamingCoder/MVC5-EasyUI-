//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:48
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
    /// 【系统菜角色】服务实例
    /// </summary>
    public class Base_RoleService : BaseService<Base_Role>
    {
        /// <summary>
        /// 【系统菜角色】服务实例【单例模式】
        /// </summary>
        private static readonly Base_RoleService _Instance = new Base_RoleService();

        /// <summary>
        /// 获取【系统菜单】服务实例【单例模式】
        /// </summary>
        public static Base_RoleService Instance
        {
            get
            {
                return _Instance;
            }
        }

        /// <summary>
        /// 根据角色代码查询角色菜单权限数据
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>dynamic</returns>
        public dynamic GetRoleMenus(string roleCode)
        {
            string sql = "select * from [dbo].[Base_RoleMenu] where RoleCode='" + roleCode + "'";
            return this.GetDynamicList(sql);
        }

        /// <summary>
        /// 根据角色代码查询角色菜单按钮权限数据
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>dynamic</returns>
        public dynamic GetRoleMenuButtons(string roleCode)
        {
            string sql = "select * from [dbo].[V_Base_RoleMenuButton] where RoleCode='" + roleCode + "'";
            return this.GetDynamicList(sql);
        }

        /// <summary>
        /// 根据角色代码查询角色的用户数据
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>dynamic</returns>
        public dynamic GetRoleUsers(string roleCode)
        {
            string sql = "select a.RoleCode,a.UserId,b.RealName,b.UserCode,b.UserType,b.Sort,b.Enabled,b.Remark from [dbo].[Base_UserRole] a left join [dbo].[Base_User] b on a.[UserId]=b.[UserId] where a.RoleCode='" + roleCode + "'";
            return this.GetDynamicList(sql);
        }

        /// <summary>
        /// 添加角色用户数据
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <param name="userId">用户编号</param>
        /// <returns>CommandResult</returns>
        public CommandResult AddRoleUser(string roleCode, int userId)
        {
            CommandResult cmdResult = CommandResult.Set(true, "保存成功");
            bool exists_UserRole = Base_UserRoleService.Instance.Exists_Fish(" and RoleCode='" + roleCode + "'  and UserId=" + userId + " ");
            if (exists_UserRole == false)
            {
                string sql = "insert into [dbo].[Base_UserRole](UserId,RoleCode) values(" + userId + ",'" + roleCode + "')";
                int intResult = this.ExecuteNonQuery_Fish(sql);
                if (intResult == 0)
                {
                    cmdResult = CommandResult.Set(true, "插入角色用户失败");
                }
            }
            return cmdResult;
        }

        /// <summary>
        /// 移除角色用户记录
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <param name="userId">用户编号</param>
        /// <returns>CommandResult</returns>
        public CommandResult RemoveRoleUser(string roleCode, int userId)
        {
            CommandResult cmdResult = CommandResult.Set(true, "操作成功");
            string sql = "delete from [dbo].[Base_UserRole] where RoleCode='" + roleCode + "' and UserId=" + userId;
            int intResult = this.ExecuteNonQuery_Fish(sql);
            if (intResult == 0)
            {
                cmdResult = CommandResult.Set(true, "移除角色用户失败");
            }
            return cmdResult;
        }


        /// <summary>
        /// 保存角色的菜单权限数据
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <param name="menuCode">菜单代码</param>
        /// <param name="flagAdd">是否新增</param>
        /// <returns>CommandResult</returns>
        public CommandResult SaveRoleMenus(string roleCode, string menuCode, bool flagAdd)
        {
            CommandResult cmdResult = CommandResult.Set(true, "保存成功");
            bool exists = Base_RoleMenuService.Instance.Exists_Fish(" and RoleCode='" + roleCode + "'  and MenuCode='" + menuCode + "'");
            if (flagAdd)
            {
                if (exists == false)
                {
                    string sql = string.Format("INSERT INTO [dbo].[Base_RoleMenu]([RoleCode],[MenuCode])VALUES('{0}','{1}')", roleCode, menuCode);
                    int intResult = this.ExecuteNonQuery_Fish(sql);
                    if (intResult == 0)
                    {
                        cmdResult = CommandResult.Set(true, "保存失败");
                    }
                }
            }
            else
            {
                if (exists == true)
                {
                    string sql = string.Format("DELETE FROM [dbo].[Base_RoleMenu] WHERE RoleCode='{0}' AND MenuCode='{1}'", roleCode, menuCode);
                    int intResult = this.ExecuteNonQuery_Fish(sql);
                    if (intResult == 0)
                    {
                        cmdResult = CommandResult.Set(true, "保存失败");
                    }
                }
            }
            return cmdResult;
        }
        /// <summary>
        /// 保存单个按钮权限
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <param name="menuCode">菜单代码</param>
        /// <param name="flagAdd">是否新增</param>
        /// <returns>CommandResult</returns>
        public CommandResult SaveRoleMenuButton(bool flagAdd, string roleCode, string menuCode, string buttonCode)
        {
            CommandResult cmdResult = CommandResult.Set(true, "保存成功");
            string w = string.Format(" and RoleCode='{0}' and MenuCode='{1}' and ButtonCode='{2}' ", roleCode, menuCode, buttonCode);
            bool exists = Base_RoleMenuButtonService.Instance.Exists_Fish(w);
            if (flagAdd)
            {
                if (exists == false)
                {
                    string sql = string.Format("INSERT INTO [dbo].[Base_RoleMenuButton]([RoleCode],[MenuCode],[ButtonCode])VALUES('{0}','{1}','{2}')", roleCode, menuCode, buttonCode);
                    int intResult = this.ExecuteNonQuery_Fish(sql);
                    if (intResult == 0)
                    {
                        cmdResult = CommandResult.Set(true, "保存失败");
                    }
                }
            }
            else
            {
                if (exists == true)
                {
                    string sql = string.Format("DELETE FROM [dbo].[Base_RoleMenuButton] WHERE RoleCode='{0}' AND MenuCode='{1}' AND ButtonCode='{2}'", roleCode, menuCode, buttonCode);
                    int intResult = this.ExecuteNonQuery_Fish(sql);
                    if (intResult == 0)
                    {
                        cmdResult = CommandResult.Set(true, "保存失败");
                    }
                }
            }
            return cmdResult;
        }
        /// <summary>
        /// 保存单个按钮权限-全选和反选操作
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <param name="menuCode">菜单代码</param>
        /// <param name="flagAdd">是否新增</param>
        /// <returns>CommandResult</returns>
        public CommandResult SaveRoleMenuButton_All(bool flagAdd, string roleCode, string menuCode)
        {
            CommandResult cmdResult = CommandResult.Set(true, "保存成功");
            string sql = string.Format("DELETE FROM [dbo].[Base_RoleMenuButton] WHERE RoleCode='{0}' AND MenuCode='{1}' ", roleCode, menuCode);
            this.ExecuteNonQuery_Fish(sql);
            if (flagAdd)
            {
                sql = "INSERT INTO [dbo].[Base_RoleMenuButton]([RoleCode],[MenuCode],[ButtonCode]) ";
                sql += string.Format(" SELECT '{0}',[MenuCode], [ButtonCode] FROM [dbo].[Base_MenuButton] WHERE [MenuCode]='{1}'", roleCode, menuCode);
                int intResult = this.ExecuteNonQuery_Fish(sql);
                if (intResult == 0)
                {
                    cmdResult = CommandResult.Set(true, "保存失败");
                }
            }
            return cmdResult;
        }
    }
}
