using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        //[HttpGet("/asyncexamplewithcancel")]
        //public async Task<string> Get(CancellationToken cancellationToken)
        //{
        //    return "";
        //}
    }
}
