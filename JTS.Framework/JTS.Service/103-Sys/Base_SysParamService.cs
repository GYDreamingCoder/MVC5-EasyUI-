//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:53
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
    /// 【系统参数】服务
    /// </summary>
    public class Base_SysParamService : BaseService<Base_SysParam>
    {

        /// <summary>
        /// 【系统参数】服务实例【单例模式】
        /// </summary>
        private static readonly Base_SysParamService _Instance = new Base_SysParamService();

        /// <summary>
        /// 获取【系统参数】服务实例【单例模式】
        /// </summary>
        public static Base_SysParamService Instance
        {
            get
            {
                return _Instance;
            }
        }

    }
}
