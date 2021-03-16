//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:38
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
    /// 【功能按钮】服务类
    /// </summary>
    public class Base_ButtonService : BaseService<Base_Button>
    {

        /// <summary>
        /// 【功能按钮】服务实例【单例模式】
        /// </summary>
        private static readonly Base_ButtonService _Instance = new Base_ButtonService();

        /// <summary>
        /// 获取【功能按钮】服务实例【单例模式】
        /// </summary>
        public static Base_ButtonService Instance
        {
            get
            {
                return _Instance;
            }
        }

    }
}
