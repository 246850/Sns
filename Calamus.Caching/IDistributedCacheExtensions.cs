using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Calamus.Caching
{
    /// <summary>
    /// IDistributedCache  扩展
    /// </summary>
    public static class IDistributedCacheExtensions
    {
        /// <summary>
        /// 获取缓存值 并转换 为 T 对象 - json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static T Get<T>(this IDistributedCache distributedCache, string key)
            where T : class
        {
            var json = distributedCache.GetString(key);
            if (string.IsNullOrWhiteSpace(json)) return default(T);

            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// 获取缓存值 并转换 为 T 对象 - json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken))
            where T : class
        {
            var json = await distributedCache.GetStringAsync(key, token);
            if (string.IsNullOrWhiteSpace(json)) return default(T);

            return JsonSerializer.Deserialize<T>(json);
        }
        /// <summary>
        /// 存在直接返回，不存在先创建缓存再返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this IDistributedCache distributedCache, string key, Func<T> valueFactory, long? expiry)
            where T : class
        {
            T result = Get<T>(distributedCache, key);

            if (result == null)
            {
                T item = valueFactory();
                Set(distributedCache, key, item, new DistributedCacheEntryOptions { SlidingExpiration = expiry.ToTimeSpan() });
                return item;
            }

            return result;
        }
        /// <summary>
        /// 存在直接返回，不存在先创建缓存再返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this IDistributedCache distributedCache, string key, Func<T> valueFactory, DistributedCacheEntryOptions options)
            where T : class
        {
            T result = Get<T>(distributedCache, key);

            if (result == null)
            {
                T item = valueFactory();
                Set(distributedCache, key, item, options);
                return item;
            }

            return result;
        }
        /// <summary>
        /// 存在直接返回，不存在先创建缓存再返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache distributedCache, string key, Func<T> valueFactory, long? expiry, CancellationToken token = default(CancellationToken))
            where T : class
        {
            T result = await GetAsync<T>(distributedCache, key);

            if (result == null)
            {
                T item = valueFactory();
                await SetAsync(distributedCache, key, item, new DistributedCacheEntryOptions { SlidingExpiration = expiry.ToTimeSpan() }, token);
                return item;
            }

            return result;
        }
        /// <summary>
        /// 存在直接返回，不存在先创建缓存再返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache distributedCache, string key, Func<T> valueFactory, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
            where T : class
        {
            T result = await GetAsync<T>(distributedCache, key);

            if (result == null)
            {
                T item = valueFactory();
                await SetAsync(distributedCache, key, item, options, token);
                return item;
            }

            return result;
        }
        /// <summary>
        /// 设置缓存值 T 对象 json序列化
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set<T>(this IDistributedCache distributedCache, string key, T value, long? expiry) where T : class
            => Set<T>(distributedCache, key, value, new DistributedCacheEntryOptions { SlidingExpiration = expiry.ToTimeSpan() });
        /// <summary>
        /// 设置缓存值 T 对象 json序列化
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options)
            where T : class
        {
            var json = JsonSerializer.Serialize(value);
            distributedCache.SetString(key, json, options);
        }
        /// <summary>
        /// 设置缓存值 T 对象 json序列化
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, long? expiry, CancellationToken token = default(CancellationToken)) where T : class
            => SetAsync<T>(distributedCache, key, value, new DistributedCacheEntryOptions { SlidingExpiration = expiry.ToTimeSpan() }, token);
        /// <summary>
        /// 设置缓存值 T 对象 json序列化
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
            where T : class
        {
            var json = JsonSerializer.Serialize(value);
            return distributedCache.SetStringAsync(key, json, options, token);
        }
    }
}
