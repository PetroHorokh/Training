namespace Rent.DAL.DTO;

public class BillToGetDto
{
    public Guid BillId { get; set; }

    public Guid TenantId { get; set; }

    public Guid RentId { get; set; }

    public decimal BillAmount { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? EndDate { get; set; }

    public override string ToString() => $"\nBill with id {BillId} information\nTenant id: {TenantId}\nRent id: {RentId}\nBill amount: {BillAmount}\nIssue date: {IssueDate}\nEnd date: {(EndDate == null ? "not paid" : EndDate.ToString())}";
}