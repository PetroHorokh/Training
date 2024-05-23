namespace Rent.AWS.Lambda.Models;

public class Bill
{
    public Guid BillId { get; set; }

    public Guid TenantId { get; set; }

    public Guid RentId { get; set; }

    public decimal BillAmount { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? EndDate { get; set; }
}