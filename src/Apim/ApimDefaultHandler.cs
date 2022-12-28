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
        private const string HeaderName = "Authorization";        

        public ApimDefaultHandler(IHttpContextAccessor httpContextAccessor, IOptions<ApimOptions> apimOptions)
        {
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
            var token = GetSasToken();

            if (!string.IsNullOrEmpty(token))
                request.Headers.TryAddWithoutValidation(HeaderName, "SharedAccessSignature " + token);

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
