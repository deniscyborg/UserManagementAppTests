using NUnit.Framework;
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tests
{
    public class UserGridTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private TestConfig _config;

        [SetUp]
        public async Task SetUp()
        {
            _config = TestConfig.Load();
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [Test]
        public async Task CanSeeUsersInGrid()
        {
            var page = await _browser.NewPageAsync();
            var userGridPage = new UserGridPage(page);

            await userGridPage.GoToAsync(_config.BaseUrl);

            var users = await userGridPage.GetAllUserNamesAsync();
            Assert.Contains("Алексей", users);
        }
    }
}
