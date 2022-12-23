using Apim;
using Apim.Models;
using InternalPortal.Web.Consts;
using InternalPortal.Web.Filters;
using InternalPortal.Web.Models.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternalPortal.Web.Controllers
{
    [ActiveHeaderItemFilter(ActiveHeaderItem.Apis)]
    [Route("[controller]")]
    public class ApisController : BaseController
    {
        private readonly IApimClient _client;
        private readonly ApimOptions _options;
        private readonly ILogger<ApisController> _logger;

        public ApisController(IApimClient client, IOptions<ApimOptions> options, ILogger<ApisController> logger)
        {
            _client = client;
            _options = options.Value;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Apis(int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        {
            ViewData["Title"] = "Apis";

            // validate user input
            take = Math.Max(Math.Min(take, 64), 1);
            skip = Math.Max(skip, 0);

            var managedApis = await _client.GetApisAsync(skip, take, _options.IncludeUnmanaged, cancellationToken);

            var model = new ApisViewModel(
                managedApis?.count,
                skip,
                take,
                managedApis?.value?.Select(x => new ApiViewModel(x.name, x.properties?.displayName, x.properties?.description, x.properties?.apiVersion)).ToList()
            );

            // do in parallel with Task.WhenAll if this becomes a real feature
            if (_options.IncludeUnmanaged)
                await AddUnmanagedApis(skip, managedApis, model, cancellationToken);

            return View(model);
        }

        private async Task AddUnmanagedApis(int skip, ApisResponse<ApimResponse<ApiResponse>>? managedApis, ApisViewModel model, CancellationToken cancellationToken)
        {
            var otherApis = await GetOtherApisAsync(cancellationToken);

            if (otherApis != null && otherApis.Any())
            {
                bool firstPage = skip == 0;
                var first = model.Apis.FirstOrDefault()?.Name;
                var next = managedApis?.nextName;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Api(string id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _client.GetApiAsync(id, cancellationToken);

            if (apiResponse == null)
                return NotFound();

            var apiOperations = await _client.GetOperations(id, cancellationToken);

            var api = new ApiViewModel(apiResponse.name, apiResponse.properties?.displayName, apiResponse.properties?.description, apiResponse.properties?.apiVersion, apiOperations?.value);
            ViewData["Title"] = api.Name;
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Apis", "/apis"));

            return View(api);
        }

        [HttpGet("{id}/definition")]
        public async Task<IActionResult> ApiDefinition(string id, string type, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(type))
                return NotFound();

            var responseMessage = await _client.GetApiAsync(id, type, cancellationToken);

            Response.StatusCode = (int)responseMessage.StatusCode;

            Response.Headers.Add("content-disposition", "attachment; filename=\"" + id + ".yml\"");

            await responseMessage.Content.CopyToAsync(Response.Body);

            return Ok();
        }

        [HttpGet("unmanaged/{id}")]
        public async Task<IActionResult> UnmanagedApi(string id, CancellationToken cancellationToken = default)
        {
            var apis = await GetOtherApisAsync(cancellationToken);
            var api = apis.FirstOrDefault(x => x.Id == $"unmanaged/{id}");

            if (api == null)
                return NotFound();

            ViewData["Title"] = api.Name;
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Apis", "/apis"));

            return View(api);
        }

        // example to simulate listing non apim hosted Apis
        private async Task<List<ApiViewModel>> GetOtherApisAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ApiViewModel>() {
                new ApiViewModel("unmanaged/aexample", "A - example non hosted API", "This is an example of an API we could list outside of APIM"),
                new ApiViewModel("unmanaged/mexample", "M - example non hosted API", "This is an example of an API we could list outside of APIM"),
                new ApiViewModel("unmanaged/zexample", "Z - example non hosted API", "This is an example of an API we could list outside of APIM"),
            });
        }
    }
}
