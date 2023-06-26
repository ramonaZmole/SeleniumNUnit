using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models.ApiModels;
using SeleniumNUnit.Helpers.Models.Enum;

namespace SeleniumNUnit.Tests.Admin;

[TestFixture]
public class ReportTests : BaseTest
{
    private CreateRoomOutput _createRoomOutput;
    private CreateBookingInput _bookingInput;

    [SetUp]
    public override void Before()
    {
        base.Before();

        _createRoomOutput = Client.CreateRoom();

        _bookingInput = new CreateBookingInput
        {
            roomid = _createRoomOutput.roomid
        };
        Client.CreateBooking(_bookingInput);
    }

    [Test]
    public void WhenViewingReports_BookedRoomsShouldBeDisplayedTest()
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login();
        Pages.AdminHeaderPage.GoToMenu(Menu.Report);

        var bookingName = $"{_bookingInput.firstname} {_bookingInput.lastname}";
        Pages.ReportPage.IsBookingDisplayed(bookingName, _createRoomOutput.roomName).Should().BeTrue();
    }


    [TearDown]
    public override void After()
    {
        base.After();

        Client.DeleteRoom(_createRoomOutput.roomid);
    }
}