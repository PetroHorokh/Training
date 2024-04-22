using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent.DAL.DTO;

public class OwnerToCreateDto
{

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid AddressId { get; set; }
}