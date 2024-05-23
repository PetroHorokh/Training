namespace Rent.DTOs.Library;

public class PriceToCreateDto
{
    public DateTime StartDate { get; set; }

    public decimal Value { get; set; }

    public DateTime? EndDate { get; set; }

    public int RoomTypeId { get; set; }

    public Guid CreatedBy { get; set; }
}