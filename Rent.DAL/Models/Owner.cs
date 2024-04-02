namespace Rent.DAL.Models;

public partial class Owner
{
    public Guid OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public Guid AddressId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
