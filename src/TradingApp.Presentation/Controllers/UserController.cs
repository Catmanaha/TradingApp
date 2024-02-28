using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
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

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Logout()
    {
        if (HttpContext.Request.Cookies["UserId"] is not null)
        {
            HttpContext.Response.Cookies.Append("UserId", "", new CookieOptions
            {
                Expires = DateTimeOffset.Parse("2/2/2000")
            });

        }
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto user)
    {

        var result = await repository.LoginAsync(user.Email, user.Password);

        if (result is not null)
        {
            HttpContext.Response.Cookies.Append("UserId", dataProtector.Protect(result.Id.ToString()));
            return RedirectToAction("GetAll", "Stock");
        }

        ViewData.Add("Error", "Incorrect Credentials");

        return View();

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
