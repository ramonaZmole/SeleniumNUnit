using FluentAssertions;
using NsTestFrameworkUI.Helpers;
using NUnit.Framework;

namespace UIFrameworkAndNUnit
{
    public class Tests : BaseTest
    {
        [Test]
        public void Test1()
        {
            Browser.GoTo("https://nunit.org/");
             Browser.WebDriver.Url.Should().Be("https://nunit.org/234");
          //  Assert.That(Browser.WebDriver.Url, Is.EqualTo("https://nunit.org/234"));
        }
    }
}