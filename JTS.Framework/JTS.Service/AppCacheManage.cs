using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JTS.Entity;
using JTS.Core;
using JTS.Utils;


namespace JTS.Service
{
    /// <summary>
    /// 框架缓存管理类。
    /// </summary>
    public static class AppCacheManage
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static AppCacheManage() { }
 
        /// <summary>
        /// 清除指定key的MemoryCache缓存数据
        /// </summary>
        /// <param name="key">缓存键key</param>
        public static void Clear(string key)
        {
            CacheUtil.Clear(key);
        }

        /// <summary>
        /// 清除所有MemoryCache缓存数据
        /// </summary>
        public static void ClearAll()
        {
            CacheUtil.ClearAll(); 
        }

        ///// <summary>
        ///// 获取客户列表
        ///// </summary>
        //public static List<BSERP_KEHU> ListKehu
        //{
        //    get
        //    {
        //        return MemoryCacheHelper.GetCacheItem<List<BSERP_KEHU>>(Consts.Cache_BaseData_Kehu, delegate() { return BF<BLL_BSERP_KEHU>.Instance.GetList(); }, 60 * 12);
        //    }
        //}

        ///// <summary>
        ///// 获取仓库列表
        ///// </summary>
        ///// <returns></returns>
        //public static List<BSERP_CANGKU> ListCangku
        //{
        //    get
        //    {
        //        return MemoryCacheHelper.GetCacheItem<List<BSERP_CANGKU>>(Consts.Cache_BaseData_Cangku, delegate() { return BF<BLL_BSERP_CANGKU>.Instance.GetList(); }, 60 * 12);
        //    }
        //}

        ///// <summary>
        ///// 获取商品列表
        ///// </summary>
        ///// <returns></returns>
        //public static List<BSERP_SHANGPIN> ListShangpin
        //{
        //    get
        //    {
        //        return MemoryCacheHelper.GetCacheItem<List<BSERP_SHANGPIN>>(Consts.Cache_BaseData_Shangpin, delegate() { return BF<BLL_BSERP_SHANGPIN>.Instance.GetList(); }, 60 * 12);
        //    }
        //}
    }
}
