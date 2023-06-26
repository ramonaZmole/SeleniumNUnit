using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using NsTestFrameworkUI.Helpers;
using UIFrameworkAndNUnit;

namespace MagentoMsTest
{
    [TestClass]
    public class Tests2 : BaseTest
    {

        [DataRow("shirt")]
        [DataRow("pants")]
        [DataRow("vase")]
        [TestMethod]
        public void ProductsAreReturnedAtSearch(string searchedProduct)
        {
            Browser.GoTo("http://qa1magento.dev.evozon.com/");

            Pages.Homepage.SearchProduct(searchedProduct);
            Pages.Homepage.AreProductsDisplayed().Should().BeTrue();
        }


        [DataRow("admin", "password", true)]
        [DataRow("invalidUser", "invalidPassword", false)]
        [TestMethod]
        public void UserIsAbleToLogin(string username, string password, bool isLoggedIn)
        {
            Browser.GoTo("http://qa1magento.dev.evozon.com/");

            Pages.Homepage.Login(username, password);
            Pages.Homepage.IsUserLoggedIn().Should().Be(isLoggedIn);
        }



        public static IEnumerable<object> LoginData()
        {
            yield return new object[] { Faker.Internet.Email(), Faker.Lorem.GetFirstWord(), false };
            yield return new object[] { Faker.Internet.Email(), UserData.Password, true };
        }

        [DynamicData(nameof(LoginData), DynamicDataSourceType.Method)]
        [TestMethod]
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

        [DynamicData(nameof(GetLoginData), DynamicDataSourceType.Method)]
        [TestMethod]
        public void UserIsCanLogin(string username, string password, bool isLoggedIn)
        {
            Browser.GoTo("http://qa1magento.dev.evozon.com/");

            Pages.Homepage.Login(username, password);
            Pages.Homepage.IsUserLoggedIn().Should().Be(isLoggedIn);
        }
    }
}