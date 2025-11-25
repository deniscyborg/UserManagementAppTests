using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace Tests
{
    [TestFixture]
    [AllureNUnit]
    public class BasicTests : PageTest
    {
        private const string BaseUrl = "http://localhost:5173";

        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        public async Task VerifyPageLoads()
        {
            await Page.GotoAsync(BaseUrl);
            await Expect(Page).ToHaveURLAsync(BaseUrl);
        }
    }
}
