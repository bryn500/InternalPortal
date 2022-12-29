using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace Apim.Tests
{
    [TestClass]
    public class ApimClientTests
    {
        private const string TestUserAccessToken = "user-id&2022&###==";

        private ApimClient GetClient(string response, bool addAuthHeaderResponse = false)
        {
            var responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response)
            };

            if (addAuthHeaderResponse)
                responseMessage.Headers.Add("Ocp-Apim-Sas-Token", $"token=\"{TestUserAccessToken}\",refresh=\"true\"");

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(responseMessage)
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://example.com/"),
            };

            return new ApimClient(httpClient, Options.Create(new ApimOptions()));
        }

        [TestMethod]
        public async Task AuthAsync_Deserializes()
        {
            // arrange
            var apimClient = GetClient(ApimResponses.AuthResponse, true);

            // act
            var result = await apimClient.AuthAsync("username", "password");

            // assert
            Assert.AreEqual("user id", result?.Identifier);
            Assert.AreEqual(TestUserAccessToken, result?.AccessToken);
        }

        [TestMethod]
        public async Task GetUserDetailsAsync_Deserializes()
        {
            // arrange
            var apimClient = GetClient(ApimResponses.UserResponse);

            // act
            var result = await apimClient.GetUserDetailsAsync("id");

            // assert
            Assert.AreEqual("users id", result?.name);
            Assert.AreEqual("users first name", result?.properties?.firstName);
            Assert.AreEqual("users last name", result?.properties?.lastName);
            Assert.AreEqual("active", result?.properties?.state);
            Assert.AreEqual("users email", result?.properties?.email);
            Assert.IsNotNull(result?.properties?.registrationDate);
            Assert.AreEqual(2020, result?.properties?.registrationDate.Value.Year);
        }

        [TestMethod]
        public async Task GetUserGroupsAsync_Deserializes()
        {
            // arrange
            var apimClient = GetClient(ApimResponses.GroupsResponse);

            // act
            var result = await apimClient.GetUserGroupsAsync("id");

            // assert
            Assert.AreEqual("next page url", result?.nextLink);
            Assert.AreEqual(1, result?.count);
            Assert.AreEqual(1, result?.value?.Count);
            Assert.AreEqual("groupname", result?.value?.First().name);
            Assert.AreEqual("group display name", result?.value?.First().properties?.displayName);
            Assert.AreEqual("group type", result?.value?.First().properties?.type);
            Assert.AreEqual("group description", result?.value?.First().properties?.description);
        }

        [TestMethod]
        public async Task GetApisAsync_Deserializes()
        {
            // arrange
            var apimClient = GetClient(ApimResponses.ApisResponse);

            // act
            var result = await apimClient.GetApisAsync();

            // assert
            Assert.AreEqual("next link", result?.nextLink);
            Assert.IsNull(result?.nextName);
            Assert.AreEqual(100, result?.count);
            Assert.AreEqual(2, result?.value?.Count);
            Assert.AreEqual("api-name", result?.value?.First().name);
        }

        [TestMethod]
        public async Task GetApiAsync_Deserializes()
        {
            // arrange
            var apimClient = GetClient(ApimResponses.ApiResponse);

            // act
            var result = await apimClient.GetApiAsync("id");

            // assert
            Assert.AreEqual("api-name", result?.name);
            Assert.AreEqual("api display name", result?.properties?.displayName);
            Assert.AreEqual("v1", result?.properties?.apiVersion);
            Assert.AreEqual("api description", result?.properties?.description);
        }

        [TestMethod]
        public async Task GetApiAsync_GetsDefinition()
        {
            // arrange
            var apimClient = GetClient(ApimResponses.ApiResponse, false);

            // act
            var result = await apimClient.GetApiAsync("id", "application/vnd.oai.openapi");

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetOperations_Deserializes()
        {
            // arrange
            var apimClient = GetClient(ApimResponses.OperationsResponse);

            // act
            var result = await apimClient.GetOperations("id");

            // assert
            Assert.AreEqual("next-link", result?.nextLink);
            Assert.AreEqual(15, result?.count);
            Assert.AreEqual(2, result?.value?.Count);
            Assert.AreEqual("op-name", result?.value?.First().name);
        }
    }
}
