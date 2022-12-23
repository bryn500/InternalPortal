using Apim;
using InternalPortal.Web.Consts;
using InternalPortal.Web.Filters;
using InternalPortal.Web.Models.Auth;
using InternalPortal.Web.Models.Shared;
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
        private readonly IApimClient _client;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IApimClient client, ILogger<AccountController> logger)
        {
            _logger = logger;
            _client = client;
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
        public async Task<IActionResult> Login(SignInViewModel model, CancellationToken cancellationToken)
        {
            ViewData["Title"] = "Login";

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Error: " + ViewData["Title"];
                SetSignInFormModel(model);
                return View(model);
            }

            try
            {
                var principal = await GetLogin(model, cancellationToken);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("UserDetails", new { redirect = "/" });
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
        public async Task<IActionResult> UserDetails(string redirect, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(redirect))
                return NotFound();

            var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            try
            {
                if (id == null)
                    throw new AuthenticationException("Could not find user");

                List<Claim> extraClaims = await GetUserDetails(id.Value, cancellationToken);

                if (extraClaims.Any() && User.Identity != null)
                {
                    var identity = User.Identities.FirstOrDefault();

                    if (identity != null)
                        identity.AddClaims(extraClaims);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);
                }

                return Redirect(redirect);
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
            return Redirect("/");
        }

        [ActiveHeaderItemFilter(ActiveHeaderItem.Profile)]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            ViewData["Title"] = "Login";
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

        #region todo: move below logic out of controller
        private async Task<ClaimsPrincipal> GetLogin(SignInViewModel model, CancellationToken cancellationToken)
        {
            var auth = await _client.AuthAsync(model.Username, model.Password, cancellationToken);

            // todo: enforce single user session at a time with security stamp
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Authentication, auth.AccessToken),
                    new Claim(ClaimTypes.NameIdentifier, auth.Identifier)
                };

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
            );

            return principal;
        }

        private async Task<List<Claim>> GetUserDetails(string id, CancellationToken cancellationToken)
        {
            var userDetails = await _client.GetUserDetailsAsync(id, cancellationToken);
            var groups = await _client.GetUserGroupsAsync(id, cancellationToken);

            if (userDetails == null || userDetails.properties == null || userDetails.properties.state != "active" || id == null)
                throw new AuthenticationException("Could not find user");

            var claims = new List<Claim>();

            if (userDetails.properties.email != null)
                claims.Add(new Claim(ClaimTypes.Email, userDetails.properties.email));

            if (userDetails.properties.firstName != null)
                claims.Add(new Claim(ClaimTypes.GivenName, userDetails.properties.firstName));

            if (userDetails.properties.lastName != null)
                claims.Add(new Claim(ClaimTypes.Surname, userDetails.properties.lastName));

            if (userDetails.properties.firstName != null || userDetails.properties.lastName != null)
                claims.Add(new Claim(ClaimTypes.Name, $"{userDetails.properties.firstName} {userDetails.properties.lastName}".Trim()));

            var devGroup = groups?.value?.FirstOrDefault(x => x.name == "developers");
            if (devGroup != null)
                claims.Add(new Claim(CustomClaimTypes.Developer, "developer"));

            if (userDetails.properties.registrationDate != null)
                claims.Add(new Claim(CustomClaimTypes.RegistrationDate, userDetails.properties.registrationDate.Value.ToString()));

            return claims;
        }
        #endregion
    }
}
