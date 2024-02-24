using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Enums;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Presentation.Dtos;

namespace TradingApp.Presentation.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository repository;
    private readonly IDataProtector dataProtector;

    public UserController(IUserRepository repository, IDataProtectionProvider dataProtectionProvider)
    {
        this.repository = repository;
        this.dataProtector = dataProtectionProvider.CreateProtector("IdentityProtection");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterDto userDto)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }

        var user = new User
        {
            Name = userDto.Name,
            Surname = userDto.Surname,
            Email = userDto.Email,
            Password = userDto.Password,
            Role = UserRolesEnum.User
        };

        await repository.CreateAsync(user);

        return RedirectToAction("Login");
    }

    public IActionResult Login(string? ReturnUrl)
    {
        ViewData["ReturnUrl"] = ReturnUrl;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto userdto)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }

        var result = await repository.LoginAsync(userdto.Email, userdto.Password);

        if (result is null)
        {
            ViewData.Add("Error", "Incorrect Credentials");
            return View();
        }

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.Role, result.Role.ToString()),
            new Claim("UserId", result.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return RedirectPermanent(userdto.ReturnUrl ?? "/");

    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await repository.GetByIdAsync(int.Parse(User.FindFirstValue("UserId")!));

        return View(user);
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
