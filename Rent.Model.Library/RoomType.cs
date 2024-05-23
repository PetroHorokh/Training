using System.Text.Json.Serialization;

namespace Rent.Model.Library;

public class RoomType
{
    public int RoomTypeId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Price> Prices { get; set; } = new List<Price>();

    [JsonIgnore]
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
