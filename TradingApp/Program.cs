using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using TradingApp.Models;
using TradingApp.Repositories;
using TradingApp.Repositories.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

string? connectionString = builder.Configuration.GetConnectionString("TradingAppDb");

ArgumentNullException.ThrowIfNull(connectionString);

builder.Services.AddScoped<ISqlRepository<Stock>>(p =>
{
    return new StockSqlRepository(new SqlConnection(connectionString));
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
    pattern: "{controller=Stock}/{action=GetAll}");

app.Run();
