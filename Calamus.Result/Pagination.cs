using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Calamus.Result
{
    /// <summary>
    /// 分页Json结果包装类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pagination<T> where T : class
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public IEnumerable<T> List { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 聚合统计
        /// </summary>
        public IDictionary<string, object> Aggregates { get; set; }
    }
}
