using InternalPortal.Web.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace InternalPortal.Web.Tests.Extensions
{
    [TestClass]
    public class ValidationExtensionTests
    {
        [TestMethod]
        public void FieldHasError_ReturnsTrue_WithError()
        {
            // arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("a", "b");
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary<string>(modelMetadataProvider, modelState);

            // act
            var result = viewData.FieldHasError("a");

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FieldHasError_ReturnsFalse_WithoutError()
        {
            // arrange
            var modelState = new ModelStateDictionary();
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary<string>(modelMetadataProvider, modelState);

            // act
            var result = viewData.FieldHasError("a");

            // assert
            Assert.IsFalse(result);
        }
    }
}
