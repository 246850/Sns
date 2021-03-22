using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Calamus.Ioc
{
    /// <summary>
    /// 请求管道构建扩展
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用Calamus
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void UseCalamus(this IApplicationBuilder app, IHostEnvironment env)
        {
            EngineContext.Current.Configure(app, env);
        }
    }
}
