//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:43
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
    /// 【数据字典明细】服务 
    /// </summary>
    public class Base_DictItemService : BaseService<Base_DictItem>
    {
        /// <summary>
        /// 【数据字典明细】服务实例【单例模式】
        /// </summary>
        private static readonly Base_DictItemService _Instance = new Base_DictItemService();

        /// <summary>
        /// 获取【数据字典明细】服务实例【单例模式】
        /// </summary>
        public static Base_DictItemService Instance
        {
            get
            {
                return _Instance;
            }
        }
    }
}
