using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;

namespace Calamus.Ioc
{
    /// <summary>
    /// 通用应用程序配置选项
    /// </summary>
    public class CalamusOptions
    {
        public CalamusOptions()
        {
            Databases = new Dictionary<string, DatabaseItem>();
            Jwt = new JwtItem();
            Redis = new RedisItem();
            Consul = new ConsulItem();
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDictionary<string, DatabaseItem> Databases { get; set; }
        /// <summary>
        /// JSON WEB TOKEN配置
        /// </summary>
        public JwtItem Jwt { get; set; }
        /// <summary>
        /// Redis 配置
        /// </summary>
        public RedisItem Redis { get; set; }
        /// <summary>
        /// Consul 配置
        /// </summary>
        public ConsulItem Consul { get; set; }
    }

    /// <summary>
    /// 数据库配置，支持1主多从/读写分离
    /// </summary>
    public class DatabaseItem
    {
        /// <summary>
        /// 是否集群
        /// </summary>
        public bool Cluster { get; set; }
        /// <summary>
        /// 主库连接
        /// </summary>
        public string Master { get; set; }
        /// <summary>
        /// 从库连接配置数组
        /// </summary>
        public List<string> Slaves { get; set; }
        /// <summary>
        /// 从库负载均衡策略类型 0：随机，1：权重，2：轮询，默认随机
        /// </summary>
        public int LoadBalanceType { get; set; }
        /// <summary>
        /// 从库连接
        /// </summary>
        public string Slave
        {
            get
            {
                if (!Cluster) return string.Empty;
                if (Slaves == null || Slaves.Count <= 0) throw new Exception("数据库集群模式下，未配置从库连接");
                // 随机
                if (LoadBalanceType == 0)
                {
                    Random random = CreateRandom();
                    return Slaves[random.Next(0, Slaves.Count)];
                }
                // 权重（实质仍然是随机取）
                if(LoadBalanceType == 1)
                {
                    List<string> dbs = new List<string>();
                    foreach(var item in Slaves)
                    {
                        var match = Regex.Match(item, @".+?_weight=(\d+)", RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            var weight = Convert.ToInt32(match.Groups[1].Value);
                            for(int i = 0; i< weight; i++)
                            {
                                dbs.Add(item);
                            }
                        }
                    }

                    Random random = CreateRandom();
                    return dbs[random.Next(0, dbs.Count)];
                }
                // 轮询 从索引0开始取模
                if(LoadBalanceType == 2)
                {
                    var value = Interlocked.Increment(ref _index);
                    string dbString = Slaves[value % Slaves.Count];
                    if (_index > 1000) _index = 0;
                    return dbString;
                }

                return Slaves[0];
            }
        }

        /// <summary>
        ///  强随机数
        /// </summary>
        /// <returns></returns>
        Random CreateRandom()
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
        /// 轮询计数器
        /// </summary>
        int _index = 0;
    }

    /// <summary>
    /// Jwt 配置项
    /// </summary>
    public class JwtItem
    {
        /// <summary>
        /// 是否验证颁发者
        /// </summary>
        public bool ValidateIssuer { get; set; } = true;
        /// <summary>
        /// 颁发者
        /// </summary>
        public string ValidIssuer { get; set; } = string.Empty;
        /// <summary>
        /// 是否验证签名
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; } = true;
        /// <summary>
        /// 是否验证受用者
        /// </summary>
        public bool ValidateAudience { get; set; } = true;
        /// <summary>
        /// 受用者
        /// </summary>
        public string ValidAudience { get; set; } = string.Empty;
        /// <summary>
        /// 验证时间偏差（单位：秒）
        /// </summary>
        public int ClockSkew { get; set; }
        /// <summary>
        /// 是否验证有效期
        /// </summary>
        public bool ValidateLifetime { get; set; } = true;
        // 签名算法类型 1：RSA256，2：HS256
        public int SecurityType { get; set; } = 2;
        /// <summary>
        /// 对称签名密钥 - 即 HS256
        /// </summary>
        public string SymmetricSecurityKey { get; set; } = "Calamus_$(@F342";
        /// <summary>
        /// 不对称签名私钥 - 即 RS256
        /// </summary>
        public string RsaPrivateKey { get; set; } = "";
        /// <summary>
        /// 不对称签名公钥 - 即 RS256
        /// </summary>
        public string RsaPublicKey { get; set; } = "";
    }

    /// <summary>
    /// Redis 配置项
    /// </summary>
    public class RedisItem
    {
        /// <summary>
        /// 连接字符串配置 例："127.0.0.1:6379, password=123456, name=oms_client, channelPrefix=oms:, defaultDatabase=0, abortConnect=false, allowAdmin=true, connectRetry=5"
        /// </summary>
        public string Configuration { get; set; }
        /// <summary>
        /// Key前缀 - Key命名格式建议：[系统名]:[模块名]:[具体键名]
        /// </summary>
        public string KeyPrefix { get; set; } = string.Empty;

        /// <summary>
        /// 滑动过期时间 单位：秒
        /// </summary>
        public long SlidingExpiration { get; set; } = -1;
    }

    /// <summary>
    /// Consul 配置项
    /// </summary>
    public class ConsulItem
    {
        #region Consul连接
        /// <summary>
        /// http连接地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 数据中心名称
        /// </summary>
        public string Datacenter { get; set; }
        /// <summary>
        /// 连接 token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 等待时间（单位/秒）
        /// </summary>
        public long WaitTime { get; set; }
        #endregion

        #region 服务信息
        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceId { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 服务IP
        /// </summary>
        public string ServiceIP { get; set; }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { get; set; }
        /// <summary>
        /// 服务健康监测路径
        /// </summary>
        public string HealthCheckPath { get; set; }
        #endregion
    }
}
