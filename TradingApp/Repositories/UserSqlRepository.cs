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

    public async Task<User?> GetByEmailAsync(string email)
    {
        var result = await connection.QueryFirstOrDefaultAsync<User>(@"
                    select * from Users
                    where Email = @email",
                    new
                    {
                        email
                    });

        return result;

    }

    public async Task<int> CreateAsync(User user)
    {
        return await connection.ExecuteAsync(@"
            insert into Users (Email, Name, Surname, PasswordHash)
            values (@Email, @Name, @Surname, @PasswordHash)", user);
    }
}
