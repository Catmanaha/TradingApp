using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Enums;
using TradingApp.Core.Models;
using TradingApp.Core.Models.Managers;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;
using TradingApp.Infrastructure.Repositories;
using TradingApp.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins", p =>
    {
        p.RequireRole(ClaimTypes.Role, UserRolesEnum.Admin.ToString());
    });
});

builder.Services.AddDbContext<TradingAppDbContext>(options =>
{
    string connectionStringKey = "TradingAppDb";
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new NullReferenceException($"No connection string found in appsettings.json with a key '{connectionStringKey}'");
    }

    options.UseSqlServer(connectionString, o =>
    {
        o.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
    });
});

builder.Services.AddIdentity<User, IdentityRole<int>>(o =>
{
    o.Password.RequiredLength = 8;
}
).AddEntityFrameworkStores<TradingAppDbContext>();

builder.Services.ConfigureApplicationCookie(o => {
    o.AccessDeniedPath = "/User/AccessDenied";
    o.LoginPath = "/User/Login";
});

builder.Services.Configure<LogManager>(builder.Configuration.GetSection("LoggerManager"));

builder.Services.AddScoped<TradingAppDbContext>();
builder.Services.AddScoped<IBidRepository, BidSqlRepository>();
builder.Services.AddScoped<IAuctionRepository, AuctionSqlRepository>();
builder.Services.AddScoped<IStockRepository, StockSqlRepository>();
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