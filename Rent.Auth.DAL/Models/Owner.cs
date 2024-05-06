using System.Text.Json.Serialization;

namespace Rent.Auth.DAL.Models;

public class Owner
{
    public Guid OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public Guid UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; } = null!;
}
