using InternalPortal.Web.Consts;
using InternalPortal.Web.Filters;
using InternalPortal.Web.Models.Auth;
using InternalPortal.Web.Models.Shared;
using InternalPortal.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;

namespace InternalPortal.Web.Controllers
{
    [Route("[controller]")]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserService userService, ILogger<AccountController> logger)
        {
            _logger = logger;
            _userService = userService;
        }

        [ActiveHeaderItemFilter(ActiveHeaderItem.Login)]
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";

            var model = new SignInViewModel();
            SetSignInFormModel(model);

            return View(model);
        }

        [ActiveHeaderItemFilter(ActiveHeaderItem.Login)]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(SignInViewModel model, string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            ViewData["Title"] = "Login";

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                ViewData["Title"] = "Error: " + ViewData["Title"];
                SetSignInFormModel(model);
                return View(model);
            }

            try
            {
                var principal = await _userService.GetLogin(model.Username, model.Password, cancellationToken);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                    returnUrl = "/";

                return RedirectToAction("UserDetails", new { returnUrl });
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                await HttpContext.SignOutAsync();
                throw;
            }
        }

        [HttpGet("userdetails")]
        public async Task<IActionResult> UserDetails(string returnUrl, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return NotFound();

            var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            try
            {
                if (id == null)
                    throw new AuthenticationException("Could not find user");

                List<Claim> extraClaims = await _userService.GetUserDetails(id.Value, cancellationToken);

                if (extraClaims.Any() && User.Identity != null)
                {
                    var identity = User.Identities.FirstOrDefault();

                    if (identity != null)
                        identity.AddClaims(extraClaims);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);
                }

                return Redirect(returnUrl);
            }
            catch (Exception)
            {
                await HttpContext.SignOutAsync();
                throw;
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("LogoutSuccess");
        }

        [AllowAnonymous]
        [HttpGet("logout-success")]
        public IActionResult LogoutSuccess()
        {
            ViewData["Title"] = "Logout successful";
            return View();
        }

        [ActiveHeaderItemFilter(ActiveHeaderItem.Profile)]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            ViewData["Title"] = "Profile";
            var model = new ProfileViewModel(User.Claims);
            return View(model);
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
