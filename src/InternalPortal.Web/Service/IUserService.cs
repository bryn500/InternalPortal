using System.Security.Claims;

namespace InternalPortal.Web.Service
{
    public interface IUserService
    {
        /// <summary>
        /// Returns a claims principal representing the user matched by the login details provided
        /// </summary>
        /// <param name="userName">username for user</param>
        /// <param name="password">password for user</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ClaimsPrincipal with 2 claims, an apim access token and user id</returns>
        Task<ClaimsPrincipal> GetLogin(string userName, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Gets details about user and known groups based on the user id provided
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Claim>> GetUserDetails(string id, CancellationToken cancellationToken);
    }
}
