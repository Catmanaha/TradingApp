using System.Security.Claims;
using TradingApp.Core.Dtos;
using TradingApp.Core.Models;

namespace TradingApp.Core.Services;

public interface IUserService
{
    public Task Register(UserRegisterDto userDto);
    public Task Login(UserLoginDto userdto);
    public Task ChangePassword(ChangePasswordDto dto, ClaimsPrincipal user);
    public Task<User> GetUser(ClaimsPrincipal user);
    public int GetId(ClaimsPrincipal user);
    public Task<User> GetById(int id);
}
