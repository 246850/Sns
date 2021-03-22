using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace Calamus.AspNetCore.Users
{
    /// <summary>
    ///  注册 当前授权用户 IIdentityUser
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册 当前授权用户 IIdentityUser
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityUser<TKey, TUser>(this IServiceCollection services, Action<IIdentityUser<TKey, TUser>, ClaimsPrincipal> setup)
            where TKey : struct
            where TUser : class, new()
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IIdentityUser<TKey, TUser>>(serviceProvider =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var identity = httpContextAccessor.GetIdentityUser<TKey, TUser>(setup);
                return identity;
            });
            return services;
        }
    }
}
