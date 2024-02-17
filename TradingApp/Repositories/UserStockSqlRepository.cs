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
        return await connection.ExecuteAsync(@"merge into UsersStocks as target
                                               using (values(@UserId, @StockId, @StockCount)) as source (UserId, StockId, StockCount)
                                               on target.StockId = source.StockId
                                               when matched then
                                               update set target.StockCount = target.StockCount + source.StockCount
                                               when not matched then
                                               insert (UserId, StockId, StockCount)
                                               values (source.UserId, source.StockId, source.StockCount);",
                                               model);
    }

    public async Task<IEnumerable<UserStock>> GetAllForUserAsync(int id)
    {
        return await connection.QueryAsync<UserStock>(@"select UserId, StockId, StockCount, StockName = s.Name from UsersStocks
                                             join Stocks s on s.Id = UsersStocks.StockId
                                             where UserId = @id", new { id });
    }
}
