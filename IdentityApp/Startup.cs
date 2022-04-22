using IdentityApp.ClaimProviders;
using IdentityApp.CustomValidation;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp
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
            services.AddTransient<IAuthorizationHandler, ExpireDateExchangeHandler>(); // IAuthorization.. her karşılaştığında ExpireDateExchangeHandler nese örneğini oluştursun
            services.AddDbContext<AppIdentityDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:DefaultConnectionString"]);
            });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("AnkaraPolicy", policy =>
                {
                    policy.RequireClaim("city", "Ankara");
                });
                opts.AddPolicy("ViolencePolicy", policy =>
                {
                    policy.RequireClaim("violence");
                });

                opts.AddPolicy("ExchangePolicy", policy =>
                 {
                     policy.AddRequirements(new ExpireDateExchangeRequirement());
                 });
            });


            services.AddAuthentication().AddFacebook(opts=>
            {
                opts.AppId = Configuration["Authentication:Facebook:AppId"];
                opts.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            }).AddGoogle(opts=>
            {
                opts.ClientId = Configuration["Authentication:Google:ClientID"];
                opts.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });

            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                //opts.User.AllowedUserNameCharacters = "abcçdefgğhiıjklmnoöpqrsştuüvwxyzABCDEFGHIİJKLMNOPQRSTUVWXYZ-._";
                opts.Password.RequiredLength = 5;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireDigit = false;

            }).AddPasswordValidator<CustomPasswordValidator>().AddUserValidator<CustomUserValidator>()
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();


            CookieBuilder cookieBuilder = new CookieBuilder();

            cookieBuilder.Name = "MyBlog";
            cookieBuilder.HttpOnly = false;
            cookieBuilder.SameSite = SameSiteMode.Lax;
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;

            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = new PathString("/Home/Login");
                opts.LogoutPath = new PathString("/Member/LogOut");
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true;
                opts.ExpireTimeSpan = System.TimeSpan.FromDays(60);
                opts.AccessDeniedPath = new PathString("/Member/AccessDenied");
            });
            services.AddScoped<IClaimsTransformation, ClaimProvider>();

            services.AddRazorPages();
            services.AddMvc();

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{Action=Index}/{id?}"
                    );
            });
                
        }
    }
}
