using NUnit.Framework;
using Allure.NUnit.Attributes;

[TestFixture]
[AllureSuite("Logic Tests")]
public class ValidationLogicTests
{
    [Test]
    public void UserEmail_ShouldContainAtSymbol() { }
}
