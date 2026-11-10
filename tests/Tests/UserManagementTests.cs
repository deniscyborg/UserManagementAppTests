using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace UserManagementApp.Tests
{
    [TestFixture]
    [AllureNUnit]
    public class UserManagementTests : PageTest
    {
        [Test]
        public async Task Should_Load_HomePage()
        {
            // Arrange & Act
            await Page.GotoAsync("http://localhost:5173");
            
            // Assert
            await Expect(Page).ToHaveTitleAsync("User Management");
        }
        
        [Test]
        public async Task Should_Create_New_User()
        {
            // Arrange
            await Page.GotoAsync("http://localhost:5173");
            
            // Act
            await Page.ClickAsync("button:has-text('Добавить пользователя')");
            await Page.FillAsync("input[name='name']", "TestUser");
            await Page.FillAsync("input[name='surname']", "Testov");
            await Page.FillAsync("input[name='email']", "testuser@example.ru");
            await Page.ClickAsync("button[type='submit']");
            
            // Assert
            await Expect(Page.Locator("td", new() { HasText = "TestUser" }).First).ToBeVisibleAsync();
        }
    }
}
