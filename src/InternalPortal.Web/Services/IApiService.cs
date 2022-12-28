using InternalPortal.Web.Models.Apis;

namespace InternalPortal.Web.Services
{
    public interface IApiService
    {
        /// <summary>
        /// Gets all apis
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApisViewModel> GetApisAsync(int skip, int take, CancellationToken cancellationToken);

        /// <summary>
        /// Get an individual api details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApiViewModel?> GetApiAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Get an individual api defintion file response
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetApiAsync(string id, string type, CancellationToken cancellationToken);

        /// <summary>
        /// Get an individual api from unmananged list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApiViewModel?> GetOtherApiAsync(string id, CancellationToken cancellationToken);
    }
}
