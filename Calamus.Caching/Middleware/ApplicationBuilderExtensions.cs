using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Calamus.Caching.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用Redis服务器信息中间件 默认分支路由：/redis_info
        /// </summary>
        /// <param name="app"></param>
        public static void UseRedisInformation(this IApplicationBuilder app)
            => UseRedisInformation(app, "/redis_info", null);

        /// <summary>
        /// 使用Redis服务器信息中间件 自定义分支路由
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path">分支路由</param>
        /// <param name="setup">配置项</param>
        public static void UseRedisInformation(this IApplicationBuilder app, [NotNull] PathString path, Action<RedisInformationOptions> setup)
        {
            RedisInformationOptions options = new RedisInformationOptions();
            setup?.Invoke(options);

            Func<HttpContext, bool> predicate = context =>
            {
                return context.Request.Path.StartsWithSegments(path, out var remaining);
            };
            app.MapWhen(predicate, b => b.UseMiddleware<RedisInformationMiddleware>(Options.Create(options)));
        }
    }
}
