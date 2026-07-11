using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Dtos;
using TradingApp.Models;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository repository;
    private readonly IPasswordHasher<User> passwordHasher;

    public UserController(IUserRepository repository, IPasswordHasher<User> passwordHasher)
    {
        this.repository = repository;
        this.passwordHasher = passwordHasher;
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult Login() => View();

    [AllowAnonymous]
    public IActionResult RegisterDemo()
    {
        return IsDevelopment() ? View() : NotFound();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterDemo(UserLoginDto user)
    {
        if (!IsDevelopment())
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 12)
        {
            ViewData["Error"] = "Email and a password of at least 12 characters are required.";
            return View(user);
        }

        if (await repository.GetByEmailAsync(user.Email) is not null)
        {
            ViewData["Error"] = "A user with that email already exists.";
            return View(user);
        }

        var newUser = new User
        {
            Email = user.Email,
            Name = "Local",
            Surname = "Demo"
        };
        newUser.PasswordHash = passwordHasher.HashPassword(newUser, user.Password);
        await repository.CreateAsync(newUser);
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
        {
            ViewData["Error"] = "Incorrect Credentials";
            return View(user);
        }

        var result = await repository.GetByEmailAsync(user.Email);

        if (result?.PasswordHash is not null &&
            passwordHasher.VerifyHashedPassword(result, result.PasswordHash, user.Password) != PasswordVerificationResult.Failed)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
                new Claim(ClaimTypes.Email, result.Email ?? user.Email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("GetAll", "Stock");
        }

        ViewData["Error"] = "Incorrect Credentials";
        return View(user);
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => Forbid();

    private bool IsDevelopment() => HttpContext.RequestServices
        .GetRequiredService<IWebHostEnvironment>().IsDevelopment();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View("Error!");
}
