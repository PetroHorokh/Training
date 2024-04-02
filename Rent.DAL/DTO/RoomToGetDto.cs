namespace Rent.DAL.DTO;

public class RoomToGetDto
{
    public Guid RoomId { get; set; }

    public int Number { get; set; }

    public decimal Area { get; set; }

    public int RoomTypeId { get; set; }

    public override string ToString() =>
        $"\nRoom with id {RoomId}\nNumber: {Number}\nArea: {Area}\nRoom type id: {RoomTypeId}";
}