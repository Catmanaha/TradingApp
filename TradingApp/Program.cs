using Microsoft.Data.SqlClient;
using TradingApp.Models;
using TradingApp.Repositories;
using TradingApp.Repositories.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IStockRepository>(p =>
{
    string connectionStringKey = "TradingAppDb";
    string? connectionString = builder.Configuration.GetConnectionString(connectionStringKey);

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new NullReferenceException($"No connection string found in appsettings.json with a key '{connectionStringKey}'");
    }
    return new StockSqlRepository(connectionString);
});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

