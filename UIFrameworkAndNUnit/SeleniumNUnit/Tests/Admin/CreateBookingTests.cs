using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models;
using SeleniumNUnit.Helpers.Models.ApiModels;
using SeleniumNUnit.Helpers.Models.Enum;
using Room = SeleniumNUnit.Helpers.Models.Room;

namespace SeleniumNUnit.Tests.Admin;

[TestFixture]
public class CreateBookingTests : BaseTest
{
    private CreateRoomOutput _createRoomOutput;
    private readonly User _user = new();
    private Room _room = new();

    [SetUp]
    public override void Before()
    {
        base.Before();

        _createRoomOutput = Client.CreateRoom();

        _room = new Room
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

        Pages.ReportPage.InsertBookingDetails(_user, _room);
        Pages.ReportPage.Book();

        var bookingName = $"{_user.FirstName} {_user.LastName}";
        Pages.ReportPage.IsBookingDisplayed(bookingName, _createRoomOutput.roomName).Should().BeTrue();
    }


    [TearDown]
    public override void After()
    {
        base.After();

        Client.DeleteRoom(_createRoomOutput.roomid);
    }
}