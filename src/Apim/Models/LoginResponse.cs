using System.Text.RegularExpressions;

namespace Apim.Models
{
    public class LoginResponse
    {
        public readonly static Regex tokenRegex = new Regex("token=\"(.*)\",refresh", RegexOptions.Compiled);

        public string Identifier { get; }

        public string AccessToken { get; }

        public LoginResponse(string accessTokenResponse, string identifier)
        {
            Identifier = identifier;

            var match = tokenRegex.Match(accessTokenResponse);

            if (!match.Success)
                throw new Exception("Failed to login");

            AccessToken = match.Groups[1].Value;
        }
    }
}
