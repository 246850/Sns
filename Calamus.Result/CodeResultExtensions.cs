using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.Result
{
    /// <summary>
    /// Code结果扩展
    /// </summary>
    public static class CodeResultExtensions
    {
        /// <summary>
        /// 根据 Boolean 创建默认结果对象：True成功，False失败
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static CodeResult ToResult(this bool source)
        {
            return source ? CodeResult.Success : CodeResult.Failed;
        }
        /// <summary>
        /// 根据 Code 创建结果对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static CodeResult ToResult(this int code, string msg)
        {
            return new CodeResult(code, msg);
        }

        /// <summary>
        /// 根据 模型TModel对象 创建 成功 结果对象 - 默认提示语
        /// </summary>
        /// <typeparam name="TModel">业务模型类型</typeparam>
        /// <param name="model">数据载体对象</param>
        /// <returns></returns>
        public static CodeResult<TModel> Success<TModel>(this TModel model) where TModel : class
        {
            return new CodeResult<TModel> { Data = model };
        }

        /// <summary>
        /// 根据 模型TModel对象 创建 成功 结果对象 - 自定义提示语
        /// </summary>
        /// <typeparam name="TModel">业务模型类型</typeparam>
        /// <param name="model">数据载体对象</param>
        /// <param name="msg">自定义提示语</param>
        /// <returns></returns>
        public static CodeResult<TModel> Success<TModel>(this TModel model, string msg) where TModel : class
        {
            return new CodeResult<TModel> { Data = model, Msg = msg };
        }

        /// <summary>
        /// 根据 模型TModel对象 创建 结果对象 - 自定义Code/提示语
        /// </summary>
        /// <typeparam name="TModel">业务模型类型</typeparam>
        /// <param name="model">数据载体对象</param>
        /// <param name="code">自定义Code</param>
        /// <param name="msg">自定义提示语</param>
        /// <returns></returns>
        public static CodeResult<TModel> ToResult<TModel>(this TModel model, int code, string msg) where TModel : class
        {
            return new CodeResult<TModel> { Data = model, Code = code, Msg = msg };
        }

        /// <summary>
        /// 构建 错误信息 结果
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>

        public static ErrorResult<TModel> ToError<TModel>(this TModel model, int code, string msg)
        {
            return new ErrorResult<TModel>(code, msg, model);
        }
    }
}
