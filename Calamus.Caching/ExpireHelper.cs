using System;
using System.Security.Cryptography;

namespace Calamus.Caching
{
    /// <summary>
    /// 随机 过期时间 工具类 - 有助于 防止雪崩
    /// </summary>
    public sealed class ExpireHelper
    {
        static Random CreateRandom()
        {
            using (RandomNumberGenerator generator = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[10];
                generator.GetBytes(data);
                int seedInt = BitConverter.ToInt32(data, 0);
                return new Random(seedInt);
            }
        }

        /// <summary>
        /// 随机过期时间 单位：秒
        /// </summary>
        /// <param name="max">最大 单位：秒</param>
        /// <returns></returns>
        public static long RandomExpiry(int max)
            => RandomExpiry(0, 0, max);
        /// <summary>
        /// 随机过期时间 单位：秒
        /// </summary>
        /// <param name="min">最小 单位：秒</param>
        /// <param name="max">最大 单位：秒</param>
        /// <returns></returns>
        public static long RandomExpiry(int min, int max)
            => RandomExpiry(0, min, max);
        /// <summary>
        /// 在随机过期时间 单位：秒，在基数秒增加随机秒数
        /// </summary>
        /// <param name="basic">基数秒</param>
        /// <param name="min">最小 单位：秒</param>
        /// <param name="max">最大 单位：秒</param>
        /// <returns></returns>
        public static long RandomExpiry(long basic, int min, int max)
        {
            Random rn = CreateRandom();
            long value = rn.Next(min, max);
            return basic + value;
        }
    }
}
