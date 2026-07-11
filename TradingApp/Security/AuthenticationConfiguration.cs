using Microsoft.AspNetCore.Authentication.Cookies;

namespace TradingApp.Security;

public static class AuthenticationConfiguration
{
    public static void ConfigureCookie(CookieAuthenticationOptions options)
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
        options.Cookie.Name = "TradingApp.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    }
}
