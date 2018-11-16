using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DAL;
using WebApp.Services;
using WebAppCore.Services;

namespace WebAppCore
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton(typeof(MemoryCacheManager));
            services.AddSingleton<IStringLocalizer>(s => new JsonLocalizationService(s.GetService<IHostingEnvironment>(), "Resources"));
            services.AddSingleton<IStringLocalizerFactory, JsonLocalizerFactory>();

            bool condition = true;
            services.AddSingleton<ISimple>(c => { if (!condition) return new ClassA(); else return new ClassB(); });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            // можно и через singleton
            services.AddTransient<ContextCheckingService>();
            services.AddMemoryCache();
            //services.AddSingleton<PersonsCachingService>();
            services.AddSingleton(typeof(DbSetCachingService<>));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(typeof(LocalizationService));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

           // return services.BuildServiceProvider();
        }
    }
}
