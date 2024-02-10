using TradingApp.Core.Models;

namespace TradingApp.Core.Repositories;

public interface IUserRepository
{
    public Task<User?> LoginAsync(string? email, string? password);
}
