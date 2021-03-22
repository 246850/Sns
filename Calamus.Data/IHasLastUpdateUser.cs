using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Data
{
    /// <summary>
    /// 最后更新人
    /// </summary>
    public interface IHasLastUpdateUser
    {
        /// <summary>
        /// 最后更新人
        /// </summary>
        string LastUpdateUser { get; set; }
    }
}
