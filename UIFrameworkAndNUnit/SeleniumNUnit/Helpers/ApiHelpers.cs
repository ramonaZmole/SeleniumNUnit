using Newtonsoft.Json;
using RestSharp;
using SeleniumNUnit.Helpers.Models.ApiModels;

namespace SeleniumNUnit.Helpers;

public static class ApiHelpers
{
    public static string GetLoginToken(this RestClient client)
    {
        var output = client.CreateRequest(ApiResource.Login, new LoginInput(), Method.POST);
        if (string.IsNullOrEmpty(output?.Content)) return string.Empty;

        var content = Newtonsoft.Json.Linq.JObject.Parse(output.Content);

        return (string?)content["token"];
    }

    public static CreateRoomOutput CreateRoom(this RestClient client)
    {
        var input = new CreateRoomInput();

        var createRoomResponse = client.CreateRequest(ApiResource.Room, input, Method.POST);

        var rooms = client.GetRooms();
        var created = rooms?.rooms?
            .FirstOrDefault(r => !string.IsNullOrEmpty(r.description)
            && r.description.Contains(input.description));

        var createRoomOutput = new CreateRoomOutput
        {
            success = createRoomResponse?.Content ?? string.Empty
        };

        if (created is not null)
        {
            createRoomOutput.roomid = created.roomid;
            createRoomOutput.roomName = created.roomName;
            createRoomOutput.description = created.description;
        }

        return createRoomOutput;
    }

    public static void CreateBooking(this RestClient client, CreateBookingInput createBookingInput)
    {
        createBookingInput.bookingdates.checkin = createBookingInput.bookingdates.checkin.Remove(8, 2).Insert(8, Constants.BookingStartDay);
        createBookingInput.bookingdates.checkout = createBookingInput.bookingdates.checkout.Remove(8, 2).Insert(8, Constants.BookingEndDay);
        client.CreateRequest(ApiResource.Booking, createBookingInput, Method.POST);
    }

    public static void DeleteRoom(this RestClient client, string roomName)
    {
        var roomsList = client.GetRooms();
        if (roomsList?.rooms == null) return;

        var room = roomsList.rooms.FirstOrDefault(x => x.roomName == roomName);
        if (room == null) return;

        client.CreateRequest($"{ApiResource.Room}{room.roomid}", Method.DELETE);
    }

    public static void DeleteRoom(this RestClient client, int roomId)
    {
        var t = client.CreateRequest($"{ApiResource.Room}{roomId}", Method.DELETE);
    }

    private static GetRoomsOutput GetRooms(this RestClient client)
    {
        var response = client.CreateRequest(ApiResource.Room);
        return JsonConvert.DeserializeObject<GetRoomsOutput>(response.Content);
    }
}