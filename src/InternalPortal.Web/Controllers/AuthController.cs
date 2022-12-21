using Apim;
using InternalPortal.Web.Models.Auth;
using InternalPortal.Web.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly ApimClient _client;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ApimClient client, ILogger<AuthController> logger)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet("/signin")]
        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign In";

            BreadCrumbs?.Add(new KeyValuePair<string, string>("Sign In", "/signin"));

            var model = new SignInViewModel();
            SetSignInFormModel(model);

            return View(model);
        }

        [HttpPost("/signin")]
        public async Task<IActionResult> SignIn(SignInViewModel model, CancellationToken cancellationToken)
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

                return Json(result);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public void SetSignInFormModel(SignInViewModel model)
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
