using System;

namespace YFK.Core.Cache
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CacheExtensions
    {
        private static readonly object SyncObject = new object();

        /// <summary>
        /// ȡ�������(��������в����ڸ������Ϊ����,Ĭ�ϻ���ʱ��60����)
        /// </summary>
        /// <typeparam name="T">obj</typeparam>
        /// <param name="cacheManager">cacheManager</param>
        /// <param name="key">key</param>
        /// <param name="acquire">acquire</param>
        /// <returns></returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            lock (SyncObject)
            {
                if (cacheManager.IsSet(key))
                {
                    return cacheManager.Get<T>(key);
                }

                var result = acquire();
                if (cacheTime > 0)
                    cacheManager.Set(key, result, cacheTime);
                return result;
            }
        }
    }
}
