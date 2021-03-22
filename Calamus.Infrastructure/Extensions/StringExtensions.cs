using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Calamus.Infrastructure.Extensions
{
    /// <summary>
    /// String 扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// String 转 Unicode - Unicode编码
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码字符串</returns>
        public static string ToUnicode(this string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder(256);
            for (int i = 0; i < bytes.Length; i += 2)
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode 转 String - Unicode反编码
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode反编码字符串</returns>
        public static string DeUnicode(this string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        /// <summary>
        /// 字符串 过滤 Html标签
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>过滤后 字符串</returns>
        public static string FilterHtml(this string source)
        {
            string result = Regex.Replace(source, "<[^>]+>", "");
            result = Regex.Replace(result, "&[^;]+;", "");
            return result;
        }

        /// <summary>
        /// String 转 Int32
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Int32数字</returns>
        public static int ToInt32(this string source)
        {
            int result;
            if (int.TryParse(source, out result))
                return result;
            return 0;
        }

        /// <summary>
        /// String 转 Int64
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Int64数字</returns>
        public static long ToInt64(this string source)
        {
            long result;
            if (long.TryParse(source, out result))
                return result;
            return 0;
        }

        #region - 字符串裁剪
        /// <summary>
        /// 字符串后缀 - “...”
        /// </summary>
        public const string Suffix = "...";
        /// <summary>
        /// 裁剪 字符串 无后缀
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="length">长度</param>
        /// <returns>裁剪后字符串</returns>
        public static string Cut(this string source, int length)
        {
            return CutWithSuffix(source, length, string.Empty);
        }

        /// <summary>
        /// 裁剪 字符串 并 默认带后缀 ...
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="length">长度</param>
        /// <returns>裁剪后字符串</returns>
        public static string CutWithSuffix(this string source, int length)
        {
            return CutWithSuffix(source, length, Suffix);
        }

        /// <summary>
        /// 裁剪 字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="length">长度</param>
        /// <param name="suffix">后缀</param>
        /// <returns>裁剪后字符串</returns>
        public static string CutWithSuffix(this string source, int length, string suffix)
        {
            if (string.IsNullOrWhiteSpace(source)) return string.Empty;

            int total = source.Length;
            string result = source;
            if (total > length)
            {
                result = result.Substring(0, length);
                if (!string.IsNullOrWhiteSpace(suffix))
                    result = string.Concat(result, suffix);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Base64 编码 - 默认编码 UTF-8
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Base64编码字符串</returns>
        public static string ToBase64(this string source)
        {
            return ToBase64(source, Encoding.UTF8);
        }
        /// <summary>
        /// Base64 编码
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>Base64编码字符串</returns>
        public static string ToBase64(this string source, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64 解码 - 默认编码 UTF-8
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Base64解码字符串</returns>
        public static string DeBase64(this string source)
        {
            return DeBase64(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64 解码
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>Base64解码字符串</returns>
        public static string DeBase64(this string source, Encoding encoding)
        {
            byte[] bytes = Convert.FromBase64String(source);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将给定的字符串转首字母小写
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToFirstCharLower(this string source)
        {
            if (string.IsNullOrEmpty(source) || !char.IsUpper(source[0]))
            {
                return source;
            }

            char[] chars = source.ToCharArray();
            FixCasing(chars);
            return new string(chars);
        }
        static void FixCasing(Span<char> chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);

                // Stop when next char is already lowercase.
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // If the next char is a space, lowercase current char before exiting.
                    if (chars[i + 1] == ' ')
                    {
                        chars[i] = char.ToLowerInvariant(chars[i]);
                    }

                    break;
                }

                chars[i] = char.ToLowerInvariant(chars[i]);
            }
        }
    }
}
