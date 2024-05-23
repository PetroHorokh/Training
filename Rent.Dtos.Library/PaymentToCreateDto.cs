namespace Rent.DTOs.Library;

public class PaymentToCreateDto
{
    public Guid TenantId { get; set; }

    public Guid BillId { get; set; }

    public decimal Amount { get; set; }
}