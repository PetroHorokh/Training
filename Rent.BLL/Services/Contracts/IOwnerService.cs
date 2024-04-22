using Rent.DAL.DTO;
using Rent.DAL.Responses;

namespace Rent.BLL.Services.Contracts;

public interface IOwnerService
{
    Task<IEnumerable<OwnerToGetDto>> GetAllOwnersAsync();

    Task<IEnumerable<AssetToGetDto>> GetAllAssetsAsync();

    Task<OwnerToGetDto?> GetOwnerByIdAsync(Guid ownerId);

    Task<AssetToGetDto?> GetAssetByIdAsync(Guid assetId);

    Task<AddressToGetDto?> GetOwnerAddressAsync(Guid ownerId);

    Task<IEnumerable<AssetToGetDto>> GetOwnerAssetsAsync(Guid ownerId);

    Task<CreationResponse> CreateOwnerAsync(OwnerToCreateDto owner);

    Task<UpdatingResponse> UpdateOwnerAsync(OwnerToGetDto newOwner);

    Task<UpdatingResponse> DeleteOwnerAsync(Guid ownerId);

    Task<CreationResponse> CreateAssetAsync(AssetToCreateDto asset);

    Task<UpdatingResponse> DeleteAssetAsync(Guid assetId);
}