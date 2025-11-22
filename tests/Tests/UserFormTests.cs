using NUnit.Framework;
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;

namespace Tests
{
    [AllureSuite("UI: User Form")]
    public class UserFormTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private TestConfig _config;

        [SetUp]
        public async Task SetUp()
        {
            _config = TestConfig.Load();
            _playwright = await Playwright.CreateAsync();

            var browserType = _config.Browser.ToLower() switch
            {
                "chromium" => _playwright.Chromium,
                "firefox" => _playwright.Firefox,
                "webkit" => _playwright.Webkit,
                _ => _playwright.Chromium
            };

            _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [Test]
        [AllureTag("ui", "form", "add-user")]
        [AllureSeverity(SeverityLevel.blocker)]
        [AllureOwner("denis")]
        [AllureStory("Add user via UI form")]
        public async Task CanAddUserViaUI()
        {
            var page = await _browser.NewPageAsync();
            var userFormPage = new UserFormPage(page);

            await userFormPage.GoToAsync(_config.BaseUrl);
            await userFormPage.FillNameAsync("Алексей");
            await userFormPage.FillEmailAsync("alexey@test.ru");
            await userFormPage.SubmitAsync();

            var successMsg = await page.InnerTextAsync("#successMsg");
            Assert.AreEqual("Пользователь добавлен", successMsg);
        }
    }
}
