using RestSharp;
using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models;
using SeleniumNUnit.Helpers.Models.ApiModels;
using Room = SeleniumNUnit.Helpers.Models.Room;

namespace SeleniumNUnit.Tests.Admin
{
    [TestFixture]
    public class CreateBookingTests : BaseTest
    {
        private CreateRoomOutput _createRoomOutput;
        private User user = new();
        private Room room;

        [SetUp]
        public override void Before()
        {
            base.Before();
            _createRoomOutput = Client.CreateRoom();
            room = new Room
            {
                RoomName = _createRoomOutput.roomName.ToString()
            };
        }

        [Test]
        public void WhenBookingARoom_BookingShouldBeDisplayedTest()
        {
            Browser.GoTo(Constants.AdminUrl);

            Pages.LoginPage.Login();
            Pages.AdminHeaderPage.GoToMenu(Menu.Report);

            Pages.ReportPage.SelectDates();
            Pages.ReportPage.Book();
            Pages.ReportPage.IsErrorMessageDisplayed().Should().BeTrue();

            Pages.ReportPage.InsertBookingDetails(user, room);
            Pages.ReportPage.Book();

            var bookingName = $"{user.FirstName} {user.LastName}";
            Pages.ReportPage.IsBookingDisplayed(bookingName, _createRoomOutput.roomName).Should().BeTrue();
        }


        [TearDown]
        public override void After()
        {
            base.After();
            var t = Client.CreateRequest($"{ApiResource.Room}{_createRoomOutput.roomid}", Method.DELETE);
        }
    }
}
