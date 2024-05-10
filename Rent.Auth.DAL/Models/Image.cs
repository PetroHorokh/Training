namespace Rent.Auth.DAL.Models;

public class Image
{
    public Guid ImageId { get; set; }

    public string Name { get; set; } = null!;

    public byte[] Data { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public bool IsActive { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}