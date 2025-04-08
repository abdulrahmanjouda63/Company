using Company.BLL;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.Mapping;
using Company.PL.Services;
using Company.PL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder Builder = WebApplication.CreateBuilder(args);

            // 1. Configure Services
            // ------------------------------------------------------------------------

            // 1.1 Configure Identity
            Builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();

            // 1.2 Configure Authentication Cookies
            Builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.LogoutPath = "/Account/SignOut";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            // 1.3 Configure Authentication Providers
            Builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            })
            .AddGoogle(options =>
            {
                options.ClientId = Builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Builder.Configuration["Authentication:Google:ClientSecret"];
            });

            /*
            // Facebook login (commented out for now)
            .AddFacebook(options =>
            {
                options.AppId = Builder.Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Builder.Configuration["Authentication:Facebook:AppSecret"];
            });
            */

            // 1.4 Configure EF Core and Database Context
            Builder.Services.AddDbContext<CompanyDbContext>(options =>
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // 1.5 Register Application Services
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Builder.Services.AddScoped<IScopedService, ScopedService>();
            Builder.Services.AddTransient<ITransientService, TransientService>();
            Builder.Services.AddSingleton<ISingletonService, SingletonService>();

            // 1.6 Configure External Services (Mail, Twilio)
            Builder.Services.Configure<MailSettings>(Builder.Configuration.GetSection(nameof(MailSettings)));
            Builder.Services.AddScoped<IMailService, MailService>();

            Builder.Services.Configure<TwilioSettings>(Builder.Configuration.GetSection(nameof(TwilioSettings)));
            Builder.Services.AddScoped<ITwilioService, TwilioService>();

            // 1.7 Register AutoMapper
            Builder.Services.AddAutoMapper(config => config.AddProfile(new EmployeeProfile()));

            // 1.8 Register MVC Controllers
            Builder.Services.AddControllersWithViews();

            // 2. Build Application
            WebApplication App = Builder.Build();

            // 3. Configure Middleware
            // ------------------------------------------------------------------------

            if (!App.Environment.IsDevelopment())
            {
                App.UseExceptionHandler("/Home/Error");
                App.UseHsts();
            }

            App.UseHttpsRedirection();
            App.UseStaticFiles();

            App.UseRouting();

            App.UseAuthentication();
            App.UseAuthorization();

            // 4. Configure Endpoints
            // ------------------------------------------------------------------------

            App.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            // 5. Run Application
            App.Run();

        }


    }
}
