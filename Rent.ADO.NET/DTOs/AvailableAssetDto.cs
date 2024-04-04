namespace Rent.ADO.NET.DTOs;

public class AvailableAssetDto
{
    public Guid AssetId { get; set; }

    public override string ToString() => $"\nAsset with id {AssetId} is available";
}