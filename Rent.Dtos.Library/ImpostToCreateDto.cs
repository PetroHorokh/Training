namespace Rent.DTOs.Library;

public class ImpostToCreateDto
{
    public decimal Tax { get; set; }

    public decimal Fine { get; set; }

    public int PaymentDay { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}