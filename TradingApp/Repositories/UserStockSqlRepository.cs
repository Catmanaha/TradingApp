using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TradingApp.Models;
using TradingApp.Models.Managers;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Repositories;

public class UserStockSqlRepository : IUserStockRepository
{

    private readonly SqlConnection connection;

    public UserStockSqlRepository(IOptions<ConnectionManager> connectionManager)
    {
        this.connection = new SqlConnection(connectionManager.Value.DefaultConnectionString);
    }

    public async Task<int> CreateAsync(UserStock model)
    {
        return await connection.ExecuteAsync(@"insert into UsersStocks (UserId, StockId, StockCount) 
                                               values(@UserId, @StockId, @StockCount)", model);
    }
}
