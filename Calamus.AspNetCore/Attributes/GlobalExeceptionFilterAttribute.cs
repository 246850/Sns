using Calamus.Infrastructure.Models;
using Calamus.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Calamus.AspNetCore.Attributes
{
    /// <summary>
    /// 全局异常处理过滤器 - 支持依赖注入 - TypeFilter
    /// </summary>
    public class GlobalExceptionFilterAttribute : TypeFilterAttribute
    {
        public GlobalExceptionFilterAttribute() : base(typeof(GlobalExeceptionFilter))
        {

        }

        private class GlobalExeceptionFilter : IExceptionFilter
        {
            private readonly ILogger<GlobalExeceptionFilter> _logger;
            private readonly IHostEnvironment _hostEnvironment;
            public GlobalExeceptionFilter(ILogger<GlobalExeceptionFilter> logger,
                IHostEnvironment hostEnvironment)
            {
                _logger = logger;
                _hostEnvironment = hostEnvironment;
            }
            public void OnException(ExceptionContext context)
            {
                Exception exception = context.Exception;
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
                _logger.LogError(exception, exception.Message);

                if (exception is CodeException temp)
                {
                    context.ExceptionHandled = true;
                    context.Result = new JsonResult(temp.Code.ToResult(temp.Message));
                    return;
                }

                if (!_hostEnvironment.IsDevelopment())
                {
                    context.ExceptionHandled = true;
                    context.Result = new JsonResult(CodeResult.InternalServerError);
                }
                //context.Result = new JsonResult(new { TargetSite = exception.TargetSite.ToString(), Source = exception.Source, StackTrace = $"{exception.GetType().FullName}：{exception.StackTrace}" }.ToError(DefaultCode.ServerError, exception.Message));
            }
        }
    }
}
