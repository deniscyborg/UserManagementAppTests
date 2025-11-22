using NUnit.Framework;
using Allure.NUnit.Attributes;       // <--- Добавлено
using Allure.Net.Commons;            // <--- Добавлено

namespace Tests
{
    [AllureSuite("Smoke")]           // <--- Имя группы тестов для отчёта
    public class SmokeTest
    {
        [Test]
        [AllureTag("smoke", "basic")]              // <--- Теги для Allure отчёта
        [AllureSeverity(SeverityLevel.trivial)]    // <--- Обозначим тест как вспомогательный
        [AllureOwner("denis")]                     // <--- Автор теста (по желанию)
        [AllureStory("Project runs smoke test")]   // <--- Описание сценария
        public void TestProjectRuns()
        {
            Assert.Pass("Smoke test runs!");
        }
    }
}
