namespace UITests.PageObjectModels.Home
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private const string pageURL = "";
        public const string pageTitle = "Home";

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void GoToHomePage(string appURL)
        {
            _driver.Navigate().GoToUrl($"{appURL}{pageURL}");
        }
    }
}
