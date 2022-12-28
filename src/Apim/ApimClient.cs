using Apim.Extensions;
using Apim.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Security.Authentication;
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
                throw new AuthenticationException("Unauthorized");

            var accessToken = accessTokens.FirstOrDefault();
            var responseContent = await response.Content.ReadFromJsonAsync<UserIdResponse>();

            if (accessToken == null)
                throw new AuthenticationException("Unauthorized");

            return new LoginResponse(accessToken, responseContent);
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/user/get?tabs=HTTP
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApimResponse<UserResponse>?> GetUserDetailsAsync(string? userid, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"{_options.SubscriptionPath}/users/{userid}", cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApimResponse<UserResponse>>(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/user-group/list?tabs=HTTP
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CollectionResponse<ApimResponse<GroupResponse>>?> GetUserGroupsAsync(string? userid, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync($"{_options.SubscriptionPath}/users/{userid}/groups", cancellationToken);
            return await response.Content.ReadFromJsonAsync<CollectionResponse<ApimResponse<GroupResponse>>>(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/apis/list-by-service?tabs=HTTP
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>ApisResponse</returns>
        public async Task<ApisResponse<ApimResponse<ApiResponse>>?> GetApisAsync(int skip = 0, int take = 10, bool includeNext = false, CancellationToken cancellationToken = default)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("expandApiVersionSet", "true");
            queryString.Add("$top", take.ToString());
            queryString.Add("$skip", skip.ToString());

            var response = await _client.GetAsync($"{_options.SubscriptionPath}/apis?{queryString}", cancellationToken);
            var apisResponse = await response.Content.ReadFromJsonAsync<ApisResponse<ApimResponse<ApiResponse>>>(cancellationToken: cancellationToken);

            if (apisResponse == null)
                return null;

            if (includeNext)
            {
                // do in parallel with Task.WhenAll if unmaanged apis becomes a real feature
                var next = await GetApisAsync(skip + take, 1, false, cancellationToken);
                apisResponse.nextName = next?.value?.FirstOrDefault()?.properties.displayName;
            }

            return apisResponse;
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/apis/get?tabs=HTTP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApimResponse<ApiResponse>?> GetApiAsync(string id, CancellationToken cancellationToken = default)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("expandApiVersionSet", "true");

            var responseMessage = await _client.GetAsync($"{_options.SubscriptionPath}/apis/{id}?{queryString}", cancellationToken);
            return await responseMessage.Content.ReadFromJsonAsync<ApimResponse<ApiResponse>>(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/apis/get?tabs=HTTP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> GetApiAsync(string id, string type, CancellationToken cancellationToken = default)
        {
            var headers = new Dictionary<string, string>() {
                { "Accept", type }
            };

            return _client.GetWithHeadersAsync($"{_options.SubscriptionPath}/apis/{id}", headers, cancellationToken);
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/api-operation/list-by-api?tabs=HTTP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CollectionResponse<ApimResponse<OperationResponse>>?> GetOperations(string id, CancellationToken cancellationToken = default)
        {
            var responseMessage = await _client.GetAsync($"{_options.SubscriptionPath}/apis/{id}/operations", cancellationToken);
            return await responseMessage.Content.ReadFromJsonAsync< CollectionResponse<ApimResponse<OperationResponse>>>(cancellationToken: cancellationToken);
        }
    }
}
