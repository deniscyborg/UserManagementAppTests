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
            await Page.FillAsync("input[name='username']", "TestUser");
            await Page.FillAsync("input[name='email']", "test@example.com");
            await Page.ClickAsync("button[type='submit']");
            
            // Assert
            await Expect(Page.Locator("text=TestUser")).ToBeVisibleAsync();
        }
    }
}
