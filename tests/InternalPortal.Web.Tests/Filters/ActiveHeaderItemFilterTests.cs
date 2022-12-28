using InternalPortal.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using InternalPortal.Web.Controllers;

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
            var homeController = new HomeController(null);
            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext(httpContext,
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
