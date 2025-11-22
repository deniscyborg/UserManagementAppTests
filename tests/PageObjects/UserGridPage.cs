using NUnit.Framework;
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
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
        public async Task AddEditDeleteUser()
        {
            var page = await _browser.NewPageAsync();
            var usersGrid = new UserGridPage(page);

            // Переход на страницу грида
            await usersGrid.GoToAsync(_config.BaseUrl);

            // --- Шаг 1: Добавление пользователя через UI ---
            // Здесь предполагаем, что пользователь "Алексей" теперь есть

            // --- Шаг 2: Проверка, что пользователь появился ---
            var allUserNames = await usersGrid.GetAllUserNamesAsync();
            Assert.That(allUserNames, Does.Contain("Алексей"));

            // --- Шаг 3: Редактирование пользователя ---
            await usersGrid.ClickEditUserAsync("Алексей");
            // Тут можно добавить действия для заполнения и сохранения новой информации, если есть в PageObject

            // --- Шаг 4: Удаление пользователя ---
            await usersGrid.DeleteUserAsync("Алексей");

            // --- Шаг 5: Проверка, что пользователя больше нет ---
            var allUserNamesAfterDelete = await usersGrid.GetAllUserNamesAsync();
            Assert.That(allUserNamesAfterDelete, Does.Not.Contain("Алексей"));
        }
    }
}
