using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;

namespace Tests
{
    [TestFixture]
    [AllureSuite("Playwright Smoke")]                   // <--- Группа тестов для отчёта
    public class PlaywrightSmoke
    {
        [Test]
        [AllureTag("smoke", "playwright", "startup")]   // <--- Теги для отчёта
        [AllureSeverity(SeverityLevel.normal)]          // <--- Важность: нормальная
        [AllureOwner("denis")]                          // <--- Автор теста (опционально)
        [AllureStory("Playwright launches and loads")]  // <--- Описание сценария
        public async Task PlaywrightLoads()
        {
            using var playwright = await Playwright.CreateAsync();
            Assert.That(playwright, Is.Not.Null);
        }
    }
}
