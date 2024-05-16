using System.ComponentModel.DataAnnotations;

namespace Rent.Auth.DAL.Models;

public class Image
{
    public Guid ImageId { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public byte[]? Data { get; set; }

    [MaxLength(20)]
    public string ContentType { get; set; } = null!;

    [MaxLength(100)]
    public string? Url { get; set; }

    public bool IsActive { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}