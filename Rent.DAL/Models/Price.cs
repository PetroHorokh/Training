using System.Text.Json.Serialization;

namespace Rent.DAL.Models;

public class Price
{
    public Guid PriceId { get; set; }

    public DateTime StartDate { get; set; }

    public decimal Value { get; set; }

    public DateTime? EndDate { get; set; }

    public int RoomTypeId { get; set; }

    [JsonIgnore]
    public RoomType RoomType { get; set; } = null!;
}
