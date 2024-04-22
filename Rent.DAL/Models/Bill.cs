using temp;

namespace Rent.DAL.Models;

public partial class Bill
{
    public Guid BillId { get; set; }

    public Guid TenantId { get; set; }

    public Guid RentId { get; set; }

    public decimal BillAmount { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Rent Rent { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
