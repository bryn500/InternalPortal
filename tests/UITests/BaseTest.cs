using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace UITests
{
    public class BaseTest
    {
        /// <summary>
        /// Config for variables
        /// </summary>
        public IConfiguration Config { get; set; }

        /// <summary>
        /// Url used for testing - set locally or through env variable
        /// </summary>
        public string AppURL { get; set; }

        /// <summary>
        /// The driver used to control the browser
        /// </summary>
        public IWebDriver? Driver { get; set; }

        /// <summary>
        /// List of browsers for tests to use
        /// Firefox and IE are currently quite slow
        /// </summary>
        public static object[] TestBrowsers
        {
            get => new[]
            {
                //new object[] {"Chrome"},
                new object[] {"Edge"},
                //new object[] {"Firefox"}
            };
        }

        /// <summary>
        /// Set up config
        /// </summary>
        public BaseTest()
        {
            Config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();            

            var host = Config["TEST_HOST"];

            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("TEST_HOST");
            
            AppURL = host;
        }

        /// <summary>
        /// Creates driver based on browser
        /// </summary>
        /// <param name="browser"></param>
        public IWebDriver CreateDriver(string browser)
        {
            switch (browser)
            {
                case "Chrome":
                    Driver = new ChromeDriver(new ChromeOptions()
                    {
                    });
                    break;
                case "Firefox":
                    Driver = new FirefoxDriver(new FirefoxOptions
                    {
                        AcceptInsecureCertificates = true
                    });
                    break;
                case "Edge":
                    Driver = new EdgeDriver(new EdgeOptions()
                    {
                    });
                    break;
                default:
                    Driver = new ChromeDriver(new ChromeOptions()
                    {
                    });
                    break;
            }

            // Navigate to app
            Driver.Navigate().GoToUrl(AppURL);

            return Driver;
        }

        /// <summary>
        /// Called before every test
        /// </summary>
        [TestInitialize()]
        public void BaseTestInitialize()
        {
            // call test initialize, which can get overridden in each test case class
            TestInitialize();
        }

        /// <summary>
        /// Called after every test
        /// </summary>
        [TestCleanup()]
        public void BaseTestCleanup()
        {
            // call test cleanup, which can get overridden in each test case class
            TestCleanup();

            // universal cleanup: close & quit driver
            Driver?.Close();
            Driver?.Quit();
        }

        protected virtual void TestInitialize()
        {
            // override this method in your test class & write initialize steps
        }

        protected virtual void TestCleanup()
        {
            // override this method in your test class & write additional cleanup steps
        }
    }
}