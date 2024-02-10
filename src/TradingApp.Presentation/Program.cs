using TradingApp.Core.Models.Managers;
using TradingApp.Core.Repositories;
using TradingApp.Presentation.Middlewares;
using TradingApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

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

app.UseAuthorization();

app.UseMiddleware<LogMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

