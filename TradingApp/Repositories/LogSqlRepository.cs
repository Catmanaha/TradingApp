using Dapper;
using Microsoft.Data.SqlClient;
using TradingApp.Models;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Repositories;

public class LogSqlRepository : ILogRepository
{
    private readonly SqlConnection connection;

    public LogSqlRepository(string connectionString)
    {
        this.connection = new SqlConnection(connectionString);
    }

    public async Task<int> CreateAsync(Log log)
    {
        return await connection.ExecuteAsync(@"insert into Logs(UserId, Url, MethodType, StatusCode, RequestBody, ResponseBody) 
                                               values(@UserId, @Url, @MethodType, @StatusCode, @RequestBody, @ResponseBody)", log);
    }
}
