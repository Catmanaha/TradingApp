using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Dtos;
using TradingApp.Core.Services;

namespace TradingApp.Presentation.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly IUserService userService;

    public UserController(
        SignInManager<User> signInManager,
        IUserService userService
    )
    {
        this.signInManager = signInManager;
        this.userService = userService;
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserRegisterDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        await userService.Register(userDto);
        return RedirectToAction("Login");
    }

    [AllowAnonymous]
    public IActionResult Login(string? ReturnUrl)
    {
        ViewData["ReturnUrl"] = ReturnUrl;

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        await userService.Login(userDto);
        return RedirectPermanent(userDto.ReturnUrl ?? "/");
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        await userService.ChangePassword(dto, User);
        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> Profile()
    {
        var user = await userService.GetUser(User);
        return View(user);
    }

    public async Task<IActionResult> CashIn()
    {
        var user = await userService.GetUser(User);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> CashIn(CashInDto dto)
    {
        if (!ModelState.IsValid)
        {
            var user = await userService.GetUser(User);
            return View(user);
        }

        await userService.CashIn(User, dto.AmoutToAdd);
        return RedirectToAction("Profile");
    }

    [Authorize(Policy = "Admins")]
    public async Task<IActionResult> GetAll()
    {
        return View((await userService.GetAllAsync()).Where(o => o.UserName != "Admin"));
    }

    [Authorize(Policy = "Admins")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int userId)
    {
        await userService.DeleteAsync(userId);
        return RedirectToAction("GetAll");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
