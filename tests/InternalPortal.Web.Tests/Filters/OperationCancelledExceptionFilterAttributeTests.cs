using InternalPortal.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;

namespace InternalPortal.Web.Tests.Filters
{
    [TestClass]
    public class OperationCancelledExceptionFilterAttributeTests
    {
        [TestMethod]
        public void OperationCancelledExceptionFilterAttribute_HandlesOperationCanceledExceptionException()
        {
            //arrange
            var actionFilter = new OperationCancelledExceptionFilterAttribute(new NullLoggerFactory());
            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext(httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>());
            exceptionContext.Exception = new OperationCanceledException();

            //act
            actionFilter.OnException(exceptionContext);

            //assert
            var result = (StatusCodeResult)exceptionContext?.Result;
            Assert.IsTrue(exceptionContext.ExceptionHandled);
            Assert.AreEqual(499, result.StatusCode);
        }

        [TestMethod]
        public void OperationCancelledExceptionFilterAttribute_IgnoresNonOperationCanceledExceptionException()
        {
            //arrange
            var actionFilter = new OperationCancelledExceptionFilterAttribute(new NullLoggerFactory());
            var httpContext = new DefaultHttpContext();

            var actionContext = new ActionContext(httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>());
            exceptionContext.Exception = new Exception();

            //act
            actionFilter.OnException(exceptionContext);

            //assert
            Assert.IsFalse(exceptionContext.ExceptionHandled);
            Assert.IsFalse(exceptionContext?.Result is StatusCodeResult);
        }
    }
}
