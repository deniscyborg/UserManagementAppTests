using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    [AllureSuite("Sanity Checks")]
    public class NUnitSanity
    {
        [Test]
        public void SanityTest()
        {
            Assert.Pass();
        }
    }
}