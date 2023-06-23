using RestSharp;
using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models;
using SeleniumNUnit.Helpers.Models.ApiModels;


namespace SeleniumNUnit.Tests.Book;

[TestFixture]
public class BookingTests : BaseTest
{
    private CreateRoomOutput _createRoomResponse;

    [SetUp]
    public override void Before()
    {
        base.Before();

        _createRoomResponse = Client.CreateRoom();
    }

    [Test]
    public void WhenBookingRoom_SuccessMessageShouldBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.HomePage.BookThisRoom(_createRoomResponse.description);
        Pages.HomePage.InsertBookingDetails(new User());
        Pages.HomePage.BookRoom();
        Pages.HomePage.IsSuccessMessageDisplayed().Should().BeTrue();
    }

    [Test]
    public void WhenCancellingBooking_FormShouldNotBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.HomePage.BookThisRoom(_createRoomResponse.description);
        Pages.HomePage.InsertBookingDetails(new User());
        Pages.HomePage.CancelBooking();
        Pages.HomePage.IsBookingFormDisplayed().Should().BeFalse();
        Pages.HomePage.IsCalendarDisplayed().Should().BeFalse();
    }

    [TearDown]
    public override void After()
    {
        base.After();
        Client.CreateRequest($"{ApiResource.Room}{_createRoomResponse.roomid}", Method.DELETE);
    }
}