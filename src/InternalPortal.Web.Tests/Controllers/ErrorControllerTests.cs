using InternalPortal.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternalPortal.Web.Tests.Controllers
{
    [TestClass]
    public class ErrorControllerTests
    {
        private readonly Mock<ILogger<ErrorController>> _logger = new Mock<ILogger<ErrorController>>();

        public ErrorController GetController()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "";

            return new ErrorController(_logger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = context
                },
                Url = Mock.Of<IUrlHelper>()
            };
        }

        [TestMethod]
        public void ServerError_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = controller.ServerError();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void Status404_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = controller.Status404();

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }     
    }
}