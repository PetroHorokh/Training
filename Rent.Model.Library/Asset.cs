using System.Text.Json.Serialization;

namespace Rent.Model.Library;

public class Asset
{
    public Guid AssetId { get; set; }

    public Guid OwnerId { get; set; }

    public Guid RoomId { get; set; }

    [JsonIgnore]
    public Owner Owner { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Rent> Rents { get; set; } = new List<Rent>();

    [JsonIgnore]
    public Room Room { get; set; } = null!;
}
