using FluentAssertions;
using Newtonsoft.Json;
using NsTestFrameworkApi.RestSharp;
using NsTestFrameworkUI.Helpers;
using NUnit.Framework;
using RestSharp;

namespace UIFrameworkAndNUnit
{
    [TestFixture]
    public class Tests : BaseTest
    {
        private readonly RestClient _client = RequestHelper.GetRestClient("https://automationintesting.online/");
        private int _roomId;

        [SetUp]
        public override void Before()
        {
            _client.AddDefaultHeader("cookie", "__cfduid=da238cb8c831b6c8851619a317d638e571616569469; _ga=GA1.2.289307235.1616569470; _gid=GA1.2.1338565523.1616569470; banner=true; token=1KltCxJzvXvDcnqJ; _gat=1");
            var response = _client.CreateRequest("/room/", new CreateRoomInput(), Method.POST);
            _roomId = JsonConvert.DeserializeObject<CreateRoomOutput>(response.Content).roomId;
            base.Before();
        }

        [Test]
        public void Test1()
        {

            Browser.GoTo("https://nunit.org/");
            Browser.WebDriver.Url.Should().Be("https://nunit.org/");
            //  Assert.That(Browser.WebDriver.Url, Is.EqualTo("https://nunit.org/234"));
        }

        [Test]
        public void Test2()
        {

            Browser.GoTo("https://nunit.org/");
            Browser.WebDriver.Url.Should().Be("https://nunit.org/");
            //  Assert.That(Browser.WebDriver.Url, Is.EqualTo("https://nunit.org/234"));
        }


        [TearDown]
        public override void After()
        {
            base.After();
            _client.CreateRequest($"/room//{_roomId}", Method.DELETE);
        }
    }
}