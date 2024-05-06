namespace Rent.DAL.Models;

public class Impost
{
    public Guid ImpostId { get; set; }

    public decimal Tax { get; set; }

    public decimal Fine { get; set; }

    public int PaymentDay { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

}
