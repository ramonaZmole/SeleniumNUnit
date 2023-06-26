using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models;
using SeleniumNUnit.Helpers.Models.ApiModels;

namespace SeleniumNUnit.Tests.Book;

[TestFixture]
public class BookingFormTests : BaseTest
{
    private CreateRoomOutput _createRoomOutput;

    [SetUp]
    public override void Before()
    {
        base.Before();

        _createRoomOutput = Client.CreateRoom();

        var bookingInput = new CreateBookingInput
        {
            roomid = _createRoomOutput.roomid
        };
        Client.CreateBooking(bookingInput);
    }

    [Test]
    public void WhenBookingRoom_ErrorMessageShouldBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.Homepage.BookThisRoom(_createRoomOutput.description);
        Pages.Homepage.BookRoom();
        Pages.Homepage.GetErrorMessages().Should().BeEquivalentTo(Messages.FormErrorMessages);

        Pages.Homepage.InsertBookingDetails(new User());
        Pages.Homepage.BookRoom();
        Pages.Homepage.GetErrorMessages()[0].Should().Be(Messages.AlreadyBookedErrorMessage);
    }

    [TearDown]
    public override void After()
    {
        base.After();

        Client.DeleteRoom(_createRoomOutput.roomid);
    }
}