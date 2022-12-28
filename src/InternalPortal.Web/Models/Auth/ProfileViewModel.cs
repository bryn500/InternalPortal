using InternalPortal.Web.Consts;
using InternalPortal.Web.Extensions;
using System.Security.Claims;

namespace InternalPortal.Web.Models.Auth
{
    public class ProfileViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? RegistrationDate { get; set; }
        public bool Developer { get; set; }

        public ProfileViewModel(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.GivenName:
                        FirstName = claim.Value;
                        break;
                    case ClaimTypes.Surname:
                        LastName = claim.Value;
                        break;
                    case ClaimTypes.Name:
                        FullName = claim.Value;
                        break;
                    case ClaimTypes.Email:
                        Email = claim.Value;
                        break;
                    case CustomClaimTypes.RegistrationDate:
                        var success = DateTime.TryParse(claim.Value, out DateTime date);
                        if (success) RegistrationDate = date.ToGdsString();
                        break;
                    case CustomClaimTypes.Developer:
                        Developer = true;
                        break;
                }
            }
        }
    }
}
