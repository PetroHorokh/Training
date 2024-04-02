namespace Rent.DAL.Models;

public partial class Impost
{
    public Guid ImpostId { get; set; }

    public decimal Tax { get; set; }

    public decimal Fine { get; set; }

    public int PaymentDay { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }
}
