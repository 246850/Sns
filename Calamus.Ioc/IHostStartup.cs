using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calamus.Ioc
{
    /// <summary>
    /// 应用程序启动执行 接口
    /// </summary>
    public interface IHostStartup
    {
        /// <summary>
        /// 注册服务 - DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        /// <param name="calamusOptions"></param>
        /// <param name="typeFinder"></param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder);
        /// <summary>
        /// 使用服务（中间件请求管道） - Middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        void Configure(IApplicationBuilder app, IHostEnvironment env);

        /// <summary>
        /// 执行顺序 从小到大
        /// </summary>
        int Order { get; }
    }
}
