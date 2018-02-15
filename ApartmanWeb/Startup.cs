using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApartmanWeb.Data;
using ApartmanWeb.Models;
using ApartmanWeb.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ApartmanWeb
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
                .AddSessionStateTempDataProvider();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            Task createRoles = CreateRoles(serviceProvider);
            createRoles.Wait();
            Task initializeAdmin = InitializeAdmin(serviceProvider);
            initializeAdmin.Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Member" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task InitializeAdmin(IServiceProvider serviceProvider)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var admin = new ApplicationUser
            {
                UserName = Configuration["AdminData:AdminName"],
                Email = Configuration["AdminData:AdminEmail"],
            };
            string userPWD = Configuration["AdminData:AdminPassword"];
   
            ApplicationUser appUser = await UserManager.FindByEmailAsync(admin.Email);

            if (appUser == null)
            {
                ApplicationUser newAppUser = new ApplicationUser
                {
                    Email = admin.Email,
                    UserName = admin.UserName
                };

                var taskCreateAppUser = await UserManager.CreateAsync(newAppUser, userPWD);
                appUser = newAppUser;
                
            }
            await UserManager.AddToRoleAsync(appUser, "Admin");
        }
    }
}
