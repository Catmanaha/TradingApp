using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TradingApp.UnitTest;

public class UserServiceTests
{
    [Fact]
    public async void GetById_IdNegative_ThrowArgumentException()
    {
        var id = -1;
        var service = new UserService(null, null, null);

        await Assert.ThrowsAsync<ArgumentException>(() => service.GetById(id));
    }

    [Fact]
    public async void GetById_UserNull_ThrowNullReferenceException()
    {
        var id = 1;
        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        userManager.Setup(repo => repo.FindByIdAsync(id.ToString())).ReturnsAsync((User?)null);

        var service = new UserService(userManager.Object, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetById(id));
    }

    [Fact]
    public void GetId_UserNull_ThrowNullReferenceException()
    {
        var service = new UserService(null, null, null);

        Assert.Throws<NullReferenceException>(() => service.GetId(null));
    }

    [Fact]
    public void GetById_IdNull_ThrowNullReferenceException()
    {
        var user = new ClaimsPrincipal();
        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        userManager.Setup(repo => repo.GetUserId(user)).Returns((string?)null);

        var service = new UserService(userManager.Object, null, null);

        Assert.Throws<NullReferenceException>(() => service.GetId(user));
    }

    [Fact]
    public async void ChangePassword_UserNull_ThrowNullReferenceException()
    {
        var service = new UserService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.ChangePassword(new ChangePasswordDto(), null));
    }

    [Fact]
    public async void ChangePassword_DtoNull_ThrowNullReferenceException()
    {
        var service = new UserService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.ChangePassword(null, new ClaimsPrincipal()));
    }

    [Fact]
    public async void GetUser_UserNull_ThrowNullReferenceException()
    {
        var service = new UserService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetUser(null));
    }

    [Fact]
    public async void GetUser_ResultNull_ThrowNullReferenceException()
    {
        var user = new ClaimsPrincipal();
        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        userManager.Setup(repo => repo.GetUserAsync(user)).ReturnsAsync((User?)null);

        var service = new UserService(userManager.Object, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetUser(user));
    }

    [Fact]
    public async void Login_DtoNull_ThrowNullReferenceException()
    {
        var service = new UserService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.Login(null));
    }

    [Fact]
    public async void Login_UserNull_ThrowNullReferenceException()
    {
        var userDto = new UserLoginDto
        {
            Email = string.Empty
        };

        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        userManager.Setup(repo => repo.FindByEmailAsync(userDto.Email)).ReturnsAsync((User?)null);

        var service = new UserService(userManager.Object, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.Login(userDto));
    }

    [Fact]
    public async void Register_DtoNull_ThrowNullReferenceException()
    {
        var service = new UserService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.Register(null));
    }
}
