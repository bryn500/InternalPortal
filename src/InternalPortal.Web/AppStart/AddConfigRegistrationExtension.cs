using Apim;

namespace InternalPortal.Web.AppStart
{
    public static class AddConfigRegistrationExtension
    {
        public static void AddConfigRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApimOptions>(configuration.GetSection(ApimOptions.ConfigName));
        }
    }
}
