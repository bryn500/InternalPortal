using InternalPortal.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternalPortal.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _logger = new Mock<ILogger<HomeController>>();

        public HomeController GetController()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "";

            return new HomeController(_logger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = context
                },
                Url = Mock.Of<IUrlHelper>()
            };
        }

        [TestMethod]
        public void Home_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = controller.Index();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void Terms_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = controller.Terms();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void Accessibility_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = controller.Accessibility();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void Cookies_ReturnsView()
        {
            // arrange
            var controller = new HomeController(_logger.Object);

            // act
            var result = controller.Cookies();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}