using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.AppStart
{
    public static class CacheProfiles
    {
        public const string Default = "Default";

        public static void AddCacheProfiles(MvcOptions options)
        {
            options.CacheProfiles.Add(Default,
                new CacheProfile()
                {
                    VaryByHeader = "Accept-Encoding",
                    Location = ResponseCacheLocation.Any,
                    Duration = 300
                });
        }
    }
}
