namespace Rent.DTOs.Library;

public class AccommodationRoomToGetDto
{
    public Guid AccommodationRoomId { get; set; }
    public int AccommodationId { get; set; }
    public Guid RoomId { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }

    public override string ToString() =>
        $"\nRoom accommodation with id {AccommodationRoomId} information\nAccommodation {AccommodationId} has {Name} and is in quantity of {Quantity}";
}