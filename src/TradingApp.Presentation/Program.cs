using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using TradingApp.Core.Enums;
using TradingApp.Core.Models.Managers;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Repositories;
using TradingApp.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/User/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins", p =>
    {
        p.RequireRole(ClaimTypes.Role, UserRolesEnum.Admin.ToString());
    });
});

string connectionStringKey = "TradingAppDb";
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

if (string.IsNullOrEmpty(connectionString))
{
    throw new NullReferenceException($"No connection string found in appsettings.json with a key '{connectionStringKey}'");
}

builder.Services.Configure<ConnectionManager>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<LogManager>(builder.Configuration.GetSection("LoggerManager"));

builder.Services.AddScoped<IStockRepository, StockSqlRepository>();
builder.Services.AddScoped<IUserRepository, UserSqlRepository>();
builder.Services.AddScoped<IUserStockRepository, UserStockSqlRepository>();
builder.Services.AddScoped<ILogRepository, LogSqlRepository>();

builder.Services.AddTransient<LogMiddleware>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Stock/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LogMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

