using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.Data
{
    /// <summary>
    /// 实体 - 关系 映射的 默认 公共字段
    /// </summary>
    public sealed class EntityFieldDefaults
    {
        /// <summary>
        /// 创建时间字段 CreateTime
        /// </summary>
        public const string CreateTime = "CreateTime";
        /// <summary>
        /// 创建人字段
        /// </summary>
        public const string CreateBy = "CreateBy";
        /// <summary>
        /// 最后更新时间字段 LastUpdateTime
        /// </summary>
        public const string LastUpdateTime = "LastUpdateTime";
        /// <summary>
        /// 最后更新人字段
        /// </summary>
        public const string LastUpdateBy = "LastUpdateBy";
        /// <summary>
        /// 是否删除字段 Deleted
        /// </summary>
        public const string Deleted = "Deleted";
    }
}
