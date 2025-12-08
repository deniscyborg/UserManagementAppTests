using NUnit.Framework;
using Allure.NUnit.Attributes;
using Allure.Net.Commons; 
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    [AllureNUnit]
    public class UserGridTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _browserContext;
        private TestConfig _config;

        [SetUp]
        public async Task SetUp()
        {
            _config = TestConfig.Load();
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            _browserContext = await _browser.NewContextAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (_browserContext != null)
                await _browserContext.CloseAsync();
            if (_browser != null)
                await _browser.CloseAsync();
            if (_playwright != null)
                _playwright.Dispose();
        }

        [Test]
        public async Task SmokeOpenUserGrid()
        {
            var page = await _browserContext.NewPageAsync();
            var usersGrid = new UserGridPage(page);

            // Только переходим на страницу грида (идёт базовая smoke-проверка)
            await usersGrid.GoToAsync(_config.BaseUrl);
            Assert.Pass("User grid opened successfully");
        }
    }
}
