using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TradingApp.Core.Models;
using TradingApp.Core.Models.Managers;
using TradingApp.Core.Repositories;

namespace TradingApp.Infrastructure.Repositories;

public class LogSqlRepository : ILogRepository
{
    private readonly SqlConnection connection;

    public LogSqlRepository(IOptions<ConnectionManager> connectionManager)
    {
        this.connection = new SqlConnection(connectionManager.Value.DefaultConnectionString);
    }

    public async Task<int> CreateAsync(Log log)
    {
        return await connection.ExecuteAsync(@"insert into Logs(UserId, Url, MethodType, StatusCode, RequestBody, ResponseBody) 
                                               values(@UserId, @Url, @MethodType, @StatusCode, @RequestBody, @ResponseBody)", log);
    }
}