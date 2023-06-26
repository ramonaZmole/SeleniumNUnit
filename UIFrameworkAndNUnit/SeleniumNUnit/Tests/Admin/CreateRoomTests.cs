using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models.Enum;
using Room = SeleniumNUnit.Helpers.Models.Room;

namespace SeleniumNUnit.Tests.Admin;

[TestFixture]
public class CreateRoomTests : BaseTest
{
    private Room _roomModel = new();

    [TestCase(RoomType.Double)]
    [TestCase(RoomType.Family)]
    [TestCase(RoomType.Single)]
    [TestCase(RoomType.Suite)]
    [TestCase(RoomType.Twin)]
    [Test]
    public void WhenCreatingARoom_ThenItShouldBeCreatedTest(RoomType roomType)
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login();

        Pages.RoomPage.CreateRoom();
        Pages.RoomPage.IsErrorMessageDisplayed().Should().BeTrue();
        var errorMessages = Pages.RoomPage.GetErrorMessages();
        errorMessages.Should().Contain("must be greater than or equal to 1");
        errorMessages.Should().Contain("Room name must be set");

        _roomModel = new Room
        {
            Type = roomType.ToString()
        };
        Pages.RoomPage.InsertRoomDetails(_roomModel);
        Pages.RoomPage.CreateRoom();
        Pages.RoomPage.GetLastRoomDetails().Should().BeEquivalentTo(_roomModel);
    }

    [Test]
    public void WhenCreatingRoomWithNoRoomDetails_NoFeaturesShouldBeDisplayedTest()
    {
        _roomModel.RoomDetails = string.Empty;

        Browser.GoTo(Constants.AdminUrl);
        Pages.LoginPage.Login();

        Pages.RoomPage.InsertRoomDetails(_roomModel);
        Pages.RoomPage.CreateRoom();
        Pages.RoomPage.GetLastRoomDetails().RoomDetails.Should().Be("No features added to the room");
    }


    [TearDown]
    public override void After()
    {
        base.After();

        Client.DeleteRoom(_roomModel.RoomName);
    }
}