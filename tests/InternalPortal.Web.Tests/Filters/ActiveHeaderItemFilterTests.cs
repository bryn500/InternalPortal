using InternalPortal.Web.Controllers;
using InternalPortal.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace InternalPortal.Web.Tests.Filters
{
    [TestClass]
    public class ActiveHeaderItemFilterTests
    {
        [TestMethod]
        public void ActiveHeaderItemFilterAttribute_SetsViewBag()
        {
            //arrange
            var requiredHeader = "Test";
            var actionFilter = new ActiveHeaderItemFilterAttribute(requiredHeader);
            var mockLogger = new Mock<ILogger<HomeController>>();
            var homeController = new HomeController(mockLogger.Object);

            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            var actionExecutingContext = new ActionExecutingContext(actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                controller: homeController);

            //act
            actionFilter.OnActionExecuting(actionExecutingContext);

            //assert
            Assert.AreEqual(requiredHeader, homeController.ViewBag.ActiveHeaderItem);
        }
    }
}
