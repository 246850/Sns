using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Extensions
{
    /// <summary>
    /// Guid 扩展方法
    /// </summary>
    public static class GuidExtension
    {
        /// <summary>
        /// Guid 转 16位长度字符串 - 几乎唯一性
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string To16String(this Guid source)
        {
            long i = 1;
            foreach (byte b in source.ToByteArray())
                i *= (b + 1);
            return string.Format("{0:x}", i - DateTime.Now.ToTimestamp());
        }

        /// <summary>
        /// Guid 转 Int64 - 几乎唯一性
        /// </summary>
        /// <param name="source">源 Guid</param>
        /// <returns>Int64数字</returns>
        public static long ToInt64(this Guid source)
        {
            byte[] buffer = source.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Guid 转 字符串 - 去掉 横杠-
        /// </summary>
        /// <param name="source">源 Guid</param>
        /// <returns>Guid去掉横杠字符串</returns>
        public static string ToGuidString(this Guid source)
        {
            string guid = source.ToString();
            return guid.Replace("-", string.Empty);
        }
    }
}
