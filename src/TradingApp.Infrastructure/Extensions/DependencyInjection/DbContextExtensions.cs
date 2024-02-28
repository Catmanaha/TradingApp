using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingApp.Core.Models;
using TradingApp.Infrastructure.Data;

namespace TradingApp.Infrastructure.Extensions.DependencyInjection;

public static class DbContextExtensions
{
    public static void InitDbContext(this IServiceCollection serviceCollection, IConfiguration configuration, Assembly assembly)
    {

        serviceCollection.AddDbContext<TradingAppDbContext>(options =>
        {
            string connectionStringKey = "TradingAppDb";
            string? connectionString = configuration.GetConnectionString("DefaultConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NullReferenceException($"No connection string found in appsettings.json with a key '{connectionStringKey}'");
            }

            options.UseSqlServer(connectionString, o =>
            {
                o.MigrationsAssembly(assembly.FullName);
            });
        });

        serviceCollection.AddIdentity<User, IdentityRole<int>>(o =>
                                                {
                                        o.Password.RequiredLength = 8;
                                                }
                                                ).AddEntityFrameworkStores<TradingAppDbContext>();
        serviceCollection.ConfigureApplicationCookie(o =>
        {
            o.AccessDeniedPath = "/User/AccessDenied";
            o.LoginPath = "/User/Login";
        });

    }
}
