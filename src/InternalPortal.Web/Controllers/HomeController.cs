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

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            // no breadcrumbs on home page
            BreadCrumbs = null;
            return View();
        }

        [HttpGet("accessibility")]
        public IActionResult Accessibility()
        {
            ViewData["Title"] = "Accessibility";
            return View();
        }

        [HttpGet("terms")]
        public IActionResult Terms()
        {
            ViewData["Title"] = "Terms";
            return View();
        }

        [HttpGet("cookies")]
        public IActionResult Cookies()
        {
            ViewData["Title"] = "Cookies";
            return View();
        }
    }
}
