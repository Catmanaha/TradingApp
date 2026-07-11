using TradingApp.Models;

namespace TradingApp.Repositories.Base.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<int> CreateAsync(User user);
}
