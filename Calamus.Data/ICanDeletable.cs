using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Data
{
    /// <summary>
    /// 可软删除接口
    /// </summary>
    public interface ICanDeletable
    {
        /// <summary>
        /// true：已软删除
        /// </summary>
        bool Deleted { get; set; }
    }
}
