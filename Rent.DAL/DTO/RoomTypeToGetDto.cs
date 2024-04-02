using Rent.DAL.Models;
using System.IO;

namespace Rent.DAL.DTO;

public class RoomTypeToGetDto
{
    public int RoomTypeId { get; set; }

    public string Name { get; set; } = null!;
    public override string ToString() =>
        $"\nRoom type {RoomTypeId} information\nName: {Name}";
}