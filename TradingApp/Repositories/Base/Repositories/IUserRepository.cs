using TradingApp.Models;

namespace TradingApp.Repositories.Base.Repositories;

public interface IUserRepository : ICreate<User>, IGetById<User?>
{
    public Task<User?> LoginAsync(string? email, string? password);
}
