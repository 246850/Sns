using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Extensions
{
    /// <summary>
    /// DateTime 扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 时间 转 时间戳 unix
        /// </summary>
        /// <param name="datetime">当前时间</param>
        /// <returns>时间戳 64位有符号整数</returns>
        public static long ToTimestamp(this DateTime datetime)
        {
            return Convert.ToInt64(datetime.Subtract(TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local)).TotalSeconds);
        }
        /// <summary>
        /// 时间戳 转 DateTime
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns>日期时间 - DateTime</returns>
        public static DateTime AsDateTime(this string timestamp)
        {
            long target;
            if (long.TryParse(timestamp, out target))
                return AsDateTime(target);

            throw new Exception("不正确的时间戳格式");
        }

        /// <summary>
        /// 时间戳 转 DateTime
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns>日期时间 - DateTime</returns>
        public static DateTime AsDateTime(this long timestamp)
        {
            long longTime = long.Parse(timestamp + "0000000");
            TimeSpan date = new TimeSpan(longTime);
            return TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local).Add(date);
        }

        /// <summary>
        /// DateTime 格式化 为字符串 - 默认 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="date">当前时间</param>
        /// <returns>时间字符串</returns>
        public static string ToFormat(this DateTime date)
        {
            return ToFormat(date, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// DateTime 格式化 为字符串
        /// </summary>
        /// <param name="date">当前时间</param>
        /// <param name="format">格式字符串 如：yyyy-MM-dd HH:mm:ss</param>
        /// <returns>时间字符串</returns>
        public static string ToFormat(this DateTime date, string format)
        {
            return date.ToString(format);
        }
    }
}
