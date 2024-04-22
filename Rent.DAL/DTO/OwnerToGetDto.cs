using Rent.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Rent.DAL.DTO;

public class OwnerToGetDto
{
    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid OwnerId { get; set; }

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid AddressId { get; set; }

    public override string ToString() => $"\nOwner with id {OwnerId} information\nName: {Name}\nAddressId: {AddressId}";
}