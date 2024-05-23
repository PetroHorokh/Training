namespace Rent.DTOs.Library;

public class AccommodationRoomToCreateDto
{
    public int AccommodationId { get; set; }

    public Guid RoomId { get; set; }

    public int Quantity { get; set; }
}