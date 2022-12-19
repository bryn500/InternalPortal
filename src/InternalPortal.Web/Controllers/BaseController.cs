using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using InternalPortal.Web.Consts;

namespace InternalPortal.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public List<KeyValuePair<string, string>> MetaTags { get; set; } = new List<KeyValuePair<string, string>>();
        public List<KeyValuePair<string, string>>? BreadCrumbs { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("Home", "/")
        };


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.MetaTags = MetaTags;
            ViewBag.BreadCrumbs = BreadCrumbs;
            ViewBag.HideConsentMessage = Request.Cookies.ContainsKey(CookieNames.HideConsentMessage);

            base.OnActionExecuted(context);
        }

        /// <summary>  
        /// Send with the response to the client an instruction to add a cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void SetCookie(string key, string value, DateTimeOffset? expireTime, bool essential = false)
        {
            CookieOptions option = new CookieOptions()
            {
                IsEssential = essential,
                HttpOnly = true
            };

            if (expireTime.HasValue)
                option.Expires = expireTime.Value;

            Response.Cookies.Append(key, value, option);
        }

        /// <summary>  
        /// Send with the response to the client an instruction to delete the cookie with the specified key  
        /// </summary>  
        /// <param name="key">Cookie name</param>  
        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
