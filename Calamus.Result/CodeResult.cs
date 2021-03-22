using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Result
{
    /// <summary>
    /// Code | Msg 结果
    /// </summary>
    public class CodeResult
    {
        public CodeResult() : this(0, "成功 - Success")
        {

        }

        public CodeResult(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 提示语
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 默认成功结果
        /// </summary>
        public static CodeResult Success
        {
            get
            {
                return new CodeResult();
            }
        }
        /// <summary>
        /// 默认失败结果
        /// </summary>
        public static CodeResult Failed
        {
            get
            {
                return new CodeResult(-1, "失败 - Failed");
            }
        }
        /// <summary>
        /// 请求模型参数验证失败结果 101
        /// </summary>
        public static CodeResult ParameterError
        {
            get
            {
                return new CodeResult(DefaultCode.ParameterError, "请求参数验证错误");
            }
        }
        /// <summary>
        /// 授权认证失败结果 401
        /// </summary>
        public static CodeResult UnAuthorizeError
        {
            get
            {
                return new CodeResult(DefaultCode.UnAuthorizeError, "授权认证失败");
            }
        }
        /// <summary>
        /// 服务器内部异常结果 501
        /// </summary>
        public static CodeResult InternalServerError
        {
            get
            {
                return new CodeResult(DefaultCode.ServerError, "服务器内部异常，请稍候再试");
            }
        }
    }
}
