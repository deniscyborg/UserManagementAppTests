using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests
{
    public class UsersTests : PageTest
    {
        [Test]
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
