namespace UITests.PageObjectModels.Home
{
    public class AccessibilityPage
    {
        private readonly IWebDriver _driver;
        private const string pageURL = "accessibility";
        public const string pageTitle = "Accessibility";

        public AccessibilityPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void GoToAccessibilityPage(string appURL)
        {
            _driver.Navigate().GoToUrl($"{appURL}{pageURL}");
        }
    }
}
