//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:49
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
    /// 【角色菜单】服务实例
    /// </summary>
    public class Base_RoleMenuService : BaseService<Base_RoleMenu>
    {

        /// <summary>
        /// 【角色菜单】服务实例【单例模式】
        /// </summary>
        private static readonly Base_RoleMenuService _Instance = new Base_RoleMenuService();

        /// <summary>
        /// 获取【角色菜单】服务实例【单例模式】
        /// </summary>
        public static Base_RoleMenuService Instance
        {
            get
            {
                return _Instance;
            }
        }

    }
}
