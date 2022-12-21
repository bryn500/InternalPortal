using Apim;
using InternalPortal.Web.Models.Auth;
using InternalPortal.Web.Models.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternalPortal.Web.Controllers
{
    [Route("[controller]")]
    public class AccountController : BaseController
    {
        private readonly IApimClient _client;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IApimClient client, ILogger<AccountController> logger)
        {
            _logger = logger;
            _client = client;
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            ViewData["Title"] = "Sign In";

            BreadCrumbs?.Add(new KeyValuePair<string, string>("Sign In", "/signin"));

            var model = new SignInViewModel();
            SetSignInFormModel(model);

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(SignInViewModel model, CancellationToken cancellationToken)
        {
            ViewData["Title"] = "Sign In";
            BreadCrumbs?.Add(new KeyValuePair<string, string>("Sign In", "/signin"));

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Error: " + ViewData["Title"];
                SetSignInFormModel(model);
                return View(model);
            }

            try
            {
                var result = await _client.Auth(model.Username, model.Password, cancellationToken);
                // todo: second call to get user details for profile
                // todo: enforce single user session at a time
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.Identifier),
                    new Claim(ClaimTypes.Authentication, result.AccessToken),
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Redirect("/");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        private void SetSignInFormModel(SignInViewModel model)
        {
            model.UsernameQuestion = new QuestionViewModel()
            {
                Question = "Username",
                Hint = "This will be your email address",
                Type = "text"
            };

            model.PasswordQuestion = new QuestionViewModel()
            {
                Question = "Password",
                Type = "password"
            };
        }
    }
}
