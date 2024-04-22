using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent.DAL.DTO;

public class RoomToCreateDto
{
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
}