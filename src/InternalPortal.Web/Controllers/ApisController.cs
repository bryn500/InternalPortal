using InternalPortal.Web.Consts;
using InternalPortal.Web.Filters;
using InternalPortal.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    [ActiveHeaderItemFilter(ActiveHeaderItem.Apis)]
    [Route("[controller]")]
    public class ApisController : BaseController
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ApisController> _logger;

        public ApisController(IApiService apiService, ILogger<ApisController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Apis(int skip = 0, int take = 10, CancellationToken cancellationToken = default)
        {
            ViewData["Title"] = "Apis";

            // validate user input
            take = Math.Max(Math.Min(take, 64), 1);
            skip = Math.Max(skip, 0);

            var model = await _apiService.GetApisAsync(skip, take, cancellationToken);            

            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Api(string id, CancellationToken cancellationToken = default)
        {
            var api = await _apiService.GetApiAsync(id, cancellationToken);

            if (api == null)
                return NotFound();

            ViewData["Title"] = api.Name;
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Apis", "/apis"));

            return View(api);
        }

        [HttpGet("{id}/definition")]
        public async Task<IActionResult> ApiDefinition(string id, string type, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(type))
                return NotFound();

            var responseMessage = await _apiService.GetApiAsync(id, type, cancellationToken);

            Response.StatusCode = (int)responseMessage.StatusCode;

            Response.Headers.Add("content-disposition", "attachment; filename=\"" + id + ".yml\"");

            await responseMessage.Content.CopyToAsync(Response.Body, cancellationToken);

            return Ok();
        }

        [HttpGet("unmanaged/{id}")]
        public async Task<IActionResult> UnmanagedApi(string id, CancellationToken cancellationToken = default)
        {
            var api = await _apiService.GetOtherApiAsync(id, cancellationToken);

            if (api == null)
                return NotFound();

            ViewData["Title"] = api.Name;
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Apis", "/apis"));

            return View(api);
        }        
    }
}
