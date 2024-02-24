using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TradingApp.Core.Models;
using TradingApp.Core.Models.Managers;
using TradingApp.Core.Repositories;

namespace TradingApp.Infrastructure.Repositories;

public class StockSqlRepository : IStockRepository
{
    private readonly SqlConnection connection;

    public StockSqlRepository(IOptions<ConnectionManager> connectionManager)
    {
        this.connection = new SqlConnection(connectionManager.Value.DefaultConnectionString);
    }

    public async Task<int> CreateAsync(Stock stock)
    {
        return await connection.ExecuteAsync(@"insert into Stocks(Symbol, Name, MarketCap) 
                                               values(@Symbol, @Name, @MarketCap)", stock);
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await connection.QueryAsync<Stock>("select * from Stocks");
    }

    public async Task<IEnumerable<Stock>> GetRecentStocks()
    {
        return await connection.QueryAsync<Stock>(@"select *
                                                    from Stocks
                                                    where Id not in (
                                                        select top (
                                                                (select count(*) from Stocks) - 5
                                                            ) Id
                                                        from Stocks
                                                    )");
    }
}