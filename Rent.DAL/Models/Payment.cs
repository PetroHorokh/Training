namespace Rent.DAL.Models;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public Guid TenantId { get; set; }

    public Guid BillId { get; set; }

    public DateTime PaymentDay { get; set; }

    public decimal Amount { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
