using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace InternalPortal.Web.AppStart
{
    // todo: specify session length explicity
    public static class AddAuthExtension
    {
        public static void AddAuthOptions(MvcOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();

            options.Filters.Add(new AuthorizeFilter(policy));
        }

        public static void AddAuth(this IServiceCollection services)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = "/Account/login";
                        options.LogoutPath = "/Account/logout";
                    });
        }
    }
}
