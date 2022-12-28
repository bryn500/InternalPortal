using Apim.Models;

namespace Apim.Tests.Models
{
    [TestClass]
    public class LoginResponseTests
    {
        [TestMethod]
        public void LoginResponse_ParsesToken()
        {
            // arrange
            var token = "token=\"abc123\",refresh=\"true\"";
            var response = new LoginResponse(token, new UserIdResponse());

            // act
            var result = response.AccessToken;

            // assert
            Assert.AreEqual("abc123", result);
        }
    }
}