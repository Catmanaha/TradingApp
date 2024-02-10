using Dapper;
using Microsoft.Data.SqlClient;
using TradingApp.Models;
using TradingApp.Repositories.Base;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Repositories;

public class UserSqlRepository : IUserRepository
{

    private readonly SqlConnection connection;

    public UserSqlRepository(string connectionString)
    {
        this.connection = new SqlConnection(connectionString);
    }

    public async Task<int> CreateAsync(User user)
    {
        return await connection.ExecuteAsync(@"
        insert into [Users]([Email], [Name], [Surname], [Password], [Role])
        values(@Email, @Name, @Surname, @Password, @Role)", user);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await connection.QueryFirstOrDefaultAsync<User>(@"
        select * from [Users]
        where [Id] = @id
        ", new { id });
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
