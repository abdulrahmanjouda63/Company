using Company.BLL;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.Mapping;
using Company.PL.Services;
using Company.PL.Settings;
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
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register built-in MVC services
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow DI For DepartmentRepository
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Allow DI For EmployeeRepository
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Allow DI For UnitOfWork
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection(nameof(TwilioSettings)));
            builder.Services.AddScoped<ITwilioService, TwilioService>();
            // Life Time
            //builder.Services.AddScoped();     // Create Object Life Time Per Request - Unreachable Object
            //builder.Services.AddTransient();  // Create Object Life Time Per Operation - Reachable Object
            //builder.Services.AddSingleton();  // Create Object Life Time Per Application

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                //config.LogoutPath = "/Home/SignIn";
                //config.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                //config.AccessDeniedPath = "/Account/AccessDenied";

            });

            builder.Services.AddAuthentication(o => {
                o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddGoogle(o =>
            {
                o.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
            }).AddFacebook(o =>
            {
                o.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                o.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
