using Apim.Models;
using System.Security.Authentication;

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

            // act
            var response = new LoginResponse(token, new UserIdResponse() { id = "a" });

            // assert
            Assert.AreEqual("abc123", response.AccessToken);
            Assert.AreEqual("a", response.Identifier);
        }

        [TestMethod]
        public void LoginResponse_Throws_WithNull()
        {
            try
            {
                _ = new LoginResponse("", null);
            }
            catch (AuthenticationException)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void LoginResponse_Throws_WithNoId()
        {
            try
            {
                _ = new LoginResponse("", new UserIdResponse());
            }
            catch (AuthenticationException)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void LoginResponse_Throws_WithBadAccessToken()
        {
            try
            {
                _ = new LoginResponse("####", new UserIdResponse() { id = "a"});
            }
            catch (AuthenticationException)
            {
                return;
            }

            Assert.Fail();
        }
    }
}