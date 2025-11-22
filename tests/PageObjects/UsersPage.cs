using NUnit.Framework;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;

namespace Tests
{
    [AllureSuite("UI: Users Page")]
    public class UsersTests : PageTest      // Или другой base-класс, если используешь PageTest
    {
        [Test]
        [AllureTag("ui", "users", "page-object")]
        [AllureSeverity(SeverityLevel.normal)]
        public async Task EditUserTest()
        {
            var usersPage = new UsersPage(Page); // Page есть только если наследуешь PageTest!
            await usersPage.FirstEditButton.ClickAsync();
            // ...добавь шаги редактирования...
        }
    }
}
