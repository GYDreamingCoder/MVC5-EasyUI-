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
    /// 【地区】服务实例
    /// </summary>
    public class Base_AreaService : BaseService<Base_Area>
    {
        /// <summary>
        /// 【地区】服务实例【单例模式】
        /// </summary>
        private static readonly Base_AreaService _Instance = new Base_AreaService();

        /// <summary>
        /// 获取【地区】服务实例【单例模式】
        /// </summary>
        public static Base_AreaService Instance
        {
            get
            {
                return _Instance;
            }
        }
    }
}
