using UITests.PageObjectModels.Home;

namespace UITests.Tests
{
    [TestClass]
    public class BasicTests : BaseTest
    {
        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(TestBrowsers), typeof(BaseTest), DynamicDataSourceType.Property)]
        public void CheckHomePage(string browser)
        {
            // arrange
            var driver = CreateDriver(browser);
            var homepage = new HomePage(driver);

            // act
            homepage.GoToHomePage(AppURL);

            // assert
            Assert.IsTrue(driver.Title.StartsWith(HomePage.pageTitle));
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(TestBrowsers), typeof(BaseTest), DynamicDataSourceType.Property)]
        public void CheckTermsPage(string browser)
        {
            // arrange
            var driver = CreateDriver(browser);
            var termsPage = new TermsPage(driver);

            // act
            termsPage.GoToTermsPage(AppURL);

            // assert
            Assert.IsTrue(driver.Title.StartsWith(TermsPage.pageTitle));

        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(TestBrowsers), typeof(BaseTest), DynamicDataSourceType.Property)]
        public void CheckAccessibilityPage(string browser)
        {
            // arrange
            var driver = CreateDriver(browser);
            var accessibilityPage = new AccessibilityPage(driver);

            // act
            accessibilityPage.GoToAccessibilityPage(AppURL);

            // assert
            Assert.IsTrue(driver.Title.StartsWith(AccessibilityPage.pageTitle));
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(TestBrowsers), typeof(BaseTest), DynamicDataSourceType.Property)]
        public void CheckCookiesPage(string browser)
        {
            // arrange
            var driver = CreateDriver(browser);
            var cookiesPage = new CookiesPage(driver);

            // act
            cookiesPage.GoToCookiesPage(AppURL);

            // assert
            Assert.IsTrue(driver.Title.StartsWith(CookiesPage.pageTitle));
        }
    }
}
