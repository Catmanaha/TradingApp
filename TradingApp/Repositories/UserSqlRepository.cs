using Dapper;
using Microsoft.Data.SqlClient;
using TradingApp.Models;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Repositories;

public class UserSqlRepository : IUserRepository
{

    private readonly SqlConnection connection;

    public UserSqlRepository(string connectionString)
    {
        this.connection = new SqlConnection(connectionString);
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
