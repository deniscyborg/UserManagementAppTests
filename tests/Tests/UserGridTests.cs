using NUnit.Framework;
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;
using System.Collections.Generic;
using Allure.NUnit.Attributes;           // <--- Добавлено
using Allure.Net.Commons;                // <--- Добавлено

namespace Tests
{
    [AllureSuite("UI: User Grid")]       // <--- Название группы тестов для отчёта
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
        [AllureTag("ui", "grid", "users")]                  // <--- Теги для Allure
        [AllureSeverity(SeverityLevel.normal)]              // <--- Важность теста
        [AllureOwner("denis")]                              // <--- Автор теста (по желанию)
        [AllureStory("View user names in grid")]            // <--- Описание сценария
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
