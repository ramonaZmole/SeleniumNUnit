using RestSharp;
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

        Pages.HomePage.BookThisRoom(_createRoomOutput.description);
        Pages.HomePage.BookRoom();
        Pages.HomePage.GetErrorMessages().Should().BeEquivalentTo(Constants.FormErrorMessages);

        Pages.HomePage.InsertBookingDetails(new User());
        Pages.HomePage.BookRoom();
        Pages.HomePage.GetErrorMessages()[0].Should().Be(Constants.AlreadyBookedErrorMessage);
    }

    [TearDown]
    public override void After()
    {
        base.After();
        Client.CreateRequest($"{ApiResource.Room}{_createRoomOutput.roomid}", Method.DELETE);
    }
}