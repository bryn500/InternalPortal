using System.Security.Authentication;
using System.Text.RegularExpressions;

namespace Apim.Models
{
    public class LoginResponse
    {
        public readonly static Regex tokenRegex = new Regex("token=\"(.*)\",refresh", RegexOptions.Compiled);

        public string Identifier { get; }

        public string AccessToken { get; }

        public LoginResponse(string accessTokenResponse, UserIdResponse? identifier)
        {
            if (identifier == null || string.IsNullOrWhiteSpace(identifier.id))
                throw new AuthenticationException("Failed to login");

            Identifier = identifier.id;

            var match = tokenRegex.Match(accessTokenResponse);

            if (!match.Success)
                throw new AuthenticationException("Failed to login");

            AccessToken = match.Groups[1].Value;
        }
    }
}
