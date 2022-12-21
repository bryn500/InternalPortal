using Apim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly IApimClient _client;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IApimClient client, ILogger<HomeController> logger)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            // no breadcrumbs on home page
            BreadCrumbs = null;
            return View();
        }

        [HttpGet("/accessibility")]
        public IActionResult Accessibility()
        {
            ViewData["Title"] = "Accessibility";
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Accessibility", "/accessibility"));
            return View();
        }

        [HttpGet("/terms")]
        public IActionResult Terms()
        {
            ViewData["Title"] = "Terms";
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Terms", "/terms"));
            return View();
        }

        [HttpGet("/cookies")]
        public IActionResult Cookies()
        {
            ViewData["Title"] = "Cookies";
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Cookies", "/cookies"));
            return View();
        }

        [HttpGet("/trial")]
        public async Task<string> Trial(CancellationToken cancellationToken)
        {
            return await _client.GetApis(cancellationToken: cancellationToken);
        }

        //[HttpGet("/asyncexamplewithcancel")]
        //public async Task<string> Get(CancellationToken cancellationToken)
        //{
        //    return "";
        //}
    }
}
