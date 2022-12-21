using InternalPortal.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InternalPortal.Web.Models.Auth
{
    public class SignInViewModel
    {
        public QuestionViewModel? UsernameQuestion { get; set; }

        [BindProperty, Required(ErrorMessage = "Enter your 'Username'"), StringLength(maximumLength: 100, MinimumLength = 3)]
        public string? Username { get; set; }

        public QuestionViewModel? PasswordQuestion { get; set; }

        [BindProperty, Required(ErrorMessage = "Enter your 'Password'"), StringLength(maximumLength: 100, MinimumLength = 7)]
        public string? Password { get; set; }
    }
}
