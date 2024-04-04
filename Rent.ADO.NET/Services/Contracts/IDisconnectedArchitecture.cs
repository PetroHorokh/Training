using Rent.ADO.NET.DTOs;

namespace Rent.ADO.NET.Services.Contracts;

public interface IDisconnectedArchitecture
{
    Task<IEnumerable<AvailableAssetDto>> GetAvailableAssetsAsync(DateTime dateTime);

    Task<IEnumerable<AssetBookingDto>> GetAssetBookingAsync(Guid assetId);
}