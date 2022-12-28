using InternalPortal.Web.Controllers;
using InternalPortal.Web.Models.Apis;
using InternalPortal.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternalPortal.Web.Tests.Controllers
{
    [TestClass]
    public class ApisControllerTests
    {
        private readonly Mock<ILogger<ApisController>> _logger = new Mock<ILogger<ApisController>>();
        private readonly Mock<IApiService> _apiService = new Mock<IApiService>();

        private const string FoundId = "id";
        private const string NotFoundId = "null";

        public ApisController GetController()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "";

            _apiService
                .Setup(x => x.GetApisAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApisViewModel(10, 0, 5));

            _apiService
                .Setup(x => x.GetApiAsync(FoundId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiViewModel(null, null, null));

            _apiService
                .Setup(x => x.GetApiAsync(NotFoundId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ApiViewModel?)null);

            _apiService
                .Setup(x => x.GetApiAsync(FoundId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage());

            _apiService
                .Setup(x => x.GetOtherApiAsync(FoundId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiViewModel(null, null, null));

            _apiService
                .Setup(x => x.GetOtherApiAsync(NotFoundId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ApiViewModel?)null);

            return new ApisController(_apiService.Object, _logger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = context
                },
                Url = Mock.Of<IUrlHelper>()
            };
        }

        [TestMethod]
        public async Task Apis_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.Apis(-1, 0);
            _ = await controller.Apis(10, 100);

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            _apiService.Verify(x => x.GetApisAsync(0, 1, It.IsAny<CancellationToken>()), Times.Once);
            _apiService.Verify(x => x.GetApisAsync(10, 64, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Api_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.Api(FoundId);

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            _apiService.Verify(x => x.GetApiAsync(FoundId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Api_ReturnsNotFound_IfNoApi()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.Api(NotFoundId);

            // assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            _apiService.Verify(x => x.GetApiAsync(NotFoundId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task ApiDefinition_ReturnsResponse()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.ApiDefinition(FoundId, "abc");

            // assert
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);

            _apiService.Verify(x => x.GetApiAsync(FoundId, "abc", It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task ApiDefinition_ReturnsNotFound_WithNoType()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.ApiDefinition(FoundId, "");

            // assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            _apiService.Verify(x => x.GetApiAsync(FoundId, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [TestMethod]
        public async Task UnmanagedApi_ReturnsView()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.UnmanagedApi(FoundId);

            // assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            _apiService.Verify(x => x.GetOtherApiAsync(FoundId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task UnmanagedApi_ReturnsNotFound_IfNoApi()
        {
            // arrange
            var controller = GetController();

            // act
            var result = await controller.UnmanagedApi(NotFoundId);

            // assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            _apiService.Verify(x => x.GetOtherApiAsync(NotFoundId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}