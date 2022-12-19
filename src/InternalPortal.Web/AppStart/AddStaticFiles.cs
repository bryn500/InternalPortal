using Microsoft.Net.Http.Headers;

namespace InternalPortal.Web.AppStart
{
    public static class AddStaticFiles
    {
        public static void UseStaticFileDefaults(this IApplicationBuilder app)
        {   
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    // add cache control header to static resources
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromDays(365),
                        Public = true
                    };
                }
            });
        }
    }
}
