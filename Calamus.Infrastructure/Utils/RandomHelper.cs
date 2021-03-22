using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Calamus.Infrastructure.Utils
{
    /// <summary>
    /// Random帮助类
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// 通过RNGCryptoServiceProvider 创建强随机数生成对象
        /// </summary>
        public static Random Default => CreateRandom();

        /// <summary>
        /// 通过RNGCryptoServiceProvider 创建强随机数生成对象 
        /// </summary>
        /// <returns>强随机数生成对象</returns>
        public static Random CreateRandom()
        {
            using (RandomNumberGenerator generator = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[10];
                generator.GetBytes(data);
                int seedInt = BitConverter.ToInt32(data, 0);
                return new Random(seedInt);
            }
        }
    }
}
