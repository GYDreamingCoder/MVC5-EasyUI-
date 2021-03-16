using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTS.Core;
using JTS.Entity;

namespace JTS.Service
{
    public class SysUserService : BaseService<SysUser>
    {
        /// <summary>
        /// SysUserService实例
        /// </summary>
        /// <returns>SysUserService</returns>
        public static SysUserService Instance()
        {
            return new SysUserService();
        }
        /// <summary>
        /// 系统登录
        /// </summary>
        /// <param name="loginid">登录id</param>
        /// <param name="password">加密密码（已加密）</param>
        /// <param name="ip">登录ip</param>
        /// <param name="city">登录所在城市</param>
        /// <returns>返回IResult</returns>
        public CommandResult Login(string loginid, string password, string ip, string city)
        {
            var parms = new
            {
                loginid = loginid,
                password = password,
                logintype ="S",
                ip = ip,
                city = city
            };
            return this.GetDataItem<CommandResult>("jo_sys_SysUser_Login", parms);
        }

        /// <summary>
        /// 系统登录
        /// </summary>
        /// <param name="loginid">登录id</param>
        /// <param name="password">加密后的密码</param>
        /// <param name="logintype">系统登录策略 S=系统内部登录 Q=QQ账号登录</param>
        /// <param name="ip">登录ip</param>
        /// <param name="city">登录所在城市</param>
        /// <param name="mac">登录mac</param>
        /// <returns>返回IResult</returns>
        public CommandResult Login(string loginid, string password, string logintype, string ip, string city)
        {
            var parms = new
            {
                loginid = loginid,
                password = password,
                logintype = logintype,
                ip = ip,
                city = city
            }; 
            return this.GetDataItem<CommandResult>("jo_sys_SysUser_Login", parms);
        }


        /// <summary>
        /// 获取用户角色数据。
        /// </summary>
        /// <param name="userid">用户id。</param>
        /// <returns>返回list。</returns>
        public List<SysRole> GetUserRoles(int userid)
        {
            return this.GetList_Fish<SysRole>("jo_sys_UserRoles_Select", new { userid = userid });
        }

        /// <summary>
        /// 保存用户角色数据。
        /// </summary>
        /// <param name="userid">用户id。</param>
        /// <param name="roleids">角色id拼接字符串，分隔符 ","。</param>
        /// <returns>CommandResult。</returns>
        public CommandResult SaveUserRoles(int userid, string roleids)
        {
            return this.GetDataItem<CommandResult>("jo_sys_UserRoles_Save", new { userid = userid, userroleids = roleids });
        }

        public bool DeleteUserRole(int userid, int roleid)
        {
            var sql = string.Format("delete from SysRoleUser where userid={0} and roleid={1}", userid, roleid);
            int result = this.ExecuteNonQuery(sql);
            return result > 0;
        }

        public bool AddUserRole(int userid, int roleid)
        {
            var sql = string.Format("select * from SysRoleUser where userid={0} and roleid={1}", userid, roleid);
            DataTable dt = this.GetDataTable_Fish(sql);
            if (dt.Rows.Count == 0)
            {
                sql = string.Format("insert into SysRoleUser(userid,roleid) values({0},{1})", userid, roleid);
                int result = this.ExecuteNonQuery(sql);
                return result > 0;
            }
            return true;
        }
        public bool UpdateUserPassword(int userid, string encryptPassword)
        {
            var sql = string.Format("update SysUser set [password]='{0}' where id={1}", encryptPassword, userid);
            int result = this.ExecuteNonQuery(sql);
            return result > 0;
        } 

    }
}
