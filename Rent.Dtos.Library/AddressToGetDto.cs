using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.DTOs.Library;

public class AddressToGetDto
{
    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid AddressId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string City { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string Street { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string Building { get; set; } = null!;
}