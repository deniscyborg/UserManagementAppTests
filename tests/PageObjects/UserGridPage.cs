using NUnit.Framework;
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;

namespace Tests
{
    [AllureSuite("UI: User Grid CRUD")]                 // Группа в Allure-отчёте
    public class UserGridCrudTests
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
        [AllureTag("ui", "users", "grid", "crud")]       // Теги для отчёта
        [AllureSeverity(SeverityLevel.critical)]         // Важность сценария
        [AllureOwner("denis")]                           // Автор теста
        [AllureStory("Add, Edit, Delete user in grid")]  // Описание сценария
        public async Task AddEditDeleteUser()
        {
            var page = await _browser.NewPageAsync();
            var usersGrid = new UserGridPage(page);

            // Переход на страницу пользователей
            await usersGrid.GoToAsync(_config.BaseUrl);

            // Добавление пользователя (допустим через другой PageObject или прямым взаимодействием)
            // ...тут может быть код по добавлению через форму

            // Проверка наличия пользователя
            Assert.IsTrue(await usersGrid.IsUserPresentAsync("Алексей"), "User not present in grid after add.");

            // Редактирование пользователя
            await usersGrid.ClickEditUserAsync("Алексей");
            // ...тут могут быть шаги редактирования

            // Удаление пользователя
            await usersGrid.DeleteUserAsync("Алексей");
            Assert.IsFalse(await usersGrid.IsUserPresentAsync("Алексей"), "User still present in grid after delete.");
        }
    }
}
