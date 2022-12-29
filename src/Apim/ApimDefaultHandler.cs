using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Web;

namespace Apim
{
    public class ApimDefaultHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApimOptions _apimOptions;
        public const string AuthHeaderName = "Authorization";
        public const string AuthHeaderPrefix = "SharedAccessSignature "; // space required

        public ApimDefaultHandler(IHttpContextAccessor httpContextAccessor, IOptions<ApimOptions> apimOptions)
        {
            // avoid injecting scoped dependencies into handler, or caching data from httpcontext
            // IHttpClientFactory manages the lifetime of your HttpMessageHandler pipeline separately from the HttpClient instances.
            // HttpClient instances are created new every time, but for the 2(default value) minutes before a handler expires, every HttpClient with a given name uses the same handler pipeline.
            // i.e. the same instance of this handler will be used between requests from different users
            // use _httpContextAccessor to ensure same scope as request is used e.g. _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ScopedService>();

            _httpContextAccessor = httpContextAccessor;
            _apimOptions = apimOptions.Value;
        }

        public ApimDefaultHandler(DelegatingHandler innerHandler, IHttpContextAccessor httpContextAccessor, IOptions<ApimOptions> apimOptions) : base(innerHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _apimOptions = apimOptions.Value;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddApiVersionToQueryString(request);

            if (_httpContextAccessor.HttpContext != null)
            {
                var token = GetSasToken();
                if (!string.IsNullOrEmpty(token))
                    request.Headers.TryAddWithoutValidation(AuthHeaderName, AuthHeaderPrefix + token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Attempts to add the api version to the url requested if it is not already specified
        /// </summary>
        /// <param name="request">The http request to add the querystring to</param>
        private void AddApiVersionToQueryString(HttpRequestMessage request)
        {
            var requestUri = request.RequestUri;

            if (requestUri != null)
            {
                var qs = HttpUtility.ParseQueryString(requestUri.Query);
                if (qs["api-version"] == null)
                {
                    qs["api-version"] = _apimOptions.ManagementApiVersion;

                    var uriBuilder = new UriBuilder(requestUri)
                    {
                        Query = qs.ToString()
                    };

                    request.RequestUri = uriBuilder.Uri;
                }
            }
        }

        /// <summary>
        /// Grabs the user's sas token
        /// </summary>
        /// <returns></returns>
        private string? GetSasToken()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Authentication)?.Value;
        }
    }
}
