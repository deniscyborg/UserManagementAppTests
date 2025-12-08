using Allure.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using PageObjects;
using System;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;

namespace Tests
{   
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("UI: User Form")]
    public class UserFormTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private TestConfig _config;
        private string _setupError = null;

        [SetUp]
        public async Task SetUp()
        {
            try
            {
                AllureApi.Step("Initialize test configuration");
                _config = TestConfig.Load();
                
                AllureApi.Step("Launch Playwright browser");
                _playwright = await Playwright.CreateAsync();

                var browserType = _config.Browser.ToLower() switch
                {
                    "chromium" => _playwright.Chromium,
                    "firefox" => _playwright.Firefox,
                    "webkit" => _playwright.Webkit,
                    _ => _playwright.Chromium
                };

                _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
                AllureApi.Step("Browser launched successfully");
            }
            catch (Exception ex)
            {
                _setupError = ex.Message;
                AllureApi.Step($"Setup failed: {ex.Message}");
                AllureApi.AddAttachment("Setup Error", "text/plain", System.Text.Encoding.UTF8.GetBytes(ex.ToString()));
            }
        }

        [TearDown]
        public async Task TearDown()
        {
            if (_browser != null)
                await _browser.CloseAsync();
            
            if (_playwright != null)
                _playwright.Dispose();
        }

        [Test]
        [AllureTag("ui", "form", "add-user")]
        [AllureSeverity(SeverityLevel.blocker)]
        [AllureOwner("denis")]
        [AllureStory("Add user via UI form")]
        public async Task CanAddUserViaUI()
        {
            if (_setupError != null)
            {
                Assert.Fail($"Test skipped due to setup failure: {_setupError}");
                return;
            }

            AllureApi.Step("Open new browser page");
            var page = await _browser.NewPageAsync();
            var userFormPage = new UserFormPage(page);

            try
            {
                AllureApi.Step($"Navigate to {_config.BaseUrl}");
                await userFormPage.GoToAsync(_config.BaseUrl);

                            AllureApi.Step("Click 'Добавить пользователя' button");
            await userFormPage.ClickAddUserButtonAsync();
                
                AllureApi.Step("Fill user name: Алексей");
                await userFormPage.FillNameAsync("Алексей");

                            AllureApi.Step("Fill user surname: Иванов");
            await userFormPage.FillSurnameAsync("Иванов");
                
                AllureApi.Step("Fill user email: alexey@test.ru");
                await userFormPage.FillEmailAsync("alexey@test.ru");
                
                AllureApi.Step("Submit form");
                await userFormPage.SubmitAsync();

                AllureApi.Step("Verify success message");
                var successMsg = await page.InnerTextAsync("#successMsg");
                Assert.AreEqual("Пользователь добавлен", successMsg);
            }
            catch (Exception ex)
            {
                AllureApi.AddAttachment("Test Error", "text/plain", System.Text.Encoding.UTF8.GetBytes(ex.ToString()));
                await page.ScreenshotAsync(new() { Path = "error-screenshot.png" });
                AllureApi.AddAttachment("Error Screenshot", "image/png", System.IO.File.ReadAllBytes("error-screenshot.png"));
                throw;
            }
        }
    }
}
