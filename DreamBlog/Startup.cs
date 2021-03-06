using AspNetCore.SEOHelper;
using DreamBlog.Authorization;
using DreamBlog.BusinessManagers;
using DreamBlog.BusinessManagers.Interfaces;
using DreamBlog.Data;
using DreamBlog.Data.Models;
using DreamBlog.Service;
using DreamBlog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DreamBlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //AppService
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Add Reference With Contract Ashish
            services.AddScoped<IBlogBusinessManager, BlogBusinessManager>();
            services.AddScoped<IBlogServices, BlogServices>();
            services.AddScoped<IAdminBusinessManager, AdminBusinessManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHomeBusinessManager, HomeBusinessManager>();



            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddTransient<IAuthorizationHandler, BlogAuthorizationHandler>();
            //services.AddCustomServices();
        }

        //public static void AddCustomServices(this IServiceCollection serviceCollection)
        //{
        //    serviceCollection.AddScoped<IBlogBusinessManager, IBlogBusinessManager>();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //AppConfigure
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            if (env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();


            //routing for sitemap.txt
            app.UseXMLSitemap(env.ContentRootPath);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
