using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models;

namespace SeleniumNUnit.Tests.Admin;

[TestFixture]
public class LoginTests : BaseTest
{
    [TestCase("admin", "password", true)]
    [TestCase("invalidUser", "invalidPassword", false)]
    [Test]
    public void LoginAsAdmin(string username, string password, bool isLoggedIn)
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login(username, password);

        Pages.AdminHeaderPage.IsLogoutButtonDisplayed().Should().Be(isLoggedIn);
    }

    [TestCaseSource(nameof(GetLoginScenarios))]
    [Test]
    public void LoginAsAdmin(LoginData loginData)
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login(loginData.Username, loginData.Password);

        Pages.AdminHeaderPage.IsLogoutButtonDisplayed().Should().Be(loginData.IsLoggedIn);
    }

    private static IEnumerable<object[]> GetLoginScenarios()
    {
        var dataFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Helpers\Data\LoginData.csv";

        using var stream = new StreamReader(dataFilePath);
        using var reader = new CsvReader(stream, new CsvConfiguration(CultureInfo.CurrentCulture));
        var rows = reader.GetRecords<LoginData>();

        foreach (var row in rows)
        {
            yield return new object[] { row };
        }
    }
}