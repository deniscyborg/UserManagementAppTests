using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests
{
    public class PlaywrightSmoke
    {
        [Test]
        public async Task PlaywrightLoads()
        {
            using var playwright = await Playwright.CreateAsync();
            Assert.That(playwright, Is.Not.Null);
        }
    }
}
