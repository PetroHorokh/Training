using System.Text.Json.Serialization;
namespace Rent.Model.Library;

public class Room
{
    public Guid RoomId { get; set; }

    public int Number { get; set; }

    public decimal Area { get; set; }

    public Guid AddressId { get; set; }

    public int RoomTypeId { get; set; }

    [JsonIgnore]
    public ICollection<AccommodationRoom> AccommodationRooms { get; set; } = new List<AccommodationRoom>();

    [JsonIgnore]
    public Address Address { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Asset> Assets { get; set; } = new List<Asset>();

    [JsonIgnore]
    public RoomType RoomType { get; set; } = null!;
}
