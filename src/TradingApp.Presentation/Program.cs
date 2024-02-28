using System.Reflection;
using System.Security.Claims;
using TradingApp.Core.Enums;
using TradingApp.Core.Models.Managers;
using TradingApp.Infrastructure.Data;
using TradingApp.Infrastructure.Extensions.DependencyInjection;
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

builder.Services.Configure<LogManager>(builder.Configuration.GetSection("LoggerManager"));

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<TradingAppDbContext>();

builder.Services.InitDbContext(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.InjectServices();
builder.Services.InjectRepositories();

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