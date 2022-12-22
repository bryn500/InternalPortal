using Apim.Models;
using Microsoft.Extensions.Options;
using Shared;
using System.Net.Http.Json;
using System.Text;
using System.Web;

namespace Apim
{
    public class ApimClient : IApimClient
    {
        private readonly HttpClient _client;
        private readonly ApimOptions _options;

        public ApimClient(HttpClient client, IOptions<ApimOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        /// <summary>
        /// Login user
        /// https://github.com/Azure/api-management-developer-portal/blob/master/src/services/usersService.ts
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>LoginResponse</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task<LoginResponse> AuthAsync(string? userName, string? password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("no username provided");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("no password provided");

            return AuthInternal(userName, password, cancellationToken);
        }

        private async Task<LoginResponse> AuthInternal(string userName, string password, CancellationToken cancellationToken = default)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes($"{userName}:{password}");
            var authHeader = $"Basic {Convert.ToBase64String(toEncodeAsBytes)}";

            var response = await _client.GetWithHeadersAsync($"{_options.SubscriptionPath}/identity", new Dictionary<string, string>()
            {
                ["Authorization"] = authHeader
            }, cancellationToken);

            response.EnsureSuccessStatusCode();

            response.Headers.TryGetValues("ocp-apim-sas-token", out IEnumerable<string>? accessTokens);
            if (accessTokens == null || !accessTokens.Any())
                throw new Exception("Unauthorized");

            var accessToken = accessTokens.FirstOrDefault();
            var responseContent = await response.Content.ReadAsStringAsync();

            if (accessToken == null)
                throw new Exception("Unauthorized");

            return new LoginResponse(accessToken, responseContent);
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/apis/list-by-service?tabs=HTTP
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>ApisResponse</returns>
        public async Task<ApisResponse?> GetApisAsync(int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("expandApiVersionSet", "true");
            queryString.Add("$top", take.ToString());
            queryString.Add("$skip", skip.ToString());

            var responseMessage = await _client.GetAsync($"{_options.SubscriptionPath}/apis?{queryString}", cancellationToken);
            return await responseMessage.Content.ReadFromJsonAsync<ApisResponse>(cancellationToken: cancellationToken);

            //var responseMessage = await _client.GetAsync($"apis?{queryString}", cancellationToken);
            //var responseContent = await responseMessage.Content.ReadAsStringAsync();
            //return JsonSerializer.Deserialize<ApisResponse>(responseContent);

        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/apis/get?tabs=HTTP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResponse?> GetApiAsync(string id, CancellationToken cancellationToken = default)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("expandApiVersionSet", "true");

            var responseMessage = await _client.GetAsync($"{_options.SubscriptionPath}/apis/{id}?{queryString}", cancellationToken);
            return await responseMessage.Content.ReadFromJsonAsync<ApiResponse>(cancellationToken: cancellationToken);
        }
    }
}
