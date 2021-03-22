using Calamus.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calamus.AspNetCore.FluentValidator
{

    /// <summary>
    /// 请求模型绑定验证，支持依赖注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ModelValidatorFilterAttribute : TypeFilterAttribute
    {
        public ModelValidatorFilterAttribute() : base(typeof(ModelValidatorFilter))
        {

        }
        public class ModelValidatorFilter : ActionFilterAttribute
        {
            private readonly IHostEnvironment _hostEnvironment;
            public ModelValidatorFilter(IHostEnvironment hostEnvironment)
            {
                _hostEnvironment = hostEnvironment;
            }
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                if (!context.ModelState.IsValid)
                {
                    if (_hostEnvironment.IsDevelopment())
                    {
                        StringBuilder stringBuilder = new StringBuilder(256);
                        foreach (KeyValuePair<string, ModelStateEntry> item in context.ModelState)
                        {
                            if (item.Value.ValidationState != ModelValidationState.Valid && item.Value.Errors.Count > 0)
                            {
                                stringBuilder.AppendFormat("{0}；", item.Value.Errors.First().ErrorMessage);
                            };
                        }
                        context.Result = new JsonResult(new CodeResult(DefaultCode.ParameterError, stringBuilder.Replace("；", string.Empty, stringBuilder.Length - 1, 1).ToString()));
                    }
                    else
                    {
                        context.Result = new JsonResult(CodeResult.ParameterError);
                    }
                }
                base.OnActionExecuting(context);
            }

            public override void OnActionExecuted(ActionExecutedContext context)
            {
                base.OnActionExecuted(context);
            }
        }
    }
}
