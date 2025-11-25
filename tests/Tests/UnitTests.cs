using NUnit.Framework;
using Allure.NUnit.Attributes;

[TestFixture]
[AllureSuite("Unit Tests")]
public class UnitTests
{
    [Test]
    [AllureTag("unit", "validation")]
    public void ValidateEmailFormat() { }
}
