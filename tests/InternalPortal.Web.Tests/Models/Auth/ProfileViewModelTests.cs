using InternalPortal.Web.Consts;
using InternalPortal.Web.Models.Auth;
using System.Security.Claims;

namespace InternalPortal.Web.Tests.Models.Auth
{
    [TestClass]
    public class ProfileViewModelTests
    {
        [TestMethod]
        public void ProfileViewModel_SetsClaims()
        {
            // arrange
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.GivenName, "a"),
                new Claim(ClaimTypes.Surname, "b"),
                new Claim(ClaimTypes.Name, "c"),
                new Claim(ClaimTypes.Email, "d"),
                new Claim(CustomClaimTypes.RegistrationDate, "2020-06-12T23:59:59.3270000Z"),
                new Claim(CustomClaimTypes.Developer, "developer"),
            };

            // act
            var model = new ProfileViewModel(claims);

            // assert
            Assert.AreEqual("a", model.FirstName);
            Assert.AreEqual("b", model.LastName);
            Assert.AreEqual("c", model.FullName);
            Assert.AreEqual("d", model.Email);
            Assert.AreEqual("13 June 2020", model.RegistrationDate);
            Assert.AreEqual(true, model.Developer);
        }
    }
}
