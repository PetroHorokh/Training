using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Rent.Auth.DAL.Models;

public class User : IdentityUser<Guid>
{
    [MaxLength(100)]
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiration { get; set; }

    public IEnumerable<Image> Images { get; set; } = new List<Image>();

    [JsonIgnore]
    public ICollection<Owner> Owners { get; set; } = new List<Owner>();

    [JsonIgnore]
    public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}
