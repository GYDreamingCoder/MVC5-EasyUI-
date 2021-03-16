//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:39
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
    /// 【单据编码规则】服务
    /// </summary>
    public class Base_CodeRuleService : BaseService<Base_CodeRule>
    {
        /// <summary>
        /// 【单据编码规则】服务实例【单例模式】
        /// </summary>
        private static readonly Base_CodeRuleService _Instance = new Base_CodeRuleService();

        /// <summary>
        /// 获取【单据编码规则】服务实例【单例模式】
        /// </summary>
        public static Base_CodeRuleService Instance
        {
            get
            {
                return _Instance;
            }
        }

    }
}
