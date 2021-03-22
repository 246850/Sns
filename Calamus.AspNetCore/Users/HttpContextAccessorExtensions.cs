using Calamus.Infrastructure.Models;
using Calamus.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Calamus.AspNetCore.Users
{
    /// <summary>
    /// IHttpContextAccessor 扩展类
    /// </summary>
    public static class HttpContextAccessorExtensions
    {
        /// <summary>
        /// 获取当前 授权用户信息
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="httpContextAccessor"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static IIdentityUser<TKey, TUser> GetIdentityUser<TKey, TUser>(this IHttpContextAccessor httpContextAccessor, Action<IIdentityUser<TKey, TUser>, ClaimsPrincipal> setup)
            where TKey : struct
            where TUser : class, new()
        {
            DefaultIdentityUser<TKey, TUser> user = new DefaultIdentityUser<TKey, TUser>()
            {
                UserInfo = new TUser()
            };

            setup(user, httpContextAccessor.HttpContext.User);

            return user;

        }
    }
}
