using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace InternalPortal.Web.AppStart
{
    public static class AddAuthExtension
    {
        // todo: tie this to the token expiration from apim (1 hour)
        // todo: use an in memory store of security stamps linked to users, update on login and check on request
        // todo: add rate limit
        private const int SessionLength = 20; // set to 0 for a session cookie otherwise specify in mins

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

                        if (SessionLength != 0)
                        {
                            var sessionTimeSpan = TimeSpan.FromMinutes(SessionLength);
                            options.Cookie.MaxAge = sessionTimeSpan;
                            options.ExpireTimeSpan = sessionTimeSpan;
                            options.SlidingExpiration = false;
                        }
                    });
        }
    }
}
