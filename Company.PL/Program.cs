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
using Microsoft.EntityFrameworkCore;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder Builder = WebApplication.CreateBuilder(args);


            Builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();

            Builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.LogoutPath = "/Account/SignOut";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            Builder.Services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            }).AddGoogle(o =>
            {
                o.ClientId = Builder.Configuration["Authentication:Google:ClientId"];
                o.ClientSecret = Builder.Configuration["Authentication:Google:ClientSecret"];
            });

            //Builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = IdentityConstants.ApplicationScheme;
            //    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            //})
            //.AddFacebook(o =>
            //{
            //    o.AppId = Builder.Configuration["Authentication:Facebook:AppId"];
            //    o.AppSecret = Builder.Configuration["Authentication:Facebook:AppSecret"];
            //});

            // 2. Register Database Context
            Builder.Services.AddDbContext<CompanyDbContext>(options =>
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // 3. Register Repositories & Services
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Builder.Services.AddScoped<IScopedService, ScopedService>();
            Builder.Services.AddTransient<ITransientService, TransientService>();
            Builder.Services.AddSingleton<ISingletonService, SingletonService>();

            // 4. Configure Mail & Twilio Services
            Builder.Services.Configure<MailSettings>(Builder.Configuration.GetSection(nameof(MailSettings)));
            Builder.Services.AddScoped<IMailService, MailService>();
            Builder.Services.Configure<TwilioSettings>(Builder.Configuration.GetSection(nameof(TwilioSettings)));
            Builder.Services.AddScoped<ITwilioService, TwilioService>();

            // 5. Register AutoMapper
            Builder.Services.AddAutoMapper(config => config.AddProfile(new EmployeeProfile()));

            // 6. Register MVC Controllers
            Builder.Services.AddControllersWithViews();

            WebApplication App = Builder.Build();

            // 7. Middleware Configuration
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

            // 8. Define Default Route
            App.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            App.Run();
        }


    }
}
