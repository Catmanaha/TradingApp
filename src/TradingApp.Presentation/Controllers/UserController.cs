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
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly IUserService userService;

    public UserController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IUserService userService
    )
    {
        this.userManager = userManager;
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
        if (ModelState.IsValid == false)
        {
            return View();
        }

        try
        {

            await userService.Register(userDto);
            return RedirectToAction("Login");

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

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
        if (ModelState.IsValid == false)
        {
            return View();
        }

        try
        {

            await userService.Login(userDto);
            return RedirectPermanent(userDto.ReturnUrl ?? "/");

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }


    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }

        try
        {
            await userService.ChangePassword(dto, User);
            return RedirectToAction("Profile");

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }


    }

    public async Task<IActionResult> Profile()
    {
        try
        {
            var user = await userService.GetUser(User);
            return View(user);

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

    }

    public async Task<IActionResult> CashIn()
    {

        try
        {
            var user = await userService.GetUser(User);
            return View(user);

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

    }

    [HttpPost]
    public async Task<IActionResult> CashIn(CashInDto dto)
    {

        try
        {
            var user = await userService.GetUser(User);
            user.Balance += dto.AmoutToAdd;

            await userManager.UpdateAsync(user);
            return RedirectToAction("Profile");

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }


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
