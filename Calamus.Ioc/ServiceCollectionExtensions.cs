using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calamus.Ioc
{
    /// <summary>
    /// 服务容器注册 扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Calamus服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public static void AddCalamus(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            EngineContext.Current.ConfigureServices(services, configuration, env);
        }
    }
}
