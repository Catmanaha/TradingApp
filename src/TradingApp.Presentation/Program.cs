using System.Reflection;
using System.Security.Claims;
using TradingApp.Core.Enums;
using TradingApp.Core.Models.Managers;
using TradingApp.Infrastructure.Extensions;
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

builder.Services.AddTransient<LogMiddleware>();

builder.Services.Configure<LogManager>(builder.Configuration.GetSection("LoggerManager"));
builder.Services.Configure<ApiManager>(builder.Configuration.GetSection("ApiManager"));

builder.Services.InitDbContext(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.Inject();

var app = builder.Build();

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