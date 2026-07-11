using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TradingApp.Controllers;
using TradingApp.Dtos;
using TradingApp.Models;
using TradingApp.Repositories.Base.Repositories;

namespace TradingApp.Tests;

public class AuthenticationTests
{
    private readonly PasswordHasher<User> passwordHasher = new();

    [Fact]
    public void PasswordsAreHashedAndVerifyOnlyWithTheOriginalPassword()
    {
        var user = new User { Email = "local@example.test" };
        var hash = passwordHasher.HashPassword(user, "correct horse battery staple");

        Assert.NotEqual("correct horse battery staple", hash);
        Assert.Equal(PasswordVerificationResult.Success,
            passwordHasher.VerifyHashedPassword(user, hash, "correct horse battery staple"));
        Assert.Equal(PasswordVerificationResult.Failed,
            passwordHasher.VerifyHashedPassword(user, hash, "wrong password"));
    }

    [Fact]
    public async Task LoginSignsInAUserWithAValidPassword()
    {
        var user = new User { Id = 4, Email = "local@example.test" };
        user.PasswordHash = passwordHasher.HashPassword(user, "correct password");
        var repository = new Mock<IUserRepository>();
        repository.Setup(x => x.GetByEmailAsync(user.Email)).ReturnsAsync(user);
        var auth = new RecordingAuthenticationService();
        var controller = CreateController(repository.Object, auth);

        var result = await controller.Login(new UserLoginDto { Email = user.Email, Password = "correct password" });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("GetAll", redirect.ActionName);
        Assert.NotNull(auth.SignedInPrincipal);
        Assert.Equal("4", auth.SignedInPrincipal!.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    [Fact]
    public async Task LoginRejectsAnInvalidPassword()
    {
        var user = new User { Id = 4, Email = "local@example.test" };
        user.PasswordHash = passwordHasher.HashPassword(user, "correct password");
        var repository = new Mock<IUserRepository>();
        repository.Setup(x => x.GetByEmailAsync(user.Email)).ReturnsAsync(user);
        var auth = new RecordingAuthenticationService();
        var controller = CreateController(repository.Object, auth);

        var result = await controller.Login(new UserLoginDto { Email = user.Email, Password = "wrong password" });

        Assert.IsType<ViewResult>(result);
        Assert.Null(auth.SignedInPrincipal);
    }

    [Fact]
    public async Task LoginRejectsAnUnknownUser()
    {
        var repository = new Mock<IUserRepository>();
        repository.Setup(x => x.GetByEmailAsync("missing@example.test")).ReturnsAsync((User?)null);
        var auth = new RecordingAuthenticationService();
        var controller = CreateController(repository.Object, auth);

        var result = await controller.Login(new UserLoginDto { Email = "missing@example.test", Password = "some password" });

        Assert.IsType<ViewResult>(result);
        Assert.Null(auth.SignedInPrincipal);
    }

    [Fact]
    public void StockRoutesRequireAuthenticationWhileLoginIsAnonymous()
    {
        Assert.NotNull(typeof(StockController).GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), true).SingleOrDefault());
        Assert.NotNull(typeof(UserController).GetMethod(nameof(UserController.Login), new[] { typeof(UserLoginDto) })
            ?.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute), true).SingleOrDefault());
    }

    private static UserController CreateController(IUserRepository repository, RecordingAuthenticationService auth)
    {
        var services = new ServiceCollection();
        services.AddControllersWithViews();
        services.AddSingleton<IAuthenticationService>(auth);
        var serviceProvider = services.BuildServiceProvider();
        var context = new DefaultHttpContext { RequestServices = serviceProvider };
        return new UserController(repository, new PasswordHasher<User>())
        {
            ControllerContext = new ControllerContext(new ActionContext(
                context,
                new RouteData(),
                new ControllerActionDescriptor { ControllerName = "User", ActionName = "Login" }))
        };
    }

    private sealed class RecordingAuthenticationService : IAuthenticationService
    {
        public ClaimsPrincipal? SignedInPrincipal { get; private set; }

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme) =>
            Task.FromResult(AuthenticateResult.NoResult());

        public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties) => Task.CompletedTask;

        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties) => Task.CompletedTask;

        public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
        {
            SignedInPrincipal = principal;
            return Task.CompletedTask;
        }

        public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties) => Task.CompletedTask;
    }
}
