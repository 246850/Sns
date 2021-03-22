using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Calamus.Infrastructure.Utils
{
    /// <summary>
    /// Md5 加解密
    /// </summary>
    public sealed class Md5Helper
    {
        public static string Encrypt(string source)
        {
            return Encrypt(source, Encoding.UTF8);
        }

        public static string Encrypt(string source, Encoding encoding)
        {
            byte[] byteArray = encoding.GetBytes(source);
            using (HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider())
            {
                byteArray = hashAlgorithm.ComputeHash(byteArray);
                StringBuilder stringBuilder = new StringBuilder(256);
                foreach (byte item in byteArray)
                    stringBuilder.AppendFormat("{0:x2}", item);
                hashAlgorithm.Clear();
                return stringBuilder.ToString().ToUpper();
            }
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
