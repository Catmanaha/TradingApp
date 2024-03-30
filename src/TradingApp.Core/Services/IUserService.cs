using System.Security.Claims;
using TradingApp.Core.Dtos;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Services;

public interface IUserService : IGetAll<User>
{
    public Task Register(UserRegisterDto userDto);
    public Task Login(UserLoginDto userdto);
    public Task ChangePassword(ChangePasswordDto dto, ClaimsPrincipal user);
    public Task<User> GetUser(ClaimsPrincipal user);
    public int GetId(ClaimsPrincipal user);
    public Task<User> GetById(int id);
    public Task DeleteAsync(int id);
    public Task CashIn(ClaimsPrincipal User, double amountToAdd);
}
