//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:45
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
    /// 【系统菜单】服务实例
    /// </summary>
    public class Base_MenuService : BaseService<Base_Menu>
    {
        /// <summary>
        /// 【系统菜单】服务实例【单例模式】
        /// </summary>
        private static readonly Base_MenuService _Instance = new Base_MenuService();

        /// <summary>
        /// 获取【系统菜单】服务实例【单例模式】
        /// </summary>
        public static Base_MenuService Instance
        {
            get
            {
                return _Instance;
            }
        }
        
        /// <summary>
        /// 查询当前登录用户的菜单权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isSuperAdmin"></param>
        /// <returns></returns>
        public List<Base_Menu> GetUserMenus(int userId, bool isSuperAdmin)
        {
            var list = new List<Base_Menu>();
            if (isSuperAdmin)
            {
                string sql = "select * from Base_Menu where Enabled=1 order by Sort";
                list = Base_MenuService.Instance.GetList_Fish(sql);
                return list;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Base_Menu where Enabled=1 and MenuCode in (");
            sb.Append("select MenuCode from Base_RoleMenu where RoleCode in");
            sb.Append("(select RoleCode from Base_UserRole where UserId=" + userId + ")");
            sb.Append(") order by Sort");
            list = Base_MenuService.Instance.GetList_Fish(sb.ToString());
            return list;
        }

        /// <summary>
        /// 获取用户的菜单权限按钮数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="menucode">菜单编号</param>
        /// <returns>dynamic</returns>
        public dynamic GetUserMenuButtons(int userId,string menucode)
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append(" select a.ButtonCode,(case  b.ButtonText when '' then a.ButtonName else b.ButtonText end) as ButtonName,b.ButtonSort as Sort,a.ButtonType,a.IconClass,a.JsEvent ");
            sb.Append(" from  Base_Button a  join Base_MenuButton b on a.ButtonCode=b.ButtonCode and b.MenuCode='" + menucode + "'");
            sb.Append(" where a.[Enabled]=1 and a.ButtonCode in (");
            sb.Append(" select DISTINCT ButtonCode from Base_RoleMenuButton where MenuCode='" + menucode + "' and RoleCode in(select RoleCode from Base_UserRole where UserId=" + userId + ") ) order by b.ButtonSort");
            var list = Base_MenuService.Instance.GetDynamicList(sb.ToString());
            return list;
        }

        /// <summary>
        /// 根据菜单代码查询其子菜单
        /// </summary>
        /// <param name="MenuCode">菜单代码</param>
        /// <returns>list</returns>
        public List<Base_Menu> GetChildrenMenus(string MenuCode)
        {
            string sql1 = "select MenuCode from [dbo].[fn_GetChildrenMenu]('" + MenuCode + "')";
            string sql = string.Format("select * from Base_Menu where MenuCode in({0}) order by Sort", sql1);
            var list = Base_MenuService.Instance.GetList_Fish(sql);
            return list;
        }

        /// <summary>
        /// 查询菜单的按钮数据-所有
        /// </summary>
        /// <param name="menuCode">菜单代码</param>
        /// <returns>dynamic</returns>
        public dynamic GetMenuButtons()
        {
            string sql = "select * from V_Base_MenuButton order by MenuCode asc,ButtonSort asc";
            return this.GetDynamicList(sql);
        }

        /// <summary>
        /// 检查菜单按钮映射表中是否存在指定按钮代码的记录
        /// </summary>
        /// <param name="buttonCode">按钮代码</param>
        /// <returns>返回bool，true=存在 false=不存在</returns>
        public bool ExistsMenuButton(string buttonCode)
        {
            string sql = "select * from [dbo].[Base_MenuButton] where [ButtonCode]='" + buttonCode + "'";
            DataTable dt = this.GetDataTable_Fish(sql);
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 强制删除菜单，因为需要处理多张关联表，所以使用事务处理。
        /// </summary>
        /// <param name="menuCode">菜单代码</param>
        /// <returns>CommandResult</returns>
        public CommandResult DeleteMenu(string menuCode)
        {
            CommandResult cmdResult = CommandResult.Set(true, "删除失败");
            db.UseTransaction(true);
            string sql1 = "delete from [dbo].[Base_RoleMenuButton] where MenuCode in (select MenuCode from [dbo].[fn_GetChildrenMenu]('" + menuCode + "'))";
            string sql2 = "delete from [dbo].[Base_RoleMenu] where MenuCode in (select MenuCode from [dbo].[fn_GetChildrenMenu]('" + menuCode + "'))";
            string sql3 = "delete from [dbo].[Base_MenuButton] where MenuCode in (select MenuCode from [dbo].[fn_GetChildrenMenu]('" + menuCode + "'))";
            string sql4 = "delete from [dbo].[Base_Menu] where MenuCode in (select MenuCode from [dbo].[fn_GetChildrenMenu]('" + menuCode + "'))";
            db.Sql(sql1).Execute();
            db.Sql(sql2).Execute();
            db.Sql(sql3).Execute();
            db.Sql(sql4).Execute();
            db.Commit();
            cmdResult = CommandResult.Set(true, "删除成功");
            return cmdResult;
        }

        /// <summary>
        /// 禁用菜单及其子菜单
        /// </summary>
        /// <param name="menuCode">菜单代码</param>
        /// <returns>CommandResult</returns>
        public CommandResult DisableChildrenMenus(string menuCode)
        {
            CommandResult cmdResult = CommandResult.Set(true, "更新失败");
            db.UseTransaction(true);
            string sql1 = "update Base_Menu set Enabled=0 where MenuCode in (select MenuCode from [dbo].[fn_GetChildrenMenu]('" + menuCode + "'))"; 
            db.Sql(sql1).Execute(); 
            db.Commit();
            cmdResult = CommandResult.Set(true, "更新成功");
            return cmdResult;
        }

        /// <summary>
        /// 根据菜单代码查询菜单的按钮数据
        /// </summary>
        /// <param name="menuCode">菜单代码</param>
        /// <returns>dynamic</returns>
        //public dynamic GetMenuButtons(string menuCode)
        //{
        //    string sql = "select a.MenuCode,a.ButtonCode,a.ButtonSort,a.ButtonText,b.ButtonName,b.ButtonType,b.IconClass,b.IconUrl,b.JsEvent,b.[Enabled] from [dbo].[Base_MenuButton] a left join [dbo].[Base_Button] b on a.ButtonCode=b.ButtonCode where a.MenuCode='" + menuCode + "'";
        //    return this.GetDynamicList(sql);
        //}

        /// <summary>
        /// 添加菜单按钮数据
        /// </summary>
        /// <param name="menuCode">菜单代码</param>
        /// <param name="userId">按钮代码</param>
        /// <returns>CommandResult</returns>
        public CommandResult AddMenuButton(string menuCode, string buttonCode, int buttonSort, string buttonText)
        {
            CommandResult cmdResult = CommandResult.Set(true, "保存成功");
            bool exists = Base_MenuButtonService.Instance.Exists_Fish(" and MenuCode='" + menuCode + "' and ButtonCode='" + buttonCode + "'");
            if (exists == false)
            {
                string sql = "insert into [dbo].[Base_MenuButton](MenuCode,ButtonCode,ButtonSort,ButtonText) values('" + menuCode + "','" + buttonCode + "'," + buttonSort + ",'" + buttonText + "')";
                int intResult = this.ExecuteNonQuery_Fish(sql);
                if (intResult == 0)
                {
                    cmdResult = CommandResult.Set(true, "插入菜单按钮失败");
                }
            }
            return cmdResult;
        }

        /// <summary>
        /// 移除菜单按钮记录
        /// </summary>
        /// <param name="roleCode">菜单代码</param>
        /// <param name="userId">按钮代码</param>
        /// <returns>CommandResult</returns>
        public CommandResult RemoveMenuButton(string menuCode, string buttonCode)
        {
            CommandResult cmdResult = CommandResult.Set(true, "操作成功");
            string sql = "delete from Base_MenuButton where MenuCode='" + menuCode + "' and ButtonCode='" + buttonCode + "'";
            int intResult = this.ExecuteNonQuery_Fish(sql); 
            sql = "delete from Base_RoleMenuButton where MenuCode='" + menuCode + "' and ButtonCode='" + buttonCode + "'";
            this.ExecuteNonQuery_Fish(sql); 
            if (intResult == 0)
            {
                cmdResult = CommandResult.Set(true, "移除菜单按钮失败");
            }
            return cmdResult;
        }
        /// <summary>
        /// 添加菜单按钮数据
        /// </summary>
        /// <param name="roleCode">菜单代码</param>
        /// <param name="userId">按钮代码</param>
        /// <returns>CommandResult</returns>
        public CommandResult SaveMenuButton(string menuCode, string buttonCode, int buttonSort, string buttonText)
        {
            CommandResult cmdResult = CommandResult.Set(true, "保存成功");
            bool exists = Base_MenuButtonService.Instance.Exists_Fish(" and MenuCode='" + menuCode + "' and ButtonCode='" + buttonCode + "'");
            if (exists == true)
            {
                string sql = string.Format("update [dbo].[Base_MenuButton] set ButtonSort={0},ButtonText='{1}' where MenuCode='{2}' and ButtonCode='{3}'",
                    buttonSort, buttonText, menuCode, buttonCode);
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
