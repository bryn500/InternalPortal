using InternalPortal.Web.Extensions;

namespace InternalPortal.Web.Tests.Extensions
{
    [TestClass]
    public class DateTimeExtensionTests
    {
        [TestMethod]
        public void ToGdsString_UsesCorrectFormat()
        {
            // arrange
            var date = new DateTime(2022, 6, 13, 23, 59, 59, DateTimeKind.Utc);
            var date2 = new DateTime(2022, 6, 13, 23, 59, 59, DateTimeKind.Unspecified);

            // act
            var result = date.ToGdsString();
            var result2 = date2.ToGdsString();

            // assert
            Assert.AreEqual("14 June 2022", result);
            Assert.AreEqual("13 June 2022", result2);
        }
    }
}
