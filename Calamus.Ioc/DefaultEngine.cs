using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Calamus.Ioc.EventBus;

namespace Calamus.Ioc
{
    internal class DefaultEngine : IEngine
    {
        #region 实现
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            // IHttpContextAccessor 注入
            services.AddHttpContextAccessor();
            // 应用程序通用配置选项
            services.AddOptions()
                .Configure<CalamusOptions>(options => { configuration.GetSection("Calamus").Bind(options); });
            IOptions<CalamusOptions> appsOptions = services.BuildServiceProvider().GetService<IOptions<CalamusOptions>>();

            // 程序集类型查找者-单例
            ITypeFinder typeFinder = new WebAppTypeFinder();
            services.AddSingleton(typeFinder);

            // 服务注册
            RegisterServices(services, configuration, env, appsOptions.Value, typeFinder);

            // IDependency接口 批量依赖注入
            RegisterDependency(services, typeFinder);

            // 事件消息/发布订阅
            RegisterConsumers(services, typeFinder);

            // 生成服务容器
            _serviceProvider = services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            // 所有实现 IAppStartup 接口实例，按顺序执行构建中间件管理
            ITypeFinder typeFinder = GetService<ITypeFinder>();
            IEnumerable<Type> startupTypes = typeFinder.FindClassesOfType<IHostStartup>();
            IEnumerable<IHostStartup> instances = startupTypes.Select(type => (IHostStartup)Activator.CreateInstance(type)).OrderBy(item => item.Order);

            foreach (IHostStartup instance in instances)
                instance.Configure(app, env);
        }

        public object GetService(Type type)
        {
            return ServiceProvider.GetService(type);
        }

        public T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return ServiceProvider.GetServices(type);
        }

        public IEnumerable<T> GetServices<T>() where T : class
        {
            return ServiceProvider.GetServices<T>();
        }

        public object GetRequiredService(Type type)
        {
            return ServiceProvider.GetRequiredService(type);
        }

        public T GetRequiredService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        private IServiceProvider _serviceProvider;
        protected IServiceProvider ServiceProvider
        {
            get
            {
                IHttpContextAccessor accessor = _serviceProvider.GetService<IHttpContextAccessor>();
                if (accessor == null) return _serviceProvider;

                HttpContext context = accessor.HttpContext;
                return context?.RequestServices ?? _serviceProvider;
            }
        }
        #endregion

        #region - 私有方法

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="apps"></param>
        /// <param name="typeFinder"></param>
        void RegisterServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env,
            CalamusOptions apps, ITypeFinder typeFinder)
        {
            // 所有实现 IApplicationStartup 接口实例，按顺序执行注册服务
            IEnumerable<Type> startupTypes = typeFinder.FindClassesOfType<IHostStartup>();
            IEnumerable<IHostStartup> instances = startupTypes.Select(type => (IHostStartup)Activator.CreateInstance(type)).OrderBy(item => item.Order);

            foreach (IHostStartup instance in instances)
                instance.ConfigureServices(services, configuration, env, apps, typeFinder);
        }

        /// <summary>
        /// IDependency接口 批量依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeFinder"></param>
        void RegisterDependency(IServiceCollection services, ITypeFinder typeFinder)
        {
            var types = typeFinder.FindClassesOfType<IDependency>(false);
            var interfaces = types.Where(t => t.IsInterface).ToList();
            var impements = types.Where(t => !t.IsAbstract).ToList();
            var didatas = interfaces
                .Select(t =>
                    {
                        return new
                        {
                            serviceType = t,
                            implementationType = impements.FirstOrDefault(c => t.IsAssignableFrom(c))
                        };
                    }
                ).ToList();

            didatas.ForEach(t =>
            {
                if (t.implementationType != null)
                    services.AddScoped(t.serviceType, t.implementationType);
            });
        }

        /// <summary>
        /// 发布/订阅（事件消息）
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeFinder"></param>
        void RegisterConsumers(IServiceCollection services, ITypeFinder typeFinder)
        {
            /********消费者********/
            var types = typeFinder.FindClassesOfType(typeof(IConsumer<>));
            var consumers = types.Where(t => !t.IsAbstract).ToList();

            foreach (Type consumer in consumers)
            {
                var inters = consumer.FindInterfaces((type, criteria) =>
                {
                    bool isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>));
                foreach (var inter in inters)
                {
                    services.AddTransient(inter, consumer);
                }
            }

            /********生产者********/
            services.AddSingleton<IEventPublisher, EventPublisher>();
        }

        #endregion
    }
}
