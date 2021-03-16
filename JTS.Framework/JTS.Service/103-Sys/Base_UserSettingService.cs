//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:57
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
    /// 【用户设置】服务实例
    /// </summary>
    public class Base_UserSettingService : BaseService<Base_UserSetting>
    {

        /// <summary>
        /// 【用户设置】服务实例【单例模式】
        /// </summary>
        private static readonly Base_UserSettingService _Instance = new Base_UserSettingService();

        /// <summary>
        /// 获取【用户设置】服务实例【单例模式】
        /// </summary>
        public static Base_UserSettingService Instance
        {
            get
            {
                return _Instance;
            }
        }

    }
}
