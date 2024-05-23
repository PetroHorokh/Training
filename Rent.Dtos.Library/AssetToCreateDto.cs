using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent.DTOs.Library;

public class AssetToCreateDto
{
    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid OwnerId { get; set; }

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid RoomId { get; set; }
}