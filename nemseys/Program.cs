using Nemesys.Models.Interfaces;
using Nemesys.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Nemesys.Models.Contexts;
using Nemesys.Models;

namespace Nemesys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Configure our services
            var builder = WebApplication.CreateBuilder(args);

            //This service could be varied by environment - passing different connection strings as required (by different repositories)
            builder.Services.AddDbContext<AppDbContext>(options =>
                 //Services configuration
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string for AppDbContext not found.")));

            //Configure Identity framework core
            //Thsee are only for illustration purposes (in reality you would add what you actually require)
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

                //Password policy
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                //Lockout policy
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                //User settings
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();


            builder.Services.AddControllersWithViews();
            //Register the repository as a service
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddTransient<INemeseysRepository, NemesysRepository>();
            }

            //if (builder.Environment.IsProduction())
            //{
            //    //This would be returning an instance of a different repository (not MocksRepository)
            //    builder.Services.AddTransient<IBloggyRepository, BloggyRepository>();
            //}


            var app = builder.Build();

            //Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); 

            //Due to the fact that we're using ASP.Net Identity, which uses Razor Pages (MVVM)
            app.MapRazorPages();

            app.Run();
        }
    }
}
