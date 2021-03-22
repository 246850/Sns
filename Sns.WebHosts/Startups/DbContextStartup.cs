using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Sns.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sns.WebHosts.Startups
{
    public class DbContextStartup : IHostStartup
    {
        public int Order => -99;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            services.AddDbContext<SnsdbContext>(options =>
            {
                options.UseMySql(calamusOptions.Databases["MySql"].Master, mysql =>
                {
                    mysql.CharSet(CharSet.Utf8Mb4);
                });
            });
        }
    }
}
