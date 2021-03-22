using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Infrastructure.Utils
{
    /// <summary>
    /// 随机字符串帮助类
    /// </summary>
    public static class NonceHelper
    {
        /// <summary>
        /// 生成随机字符串[a-zA-Z0-9] 默认长度 10
        /// </summary>
        /// <returns>随机字符串</returns>
        public static string GenerateString()
        {
            return GenerateString(10);
        }

        /// <summary>
        /// 生成随机字符串[a-zA-Z0-9]
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>随机字符串</returns>
        public static string GenerateString(int length)
        {
            string[] alphabets = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
            StringBuilder nonceString = new StringBuilder(50);

            while (nonceString.Length < length)
            {
                int index = RandomHelper.Default.Next(alphabets.Length);
                nonceString.Append(alphabets[index]);
            }
            return nonceString.ToString();
        }
    }
}
