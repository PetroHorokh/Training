using System.Text.Json.Serialization;

namespace Rent.DAL.Models;

public class Accommodation
{
    public int AccommodationId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public ICollection<AccommodationRoom> AccommodationRooms { get; set; } = new List<AccommodationRoom>();
}
