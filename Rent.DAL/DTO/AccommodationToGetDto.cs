using System.IO;

namespace Rent.DAL.DTO;

public class AccommodationToGetDto
{
    public int AccommodationId { get; set; }

    public string Name { get; set; } = null!;

    public override string ToString() =>
        $"\nAccommodation with id {AccommodationId} information\nName: {Name}";
}