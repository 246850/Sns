using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Calamus.Ioc
{
    /// <summary>
    /// Ioc 引擎
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// 注册服务 - DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env);
        /// <summary>
        /// 使用服务 - Middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        void Configure(IApplicationBuilder app, IHostEnvironment env);

        /// <summary>
        /// 获取服务 - 单个
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetService(Type type);
        /// <summary>
        /// 获取服务泛型 - 单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>() where T : class;
        /// <summary>
        /// 获取服务 - 多个
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<object> GetServices(Type type);
        /// <summary>
        /// 获取服务泛型 - 多个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetServices<T>() where T : class;

        /// <summary>
        /// 获取服务 - 单个 | 会抛异常
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetRequiredService(Type type);

        /// <summary>
        /// 获取服务泛型 - 单个 | 会抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetRequiredService<T>();
    }
}
