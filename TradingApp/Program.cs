using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using TradingApp.Middlewares;
using TradingApp.Models;
using TradingApp.Models.Managers;
using TradingApp.Repositories;
using TradingApp.Repositories.Base;
using TradingApp.Repositories.Base.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDataProtection();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
        options.Cookie.Name = "TradingApp.Auth";
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

const string connectionStringKey = "DefaultConnectionString";
string? connectionString = builder.Configuration.GetConnectionString(connectionStringKey);

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException(
        $"No connection string was configured for '{connectionStringKey}'. " +
        "Set ConnectionStrings__DefaultConnectionString in the environment."
    );
}

builder.Services.Configure<ConnectionManager>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<LogManager>(builder.Configuration.GetSection("LoggerManager"));

builder.Services.AddScoped<IStockRepository, StockSqlRepository>();
builder.Services.AddScoped<IUserRepository, UserSqlRepository>();
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

