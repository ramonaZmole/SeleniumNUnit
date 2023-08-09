using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models;
using SeleniumNUnit.Helpers.Models.ApiModels;

namespace SeleniumNUnit.Tests.Book;

[TestFixture]
public class BookingTests : BaseTest
{
    private CreateRoomOutput _createRoomOutput;

    [SetUp]
    public override void Before()
    {
        base.Before();

        _createRoomOutput = Client.CreateRoom();
    }

    [Test]
    public void WhenBookingRoom_SuccessMessageShouldBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.Homepage.BookThisRoom(_createRoomOutput.description);
        Pages.Homepage.InsertBookingDetails(new User());
        Pages.Homepage.BookRoom();
        Pages.Homepage.IsSuccessMessageDisplayed().Should().BeTrue();
    }

    [Test]
    public void WhenCancellingBooking_FormShouldNotBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.Homepage.BookThisRoom(_createRoomOutput.description);
        Pages.Homepage.InsertBookingDetails(new User());
        Pages.Homepage.CancelBooking();
        Pages.Homepage.IsBookingFormDisplayed().Should().BeFalse();
        Pages.Homepage.IsCalendarDisplayed().Should().BeFalse();

        ///Pages.Homepage.IsCalendarDisplayed
        Assert.That(Pages.Homepage.GetErrorMessages()[0], Is.EqualTo(""));
    }

    [TearDown]
    public override void After()
    {
        base.After();

        Client.DeleteRoom(_createRoomOutput.roomid);
    }
}