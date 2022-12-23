using Apim.Models;

namespace Apim
{
    public interface IApimClient
    {
        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="userName">User email</param>
        /// <param name="password">user password</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LoginResponse> AuthAsync(string? userName, string? password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets details of a user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApimResponse<UserResponse>?> GetUserDetailsAsync(string? userid, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets groups of a user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CollectionResponse<ApimResponse<GroupResponse>>?> GetUserGroupsAsync(string? userid, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paged list of apis
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApisResponse<ApimResponse<ApiResponse>>?> GetApisAsync(int skip = 0, int take = 10, bool includeNext = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an individual API
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApimResponse<ApiResponse>?> GetApiAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an API definition by type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetApiAsync(string id, string type, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets operations for an api
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CollectionResponse<ApimResponse<OperationResponse>>?> GetOperations(string id, CancellationToken cancellationToken = default);
    }
}
