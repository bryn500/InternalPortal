using Apim.Models;

namespace Apim
{
    public interface IApimClient
    {
        Task<LoginResponse> Auth(string? userName, string? password, CancellationToken cancellationToken = default);

        /// <summary>
        /// https://learn.microsoft.com/en-us/rest/api/apimanagement/current-ga/apis/list-by-service?tabs=HTTP
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<string> GetApis(int skip = 0, int take = 10, CancellationToken cancellationToken = default);
    }
}
