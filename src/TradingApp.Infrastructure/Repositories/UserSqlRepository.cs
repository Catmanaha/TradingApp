using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TradingApp.Core.Models;
using TradingApp.Core.Models.Managers;
using TradingApp.Core.Repositories;

namespace TradingApp.Repositories;

public class UserSqlRepository : IUserRepository
{

    private readonly SqlConnection connection;

    public UserSqlRepository(IOptions<ConnectionManager> connectionManager)
    {
        this.connection = new SqlConnection(connectionManager.Value.DefaultConnectionString);
    }

    public async Task<User?> LoginAsync(string? email, string? password)
    {
        var result = await connection.QueryFirstOrDefaultAsync<User>(@"
                    select * from Users
                    where Email = @email and Password = @password",
                    new
                    {
                        email,
                        password
                    });

        return result;

    }
}
