using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nemesis.Interfaces;
using Nemesis.Models.Contexts;
using Nemesis.Models.Interfaces;
using Nemesis.Models.Repositories;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext and specify connection string
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the IReportRepository service with its implementation
builder.Services.AddTransient<IReportRepository, ReportRepository>();

builder.Services.AddControllersWithViews();

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Adjust expiration time as needed
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.IsHttps)
    {
        context.Request.HttpContext.Features.Get<ITlsConnectionFeature>().ClientCertificate = null;
    }
    await next();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "register",
        pattern: "/register",
        defaults: new { controller = "Account", action = "Signup" });

    endpoints.MapControllerRoute(
        name: "signin",
        pattern: "/signin",
        defaults: new { controller = "Home", action = "SignIn" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
