using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.DAL.DTO;

public class RoomToGetDto
{
    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid RoomId { get; set; }

    [Required]
    [Column(TypeName = "int")]
    public int Number { get; set; }

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid AddressId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Area { get; set; }

    [Required]
    [Column(TypeName = "int")]
    public int RoomTypeId { get; set; }

    public override string ToString() =>
        $"\nRoom with id {RoomId}\nNumber: {Number}\nArea: {Area}";
}