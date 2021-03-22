using Calamus.AspNetCore.Users;
using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sns.Models;
using StackExchange.Profiling.SqlFormatters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Sns.WebHosts.Startups
{
    public class CommonStartup : IHostStartup
    {
        public int Order => 0;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseMiniProfiler();
            }
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            services.AddIdentityUser<int, LoginAuthModel>((user, principal) =>
            {
                if (!principal.Identity.IsAuthenticated) return;    // 未授权返回

                string sub = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
                LoginAuthModel model = System.Text.Json.JsonSerializer.Deserialize<LoginAuthModel>(sub);
                user.Name = model.NickName;
                user.Account = model.Account;
                user.Avatar = model.Avatar;
                user.Id = Convert.ToInt32(principal.FindFirstValue("id"));
            });

            // Services
            var types = typeFinder.FindClassesOfType<IDependency>(false);
            var interfaces = types.Where(t => t.IsInterface && t != typeof(IDependency)).ToList();
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

            // AutoMapper
            services.AddAutoMapper(typeof(AutomapperProfiler).Assembly);

            // MiniProfiler
            if (env.IsDevelopment())
            {
                services.AddMiniProfiler(options =>
                {
                    options.SqlFormatter = new SqlServerFormatter()
                    {
                        IncludeParameterValues = true
                    };
                    options.RouteBasePath = "/profiler";
                }).AddEntityFramework();
            }

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
        }
    }
}
