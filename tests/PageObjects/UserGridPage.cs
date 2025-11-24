using NUnit.Framework;
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;

namespace Tests
{
    [TestFixture]
    [AllureSuite("UI: User Grid")] // Группа тестов для Allure-отчёта
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
        [AllureTag("ui", "grid", "users")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("denis")]
        [AllureStory("Add, edit and delete user in grid")]
        public async Task AddEditDeleteUser()
        {
            var page = await _browserContext.NewPageAsync();
            var usersGrid = new UserGridPage(page);

            // 1. Перейти на страницу грида
            await usersGrid.GoToAsync(_config.BaseUrl);

            // 2. Добавить пользователя
            await usersGrid.AddUserAsync("Алексей");
            var allUserNames = await usersGrid.GetAllUserNamesAsync();
            Assert.That(allUserNames, Does.Contain("Алексей"), "User should be added");

            // 3. Редактировать пользователя (реализуй подробности внутри PageObject)
            await usersGrid.ClickEditUserAsync("Алексей");
            // Здесь добавь действия по редактированию, если надо

            // 4. Удалить пользователя
            await usersGrid.DeleteUserAsync("Алексей");
            var allUserNamesAfterDelete = await usersGrid.GetAllUserNamesAsync();
            Assert.That(allUserNamesAfterDelete, Does.Not.Contain("Алексей"), "User should be deleted");
        }
    }
}
