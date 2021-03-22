using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Result
{
    /// <summary>
    /// 默认 状态码 值
    /// </summary>
    public sealed class DefaultCode
    {
        /// <summary>
        /// 参数验证错误状态码
        /// </summary>
        public static readonly int ParameterError = 101;
        /// <summary>
        /// 未授权认证
        /// </summary>
        public static readonly int UnAuthorizeError = 401;
        /// <summary>
        /// 服务器内部未处理异常状态码
        /// </summary>
        public static readonly int ServerError = 501;
    }
}
