namespace UITests.PageObjectModels.Home
{
    public class CookiesPage
    {
        private readonly IWebDriver _driver;
        private const string pageURL = "cookies";
        public const string pageTitle = "Cookies";

        public CookiesPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void GoToCookiesPage(string appURL)
        {
            _driver.Navigate().GoToUrl($"{appURL}{pageURL}");
        }
    }
}
