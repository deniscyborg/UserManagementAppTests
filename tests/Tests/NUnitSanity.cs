using NUnit.Framework;
using Allure.NUnit.Attributes;  // <-- Для [AllureSuite], [AllureTag] и т.д.
using Allure.Net.Commons;       // <-- Для SeverityLevel и других типов

namespace Tests
{
    [TestFixture]
    [AllureSuite("Sanity Checks")]
    public class NUnitSanity
    {
        [Test]
        [AllureTag("sanity", "nunit")]
        [AllureSeverity(SeverityLevel.trivial)]
        [AllureOwner("denis")]
        [AllureStory("NUnit sanity test")]
        public void SanityTest()
        {
            Assert.Pass();
        }
    }
}