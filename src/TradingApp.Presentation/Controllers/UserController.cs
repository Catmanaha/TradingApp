using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Enums;
using TradingApp.Core.Models;
using TradingApp.Presentation.Dtos;

namespace TradingApp.Presentation.Controllers;

public class UserController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole<int>> roleManager;
    private readonly SignInManager<User> signInManager;

    public UserController(
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        SignInManager<User> signInManager
    )
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.signInManager = signInManager;
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

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
            UserName = userDto.Username,
            Email = userDto.Email
        };

        var result = await userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            if (ModelState.Any())
            {
                return View();
            }

        }

        var userRole = new IdentityRole<int>
        {
            Name = UserRolesEnum.User.ToString()
        };

        await roleManager.CreateAsync(userRole);
        await userManager.AddToRoleAsync(user, UserRolesEnum.User.ToString());

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

        var user = await userManager.FindByEmailAsync(userdto.Email);

        if (user is null)
        {
            ViewData.Add("Error", "No user with this email found");
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(user, userdto.Password, true, true);

        if (result.Succeeded == false)
        {
            ViewData.Add("Error", "Incorrect Credentials");
            return View();
        }

        return RedirectPermanent(userdto.ReturnUrl ?? "/");

    }

    [Authorize]
    public IActionResult ChangePassword() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto) {
        if (ModelState.IsValid == false) {
            return View();
        }
 
        var user = await userManager.GetUserAsync(User);

        var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        
        if (result.Succeeded == false) {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return View();
        }

        return RedirectToAction("Profile");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await userManager.GetUserAsync(User);

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
