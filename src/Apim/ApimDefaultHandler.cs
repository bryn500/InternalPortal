using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Apim
{
    public class ApimDefaultHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly ApimOptions _apimOptions;
        private const string HeaderName = "Authorization";
        private const string CacheKey = "ApiSasTokenKey";

        public ApimDefaultHandler(IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IOptions<ApimOptions> apimOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _apimOptions = apimOptions.Value;
        }

        public ApimDefaultHandler(DelegatingHandler innerHandler, IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IOptions<ApimOptions> apimOptions) : base(innerHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
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

        /// <summary>
        /// Unused
        /// Gets an sas token for the length of time speicifed, will cache this for the length of the expiry
        /// This will represent an unauthenticated user accessing the api
        /// </summary>
        /// <param name="expiry">Datetime that the token should expire</param>
        /// <returns></returns>
        private string GetAppSasToken(DateTime expiry)
        {
            if (expiry > DateTime.Now.AddDays(30))
                throw new ArgumentException("expiry can't be over 30 days in the future");

            return _cache.GetOrCreate(CacheKey, entry =>
            {
                entry.AbsoluteExpiration = expiry.AddSeconds(-5);
                var token = CreateSharedAccessToken(_apimOptions.ManagementApiId, _apimOptions.ManagementApiPrimaryKey, expiry);

                return token;
            });
        }

        /// <summary>
        /// Unused
        /// To programmatically create an access token
        /// https://msdn.microsoft.com/library/azure/5b13010a-d202-4af5-aabf-7ebc26800b3d#ProgrammaticallyCreateToken
        /// </summary>
        /// <param name="id">the value from the identifier text box in the credentials section of the API Management REST API tab of the Security section</param>
        /// <param name="key">either the primary or secondary key from that same tab</param>
        /// <param name="expiry">the expiration date and time for the generated access token</param>
        /// <returns></returns>
        private static string CreateSharedAccessToken(string id, string key, DateTime expiry)
        {
            using var encoder = new HMACSHA512(Encoding.UTF8.GetBytes(key));

            string dataToSign = id + "\n" + expiry.ToString("O", CultureInfo.InvariantCulture);

            var hash = encoder.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));

            var signature = Convert.ToBase64String(hash);

            return string.Format("uid={0}&ex={1:o}&sn={2}", id, expiry, signature);
        }
    }
}
