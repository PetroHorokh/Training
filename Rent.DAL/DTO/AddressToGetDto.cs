using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rent.DAL.Models;

namespace Rent.DAL.DTO;

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