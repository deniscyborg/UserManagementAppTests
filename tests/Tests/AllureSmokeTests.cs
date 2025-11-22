using NUnit.Framework;
using Allure.NUnit.Attributes;

namespace Tests
{
    [AllureNUnit]
    [AllureSuite("Allure Smoke")]
    public class AllureSmokeTests
    {
        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(Allure.NUnit.Attributes.SeverityLevel.normal)]
        public void CanGenerateAllureArtifacts()
        {
            Assert.Pass("Минимальный тест для проверки Allure");
        }
    }
}
