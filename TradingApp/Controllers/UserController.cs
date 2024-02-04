using Microsoft.AspNetCore.Mvc;
using TradingApp.Dtos;
using TradingApp.Repositories.Base.Repositories;
namespace TradingApp.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository repository;

    public UserController(IUserRepository repository)
    {
        this.repository = repository;
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
            HttpContext.Response.Cookies.Append("UserId", result.Id.ToString());
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
