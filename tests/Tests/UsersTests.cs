using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Allure.NUnit.Attributes;           // <--- Добавлено
using Allure.Net.Commons;                // <--- Добавлено
using System.Threading.Tasks;

namespace Tests
{
    [AllureSuite("UI: Users Page")]      // <--- Название группы тестов для отчёта
    public class UsersTests : PageTest
    {
        [Test]
        [AllureTag("ui", "users", "crud")]              // <--- Теги для фильтрации отчёта
        [AllureSeverity(SeverityLevel.critical)]        // <--- Важность теста
        [AllureOwner("denis")]                          // <--- Автор теста (опционально)
        [AllureStory("Add, edit, delete user")]         // <--- Краткое описание сценария
        public async Task AddEditDeleteUser()
        {
            await Page.GotoAsync("http://localhost:5173");
            var usersPage = new UsersPage(Page);

            await usersPage.AddUserButton.ClickAsync();
            await Page.FillAsync("input[name='name']", "John");
            await Page.FillAsync("input[name='surname']", "Doe");
            await Page.FillAsync("input[name='email']", "john@doe.ru");
            await Page.ClickAsync("button:has-text('Добавить')");

            Assert.That(await usersPage.UserRows.CountAsync(), Is.GreaterThan(0));

            await usersPage.FirstEditButton.ClickAsync();
            await Page.FillAsync("input[name='name']", "Jonny");
            await Page.ClickAsync("button:has-text('Сохранить')");
            await Page.ClickAsync("button:has-text('Отмена')");

            await usersPage.UserRows.First.Locator("button:has-text('Удалить')").ClickAsync();
            Assert.That(await usersPage.UserRows.CountAsync(), Is.EqualTo(0));
        }
    }
}
