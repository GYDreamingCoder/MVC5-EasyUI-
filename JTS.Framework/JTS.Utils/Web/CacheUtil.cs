using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace JTS.Utils
{
    /// <summary>
    /// 基于MemoryCache的缓存工具类。
    /// </summary>
    public static class CacheUtil
    {
        private static readonly Object _locker = new object();
        #region 使用方法代理获取缓存数据
        /// <summary>
        /// 获取缓存数据。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="key">键</param>
        /// <param name="cachePopulate">用来初始化键的值</param>
        /// <param name="ExpireMinutes">过期时间，默认值：30(分钟)。（单位：分钟）</param>
        /// <returns>返回对象类型<T></returns>
        public static T GetCacheItem<T>(String key, Func<T> cachePopulate, int ExpireMinutes = 30)
        {
            TimeSpan ts = new TimeSpan(0, ExpireMinutes, 0);
            return GetCacheItem<T>(key, cachePopulate, ts, null);
        }

        /// <summary>
        /// 获取缓存数据。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="key">键</param>
        /// <param name="cachePopulate">用来初始化键的值</param>
        /// <param name="slidingExpiration">默认值：如：new TimeSpan(0, 30, 0));//30分钟过期</param>
        /// <param name="absoluteExpiration"></param>
        /// <returns></returns>
        private static T GetCacheItem<T>(String key, Func<T> cachePopulate, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid cache key");
            if (cachePopulate == null) throw new ArgumentNullException("cachePopulate");
            if (slidingExpiration == null && absoluteExpiration == null) throw new ArgumentException("Either a sliding expiration or absolute must be provided");

            if (MemoryCache.Default[key] == null)
            {
                lock (_locker)
                {
                    if (MemoryCache.Default[key] == null)
                    {
                        var item = new CacheItem(key, cachePopulate());

                        var policy = CreatePolicy(slidingExpiration, absoluteExpiration);

                        MemoryCache.Default.Add(item, policy);
                    }
                }
            }

            return (T)MemoryCache.Default[key];
        }
        #endregion


        #region 直接使用传入的数据缓存数据
        /// <summary>
        /// 获取缓存数据。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="key">键</param>
        /// <param name="Object">需要缓存的数据</param>
        /// <param name="ExpireMinutes">过期时间，默认值：30(分钟)。（单位：分钟）</param>
        /// <returns>返回对象类型<T></returns>
        public static Object GetCacheItem(String key, Object objectData, int ExpireMinutes = 30)
        {
            TimeSpan ts = new TimeSpan(0, ExpireMinutes, 0);
            return GetCacheItem(key, objectData, ts, null);
        }

        /// <summary>
        /// 获取缓存数据。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="key">键</param>
        /// <param name="objectData">需要缓存的数据</param>
        /// <param name="slidingExpiration">相对过期时间。默认值：如：new TimeSpan(0, 30, 0));//30分钟过期</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <returns></returns>
        private static Object GetCacheItem(String key, Object objectData, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid cache key");
            if (objectData == null) throw new ArgumentNullException("objectData");
            if (slidingExpiration == null && absoluteExpiration == null) throw new ArgumentException("Either a sliding expiration or absolute must be provided");

            if (MemoryCache.Default[key] == null)
            {
                lock (_locker)
                {
                    if (MemoryCache.Default[key] == null)
                    {
                        var item = new CacheItem(key, objectData);

                        var policy = CreatePolicy(slidingExpiration, absoluteExpiration);

                        MemoryCache.Default.Add(item, policy);
                    }
                }
            }

            return MemoryCache.Default[key];
        }
        #endregion










        private static CacheItemPolicy CreatePolicy(TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
            {
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            else if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = slidingExpiration.Value;
            }

            policy.Priority = CacheItemPriority.Default;

            return policy;
        }

        public static void ClearAll()
        {
            MemoryCache.Default.ToList().ForEach(kv => MemoryCache.Default.Remove(kv.Key));
            //memoryCache.ToList().ForEach(kv => memoryCache.Remove(kv.Key));
        }
        public static void Clear(String key)
        {
            MemoryCache.Default.Remove(key.ToString());
        }
    }
}
/*
 
      /// <summary>
        /// 根据用户的ID，获取用户的全名，并放到缓存里面
        /// </summary>
        /// <param name="userId">用户的ID</param>
        /// <returns></returns>
        public static string GetUserFullName(string userId)
        {            
            string key = "Security_UserFullName" + userId;
            string fullName = MemoryCacheHelper.GetCacheItem<string>(key,
                delegate() { return BLLFactory<User>.Instance.GetFullNameByID(userId.ToInt32()); },
                new TimeSpan(0, 30, 0));//30分钟过期
            return fullName;
        }
 
MemoryCacheHelper的方法GetCacheItem里面的Func<T>我使用了一个匿名函数用来获取缓存的值。

delegate() { return BLLFactory<User>.Instance.GetFullNameByID(userId.ToInt32()); }
而调用BLLFactory<User>.Instance.GetFullNameByID则是从数据库里面获取对应的数据了。

这样在第一次或者缓存过期的时候，自动调用业务对象类的方法来获取数据了。

最后，在界面上调用GetUserFullName的方法即可实现基于缓存方式的调用，程序第一次使用的，碰到指定的键没有数据，就去数据库里面获取，以后碰到该键，则直接获取缓存的数据了。
 * 
 * 
 */