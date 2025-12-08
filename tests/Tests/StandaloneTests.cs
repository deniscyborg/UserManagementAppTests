using Allure.NUnit;
using NUnit.Framework;
using Allure.NUnit.Attributes;

[TestFixture]
[AllureNUnit]
[AllureSuite("Standalone Tests")]
public class StandaloneTests
{
    [Test]
    public void CanValidateInput_WithoutServer() { }
}