using InternalPortal.Web.Controllers;
using InternalPortal.Web.Models.Auth;
using InternalPortal.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InternalPortal.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        private readonly Mock<ILogger<AccountController>> _logger = new Mock<ILogger<AccountController>>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        public AccountController GetController()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object?)null));

            var services = new ServiceCollection();            
            services.AddSingleton(authServiceMock.Object);

            var context = new DefaultHttpContext()
            {
                RequestServices = services.BuildServiceProvider()            
            };
            context.Request.Scheme = "";

            context.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, "id")
            }));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(m => m.IsLocalUrl("/")).Returns(true);
            mockUrlHelper.Setup(m => m.IsLocalUrl("not-local")).Returns(false);

            var tempDataProvider = Mock.Of<ITempDataProvider>();
            var tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            var tempData = tempDataDictionaryFactory.GetTempData(context);

            _userService
                .Setup(x => x.GetUserDetails("id", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Claim>() {
                    new Claim(ClaimTypes.GivenName, "Bob")
                });

            return new AccountController(_userService.Object, _logger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = context
                },
                Url = mockUrlHelper.Object,
                TempData = tempData
            };
        }

        [TestMethod]
        public void Login_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = controller.Login();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public async Task Logout_ReturnsRedirect()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.Logout();

            // assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
        }

        [TestMethod]
        public void LogoutSuccess_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = controller.LogoutSuccess();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void Profile_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = controller.Profile();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public async Task LoginPost_WithEmptyModel_ReturnsError()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.Login(new SignInViewModel(), "");

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsTrue(viewResult.ViewData["Title"].ToString().StartsWith("Error"));
        }

        [TestMethod]
        public async Task LoginPost_WithValidModel_ReturnsRedirect()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.Login(new SignInViewModel() { Username = "user", Password = "pass" }, "");

            // assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
        }

        [TestMethod]
        public async Task UserDetails_ReturnsRedirect()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.UserDetails("/");

            // assert
            var redirectResult = result as RedirectResult;
            Assert.IsNotNull(redirectResult);
        }

        [TestMethod]
        public async Task UserDetails_ReturnsNotFound_IfBadReturnurl()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.UserDetails("not-local");

            // assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }
    }
}