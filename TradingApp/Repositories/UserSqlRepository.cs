using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TradingApp.Models;
using TradingApp.Models.Managers;
using TradingApp.Repositories.Base.Repositories;

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
