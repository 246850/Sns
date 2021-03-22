using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.Infrastructure.Models
{
    /// <summary>
    /// 用于请求参数，IdentityId
    /// </summary>
    public interface IHasIdentity<TKey> where TKey:struct
    {
        /// <summary>
        /// 请求认证ID
        /// </summary>
        TKey IdentityId { get; set; }
    }
}
