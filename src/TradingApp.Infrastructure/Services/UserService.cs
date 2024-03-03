using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TradingApp.Core.Dtos;
using TradingApp.Core.Enums;
using TradingApp.Core.Models;
using TradingApp.Core.Services;

namespace TradingApp.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole<int>> roleManager;
    private readonly SignInManager<User> signInManager;

    public UserService(
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        SignInManager<User> signInManager
    )
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.signInManager = signInManager;
    }

    public async Task<User> GetById(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Id cannot be negative");
        }

        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            throw new NullReferenceException("User not found");
        }

        return user;
    }

    public int GetId(ClaimsPrincipal user)
    {
        if (user is null)
        {
            throw new NullReferenceException("User cannot be null");
        }

        var id = userManager.GetUserId(user);

        if (id is null)
        {
            throw new NullReferenceException("Id not found");
        }

        return int.Parse(id);
    }

    public async Task ChangePassword(ChangePasswordDto dto, ClaimsPrincipal user)
    {
        if (dto is null)
        {
            throw new NullReferenceException("Dto cannot be null");
        }

        if (user is null)
        {
            throw new NullReferenceException("User cannot be null");
        }

        var userResult = await userManager.GetUserAsync(user);
        var result = await userManager.ChangePasswordAsync(userResult, dto.CurrentPassword, dto.NewPassword);

        if (result.Succeeded == false)
        {

            var exceptions = new List<Exception>();

            foreach (var error in result.Errors)
            {
                exceptions.Add(new ArgumentException(error.Description, error.Code));

            }

            throw new AggregateException(exceptions);
        }
    }

    public async Task<User> GetUser(ClaimsPrincipal user)
    {
        if (user is null)
        {
            throw new NullReferenceException("User cannot be null");
        }

        var result = await userManager.GetUserAsync(user);

        if (result is null)
        {
            throw new NullReferenceException("User not found");
        }

        return result;
    }

    public async Task Login(UserLoginDto userDto)
    {
        if (userDto is null)
        {
            throw new NullReferenceException("Dto cannot be null");
        }

        var user = await userManager.FindByEmailAsync(userDto.Email);

        if (user is null)
        {
            throw new NullReferenceException("No user with this email found");
        }

        var result = await signInManager.PasswordSignInAsync(user, userDto.Password, true, true);

        if (result.Succeeded == false)
        {
            throw new ArgumentException("Incorrect Credentials");
        }
    }

    public async Task Register(UserRegisterDto userDto)
    {
        if (userDto is null)
        {
            throw new NullReferenceException("Dto cannot be null");
        }

        var user = new User
        {
            UserName = userDto.Username,
            Email = userDto.Email
        };

        var result = await userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {

            var exceptions = new List<Exception>();

            foreach (var error in result.Errors)
            {
                exceptions.Add(new ArgumentException(error.Description, error.Code));
            }

            throw new AggregateException(exceptions);
        }

        var userRole = new IdentityRole<int>
        {
            Name = UserRolesEnum.User.ToString()
        };

        await roleManager.CreateAsync(userRole);
        await userManager.AddToRoleAsync(user, UserRolesEnum.User.ToString());
    }
}
