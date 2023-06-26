using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;
using NsTestFrameworkApi.RestSharp;
using NsTestFrameworkUI.Helpers;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using SeleniumNUnit.Helpers.Models;

namespace UIFrameworkAndNUnit
{
    [TestFixture]
    public class Tests2 : BaseTest
    {

        [TestCase("shirt")]
        [TestCase("pants")]
        [TestCase("vase")]
        [Test]
        public void ProductsAreReturnedAtSearch(string searchedProduct)
        {
            Browser.GoTo("http://qa1magento.dev.evozon.com/");

            Pages.Homepage.SearchProduct(searchedProduct);
            Pages.Homepage.AreProductsDisplayed().Should().BeTrue();
        }

        [TestCase("admin", "password", true)]
        [TestCase("invalidUser", "invalidPassword", false)]
        [Test]
        public void UserIsAbleToLogin(string username, string password, bool isLoggedIn)
        {
            Browser.GoTo("http://qa1magento.dev.evozon.com/");

            Pages.Homepage.Login(username, password);
            Pages.Homepage.IsUserLoggedIn().Should().Be(isLoggedIn);
        }



        public static IEnumerable<TestCaseData> LoginData()
        {
            yield return new TestCaseData(Faker.Internet.Email(), Faker.Lorem.GetFirstWord(), false);
            yield return new TestCaseData(Faker.Internet.Email(), UserData.Password, true);
        }

        [TestCaseSource(nameof(LoginData))]
        [Test]
        public void UserCanLogin(string username, string password, bool isLoggedIn)
        {
            Browser.GoTo("http://qa1magento.dev.evozon.com/");

            Pages.Homepage.Login(username, password);
            Pages.Homepage.IsUserLoggedIn().Should().Be(isLoggedIn);
        }



        private static IEnumerable<object[]> GetLoginData()
        {
            var dataFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                               + @"\Helpers\Data\LoginData.csv";

            using var stream = new StreamReader(dataFilePath);
            using var reader = new CsvReader(stream, new CsvConfiguration(CultureInfo.CurrentCulture));
            var rows = reader.GetRecords<LoginData>();

            foreach (var row in rows)
            {
                yield return new object[] { row };
            }
        }

        [TestCaseSource(nameof(GetLoginData))]
        [Test]
        public void UserIsCanLogin(string username, string password, bool isLoggedIn)
        {
            Browser.GoTo("http://qa1magento.dev.evozon.com/");

            Pages.Homepage.Login(username, password);
            Pages.Homepage.IsUserLoggedIn().Should().Be(isLoggedIn);
        }
    }
}