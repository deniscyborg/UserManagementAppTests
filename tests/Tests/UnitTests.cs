using Allure.NUnit;
using NUnit.Framework;
using Allure.NUnit.Attributes;

[TestFixture]
[AllureNUnit]
[AllureSuite("Unit Tests")]
public class UnitTests
{
    [Test]
    [AllureTag("unit", "validation")]
    public void ValidateEmailFormat() { }
}
