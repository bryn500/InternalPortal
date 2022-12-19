using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.AppStart
{
    public static class CacheProfiles
    {
        public static void AddCacheProfiles(MvcOptions options)
        {
            options.CacheProfiles.Add("Default",
                new CacheProfile()
                {
                    VaryByHeader = "Accept-Encoding",
                    Location = ResponseCacheLocation.Client,
                    Duration = 120
                });

            options.CacheProfiles.Add("Never",
                new CacheProfile()
                {
                    Location = ResponseCacheLocation.None,
                    NoStore = true
                });
        }
    }
}
