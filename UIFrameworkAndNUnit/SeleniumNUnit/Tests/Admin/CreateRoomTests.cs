﻿using Newtonsoft.Json;
using SeleniumNUnit.Helpers;
using SeleniumNUnit.Helpers.Models.ApiModels;
using Room = SeleniumNUnit.Helpers.Models.Room;

namespace SeleniumNUnit.Tests.Admin;

[TestFixture]
public class CreateRoomTests : BaseTest
{
    private readonly Room _roomModel = new();

    [Test]
    public void WhenCreatingARoom_ThenItShouldBeCreatedTest()
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login();

        Pages.RoomPage.CreateRoom();
        Pages.RoomPage.IsErrorMessageDisplayed().Should().BeTrue();
        var errorMessages = Pages.RoomPage.GetErrorMessages();
        errorMessages.Should().Contain("must be greater than or equal to 1");
        errorMessages.Should().Contain("Room name must be set");

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
        var response = Client.CreateRequest(ApiResource.Room);
        var roomsList = JsonConvert.DeserializeObject<GetRoomsOutput>(response.Content);
        if (roomsList == null) return;
        var id = roomsList.rooms.First(x => x.roomName == int.Parse(_roomModel.RoomName)).roomid;
        Client.CreateRequest($"{ApiResource.Room}{id}", RestSharp.Method.DELETE);
    }
}