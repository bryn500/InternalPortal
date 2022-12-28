using Apim;
using InternalPortal.Web.Models.Apis;
using Microsoft.Extensions.Options;

namespace InternalPortal.Web.Services
{
    public class ApiService : IApiService
    {
        private readonly IApimClient _client;
        private readonly ApimOptions _options;

        public ApiService(IApimClient client, IOptions<ApimOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        /// <summary>
        /// Gets apis, including unmanaged apis if option set
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApisViewModel> GetApisAsync(int skip, int take, CancellationToken cancellationToken)
        {
            var apis = await _client.GetApisAsync(skip, take, _options.IncludeUnmanaged, cancellationToken);

            var model = new ApisViewModel(
                apis?.count,
                skip,
                take,
                apis?.value?.Select(x => new ApiViewModel(x.name, x.properties?.displayName, x.properties?.description, x.properties?.apiVersion)).ToList(),
                apis?.nextName
            );

            // do in parallel with Task.WhenAll if this becomes a real feature
            if (_options.IncludeUnmanaged)
                await AddUnmanagedApis(skip, model, cancellationToken);

            return model;
        }

        /// <summary>
        /// example to simulate listing non apim hosted Apis
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ApiViewModel>> GetOtherApisAsync(CancellationToken cancellationToken)
        {
            // todo: remove this if getting real data
            cancellationToken.ThrowIfCancellationRequested();

            return await Task.FromResult(new List<ApiViewModel>() {
                new ApiViewModel("unmanaged/aexample", "A - example non hosted API", "This is an example of an API we could list outside of APIM"),
                new ApiViewModel("unmanaged/mexample", "M - example non hosted API", "This is an example of an API we could list outside of APIM"),
                new ApiViewModel("unmanaged/zexample", "Z - example non hosted API", "This is an example of an API we could list outside of APIM"),
            });
        }

        /// <summary>
        /// Merges unmanaged and managed api lists
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task AddUnmanagedApis(int skip, ApisViewModel model, CancellationToken cancellationToken)
        {
            var otherApis = await GetOtherApisAsync(cancellationToken);

            if (otherApis != null && otherApis.Any())
            {
                bool firstPage = skip == 0;
                var first = model.Apis.FirstOrDefault()?.Name;
                var next = model.NextApiName;

                foreach (var item in otherApis)
                {
                    // compare strings alphabetically
                    var lessThanFirst = string.Compare(item.Name, first, StringComparison.Ordinal) < 0;
                    var moreThanNext = string.Compare(item.Name, next, StringComparison.Ordinal) > 0;

                    bool add = true;

                    // add to unmanged api to current page if alphabetically relevant
                    if (lessThanFirst && !firstPage)
                        add = false;
                    else if (!string.IsNullOrEmpty(next) && moreThanNext)
                        add = false;

                    if (add)
                        model.Apis.Add(item);
                }

                // order alphabetically
                model.Apis = model.Apis.OrderBy(x => x.Name).ToList();
            }
        }

        /// <summary>
        /// Get an individual api details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiViewModel?> GetApiAsync(string id, CancellationToken cancellationToken)
        {
            var apiResponse = await _client.GetApiAsync(id, cancellationToken);

            if (apiResponse == null)
                return null;

            var apiOperations = await _client.GetOperations(id, cancellationToken);

            return new ApiViewModel(apiResponse.name, apiResponse.properties?.displayName, apiResponse.properties?.description, apiResponse.properties?.apiVersion, apiOperations?.value);
        }

        /// <summary>
        /// Get an individual api defintion file response
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> GetApiAsync(string id, string type, CancellationToken cancellationToken)
        {
            return _client.GetApiAsync(id, type, cancellationToken);
        }

        /// <summary>
        /// Get an individual api from unmananged list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiViewModel?> GetOtherApiAsync(string id, CancellationToken cancellationToken)
        {
            var apis = await GetOtherApisAsync(cancellationToken);
            return apis.FirstOrDefault(x => x.Id == $"unmanaged/{id}");
        }
    }
}
