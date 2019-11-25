using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CocktailMagician.Infrastructure.Extensions;
using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ServiceLayer;
using ServiceLayer.Contracts;

namespace CocktailMagician
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHashing, Hashing>();
            services.AddDbContext<CocktailDatabaseContext>();
            services.AddScoped<IBarService, BarService>();
            services.AddScoped<ICocktailService, CocktailService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<INotificationService, NotificationService>();

            services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                //app.UseExceptionHandler("/Error/Index");
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler("/Error/Index");
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseWrongRouteHandler();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Administration",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "Magician",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "errorpage",
                    template: "errorPage",
                    defaults: new { controller = "Error", action = "Index" });
                routes.MapRoute(
                    name: "notfound",
                    template: "404",
                    defaults: new { controller = "Error", action = "PageNotFound" });
            });
        }
    }
}
