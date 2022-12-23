using Apim.Models;

namespace Apim
{
    public interface IApimClient
    {
        /// <summary>
        /// Login user
        /// https://github.com/Azure/api-management-developer-portal/blob/master/src/services/usersService.ts
        /// </summary>
        /// <param name="userName">User email</param>
        /// <param name="password">user password</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LoginResponse> AuthAsync(string? userName, string? password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets details user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApimResponse<UserResponse>?> GetUserDetailsAsync(string? userid, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets groups of user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CollectionResponse<ApimResponse<GroupResponse>>?> GetUserGroupsAsync(string? userid, CancellationToken cancellationToken = default);

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/apis/list-by-service?tabs=HTTP
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApisResponse<ApimResponse<ApiResponse>>?> GetApisAsync(int skip = 0, int take = 10, bool includeNext = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/apis/get?tabs=HTTP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApimResponse<ApiResponse>?> GetApiAsync(string id, CancellationToken cancellationToken = default);
    }
}
