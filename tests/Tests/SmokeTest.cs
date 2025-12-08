using System.Threading.Tasks;
using NUnit.Framework;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;

namespace Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Smoke")]           
    public class SmokeTest
    {
        [Test]
        [AllureTag("smoke", "basic")]        
        [AllureSeverity(SeverityLevel.trivial)]    
        [AllureOwner("denis")]                     
        [AllureStory("Project runs smoke test")]   
        public void TestProjectRuns()
        {
            Assert.Pass("Smoke test runs!");
        }
    }
}

