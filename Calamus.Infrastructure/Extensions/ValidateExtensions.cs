using System.Text.RegularExpressions;

namespace Calamus.Infrastructure.Extensions
{
    /// <summary>
    /// 字符串 数据验证 扩展方法
    /// </summary>
    public static class ValidateExtensions
    {
        /// <summary>
        /// 是否Url地址 - [http|https]
        /// </summary>
        /// <param name="source">验证字符串</param>
        /// <returns>true：是，false：否</returns>
        public static bool IsUrl(this string source)
        {
            return Regex.IsMatch(source,
                @"^((http|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否图片地址 - [jpg|jpeg|gif|bmp|png]
        /// </summary>
        /// <param name="source">验证字符串</param>
        /// <returns>true：是，false：否</returns>
        public static bool IsImageExtension(this string source)
        {
            return Regex.IsMatch(source, @".*(\.JPEG|\.jpeg|\.JPG|\.jpg|\.GIF|\.gif|\.BMP|\.bmp|\.PNG|\.png)$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否金额 - 4位小数，如：34.2156
        /// </summary>
        /// <param name="source">验证字符串</param>
        /// <returns>true：是，false：否</returns>
        public static bool IsMoney(this string source)
        {
            return Regex.IsMatch(source, @"^(([1-9]\d*)|0)(\.\d{1,4})?$");
        }

        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="source">验证字符串</param>
        /// <returns>true：是，false：否</returns>
        public static bool IsNumberic(this string source)
        {
            return Regex.IsMatch(source, @"^\d+$");
        }

        /// <summary>
        /// 是否邮箱地址
        /// </summary>
        /// <param name="source">验证字符串</param>
        /// <returns>true：是，false：否</returns>
        public static bool IsEmail(this string source)
        {
            return Regex.IsMatch(source, @"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$");
        }

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="source">验证字符串</param>
        /// <returns>true：是，false：否</returns>
        public static bool IsMobileNumber(this string source)
        {
            return Regex.IsMatch(source, @"^1[3|4|5|8][0-9]\d{8}$");
        }

        /// <summary>
        /// 是否正确格式的 sql语句
        /// </summary>
        /// <param name="source">验证字符串</param>
        /// <returns>true：是，false：否</returns>
        public static bool IsSafeSqlString(this string source)
        {
            return !Regex.IsMatch(source, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 是否Ip地址
        /// </summary>
        /// <param name="source">验证字符串</param>
        /// <returns>true：是，false：否</returns>
        public static bool IsIpAddress(this string source)
        {
            if (string.IsNullOrWhiteSpace(source) || source.Length < 7 || source.Length > 15)
                return false;

            return Regex.IsMatch(source, @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})");
        }
        /// <summary>
        /// 是否包含空格
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ContainsSpace(this string source)
        {
            return Regex.IsMatch(source, @"\s+");
        }

        /// <summary>
        /// 字符串长度区间检查 - 不包含空格
        /// </summary>
        /// <param name="source"></param>
        /// <param name="min">最小长度</param>
        /// <param name="max">最大长度</param>
        /// <returns></returns>
        public static bool MustLengthNotSpace(this string source, int min, int max)
        {
            if (string.IsNullOrWhiteSpace(source)) return false;
            return Regex.IsMatch(source, string.Format(@"^\S{{{0},{1}}}$", min, max));
        }

        /// <summary>
        /// 字符串长度区间检查 - 可包含空格
        /// </summary>
        /// <param name="source"></param>
        /// <param name="min">最小长度</param>
        /// <param name="max">最大长度</param>
        /// <returns></returns>
        public static bool MustLength(this string source, int min, int max)
        {
            if (string.IsNullOrWhiteSpace(source)) return false;
            return Regex.IsMatch(source, string.Format("^(.|\r|\n){{{0},{1}}}$", min, max));
        }
    }
}
