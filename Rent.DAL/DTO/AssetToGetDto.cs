using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.DAL.DTO;

public class AssetToGetDto
{
    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid AssetId { get; set; }

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid OwnerId { get; set; }

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid RoomId { get; set; }

    public override string ToString() => $"\nAsset with id {AssetId} information\nOwnerId: {OwnerId}\nRoomId: {RoomId}";
}