using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using TradingApp.Middlewares;
using TradingApp.Models;
using TradingApp.Repositories;
using TradingApp.Repositories.Base;
using TradingApp.Repositories.Base.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => {
    options.LoginPath = "/User/Login";
    options.LogoutPath = "/User/Logout";
});

string GetConnectionString()
{
    string connectionStringKey = "TradingAppDb";
    string? connectionString = builder.Configuration.GetConnectionString(connectionStringKey);

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new NullReferenceException($"No connection string found in appsettings.json with a key '{connectionStringKey}'");
    }

    return connectionString;
}

builder.Services.AddScoped<IStockRepository>(p =>
{
    return new StockSqlRepository(GetConnectionString());
});

builder.Services.AddScoped<IUserRepository>(p =>
{
    return new UserSqlRepository(GetConnectionString());
});

builder.Services.AddScoped<ILogRepository>(p =>
{
    return new LogSqlRepository(GetConnectionString());
});

builder.Services.Configure<LogManager>(builder.Configuration.GetSection("LoggerControl"));

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

app.UseAuthorization();
app.UseAuthentication();

app.UseMiddleware<LogMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

