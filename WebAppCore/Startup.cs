using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WebAppCore.Models;
using WebAppCore.Services;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.AspNetCore.Http;
using System.Globalization;
//using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebApp.DAL;
using WebApp.Services;

namespace WebAppCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

            

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
         //   env.ConfigureNLog("nlog.config");
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors();
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            /*
                        services.AddAuthentication().AddGoogle(googleOptions =>
                        {
                            googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                            googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                        });
                        */
            //  services.Configure<MySettings>(Configuration.GetSection("MySettings"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationServices(Configuration);
            ServiceLocator.SetLocatorProvider(services.BuildServiceProvider());

             services.AddResponseCaching();


            services.AddLocalization(options => options.ResourcesPath = "Resources");
            

            services.AddHostedService<TimedHostedService>();


            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {

                options.IdleTimeout = TimeSpan.FromSeconds(30);

            });

            services.AddMvc(o => { o.Filters.Add<GlobalExceptionFilter>(); }).AddViewLocalization()
                 .AddDataAnnotationsLocalization(options => {
                     options.DataAnnotationLocalizerProvider = (type, factory) =>
                         factory.Create(null);
                 })
                //AddDataAnnotationsLocalization(o=>)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1); ;



            // Adds a default in-memory implementation of IDistributedCache.
           

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // отключаю стандартный log, т.к. он неи работает
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
               loggerFactory.AddDebug();
            // loggerFactory.AddEventLog();


            // логирую в файл C:\Temp\nlog

            


           // loggerFactory.AddNLog();

            //add NLog.Web
          //  app.AddNLogWeb();

           
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru"),
                new CultureInfo("de")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
           /* var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value); */

            app.UseResponseCaching();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
      //          app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSession();

         

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            // app.UseSession(o => o.IdleTimeout = TimeSpan.FromSeconds(10));

           
            // Пример переключения 
            
            app.Use(async (context, next) =>
            {
                
               var _dbContext= context.RequestServices.GetService<ApplicationDbContext>();
               var product= _dbContext.Set<Product>().AsNoTracking().First();
                string culture = product.Id == 1 ? "ru": "en";

                var requestCulture = new RequestCulture(culture);
                context.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, new QueryStringRequestCultureProvider()));
                CultureInfo.CurrentCulture = requestCulture.Culture;
                CultureInfo.CurrentUICulture = requestCulture.UICulture;
                await next.Invoke();
                //    await context.Response.WriteAsync("<p>End Hello world0!</p>");
            }); 
            app.UseHello();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
            /*
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            }); */

            app.UseMvcWithDefaultRoute();
        }
    }
}
