using Microsoft.Extensions.DependencyInjection;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Repositories;

namespace TradingApp.Infrastructure.Extensions.DependencyInjection;

public static class RepositoriesExtensions
{
    public static void InjectRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBidRepository, BidSqlRepository>();
        serviceCollection.AddScoped<IAuctionRepository, AuctionSqlRepository>();
        serviceCollection.AddScoped<IStockRepository, StockSqlRepository>();
        serviceCollection.AddScoped<IUserStockRepository, UserStockSqlRepository>();
        serviceCollection.AddScoped<ILogRepository, LogSqlRepository>();
    }
}
