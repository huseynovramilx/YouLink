using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Data;
using LinkShortener.Models;
using LinkShortener.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShortener {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.Configure<CookiePolicyOptions> (options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseSqlServer (
                    Configuration.GetConnectionString ("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole> ()
                .AddDefaultUI (UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext> ()
                .AddDefaultTokenProviders();
            /* services.AddDefaultIdentity<ApplicationUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>(); */

            services.AddOptions();
            services.Configure<AppOptions>(Configuration);

            services.AddMvc()
                .SetCompatibilityVersion (CompatibilityVersion.Version_2_2);


            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddSingleton (factory => new PayPalHttpClientFactory (
                Configuration["Paypal:ClientId"],
                Configuration["Paypal:ClientSecret"],
                Convert.ToBoolean (Configuration["Paypal:IsLive"])
            ));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider) {
           // if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseDatabaseErrorPage ();
           /* } else {
                app.UseExceptionHandler ("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            */
            app.UseHttpsRedirection ();
            app.UseStaticFiles ();
            app.UseCookiePolicy ();
            CreateRoles(provider);
            app.UseAuthentication ();
            
            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id:maxlength(4)?}");

            });

        }

        private void CreateRoles (IServiceProvider serviceProvider) {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>> ();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>> ();
            string[] roleNames = { "Admin", "Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames) {
                var roleExist = RoleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist) {
                    //create the roles and seed them to the database: Question 1
                    roleResult = RoleManager.CreateAsync(new IdentityRole (roleName)).Result;
                }
            }

            //Here you could create a super user who will maintain the web app
            var poweruser = new ApplicationUser {

                UserName = Configuration["AppSettings:Admin:UserName"],
                Email = Configuration["AppSettings:Admin:Email"],
            };
            //Ensure you have these values in your appsettings.json file
            string userPWD = Configuration["AppSettings:Admin:Password"];
            var _user = UserManager.FindByEmailAsync(Configuration["AppSettings:Admin:Email"]).Result;

            if (_user == null) {
                var createPowerUser = UserManager.CreateAsync (poweruser, userPWD).Result;
                if (createPowerUser.Succeeded) {
                    //here we tie the new user to the role
                    UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }
    }
}