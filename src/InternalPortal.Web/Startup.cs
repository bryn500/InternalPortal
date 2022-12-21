using InternalPortal.Web.AppStart;
using InternalPortal.Web.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Get config options
            services.AddConfigRegistration(Configuration);

            // Configure application services
            services.AddServiceRegistration(Configuration);

            // Allow access to httpcontext via DI
            services.AddHttpContextAccessor();

            // Adds response caching and compression
            services.AddCachingAndCompression();

            // Configure security options
            services.AddSecurity();

            // add app insights
            services.AddApplicationInsightsTelemetry();

            // profiler
            services.AddMiniProfiler();

            // Add controllers + views instead of razor pages
            services.AddControllersWithViews();

            services.AddAuth();

            services.Configure<SecurityStampValidatorOptions>(o =>
                o.ValidationInterval = TimeSpan.FromSeconds(10));

            // Add/configure MVC
            services.AddMvc(options =>
            {
                // default to need auth to access views
                AddAuthExtension.AddAuthOptions(options);

                // Handle cancelled requests
                options.Filters.Add<OperationCancelledExceptionFilter>();

                // Automatically add an anti-forgery token to any http request method that alters server state
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                // add cache profiles
                CacheProfiles.AddCacheProfiles(options);
            })
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Error pages
            app.UseErrorPages(env);

            if (!env.IsDevelopment())
            {
                // Enable Strict Transport Security
                app.UseHsts();

                // Redirect to https
                app.UseHttpsRedirection();
            }

            // Add Content Security Policy + security headers
            app.UseSecurityHeaders(env);

            // Compress responses based on options from ConfigureServices
            app.UseResponseCompression();

            // setup site to serve static files
            app.UseStaticFileDefaults();

            // Cookie policy
            app.UseCookiePolicy();

            // use mini profiler in dev for performance checking
            if (env.IsDevelopment())
                app.UseMiniProfiler();

            // Matches request to an endpoint
            app.UseRouting();

            // Use cache profiles set in ConfigureServices
            app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization();

            // Execute the matched endpoint
            app.UseEndpoints(endpoints =>
            {
                // attribute routing
                endpoints.MapControllers();
            });
        }
    }
}
