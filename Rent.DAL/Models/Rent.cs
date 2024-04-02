namespace Rent.DAL.Models;

public partial class Rent
{
    public Guid RentId { get; set; }

    public Guid AssetId { get; set; }

    public Guid TenantId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = [];

    public virtual Asset Asset { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
