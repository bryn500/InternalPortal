using Apim;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    [Route("[controller]")]
    public class ApisController : BaseController
    {
        private readonly IApimClient _client;
        private readonly ILogger<ApisController> _logger;

        public ApisController(IApimClient client, ILogger<ApisController> logger)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet("")]
        public async Task<IActionResult> Apis(CancellationToken cancellationToken)
        {
            var model = await _client.GetApis(cancellationToken: cancellationToken);

            return View(model);
        }
    }
}
