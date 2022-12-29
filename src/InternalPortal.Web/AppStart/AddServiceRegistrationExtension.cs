using Apim;
using InternalPortal.Web.Services;

namespace InternalPortal.Web.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var apimOptions = new ApimOptions();
            configuration.GetSection("ManagmentApi").Bind(apimOptions);

            services.AddScoped<ApimDefaultHandler>();
            services.AddScoped<IApimClient, ApimClient>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApiService, ApiService>();

            if (!string.IsNullOrEmpty(apimOptions.BackendUrl))
            {
                // https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#typed-clients 
                services
                    .AddHttpClient<IApimClient, ApimClient>(client =>
                    {
                        client.BaseAddress = new Uri(apimOptions.BackendUrl);

                        // use below to set http version policy
                        // may need to also set on request in ApimDefaultHandler as SendAsync
                        // client.DefaultRequestVersion = HttpVersion.Version11;
                        // client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
                    })
                    .AddHttpMessageHandler<ApimDefaultHandler>();
            }
        }
    }
}
