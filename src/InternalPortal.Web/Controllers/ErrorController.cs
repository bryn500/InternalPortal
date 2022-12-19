using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class ErrorController : BaseController
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ServerError")]
        public IActionResult ServerError()
        {
            ViewData["Title"] = "Sorry, there is a problem with the service";
            return View();
        }

        [HttpGet("Status404")]
        public IActionResult Status404()
        {
            ViewData["Title"] = "Page not found";
            return View();
        }
    }
}
