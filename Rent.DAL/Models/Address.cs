using System.Text.Json.Serialization;

namespace Rent.DAL.Models;

public class Address
{
    public Guid AddressId { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Building { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Owner> Owners { get; set; } = new List<Owner>();

    [JsonIgnore]
    public ICollection<Room> Rooms { get; set; } = new List<Room>();

    [JsonIgnore]
    public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}
