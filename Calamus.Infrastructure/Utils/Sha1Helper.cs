using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Calamus.Infrastructure.Utils
{
    /// <summary>
    /// SHA1 加解密
    /// </summary>
    public sealed class Sha1Helper
    {
        public static string Encrypt(string source)
        {
            return Encrypt(source, Encoding.UTF8);
        }

        public static string Encrypt(string source, Encoding encoding)
        {
            // 第一种方式
            byte[] byteArray = encoding.GetBytes(source);
            using (HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider())
            {
                byteArray = hashAlgorithm.ComputeHash(byteArray);
                StringBuilder stringBuilder = new StringBuilder(256);
                foreach (byte item in byteArray)
                    stringBuilder.AppendFormat("{0:x2}", item);
                hashAlgorithm.Clear();
                return stringBuilder.ToString();
            }

            //// 第二种方式
            //using (SHA1 sha1 = SHA1.Create())
            //{
            //    byte[] hash = sha1.ComputeHash(encoding.GetBytes(source));
            //    StringBuilder stringBuilder = new StringBuilder();
            //    for (int index = 0; index < hash.Length; ++index)
            //        stringBuilder.Append(hash[index].ToString("x2"));
            //    sha1.Clear();
            //    return stringBuilder.ToString();
            //}
        }

        public static string Decrypt(string source)
        {
            throw new System.NotImplementedException();
        }

        public static string Decrypt(string source, Encoding encoding)
        {
            throw new System.NotImplementedException();
        }
    }
}
