using Apim.Models;
using Shared;
using System.Text;

namespace Apim
{
    public class ApimClient : IApimClient
    {
        private readonly HttpClient _client;
        public ApimClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// https://github.com/Azure/api-management-developer-portal/blob/master/src/services/usersService.ts
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Task<LoginResponse> Auth(string? userName, string? password, CancellationToken cancellationToken = default)
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

            var response = await _client.GetWithHeadersAsync("/identity", new Dictionary<string, string>()
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
        /// <param name="cancellationToken"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<string> GetApis(int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        {
            //var queryString = HttpUtility.ParseQueryString(string.Empty);
            //queryString.Add("expandApiVersionSet", "true");
            //queryString.Add("$top", take.ToString());
            //queryString.Add("$skip", skip.ToString());
            //?{queryString}

            var response = await _client.GetAsync($"/apis", cancellationToken);

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
    }
}
