using NUnit.Framework;
using Allure.NUnit.Attributes;      // <--- Добавлено
using Allure.Net.Commons;           // <--- Добавлено

namespace Tests
{
    [AllureSuite("Sanity Checks")]                    // <--- Группа тестов для отчёта
    public class NUnitSanity
    {
        [Test]
        [AllureTag("sanity", "nunit")]                // <--- Теги для отчёта
        [AllureSeverity(SeverityLevel.trivial)]       // <--- Важность: базовая проверка
        [AllureOwner("denis")]                        // <--- Автор теста (по желанию)
        [AllureStory("NUnit sanity test")]            // <--- Краткое описание
        public void SanityTest()
        {
            Assert.Pass();
        }
    }
}
