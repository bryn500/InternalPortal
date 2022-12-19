namespace InternalPortal.Web.AppStart
{
    public static class AddConfigRegistrationExtension
    {
        public static void AddConfigRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<AzureAdConfig>(configuration.GetSection("AzureAd"));
            //services.Configure<BoundConfig>(options => configuration.GetSection("BoundConfig").Bind(options));
        }
    }
}
