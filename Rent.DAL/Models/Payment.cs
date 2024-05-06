using System.Text.Json.Serialization;
using temp;

namespace Rent.DAL.Models;

public class Payment
{
    public Guid PaymentId { get; set; }

    public Guid TenantId { get; set; }

    public Guid BillId { get; set; }

    public DateTime PaymentDay { get; set; }

    public decimal Amount { get; set; }

    [JsonIgnore]
    public Bill Bill { get; set; } = null!;

    [JsonIgnore]
    public Tenant Tenant { get; set; } = null!;
}
