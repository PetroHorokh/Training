using System.Text.Json.Serialization;

namespace Rent.DAL.Models;

public class Rent
{
    public Guid RentId { get; set; }

    public Guid AssetId { get; set; }

    public Guid TenantId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [JsonIgnore]
    public Asset Asset { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [JsonIgnore]
    public Tenant Tenant { get; set; } = null!;
}
