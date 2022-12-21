namespace UITests.PageObjectModels.Home
{
    public class TermsPage
    {
        private readonly IWebDriver _driver;
        private const string pageURL = "terms";
        public const string pageTitle = "Terms";

        public TermsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void GoToTermsPage(string appURL)
        {
            _driver.Navigate().GoToUrl($"{appURL}{pageURL}");
        }
    }
}
