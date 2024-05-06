using System.Text.Json.Serialization;

namespace Rent.DAL.Models;

public class Bill
{
    public Guid BillId { get; set; }

    public Guid TenantId { get; set; }

    public Guid RentId { get; set; }

    public decimal BillAmount { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? EndDate { get; set; }

    [JsonIgnore]
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [JsonIgnore]
    public Rent Rent { get; set; } = null!;

    [JsonIgnore]
    public Tenant Tenant { get; set; } = null!;
}
