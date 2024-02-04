using Dapper;
using Microsoft.Data.SqlClient;
using TradingApp.Models;
using TradingApp.Repositories.Base;

namespace TradingApp.Repositories;

public class StockSqlRepository : IStockRepository
{
    private readonly SqlConnection connection;

    public StockSqlRepository(string connectionString)
    {
        this.connection = new SqlConnection(connectionString);
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
}