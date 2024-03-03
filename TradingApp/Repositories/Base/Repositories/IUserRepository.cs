using TradingApp.Models;

namespace TradingApp.Repositories.Base.Repositories;

public interface IUserRepository
{
    public Task<User?> LoginAsync(string? email, string? password);
}
