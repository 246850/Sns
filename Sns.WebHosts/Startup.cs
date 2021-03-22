using Calamus.AspNetCore.Attributes;
using Calamus.Infrastructure.TextJson;
using Calamus.Ioc;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sns.Domains;
using Sns.Domains.Entities;
using Sns.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Sns.WebHosts
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            HostEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<GenericResultFilterAttribute>();
                    options.Filters.Add<GlobalExceptionFilterAttribute>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    //options.JsonSerializerOptions.PropertyNamingPolicy = null;  // ������ʵ�� ����ĸ��Сд��
                    options.JsonSerializerOptions.WriteIndented = false;
                }).ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;// ��ֹĬ��ģ����֤�������
                })
            .AddFluentValidation(config =>  // ����ģ�Ͳ�����֤
            {
                //config.ValidatorFactory = new FluentValidatorFactory();
                config.RunDefaultMvcValidationAfterFluentValidationExecutes = true;    // false : ��ֹĬ��ģ����֤
                config.ValidatorOptions.CascadeMode = CascadeMode.Stop; // ��������֤����һ����������ֹͣ
                config.RegisterValidatorsFromAssemblyContaining<ArticleCreateOrUpdateValidator>();
            });
            // ��¼��Ȩ��֤
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>    // ǰ̨��¼
                {
                    options.LoginPath = "/account/login";
                    options.LogoutPath = "/account/logout";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);// Ĭ��30����Ч��
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = false;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = context =>
                        {
                            return Task.CompletedTask;
                        } 
                    };
                });
            ;
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));// �����������

            services.AddCalamus(Configuration, HostEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // cookie ��֤
            app.UseAuthentication();
            app.UseCookiePolicy();
            // ��Ȩ
            app.UseAuthorization();

            app.UseCalamus(env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Article}/{action=List}/{id?}");
            });
        }
    }
}
