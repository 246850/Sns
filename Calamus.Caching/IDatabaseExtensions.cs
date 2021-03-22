using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calamus.Caching
{
    /// <summary>
    /// Redis IDatabase 扩展
    /// </summary>
    public static class IDatabaseExtensions
    {
        #region String 命令
        /// <summary>
        /// String命令 - set
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">有效期，单位：秒</param>
        /// <returns></returns>
        public static bool StringSet(this IDatabase db, string key, RedisValue value, long? expiry, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => db.StringSet(key, value, expiry.ToTimeSpan(), when, flags);
        /// <summary>
        /// String命令 - set
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">有效期，单位：秒</param>
        /// <param name="when"></param>
        /// <returns></returns>
        public static Task<bool> StringSetAsync(this IDatabase db, string key, RedisValue value, long? expiry, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => db.StringSetAsync(key, value, expiry.ToTimeSpan(), when, flags);
        /// <summary>
        /// String命令 - 设置缓存 - json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="expiry">有效期，单位：秒</param>
        /// <returns></returns>
        public static bool Set<T>(this IDatabase db, string key, T item, long? expiry = null) where T : class
        {
            string value = JsonSerializer.Serialize(item);
            return StringSet(db, key, value, expiry);
        }
        /// <summary>
        /// String命令 - 设置缓存 - json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="expiry">有效期，单位：秒</param>
        /// <returns></returns>
        public static Task<bool> SetAsync<T>(this IDatabase db, string key, T item, long? expiry = null) where T : class
        {
            string value = JsonSerializer.Serialize(item);
            return StringSetAsync(db, key, value, expiry);
        }
        /// <summary>
        /// String命令 - 获取缓存对象 - json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDatabase db, string key) where T : class
        {
            string value = db.StringGet(key);
            if (string.IsNullOrWhiteSpace(value)) return default(T);

            T result = JsonSerializer.Deserialize<T>(value);
            return result;
        }
        /// <summary>
        /// String命令 - 获取缓存对象 - json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this IDatabase db, string key) where T : class
        {
            string value = await db.StringGetAsync(key);
            if (string.IsNullOrWhiteSpace(value)) return default(T);

            T result = JsonSerializer.Deserialize<T>(value);
            return result;
        }
        /// <summary>
        /// String命令 - 获取缓存，不存在先创建，再返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="expiry">有效期，单位：秒</param>
        /// <returns></returns>
        public static T GetOrAdd<T>(IDatabase db, string key, Func<T> valueFactory, long? epxiry) where T:class
        {
            bool flag = db.KeyExists(key);
            if (!flag)
            {
                var entry = valueFactory();
                Set<T>(db, key, entry, epxiry);
            }

            return Get<T>(db, key);
        }
        /// <summary>
        /// String命令 - 获取缓存，不存在先创建，再返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="expiry">有效期，单位：秒</param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(IDatabase db, string key, Func<T> valueFactory, long? epxiry) where T : class
        {
            bool flag = await db.KeyExistsAsync(key);
            if (!flag)
            {
                var entry = valueFactory();
                await SetAsync<T>(db, key, entry, epxiry);
            }

            return await GetAsync<T>(db, key);
        }
        #endregion

        #region Key命令
        /// <summary>
        /// Key命令 设置/刷新过期时间
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        /// <param name="expiry">单位：秒</param>
        /// <returns></returns>
        public static bool KeyExpire(this IDatabase db, string key, long? expiry)
            => db.KeyExpire(key, expiry.ToTimeSpan());
        /// <summary>
        /// Key命令 设置/刷新过期时间
        /// </summary>
        /// <param name="IDatabase"></param>
        /// <param name="key"></param>
        /// <param name="expiry">单位：秒</param>
        /// <returns></returns>
        public static Task<bool> KeyExpireAsync(this IDatabase db, string key, long? expiry)
            => db.KeyExpireAsync(key, expiry.ToTimeSpan());
        #endregion
    }
}
