namespace Rent.ADO.NET.DTOs;

public class AssetBookingDto
{
    public Guid RentId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public override string ToString() => $"\nAsset booked with id {RentId} on dates {StartDate} - {EndDate}";
}