using Calamus.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.AspNetCore.Attributes
{
    /// <summary>
    /// 通用执行结果包装处理过滤器
    /// </summary>
    public class GenericResultFilterAttribute: IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is EmptyResult)
            {
                context.Result = new JsonResult(CodeResult.Success);
            }
            else if (context.Result is ContentResult)
            {
            }
            else if (context.Result is ObjectResult temp)
            {
                if ((temp.Value as CodeResult) != null)
                {
                    return;
                }
                context.Result = new JsonResult(temp.Value.Success());
            }
        }
    }
}
