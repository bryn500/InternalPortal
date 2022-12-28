using InternalPortal.Web.Consts;
using Joonasw.AspNetCore.SecurityHeaders;

namespace InternalPortal.Web.AppStart
{
    public static class AddSecurityExtension
    {
        private const string LocalHost = "wss://localhost:44313"; // todo: lookup localhost port

        public static void AddSecurity(this IServiceCollection services)
        {
            // Configure Antiforgery
            services.AddAntiforgery(opts => opts.Cookie.Name = CookieNames.AntiForgery);

            // Configure strict transport security
            services.AddHsts(options =>
            {
                options.Preload = false;
                options.IncludeSubDomains = false;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            // Configure CSP
            services.AddCsp(nonceByteAmount: 32);
        }

        public static void UseSecurityHeaders(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCsp(csp =>
            {
                // Block by default
                csp.ByDefaultAllow
                    .FromNowhere();

                // Allow JavaScript from:
                csp.AllowScripts
                    .FromSelf()
                    .AddNonce(); // e.g. <script asp-add-nonce="true"></script>

                csp.AllowStyles
                    .FromSelf();

                // mini profiler
                if (env.IsDevelopment()) 
                    csp.AllowStyles.AllowUnsafeInline();

                // Images allowed from:
                csp.AllowImages
                    .From("data:") // inline data images - base64
                    .FromSelf();

                // Fonts allowed from:
                csp.AllowFonts
                    .FromSelf();

                csp.AllowConnections
                    .ToSelf()
                    .To(env.IsDevelopment() ? LocalHost : "");
            });

            //Add pre-CSP headers for older browser support
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.Any(x => string.Equals(x.Key, "X-Frame-Options", StringComparison.OrdinalIgnoreCase)))
                        context.Response.Headers.Add("X-Frame-Options", "DENY");
                    if (!context.Response.Headers.Any(x => string.Equals(x.Key, "X-Xss-Protection", StringComparison.OrdinalIgnoreCase)))
                        context.Response.Headers.Add("X-Xss-Protection", "0");
                    if (!context.Response.Headers.Any(x => string.Equals(x.Key, "X-Content-Type-Options", StringComparison.OrdinalIgnoreCase)))
                        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    if (!context.Response.Headers.Any(x => string.Equals(x.Key, "Referrer-Policy", StringComparison.OrdinalIgnoreCase)))
                        context.Response.Headers.Add("Referrer-Policy", "same-origin");

                    return Task.CompletedTask;
                });

                await next();
            });
        }
    }
}
