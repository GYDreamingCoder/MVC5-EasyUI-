//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:56
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
    /// 【用户角色】服务实例
    /// </summary>
    public class Base_UserRoleService : BaseService<Base_UserRole>
    {

        /// <summary>
        /// 【用户角色】服务实例【单例模式】
        /// </summary>
        private static readonly Base_UserRoleService _Instance = new Base_UserRoleService();

        /// <summary>
        /// 获取【用户角色】服务实例【单例模式】
        /// </summary>
        public static Base_UserRoleService Instance
        {
            get
            {
                return _Instance;
            }
        }

    }
}
