﻿using Apim;

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

            services
                .AddHttpClient<IApimClient, ApimClient>(c => c.BaseAddress = new Uri(apimOptions.ManagementApiUrl))
                .AddHttpMessageHandler<ApimDefaultHandler>();
        }
    }
}
