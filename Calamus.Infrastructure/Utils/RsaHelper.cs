using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Calamus.Infrastructure.Utils
{
    /// <summary>
    /// RSA 加解密
    /// </summary>
    public sealed class RsaHelper
    {
        //加密方 提供
        private static readonly string RsaExponent = "AQAB";
        //加密方 提供
        private static readonly string RsaModulus = @"jB2QANLhObNEGfhigsu7ZtHhVKH20+ChdwvvWY86nrfX0zywh/3RvjjGTMcbUgWin3/Pi03aAPIp4wZR+CfPINm4T9pEcaGV+UVBZ7za97Wlr0+gUS+RFmfmP3K1sQi58UBJQBuVb0mufNMr9Htjfaqlvhj2LrsrS/b3bhtIqWs=";
        //解密方 私钥
        private static readonly string RsaPrivateKey = @"<RSAKeyValue><Modulus>xJEmstiGQuRfLhQWI316TFBsNGVs+oi50M7M59MzT0tuq5azK8ruZ+2
                                                    fwxmIZOweex07USP2nD9d+v0I/hccjFWJ+/M3zkS9qiHZDD+GChDSUQheFS9RmmyIyH9tEwHKqUvPwT2
                                                    A3/pu1/6UFJ5j2/QMzkieo6UP3FEYXb+HKec=</Modulus><Exponent>AQAB</Exponent><P>4cHs8
                                                    /wb1ASgpuqaP1sRlp6t1XyD2xhTlKkKlifL0BIQPRG9KY82JF+knk3XbYwnM65MVQbbAk9DAGHpRprDz
                                                    w==</P><Q>3uYsNVlgoR7oBvRoUJokBAnQSpL6aBLz5COUrmBqJIJ02/scw22aKk6yYEYLymoFWIx/vs
                                                    ahqxU/GYBKPqoGaQ==</Q><DP>LgSot2dJiONUmBG0VXvLzwcTajQScKO5zdDTXp5IFmbINpqaE5GkuK
                                                    9iT/6QDj2GdCtwVdiq5gKgJsDOm1zK0w==</DP><DQ>0Em0O/IWKtmPppgTNmajiyaEfntUBZbYU3KwS
                                                    DaOWSmS+9Fu8mvj6O77Hp21/OMPtcwsv2AryIwlF7ZbKq2FKQ==</DQ><InverseQ>J8TNLuWQmqo807
                                                    beACEHIuA9w/mrrCsIQki304/1p3o9Hwrs1nImtT/3V64Uyxk3mnnITulxqn6T7qI1+WvMaw==</Inve
                                                    rseQ><D>iCLzR1g4nGloFhSpRIkpw2LLnfpE4LsC3j/rn/8hASEAE+y6SHEybl4fkNLFKQhzD9ct9sNO
                                                    j+Y/I45NxRSimsQEMbqaeQir+dFF8aFbUBpKWxyzGUmSPU87GN8GFKaKwcU6QVP9MpFEMNOcTppfCuV/
                                                    iDGmTTOoxEGKzPIud/E=</D></RSAKeyValue>";
        public static string Encrypt(string source, string exponent, string modulus, Encoding encoding)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                RSAParameters para = new RSAParameters
                {
                    Exponent = Convert.FromBase64String(exponent),
                    Modulus = Convert.FromBase64String(modulus)
                };
                rsaProvider.ImportParameters(para);
                byte[] encryptBytes = rsaProvider.Encrypt(encoding.GetBytes(source), false);
                rsaProvider.Clear();
                string result = Convert.ToBase64String(encryptBytes);
                return result;
            }
        }

        public static string Decrypt(string source, string rsaPrivateKey, Encoding encoding)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(rsaPrivateKey);
                byte[] decryptBytes = rsaProvider.Decrypt(Convert.FromBase64String(source), false);
                string result = encoding.GetString(decryptBytes);
                rsaProvider.Clear();

                return result;
            }
        }

        public static string Encrypt(string source)
        {
            return Encrypt(source, Encoding.UTF8);
        }

        public static string Encrypt(string source, Encoding encoding)
        {
            return Encrypt(source, RsaExponent, RsaModulus, encoding);
        }

        public static string Decrypt(string source)
        {
            return Decrypt(source, Encoding.UTF8);
        }

        public static string Decrypt(string source, Encoding encoding)
        {
            return Decrypt(source, RsaPrivateKey, encoding);
        }

    }
}
