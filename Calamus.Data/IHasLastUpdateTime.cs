using System;

namespace Calamus.Data
{
    /// <summary>
    /// 最后更新时间接口
    /// </summary>
    public interface IHasLastUpdateTime
    {
        /// <summary>
        /// 最后更新时间
        /// </summary>
        DateTime LastUpdateTime { get; set; }
    }
}
