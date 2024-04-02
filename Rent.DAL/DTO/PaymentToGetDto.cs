namespace Rent.DAL.DTO;

public class PaymentToGetDto
{
    public Guid PaymentId { get; set; }

    public Guid TenantId { get; set; }

    public Guid BillId { get; set; }

    public DateTime PaymentDay { get; set; }

    public decimal Amount { get; set; }

    public override string ToString() => $"\nPayment with id {PaymentId} information\nTenant id: {TenantId}\nBill id: {BillId}\nPayment date: {PaymentDay}\nAmount: {Amount}";
}