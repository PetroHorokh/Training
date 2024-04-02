namespace Rent.DAL.DTO;

public class AssetToGetDto
{
    public Guid AssetId { get; set; }

    public Guid OwnerId { get; set; }

    public Guid RoomId { get; set; }

    public override string ToString() => $"\nAsset with id {AssetId} information\nOwnerId: {OwnerId}\nRoomId: {RoomId}";
}