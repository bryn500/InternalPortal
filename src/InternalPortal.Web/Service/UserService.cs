using Apim;
using InternalPortal.Web.Consts;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Authentication;
using System.Security.Claims;

namespace InternalPortal.Web.Service
{
    public class UserService : IUserService
    {
        private readonly IApimClient _client;

        public UserService(IApimClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Logs in via APIM API
        /// </summary>
        /// <param name="userName">username for user</param>
        /// <param name="password">password for user</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ClaimsPrincipal with 2 claims, an apim access token and user id</returns>
        public async Task<ClaimsPrincipal> GetLogin(string userName, string password, CancellationToken cancellationToken)
        {
            var auth = await _client.AuthAsync(userName, password, cancellationToken);

            // todo: enforce single user session at a time with security stamp
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Authentication, auth.AccessToken),
                new Claim(ClaimTypes.NameIdentifier, auth.Identifier)
            };

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
            );

            return principal;
        }

        /// <summary>
        /// Gets details about user and known groups based on the user id provided 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>a list of claims that can be added to current user</returns>
        /// <exception cref="AuthenticationException"></exception>
        public async Task<List<Claim>> GetUserDetails(string id, CancellationToken cancellationToken)
        {
            var userDetails = await _client.GetUserDetailsAsync(id, cancellationToken);
            var groups = await _client.GetUserGroupsAsync(id, cancellationToken);

            if (userDetails == null || userDetails.properties == null || userDetails.properties.state != "active" || id == null)
                throw new AuthenticationException("Could not find user");

            var claims = new List<Claim>();

            if (userDetails.properties.email != null)
                claims.Add(new Claim(ClaimTypes.Email, userDetails.properties.email));

            if (userDetails.properties.firstName != null)
                claims.Add(new Claim(ClaimTypes.GivenName, userDetails.properties.firstName));

            if (userDetails.properties.lastName != null)
                claims.Add(new Claim(ClaimTypes.Surname, userDetails.properties.lastName));

            if (userDetails.properties.firstName != null || userDetails.properties.lastName != null)
                claims.Add(new Claim(ClaimTypes.Name, $"{userDetails.properties.firstName} {userDetails.properties.lastName}".Trim()));

            if (userDetails.properties.registrationDate != null)
                claims.Add(new Claim(CustomClaimTypes.RegistrationDate, userDetails.properties.registrationDate.Value.ToString()));

            var devGroup = groups?.value?.FirstOrDefault(x => x.name == "developers");
            if (devGroup != null)
                claims.Add(new Claim(CustomClaimTypes.Developer, "developer"));

            return claims;
        }
    }
}
