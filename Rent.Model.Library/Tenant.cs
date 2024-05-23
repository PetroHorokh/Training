using System.Text.Json.Serialization;

namespace Rent.Model.Library;

public class Tenant
{
    public Guid TenantId { get; set; }

    public Guid UserId { get; set; }

    public Guid AddressId { get; set; }

    public string Name { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string Director { get; set; } = null!;

    public string Description { get; set; } = null!;

    [JsonIgnore]
    public Address Address { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [JsonIgnore]
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [JsonIgnore]
    public ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
