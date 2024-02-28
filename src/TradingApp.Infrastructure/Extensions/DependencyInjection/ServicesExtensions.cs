using Microsoft.Extensions.DependencyInjection;
using TradingApp.Core.Services;
using TradingApp.Infrastructure.Services;

namespace TradingApp.Infrastructure.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static void InjectServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IBidService, BidService>();
        serviceCollection.AddScoped<IAuctionService, AuctionService>();
        serviceCollection.AddScoped<IUserStockService, UserStockService>();
    }
}
