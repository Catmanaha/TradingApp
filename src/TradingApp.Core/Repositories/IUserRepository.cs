using TradingApp.Core.Models;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Repositories;

public interface IUserRepository : ICreate<User>, IGetById<User?>
{
    public Task<User?> LoginAsync(string? email, string? password);
}
