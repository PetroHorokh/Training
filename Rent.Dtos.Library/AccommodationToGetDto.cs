namespace Rent.DTOs.Library;

public class AccommodationToGetDto
{
    public int AccommodationId { get; set; }

    public string Name { get; set; } = null!;

    public override string ToString() =>
        $"\nAccommodation with id {AccommodationId} information\nName: {Name}";
}