using Allure.Net.Commons;
using NUnit.Framework;
using Allure.NUnit.Attributes;

namespace Tests
{
    [AllureSuite("Allure Smoke")]
    public class AllureSmokeTests
    {
        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(SeverityLevel.normal)]
        public void MinimalAllureTest()
        {
            Assert.Pass("Success!");
        }
    }
}
