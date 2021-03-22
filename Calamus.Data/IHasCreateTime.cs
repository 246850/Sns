using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Data
{
    /// <summary>
    /// 创建时间接口
    /// </summary>
    public interface IHasCreateTime
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}
