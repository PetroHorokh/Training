namespace Rent.DTOs.Library;

public class RoomTypeToGetDto
{
    public int RoomTypeId { get; set; }

    public string Name { get; set; } = null!;
    public override string ToString() =>
        $"\nRoom type {RoomTypeId} information\nName: {Name}";
}