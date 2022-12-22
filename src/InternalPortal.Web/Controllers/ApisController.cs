using Apim;
using InternalPortal.Web.Models.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternalPortal.Web.Controllers
{
    [Route("[controller]")]
    public class ApisController : BaseController
    {
        private readonly IApimClient _client;
        private readonly ApimOptions _apimOptions;
        private readonly ILogger<ApisController> _logger;

        public ApisController(IApimClient client, IOptions<ApimOptions> apimOptions, ILogger<ApisController> logger)
        {
            _client = client;
            _apimOptions = apimOptions.Value;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Apis(int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        {
            ViewData["Title"] = "Apis";

            var managedApisTask = _client.GetApisAsync(skip, take, cancellationToken);
            var otherApisTask = GetOtherApisAsync(cancellationToken);
            await Task.WhenAll(managedApisTask, otherApisTask);

            var managedApis = await managedApisTask;
            var otherApis = await otherApisTask;

            if (managedApis != null && managedApis.value.Any())
            {
                otherApis.AddRange(managedApis.value.Select(x => new Api(x.name, x.properties.displayName, x.properties.description, x.properties.apiVersion)));

                // todo: merge lists together but keep alphabetical order while allowing paging of apim apis
                //bool morePages = !string.IsNullOrEmpty(managedApis.nextLink);
                //var lastApi = managedApis.value.Last();
                //if (morePages)
                //{
                //    var lastInNewList = otherApis.First(x => x.Id == lastApi.id);
                //    var last = otherApis.IndexOf(lastInNewList);
                //    otherApis = otherApis.Take(last).ToList();
                //}
            }

            return View(otherApis.OrderBy(x => x.Name).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Api(string id, CancellationToken cancellationToken = default)
        {
            var apiResponse = await _client.GetApiAsync(id, cancellationToken);

            if (apiResponse == null)
                return NotFound();

            var api = new Api(apiResponse.name, apiResponse.properties.displayName, apiResponse.properties.description, apiResponse.properties.apiVersion);
            ViewData["Title"] = api.Name;
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Apis", "/apis"));

            return View(api);
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
        private async Task<List<Api>> GetOtherApisAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<Api>() {
                new Api("unmanaged/anexample", "An example non hosted API", "This is an example of an API we could list outside of APIM")
            });
        }
    }
}
