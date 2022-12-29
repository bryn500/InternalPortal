using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Security.Claims;

namespace Apim.Tests
{
    public class TestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    [TestClass]
    public class ApimDefaultHandlerTests
    {
        private const string AccessToken = "test";

        private HttpMessageInvoker GetInvoker(IOptions<ApimOptions>? options = null, bool addUserDetails = false)
        {
            var context = new DefaultHttpContext();

            if (addUserDetails)
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() {
                    new Claim(ClaimTypes.Authentication, AccessToken)
                }));
            }

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            if (options == null)
                options = Options.Create(new ApimOptions());

            var invoker = new HttpMessageInvoker(new ApimDefaultHandler(new TestHandler(), mockHttpContextAccessor.Object, options));

            return invoker;
        }

        [TestMethod]
        public async Task ApimDefaultHandler_AddsApiVersionToQueryString()
        {
            // arrange
            var options = Options.Create(new ApimOptions()
            {
                ManagementApiVersion = "abc"
            });

            using var invoker = GetInvoker(options);
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://example.com/");

            // act
            using var result = await invoker.SendAsync(httpRequestMessage, new CancellationToken());

            // assert
            Assert.AreEqual("https://example.com/?api-version=abc", httpRequestMessage.RequestUri?.AbsoluteUri);
        }

        [TestMethod]
        public async Task ApimDefaultHandler_DoesNotDuplicateApiVersionQueryString()
        {
            // arrange
            var options = Options.Create(new ApimOptions()
            {
                ManagementApiVersion = "abc"
            });

            using var invoker = GetInvoker(options);
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://example.com/?api-version=abc");

            // act
            using var result = await invoker.SendAsync(httpRequestMessage, new CancellationToken());

            // assert
            Assert.AreEqual("https://example.com/?api-version=abc", httpRequestMessage.RequestUri?.AbsoluteUri);
        }


        [TestMethod]
        public async Task ApimDefaultHandler_DoesNotAddAuthHeader_WithNoUser()
        {
            // arrange
            using var invoker = GetInvoker();
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://example.com/");

            // act
            using var result = await invoker.SendAsync(httpRequestMessage, new CancellationToken());

            // assert
            Assert.IsTrue(!httpRequestMessage.Headers.Contains(ApimDefaultHandler.AuthHeaderName));
        }

        [TestMethod]
        public async Task ApimDefaultHandler_AddsAuthHeader_WithUser()
        {
            // arrange
            using var invoker = GetInvoker(addUserDetails: true);
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://example.com/");

            // act
            using var result = await invoker.SendAsync(httpRequestMessage, new CancellationToken());

            // assert
            Assert.AreEqual($"{ApimDefaultHandler.AuthHeaderPrefix}{AccessToken}", httpRequestMessage.Headers.GetValues(ApimDefaultHandler.AuthHeaderName).First());
        }
    }
}
