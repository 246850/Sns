using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Result
{
    /// <summary>
    /// 携带错误信息结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ErrorResult<T> : CodeResult
    {
        public ErrorResult():this(DefaultCode.ServerError, "Server Error", default(T))
        {

        }
        public ErrorResult(int code , string msg, T error):base(code, msg)
        {
            Error = error;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public T Error { get; set; }
    }
}
