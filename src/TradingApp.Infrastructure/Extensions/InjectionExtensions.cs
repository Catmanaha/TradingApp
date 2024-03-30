using Microsoft.Extensions.DependencyInjection;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;
using TradingApp.Infrastructure.Data;
using TradingApp.Infrastructure.Repositories;
using TradingApp.Infrastructure.Services;

namespace TradingApp.Infrastructure.Extensions;

public static class InjectionExtensions
{
    public static void Inject(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBidService, BidService>();
        serviceCollection.AddSingleton<IStockService, StockService>();
        serviceCollection.AddScoped<IAuctionService, AuctionService>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IUserStockService, UserStockService>();
        serviceCollection.AddScoped<INewsService, NewsService>();

        serviceCollection.AddSingleton<INewsRepository, NewsApiRepository>();
        serviceCollection.AddSingleton<IStockRepository, StockApiRepository>();
        serviceCollection.AddScoped<IBidRepository, BidSqlRepository>();
        serviceCollection.AddScoped<IAuctionRepository, AuctionSqlRepository>();
        serviceCollection.AddScoped<IUserStockRepository, UserStockSqlRepository>();
        serviceCollection.AddScoped<ILogRepository, LogSqlRepository>();

        serviceCollection.AddTransient<HttpClient>();
        serviceCollection.AddScoped<TradingAppDbContext>();
    }
}