//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:51
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
    /// 【角色菜单按钮】服务实例
    /// </summary>
    public class Base_RoleMenuButtonService : BaseService<Base_RoleMenuButton>
    {

        /// <summary>
        /// 【角色菜单按钮】服务实例【单例模式】
        /// </summary>
        private static readonly Base_RoleMenuButtonService _Instance = new Base_RoleMenuButtonService();

        /// <summary>
        /// 获取【角色菜单按钮】服务实例【单例模式】
        /// </summary>
        public static Base_RoleMenuButtonService Instance
        {
            get
            {
                return _Instance;
            }
        }

    }
}
