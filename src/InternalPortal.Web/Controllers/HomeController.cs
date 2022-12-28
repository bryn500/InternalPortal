using InternalPortal.Web.AppStart;
using InternalPortal.Web.Consts;
using InternalPortal.Web.Filters;
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

        [ActiveHeaderItemFilter(ActiveHeaderItem.Home)]
        [ResponseCache(CacheProfileName = CacheProfiles.Default)]
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            // no breadcrumbs on home page
            BreadCrumbs = null;
            return View();
        }

        [ResponseCache(CacheProfileName = CacheProfiles.Default)]
        [HttpGet("accessibility")]
        public IActionResult Accessibility()
        {
            ViewData["Title"] = "Accessibility";
            return View();
        }

        [ResponseCache(CacheProfileName = CacheProfiles.Default)]
        [HttpGet("terms")]
        public IActionResult Terms()
        {
            ViewData["Title"] = "Terms";
            return View();
        }

        [ResponseCache(CacheProfileName = CacheProfiles.Default)]
        [HttpGet("cookies")]
        public IActionResult Cookies()
        {
            ViewData["Title"] = "Cookies";
            return View();
        }
    }
}
