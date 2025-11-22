using NUnit.Framework;
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;           // <--- Добавлено
using Allure.Net.Commons;                // <--- Добавлено

namespace Tests
{
    [AllureSuite("UI: User Form")]       // <--- Название группы тестов для отчёта
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
        [AllureTag("ui", "form", "add-user")]                 // <--- Теги для Allure
        [AllureSeverity(SeverityLevel.blocker)]               // <--- Наивысший приоритет для UI формы
        [AllureOwner("denis")]                                // <--- Автор теста (опционально
