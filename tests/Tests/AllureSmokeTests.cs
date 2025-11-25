using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using NUnit.Framework;

namespace Tests
{   
    [TestFixture]
    [AllureSuite("Allure Smoke")]
    public class AllureSmokeTests
    {
        [Test]
        [AllureTag("smoke", "allure", "minimal")]              // <--- Развёрнутые теги
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("denis")]                                 // <--- Автор теста (можно заменить на свой ник)
        [AllureStory("Minimal success allure test")]            // <--- Описание сценария
        public void MinimalAllureTest()
        {
            Assert.Pass("Success!");
        }
    }
}
